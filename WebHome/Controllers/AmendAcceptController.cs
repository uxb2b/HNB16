using Microsoft.AspNetCore.Mvc;

namespace WebHome.Controllers
{
    public class AmendAcceptController : SampleController
    {
        public AmendAcceptController(IServiceProvider serviceProvider, ILoggerFactory loggerFactory) : base(serviceProvider, loggerFactory)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PreviewCheckHandler()
        {
            // 可根據需求傳遞 model
            return PartialView("~/Views/AmendAccept/Module/CheckHandler.cshtml");
        }

        [HttpPost]
        public IActionResult ConfirmHandler()
        {
            // 可根據需求傳遞 model
            return PartialView("~/Views/MainPage/Module/CompleteMessage.cshtml");
        }

    }
}
