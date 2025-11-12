using CommonLib.Core.Utility;
using CommonLib.Core.DataWork;
using CommonLib.Utility;
using Microsoft.AspNetCore.Mvc;
using ModelCore.BankManagement;
using ModelCore.DataModel;
using ModelCore.EventMessageApp;
using ModelCore.Helper;
using ModelCore.Locale;
using ModelCore.Models.ViewModel;
using ModelCore.UserManagement;
using System.Text;
using System.Text.RegularExpressions;
using WebHome.Controllers;
using WebHome.Controllers.Base;

namespace WebHome.Helper
{
    public static class DataProcessExtensions
    {
        public static void TransferLevel(this UserProfile profile, Documentary docItem, Naming.DocumentLevel docLevel, GenericManager<LcEntityDbContext>? models = null)
        {
            BusinessManager manager = new BusinessManager(models)
            {
                UserProfile = profile
            };

            if (docItem.DocType == (int)Naming.DocumentTypeDefinition.開狀申請書)
            {
                if (manager.ApproveLcApplication(docItem.DocID, profile.PID, docLevel))
                {
                    switch (docLevel)
                    {
                        case Naming.DocumentLevel.待經辦審核:
                            docItem.DocID.CreateInboxMessage(Naming.MessageTypeDefinition.MSG_CREDIT_APP_READY, Naming.MessageReceipent.ForBank);
                            manager.CreateMailMessage(docItem.DocID, Naming.MessageTypeDefinition.MSG_CREDIT_APP_READY, Naming.MessageReceipent.ForBank);
                            break;
                        case Naming.DocumentLevel.企業主管核放中:
                            docItem.DocID.CreateInboxMessage(Naming.MessageTypeDefinition.MSG_CREDIT_APP_APPLY, Naming.MessageReceipent.ForApplicant);
                            manager.CreateMailMessage(docItem.DocID, Naming.MessageTypeDefinition.MSG_CREDIT_APP_APPLY, Naming.MessageReceipent.ForApplicant);
                            break;
                        case Naming.DocumentLevel.企業主管退回_審核:
                        case Naming.DocumentLevel.企業主管退回_放行:
                            docItem.DocID.CreateInboxMessage(Naming.MessageTypeDefinition.MSG_CREDIT_APP_CANCELLED, Naming.MessageReceipent.ForApplicant);
                            manager.CreateMailMessage(docItem.DocID, Naming.MessageTypeDefinition.MSG_CREDIT_APP_CANCELLED, Naming.MessageReceipent.ForApplicant);
                            break;
                    }
                }
            }
            else if (docItem.DocType == (int)Naming.DocumentTypeDefinition.修狀申請書)
            {
                if (manager.ApproveAmendmentApplication(docItem.DocID, profile.PID, docLevel))
                {
                    switch (docLevel)
                    {
                        case Naming.DocumentLevel.待經辦審核:
                            docItem.DocID.CreateInboxMessage(Naming.MessageTypeDefinition.MSG_AMENDMENT_APP_READY, Naming.MessageReceipent.ForBank);
                            manager.CreateMailMessage(docItem.DocID, Naming.MessageTypeDefinition.MSG_AMENDMENT_APP_READY, Naming.MessageReceipent.ForBank);
                            break;
                        case Naming.DocumentLevel.企業主管核放中:
                            docItem.DocID.CreateInboxMessage(Naming.MessageTypeDefinition.MSG_AMENDMENT_APP_APPLY, Naming.MessageReceipent.ForApplicant);
                            manager.CreateMailMessage(docItem.DocID, Naming.MessageTypeDefinition.MSG_AMENDMENT_APP_APPLY, Naming.MessageReceipent.ForApplicant);
                            break;
                        case Naming.DocumentLevel.企業主管退回_審核:
                        case Naming.DocumentLevel.企業主管退回_放行:
                            docItem.DocID.CreateInboxMessage(Naming.MessageTypeDefinition.MSG_AMENDMENT_APP_CANCELLED, Naming.MessageReceipent.ForApplicant);
                            manager.CreateMailMessage(docItem.DocID, Naming.MessageTypeDefinition.MSG_AMENDMENT_APP_CANCELLED, Naming.MessageReceipent.ForApplicant);
                            break;
                    }
                }

            }
            else if (docItem.DocType == (int)Naming.DocumentTypeDefinition.信用狀註銷申請書)
            {
                if (manager.ApproveCreditCancellation(docItem.DocID, profile.PID, docLevel))
                {
                    switch (docLevel)
                    {
                        case Naming.DocumentLevel.待經辦審核:
                            docItem.DocID.CreateInboxMessage(Naming.MessageTypeDefinition.MSG_CANCELLATION_APP_READY, Naming.MessageReceipent.ForBank);
                            manager.CreateMailMessage(docItem.DocID, Naming.MessageTypeDefinition.MSG_CANCELLATION_APP_READY, Naming.MessageReceipent.ForBank);
                            break;
                        case Naming.DocumentLevel.企業主管核放中:
                            docItem.DocID.CreateInboxMessage(Naming.MessageTypeDefinition.MSG_CANCELLATION_APP_APPLY, Naming.MessageReceipent.ForApplicant);
                            manager.CreateMailMessage(docItem.DocID, Naming.MessageTypeDefinition.MSG_CANCELLATION_APP_APPLY, Naming.MessageReceipent.ForApplicant);
                            break;
                        case Naming.DocumentLevel.待賣方核定:
                            docItem.DocID.CreateInboxMessage(Naming.MessageTypeDefinition.MSG_CANCELLATION_APP_SELLER, Naming.MessageReceipent.ForBeneficiary);
                            manager.CreateMailMessage(docItem.DocID, Naming.MessageTypeDefinition.MSG_CANCELLATION_APP_SELLER, Naming.MessageReceipent.ForBeneficiary);
                            break;
                        case Naming.DocumentLevel.企業主管退回_審核:
                        case Naming.DocumentLevel.企業主管退回_放行:
                            docItem.DocID.CreateInboxMessage(Naming.MessageTypeDefinition.MSG_CANCELLATION_APP_CANCELLED, Naming.MessageReceipent.ForApplicant);
                            manager.CreateMailMessage(docItem.DocID, Naming.MessageTypeDefinition.MSG_CANCELLATION_APP_CANCELLED, Naming.MessageReceipent.ForApplicant);
                            break;
                    }
                }

            }
            else if (docItem.DocType == (int)Naming.DocumentTypeDefinition.押匯申請書)
            {
                if (manager.ApproveNegoDraft(docItem.DocID, profile.PID, docLevel))
                {
                    switch (docLevel)
                    {
                        case Naming.DocumentLevel.待經辦審核:
                            docItem.DocID.CreateInboxMessage(Naming.MessageTypeDefinition.MSG_NEGO_APP_READY, Naming.MessageReceipent.ForBank);
                            manager.CreateMailMessage(docItem.DocID, Naming.MessageTypeDefinition.MSG_NEGO_APP_READY, Naming.MessageReceipent.ForBank);
                            break;
                        case Naming.DocumentLevel.企業主管核放中:
                            docItem.DocID.CreateInboxMessage(Naming.MessageTypeDefinition.MSG_DRAFT, Naming.MessageReceipent.ForApplicant);
                            manager.CreateMailMessage(docItem.DocID, Naming.MessageTypeDefinition.MSG_DRAFT, Naming.MessageReceipent.ForApplicant);
                            break;
                        case Naming.DocumentLevel.企業主管退回_審核:
                        case Naming.DocumentLevel.企業主管退回_放行:
                            docItem.DocID.CreateInboxMessage(Naming.MessageTypeDefinition.MSG_NEGO_APP_CANCELLED, Naming.MessageReceipent.ForApplicant);
                            manager.CreateMailMessage(docItem.DocID, Naming.MessageTypeDefinition.MSG_NEGO_APP_CANCELLED, Naming.MessageReceipent.ForApplicant);
                            break;
                    }
                }

            }
        }

