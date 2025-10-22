using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace WebHome.Controllers
{
    public class CurrentAmendDraftAppController : SampleController
    {
        public CurrentAmendDraftAppController(IServiceProvider serviceProvider, ILoggerFactory loggerFactory) : base(serviceProvider, loggerFactory)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LcAppList()
        {
            // return partial that contains the draft (票) application details
            return PartialView("_LcAppList");
        }

        public class ConfirmRequest
        {
            public string lcNo { get; set; }
            public string authType { get; set; }
            public bool revertStatus { get; set; }
        }

        [HttpPost]
        public IActionResult ConfirmReview([FromBody] ConfirmRequest request)
        {
            ViewData["lcNo"] = request?.lcNo ?? string.Empty;
            ViewData["authType"] = request?.authType ?? string.Empty;
            ViewData["revertStatus"] = request?.revertStatus ?? false;

            // reuse shared confirm modal from AmendLcApp
            return View("~/Views/CurrentAmendDraftApp/Module/ConfirmReview.cshtml");
        }

        [HttpPost]
        public IActionResult ConfirmHandler([FromBody] ConfirmRequest request)
        {
            // perform server-side confirm logic here as needed
            ViewData["lcNo"] = request?.lcNo ?? string.Empty;
            ViewData["revertStatus"] = request?.revertStatus ?? false;

            return View("~/Views/CurrentAmendDraftApp/Module/ConfirmComplete.cshtml");
        }
    }
}
