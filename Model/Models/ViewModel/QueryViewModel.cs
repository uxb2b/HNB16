using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ModelCore.Locale;
using ModelCore.DataModel;
using CommonLib.Utility;
using ModelCore.Helper;
using System.Linq.Expressions;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Http;
using ModelCore.Properties;

namespace ModelCore.Models.ViewModel
{

    public class QueryViewModel : TaskViewModel
    {
        public int? PageSize { get; set; } = AppSettings.Default.PageSize;
        public int? PageIndex { get; set; }
        public int? PageOffset { get; set; } = 0;
        public string[] SortName { get; set; }
        public int?[] SortType { get; set; }
        public bool? Paging { get; set; }
        public String KeyID { get; set; }
        public String FileDownloadToken { get; set; }
        public int?[] Sort { get; set; }
        //public DataResultMode? ResultMode { get; set; }
        public int? RecordCount { get; set; }

        [JsonIgnore]
        public String OnPageCallScript { get; set; }
        public bool? ForTest { get; set; }

        [JsonIgnore]
        public String QueryForm { get; set; }
        public bool? StartQuery { get; set; }
        [JsonIgnore]
        public bool? InitQuery { get; set; }


        [JsonIgnore]
        public String ResultView { get; set; }
        public String EncResultView
        {
            get => ResultView?.EncryptData();
            set
            {
                if (value != null)
                    ResultView = value.DecryptData();
            }
        }
        [JsonIgnore]
        public String QueryResult { get; set; }
        public String EncQueryResult
        {
            get => QueryResult?.EncryptData();
            set
            {
                if (value != null)
                    QueryResult = value.DecryptData();
            }
        }
        public bool? RowNumber { get; set; } = true;
        public bool? GroupingQuery { get; set; }

        public bool? Encrypt { get; set; }
        public String QuickSearch { get; set; }
        public Naming.FieldDisplayType? DisplayType { get; set; }
        public String GoBackToken { get; set; }

        [JsonIgnore]
        public String GoBackUrl { get; set; }
        public String[] KeyItems { get; set; }
        [JsonIgnore]
        public String Term
        {
            get ;
            set ;
        }
        [JsonIgnore]
        public String AlertMessage { get; set; }
        public String EncodedAlertMessage
        {
            get => AlertMessage != null ? Convert.ToBase64String(Encoding.Default.GetBytes(AlertMessage)) : null;
            set
            {
                if (value != null)
                {
                    AlertMessage = Encoding.Default.GetString(Convert.FromBase64String(value));
                }
            }
        }

        public String AlertTitle { get; set; }

        public String DialogID { get; set; }
        public bool? ReuseModal { get; set; }
        public Naming.DataResultMode? ResultMode { get; set; }

        [JsonIgnore] 
        public String UrlAction { get; set; }
        [JsonIgnore]
        public bool? IgnoreEmpty { get; set; }
        public String DataToSign { get; set; }
        public String DataSignature { get; set; }

        [JsonIgnore] 
        public String PartialView { get; set; }
        public String ProcessRemark { get; set; }
        public int?[] DocID { get; set; }

        [JsonIgnore]
        public String ActionTitle { get; set; }
        public QueryViewModel Duplicate()
        {
            return (QueryViewModel)this.MemberwiseClone();
        }
        public String[] ApprovalKey { get; set; }
        public String[] DenialKey { get; set; }
        public String[] Reason { get; set; }
        public String[] Others { get; set; }
        public String[] Remark { get; set; }
        public String[] PreparedACTNO { get; set; }

        [JsonIgnore]
        public String EmptyQueryResult { get; set; }
        [JsonIgnore]
        public String DeleteAction { get; set; }
        [JsonIgnore]
        public String LoadAction { get; set; }
        [JsonIgnore]
        public String EditAction { get; set; }
        [JsonIgnore]
        public String CommitAction { get; set; }
        public String EmptyKeyID { get; set; }
    }

    public class DataProcessViewModel
    {
        public String ApprovalKey { get; set; }
        public String DenialKey { get; set; }
        public String Reason { get; set; }
        public String Others { get; set; }
        public String BranchNo { get; set; }
    }

    public class PostMessageViewModel : QueryViewModel
    {
        public String PageContent { get; set; }
    }

    public class AuthorizedRoleQueryViewModel : QueryViewModel
    {
        public Naming.BranchType? UserBranchRole { get; set; }
        [JsonIgnore]
        public bool? SyncEAI { get; set; }
    }


