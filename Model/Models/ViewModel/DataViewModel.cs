using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using ModelCore.DataModel;
using ModelCore.Helper;
using ModelCore.Locale;
using Newtonsoft.Json;
using CommonLib.Utility;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ModelCore.LcManagement;

namespace ModelCore.Models.ViewModel
{
    public class TaskViewModel
    {
        public String EncTaskID { get; set; }
        public Naming.TaskID? TaskName { get; set; }
        public String FlowSchema { get; set; }
        public String StepID { get; set; }
        [JsonIgnore]
        public bool? IsPostBack { get; set; }
        public TaskViewModel AntiXSS(ViewDataDictionary dataItems)
        {
            dataItems["ViewModel"] = this;
            return this;
        }
    }

    public class LoginViewModel : TaskViewModel
    {
        public string UID { get; set; }

        public string UUID { get; set; }

        public String ReturnUrl { get; set; }

        public String Epsk { get; set; }

        public String Auth { get; set; }
        public String Type { get; set; }
        public String TaskID { get; set; }

        public String Branch { get; set; }
        public String Path
        {
            get => ReturnUrl;
            set => ReturnUrl = value;
        }
        public String Remark { get; set; }
        public String APID { get; set; }
    }

    public class DocumentaryQueryViewModel : AuthorizedRoleQueryViewModel
    {
        public String[] Recipient { get; set; }
        public bool? ByUserRole { get; set; }
        public Naming.DocumentLevelQueryScope? QueryScope { get; set; }
        public Naming.DocumentLevel?[] LevelID { get; set; }
        public String DesiredDate
        {
            get => ValidityAgent.ConvertChineseDate(DesiredDate_D);
            set => DesiredDate_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? DesiredDate_D
        {
            get;
            private set;
        }
        public void SetDesiredDate(DateTime? value)
        {
            DesiredDate_D = value;
        }
    }