        public static void PrepareViewModel(this LcApplicationViewModel viewModel, CreditApplicationDocumentary item, AmendingLcApplication? amendment = null, LetterOfCredit? lc = null)
        {
            if(item == null)
                return;

            if (item.FpgLcItem != null)
            {
                viewModel.ServiceType = (Naming.DraftType?)item.Beneficiary.DraftType;
            }
            else if (item.ForService())
            {
                viewModel.ServiceType = Naming.DraftType.CDS_CSC;
            }

            var currentVersion = lc?.CurrentVersion;
            var lcItem = amendment?.LcItems ?? currentVersion?.LcItems ?? item.LcItems;
            var snItem = amendment?.SpecificNotes ?? currentVersion?.SpecificNotes ?? item.SpecificNotes;
            var attachableItem = amendment?.AttachableDocument ?? currentVersion?.AttachableDocument ?? item.AttachableDocument;

            void getGoodsDetail()
            {
                List<GoodsDetail> goods = new List<GoodsDetail>();
                var legacyGoods = lcItem.Goods.GetEfficientString();
                if (legacyGoods != null)
                {
                    goods.Add(new GoodsDetail
                    {
                        ProductName = legacyGoods
                    });
                }

                if (lcItem.GoodsDetail != null && lcItem.GoodsDetail.Count > 0)
                {
                    goods.AddRange(lcItem.GoodsDetail.ToArray());
                }

                viewModel.Amount = goods.Select(g => g.Amount).ToArray();
                viewModel.ProductName = goods.Select(g => g.ProductName).ToArray();
                viewModel.ProductSize = goods.Select(g => g.ProductSize).ToArray();
                viewModel.Quantity = goods.Select(g => g.Quantity).ToArray();
                viewModel.Remark = goods.Select(g => g.Remark).ToArray();
                viewModel.UnitPrice = goods.Select(g => g.UnitPrice).ToArray();

            }

            viewModel.Applicant = item.ApplicantID;
            viewModel.Beneficiary = item.BeneficiaryID;
            viewModel.IssuingBank = item.IssuingBankCode;
            viewModel.IssuingBankName = $"{item.IssuingBankCode} {item.CustomerOfBranch.BankCodeNavigation.BranchName}";

            viewModel.AdvisingBank = item.AdvisingBankCode;
            viewModel.AdvisingBankName = item.AdvisingBankCodeNavigation.BranchName;
            viewModel.AdvisingBankAddr = item.AdvisingBankCodeNavigation.Address;


            viewModel.AtSight = item.AtSight;
            viewModel.UsanceDay = item.UsanceDays;
            viewModel.VersionID = item.CustomerOfBranch.CustomerOfBranchVersionID;
            viewModel.BeneDetailID = item.Beneficiary.CustomerOfBranchVersionID;

            if (item.AtSight)
            {
                viewModel.AttachPayingAcceptance = attachableItem.匯票付款申請書;
            }
            else
            {
                viewModel.AttachPayingAcceptance = attachableItem.匯票承兌申請書;
            }
            viewModel.AttachInv = attachableItem.統一發票;
            viewModel.AttachEInv = attachableItem.電子發票證明聯;
            viewModel.AttachAdditional = attachableItem.其他.GetEfficientString();

            viewModel.LcAmt = lcItem.開狀金額;
            viewModel.SetLcExpiry(lcItem.有效期限);
            viewModel.Currency = lcItem.CurrencyTypeID;
            //viewModel.Goods = lcItem.Goods;
            getGoodsDetail();
            viewModel.UsanceDay = lcItem.定日付款;
            viewModel.SetPaymentDate(lcItem.PaymentDate);

            viewModel.Seal = snItem.原留印鑑相符;
            viewModel.BeneSeal = snItem.受益人單獨蓋章;
            viewModel.Partial = snItem.分批交貨;
            viewModel.SetNoAfterThan(snItem.最後交貨日);
            viewModel.SNAdditional = snItem.其他.GetEfficientString();
            viewModel.SetDraftDateStart(snItem.押匯起始日);
            viewModel.SetInvoiceDateStart(snItem.押匯發票起始日);
            viewModel.EarlyInvDate = snItem.接受發票早於開狀日;
            viewModel.InvoiceProductDetail = snItem.貨品明細以發票為準;
            viewModel.InvoiceAddr = snItem.接受發票人地址與受益人地址不符;

            if (item.FpgLcItem != null)
            {
                viewModel.CustomerNo = item.FpgLcItem.CustomerNo;
                viewModel.Department = item.FpgLcItem.DepartID;
                viewModel.CustomerNo = item.FpgLcItem.CustomerNo;
                viewModel.ContactName = item.FpgLcItem.ContactName;
                viewModel.ContactPhone = item.FpgLcItem.ContactPhone;
            }
        }


