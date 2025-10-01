using CommonLib.Core.Utility;
using CommonLib.DataAccess;
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
        public static void TransferLevel(this UserProfile profile, Documentary docItem, Naming.DocumentLevel docLevel, GenericManager<LcEntityDataContext>? models = null)
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
            else if (docItem.DocType == (int)Naming.DocumentTypeDefinition.還款改貸申請書)
            {
                if (manager.ApproveReimbursement(docItem.DocID, profile.PID, docLevel))
                {
                    switch (docLevel)
                    {
                        case Naming.DocumentLevel.待經辦審核:
                            docItem.DocID.CreateInboxMessage(Naming.MessageTypeDefinition.MSG_REIM_APP_READY, Naming.MessageReceipent.ForBank);
                            manager.CreateMailMessage(docItem.DocID, Naming.MessageTypeDefinition.MSG_REIM_APP_READY, Naming.MessageReceipent.ForBank);
                            break;
                        case Naming.DocumentLevel.企業主管核放中:
                            docItem.DocID.CreateInboxMessage(Naming.MessageTypeDefinition.MSG_REIM_APP_APPLY, Naming.MessageReceipent.ForApplicant);
                            manager.CreateMailMessage(docItem.DocID, Naming.MessageTypeDefinition.MSG_REIM_APP_APPLY, Naming.MessageReceipent.ForApplicant);
                            break;
                        case Naming.DocumentLevel.企業主管退回_審核:
                        case Naming.DocumentLevel.企業主管退回_放行:
                            docItem.DocID.CreateInboxMessage(Naming.MessageTypeDefinition.MSG_REIM_APP_CANCELLED, Naming.MessageReceipent.ForApplicant);
                            manager.CreateMailMessage(docItem.DocID, Naming.MessageTypeDefinition.MSG_REIM_APP_CANCELLED, Naming.MessageReceipent.ForApplicant);
                            break;
                    }
                }
            }
            else if (docItem.DocType == (int)Naming.DocumentTypeDefinition.改貸還款)
            {
                if (manager.ApproveNegoLoanRepayment(docItem.DocID, profile.PID, docLevel))
                {
                    //switch (docLevel)
                    //{
                    //    case Naming.DocumentLevel.待經辦審核:
                    //        docItem.DocID.CreateInboxMessage(Naming.MessageTypeDefinition.MSG_REIM_APP_READY, Naming.MessageReceipent.ForBank);
                    //        manager.CreateMailMessage(docItem.DocID, Naming.MessageTypeDefinition.MSG_REIM_APP_READY, Naming.MessageReceipent.ForBank);
                    //        break;
                    //    case Naming.DocumentLevel.企業主管核放中:
                    //        docItem.DocID.CreateInboxMessage(Naming.MessageTypeDefinition.MSG_REIM_APP_APPLY, Naming.MessageReceipent.ForApplicant);
                    //        manager.CreateMailMessage(docItem.DocID, Naming.MessageTypeDefinition.MSG_REIM_APP_APPLY, Naming.MessageReceipent.ForApplicant);
                    //        break;
                    //    case Naming.DocumentLevel.企業主管退回_審核:
                    //    case Naming.DocumentLevel.企業主管退回_放行:
                    //        docItem.DocID.CreateInboxMessage(Naming.MessageTypeDefinition.MSG_REIM_APP_CANCELLED, Naming.MessageReceipent.ForApplicant);
                    //        manager.CreateMailMessage(docItem.DocID, Naming.MessageTypeDefinition.MSG_REIM_APP_CANCELLED, Naming.MessageReceipent.ForApplicant);
                    //        break;
                    //}
                }
            }
        }

        public static void PrepareViewModel(this LcApplicationViewModel viewModel, CreditApplicationDocumentary item, AmendingLcApplication? amendment = null, LetterOfCredit? lc = null)
        {
            if(item == null)
                return;

            if (item.FpgLcItem != null)
            {
                viewModel.ServiceType = (Naming.DraftType?)item.BeneficiaryData.DraftType;
            }
            else if (item.ForService())
            {
                viewModel.ServiceType = Naming.DraftType.CDS_CSC;
            }

            var currentVersion = lc?.CurrentVersion;
            var lcItem = amendment?.LcItem ?? currentVersion?.LcItem ?? item.LcItem;
            var snItem = amendment?.SpecificNote ?? currentVersion?.SpecificNote ?? item.SpecificNote;
            var attachableItem = amendment?.AttachableDocument ?? currentVersion?.AttachableDocument ?? item.AttachableDocument;

            void getGoodsDetails()
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

                if (lcItem.GoodsDetails != null && lcItem.GoodsDetails.Count > 0)
                {
                    goods.AddRange(lcItem.GoodsDetails.ToArray());
                }

                viewModel.Amount = goods.Select(g => g.Amount).ToArray();
                viewModel.ProductName = goods.Select(g => g.ProductName).ToArray();
                viewModel.ProductSize = goods.Select(g => g.ProductSize).ToArray();
                viewModel.Quantity = goods.Select(g => g.Quantity).ToArray();
                viewModel.Remark = goods.Select(g => g.Remark).ToArray();
                viewModel.UnitPrice = goods.Select(g => g.UnitPrice).ToArray();

            }

            viewModel.Applicant = item.申請人;
            viewModel.Beneficiary = item.受益人;
            viewModel.IssuingBank = item.開狀行;
            viewModel.IssuingBankName = $"{item.開狀行} {item.CustomerOfBranch.BankData.BranchName}";

            viewModel.AdvisingBank = item.通知行;
            viewModel.AdvisingBankName = item.AdvisingBank.BranchName;
            viewModel.AdvisingBankAddr = item.AdvisingBank.Address;


            viewModel.AtSight = item.見票即付;
            viewModel.UsanceDay = item.定日付款;
            viewModel.BalanceMode = item.沖銷保證金方式;
            viewModel.VersionID = item.CustomerOfBranch.CurrentVersion;
            viewModel.BeneDetailID = item.BeneficiaryData.CurrentVersion;

            if (item.見票即付)
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
            viewModel.Currency = lcItem.幣別;
            //viewModel.Goods = lcItem.Goods;
            getGoodsDetails();
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

        public static void PrepareViewModel(this PaymentNotificationQueryViewModel viewModel, PaymentNotification item)
        {
            viewModel.CreditAmt = item.實際送保金額;
            viewModel.IndividualCredit = item.個人金融授信;
            viewModel.IndustryCredit = item.企業金融授信;
            viewModel.CustomerID = item.CustomerID;
            viewModel.GuarRcptNo_1 = item.保證人統編_1;
            viewModel.GuarRcptNo_2 = item.保證人統編_2;
            viewModel.GuarRcptNo_3 = item.保證人統編_3;
            viewModel.GuarRcptNo_4 = item.保證人統編_4;
            viewModel.SecurityStatusValue = item.擔保品放貸文件徵提;
            viewModel.PayableAccount = item.扣款帳號;
            viewModel.CommissionStatusValue = item.費用收取;
            viewModel.保證手續費 = item.保證手續費;
            viewModel.IssuingFee = item.開改狀手續費;
            viewModel.AcceptanceCommission = item.承兌手續費;
            viewModel.ReceivableCommission = item.應收帳款墊款息;
            viewModel.AccountFee = item.帳戶管理費;
            viewModel.Security = item.保證金;
            viewModel.InitialFee = item.開辦費;
            viewModel.FundingFee = item.移送基金保證手續費;
            viewModel.LoanAccount = item.借戶帳號;
            viewModel.BorrowedAmount = item.借戶本金;
            viewModel.BorrowedInterest = item.借戶利息;
            viewModel.Renew = item.借新還舊.HasValue
                                ? item.借新還舊 == true
                                : item.信用狀改貸.HasValue
                                    ? !(item.信用狀改貸 == true)
                                    : (bool?)null;
            viewModel.LoanTypeValue = item.貸款種類;
            viewModel.SetAllowingDate(item.批准日期);
            viewModel.CreditNo = item.額度號碼;
            viewModel.LoanMonth = item.貸放期限月;
            viewModel.LoanDay = item.貸放期限天;
            viewModel.LoanAmt = item.貸放金額;
            viewModel.ServiceChargeRate = item.手續費率;
            viewModel.TranCertNo = item.交易憑證號碼;
            viewModel.UsageType = item.用途別;
            viewModel.SetInitLoanDate(item.初放貸放日);
            viewModel.PayingType = item.還本方式;
            viewModel.InterestType = item.利率型態;
            viewModel.ContractInterestType = item.訂約利率型態;
            viewModel.SetAdjustRateDate(item.下次利率調整日);
            viewModel.AllowanceDays = item.本筆寬限期;
            viewModel.CreditPercentage = item.信保成數;
            viewModel.CreditType = item.信保種類;
            viewModel.AllocationType = item.AllocationType();
            viewModel.TransferenceType = item.TransferenceType();
            viewModel.AccountNo = item.銷帳編號;
            viewModel.BusinessType = item.融資業務分類;
            viewModel.GovLoanType = item.政府專案補助貸款分類;
            viewModel.SetScanningDate(item.掃描日期);
            viewModel.LoanAdditional = item.貸放明細其他;
            viewModel.BadRecord = item.票據使用不良紀錄 == true;
            viewModel.DraftWithoutPayoff = item.押匯未清償 == true;
            viewModel.PrincipalOverdue = item.授信戶本金逾期 == true;
            viewModel.BackInterest = item.積欠利息 == true;
            viewModel.CreditCardOverdue = item.信用卡帳款逾期 == true;
            viewModel.AccountAlert = item.列管為警示戶 == true;
            viewModel.CreditAlert = item.信用貶落管控指標 == true;
            viewModel.Details = item.聯絡事項;
            viewModel.OtherSecurityStatus = item.擔保品放貸文件徵提其他;
            viewModel.Stakeholder = item.利害關係人 == true;
            viewModel.BankRelation = item.銀行法關係人 == true;
            viewModel.BankRelationContent = item.銀行法關係內容;
            viewModel.RetiredGuarantor = item.保證人辭卸解任;
            viewModel.AMLCheck = item.交易對手AML查詢;    // == true;
            viewModel.DocumentCheck = item.憑交易文件撥貸 == true;
            viewModel.ThreatCheck = item.資恐檢核表 == true;
            viewModel.SINo = item.聯貸流水號;
            viewModel.SILFG = item.聯貸流水號.HasValue;
            viewModel.SetLoanDue(item.貸放期限);
            viewModel.SetLoanExpiry(item.貸放到期日);
            viewModel.AccountType = item.帳戶性質;
            viewModel.AccountingSubject = item.會計科目;
            viewModel.IncomingAccount = item.入戶帳號;
            viewModel.SetInsuranceExpiry(item.送保到期日);

        }

        public static void PrepareViewModel(this L1203ViewModel viewModel, OpeningApplicationDocumentary item)
        {
            viewModel.SetOpeningDate(item.開狀日期);
            viewModel.CheckDueDays = item.匯票期限;
            viewModel.LoanMasterNo = item.貸放主管編號;
            viewModel.Payer = item.付款人;
            viewModel.OddDay = item.零星天數計收 == true;
            viewModel.CurrencyAmount = item.記帳金額;
            viewModel.GUNO = item.交易憑證編號;
        }

        public static void PrepareViewModel(this AmendmentRegistrationViewModel viewModel, AmendingLcRegistry item)
        {
            viewModel.ExchangeRate = item.匯率;
            viewModel.LcExpiryExtReason = item.延長信用狀原因;
            viewModel.DraftExtReason = item.延長匯票期限原因;
            viewModel.CancellationReason = item.沖銷原因;
            viewModel.GUNO = item.交易憑證編號;
        }

        public static void PrepareViewModel(this CancellationRegistrationViewModel viewModel, CancellationRegistry item)
        {
            viewModel.CancellationReason = item.沖銷原因;
            viewModel.IncLcAmt = item.沖銷信用狀金額;
            viewModel.ExchangeRate = item.匯率;
            viewModel.Security = item.沖銷存入保證金金額;
            viewModel.AllocationType = item.撥款方式.ToAllocationType();
            viewModel.IncomingAccount = item.入戶帳號;
            viewModel.ServiceCharge = item.手續費;
        }

        public static void PrepareViewModel(this NegoDraftRegistrationViewModel viewModel, NegoDraftRegistry item)
        {
            viewModel.CancellationReason = item.沖銷原因;
            viewModel.IncLcAmt = item.沖銷信用狀金額;
            viewModel.ExchangeRate = item.匯率;
            viewModel.Security = item.沖銷存入保證金金額;
            viewModel.AllocationType = item.撥款方式.ToAllocationType();
            viewModel.IncomingAccount = item.撥款帳號科目;
            viewModel.ServiceCharge = item.押匯手續費金額;
            viewModel.ServiceChargeRate = item.押匯手續費率;

            viewModel.BranchNo = item.支號;
            viewModel.InterestRateOfBank = item.掛牌利率;
            viewModel.InterestType = item.利率型態;
            viewModel.IncInterestRate = item.加減碼;
            viewModel.InterestAttribute = item.利率別;
            viewModel.GTXNO = item.GTXNO;
            viewModel.AcceptanceRate = item.承兌手續費率;
            viewModel.AcceptanceCommission = item.承兌手續費金額;
            viewModel.TransferenceType = item.現轉別.ToTransferenceType();
            viewModel.NegoCharge = item.票繳金額;
            viewModel.CheckNo = item.支票號碼;
            viewModel.UsageType = item.用途別;
            viewModel.BusinessType = item.融資業務分類;
            viewModel.GovLoanType = item.政府專案補助貸款分類;
            viewModel.Guarantee = item.十足擔保記號;
            viewModel.CreditPercentage = item.信保成數;
            viewModel.CreditType = item.信保種類;
            viewModel.ApplicationType = item.申請種類;
        }

        public static void PrepareViewModel(this LcCancellationQueryViewModel viewModel, CreditCancellation item)
        {

        }

        public static void PrepareViewModel(this AcceptanceRegistrationQueryViewModel viewModel, L4700 item)
        {
            viewModel.AllocationType = item.撥款方式.ToAllocationType();
            viewModel.IncomingAccount = item.入戶帳號;
            viewModel.SetAdvanceDate(item.墊款日);
            viewModel.InterestKind = item.利率種類;
            viewModel.AdvanceRate = item.墊款利率;
            viewModel.InterestAttribute = item.利率別;
            viewModel.IncInterestRate = item.加減碼;
            viewModel.InterestType = item.利率型態;
            viewModel.InterestRateOfBank = item.承作利率;
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
                                && b.CompanyID == viewModel.Applicant)
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
                    ModelState.AddModelError("AdvisingBank", "通知銀行不存在");
                }
                else
                {
                    var DisabledBranch = models.GetTable<DisabledBranch>()
                            .Where(b => b.BankCode == viewModel.AdvisingBank)
                            .FirstOrDefault();
                    if (DisabledBranch != null)
                    {
                        ModelState.AddModelError("AdvisingBank", "通知行已不存在，請再重新選取!");
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
                    ModelState.AddModelError("AdvisingBank", "請選擇通知銀行");
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
                        if (!cust!.UsanceLimitedDays.HasValue)
                        {
                            ModelState.AddModelError("UsanceDay", "定日付款天數不可大於180天");
                        }
                        else if (viewModel.UsanceDay > cust.UsanceLimitedDays)
                        {
                            ModelState.AddModelError("UsanceDay", String.Format("定日付款天數不可大於{0}天", cust.UsanceLimitedDays));
                        }
                    }
                    else if (viewModel.PaymentDate_D.HasValue && viewModel.PaymentDate_D < DateTime.Today)
                    {
                        ModelState.AddModelError("PaymentDate", "指定到期日不能小於今天");
                    }
                    else if (viewModel.PaymentDate_D.HasValue && viewModel.PaymentDate_D.Value.Subtract(DateTime.Today).Days > 180)
                    {
                        if (!cust!.UsanceLimitedDays.HasValue)
                        {
                            ModelState.AddModelError("PaymentDate", "指定到期日天數不可大於180天");
                        }
                        else if (viewModel.PaymentDate_D.Value.Subtract(DateTime.Today).Days > cust.UsanceLimitedDays.Value)
                        {
                            ModelState.AddModelError("PaymentDate", String.Format("指定到期日天數不可大於{0}天", cust.UsanceLimitedDays));
                        }
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
                    LcItem = new LcItem
                    {
                        有效期限 = DateTime.Today
                    },
                    SpecificNote = new SpecificNote
                    {
                        最後交貨日 = DateTime.Today,
                        分批交貨 = true
                    },
                    AttachableDocument = new AttachableDocument { },
                    ApplicationDate = now,
                    見票即付 = true,
                    OverTheCounter = overTheCounter,
                };

                if (viewModel.ServiceType == Naming.DraftType.FPG || viewModel.ServiceType == Naming.DraftType.CHIMEI)
                {
                    item.FpgLcItem = new FpgLcItem { };
                }

                models.GetTable<CreditApplicationDocumentary>().InsertOnSubmit(item);

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

            item.付款行 = viewModel.IssuingBank;
            item.開狀行 = viewModel.IssuingBank;
            item.申請人 = viewModel.Applicant!.Value;
            item.受益人 = viewModel.Beneficiary!.Value;
            item.通知行 = viewModel.AdvisingBank;
            item.見票即付 = viewModel.AtSight == true;
            item.定日付款 = viewModel.AtSight == true ? 0 : viewModel.UsanceDay!.Value;
            item.沖銷保證金方式 = viewModel.BalanceMode;
            item.Instrunction = "非本行制式特別指示之申請原因及依據：";

            var bene = models.GetTable<BeneficiaryData>()
                .Where(b => b.BeneID == item.受益人).First();
            item.VersionID = bene.CurrentVersion;
            item.BeneDetailID = bene.CurrentVersion;

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

            item.LcItem.開狀金額 = viewModel.LcAmt!.Value;
            item.LcItem.有效期限 = viewModel.LcExpiry_D;
            item.LcItem.幣別 = viewModel.Currency!.Value;
            item.LcItem.Goods = viewModel.Goods;

            item.LcItem.定日付款 = viewModel.AtSight == true ? 0 : viewModel.UsanceDay!.Value;
            item.LcItem.PaymentDate = viewModel.PaymentDate_D;

            //item.SpecificNote.原留印鑑相符 = Seal == true;
            //item.SpecificNote.受益人單獨蓋章 = BeneSeal == true;
            item.SpecificNote.原留印鑑相符 = bene.AppCountersign ?? true;
            item.SpecificNote.受益人單獨蓋章 = !item.SpecificNote.原留印鑑相符.Value;

            item.SpecificNote.分批交貨 = viewModel.Partial == true;
            item.SpecificNote.最後交貨日 = viewModel.NoAfterThan_D;
            item.SpecificNote.其他 = viewModel.ckSNAdditional == true ? viewModel.SNAdditional : null;
            item.SpecificNote.接受發票電子訊息 = viewModel.AcceptEInvoice;

            int sno = 1;
            if (item.LcItem.GoodsDetails.Count > 0)
            {
                sno = item.LcItem.GoodsDetails.Max(g => g.sno) + 1;
                models.ExecuteCommand("delete GoodsDetail where ItemID = {0}", item.LcItem.ItemID);
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
                        sno = idx + sno,
                    };
                    item.LcItem.GoodsDetails.Add(detail);
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
                    .Where(b => b.BankCode == item.通知行 && b.ApplicantID == item.申請人 && b.BeneID == item.受益人).FirstOrDefault();
                if (status != null)
                    item.FpgLcItem.押匯允差比例 = status.押匯允差比例;

                item.FpgLcItem.GroupID = bene!.Organization!.OrganizationStatus!.GroupID!.Value;

                item.SpecificNote.接受發票人地址與受益人地址不符 = viewModel.InvoiceAddr == true;
                item.SpecificNote.貨品明細以發票為準 = viewModel.InvoiceProductDetail == true;
                item.SpecificNote.接受發票電子訊息 = true;  // viewModel.AcceptEInvoice == true;
                item.SpecificNote.接受發票金額大於匯票金額 = true;  // viewModel.LargerInvDraft == true;

                item.SpecificNote.原留印鑑相符 = false;
                item.SpecificNote.受益人單獨蓋章 = viewModel.BeneSeal == true;
                item.SpecificNote.分批交貨 = viewModel.Partial == true;
                item.SpecificNote.最後交貨日 = viewModel.NoAfterThan_D;
                item.SpecificNote.押匯發票起始日 = viewModel.InvoiceDateStart_D;
                item.SpecificNote.押匯起始日 = viewModel.DraftDateStart_D;
                item.SpecificNote.接受發票早於開狀日 = viewModel.EarlyInvDate == true;
                item.SpecificNote.接受發票金額大於開狀金額 = true;  // viewModel.LargerInvAmt == true;

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
                        models.GetTable<GroupDepartment>().InsertOnSubmit(defaultDepartment);
                    }

                    item.FpgLcItem.GroupDepartment = defaultDepartment;

                }
            }

            models.SubmitChanges();
            return item;
        }

        public static void PrepareViewModel(this NegoDraftQueryViewModel viewModel, NegoDraft item)
        {
            viewModel.LcID = item.LcID;
            viewModel.DraftNo = item.DraftNo;
            viewModel.DraftAmount = item.Amount;
            viewModel.SetNegotiateDate(item.ShipmentDate);
            viewModel.AccountNo = item.NegoDraftExtension.入戶帳號;
            viewModel.NegoBranch = item.NegoDraftExtension.NegoBranch;
            viewModel.ItemName = item.ItemName;
            viewModel.ItemQuantity = item.ItemQuantity;
            viewModel.ItemSubtotal = item.ItemSubtotal;
        }


        public static void PrepareViewModel(this NegoInvoiceQueryViewModel viewModel, NegoInvoice item)
        {
            viewModel.DraftID = item.DraftID;
            viewModel.InvoiceNo = item.InvoiceNo;
            viewModel.SetInvoiceDate(item.InvoiceDate);
            viewModel.InvoiceAmount = item.InvoiceAmount;
            viewModel.NegoAmount = item.NegoDraft.NegoAffair.Where(i => i.InvoiceID == item.InvoiceID).FirstOrDefault()?.NegoAmount;
        }

        public static void PrepareViewModel(this ReimbursementQueryViewModel viewModel, Reimbursement item)
        {
            viewModel.DraftID = item.DraftID;
            viewModel.HasLoan = item.NegoLoan != null;
        }

        public static void PrepareViewModel(this NegoLoanRepaymentQueryViewModel viewModel, NegoLoanRepayment item)
        {
            viewModel.LoanID = item.LoanID;
            viewModel.RepaymentAmount = item.RepaymentAmount;
            viewModel.RepaymentDate = item.RepaymentDate;
            viewModel.InterestAmount = item.InterestAmount;
        }


        public static void PrepareViewModel(this CustomerOfBranchViewModel viewModel, CustomerOfBranch item)
        {
            viewModel.BankCode = item.BankCode;
            viewModel.CompanyID = item.CompanyID;
            viewModel.PayableAccount = item.PayableAccount;
            viewModel.Addr = item.Addr;
            viewModel.Phone = item.Phone;
            viewModel.ContactEmail = item.ContactEmail;
            viewModel.Undertaker = item.Undertaker;
            viewModel.CompanyName = item.CompanyName;
            viewModel.CurrentVersion = item.CurrentVersion;
            viewModel.CurrentLevel = item.CurrentLevel;
            viewModel.PostponeMonths = item.PostponeMonths;
            viewModel.UsancelimitedDays = item.UsanceLimitedDays;
            viewModel.ReceiptNo = item.Organization.ReceiptNo;
            if (item.CustomerOfBranchExtension != null)
            {
                var extension = item.CustomerOfBranchExtension;

                viewModel.ReimAccount = extension.約定還款帳號;
                viewModel.InterestRateLowerBound = extension.InterestRateLowerBound;
                viewModel.SetInterestRateLowerBoundExpiration(extension.InterestRateLowerBoundExpiration);
                viewModel.UseNegoLoan = extension.UseNegoLoan;
                viewModel.UseCreditInsurance = extension.UseCreditInsurance;
                viewModel.LoanCreditNo = extension.LoanCreditNo;
                viewModel.InsuranceCreditNo = extension.InsuranceCreditNo;
                viewModel.LoanPeriod = extension.LoanPeriod;
                viewModel.StartLoanAccordingTo = (CustomerOfBranchExtension.StartLoanBy?)extension.StartLoanAccordingTo;
                viewModel.SetChristeningDueDate(extension.ChristeningDueDate);
                viewModel.SetInsuranceExpiry(extension.InsuranceExpiry);
                viewModel.InsuredCreditAmount = extension.InsuredCreditAmount;
                viewModel.CreditPercentage = extension.CreditPercentage;
                viewModel.CreditType = extension.CreditType;
                viewModel.AuthorizeToRepay = extension.AuthorizeToRepay;
                viewModel.InterestRepayingType = (CustomerOfBranchExtension.InterestRepayment?)extension.InterestRepayingType;
                viewModel.InterestRepayingDay = extension.InterestRepayingDay;
                viewModel.AdjustInterestRate = extension.AdjustInterestRate;
                viewModel.InterestKind = extension.InterestKind;
                viewModel.InterestAttribute = extension.InterestAttribute;
                viewModel.InterestType = extension.InterestType;

            }
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
