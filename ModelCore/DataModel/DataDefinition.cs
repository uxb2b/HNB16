using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using CommonLib.DataAccess;
using EAI.Service.Transaction;
using ModelCore.Locale;
using CommonLib.Utility;

namespace ModelCore.DataModel
{
    public partial class PaymentNotification
    {
        public PaymentNotification Clone()
        {
            return (PaymentNotification)this.MemberwiseClone();
        }

        public enum TransferenceType
        {
            轉帳 = 1,
            連動轉 = 3,
            CRC分行收取費用 = 4,
        }

        public static readonly Dictionary<TransferenceType, String> TransferenceTypeValues =
            new Dictionary<TransferenceType, string>()
            {
                { TransferenceType.轉帳,"1.轉帳" },
                { TransferenceType.連動轉,"3.連動轉" },
                { TransferenceType.CRC分行收取費用,"4.CRC-分行收取費用" },
            };

        public enum AllocationType
        {
            連動入戶 = 1,
            同存支票 = 2,
            匯出匯款 = 3,
            本行支票 = 4,
            轉帳 = 5,
            電子押匯待處理款 = 6,
            電子押匯待匯出款 = 7,
        }

        public static readonly Dictionary<AllocationType, String> AllocationTypeValues =
            new Dictionary<AllocationType, string>()
            {
                {AllocationType.連動入戶, "1.連動入戶"},
                {AllocationType.同存支票, "2.同存支票"},
                {AllocationType.匯出匯款, "3.匯出匯款"},
                {AllocationType.本行支票, "4.本行支票"},
                {AllocationType.轉帳, "5.轉帳"},
                {AllocationType.電子押匯待處理款,"6.電子押匯待處理款" },
                {AllocationType.電子押匯待匯出款,"7.電子押匯待匯出款" },
            };

        [Flags]
        public enum LoanType
        {
            ㄧ般新台幣貸放 = 1,
            國內信用狀開狀 = 2,
            押匯 = 4,
            承兌 = 8,
            改貸 = 16,
            國內信用狀修改 = 32,
            保證_承兌 = 64,
            利率_期限異動 = 128,
            透支 = 256,
            憑卡融資 = 512,
            個人理財 = 1024,
            消費者貸款 = 2048,
            其他 = 4096,
        }

        [Flags]
        public enum SecurityStatus
        {
            相關擔保物權均已設定訖 = 1,
            相關約定書_保證書及約據均已徵提訖 = 2,
            存摺存款或定存單徵提設質訖 = 4,
            控管票據金額徵提訖 = 8,
            還款本票金額徵提訖 = 16,
            自動繳息 = 32,
            自動償還 = 64,
            已徵提授權扣款授權書 = 128,
            約定扣款帳號 = 256,
            已填寫信保基金審核單_通知單 = 512,
            其他 = 1024,
            活期存款徵提設質訖 = 2048,
        }

        [Flags]
        public enum CommissionStatus
        {
            保證手續費 = 1,
            國內信用狀開狀_修改手續費 = 2,
            承兌保證費 = 4,
            保證金 = 8,
            開辦費 = 16,
            應收帳款墊款息 = 32,
            移送基金保證手續費 = 64,
            匯存基金指定帳戶 = 128,
            帳戶管理費 = 256,
            其他費用 = 512,
        }
        public static String[] __LoanType =
            {
                "ㄧ般新台幣貸放",
                "國內信用狀開狀",
                "押匯",
                "承兌",
                "改貸",
                "國內信用狀修改",
                "保證、承兌",
                "利率、期限異動",
                "透支",
                "easy自由貸",
                "個人理財",
                "消費者貸款",
                "其他："
            };

        public static String[] __SecurityStatus =
        {
            "相關擔保物權均已設定訖。",
            "相關約定書、保證書及約據均已徵提訖。",
            "存摺存款或定存單徵提設質訖。",
            "控管票據金額徵提訖。",
            "還款本票金額徵提訖。",
            "自動繳息  ",
            "自動償還。",
            "已徵提授權扣款授權書。",
            "約定扣款帳號：",
            "已填寫信保基金審核單、通知單。",
            "其他:",
            "活期存款徵提設質訖",
        };