        public static void PrepareViewModel(this AmendmentRegistrationViewModel viewModel, AmendingLcRegistry item)
        {
            viewModel.ExchangeRate = item.匯率;
            viewModel.LcExpiryExtReason = item.延長信用狀原因;
            viewModel.DraftExtReason = item.延長匯票期限原因;
            viewModel.CancellationReason = item.沖銷原因;
            viewModel.GUNO = item.交易憑證編號;
        }



        public static void PrepareViewModel(this LcCancellationQueryViewModel viewModel, CreditCancellation item)
        {

        }


        public static readonly String[] __AcceptedFileFormat = new string[]
        {
            ".doc",".docx",".pdf",".xls",".xlsx",".txt",".gif",".jpg",".jpeg",".png",".tif",".bmp"
        };
        public static String? ProcessUploadFile(this Controller controller)
        {
            var ModelState = controller.ModelState;
            var ViewBag = controller.ViewBag;
            var Request = controller.Request;

            if (Request.Form.Files.Count > 0)
            {
                var docAttached = Request.Form.Files[0];
                String extName = Path.GetExtension(docAttached.FileName);
                if (!__AcceptedFileFormat.Contains(extName))
                {
                    ModelState.AddModelError("Message", String.Concat("只接受檔案格式：", String.Join("、", __AcceptedFileFormat)));
                    return null;
                }

                String attachedFile = Path.Combine(CommonLib.Core.Utility.Logger.LogDailyPath, $"{Guid.NewGuid()}{extName}");
                docAttached.SaveAs(attachedFile);

                return attachedFile;
            }

            return null;
        }

        public static String? ProcessUploadFile(this IFormFile docAttached, Controller controller)
        {
            var ModelState = controller.ModelState;
            var ViewBag = controller.ViewBag;
            var Request = controller.Request;

            String extName = Path.GetExtension(docAttached.FileName);
            if (!__AcceptedFileFormat.Contains(extName))
            {
                ModelState.AddModelError("Message", String.Concat("只接受檔案格式：", String.Join("、", __AcceptedFileFormat)));
                return null;
            }

            String attachedFile = Path.Combine(CommonLib.Core.Utility.Logger.LogDailyPath, $"{Guid.NewGuid()}{extName}");
            docAttached.SaveAs(attachedFile);

            return attachedFile;

        }

