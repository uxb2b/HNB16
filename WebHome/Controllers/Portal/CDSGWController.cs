using Microsoft.AspNetCore.Mvc;
using ModelCore.Helper;
using ModelCore.NegoManagement;
using Newtonsoft.Json;

namespace WebHome.Controllers.Portal
{
    public class CDSGWController : SampleController
    {
        public CDSGWController(IServiceProvider serviceProvider, ILoggerFactory loggerFactory) : base(serviceProvider, loggerFactory)
        {
            DumpRequest = true;
        }

        [HttpPost]
        [Route("CDSGW/PromptNegoData")]
        public ActionResult PromptNegoData()
        {
            try
            {
                var negoData = JsonConvert.DeserializeObject<ModelCore.Schema.UXCDS.NegoData>(RequestBody.DecryptData());
                using (UXCDSNegoDraftManager dalc = new UXCDSNegoDraftManager())
                {
                    dalc.SavePromptInfo(negoData);
                }
                return Json(new { result = true, message = "Data_Successful" });
            }
            catch (Exception ex)
            {
                ModelCore.Helper.Logger.Error(ex);
                return Json(new { result = false, message = ex.Message });
            }
        }
    }
}
