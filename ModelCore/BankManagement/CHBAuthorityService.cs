
using ModelCore.Schema;
using ModelCore.UserManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ModelCore.BankManagement
{
    public static class CHBAuthorityService
    {
        public static Func<UserProfile, eAuth> ReloadTodo { get; set; }
        public static Func<UserProfile, String, eAuth> QueryTaskFlowSchema { get; set; }
        public static Func<UserProfile, String, eAuth> QueryAssumeNextUsers { get; set; }
        public static Func<String, String, XmlDocument> DownloadUserInfo { get; set; }
        public static Func<UserProfile, String, int?, String, String, Dictionary<String, String>> StartDocumentFlow { get; set; }
        public static Func<UserProfile, String, Dictionary<String, String>> VerifyDocumentFlow { get; set; }
        public static Func<UserProfile, String, decimal, Dictionary<String, String>> PassDocumentFlow { get; set; }
        public static Func<UserProfile, String, String, Dictionary<String, String>> AbandonDocumentFlow { get; set; }
        public static Func<UserProfile, String, Dictionary<String, String>> DeleteDocumentFlow { get; set; }
        public static Func<UserProfile, eAuth> CheckPortalSession { get; set; }

    }
}