        public static CreditApplicationDocumentary? CommitCreditApplication(this LcAppBaseController controller, LcApplicationViewModel viewModel, bool overTheCounter = false)
        {
            ViewResult result = (ViewResult)controller.LoadCreditApplication(viewModel);
            CreditApplicationDocumentary? item = result.Model as CreditApplicationDocumentary;

            var profile = controller.HttpContext.GetUser();
            var models = controller.DataSource;
            var ModelState = controller.ModelState;
            var ViewBag = controller.ViewBag;
            var Request = controller.Request;

            void checkInput()
            {
                var cust = models.GetTable<CustomerOfBranch>()
                            .Where(b => b.BankCode == viewModel.IssuingBank
                                && b.OrganizationID == viewModel.Applicant)
                            .FirstOrDefault();
                if (cust == null)
                {
                    ModelState.AddModelError("Applicant", "信用狀開立申請人資料錯誤");
                }

                var advising = models.GetTable<BankData>()
                            .Where(b => b.BankCode == viewModel.AdvisingBank)
                            .FirstOrDefault();

                if (advising == null)
                {
                    ModelState.AddModelError("AdvisingBankCodeNavigation", "通知銀行不存在");
                }
                else
                {
                    var DisabledBranch = models.GetTable<DisabledBranch>()
                            .Where(b => b.BankCode == viewModel.AdvisingBank)
                            .FirstOrDefault();
                    if (DisabledBranch != null)
                    {
                        ModelState.AddModelError("AdvisingBankCodeNavigation", "通知行已不存在，請再重新選取!");
                    }
                }

                if (viewModel.AtSight == true)
                {
                    viewModel.UsanceDay = 0;
                    viewModel.PaymentDate = null;
                }

                viewModel.SNAdditional = viewModel.SNAdditional.GetEfficientString();
                if (viewModel.ckSNAdditional == true)
                {
                    if (viewModel.SNAdditional == null)
                    {
                        ModelState.AddModelError("SNAdditional", "請輸入特別指示其他內容");
                    }
                    else if (viewModel.SNAdditional.Length > 1024)
                    {
                        ModelState.AddModelError("SNAdditional", "特別指示-其他 長度不得大於1024");
                    }
                }

                viewModel.AttachAdditional = viewModel.AttachAdditional.GetEfficientString();
                if (viewModel.AttachEInv == true && viewModel.AttachInv != true)
                {
                    ModelState.AddModelError("AttachInv", "勾選檢附電子發票證明聯時需同時勾選檢附統一發票");
                }

                if (viewModel.ckAttachAdditional == true && viewModel.AttachAdditional == null)
                {
                    ModelState.AddModelError("AttachAdditional", "請輸入其他內容");
                }
                else if (viewModel.ckAttachAdditional == true && viewModel.AttachAdditional.Length > 256)
                {
                    ModelState.AddModelError("AttachAdditional", "檢附單證-其他 長度不得大於256");
                }
                else if (!(viewModel.AttachPayingAcceptance.HasValue || viewModel.AttachInv.HasValue || viewModel.ckAttachAdditional.HasValue))
                {
                    ModelState.AddModelError("AttachPayingAcceptance", "檢附單證-最少要勾選一種");
                    ModelState.AddModelError("AttachInv", "檢附單證-最少要勾選一種");
                    ModelState.AddModelError("ckAttachAdditional", "檢附單證-最少要勾選一種");
                }

                viewModel.AdvisingBank = viewModel.AdvisingBank.GetEfficientString();
                if (viewModel.AdvisingBank == null)
                {
                    ModelState.AddModelError("AdvisingBankCodeNavigation", "請選擇通知銀行");
                }

                if (!viewModel.LcExpiry_D.HasValue)
                {
                    ModelState.AddModelError("LcExpiry", "請選擇有效期限");
                }
                else if (viewModel.LcExpiry_D < DateTime.Today)
                {
                    ModelState.AddModelError("LcExpiry", "日期不能小於今天");
                }
                else if (cust?.CurrentLevel == (int)Naming.BeneficiaryStatus.已核准 && cust.PostponeMonths.HasValue)
                {
                    DateTime applyingStart = DateTime.Today;

                    if (viewModel.DesiredDate_D.HasValue
                        && viewModel.DesiredDate_D >= DateTime.Today
                        && viewModel.DesiredDate_D < DateTime.Today.AddMonths(1).AddDays(1))
                    {
                        applyingStart = viewModel.DesiredDate_D.Value;
                    }

                    if (viewModel.LcExpiry_D > applyingStart.AddMonths(cust.PostponeMonths.Value))
                    {
                        ModelState.AddModelError("LcExpiry", "有效期限超過延長開狀有效期限：" + cust.PostponeMonths.Value.ToString() + "個月");
                    }
                }
                else
                {
                    DateTime applyingStart = DateTime.Today;

                    if (viewModel.DesiredDate_D.HasValue
                        && viewModel.DesiredDate_D >= DateTime.Today
                        && viewModel.DesiredDate_D < DateTime.Today.AddMonths(1).AddDays(1))
                    {
                        applyingStart = viewModel.DesiredDate_D.Value;
                    }

                    if (viewModel.LcExpiry_D > applyingStart.AddMonths(4))
                    {
                        ModelState.AddModelError("LcExpiry", "有效期限超過四個月");
                    }
                }

                if (viewModel.AtUsance == true)
                {
                    if (!viewModel.UsanceDay.HasValue && !viewModel.PaymentDate_D.HasValue)
                    {
                        ModelState.AddModelError("UsanceDay", "定日付款天數及指定到期日必輸入一項");
                    }
                    else if (viewModel.UsanceDay != 0 && viewModel.PaymentDate_D.HasValue)
                    {
                        ModelState.AddModelError("UsanceDay", "定日付款天數及指定到期日只能擇一輸入");
                    }
                    else if ((!viewModel.UsanceDay.HasValue || viewModel.UsanceDay <= 0) && !viewModel.PaymentDate_D.HasValue)
                    {
                        ModelState.AddModelError("UsanceDay", "定日付款天數必須為數字且須大於 0 天");
                    }
                    else if (viewModel.UsanceDay > 180)
                    {
                        ModelState.AddModelError("UsanceDay", "定日付款天數不可大於180天");
                    }
                    else if (viewModel.PaymentDate_D.HasValue && viewModel.PaymentDate_D < DateTime.Today)
                    {
                        ModelState.AddModelError("PaymentDate", "指定到期日不能小於今天");
                    }
                }

                if (!viewModel.NoAfterThan_D.HasValue)
                {
                    ModelState.AddModelError("NoAfterThan", "請選擇最後交貨日");
                }
                else if (viewModel.NoAfterThan_D < DateTime.Today)
                {
                    ModelState.AddModelError("NoAfterThan", "交貨日期不能小於今天");
                }
                else if (viewModel.NoAfterThan_D > viewModel.LcExpiry_D)
                {
                    ModelState.AddModelError("NoAfterThan", "交貨日期不能大於信用狀有效期限");
                }

                viewModel.Goods = viewModel.Goods.GetEfficientString();
                if (viewModel.Goods == null)
                {
                    if (viewModel.ProductName == null || viewModel.ProductName.Length == 0)
                    {
                        ModelState.AddModelError("Goods", "請輸入貨物名稱");
                    }
                }
                else if (viewModel.Goods.Length > 512)
                {
                    ModelState.AddModelError("Goods", "貨物名稱長度不得大於512");
                }

                if (!viewModel.LcAmt.HasValue)
                {
                    ModelState.AddModelError("LcAmt", "請輸入開狀金額");
                }
                else if (viewModel.LcAmt <= 0)
                {
                    ModelState.AddModelError("LcAmt", "開狀金額錯誤");
                }

                if (overTheCounter == false && viewModel.ViewMode != Naming.ApplicationViewMode.Pending)
                {
                    if (viewModel.Agree != true)
                    {
                        ModelState.AddModelError("Agree", "請勾選同意!!");
                    }
                }

                if (viewModel.ServiceType == Naming.DraftType.FPG || viewModel.ServiceType == Naming.DraftType.CHIMEI)
                {
                    if (viewModel.ServiceType == Naming.DraftType.FPG)
                    {
                        viewModel.CustomerNo = viewModel.CustomerNo.GetEfficientString();
                        if (viewModel.CustomerNo == null)
                        {
                            ModelState.AddModelError("CustomerNo", "請輸入客戶編號");
                        }
                        else
                        {
                            Regex reg = new Regex("^[a-zA-Z0-9-]{1,6}$");
                            if (!reg.IsMatch(viewModel.CustomerNo))
                            {
                                ModelState.AddModelError("CustomerNo", "客戶編號僅限輸入英數字及 '-' 符號，長度不超過6");
                            }
                        }
                    }

                    viewModel.ContactName = viewModel.ContactName.GetEfficientString();
                    if (viewModel.ContactName == null)
                    {
                        ModelState.AddModelError("ContactName", "請輸入聯絡人姓名");
                    }

                    viewModel.ContactPhone = viewModel.ContactPhone.GetEfficientString();
                    if (viewModel.ContactPhone == null)
                    {
                        ModelState.AddModelError("ContactPhone", "請輸入聯絡人電話");
                    }

                    if (viewModel.EarlyInvDate == true)
                    {
                        if (!viewModel.InvoiceDateStart_D.HasValue)
                        {
                            ModelState.AddModelError("InvoiceDateStart", "請填入押匯發票起始日");
                        }
                        else
                        {
                            if (viewModel.InvoiceDateStart_D > viewModel.LcExpiry_D)
                            {
                                ModelState.AddModelError("InvoiceDateStart", "押匯發票起始日不能大於信用狀有效期限");
                            }
                            else if (viewModel.InvoiceDateStart_D < DateTime.Today.AddMonths(-3))
                            {
                                ModelState.AddModelError("InvoiceDateStart", "押匯發票起始日不早於開狀日3個月");
                            }
                        }
                    }

                    if (viewModel.DraftDateStart_D.HasValue)
                    {
                        if (viewModel.DraftDateStart_D > viewModel.LcExpiry_D)
                        {
                            ModelState.AddModelError("DraftDateStart", "押匯起始日不能大於信用狀有效期限");
                        }
                    }

                    viewModel.ContactPhone = viewModel.ContactPhone.GetEfficientString();
                    if (viewModel.ContactPhone != null)
                    {
                        viewModel.ContactPhone = viewModel.ContactPhone.Replace("分機", "#");
                        if (Encoding.Default.GetBytes(viewModel.ContactPhone).Length > 20)
                        {
                            ModelState.AddModelError("連絡人電話", "電話號碼長度不得大於20!!");
                        }
                    }
                }

                if (viewModel.ProductName != null)
                {
                    for (int idx = 0; idx < viewModel.ProductName.Length; idx++)
                    {
                        viewModel.ProductName[idx] = viewModel.ProductName[idx].GetEfficientString();
                        if (viewModel.ProductName[idx] == null)
                        {
                            ModelState.AddModelError("Goods", "請輸入貨物名稱");
                            break;
                        }

                        String val;
                        if (((val = viewModel.Amount[idx].GetEfficientString()) != null && !decimal.TryParse(val, out decimal decVal))
                            || ((val = viewModel.Quantity[idx].GetEfficientString()) != null && !decimal.TryParse(val, out decVal))
                            || ((val = viewModel.UnitPrice[idx].GetEfficientString()) != null && !decimal.TryParse(val, out decVal)))
                        {
                            ModelState.AddModelError("Goods", "單價、數量、金額需為數值!!");
                            break;
                        }
                    }
                }


                if (viewModel.DesiredDate_D.HasValue)
                {
                    if (viewModel.DesiredDate_D < DateTime.Today)
                    {
                        ModelState.AddModelError("DesiredDate", "預約開立日期不能小於今天");
                    }
                    else if (viewModel.DesiredDate_D > DateTime.Today.AddMonths(1))
                    {
                        ModelState.AddModelError("DesiredDate", "預約開立日期最長為一個月");
                    }
                }
            }

            checkInput();
            String? attachedFile = controller.ProcessUploadFile();

            if (!ModelState.IsValid)
            {
                ViewBag.ModelState = ModelState;
                return null;
            }

            if (viewModel.CopyFrom == true)
            {
                item = null;
            }

            if (item == null)
            {
                DateTime now = DateTime.Now;
                item = new CreditApplicationDocumentary
                {
                    Documentary = new Documentary
                    {
                        CurrentLevel = (int)Naming.DocumentLevel.文件預覽,
                        DocDate = now,
                        DocType = (int)Naming.DocumentTypeDefinition.開狀申請書,
                        SysDocID = "CDS",
                    },
                    LcItems = new LcItems
                    {
                        有效期限 = DateTime.Today
                    },
                    SpecificNotes = new SpecificNotes
                    {
                        最後交貨日 = DateTime.Today,
                        分批交貨 = true
                    },
                    AttachableDocument = new AttachableDocument { },
                    ApplicationDate = now,
                    AtSight = true,
                    OverTheCounter = overTheCounter,
                };

                if (viewModel.ServiceType == Naming.DraftType.FPG || viewModel.ServiceType == Naming.DraftType.CHIMEI)
                {
                    item.FpgLcItem = new FpgLcItem { };
                }

                models.GetTable<CreditApplicationDocumentary>().Add(item);

                item.Documentary.DocumentaryLevel.Add(new DocumentaryLevel
                {
                    DocLevel = (int)Naming.DocumentLevel.文件預覽,
                    LevelDate = now
                });

                DateTime currentYear = new DateTime(now.Year, 1, 1);

                if (viewModel.ServiceType == Naming.DraftType.FPG)
                {
                    var maxItem = models.GetTable<CreditApplicationDocumentary>()
                                .Where(a => a.ApplicationDate >= currentYear
                                    && a.ApplicationNo.StartsWith("FPG"))
                                .OrderByDescending(a => a.ApplicationNo).FirstOrDefault();

                    item.ApplicationNo = String.Format("FPG{0:000}{1:MMdd}{2:00000}", now.Year - 1911, now,
                        maxItem != null ? int.Parse(maxItem.ApplicationNo.Substring(maxItem.ApplicationNo.Length - 5)) + 1 : 1);

                }
                else if (viewModel.ServiceType == Naming.DraftType.CHIMEI)
                {
                    var maxItem = models.GetTable<CreditApplicationDocumentary>()
                                .Where(a => a.ApplicationDate >= currentYear
                                    && a.ApplicationNo.StartsWith("CHI"))
                                .OrderByDescending(a => a.ApplicationNo).FirstOrDefault();

                    item.ApplicationNo = String.Format("CHI{0:000}{1:MMdd}{2:00000}", now.Year - 1911, now,
                        maxItem != null ? int.Parse(maxItem.ApplicationNo.Substring(maxItem.ApplicationNo.Length - 5)) + 1 : 1);

                }
                else
                {
                    var maxItem = models.GetTable<CreditApplicationDocumentary>()
                                .Where(a => a.ApplicationDate >= currentYear
                                    && a.ApplicationNo.StartsWith("CHB"))
                                .OrderByDescending(a => a.ApplicationNo).FirstOrDefault();

                    item.ApplicationNo = String.Format("CHB{0:000}{1:MMdd}{2:00000}", now.Year - 1911, now,
                        maxItem != null ? int.Parse(maxItem.ApplicationNo.Substring(maxItem.ApplicationNo.Length - 5)) + 1 : 1);

                }

            }
            else
            {

            }

            item.Documentary.DesiredDate = viewModel.DesiredDate_D;

            item.PayableBankCode = viewModel.IssuingBank;
            item.IssuingBankCode = viewModel.IssuingBank;
            item.ApplicantID = viewModel.Applicant!.Value;
            item.BeneficiaryID = viewModel.Beneficiary!.Value;
            item.AdvisingBankCode = viewModel.AdvisingBank;
            item.AtSight = viewModel.AtSight == true;
            item.UsanceDays = viewModel.AtSight == true ? 0 : viewModel.UsanceDay!.Value;
            item.Instrunction = "非本行制式特別指示之申請原因及依據：";

            var bene = models.GetTable<BeneficiaryData>()
                .Where(b => b.OrganizationID == item.BeneficiaryID).First();
            item.ApplicantDetailsID = bene.CustomerOfBranchVersionID;
            item.BeneDetailsID = bene.CustomerOfBranchVersionID;

            if (viewModel.AtSight == true)
            {
                item.AttachableDocument.匯票付款申請書 = viewModel.AttachPayingAcceptance == true;
                item.AttachableDocument.匯票承兌申請書 = false;
            }
            else
            {
                item.AttachableDocument.匯票付款申請書 = false;
                item.AttachableDocument.匯票承兌申請書 = viewModel.AttachPayingAcceptance == true;
            }

            item.AttachableDocument.統一發票 = viewModel.AttachInv == true;
            item.AttachableDocument.電子發票證明聯 = viewModel.AttachEInv == true;
            item.AttachableDocument.其他 = viewModel.ckAttachAdditional == true
                ? viewModel.AttachAdditional
                : null;

            item.LcItems.開狀金額 = viewModel.LcAmt!.Value;
            item.LcItems.有效期限 = viewModel.LcExpiry_D;
            item.LcItems.CurrencyTypeID = viewModel.Currency!.Value;
            item.LcItems.Goods = viewModel.Goods;

            item.LcItems.定日付款 = viewModel.AtSight == true ? 0 : viewModel.UsanceDay!.Value;
            item.LcItems.PaymentDate = viewModel.PaymentDate_D;

            //item.SpecificNotes.原留印鑑相符 = Seal == true;
            //item.SpecificNotes.受益人單獨蓋章 = BeneSeal == true;
            item.SpecificNotes.原留印鑑相符 = bene.AppCountersign ?? true;
            item.SpecificNotes.受益人單獨蓋章 = !item.SpecificNotes.原留印鑑相符.Value;

            item.SpecificNotes.分批交貨 = viewModel.Partial == true;
            item.SpecificNotes.最後交貨日 = viewModel.NoAfterThan_D;
            item.SpecificNotes.其他 = viewModel.ckSNAdditional == true ? viewModel.SNAdditional : null;
            item.SpecificNotes.接受發票電子訊息 = viewModel.AcceptEInvoice;

            int sno = 1;
            if (item.LcItems.GoodsDetail.Count > 0)
            {
                sno = item.LcItems.GoodsDetail.Max(g => g.Sno) + 1;
                models.ExecuteCommand("delete GoodsDetail where LcItemsID = {0}", item.LcItems.ItemID);
            }

            if (viewModel.ProductName != null && viewModel.ProductName.Length > 0)
            {
                for (int idx = 0; idx < viewModel.ProductName.Length; idx++)
                {
                    GoodsDetail detail = new GoodsDetail
                    {
                        Amount = viewModel.Amount[idx],
                        ProductName = viewModel.ProductName[idx],
                        ProductSize = viewModel.ProductSize[idx],
                        Quantity = viewModel.Quantity[idx],
                        Remark = viewModel.Remark[idx],
                        UnitPrice = viewModel.UnitPrice[idx],
                        Sno = idx + sno,
                    };
                    item.LcItems.GoodsDetail.Add(detail);
                }
            }

            if (attachedFile != null)
            {
                item.Documentary.AttachedFile = attachedFile;
            }

            if (viewModel.ServiceType == Naming.DraftType.FPG || viewModel.ServiceType == Naming.DraftType.CHIMEI)
            {
                item.FpgLcItem.DepartID = viewModel.Department.GetEfficientString();
                item.FpgLcItem.CustomerNo = viewModel.CustomerNo;
                item.FpgLcItem.ContactName = viewModel.ContactName;
                item.FpgLcItem.ContactPhone = viewModel.ContactPhone;
                var status = models.GetTable<FpgBeneficiaryStatus>()
                    .Where(b => b.BeneID == item.BeneficiaryID).FirstOrDefault();
                if (status != null)
                    item.FpgLcItem.押匯允差比例 = status.押匯允差比例;

                item.FpgLcItem.GroupID = bene!.Organization!.OrganizationStatus!.GroupID!.Value;

                item.SpecificNotes.接受發票人地址與受益人地址不符 = viewModel.InvoiceAddr == true;
                item.SpecificNotes.貨品明細以發票為準 = viewModel.InvoiceProductDetail == true;
                item.SpecificNotes.接受發票電子訊息 = true;  // viewModel.AcceptEInvoice == true;
                item.SpecificNotes.接受發票金額大於匯票金額 = true;  // viewModel.LargerInvDraft == true;

                item.SpecificNotes.原留印鑑相符 = false;
                item.SpecificNotes.受益人單獨蓋章 = viewModel.BeneSeal == true;
                item.SpecificNotes.分批交貨 = viewModel.Partial == true;
                item.SpecificNotes.最後交貨日 = viewModel.NoAfterThan_D;
                item.SpecificNotes.押匯發票起始日 = viewModel.InvoiceDateStart_D;
                item.SpecificNotes.押匯起始日 = viewModel.DraftDateStart_D;
                item.SpecificNotes.接受發票早於開狀日 = viewModel.EarlyInvDate == true;
                item.SpecificNotes.接受發票金額大於開狀金額 = true;  // viewModel.LargerInvAmt == true;

                if (item.FpgLcItem.DepartID == null)
                {
                    var defaultDepartment = models.GetTable<GroupDepartment>()
                                                .Where(d => d.DepartID == GroupDepartment.DefaultDepartID)
                                                .Where(d => d.GroupID == item.FpgLcItem.GroupID)
                                                .FirstOrDefault();
                    if (defaultDepartment == null)
                    {
                        defaultDepartment = new GroupDepartment
                        {
                            DepartID = GroupDepartment.DefaultDepartID,
                            GroupID = item.FpgLcItem.GroupID,
                            Department = String.Empty,
                        };
                        models.GetTable<GroupDepartment>().Add(defaultDepartment);
                    }

                    item.FpgLcItem.GroupDepartment = defaultDepartment;

                }
            }

            models.SubmitChanges();
            return item;
        }

