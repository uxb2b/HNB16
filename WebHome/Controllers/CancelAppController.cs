using Microsoft.AspNetCore.Mvc;
using WebHome.Models;
using static WebHome.Controllers.DraftAppController;

namespace WebHome.Controllers
{
    public class CancelAppController : SampleController
    {
        public CancelAppController(IServiceProvider serviceProvider, ILoggerFactory loggerFactory) : base(serviceProvider, loggerFactory)
        {
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search([FromForm] CancelAppQueryModel model)
        {
            // TODO: Replace with real data access
            var result = new CancelAppQueryResultModel();
            result.PageIndex = 1;
            result.PageSize = 10;
            result.TotalCount = 3;
            result.TotalAmount = 522664;

            result.Items.Add(new CancelAppQueryItem {
                LcNo = "099700049161000860",
                Bank = "高雄分行",
                IssueDate = new System.DateTime(2025,6,5),
                Amount = 282125.00m,
                AvailableAmount = 282125.00m,
                BeneInNo = "30414175",
                Status = "未到期",
                LcType = "即期",
                BeneType = "cds",
            });
            result.Items.Add(new CancelAppQueryItem {
                LcNo = "099700049161000861",
                Bank = "高雄分行",
                IssueDate = new System.DateTime(2025,6,5),
                Amount = 282125.00m,
                AvailableAmount = 282125.00m,
                BeneInNo = "30414175",
                Status = "未到期",
                LcType = "遠期",
                BeneType = "fpg"
            });
            result.Items.Add(new CancelAppQueryItem {
                LcNo = "099700049161000862",
                Bank = "高雄分行",
                IssueDate = new System.DateTime(2025,6,5),
                Amount = 282125.00m,
                AvailableAmount = 282125.00m,
                BeneInNo = "30414175",
                Status = "未到期",
                LcType = "即期",
                BeneType = "cds"
            });

            return PartialView("_QueryListPartial", result);
        }

        [HttpPost]
        public IActionResult CancelLcApp([FromForm] CancelAppQueryModel req)
        {
            return PartialView("~/Views/CancelApp/CancelLcApp.cshtml", req);
        }

        [HttpPost]
        public IActionResult ConfirmReview([FromBody] CancelAppQueryModel request)
        {
            return View("~/Views/CancelApp/Module/ConfirmReview.cshtml", request);
        }

        [HttpPost]
        public IActionResult ConfirmHandler([FromBody] CancelAppQueryModel request)
        {
            // 回傳 modal 內容 view
            return PartialView("~/Views/CancelApp/Module/ConfirmComplete.cshtml", request);
        }
    }
}
