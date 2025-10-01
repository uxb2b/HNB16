using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace WebHome.Controllers
{
    public class AmendAppController : SampleController
    {
        public AmendAppController(IServiceProvider serviceProvider, ILoggerFactory loggerFactory) : base(serviceProvider, loggerFactory)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Query(string beneType, string lcNo, string beneNo, string beneInNo, string status, string startDate, string endDate)
        {
            // TODO: 實際查詢邏輯，這裡先用假資料
            var list = new List<dynamic>
            {
                new { LcNo = "099700049161000860", Bank = "高雄分行", Date = "2025/06/05", Amount = "$ 282,125.00", Balance = "$ 282,125.00", BeneNo = "30414175", Status = "未到期" },
                new { LcNo = "099700049161000861", Bank = "高雄分行", Date = "2025/06/05", Amount = "$ 282,125.00", Balance = "$ 282,125.00", BeneNo = "30414175", Status = "未到期" },
                new { LcNo = "099700049161000862", Bank = "高雄分行", Date = "2025/06/05", Amount = "$ 282,125.00", Balance = "$ 282,125.00", BeneNo = "30414175", Status = "未到期" }
            };
            // 可根據查詢參數過濾 list
            return PartialView("_QueryListPartial", list);
        }

        [HttpGet]
        public IActionResult LcDetailModalContent()
        {
            // 可依需求帶入資料
            return PartialView("~/Views/AmendApp/Module/LcDetailModalContent.cshtml");
        }
    }
}