        public static void PrepareViewModel(this NegoDraftQueryViewModel viewModel, NegoDraft item)
        {
            viewModel.LcID = item.NegoLcVersionID;
            viewModel.DraftNo = item.DraftNo;
            viewModel.DraftAmount = item.Amount;
            viewModel.SetNegotiateDate(item.ShipmentDate);
            viewModel.AccountNo = item.NegoDraftExtension.BeneficiaryAccountNo;
            viewModel.NegoBranch = item.NegoDraftExtension.NegoBranch;
            viewModel.ItemName = item.ItemName;
            viewModel.ItemQuantity = item.ItemQuantity;
            viewModel.ItemSubtotal = item.ItemSubtotal;
        }


        public static void PrepareViewModel(this NegoInvoiceQueryViewModel viewModel, NegoInvoice item)
        {
            viewModel.DraftID = item.NegoDraftID;
            viewModel.InvoiceNo = item.InvoiceNo;
            viewModel.SetInvoiceDate(item.InvoiceDate);
            viewModel.InvoiceAmount = item.InvoiceAmount;
            viewModel.NegoAmount = item.NegoDraft.NegoAffair.Where(i => i.NegoInvoiceID == item.InvoiceID).FirstOrDefault()?.NegoAmount;
        }


        public static void PrepareViewModel(this CustomerOfBranchViewModel viewModel, CustomerOfBranch item)
        {
            viewModel.BankCode = item.BankCode;
            viewModel.CompanyID = item.OrganizationID;
            viewModel.PayableAccount = item.PayableAccount;
            viewModel.Addr = item.Addr;
            viewModel.Phone = item.Phone;
            viewModel.ContactEmail = item.ContactEmail;
            viewModel.Undertaker = item.Undertaker;
            viewModel.CompanyName = item.CompanyName;
            viewModel.CurrentVersion = item.CustomerOfBranchVersionID;
            viewModel.CurrentLevel = item.CurrentLevel;
            viewModel.PostponeMonths = item.PostponeMonths;
            viewModel.ReceiptNo = item.Organization.ReceiptNo;
        }

