using Microsoft.AspNetCore.Mvc;
using CommonLib.Core.Utility;
using ModelCore.Helper;

namespace WebHome.Controllers
{
    public class DraftAppController : SampleController
    {
        public DraftAppController(IServiceProvider serviceProvider, ILoggerFactory loggerFactory) : base(serviceProvider, loggerFactory)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SearchHandler()
        {
            return View("~/Views/DraftApp/QueryList.cshtml");
        }

        [HttpPost]
        public IActionResult CheckDraftHandler([FromBody] CheckDraftRequest request)
        {
            // 根據 IsAccepted 判斷
            if (request.IsAccepted == null || request.IsAccepted == false)
            {
                // 尚未接受註記
                return View("~/Views/DraftApp/Module/CheckHandler.cshtml");
            }
            else
            {
                // 已接受註記，可進入押匯申請
                return Json(new { result = true });
            }
        }

        [HttpPost]
        public IActionResult ConfirmHandler()
        {
            // 這裡可處理表單資料，暫以顯示 modal 為主
            return View("~/Views/DraftApp/Module/ConfirmHandler.cshtml");
        }

        public class CheckDraftRequest
        {
            public string? LcNo { get; set; }
            public bool? IsAccepted { get; set; }
        }
    }
}
