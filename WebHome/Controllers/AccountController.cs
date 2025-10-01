using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebHome.Helper;
using WebHome.Models;

namespace WebHome.Controllers;
[Authorize]
public class AccountController : SampleController
{
    public AccountController(IServiceProvider serviceProvider, ILoggerFactory loggerFactory) : base(serviceProvider, loggerFactory)
    {
    }

    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync()
    {
        var profile = await HttpContext.GetUserAsync();
        if(profile != null)
        {
            HttpContext.Logout();
        }
        return View("~/Views/Login/Index.cshtml");
    }

    public IActionResult Logout()
    {
        HttpContext.Logout();
        return View("~/Views/Login/Index.cshtml");
    }

}