        public static bool ValidateProcess(this ApplicationProcessViewModel viewModel, SampleController controller, bool checkRemark = true, bool checkReason = true)
        {
            var profile = controller.HttpContext.GetUser();
            var models = controller.DataSource;
            var ModelState = controller.ModelState;
            var ViewBag = controller.ViewBag;
            var Request = controller.Request;

            viewModel.AntiXSS(controller.ViewData);

            void checkInput()
            {
                if (!((viewModel.ApprovalKey != null && viewModel.ApprovalKey.Length > 0)
                        || (viewModel.DenialKey != null && viewModel.DenialKey.Length > 0)))
                {
                    ModelState.AddModelError("Message", "請勾選審核項目!!");
                }
                else
                {
                    if (checkRemark)
                    {
                        if (viewModel.ApprovalKey != null && viewModel.ApprovalKey.Length > 0)
                        {
                            foreach (var appID in viewModel.ApprovalKey.Select(k => k.DecryptKeyValue()))
                            {
                                if (viewModel.NeededRemark(appID))
                                {
                                    ModelState.AddModelError("Message", "請填寫客戶有信用貶弱之狀態續送原因!!");
                                    break;
                                }
                            }
                        }
                    }
                }

                if (checkReason == true)
                {
                    if (viewModel.DenialKey != null && viewModel.DenialKey.Length > 0)
                    {
                        bool hasError = false;
                        if (viewModel.Reason == null || viewModel.Reason.Length != viewModel.DenialKey.Length
                            || viewModel.Others == null || viewModel.Others.Length != viewModel.DenialKey.Length)
                        {
                            hasError = true;
                        }

                        for (int idx = 0; idx < viewModel!.DenialKey.Length; idx++)
                        {
                            if (String.IsNullOrEmpty(viewModel!.Reason?[idx])
                                || (viewModel.Reason[idx].StartsWith("其他") && String.IsNullOrEmpty(viewModel!.Others?[idx])))
                            {
                                hasError = true;
                            }
                        }
                        if (hasError)
                        {
                            ModelState.AddModelError("", "勾選拒絕項目時，請選擇或輸入拒絕原因!!");
                        }
                    }

                }
            }

            checkInput();

            if (!ModelState.IsValid)
            {
                ViewBag.ModelState = ModelState;
                return false;
            }

            return true;
        }

