using System;
using System.Security.Cryptography;
using System.Text;

namespace WebHome.Services
{
    // Support verifying stored base64 hash strings.
    // The stored string format supported:
    // - If the string length is 44/88 (base64) we assume raw SHA256 (32 bytes) without salt stored as base64.
    // - If the string contains a colon-separated PBKDF2 format: iterations:saltBase64:subkeyBase64
    public static class PasswordHasher
    {
        public static bool Verify(string password, string stored)
        {
            if (string.IsNullOrEmpty(stored) || string.IsNullOrEmpty(password))
                return false;

            // PBKDF2 format iterations:salt:subkey
            var parts = stored.Split(':');
            if (parts.Length == 3 && int.TryParse(parts[0], out int iterations))
            {
                try
                {
                    var salt = Convert.FromBase64String(parts[1]);
                    var subkey = Convert.FromBase64String(parts[2]);
                    using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
                    var computed = pbkdf2.GetBytes(subkey.Length);
                    return CryptographicOperations.FixedTimeEquals(computed, subkey);
                }
                catch
                {
                    return false;
                }
            }

            // Fallback: stored is raw SHA256 (32 bytes) base64
            try
            {
                var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
                var base64 = Convert.ToBase64String(bytes);
                // some systems may trim padding, compare ignoring padding
                return NormalizeBase64(base64) == NormalizeBase64(stored);
            }
            catch
            {
                return false;
            }
        }

        private static string? NormalizeBase64(string? s)
        {
            return s?.Trim().TrimEnd('=');
        }
    }
}