    public class PaymentNotificationQueryViewModel : QueryViewModel
    {
        public int? PaymentID { get; set; }
        public int? AppID { get; set; }
        public int? AmendingID { get; set; }
        public int? DraftID { get; set; }
        public int? LoanID { get; set; }
        public bool? IndustryCredit { get; set; }
        public bool? IndividualCredit { get; set; }
        public String CustomerID { get; set; }
        public String GuarRcptNo_1 { get; set; }
        public String GuarRcptNo_2 { get; set; }
        public String GuarRcptNo_3 { get; set; }
        public String GuarRcptNo_4 { get; set; }
        public String AccountingSubject { get; set; }
        public PaymentNotification.LoanType?[] LoanType { get; set; }
        public int? LoanTypeValue { get; set; }
        public PaymentNotification.SecurityStatus?[] SecurityStatus { get; set; }
        public int? SecurityStatusValue { get; set; }
        public String OtherSecurityStatus { get; set; }
        public String AllowingDate
        {
            get => ValidityAgent.ConvertChineseDate(AllowingDate_D);
            set => AllowingDate_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? AllowingDate_D
        {
            get;
            private set;
        }
        public void SetAllowingDate(DateTime? value)
        {
            AllowingDate_D = value;
        }

        public String CreditNo
        {
            get => CreditNoField.GetAccount();
            set => CreditNoField = value.SetAccountField(4, 2, 2, 6);
        }
        public String[] CreditNoField { get; set; }
        public int? LoanDay { get; set; }
        public int? LoanMonth { get; set; }
        public decimal? ServiceChargeRate { get; set; }
        public decimal? InterestRate { get; set; }
        public String TranCertNo { get; set; }
        public String UsageType { get; set; }
        public String InitLoanDate
        {
            get => ValidityAgent.ConvertChineseDate(InitLoanDate_D);
            set => InitLoanDate_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? InitLoanDate_D
        {
            get;
            private set;
        }
        public void SetInitLoanDate(DateTime? value)
        {
            InitLoanDate_D = value;
        }
        public String PayingType { get; set; }
        public String InterestType { get; set; }
        public String ContractInterestType { get; set; }
        public String AdjustRateDate
        {
            get => ValidityAgent.ConvertChineseDate(AdjustRateDate_D);
            set => AdjustRateDate_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? AdjustRateDate_D
        {
            get;
            private set;
        }
        public void SetAdjustRateDate(DateTime? value)
        {
            AdjustRateDate_D = value;
        }
        public int? AllowanceDays { get; set; }
        public decimal? CreditPercentage { get; set; }
        public String CreditType { get; set; }
        public decimal? CreditAmt { get; set; }
        public String InsuranceExpiry
        {
            get => ValidityAgent.ConvertChineseDate(InsuranceExpiry_D);
            set => InsuranceExpiry_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? InsuranceExpiry_D
        {
            get;
            private set;
        }
        public void SetInsuranceExpiry(DateTime? value)
        {
            InsuranceExpiry_D = value;
        }
        public PaymentNotification.AllocationType? AllocationType { get; set; }
        public PaymentNotification.TransferenceType? TransferenceType { get; set; }
        public String PayableAccount
        {
            get => PayableAccountField.GetAccount();
            set => PayableAccountField = value.SetAccountField(4, 2, 5, 1, 2);
        }
        public String[] PayableAccountField { get; set; }
        public String AccountNo { get; set; }
        public String BusinessType { get; set; }
        public String GovLoanType { get; set; }
        public String ScanningDate
        {
            get => ValidityAgent.ConvertChineseDate(ScanningDate_D);
            set => ScanningDate_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? ScanningDate_D
        {
            get;
            private set;
        }
        public void SetScanningDate(DateTime? value)
        {
            ScanningDate_D = value;
        }
        public String LoanAdditional { get; set; }
        public PaymentNotification.CommissionStatus?[] CommissionStatus { get; set; }
        public int? CommissionStatusValue { get; set; }
        [JsonIgnore]
        public decimal? 保證手續費 { get; set; }
        public decimal? SecurityCommission
        {
            get => 保證手續費;
            set => 保證手續費 = value;
        }

        public decimal? IssuingFee { get; set; }
        public decimal? AcceptanceCommission { get; set; }
        public decimal? ReceivableCommission { get; set; }
        public decimal? Security { get; set; }
        public decimal? BorrowedAmount { get; set; }
        public decimal? BorrowedInterest { get; set; }
        public decimal? InitialFee
        {
            get;
            set;
        }
        public decimal? FundingFee { get; set; }

        public decimal? LoanAmt { get; set; }
        public decimal? AccountFee { get; set; }
        public bool? BadRecord { get; set; }
        public bool? DraftWithoutPayoff { get; set; }
        public bool? PrincipalOverdue { get; set; }
        public bool? BackInterest { get; set; }
        public bool? Stakeholder { get; set; }
        public bool? BankRelation { get; set; }
        public bool? CreditCardOverdue { get; set; }
        public bool? AccountAlert { get; set; }
        public bool? CreditAlert { get; set; }
        public bool? AMLCheck { get; set; }
        public bool? ThreatCheck { get; set; }
        public bool? DocumentCheck { get; set; }

        public String BankRelationContent { get; set; }
        public int? RetiredGuarantor { get; set; }
        public bool? SILFG { get; set; }
        public int? SINo { get; set; }
        public String Details { get; set; }
        public bool? Renew { get; set; }
        public String AccountType
        {
            get => AccountTypeField.GetAccount();
            set => AccountTypeField = value.SetAccountField();
        }
        public String[] AccountTypeField { get; set; }
        public String LoanExpiry
        {
            get => ValidityAgent.ConvertChineseDate(LoanExpiry_D);
            set => LoanExpiry_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? LoanExpiry_D
        {
            get;
            private set;
        }
        public void SetLoanExpiry(DateTime? value)
        {
            LoanExpiry_D = value;
        }
        public String LoanDue
        {
            get => ValidityAgent.ConvertChineseDate(LoanDue_D);
            set => LoanDue_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? LoanDue_D
        {
            get;
            private set;
        }
        public void SetLoanDue(DateTime? value)
        {
            LoanDue_D = value;
        }

        public String BranchName { get; set; }
        public String BranchCode { get; set; }
        public String ReceiptNo { get; set; }
        public String Borrower { get; set; }

        public String IncomingAccount
        {
            get => IncomingAccountField.GetAccount();
            set => IncomingAccountField = value.SetAccountField(4, 2, 5, 1, 2);
        }
        public String[] IncomingAccountField { get; set; }
        public String LoanAccount
        {
            get => LoanAccountField.GetAccount();
            set => LoanAccountField = value.SetAccountField();
        }
        [JsonIgnore]
        public String[] LoanAccountField { get; set; }
    }

    public class L1203ViewModel : QueryViewModel
    {
        static long __SEQ_NO;

        static L1203ViewModel()
        {
            using (LcManager mgr = new LcManager())
            {
                __SEQ_NO = mgr.GetTable<OpeningApplicationDocumentary>().Count();
            }
        }

        public L1203ViewModel()
        {
            GTXNO = System.Threading.Interlocked.Increment(ref __SEQ_NO);
        }

        public int? OpeningID { get; set; }
        public int? AppID { get; set; }
        public String OpeningDate
        {
            get => ValidityAgent.ConvertChineseDate(OpeningDate_D);
            set => OpeningDate_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? OpeningDate_D
        {
            get;
            private set;
        }
        public void SetOpeningDate(DateTime? value)
        {
            OpeningDate_D = value;
        }

        public String 信用狀種類
        {
            get => "1台幣一般";
        }
        public decimal? CurrencyRate { get; set; }
        public decimal? CurrencyAmount { get; set; }
        public int? CheckDueDays { get; set; }
        public decimal? AcceptChargeRate { get; set; }
        public decimal? PayAmount { get; set; }
        public String CheckNo { get; set; }
        public String PayingBranchNo
        {
            get => PayingBranchNoField.GetAccount();
            set => PayingBranchNoField = value.SetAccountField();
        }
        public String[] PayingBranchNoField { get; set; }
        public String ReceiptPersonAccount { get; set; }
        public String LoanMasterNo { get; set; }
        public String LoanPersonNo { get; set; }
        public String BusinessType { get; set; }
        public String GovLoanType { get; set; }
        public String CreditType { get; set; }
        public bool? GuaranteeYes { get; set; }
        public String Payer
        {
            get => PayerField.GetAccount();
            set => PayerField = value.SetAccountField();
        }
        public String[] PayerField { get; set; }
        public long GTXNO { get; }
        public bool? OddDay { get; set; }
        public String GUNO { get; set; }

    }

    public class AmendmentRegistrationViewModel : QueryViewModel
    {
        public int? AmendingID { get; set; }
        public int? RegistrationID { get; set; }
        public decimal? ExchangeRate { get; set; }
        public String LcExpiryExtReason { get; set; }
        public String DraftExtReason { get; set; }
        public String CancellationReason { get; set; }
        public String CreditType { get; set; }
        public bool? HasGUNO { get; set; }
        public String GUNO { get; set; }

    }

    public class ApplicationProcessViewModel : CreditApplicationQueryViewModel
    {
        public bool? Approval { get; set; }
        public String RejectReason
        {
            get => Reason?[0];
        }
        public String OtherRejectReason
        {
            get => Others?[0].GetEfficientString();
        }
        public String Memo { get; set; }
        public String Instruction { get; set; }

        protected int?[] _ItemID;
        public String GetRemark(int? appID, bool reset = false)
        {
            if (_ItemID == null || reset)
            {
                if (KeyItems != null && KeyItems.Length > 0)
                {
                    _ItemID = KeyItems.Select(k => (int?)k.DecryptKeyValue()).ToArray();
                }
            }

            if (_ItemID != null)
            {
                int idx = Array.IndexOf(_ItemID, appID);
                if (idx >= 0)
                {
                    return Remark?[idx];
                }
            }
            return null;
        }

        public bool NeededRemark(int? appID, bool reset = false)
        {
            if (_ItemID == null || reset)
            {
                if (KeyItems != null && KeyItems.Length > 0)
                {
                    _ItemID = KeyItems.Select(k => (int?)k.DecryptKeyValue()).ToArray();
                }
            }

            if (_ItemID == null)
                return false;

            int idx = Array.IndexOf(_ItemID, appID);
            if (idx < 0)
                return false;

            return Remark?[idx].GetEfficientString() == null;
        }

        public bool? MarkManual { get; set; }

        [JsonIgnore]
        public bool? IsSuccessful { get; set; }
        public Naming.DraftType? ServiceType { get; set; }

        [JsonIgnore]
        public Naming.DraftType? DraftType
        {
            get => ServiceType;
            set => ServiceType = value;
        }

    }

    public class LcApplicationViewModel : ApplicationProcessViewModel
    {

        public String IssuingBank { get; set; }
        public String IssuingBankName { get; set; }
        public String AdvisingBank { get; set; }
        public String AdvisingBankName { get; set; }
        public String AdvisingBankAddr { get; set; }
        public String LcExpiry
        {
            get => ValidityAgent.ConvertChineseDate(LcExpiry_D);
            set => LcExpiry_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? LcExpiry_D
        {
            get;
            private set;
        }
        public void SetLcExpiry(DateTime? value)
        {
            LcExpiry_D = value;
        }


        public decimal? LcAmt { get; set; }
        public bool? CopyFrom { get; set; }
        public int? UsanceDay { get; set; }
        public String BalanceMode { get; set; }
        public int? VersionID { get; set; }
        public int? BeneDetailID { get; set; }
        public bool? AttachPayingAcceptance { get; set; }
        public bool? AttachInv { get; set; }
        public bool? AttachEInv { get; set; }
        public String AttachAdditional { get; set; }
        public int? Currency { get; set; }
        public String Goods { get; set; }
        public String[] ProductName { get; set; }
        public String[] ProductSize { get; set; }
        public String[] UnitPrice { get; set; }
        public String[] Quantity { get; set; }
        public String[] Amount { get; set; }
        public String PaymentDate
        {
            get => ValidityAgent.ConvertChineseDate(PaymentDate_D);
            set => PaymentDate_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? PaymentDate_D
        {
            get;
            private set;
        }
        public void SetPaymentDate(DateTime? value)
        {
            PaymentDate_D = value;
        }

        public bool? Seal { get; set; }
        public bool? BeneSeal { get; set; }
        public String NoAfterThan
        {
            get => ValidityAgent.ConvertChineseDate(NoAfterThan_D);
            set => NoAfterThan_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? NoAfterThan_D
        {
            get;
            private set;
        }
        public void SetNoAfterThan(DateTime? value)
        {
            NoAfterThan_D = value;
        }

        public bool? Partial { get; set; }
        public String SNAdditional { get; set; }
        public bool? ckSNAdditional { get; set; }
        public bool? ckAttachAdditional { get; set; }
        public String AttachedFile { get; set; }
        public bool? Agree { get; set; }
        public String ContactName { get; set; }
        public String ContactPhone { get; set; }
        public String CustomerNo { get; set; }
        public String Department { get; set; }
        public bool? LargerInvAmt { get; set; }
        public bool? EarlyInvDate { get; set; }
        public bool? InvoiceAddr { get; set; }
        public bool? InvoiceProductDetail { get; set; }
        public bool? AcceptEInvoice { get; set; }
        public bool? LargerInvDraft { get; set; }
        public String InvoiceDateStart
        {
            get => ValidityAgent.ConvertChineseDate(InvoiceDateStart_D);
            set => InvoiceDateStart_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? InvoiceDateStart_D
        {
            get;
            private set;
        }
        public void SetInvoiceDateStart(DateTime? value)
        {
            InvoiceDateStart_D = value;
        }

        public String DraftDateStart
        {
            get => ValidityAgent.ConvertChineseDate(DraftDateStart_D);
            set => DraftDateStart_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? DraftDateStart_D
        {
            get;
            private set;
        }

        public void SetDraftDateStart(DateTime? value)
        {
            DraftDateStart_D = value;
        }

    }

    public class LcQueryViewModel : LcApplicationViewModel
    {
        public int? LcID { get; set; }
        public String LcDateFrom
        {
            get => ValidityAgent.ConvertChineseDate(LcDateFrom_D);
            set => LcDateFrom_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? LcDateFrom_D
        {
            get;
            private set;
        }
        public void SetLcDateFrom(DateTime? value)
        {
            LcDateFrom_D = value;
        }
        public String LcDateTo
        {
            get => ValidityAgent.ConvertChineseDate(LcDateTo_D);
            set => LcDateTo_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? LcDateTo_D
        {
            get;
            private set;
        }
        public void SetLcDateTo(DateTime? value)
        {
            LcDateTo_D = value;
        }
        public Naming.LcStatus? LcStatus { get; set; }
        public int? Version { get; set; }
        public bool? ShowVersion { get; set; }
    }

    public class LcAmendmentQueryViewModel : LcQueryViewModel
    {
        public String AmendingDateFrom
        {
            get => ValidityAgent.ConvertChineseDate(AmendingDateFrom_D);
            set => AmendingDateFrom_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? AmendingDateFrom_D
        {
            get;
            private set;
        }
        public void SetAmendingDateFrom(DateTime? value)
        {
            AmendingDateFrom_D = value;
        }

        public String AmendingDateTo
        {
            get => ValidityAgent.ConvertChineseDate(AmendingDateTo_D);
            set => AmendingDateTo_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? AmendingDateTo_D
        {
            get;
            private set;
        }
        public void SetAmendingDateTo(DateTime? value)
        {
            AmendingDateTo_D = value;
        }
        public String AmendingNo { get; set; }
        public int? AmendingID { get; set; }
        public bool? Amending { get; set; }
        public String InformationNo { get; set; }
    }

    public class LcApplicationProcessViewModel : ApplicationProcessViewModel
    {
        public int? CreditNo { get; set; }
    }

    public class LcAmendmentProcessViewModel : LcAmendmentQueryViewModel
    {

    }

    public class LcCancellationQueryViewModel : LcQueryViewModel
    {
        public int? CancellationID { get; set; }
        public bool? Cancelling { get; set; }
        public String CancellationNo { get; set; }
        public String CancellationDateFrom
        {
            get => ValidityAgent.ConvertChineseDate(CancellationDateFrom_D);
            set => CancellationDateFrom_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? CancellationDateFrom_D
        {
            get;
            private set;
        }
        public void SetCancellationDateFrom(DateTime? value)
        {
            CancellationDateFrom_D = value;
        }

        public String CancellationDateTo
        {
            get => ValidityAgent.ConvertChineseDate(CancellationDateTo_D);
            set => CancellationDateTo_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? CancellationDateTo_D
        {
            get;
            private set;
        }
        public void SetCancellationDateTo(DateTime? value)
        {
            CancellationDateTo_D = value;
        }
    }

    public class NegoDraftQueryViewModel : LcQueryViewModel
    {
        public int? DraftID { get; set; }
        public String DraftNo { get; set; }
        public int? AppYear { get; set; }
        public int? AppSeq { get; set; }
        public decimal? DraftAmount { get; set; }
        public String NegotiateDate
        {
            get => ValidityAgent.ConvertChineseDate(NegotiateDate_D);
            set => NegotiateDate_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? NegotiateDate_D
        {
            get;
            private set;
        }
        public void SetNegotiateDate(DateTime? value)
        {
            NegotiateDate_D = value;
        }
        public String AccountNo { get; set; }
        public String NegoBranch { get; set; }
        public String ItemName { get; set; }
        public decimal? ItemQuantity { get; set; }
        public decimal? ItemSubtotal { get; set; }
        public String ImportDateFrom
        {
            get => ValidityAgent.ConvertChineseDate(ImportDateFrom_D);
            set => ImportDateFrom_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? ImportDateFrom_D
        {
            get;
            private set;
        }
        public void SetImportDateFrom(DateTime? value)
        {
            ImportDateFrom_D = value;
        }
        public String ImportDateTo
        {
            get => ValidityAgent.ConvertChineseDate(ImportDateTo_D);
            set => ImportDateTo_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? ImportDateTo_D
        {
            get;
            private set;
        }
        public void SetImportDateTo(DateTime? value)
        {
            ImportDateTo_D = value;
        }

        public Naming.BusinessType? BusinessType { get; set; }
        public String ReceiptNo { get; set; }
    }

    public class NegoInvoiceQueryViewModel : DocumentaryQueryViewModel
    {
        public int? DraftID { get; set; }
        public int? InvoiceID { get; set; }
        public String InvoiceNo { get; set; }
        public String InvoiceDate
        {
            get => ValidityAgent.ConvertChineseDate(InvoiceDate_D);
            set => InvoiceDate_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? InvoiceDate_D
        {
            get;
            private set;
        }
        public void SetInvoiceDate(DateTime? value)
        {
            InvoiceDate_D = value;
        }
        public decimal? InvoiceAmount { get; set; }
        public decimal? NegoAmount { get; set; }
        public String[] ProductName { get; set; }
        public decimal?[] UnitPrice { get; set; }
        public decimal?[] Quantity { get; set; }
        public decimal?[] Amount { get; set; }
    }

    public class InvoiceDetailQueryViewModel : DocumentaryQueryViewModel
    {
        public int? InvoiceID { get; set; }
        public int? ProductID { get; set; }

        public String ProductName { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Amount { get; set; }

    }

    public class ReimbursementQueryViewModel : NegoDraftQueryViewModel
    {
        public int? ReimID { get; set; }
        public bool? HasLoan { get; set; }

        public String ReimbursementNo { get; set; }

        public String ReimbursementDateFrom
        {
            get => ValidityAgent.ConvertChineseDate(ReimbursementDateFrom_D);
            set => ReimbursementDateFrom_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? ReimbursementDateFrom_D
        {
            get;
            private set;
        }
        public void SetReimbursementDateFrom(DateTime? value)
        {
            ReimbursementDateFrom_D = value;
        }
        public String ReimbursementDateTo
        {
            get => ValidityAgent.ConvertChineseDate(ReimbursementDateTo_D);
            set => ReimbursementDateTo_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? ReimbursementDateTo_D
        {
            get;
            private set;
        }
        public void SetReimbursementDateTo(DateTime? value)
        {
            ReimbursementDateTo_D = value;
        }

        public decimal? LoanInterestRate { get; set; }
        public DateTime? InitLoanDate { get; set; }
        public int? LoanDay { get; set; }

    }

    public class NegoLoanQueryViewModel : ReimbursementQueryViewModel
    {
        [JsonIgnore]
        public int? LoanID
        {
            get => ReimID;
            set => ReimID = value;
        }
    }

    public class NegoLoanRepaymentQueryViewModel : NegoLoanQueryViewModel
    {
        public int? RepaymentID { get; set; }
        public DateTime? RepaymentDate { get; set; }
        public decimal? RepaymentAmount { get; set; }
        public decimal? InterestAmount { get; set; }
        public bool? CheckBalance { get; set; }
    }

    public class CancellationRegistrationViewModel : L4500ViewModel
    {
        public int? CancellationID { get; set; }
        public int? RegistrationID { get; set; }
    }

    public class NegoDraftRegistrationViewModel : L4500ViewModel, IInterestRateQueryViewModel
    {
        public int? DraftID { get; set; }
        public int? RegistrationID { get; set; }
        public decimal? InterestRateOfBank { get; set; }
        public String InterestKind { get; set; }
        public String InterestAttribute { get; set; }
        public String InterestType { get; set; }

        private decimal _incInterestRate = 0;
        public decimal? ABSIncInterestRate
        {
            get => Math.Abs(_incInterestRate);
            set
            {
                if (value.HasValue)
                {
                    _incInterestRate = Math.Abs(value.Value) * _factor;
                }
                else
                {
                    _incInterestRate = 0;
                }
            }
        }

        [JsonIgnore]
        public decimal? IncInterestRate
        {
            get => _incInterestRate;
            set
            {
                if (value.HasValue)
                {
                    _incInterestRate = value.Value;
                    _factor = value >= 0 ? 1 : -1;
                }
                else
                {
                    _incInterestRate = 0;
                }
            }
        }

        private int _factor = -1;
        public String PlusMinus
        {
            get => _factor >= 0 ? "+" : "-";
            set
            {
                _factor = value == "+" ? 1 : -1;
                _incInterestRate = Math.Abs(_incInterestRate) * _factor;
            }
        }

        public decimal? CreditPercentage { get; set; }
        public String CreditType { get; set; }
        public String ApplicationType { get; set; } = "1：電子化";
        public decimal? AcceptanceRate { get; set; }
        public decimal? AcceptanceCommission { get; set; }
        public PaymentNotification.TransferenceType? TransferenceType { get; set; }
        public decimal? NegoCharge { get; set; }
        public String CheckNo { get; set; }
        public String CheckBankNo { get; set; }
        public String UsageType { get; set; }
        public String BusinessType { get; set; }
        public String GovLoanType { get; set; }
        public bool? Guarantee { get; set; }
        public String AccountingNo { get; set; } = "1154";
        public String AccountingMinorNo { get; set; } = "300";


    }

    public class DraftAcceptanceQueryViewModel : NegoDraftQueryViewModel
    {
        public int? AcceptanceID { get; set; }
    }

    public class AcceptanceRegistrationQueryViewModel : NegoDraftRegistrationViewModel
    {
        public int? AcceptanceID { get; set; }
        public int? L4700ID { get; set; }

        public String AdvanceDate
        {
            get => ValidityAgent.ConvertChineseDate(AdvanceDate_D);
            set => AdvanceDate_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? AdvanceDate_D
        {
            get;
            private set;
        }
        public void SetAdvanceDate(DateTime? value)
        {
            AdvanceDate_D = value;
        }

    }

    public class ReimbursementRegistrationQueryViewModel : NegoDraftRegistrationViewModel
    {
        public int? ReimID { get; set; }
        public int? L4600ID { get; set; }
        public int? L8600ID { get; set; }

        public decimal? WriteoffAmount
        {
            get => IncLcAmt;
            set => IncLcAmt = value;
        }
        public decimal? LoanHandlingCharge { get; set; }
        public String AdvanceDate
        {
            get => ValidityAgent.ConvertChineseDate(AdvanceDate_D);
            set => AdvanceDate_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? AdvanceDate_D
        {
            get;
            private set;
        }
        public void SetAdvanceDate(DateTime? value)
        {
            AdvanceDate_D = value;
        }
        public decimal? AdvanceAmount { get; set; }
        public L8600.ChargeType? ChargeType { get; set; }
        public decimal? ChargeAmount { get; set; }
        public decimal? SupplementFee { get; set; }
        public String PayableAccount { get; set; }
        public decimal? CheckAmount { get; set; }
        public String CheckBranch { get; set; }
        public String CheckAccount { get; set; }
        public String NPLCause1 { get; set; }
        public String NPLCause2 { get; set; }
        public String NPLCause3 { get; set; }
        public String NPLCause { get; set; }
        public int? CountNo { get; set; }
        public decimal? ActualAmount { get; set; }
        public L4600.Cause? Cause { get; set; }

    }

    public class NegoLoanRegistrationQueryViewModel : NegoDraftRegistrationViewModel
    {
        public int? L1000ID { get; set; }
        public int? ReimID { get; set; }
        [JsonIgnore]
        public int? LoanID
        {
            get => ReimID;
            set => ReimID = value;
        }
        [JsonIgnore]
        public String AccountType
        {
            get => AccountTypeField.GetAccount();
            set => AccountTypeField = value.SetAccountField();
        }
        public String[] AccountTypeField { get; set; }
        public decimal? FinalRefundRate { get; set; }
        public int? InterestCycleInMonth { get; set; }
        public int? InterestCycleInWeek { get; set; }
        public decimal? DefaultChargeRate { get; set; }
        public String Effectiveness { get; set; }
        public String InterestPeriod { get; set; }
        public String Recalc { get; set; }
        public int? RepayMonth { get; set; }
        public int? RepayWeek { get; set; }
        public int? AdjustPeriod { get; set; }
        public int? InterestRepayingDay { get; set; }
        public int? InterestMoratorium { get; set; }
        public int? Moratorium { get; set; }
        public String RepayDate
        {
            get => ValidityAgent.ConvertChineseDate(RepayDate_D);
            set => RepayDate_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? RepayDate_D
        {
            get;
            private set;
        }
        public void SetRepayDate(DateTime? value)
        {
            RepayDate_D = value;
        }
        public String AutoRepay { get; set; }
        [JsonIgnore]
        public String AutoPayableAccount
        {
            get => AutoPayableAccountField.GetAccount();
            set => AutoPayableAccountField = value.SetAccountField();
        }
        public String[] AutoPayableAccountField { get; set; }
        public String LoanMaster { get; set; }
        public String LoanPrincipal { get; set; }
        public String LoanType { get; set; }
        public decimal? LoanAmount { get; set; }
        [JsonIgnore]
        public String CreditNo
        {
            get => CreditNoField.GetAccount();
            set => CreditNoField = value.SetAccountField(4, 2, 2, 6);
        }
        public String[] CreditNoField { get; set; }
        public int? ProjectInYear { get; set; }
        public String SILFG { get; set; }
        public String SINo { get; set; }
        [JsonIgnore]
        public String CreditInsuranceAccount
        {
            get => CreditInsuranceAccountField.GetAccount();
            set => CreditInsuranceAccountField = value.SetAccountField();
        }
        public String[] CreditInsuranceAccountField { get; set; }
        public decimal? ManagementFee { get; set; }
        public String PayableAccount { get; set; }
        public decimal? InterestRateLowerBound { get; set; }
        public bool? CreditInsurance { get; set; }
    }

    public class NegoDraftRemittanceProcessViewModel : ApplicationProcessViewModel
    {
        [JsonIgnore]
        public DateTime?[] RemittanceDate_D => Remark?.Select(r => r.FromChineseDateString()).ToArray();
    }

    public class FpgCustomerDepartmentViewModel : FpgServiceQueryViewModel
    {
        public String Usage { get; set; }
        [JsonIgnore]
        public String FilePath { get; set; }

        public String DataFile
        {
            get => FilePath?.EncryptData();
            set => FilePath = value.GetEfficientString()?.DecryptData();
        }

    }

    public class FpgNegoDraftSupplementViewModel : FpgServiceQueryViewModel
    {
        private String[] _fileName = new String[3];

        public String DataContent
        {
            get => JsonConvert.SerializeObject(_fileName).EncryptData();
            set
            {
                if (value != null)
                {
                    _fileName = JsonConvert.DeserializeObject<String[]>(value.DecryptData());
                }
            }
        }

        [JsonIgnore]
        public String TXD2SB30
        {
            get => _fileName[0];
            set => _fileName[0] = value;
        }
        [JsonIgnore]
        public String TXD2SB32
        {
            get => _fileName[1];
            set => _fileName[1] = value;
        }
        [JsonIgnore]
        public String TXD2SB33
        {
            get => _fileName[2];
            set => _fileName[2] = value;
        }
    }

    public class FpgRemittanceQueryViewModel : NegoDraftQueryViewModel
    {
        public String RemittanceDateFrom
        {
            get => ValidityAgent.ConvertChineseDate(RemittanceDateFrom_D);
            set => RemittanceDateFrom_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? RemittanceDateFrom_D
        {
            get;
            private set;
        }
        public void SetRemittanceDateFrom(DateTime? value)
        {
            RemittanceDateFrom_D = value;
        }

        public String RemittanceDateTo
        {
            get => ValidityAgent.ConvertChineseDate(RemittanceDateTo_D);
            set => RemittanceDateTo_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? RemittanceDateTo_D
        {
            get;
            private set;
        }
        public void SetRemittanceDateTo(DateTime? value)
        {
            RemittanceDateTo_D = value;
        }

        public Naming.RemittanceStatusDefinition? RemittanceStatus { get; set; }
        public String RemittanceAccount { get; set; }
        public String ReserveAccount { get; set; }
        public int? LogID { get; set; }
    }

    public class FpgLcSummaryQueryViewModel : QueryViewModel
    {
        public int? Year { get; set; }
        public int? Month { get; set; }
    }

    public class InformationContactViewModel : QueryViewModel
    {
        public String[] UserName { get; set; }
        public String[] EMail { get; set; }

    }

    public class MessageBoxViewModel : QueryViewModel
    {
        public int? MsgDocID { get; set; }
        public Naming.MessageTypeDefinition? TypeID { get; set; }
    }

    public class SysOPViewModel : QueryViewModel
    {
        public DateTime? ActionDate { get; set; }
        public String FileName { get; set; }
        public bool? OperationDone { get; set; }
    }
}