        public static bool ValidateManualProcess(this ApplicationProcessViewModel viewModel, LcAppBaseController controller)
        {
            var profile = controller.HttpContext.GetUser();
            var models = controller.DataSource;
            var ModelState = controller.ModelState;
            var ViewBag = controller.ViewBag;
            var Request = controller.Request;

            viewModel.AntiXSS(controller.ViewData);

            void checkInput()
            {
                if (!((viewModel.ApprovalKey != null && viewModel.ApprovalKey.Length > 0)
                        /*|| (viewModel.DenialKey != null && viewModel.DenialKey.Length > 0)*/))
                {
                    ModelState.AddModelError("Message", "請勾選審核項目!!");
                }
                else
                {
                    if (viewModel.ApprovalKey != null && viewModel.ApprovalKey.Length > 0)
                    {
                        foreach (var appID in viewModel.ApprovalKey.Select(k => k.DecryptKeyValue()))
                        {
                            if (viewModel.NeededRemark(appID))
                            {
                                ModelState.AddModelError("Message", "請填寫客戶有信用貶弱之狀態續送原因!!");
                                break;
                            }
                        }
                    }
                }

                viewModel.DenialKey = null;
            }

            checkInput();

            if (!ModelState.IsValid)
            {
                ViewBag.ModelState = ModelState;
                return false;
            }

            return true;
        }

