using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebHome.Controllers.Query
{
    [Authorize]
    public class QueryAmendAppController : SampleController
    {
        public QueryAmendAppController(IServiceProvider serviceProvider, ILoggerFactory loggerFactory) : base(serviceProvider, loggerFactory)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