        public static String[] __CommissionStatus =
        {
            "保證手續費",
            "國內信用狀開狀/修改手續費",
            "承兌手續費",
            "保證金",
            "開辦費",
            "應收帳款墊款息",
            "移送基金保證手續費",
            "匯存基金指定帳戶",
            "帳戶管理費：",
            "其他費用：",
        };
    }

    public static partial class DataDefinitionExtensions
    {
        public static String LcType(this CreditApplicationDocumentary item)
        {
            return item.見票即付 ? "即期" : "遠期";
        }

        public static DocumentaryLevel GetCurrentDocumentaryLevel(this Documentary item)
        {
            return item.DocumentaryLevel.OrderByDescending(d => d.LevelDate).FirstOrDefault();
        }

        public static DocumentaryDenial GetCurrentDocumentaryDenial(this Documentary item)
        {
            var level = item.GetCurrentDocumentaryLevel();
            if (level != null)
            {
                return item.DocumentaryDenial.Where(d => d.DenialDate == level.LevelDate).FirstOrDefault();
            }
            return null;
        }

        public static String GetReason(this Documentary item)
        {
            var denial = item.GetCurrentDocumentaryDenial();
            if (denial != null)
            {
                return denial.Reason;
            }
            return null;
        }

        public static String GetCurrentDenialApplicationReason(this CreditApplicationDocumentary item)
        {
            return item.Documentary.GetCurrentDenialReason();

        }

        public static String GetCurrentDenialReason(this Documentary item)
        {
            if (item.CurrentLevel == (int)Naming.DocumentLevel.銀行已拒絕
                || item.CurrentLevel == (int)Naming.DocumentLevel.已退回_CRC主管退回
                || item.CurrentLevel == (int)Naming.DocumentLevel.已退回_CRC退回
                || item.CurrentLevel == (int)Naming.DocumentLevel.已退回_主管退回)
            {
                var denial = item.GetCurrentDocumentaryDenial();
                if (denial != null)
                {
                    return denial.Reason;
                }
            }
            return null;
        }

        public static String InboxMessageContent(this BankInbox item)
        {
            var docItem = item.Documentary;

            if (docItem != null)
            {

                switch ((Naming.DocumentTypeDefinition?)docItem.DocType)
                {
                    case Naming.DocumentTypeDefinition.押匯申請書:
                        return String.Format("押匯申請書(匯票號碼):{0}", docItem.NegoDraft.DraftNo);
                    case Naming.DocumentTypeDefinition.信用狀註銷申請書:
                        return String.Format("信用狀註銷申請書:{0}", docItem.CreditCancellation.註銷申請號碼);
                    case Naming.DocumentTypeDefinition.修狀申請書:
                        return String.Format("改狀申請號碼:{0}", docItem.AmendingLcApplication.AmendmentNo);
                    case Naming.DocumentTypeDefinition.開狀申請書:
                        return String.Format("開狀申請號碼:{0}", docItem.CreditApplicationDocumentary.ApplicationNo);
                    case Naming.DocumentTypeDefinition.還款改貸申請書:
                        return String.Format("還款改貸申請號碼:{0}", docItem.Reimbursement.ReimbursementNo);
                    default:
                        return item.MsgContent;
                }
            }

            return item.MsgContent;
        }

        public static String InboxMessageContent(this CustomerInbox item)
        {
            var docItem = item.Documentary;

            if (docItem != null)
            {
                switch ((Naming.DocumentTypeDefinition?)docItem.DocType)
                {
                    case Naming.DocumentTypeDefinition.押匯申請書:
                        return String.Format("押匯申請書(匯票號碼):{0}", docItem.NegoDraft.DraftNo);
                    case Naming.DocumentTypeDefinition.信用狀註銷申請書:
                        return String.Format("信用狀註銷申請書:{0}", docItem.CreditCancellation.註銷申請號碼);
                    case Naming.DocumentTypeDefinition.修狀申請書:
                        return String.Format("改狀申請號碼:{0}", docItem.AmendingLcApplication.AmendmentNo);
                    case Naming.DocumentTypeDefinition.開狀申請書:
                        return String.Format("開狀申請號碼:{0}", docItem.CreditApplicationDocumentary.ApplicationNo);
                    case Naming.DocumentTypeDefinition.還款改貸申請書:
                        return String.Format("還款改貸申請號碼:{0}", docItem.Reimbursement.ReimbursementNo);
                    default:
                        return null;
                }
            }

            return null;
        }