        public static bool ValidateManualProcessWithRemark(this ApplicationProcessViewModel viewModel, LcAppBaseController controller, String? alertEmptyRemark = null)
        {
            var profile = controller.HttpContext.GetUser();
            var models = controller.DataSource;
            var ModelState = controller.ModelState;
            var ViewBag = controller.ViewBag;
            var Request = controller.Request;

            if (alertEmptyRemark == null)
            {
                alertEmptyRemark = "請填必要欄位!!";
            }

            viewModel.AntiXSS(controller.ViewData);

            void checkInput()
            {
                if (!(viewModel.ApprovalKey != null && viewModel.ApprovalKey.Length > 0))
                {
                    ModelState.AddModelError("Message", "請勾選審核項目!!");
                }
                else
                {
                    if (viewModel.Remark == null || viewModel.Remark.Length == 0)
                    {
                        ModelState.AddModelError("Message", alertEmptyRemark);
                    }
                    else
                    {
                        for (int idx = 0; idx < viewModel.Remark.Length; idx++)
                        {
                            viewModel.Remark[idx] = viewModel.Remark[idx].GetEfficientString();
                            if (viewModel.Remark[idx] == null)
                            {
                                ModelState.AddModelError("Message", alertEmptyRemark);
                                break;
                            }
                        }
                    }
                }

                viewModel.DenialKey = null;
            }

            checkInput();

            if (!ModelState.IsValid)
            {
                ViewBag.ModelState = ModelState;
                return false;
            }

            return true;
        }

    }
}
