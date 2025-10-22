using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace WebHome.Controllers
{
    public class AmendLcAppController : SampleController
    {
        public AmendLcAppController(IServiceProvider serviceProvider, ILoggerFactory loggerFactory) : base(serviceProvider, loggerFactory)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LcAppList()
        {
            // In real implementation, load model/data here and pass to partial view
            return PartialView("_LcAppList");
        }

        public IActionResult CreditModalContent()
        {
            // 可依需求帶入資料
            return PartialView("~/Views/AmendLcApp/Module/CreditModalContent.cshtml");
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
            // Pass data to the view using ViewData
            ViewData["lcNo"] = request?.lcNo ?? string.Empty;
            ViewData["authType"] = request?.authType ?? string.Empty;
            ViewData["revertStatus"] = request?.revertStatus ?? false;

            // Return a view that uses the shared _MessageModalLayout to render the modal
            return View("~/Views/AmendLcApp/Module/ConfirmReview.cshtml");
        }

        [HttpPost]
        public IActionResult ConfirmHandler([FromBody] ConfirmRequest request)
        {
            // After performing server-side confirm logic, return the completion modal content
            ViewData["lcNo"] = request?.lcNo ?? string.Empty;
            ViewData["revertStatus"] = request?.revertStatus ?? false;

            return View("~/Views/AmendLcApp/Module/ConfirmComplete.cshtml");
        }
    }
}