    public class CreditApplicationQueryViewModel : DocumentaryQueryViewModel
    {
        [JsonIgnore]
        public int? Applicant { get; set; }
        public String EncApplicant 
        { 
            get => Applicant.HasValue ? Applicant.Value.EncryptKey() : null;
            set
            {
                if (value != null)
                    Applicant = value.DecryptKeyValue();
            }
        }

        [JsonIgnore]
        public int? Beneficiary
        {
            get;
            set;
        }
        public String EncBeneficiary
        {
            get => Beneficiary.HasValue ? Beneficiary.Value.EncryptKey() : null;
            set
            {
                if (value != null)
                    Beneficiary = value.DecryptKeyValue();
            }
        }

        public String DateFrom
        {
            get => ValidityAgent.ConvertChineseDate(DateFrom_D);
            set => DateFrom_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? DateFrom_D
        {
            get;
            private set;
        }
        public void SetDateFrom(DateTime? value)
        {
            DateFrom_D = value;
        }
        public String DateTo
        {
            get => ValidityAgent.ConvertChineseDate(DateTo_D);
            set => DateTo_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? DateTo_D
        {
            get;
            private set;
        }
        public void SetDateTo(DateTime? value)
        {
            DateTo_D = value;
        }
        public String BeneReceiptNo { get; set; }
        public String AppReceiptNo { get; set; }
        public bool? AtSight { get; set; }

        public bool? AtUsance
        {
            get => AtSight.HasValue ? !AtSight : (bool?)null;
            //set => AtSight = (value.HasValue ? !value : (bool?)null);
        }
        public bool? HasEntMember { get; set; }
        public String AppNo { get; set; }
        public String BranchCode { get; set; }
        public String IssuingBranch { get; set; }
        public Naming.LcQueryBranch? QueryBranch { get; set; }

        public int? AppID { get; set; }
        public Naming.ApplicationViewMode? ViewMode { get; set; }
        public int? MemberID { get; set; }
        public bool? OverTheCounter { get; set; }
        public String ActionBranch { get; set; }
        public String LcNo { get; set; }
    }

    public class CustomerOfBranchViewModel : AuthorizedRoleQueryViewModel,IInterestRateQueryViewModel
    {
        public int? CompanyID { get; set; }
        public String BankCode { get; set; }
        public bool? QueryForAllowing { get; set; }

        public String PayableAccount
        {
            get => PayableAccountField.GetAccount();
            set => PayableAccountField = value.SetAccountField();
        }
        [JsonIgnore]
        public String[] PayableAccountField { get; set; }

        public String Addr { get; set; }
        public String Phone { get; set; }
        public String ContactEmail { get; set; }
        public String Undertaker { get; set; }
        public String CompanyName { get; set; }
        public DateTime? RegSDay { get; set; }
        public int? CurrentVersion { get; set; }
        public int? CurrentLevel { get; set; }
        public int? PostponeMonths { get; set; }
        public int? UsancelimitedDays { get; set; }
        public String ReceiptNo { get; set; }
        public String ReimAccount
        {
            get => ReimAccountField.GetAccount();
            set => ReimAccountField = value.SetAccountField(4, 2, 5, 1, 2);
        }
        [JsonIgnore]
        public String[] ReimAccountField { get; set; }
        public decimal? InterestRateLowerBound { get; set; }
        public String InterestRateLowerBoundExpiration
        {
            get => ValidityAgent.ConvertChineseDate(InterestRateLowerBoundExpiration_D);
            set => InterestRateLowerBoundExpiration_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? InterestRateLowerBoundExpiration_D
        {
            get;
            private set;
        }
        public void SetInterestRateLowerBoundExpiration(DateTime? value)
        {
            InterestRateLowerBoundExpiration_D = value;
        }

        public bool? UseNegoLoan { get; set; }
        public bool? UseCreditInsurance { get; set; }
        public String LoanCreditNo { get; set; }
        public String InsuranceCreditNo { get; set; }
        public int? LoanPeriod { get; set; }
        public CustomerOfBranchExtension.StartLoanBy? StartLoanAccordingTo { get; set; }
        public String ChristeningDueDate
        {
            get => ValidityAgent.ConvertChineseDate(ChristeningDueDate_D);
            set => ChristeningDueDate_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? ChristeningDueDate_D
        {
            get;
            private set;
        }
        public void SetChristeningDueDate(DateTime? value)
        {
            ChristeningDueDate_D = value;
        }
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
        public decimal? InsuredCreditAmount { get; set; }
        public decimal? CreditPercentage { get; set; }
        public String CreditType { get; set; }
        public bool? AuthorizeToRepay { get; set; }
        public CustomerOfBranchExtension.InterestRepayment? InterestRepayingType { get; set; }
        public int? InterestRepayingDay { get; set; }
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
        public decimal? InterestRateOfBank { get; set; }

        [JsonIgnore]
        public decimal? AdjustInterestRate
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

        public String ReserveAccount
        {
            get;
            set;
        }

        public String ReserveAccountName
        {
            get;
            set;
        }

        public String ReserveAccountReceiptNo
        {
            get;
            set;
        }

        public Naming.BeneficiaryStatus?[] LevelID { get; set; }
        public Naming.DraftType? DraftType { get; set; }

    }

    public class BranchDataViewModel : CustomerOfBranchViewModel
    {
        public String BranchName { get; set; }
    }

    public class BeneficiaryDataViewModel : CustomerOfBranchViewModel
    {
        public int? BeneID { get; set; }
        [JsonIgnore]
        public int? ApplicantID
        {
            get => CompanyID;
            set => CompanyID = value;
        }

        public int? Status
        {
            get => CurrentLevel;
            set => CurrentLevel = value;
        }

        public String[] AccountInfo
        {
            get;
            set;
        }

        public bool? AppCountersign { get; set; }
        public String Department { get; set; }
        [JsonIgnore]
        public int? Applicant
        {
            get => ApplicantID;
            set => ApplicantID = value;
        }
        [JsonIgnore]
        public int? Beneficiary
        {
            get => BeneID;
            set => BeneID = value;
        }

    }

    public class BeneficiaryStatusDataViewModel : BeneficiaryDataViewModel
    {
        public decimal? OverRatio { get; set; }
    }

    public class OrganizationSettingsViewModel : CustomerOfBranchViewModel
    {
        public int? SettingID { get; set; }
        public decimal? StepCharge { get; set; }
        public decimal? HandlingCharge { get; set; }

    }

    public class QuickSearchViewModel : QueryViewModel
    {
        public String FieldName { get; set; }
        public String PlaceHolder { get; set; }
        public bool? SearchAll { get; set; }
        public Object Filter { get; set; }
    }

    public class JobListViewModel : QueryViewModel
    {
        public int? JobID { get; set; }
    }

    public class R3801QueryViewModel : QueryViewModel
    {
        public String LogDate
        {
            get => ValidityAgent.ConvertChineseDate(LogDate_D);
            set => LogDate_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? LogDate_D
        {
            get;
            private set;
        }
        public void SetLogDate(DateTime? value)
        {
            LogDate_D = value;
        }

    }

    public class TransactionReportQueryViewModel : CreditApplicationQueryViewModel
    {
        public Naming.BranchType? ReportType { get; set; }
    }
    public class FpgServiceQueryViewModel : CreditApplicationQueryViewModel
    {
        public Naming.FpgServiceType? ServiceType { get; set; }
        public Naming.DraftType? DraftType { get; set; }
    }

    public class SystemCertificateViewModel : CreditApplicationQueryViewModel
    {
        public String PIN { get; set; }
        public String CertPIN { get; set; }
        public String ReferenceID { get; set; }
        public IFormFile UploadPFX { get; set; }
        public IFormFile SignedXml { get; set; }
        public new IFormFile DataToSign { get; set; }
    }

    public class BrowseLogViewModel : QueryViewModel
    {
        public QueryViewModel ViewModel { get; set; }
        public String RawUrl { get; set; }
    }

    public class AuthorizationQueryViewModel : QueryViewModel
    {
        public string UID { get; set; }

        public string UUID { get; set; }
    }

    public class LogDataQueryViewModel : QueryViewModel
    {
        public String LogDate
        {
            get => ValidityAgent.ConvertChineseDate(LogDate_D);
            set => LogDate_D = value.FromChineseDateString();
        }
        [JsonIgnore]
        public DateTime? LogDate_D
        {
            get;
            private set;
        }
        public void SetLogDate(DateTime? value)
        {
            LogDate_D = value;
        }

        public String LogPattern { get; set; }
    }

    public class DBQueryViewModel : QueryViewModel
    {
        public String CommandText { get; set; }
    }

    

    public static class QueryViewModelExtensions
    {
        public static T DeserializeObject<T>(this T viewModel)
            where T : QueryViewModel
        {
            return JsonConvert.DeserializeObject<T>(viewModel.KeyID.DecryptData());
        }
        public static String SerializeObject(this QueryViewModel viewModel)
        {
            return viewModel.JsonStringify().EncryptData();
        }
    }
}