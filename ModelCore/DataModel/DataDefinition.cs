using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using CommonLib.Core.DataWork;
using EAI.Service.Transaction;
using ModelCore.Locale;
using CommonLib.Utility;

namespace ModelCore.DataModel
{

    public static partial class DataDefinitionExtensions
    {
        public static String LcType(this CreditApplicationDocumentary item)
        {
            return item.AtSight ? "即期" : "遠期";
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
                    default:
                        return null;
                }
            }

            return null;
        }

        public static String MinorAccounting(this CreditApplicationDocumentary item)
        {
            return item.AtSight ? "600" : "700";
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
        public String CurrentNotifyingBranch => this.NotifyingBank ?? this.Application?.AdvisingBankCode;
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
                return LetterOfCreditVersion.Where(l => l.AmendingLcInformationID == this.AmendingLcApplicationID).First();
            }
        }
    }

    public partial class AmendingLcApplication
    {
        public bool IsAmending =>
            Documentary.CurrentLevel != (int)Naming.DocumentLevel.文件預覽;
    }
}
