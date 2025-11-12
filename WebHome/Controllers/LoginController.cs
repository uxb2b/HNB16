using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using CommonLib.Core.DataWork;
using ModelCore.DataModel;
using WebHome.Models;
using WebHome.Services;
using CommonLib.Utility;
using ModelCore.UserManagement;
using WebHome.Helper;
using Microsoft.AspNetCore.Authorization;

namespace WebHome.Controllers
{
    public class LoginController : SampleController
    {
        public LoginController(IServiceProvider serviceProvider, ILoggerFactory loggerFactory) : base(serviceProvider, loggerFactory)
        {
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var profile = await HttpContext.GetUserAsync();
            if (profile != null)
            {
                HttpContext.Logout();
            }
            return View("~/Views/Login/Index.cshtml");
        }

        [HttpPost]
        [Route("Login/SignIn")]
        public async Task<IActionResult> SignInAsync([FromBody] LoginViewModel payload)
        {
            if (payload == null || string.IsNullOrWhiteSpace(payload.Account) || string.IsNullOrWhiteSpace(payload.Password))
            {
                return Json(new { success = false, message = "帳號與密碼不可為空白" });
            }

            try
            {
                var profile = UserProfile.CreateInstance(payload.Account/*, payload.Password*/);
                if (profile == null)
                {
                    return Json(new { success = false, message = "帳號或密碼錯誤" });
                }

                await HttpContext.SignOnAsync(profile, false);

                return Json(new { success = true, account = profile.PID, name = profile.UserName });
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, "SignIn failed");
                return Json(new { success = false, message = "系統發生錯誤，請稍後再試" });
            }
        }
    }
}