        public static String MinorAccounting(this CreditApplicationDocumentary item)
        {
            return item.見票即付 ? "600" : "700";
        }

        public static bool ReadyToPrintOut(this LetterOfCredit lc)
        {
            var item = lc.CreditApplicationDocumentary;

            return lc != null && item != null
               && item.Documentary.LcPrintNotice == null
               && (item.BeneficiaryData.DraftType == (int)Naming.BeneficiaryDraftType.非電子押匯
                   || item.BeneficiaryData.DraftType == (int)Naming.BeneficiaryDraftType.電子押匯_電子申請
                   || item.BeneficiaryData.DraftType == (int)Naming.BeneficiaryDraftType.電子押匯_台塑集團);
        }

        public static PaymentNotification.AllocationType? AllocationType(this PaymentNotification item)
        {
            return item.撥款方式.ToAllocationType();
        }

        public static PaymentNotification.AllocationType? ToAllocationType(this String item)
        {
            return !String.IsNullOrEmpty(item) ? (PaymentNotification.AllocationType)int.Parse(item.Substring(0, 1)) : (PaymentNotification.AllocationType?)null;
        }


        public static PaymentNotification.TransferenceType? TransferenceType(this PaymentNotification item)
        {
            return item.現轉別.ToTransferenceType();
        }

        public static PaymentNotification.TransferenceType? ToTransferenceType(this String item)
        {
            return !String.IsNullOrEmpty(item) ? (PaymentNotification.TransferenceType)int.Parse(item.Substring(0, 1)) : (PaymentNotification.TransferenceType?)null;
        }

        public static String ValueString(this PaymentNotification.AllocationType? allocationType)
        {
            return allocationType.HasValue ? PaymentNotification.AllocationTypeValues[allocationType.Value] : null;
        }

        public static String ValueString(this PaymentNotification.TransferenceType? data)
        {
            return data.HasValue ? PaymentNotification.TransferenceTypeValues[data.Value] : null;
        }

        public static L8600.ChargeType? ToChargeType(this String item)
        {
            return !String.IsNullOrEmpty(item) ? (L8600.ChargeType)int.Parse(item.Substring(0, 1)) : (L8600.ChargeType?)null;
        }

        public static String ValueString(this L8600.ChargeType? data)
        {
            return data.HasValue ? L8600.ChargeTypeValues[data.Value] : null;
        }

        public static L4600.Cause? ToCause(this String item)
        {
            return !String.IsNullOrEmpty(item) ? (L4600.Cause)int.Parse(item.Substring(0, 2)) : (L4600.Cause?)null;
        }

        public static String ValueString(this L4600.Cause? data)
        {
            return data.HasValue ? L4600.CauseValues[data.Value] : null;
        }

        public static readonly Dictionary<String, String> CreditTypeValues =
            new Dictionary<String, string>()
            {
                {"01","01一般貸款"},
                {"02","02商業本票保證"},
                {"03","03外銷貸款"},
                {"04","04購料週轉融資"},
                {"05","05促進產業研究發展貸款"},
                {"06","06履約保證"},
                {"07","07政策性貸款"},
                {"08","08小額簡便貸款"},
                {"09","09自創品牌貸款"},
                {"10","10青年創業貸款"},
                {"11","11建構研發環境優惠貸款"},
                {"12","12知識經濟企業融資"},
                {"13","13國內民營企業國際專利權訴訟貸款"},
                {"14","14微型企業創業貸款"},
                {"15","15非中小企業之數位內容產業及文化創意產業優惠貸款"},
                {"16","16協助中小企業赴有邦交國家投資融資"},
                {"17","17火金姑（相對保證）專案"},
                {"18","18中小企業災害復舊專案"},
                {"19","19創業鳳凰婦女小額貸款"},
                {"20","20更生事業甘霖專案貸款"},
                {"21","21離島永續發展貸款"},
                {"22","22協助中堅企業專案貸款"},
                {"23","23微型創業鳳凰貸款"},
                {"97","97直接保證"},
                {"98","98批次信用保證融資"},
                {"99","99其他"},
            };

