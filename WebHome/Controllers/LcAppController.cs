using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebHome.Models;

namespace WebHome.Controllers
{
    [Authorize]
    public class LcAppController : SampleController
    {
        public LcAppController(IServiceProvider serviceProvider, ILoggerFactory loggerFactory) : base(serviceProvider, loggerFactory)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ForCDS()
        {
            return View();
        }

        [HttpPost]
        [Route("LcApp/NextStep")]
        public IActionResult NextStep([FromBody] NextStepRequest req)
        {
            string? next;
            if (req.inputType == "reWrite")
            {
                if (req.beneType == "cds")
                    next = Url.Action("ForCDS");
                else if (req.beneType == "fpc")
                    next = Url.Action("ForFPG");
                else
                    next = Url.Action("ForOthers");
            }
            else
                next = Url.Action("QueryForm");
            return Json(new { next });
        }

        public IActionResult ConfirmReview()
        {
            return View("~/Views/LcApp/ConfirmReview.cshtml");
        }

        public IActionResult SearchHandler()
        {
            return View("~/Views/LcApp/QueryList.cshtml");
        }

        [HttpGet]
        public IActionResult LcAppDetailModalContent()
        {
            // 可依需求帶入資料
            return PartialView("~/Views/LcApp/Module/LcAppDetailModalContent.cshtml");
        }

        [HttpGet]
        public IActionResult LcDetailModalContent()
        {
            // 可依需求帶入資料
            return PartialView("~/Views/LcApp/Module/LcDetailModalContent.cshtml");
        }

        [HttpGet]
        public IActionResult NoticeModalContent()
        {
            // 可依需求帶入資料
            return PartialView("~/Views/LcApp/Module/NoticeModalContent.cshtml");
        }

    }
}
