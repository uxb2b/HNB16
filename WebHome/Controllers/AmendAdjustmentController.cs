using Microsoft.AspNetCore.Mvc;

namespace WebHome.Controllers
{
    public class AmendAdjustmentController : SampleController
    {
        public AmendAdjustmentController(IServiceProvider serviceProvider, ILoggerFactory loggerFactory) : base(serviceProvider, loggerFactory)
        {
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreditListBh()
        {
            // return partial that contains the credit data for BH
            return PartialView("_CreditListBh");
        }

        public IActionResult CreditListBs()
        {
            // return partial that contains the credit data for BS
            return PartialView("_CreditListBs");
        }

        public class ConfirmRequest
        {
            public string lcNo { get; set; }
            public string authType { get; set; }
            public bool adjustStatus { get; set; }
        }

        [HttpPost]
        public IActionResult ConfirmReview([FromBody] ConfirmRequest request)
        {
            ViewData["lcNo"] = request?.lcNo ?? string.Empty;
            ViewData["authType"] = request?.authType ?? string.Empty;
            ViewData["adjustStatus"] = request?.adjustStatus ?? false;

            return View("~/Views/AmendAdjustment/Module/ConfirmReview.cshtml");
        }

        [HttpPost]
        public IActionResult ConfirmHandler([FromBody] ConfirmRequest request)
        {
            ViewData["lcNo"] = request?.lcNo ?? string.Empty;
            ViewData["authType"] = request?.authType ?? string.Empty;
            ViewData["adjustStatus"] = request?.adjustStatus ?? false;

            return View("~/Views/AmendAdjustment/Module/ConfirmComplete.cshtml");
        }
    }
}