        public static String CreditTypeValue(this String creditType)
        {
            if (creditType != null && CreditTypeValues.ContainsKey(creditType))
            {
                return CreditTypeValues[creditType];
            }
            return null;
        }
    }

    public partial class CustomerOfBranchExtension
    {
        public enum StartLoanBy
        {
            押匯日 = 1,
            開狀日 = 2,
        }

        public enum InterestRepayment
        {
            按月繳息 = 1,
            到期結清 = 2
        }
    }

    public partial class L8600
    {
        public enum ChargeType
        {
            正常 = 1,
            自行輸入 = 2,
            不收 = 3,
        }

        public static readonly Dictionary<ChargeType, String> ChargeTypeValues =
            new Dictionary<ChargeType, string>()
            {
                { ChargeType.正常,"1.正常" },
                { ChargeType.自行輸入,"2.自行輸入" },
                { ChargeType.不收,"3.不收" },
            };
    }

    public partial class L4600
    {
        public enum Cause
        {
            還款 = 1,
            改貸 = 2,
        }

        public static readonly Dictionary<Cause, String> CauseValues =
            new Dictionary<Cause, string>()
            {
                { Cause.還款,"01.還款" },
                { Cause.改貸,"02.改貸" },
            };
    }

    public partial class NegoLoanRepayment
    {
        public EAI.Service.Transaction.LR006_Rs.IFX LoadLR006_Rs(out EAI.Service.Transaction.LR006_Rs.IFX insured)
        {
            EAI.Service.Transaction.LR006_Rs.IFX result = null;
            insured = null;
            if (this.RepaymentDate.HasValue)
            {
                String logFile = Path.Combine(CommonLib.Core.Utility.Logger.LogPath.GetDateStylePath(this.RepaymentDate.Value), $"LR006-{this.NegoLoan?.ACTNO}.xml");
                if (File.Exists(logFile))
                {
                    result = Txn_LR006.LoadRsFile(logFile);
                }

                if (this.NegoLoan?.CreditInsuranceLoan != null)
                {
                    logFile = Path.Combine(CommonLib.Core.Utility.Logger.LogPath.GetDateStylePath(this.RepaymentDate.Value), $"LR006-{this.NegoLoan?.CreditInsuranceLoan?.ACTNO}.xml");
                    if (File.Exists(logFile))
                    {
                        insured = Txn_LR006.LoadRsFile(logFile);
                    }
                }
            }
            return result;
        }

        public void ClearEAICache()
        {
            ///clear EAI L0104 cache
            ///
            NegoLoan loan = this.NegoLoan;
            String logFile = Path.Combine(CommonLib.Core.Utility.Logger.LogDailyPath, $"L0104-{loan.ACTNO}.xml");
            if (System.IO.File.Exists(logFile))
            {
                System.IO.File.Delete(logFile);
            }

            logFile = Path.Combine(CommonLib.Core.Utility.Logger.LogDailyPath, $"LR006-{loan.ACTNO}.xml");
            if (System.IO.File.Exists(logFile))
            {
                System.IO.File.Delete(logFile);
            }
            if (loan.CreditInsuranceLoan != null)
            {
                logFile = Path.Combine(CommonLib.Core.Utility.Logger.LogDailyPath, $"L0104-{loan.CreditInsuranceLoan.ACTNO}.xml");
                if (System.IO.File.Exists(logFile))
                {
                    System.IO.File.Delete(logFile);
                }

                logFile = Path.Combine(CommonLib.Core.Utility.Logger.LogDailyPath, $"LR006-{loan.CreditInsuranceLoan.ACTNO}.xml");
                if (System.IO.File.Exists(logFile))
                {
                    System.IO.File.Delete(logFile);
                }
            }
        }
    }

