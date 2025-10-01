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

    // �ʺA���o���q�H�M��
    [HttpPost]
    [Route("api/beneficiaries")]
    [AllowAnonymous]
    public IActionResult GetBeneficiaries([FromBody] BeneficiaryRequest request)
    {
        var cdsList = new List<object>
        {
            new { text = "30414175 ������K�ѥ��������q", value = "30414175" },
            new { text = "07838854 ���E���K�ѥ��������q", value = "07838854" },
            new { text = "96971313 �����T�~�ѥ��������q", value = "96971313" },
            new { text = "75460005 ��a�K�u�t�ѥ��������q", value = "75460005" }
        };
        var fpcList = new List<object>
        {
            new { text = "�x�춰��", value = "30414175" },
            new { text = "�_������", value = "75460005" }
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
