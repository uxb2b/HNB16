using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelCore.DataModel;
using ModelCore.Models.ViewModel;
using Newtonsoft.Json;
using ModelCore.Helper;
using CommonLib.Utility;
using WebHome.Helper;

namespace WebHome.Controllers.Base
{
    public class LcAppBaseController : SampleController
    {
        public LcAppBaseController(IServiceProvider serviceProvider, ILoggerFactory loggerFactory) : base(serviceProvider, loggerFactory)
        {
        }

        [Authorize]
        public ActionResult LoadCreditApplication(CreditApplicationQueryViewModel viewModel)
        {
            viewModel.AntiXSS(ViewData);

            var tmp = viewModel.AppID;
            if (viewModel.KeyID != null)
            {
                tmp = viewModel.DecryptKeyValue();
            }

            var item = models!.GetTable<CreditApplicationDocumentary>()
                .Where(c => c.AppID == tmp)
                .FirstOrDefault();

            if (viewModel.IgnoreEmpty != true)
            {
                if (item == null)
                {
                    viewModel.AlertTitle = "作業失敗";
                    viewModel.AlertMessage = "「資料錯誤」。";
                    return View("~/Views/Portal/ApplicationMessage.cshtml");
                }
            }

            return View("~/Views/WDELC/WDELC0101/EditCreditApplication.cshtml", item);
        }

        [Authorize]
        public ActionResult LoadLcAmendment(LcAmendmentQueryViewModel viewModel)
        {
            viewModel.AntiXSS(ViewData);

            var tmp = viewModel.AmendingID;
            if (viewModel.KeyID != null)
            {
                tmp = viewModel.DecryptKeyValue();
            }

            var item = models!.GetTable<AmendingLcApplication>()
                .Where(c => c.AmendingID == tmp)
                .FirstOrDefault();

            if (viewModel.IgnoreEmpty != true)
            {
                if (item == null)
                {
                    viewModel.AlertTitle = "作業失敗";
                    viewModel.AlertMessage = "「資料錯誤」。";
                    return View("~/Views/Portal/ApplicationMessage.cshtml");
                }
            }

            return View("~/Views/WDELC/WDELC0201/EditLcAmendment.cshtml", item);
        }

        [Authorize]
        public ActionResult LoadDocumentary(QueryViewModel viewModel)
        {
            viewModel.AntiXSS(ViewData);

            int? docID = viewModel.DocID != null && viewModel.DocID.Length > 0
                            ? viewModel.DocID[0]
                            : null;

            if (viewModel.KeyID != null)
            {
                docID = viewModel.DecryptKeyValue();
            }

            Documentary? item = models!.GetTable<Documentary>().Where(d => d.DocID == docID).FirstOrDefault();

            if (viewModel.IgnoreEmpty != true)
            {
                if (item == null)
                {
                    viewModel.AlertTitle = "作業失敗";
                    viewModel.AlertMessage = "「資料錯誤」。";
                    return View("~/Views/Portal/ApplicationMessage.cshtml");
                }
            }

            return View("~/Views/Portal/ApplicationMessage.cshtml", item);
        }

        [Authorize]
        public ActionResult LoadLcCancellation(LcCancellationQueryViewModel viewModel)
        {
            viewModel.AntiXSS(ViewData);

            var tmp = viewModel.CancellationID;
            if (viewModel.KeyID != null)
            {
                tmp = viewModel.DecryptKeyValue();
            }

            var item = models!.GetTable<CreditCancellation>()
                .Where(c => c.CancellationID == tmp)
                .FirstOrDefault();

            if (viewModel.IgnoreEmpty != true)
            {
                if (item == null)
                {
                    viewModel.AlertTitle = "作業失敗";
                    viewModel.AlertMessage = "「資料錯誤」。";
                    return View("~/Views/Portal/ApplicationMessage.cshtml");
                }
            }

            return View("", item);
        }

        protected internal void ValidateProcess(ApplicationProcessViewModel viewModel, Documentary item)
        {
            if (!viewModel.Approval.HasValue)
            {
                ModelState.AddModelError("Approval", "拒絕與核准都未被選取!!");
            }

            if (viewModel.Approval == false)
            {
                if (String.IsNullOrEmpty(viewModel.RejectReason))
                {
                    ModelState.AddModelError("Reason", "請選擇拒絕原因!!");
                }
                else if (viewModel.RejectReason == "其他:" && viewModel.OtherRejectReason == null)
                {
                    ModelState.AddModelError("Others", "未填寫其他拒絕原因!!");
                }
            }
            else if (viewModel.Approval == true)
            {
                if (item.CustomerCreditAlert.Any())
                {
                    viewModel.Memo = viewModel.Memo.GetEfficientString();
                    if (viewModel.Memo == null)
                    {
                        ModelState.AddModelError("Memo", "未填寫客戶有信用貶弱之狀態續送原因!!");
                    }
                }
            }
        }