    public partial class NegoLoan
    {
        public bool CheckLoanBalance(GenericManager<LcEntityDataContext> models)
        {
            bool result = false;
            String logFile = Path.Combine(CommonLib.Core.Utility.Logger.LogDailyPath, $"L0104-{this.ACTNO}.xml");
            //if (!File.Exists(logFile))
            {
                EAI.Service.Transaction.Txn_L0104 txn = new EAI.Service.Transaction.Txn_L0104();
                txn.Rq.EAIBody.MsgRq.SvcRq.ACTNO = this.ACTNO;
                txn.Rq.EAIBody.MsgRq.SvcRq.INQSDAY = "2004-01-01";
                txn.Rq.EAIBody.MsgRq.SvcRq.INQEDAY = String.Format("{0:yyyy-MM-dd}", DateTime.Today);

                if (txn.Commit())
                {
                    var rs = txn.Rs;
                    rs.ConvertToXml().Save(logFile);
                    this.ACTBAL = this.BalanceAmount = decimal.Parse(rs.EAIBody.MsgRs.SvcRs.ACTBAL);
                    models.SubmitChanges();

                    if (this.CreditInsuranceLoan != null)
                    {
                        this.BalanceAmount += this.CreditInsuranceLoan.CheckCreditLoanBalance(models);
                        models.SubmitChanges();
                    }
                    result = true;
                }
            }

            return result;
        }
    }

    public partial class CreditInsuranceLoan
    {
        public decimal CheckCreditLoanBalance(GenericManager<LcEntityDataContext> models)
        {
            String logFile = Path.Combine(CommonLib.Core.Utility.Logger.LogDailyPath, $"L0104-{this.ACTNO}.xml");
            EAI.Service.Transaction.L0104_Rs.IFX rs = null;
            //if (File.Exists(logFile))
            //{
            //    System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            //    doc.Load(logFile);
            //    rs = doc.ConvertTo<EAI.Service.Transaction.L0104_Rs.IFX>();
            //}
            //else
            {
                EAI.Service.Transaction.Txn_L0104 txn = new EAI.Service.Transaction.Txn_L0104();
                txn.Rq.EAIBody.MsgRq.SvcRq.ACTNO = this.ACTNO;
                txn.Rq.EAIBody.MsgRq.SvcRq.INQSDAY = "2004-01-01";
                txn.Rq.EAIBody.MsgRq.SvcRq.INQEDAY = String.Format("{0:yyyy-MM-dd}", DateTime.Today);

                if (txn.Commit())
                {
                    rs = txn.Rs;
                    rs.ConvertToXml().Save(logFile);
                }
            }

            if (rs != null)
            {
                this.ACTBAL = decimal.Parse(rs.EAIBody.MsgRs.SvcRs.ACTBAL);
                return this.ACTBAL.Value;
            }

            return 0;
        }

    }

    public partial class OrganizationStatus
    {
        public enum CustomerStatusType
        {
            Inviting = 1,
            Invited = 2,
        }
    }

    public partial class LetterOfCredit
    {
        public String CurrentNotifyingBranch => this.NotifyingBank ?? this.CreditApplicationDocumentary?.通知行;
        public LetterOfCreditVersion CurrentVersion
        {
            get
            {
                return LetterOfCreditVersion.OrderByDescending(l => l.VersionID).FirstOrDefault();
            }
        }
    }

    public partial class BeneficiaryServiceGroup
    {
        public enum ServiceDefinition
        {
            Fpg = 1,
            Chimei = 2,
            UXCDS = 3,
        }
    }

    public partial class GroupDepartment
    {
        public const String DefaultDepartID = "0000";
    }

    public partial class NegoHost
    {
        public enum HostStatusType
        {
            Disabled = 0,
            Enabled = 1,
        }
    }

    public partial class AmendingLcInformation
    {
        public LetterOfCreditVersion CurrentLc
        {
            get
            {
                return LetterOfCreditVersion.Where(l => l.AmendingID == this.AmendingID).First();
            }
        }
    }

    public partial class AmendingLcApplication
    {
        public bool IsAmending =>
            Documentary.CurrentLevel != (int)Naming.DocumentLevel.文件預覽;
    }
}
