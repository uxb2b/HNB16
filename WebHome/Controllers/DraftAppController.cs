using Microsoft.AspNetCore.Mvc;

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

    }
}