        [Authorize]
        public ActionResult LoadNegoDraft(NegoDraftQueryViewModel viewModel)
        {
            viewModel.AntiXSS(ViewData);

            var tmp = viewModel.DraftID;
            if (viewModel.KeyID != null)
            {
                tmp = viewModel.DecryptKeyValue();
            }

            var item = models!.GetTable<NegoDraft>()
                .Where(c => c.DraftID == tmp)
                .FirstOrDefault();

            if (viewModel.IgnoreEmpty != true)
            {
                if (item == null)
                {
                    viewModel.AlertTitle = "作業失敗";
                    viewModel.AlertMessage = "「資料錯誤」。";
                    return View("~/Views/Portal/ApplicationMessage.cshtml");
                }
            }

            return View("", item);
        }

        [Authorize]
        public ActionResult LoadNegoInvoice(NegoInvoiceQueryViewModel viewModel, out NegoDraft? draftItem)
        {
            viewModel.AntiXSS(ViewData);
            draftItem = null;

            var tmp = viewModel;
            if (viewModel.KeyID != null)
            {
                tmp = JsonConvert.DeserializeObject<NegoInvoiceQueryViewModel>(viewModel.KeyID.DecryptData());
            }

            var item = models!.GetTable<NegoInvoice>()
                .Where(c => c.InvoiceID == tmp!.InvoiceID)
                .Where(c => c.DraftID == tmp!.DraftID)
                .FirstOrDefault();

            if (item != null)
            {
                draftItem = item.NegoDraft;
            }
            else
            {
                draftItem = models.GetTable<NegoDraft>()
                    .Where(c => c.DraftID == tmp!.DraftID)
                    .FirstOrDefault();
            }

            if (viewModel.IgnoreEmpty != true)
            {
                if (item == null)
                {
                    viewModel.AlertTitle = "作業失敗";
                    viewModel.AlertMessage = "「資料錯誤」。";
                    return View("~/Views/Portal/ApplicationMessage.cshtml");
                }
            }

            return View("", item);
        }

        [Authorize]
        public ActionResult LoadReimbursement(ReimbursementQueryViewModel viewModel)
        {
            viewModel.AntiXSS(ViewData);

            var tmp = viewModel.ReimID;
            if (viewModel.KeyID != null)
            {
                tmp = viewModel.DecryptKeyValue();
            }

            var item = models!.GetTable<Reimbursement>()
                .Where(c => c.ReimID == tmp)
                .FirstOrDefault();

            if (viewModel.IgnoreEmpty != true)
            {
                if (item == null)
                {
                    viewModel.AlertTitle = "作業失敗";
                    viewModel.AlertMessage = "「資料錯誤」。";
                    return View("~/Views/Portal/ApplicationMessage.cshtml");
                }
            }

            return View("", item);
        }

        [Authorize]
        public ActionResult LoadNegoLoanRepayment(NegoLoanRepaymentQueryViewModel viewModel)
        {
            viewModel.AntiXSS(ViewData);

            var tmp = viewModel.RepaymentID;
            if (viewModel.KeyID != null)
            {
                tmp = viewModel.DecryptKeyValue();
            }

            var item = models!.GetTable<NegoLoanRepayment>()
                .Where(c => c.RepaymentID == tmp)
                .FirstOrDefault();

            if (viewModel.IgnoreEmpty != true)
            {
                if (item == null)
                {
                    viewModel.AlertTitle = "作業失敗";
                    viewModel.AlertMessage = "「資料錯誤」。";
                    return View("~/Views/Portal/ApplicationMessage.cshtml");
                }
            }

            return View("", item);
        }

        [Authorize]
        public ActionResult LoadDraftAcceptance(DraftAcceptanceQueryViewModel viewModel)
        {
            viewModel.AntiXSS(ViewData);

            var tmp = viewModel.AcceptanceID;
            if (viewModel.KeyID != null)
            {
                tmp = viewModel.DecryptKeyValue();
            }

            var item = models!.GetTable<NegoDraftAcceptance>()
                .Where(c => c.AcceptanceID == tmp)
                .FirstOrDefault();

            if (viewModel.IgnoreEmpty != true)
            {
                if (item == null)
                {
                    viewModel.AlertTitle = "作業失敗";
                    viewModel.AlertMessage = "「資料錯誤」。";
                    return View("~/Views/Portal/ApplicationMessage.cshtml");
                }
            }

            return View("", item);
        }

    }
}
