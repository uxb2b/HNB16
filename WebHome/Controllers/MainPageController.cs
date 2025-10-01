using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebHome.Models;
using System.Collections.Generic;

namespace WebHome.Controllers;
[Authorize]
public class MainPageController : SampleController
{
    public MainPageController(IServiceProvider serviceProvider, ILoggerFactory loggerFactory) : base(serviceProvider, loggerFactory)
    {
    }

    public IActionResult Index()
    {
        return View("~/Views/MainPage/Index.cshtml");
    }

    // 動態取得受益人清單
    [HttpPost]
    [Route("api/beneficiaries")]
    [AllowAnonymous]
    public IActionResult GetBeneficiaries([FromBody] BeneficiaryRequest request)
    {
        var cdsList = new List<object>
        {
            new { text = "30414175 中國鋼鐵股份有限公司", value = "30414175" },
            new { text = "07838854 中鴻鋼鐵股份有限公司", value = "07838854" },
            new { text = "96971313 中鋼鋁業股份有限公司", value = "96971313" },
            new { text = "75460005 唐榮鐵工廠股份有限公司", value = "75460005" }
        };
        var fpcList = new List<object>
        {
            new { text = "台塑集團", value = "30414175" },
            new { text = "奇美集團", value = "75460005" }
        };
        var result = new List<object>();
        if (request.Type == "cds")
            result = cdsList;
        else if (request.Type == "fpc")
            result = fpcList;
        return Json(result);
    }

    public class BeneficiaryRequest
    {
        public string? Type { get; set; }
    }
}
