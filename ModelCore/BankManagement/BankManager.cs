using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml;


using CommonLib.DataAccess;


using EAI.Service.Transaction;
using ModelCore.DataModel;
using ModelCore.Helper;
using ModelCore.Locale;
using ModelCore.NegoManagement;
using ModelCore.Properties;
using ModelCore.UserManagement;
using CommonLib.Utility;
using ModelCore.LcManagement;
using System.Globalization;
using ModelCore.Schema;

namespace ModelCore.BankManagement
{
    /// <summary>
    /// BankManager 的摘要描述。
    /// </summary>
    public class BankManager : LcEntityManager<CreditApplicationDocumentary>
	{
		public BankManager() : base()
		{
			//
			// TODO: 在此加入建構函式的程式碼
			//
		}

        public BankManager(GenericManager<LcEntityDataContext> mgr) : base(mgr) { }

        public UserManagement.UserProfile UserProfile { get; set; }

        private const int MAX_GOODS_LENGTH_PER_LINE = 38;
        private const int MAX_GOODS_LENGTH_PER_LINE1 = 60;

        public BankManager(UserProfile userProfile)
            : this()
        {
            //
            // TODO: 在此加入建構函式的程式碼
            //
            UserProfile = userProfile;
        }

        public static void DoLcR3801(int? appID)
        {
            ThreadPool.QueueUserWorkItem(state => 
            {
                using (BankManager mgr = new BankManager())
                {
                    var item = mgr.EntityList.Where(c => c.AppID == appID).FirstOrDefault();
                    if (item!=null)
                    {
                        Txn_R3801 txn = new EAI.Service.Transaction.Txn_R3801();
                        txn.REMDAY = DateTime.Now.ToString("yyyy-MM-dd");
                        txn.RCVBK = "009" + item.開狀行;
                        txn.REMTYPE = "04";
                        txn.MSG1 = "電子信用狀開狀待受理通知";
                        txn.MSG2 = "申請人統編:" + item.CustomerOfBranch.Organization.ReceiptNo + ";"
                                 + "申請書編號:" + item.ApplicationNo;
                        //txn.MSG3 = item.Documentary.CustomerCreditAlert.Count > 0
                        //    ? String.Concat("該戶有信用貶落指標：",
                        //            String.Join("、",item.Documentary.CustomerCreditAlert.Select(a=>a.AlertCode + " " + a.Description)),
                        //            "。請先行確認。") 
                        //    : "";
                        txn.MSG3 = item.Documentary.CustomerCreditAlert.Count > 0
                            ? String.Concat("該戶有信用貶落指標：",
                                    String.Join("、", item.Documentary.CustomerCreditAlert.Select(a => a.AlertCode)))
                            : "";

                        txn.RECVER = "放款作業專員";
                        txn.SENDER = "總行";
                        txn.Commit();
                    }
                }

            });
        }


        public static void DoCreditCreatedR3801(int? appID)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                using (BankManager mgr = new BankManager())
                {
                    var item = mgr.EntityList.Where(c => c.AppID == appID).FirstOrDefault();
                    if (item!=null)
                    {
                        if (item.通知行!=item.開狀行)
                        {
                            Txn_R3801 txn = new EAI.Service.Transaction.Txn_R3801();
                            txn.REMDAY = DateTime.Now.ToString("yyyy-MM-dd");
                            txn.RCVBK = "009" + item.通知行;
                            txn.REMTYPE = "04";
                            //txn.MSG1 = "電子信用狀開狀已開立通知";
                            txn.MSG1 = "電子信用狀開狀已開立通知:申請人統編:" + item.CustomerOfBranch.Organization.ReceiptNo;
                            //txn.MSG2 = "申請人統編:" + item.CustomerOfBranch.Organization.ReceiptNo + "於" + DateTime.Now.ToString("MM/dd hh:mm") + "從" + item.CustomerOfBranch.BankData.BranchName + "開立";
                            txn.MSG2 = DateTime.Now.ToString("MM/dd HH:mm") + item.CustomerOfBranch.BankData.BranchName + "開立受益人統編:" + item.BeneficiaryData.Organization.ReceiptNo + "之信用狀";
                            //txn.MSG3 = "受益人統編:" + item.BeneficiaryData.Organization.ReceiptNo + "之信用狀，請受理並通知受益人。"
                            //        + (item.Documentary.CustomerCreditAlert.Count > 0
                            //            ? String.Concat("該戶有信用貶落指標：",
                            //                    String.Join("、", item.Documentary.CustomerCreditAlert.Select(a => a.AlertCode + " " + a.Description)),
                            //                    "。請先行確認。")
                            //            : "");
                            //txn.MSG3 = "受益人統編:" + item.BeneficiaryData.Organization.ReceiptNo + "之信用狀，請受理並通知受益人。"
                            txn.MSG3 = "請受理通知受益人。"
                                    + (item.Documentary.CustomerCreditAlert.Count > 0
                                        ? String.Concat("信用貶落：",
                                                String.Join("、", item.Documentary.CustomerCreditAlert.Select(a => a.AlertCode)))
                                        : "");
                            txn.RECVER = "授信支援主管及放款作業專員";
                            txn.SENDER = "總行";
                            txn.Commit();
                        }
                    }
                }
            });


        }
        public static void DoDraftR3801(int? draftID)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                using (LcEntityManager<NegoDraft> mgr = new LcEntityManager<NegoDraft>())
                {
                    var item = mgr.EntityList.Where(d => d.DraftID == draftID).FirstOrDefault();
                    if (item!=null)
                    {
                        if(item.LcID.HasValue)
                        {
                            Txn_R3801 txn = new EAI.Service.Transaction.Txn_R3801();
                            txn.REMDAY = DateTime.Now.ToString("yyyy-MM-dd");
                            txn.REMTYPE = "04";
                            if (item.Documentary.CurrentLevel == (int)Naming.DocumentLevel.已開立)
                            {
                                txn.RCVBK = "009" + item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.開狀行;
                                txn.MSG1 = "電子押匯已完成"
                                         + string.Format("({0}{1:MMdd})",new TaiwanCalendar().GetYear(DateTime.Now), DateTime.Now)
                                         +"通知";
                                txn.MSG2 = "申請人統編:" + item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo + ";"
                                         + "信用狀號碼:" + item.LetterOfCreditVersion.LetterOfCredit.LcNo + ";";

                                txn.MSG3 = "匯票號碼:" + item.DraftNo + ";"
                                         + "押匯金額:" + item.Amount.ToString() + ";";
                            }
                            else
                            {
                                txn.RCVBK = "009" + item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.通知行;
                                txn.MSG1 = "電子信用狀押匯待受理通知";
                                if (item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.Documentary.CustomerCreditAlert.Count > 0)
                                {
                                    txn.MSG2 = String.Concat("信用狀號碼:", item.LetterOfCreditVersion.LetterOfCredit.LcNo, ";",
                                                    "匯票號碼:", item.DraftNo, ";");

                                    txn.MSG3 = String.Concat("該戶有信用貶落指標：",
                                                    String.Join("、", item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.Documentary.CustomerCreditAlert.Select(a => a.AlertCode)),
                                                    "。請至內網查詢確認。");
                                }
                                else
                                {
                                    txn.MSG2 = String.Concat("申請人統編:", item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo, ";",
                                                    "信用狀號碼:", item.LetterOfCreditVersion.LetterOfCredit.LcNo, ";");
                                    txn.MSG3 = String.Concat("匯票號碼:", item.DraftNo, ";",
                                                    "押匯金額:", item.Amount.ToString(), ";");

                                }
                            }
                            txn.RECVER = "放款作業專員";
                            txn.SENDER = "總行";
                            txn.Commit();

                        }
                        else
                        {
                            Txn_R3801 txn = new EAI.Service.Transaction.Txn_R3801();
                            txn.REMDAY = DateTime.Now.ToString("yyyy-MM-dd");
                            txn.REMTYPE = "04";
                            if (item.Documentary.CurrentLevel == (int)Naming.DocumentLevel.已開立)
                            {
                                txn.RCVBK = "009" + item.NegoLC.IssuingBank;
                                txn.MSG1 = "電子押匯已完成"
                                         + string.Format("({0}{1:MMdd})", new TaiwanCalendar().GetYear(DateTime.Now), DateTime.Now)
                                         + "通知";
                            }
                            else
                            {
                                txn.RCVBK = "009" + item.NegoLC.AdvisingBank;
                                txn.MSG1 = "電子押匯待受理通知";
                            }
                            txn.MSG2 = "申請人統編:" + item.NegoLC.CustomerOfBranch.Organization.ReceiptNo + ";"
                                     + "信用狀號碼:" + item.NegoLC.LCNo + ";";

                            txn.MSG3 = "匯票號碼:" + item.DraftNo + ";"
                                     + "押匯金額:" + item.Amount.ToString() + ";";

                            txn.RECVER = "放款作業專員";
                            txn.SENDER = "總行";
                            txn.Commit();

                        }
                    }
                }
            });
        }

        public static void DoAmendR3801(int? amendingID)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                using (LcAmendmentManager mgr = new LcAmendmentManager())
                {
                    var item = mgr.EntityList.Where(a => a.AmendingID == amendingID).FirstOrDefault();

                    if (item != null)
                    {
                        Txn_R3801 txn = new EAI.Service.Transaction.Txn_R3801();
                        txn.REMDAY = DateTime.Now.ToString("yyyy-MM-dd");
                        txn.RCVBK = "009" + item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.開狀行;
                        txn.REMTYPE = "04";
                        txn.MSG1 = "電子信用狀修狀申請已開立通知";
                        txn.MSG2 = "申請人統編:" + item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo + ";"
                                 + "修狀申請書號碼:" + item.AmendmentNo + ";";
                        //txn.MSG3 = item.Documentary.CustomerCreditAlert.Count > 0
                        //    ? String.Concat("該戶有信用貶落指標：",
                        //            String.Join("、", item.Documentary.CustomerCreditAlert.Select(a => a.AlertCode + " " + a.Description)),
                        //            "。請先行確認。")
                        //    : "";
                        txn.MSG3 = item.Documentary.CustomerCreditAlert.Count > 0
                            ? String.Concat("信用貶落：",
                                    String.Join("、", item.Documentary.CustomerCreditAlert.Select(a => a.AlertCode)))
                            : "";
                        txn.RECVER = "放款作業專員";
                        txn.SENDER = "總行";
                        txn.Commit();

                        if (item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.開狀行 != item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.通知行)
                        {
                            txn.RCVBK = $"009{item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.通知行}";
                            txn.Commit();
                        }
                    }

                }
            });
        }

        public static void DoApplyAmendingR3801(int? amendingID)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                using (LcAmendmentManager mgr = new LcAmendmentManager())
                {
                    var item = mgr.EntityList.Where(a => a.AmendingID == amendingID).FirstOrDefault();

                    if (item != null)
                    {
                        Txn_R3801 txn = new EAI.Service.Transaction.Txn_R3801();
                        txn.REMDAY = DateTime.Now.ToString("yyyy-MM-dd");
                        txn.RCVBK = "009" + item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.開狀行;
                        txn.REMTYPE = "04";
                        txn.MSG1 = "電子信用狀修狀申請待受理通知";
                        txn.MSG2 = "申請人統編:" + item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo + ";"
                                 + "修狀申請書號碼:" + item.AmendmentNo + ";";
                        //txn.MSG3 = item.Documentary.CustomerCreditAlert.Count > 0
                        //    ? String.Concat("該戶有信用貶落指標：",
                        //            String.Join("、", item.Documentary.CustomerCreditAlert.Select(a => a.AlertCode + " " + a.Description)),
                        //            "。請先行確認。")
                        //    : "";
                        txn.MSG3 = item.Documentary.CustomerCreditAlert.Count > 0
                            ? String.Concat("信用貶落：",
                                    String.Join("、", item.Documentary.CustomerCreditAlert.Select(a => a.AlertCode)))
                            : "";

                        txn.RECVER = "放款作業專員";
                        txn.SENDER = "總行";
                        txn.Commit();
                    }
                }
            });
        }

        public static void DoApplyCancellingR3801(int? cancellationID)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                using (CreditCancellationManager mgr = new CreditCancellationManager())
                {
                    var item = mgr.EntityList.Where(a => a.CancellationID == cancellationID).FirstOrDefault();

                    if (item != null)
                    {
                        Txn_R3801 txn = new EAI.Service.Transaction.Txn_R3801();
                        txn.REMDAY = DateTime.Now.ToString("yyyy-MM-dd");
                        txn.RCVBK = "009" + item.LetterOfCredit.CreditApplicationDocumentary.開狀行;
                        txn.REMTYPE = "04";
                        txn.MSG1 = "電子信用狀註銷申請待受理通知";
                        txn.MSG2 = "申請人統編:" + item.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo + ";"
                                 + "註銷申請書號碼:" + item.註銷申請號碼 + ";";
                        txn.MSG3 =  "";

                        txn.RECVER = "放款作業專員";
                        txn.SENDER = "總行";
                        txn.Commit();
                    }
                }
            });
        }

        public static void DoReimburseR3801(int? reimID)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                using (LcEntityManager<NegoDraft> mgr = new LcEntityManager<NegoDraft>())
                {
                    var item = mgr.GetTable<Reimbursement>().Where(d => d.ReimID == reimID).FirstOrDefault();
                    if (item != null)
                    {
                        String subject = item.Documentary.CurrentLevel == (int)Naming.DocumentLevel.待經辦審核
                            ? "匯票還款待受理通知"
                            : "匯票還款已完成通知";

                        if (item.NegoDraft.LcID.HasValue)
                        {
                            Txn_R3801 txn = new EAI.Service.Transaction.Txn_R3801();
                            txn.REMDAY = DateTime.Now.ToString("yyyy-MM-dd");
                            txn.RCVBK = "009" + item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.開狀行;
                            txn.REMTYPE = "04";
                            txn.MSG1 = subject;
                            txn.MSG2 = "申請人統編:" + item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo + ";"
                                     + "信用狀號碼:" + item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.LcNo + ";";

                            txn.MSG3 = "還款申請號碼:" + item.ReimbursementNo + ";"
                                     + "還款金額:" + item.Amount.ToString() + ";";
                            txn.RECVER = "放款作業專員";
                            txn.SENDER = "總行";
                            txn.Commit();

                        }
                        else
                        {
                            Txn_R3801 txn = new EAI.Service.Transaction.Txn_R3801();
                            txn.REMDAY = DateTime.Now.ToString("yyyy-MM-dd");
                            txn.RCVBK = "009" + item.NegoDraft.NegoLC.IssuingBank;
                            txn.REMTYPE = "04";
                            txn.MSG1 = subject;
                            txn.MSG2 = "申請人統編:" + item.NegoDraft.NegoLC.CustomerOfBranch.Organization.ReceiptNo + ";"
                                     + "信用狀號碼:" + item.NegoDraft.NegoLC.LCNo + ";";

                            txn.MSG3 = "還款申請號碼:" + item.ReimbursementNo + ";"
                                     + "還款金額:" + item.Amount.ToString() + ";";
                            txn.RECVER = "放款作業專員";
                            txn.SENDER = "總行";
                            txn.Commit();
                        }
                    }
                }
            });
        }

        public static void DoNegoLoanR3801(int? reimID)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                using (LcEntityManager<NegoDraft> mgr = new LcEntityManager<NegoDraft>())
                {
                    var item = mgr.GetTable<Reimbursement>().Where(d => d.ReimID == reimID).FirstOrDefault();
                    if (item != null && item.NegoLoan!=null)
                    {
                        String subject = item.Documentary.CurrentLevel == (int)Naming.DocumentLevel.待經辦審核
                            ? "匯票還款改貸待受理通知"
                            : "匯票還款改貸已完成通知";

                        if (item.NegoDraft.LcID.HasValue)
                        {
                            Txn_R3801 txn = new EAI.Service.Transaction.Txn_R3801();
                            txn.REMDAY = DateTime.Now.ToString("yyyy-MM-dd");
                            txn.RCVBK = "009" + item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.開狀行;
                            txn.REMTYPE = "04";
                            txn.MSG1 = subject;
                            txn.MSG2 = "申請人統編:" + item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo + ";"
                                     + "信用狀號碼:" + item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.LcNo + ";";

                            //txn.MSG3 = "還款改貸申請號碼:" + item.ReimbursementNo + ";"
                            //         + "還款改貸金額:" + item.Amount.ToString() + ";"
                            //         + (item.Documentary.CustomerCreditAlert.Count > 0
                            //            ? String.Concat("該戶有信用貶落指標：",
                            //                    String.Join("、", item.Documentary.CustomerCreditAlert.Select(a => a.AlertCode + " " + a.Description)),
                            //                    "。請先行確認。")
                            //            : "");
                            txn.MSG3 = "還款改貸申請號碼:" + item.ReimbursementNo + ";"
                                     + "還款改貸金額:" + item.Amount.ToString() + ";"
                                     + (item.Documentary.CustomerCreditAlert.Count > 0
                                        ? String.Concat("信用貶落：",
                                                String.Join("、", item.Documentary.CustomerCreditAlert.Select(a => a.AlertCode)))
                                        : "");
                            txn.RECVER = "放款作業專員";
                            txn.SENDER = "總行";
                            txn.Commit();

                        }
                        else
                        {
                            Txn_R3801 txn = new EAI.Service.Transaction.Txn_R3801();
                            txn.REMDAY = DateTime.Now.ToString("yyyy-MM-dd");
                            txn.RCVBK = "009" + item.NegoDraft.NegoLC.IssuingBank;
                            txn.REMTYPE = "04";
                            txn.MSG1 = subject;
                            txn.MSG2 = "申請人統編:" + item.NegoDraft.NegoLC.CustomerOfBranch.Organization.ReceiptNo + ";"
                                     + "信用狀號碼:" + item.NegoDraft.NegoLC.LCNo + ";";

                            txn.MSG3 = "還款改貸申請號碼:" + item.ReimbursementNo + ";"
                                     + "還款改貸金額:" + item.Amount.ToString() + ";";
                            txn.RECVER = "放款作業專員";
                            txn.SENDER = "總行";
                            txn.Commit();
                        }
                    }
                }
            });
        }

        public static void DoDraftAcceptanceR3801(int? acceptanceID)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                using (LcEntityManager<NegoDraft> mgr = new LcEntityManager<NegoDraft>())
                {
                    var item = mgr.GetTable<NegoDraftAcceptance>().Where(d => d.AcceptanceID == acceptanceID).FirstOrDefault();
                    if (item != null)
                    {
                        if (item.NegoDraft.LcID.HasValue)
                        {
                            Txn_R3801 txn = new EAI.Service.Transaction.Txn_R3801();
                            txn.REMDAY = DateTime.Now.ToString("yyyy-MM-dd");
                            txn.RCVBK = "009" + item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.開狀行;
                            txn.REMTYPE = "04";
                            txn.MSG1 = "承兌匯票已完成通知";
                            txn.MSG2 = "申請人統編:" + item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo + ";"
                                     + "信用狀號碼:" + item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.LcNo + ";";

                            txn.MSG3 = "匯票號碼:" + item.NegoDraft.DraftNo + ";"
                                     + "押匯金額:" + item.NegoDraft.Amount.ToString() + ";";
                            txn.RECVER = "放款作業專員";
                            txn.SENDER = "總行";
                            txn.Commit();
                        }
                    }
                }
            });
        }

        public static void DoNegoLoanRepaymentR3801(int? repaymentID)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                using (LcEntityManager<NegoDraft> mgr = new LcEntityManager<NegoDraft>())
                {
                    var repayment = mgr.GetTable<NegoLoanRepayment>().Where(d => d.RepaymentID == repaymentID).FirstOrDefault();
                    if (repayment != null)
                    {
                        Reimbursement item = repayment.NegoLoan.Reimbursement;
                        String subject = "客戶改貸還款待放行通知";

                        if (item.NegoDraft.LcID.HasValue)
                        {
                            Txn_R3801 txn = new EAI.Service.Transaction.Txn_R3801();
                            txn.REMDAY = DateTime.Now.ToString("yyyy-MM-dd");
                            txn.RCVBK = "009" + item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.開狀行;
                            txn.REMTYPE = "04";
                            txn.MSG1 = subject;
                            txn.MSG2 = "申請人統編:" + item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo + ";"
                                     + "信用狀號碼:" + item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.LcNo + ";";

                            txn.MSG3 = "改貸／還款申請號碼:" + item.ReimbursementNo + ";"
                                     + "改貸還款金額:" + repayment.RepaymentAmount.ToString() + ";";
                            txn.RECVER = "放款作業專員";
                            txn.SENDER = "總行";
                            txn.Commit();

                        }
                        else
                        {
                            Txn_R3801 txn = new EAI.Service.Transaction.Txn_R3801();
                            txn.REMDAY = DateTime.Now.ToString("yyyy-MM-dd");
                            txn.RCVBK = "009" + item.NegoDraft.NegoLC.IssuingBank;
                            txn.REMTYPE = "04";
                            txn.MSG1 = subject;
                            txn.MSG2 = "申請人統編:" + item.NegoDraft.NegoLC.CustomerOfBranch.Organization.ReceiptNo + ";"
                                     + "信用狀號碼:" + item.NegoDraft.NegoLC.LCNo + ";";

                            txn.MSG3 = "改貸／還款申請號碼:" + item.ReimbursementNo + ";"
                                     + "改貸還款金額:" + repayment.RepaymentAmount.ToString() + ";";
                            txn.RECVER = "放款作業專員";
                            txn.SENDER = "總行";
                            txn.Commit();
                        }
                    }
                }
            });
        }

        public static void DoActiveCancellationR3801(int? cancellationID)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                using (BankManager models = new BankManager())
                {
                    var item = models.GetTable<CreditCancellation>().Where(c => c.CancellationID == cancellationID).FirstOrDefault();
                    if (item != null)
                    {
                        Txn_R3801 txn = new EAI.Service.Transaction.Txn_R3801();
                        txn.REMDAY = DateTime.Now.ToString("yyyy-MM-dd");
                        txn.RCVBK = "009" + item.LetterOfCredit.CreditApplicationDocumentary.開狀行;
                        txn.REMTYPE = "04";
                        txn.MSG1 = "電子信用狀主動餘額註銷待登錄通知";
                        txn.MSG2 = "申請人統編:" + item.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo + ";"
                                 + "信用狀號碼:" + item.LetterOfCredit.LcNo;

                        txn.MSG3 =  "";

                        txn.RECVER = "放款作業專員";
                        txn.SENDER = "總行";
                        txn.Commit();
                    }
                }

            });
        }

        public static void DoReadyTodoR3801(String branch, String branchCRC, Naming.DocumentTypeDefinition docType)
        {
            DoReadyTodoR3801(branch, branchCRC, $"{docType}分行經辦未受理");
        }

        public static void DoReadyTodoR3801(String branch, String branchCRC, String message, String reason = null)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                Txn_R3801 txn = new EAI.Service.Transaction.Txn_R3801();
                txn.REMDAY = DateTime.Now.ToString("yyyy-MM-dd");
                txn.RCVBK = $"009{branchCRC}";
                txn.REMTYPE = "04";

                txn.MSG1 = message;
                txn.MSG2 = $"受理分行：{branch}";
                if (reason != null)
                {
                    txn.MSG3 = reason;
                }

                txn.RECVER = "放款作業專員";
                txn.SENDER = "總行";
                txn.Commit();

                txn.RCVBK = $"009{branch}";
                txn.Commit();
            });
        }


        // add 三個out 參數 980603
        public bool AllowLcWithEAI(int appID, out IEAITransaction transaction, out CreditApplicationDocumentary item, out String lcNo)
        {
            bool bResult = true;
            lcNo = null;
            transaction = null;
            //btnConfirm.CommandArgument = Request["AppID"];				
            item = this.EntityList.Where(a => a.AppID == appID).FirstOrDefault();
            if (item != null && item.OpeningID.HasValue && item.PaymentID.HasValue)
            {
                Txn_L1203 txn = new Txn_L1203();
                transaction = txn;
                txn.KINBR = UserProfile.UserProfileRow.branch;
                txn.HCODE = "0";
                txn.RBRNO = item.OpeningApplicationDocumentary.實績行;
                txn.SBRNO = item.OpeningApplicationDocumentary.掛帳行;
                txn.CIFKEY = item.CustomerOfBranch.Organization.ReceiptNo;
                txn.MGDATA = item.PaymentNotification.額度號碼.Replace("-", "");
                txn.ACTNO = item.OpeningApplicationDocumentary.帳號.Replace("-", "");
                var seg = item.PaymentNotification.帳戶性質.Split('-');
                txn.CHARCD1 = seg[0];
                txn.CHARCD2 = seg[1];
                txn.ACNO = item.PaymentNotification.會計科目;
                txn.SBNO = item.OpeningApplicationDocumentary.會計子目;
                txn.SDAY = String.Format("{0:yyyyMMdd}", System.DateTime.Now);
                txn.GRAMT = item.PaymentNotification.保證金.ToString();
                txn.EDAY = String.Format("{0:yyyyMMdd}", item.LcItem.有效期限);
                txn.RTDAY = item.OpeningApplicationDocumentary.匯票期限.ToString();
                txn.NOTEBK = "009" + item.通知行;
                txn.BUNINO = item.BeneficiaryData.Organization.ReceiptNo;
                txn.ISDAYCNT = item.OpeningApplicationDocumentary.零星天數計收 == true ? "2" : "1";
                txn.RATE = item.OpeningApplicationDocumentary.手續費率.ToString();

                //***************************************************
                //***************************************************
                txn.FEE = item.OpeningApplicationDocumentary.手續費金額.ToString();          //手續費金額
                txn.HCRATE = item.OpeningApplicationDocumentary.承兌費率.ToString();      //承兌費費率
                txn.HCFEE = item.PaymentNotification.承兌手續費.ToString();           //承兌手續費
                txn.TXTYPE = item.PaymentNotification.現轉別.Substring(0, 1);           //現轉別
                txn.PACTNO = !String.IsNullOrEmpty(item.PaymentNotification.扣款帳號) ? item.PaymentNotification.扣款帳號.Replace("-", "") : null;         //扣款帳號
                txn.CKAMT = item.OpeningApplicationDocumentary.票繳金額.ToString();              //票繳金額
                txn.CKNO = item.OpeningApplicationDocumentary.支票號碼;                 //支票號碼
                txn.CKBKNO7 = !String.IsNullOrEmpty(item.OpeningApplicationDocumentary.支票付款行) ? item.OpeningApplicationDocumentary.支票付款行.Replace("-", "") : null;                  //支票付款行
                txn.CKACTNO = item.OpeningApplicationDocumentary.發票人帳號; //出票人帳號
                txn.FUNCD = !String.IsNullOrEmpty(item.PaymentNotification.用途別) ? item.PaymentNotification.用途別.Substring(0, 1) : null;                //用途別
                txn.MGENO = item.OpeningApplicationDocumentary.貸放主管編號;           //貸放主管編號
                txn.SPENO = item.OpeningApplicationDocumentary.貸放負責人編號;           //貸放負責人編號
                txn.PAYNO = item.OpeningApplicationDocumentary.付款人.Replace("-", "");       //付款人
                txn.PAYTYPE = item.見票即付 ? "1" : "2";           //付款期限-天數
                txn.PAYDAY = item.LcItem.定日付款.ToString();            //付款期限-天數
                txn.SEAL = item.SpecificNote.原留印鑑相符 == true ? "1" : "2";                     //匯票付款或承兌申請書上印鑑方式
                txn.DELIVER = item.SpecificNote.分批交貨 == true ? "1" : "2";                  //交貨方式
                if (item.LcItem.PaymentDate.HasValue)
                {
                    txn.PAYDATE = String.Format("{0:yyyyMMdd}", item.LcItem.PaymentDate);
                }
                else
                {
                    txn.PAYDATE = "00000000";
                }

                String goods = "";
                if (item.LcItem.Goods != null && (goods = item.LcItem.Goods.Trim()).Length > 0)
                {
                    goods = ValidityAgent.ConverttoFullWidthString(goods);
                }
                else if (item.LcItem.GoodsDetails.Count > 0)
                {

                    goods =  ValidityAgent.ConverttoFullWidthString(String.Join(" ", item.LcItem.GoodsDetails.Select(s => String.Format("品名:{0} 規格:{1} 單價:{2} 數量:{3} 金額:{4} 備註:{5}",
                            s.ProductName, s.ProductSize, s.UnitPrice, s.Quantity, s.Amount,s.Remark))));
                }

                buildGoodsContent(txn, goods);

                txn.DEADLE = String.Format("{0:yyyyMMdd}", item.SpecificNote.最後交貨日);        //最後交貨日期
                //					txn.DEADLE = System.DateTime.ParseExact(txn.DEADLE,"yyyy.MM.dd",System.Globalization.CultureInfo.CurrentCulture).ToString();
                txn.GTXNO = item.OpeningApplicationDocumentary.交易流水號.ToString();                    //**信用狀號碼
                txn.RVSNO = item.PaymentNotification.銷帳編號;               //銷帳編號
                txn.LOANTYP = !String.IsNullOrEmpty(item.PaymentNotification.融資業務分類) ? item.PaymentNotification.融資業務分類.Substring(0, 1) : null;         //融資業務分類

                txn.GOVSTYP = !String.IsNullOrEmpty(item.PaymentNotification.政府專案補助貸款分類) ? item.PaymentNotification.政府專案補助貸款分類.Substring(0, 2) : null;          //政府專案補助貸款分類
                txn.SETGFG = item.OpeningApplicationDocumentary.十足擔保記號 == true ? "2" : "1";         //十足擔保記號
                //					txn.TYPE = rowDetail["CreditKind"].ToString();              //信用狀種類
                txn.TYPE = "1";                                             //信用狀種類
                txn.FSQAMT = item.LcItem.開狀金額.ToString();            //開狀金額
                //					txn.FCURCD = rowDetail["CreditKind"].ToString();            //開狀幣別
                txn.FCURCD = "00";           //開狀幣別
                txn.EXRATE = item.OpeningApplicationDocumentary.開狀匯率.ToString();          //開狀匯率
                txn.TXAMT = item.OpeningApplicationDocumentary.記帳金額.ToString();         //記帳金額
                txn.ELCDTLFG = item.ForFpgService(BeneficiaryServiceGroup.ServiceDefinition.Fpg) 
                                    ? "2" 
                                    : item.ForFpgService(BeneficiaryServiceGroup.ServiceDefinition.Chimei)
                                        ? "3"
                                        : "1";              //申請種類
                txn.CRGRRATE = String.Format("{0:.}", item.PaymentNotification.信保成數);      //信保成數
                txn.CRGRTYPE = item.PaymentNotification.信保種類;            //信保種類
                txn.CRGRAMT = item.PaymentNotification.實際送保金額.ToString();//實際送保金額
                txn.Rq.EAIBody.MsgRq.SvcRq.CRGRDT = String.Format("{0:yyyyMMdd}", item.PaymentNotification.送保到期日);        //送保到期日
                if(item.PaymentNotification.聯貸流水號.HasValue)
                {
                    txn.Rq.EAIBody.MsgRq.SvcRq.SINO = item.開狀行 + item.PaymentNotification.CustomerID + String.Format("{0:0000}", item.PaymentNotification.聯貸流水號);
                }
                txn.Rq.EAIBody.MsgRq.SvcRq.GUNO = item.OpeningApplicationDocumentary.交易憑證編號;

                bResult = txn.Commit();
                if (bResult)
                {
                    lcNo = txn.Rs_ACTNO;
                }
            }
            return bResult;
        }

        private void buildGoodsContent(Txn_L1203 txn, String goods)
        {
            if (goods.Length <= MAX_GOODS_LENGTH_PER_LINE)
            {
                txn.CONT1 = goods;										//貨品內容1
                txn.CONT2 = "";											//貨品內容2
                txn.CONT3 = "";											//貨品內容3
            }
            else if (goods.Length <= MAX_GOODS_LENGTH_PER_LINE * 2)
            {
                txn.CONT1 = goods.Substring(0, MAX_GOODS_LENGTH_PER_LINE);
                txn.CONT2 = goods.Substring(MAX_GOODS_LENGTH_PER_LINE);
                txn.CONT3 = "";
            }
            else if (goods.Length <= MAX_GOODS_LENGTH_PER_LINE * 3)
            {
                txn.CONT1 = goods.Substring(0, MAX_GOODS_LENGTH_PER_LINE);
                txn.CONT2 = goods.Substring(MAX_GOODS_LENGTH_PER_LINE, MAX_GOODS_LENGTH_PER_LINE);
                txn.CONT3 = goods.Substring(MAX_GOODS_LENGTH_PER_LINE * 2);
            }
            else
            {
                txn.CONT1 = goods.Substring(0, MAX_GOODS_LENGTH_PER_LINE);
                txn.CONT2 = goods.Substring(MAX_GOODS_LENGTH_PER_LINE, MAX_GOODS_LENGTH_PER_LINE);
                txn.CONT3 = goods.Substring(MAX_GOODS_LENGTH_PER_LINE * 2, MAX_GOODS_LENGTH_PER_LINE);
            }
        }

        public bool AmendLcWithEAI(int amendingID, out string rspCode,out AmendingLcApplication item)
        {
            bool bResult = true;
            bool bRetrial = false;

            rspCode = null;
            System.Text.StringBuilder sbRspCode = new System.Text.StringBuilder();

            Txn_L3700 l3700 = null;
            Txn_L4500 l4500 = null;
            Txn_L5300 l5300 = null;

            item = this.GetTable<AmendingLcApplication>().Where(a => a.AmendingID == amendingID).FirstOrDefault();
            if (item != null)
            {
                LcItem newItem, oldItem;
                AttachableDocument newAttach, oldAttach;
                SpecificNote newSN, oldSN;
                String description;
                String newNB, oldNB;
                int itemNo;
                bool revised = false;

                if (item.CheckL3700Items(out newItem, out oldItem,
                    out newAttach, out oldAttach,
                    out newSN, out oldSN, out description, out itemNo,out newNB,out oldNB))
                {
                    revised = true;
                    l3700 = new Txn_L3700();
                    l3700.KINBR = UserProfile.UserProfileRow.branch;
                    l3700.HCODE = "0";
                    l3700.ACTNO = item.LetterOfCreditVersion.LetterOfCredit.LcNo.Replace("-", "");
                    l3700.CIFKEY = item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo;
                    l3700.ITEM = String.Format("{0:00}", itemNo);
                    if (newItem.開狀金額 > oldItem.開狀金額)
                    {
                        l3700.SQAMT = (newItem.開狀金額 - oldItem.開狀金額).ToString();
                    }
                    else
                    {
                        l3700.SQAMT = "";
                    }
                    //MiYu 2006-10-31
                    //l3700.DATE = String.Format("{0:yyyyMMdd}",rowDetail["LcExpiry"]);
                    l3700.DATE = String.Format("{0:yyyyMMdd}", newItem.有效期限);
                    l3700.CAUSE = !String.IsNullOrEmpty(item.AmendingLcRegistry.延長信用狀原因) ? item.AmendingLcRegistry.延長信用狀原因.Substring(0, 1) : null;
                    l3700.RTDATE = item.AmendingLcRegistry.延長匯票期限.ToString();
                    l3700.RTCAUSE = !String.IsNullOrEmpty(item.AmendingLcRegistry.延長匯票期限原因) ? item.AmendingLcRegistry.延長匯票期限原因.Substring(0, 1) : null;
                    l3700.GRAMT = String.Format("{0}", item.PaymentNotification.增加保證金金額);
                    l3700.CHRATE = String.Format("{0}", item.PaymentNotification.手續費率);
                    l3700.FEE = String.Format("{0}", item.AmendingLcRegistry.改狀費金額);
                    l3700.HCFEE = String.Format("{0}", item.PaymentNotification.承兌手續費);
                    l3700.TXTYPE = !String.IsNullOrEmpty(item.PaymentNotification.現轉別) ? item.PaymentNotification.現轉別.Substring(0, 1) : null;
                    if (l3700.TXTYPE == "4")
                    {
                        if (!String.IsNullOrEmpty(item.PaymentNotification.銷帳編號))
                        {
                            l3700.RVSYEAR = item.PaymentNotification.銷帳編號.Substring(0, 1);
                            l3700.RVSSRNO = item.PaymentNotification.銷帳編號.Substring(1);
                        }
                    }


                    l3700.PACTNO = !String.IsNullOrEmpty(item.PaymentNotification.扣款帳號) ? item.PaymentNotification.扣款帳號.Replace("-", "") : "";

                    if (item.AmendingLcRegistry.增加信用狀記帳金額 > 0)
                    {
                        l3700.FSQAMT = String.Format("{0}", item.AmendingLcRegistry.增加信用狀記帳金額);
                    }
                    else
                    {
                        l3700.FSQAMT = "";
                    }

                    l3700.FCURCD = String.Format("{0:00}", item.LcItem.幣別);
                    l3700.EXRATE = String.Format("{0}", item.AmendingLcRegistry.匯率);

                    bResult = l3700.Commit();
                    if (!bResult)
                    {
                        sbRspCode.Append(l3700.RspCode).Append(":").Append(this.ReadErrCodeDis(l3700));
                        bRetrial = "3001".Equals(l3700.RspCode);
                    }

                }
                //檢查L4500是否有修改項目
                if (bResult && newItem.CheckL4500Items(oldItem))
                {
                    revised = true;
                    l4500 = new Txn_L4500();
                    l4500.KINBR = UserProfile.UserProfileRow.branch;
                    l4500.HCODE = "0";
                    l4500.ACTNO = item.LetterOfCreditVersion.LetterOfCredit.LcNo.Replace("-", "");
                    l4500.CIFKEY = item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo;
                    l4500.CAUSE = !String.IsNullOrEmpty(item.AmendingLcRegistry.沖銷原因) ? item.AmendingLcRegistry.沖銷原因.Substring(0, 2) : null;
                    l4500.TXAMT = String.Format("{0}", Math.Abs(newItem.開狀金額.Value - oldItem.開狀金額.Value));
                    l4500.EXRATE = String.Format("{0}", item.AmendingLcRegistry.匯率);
                    l4500.GRAMT = String.Format("{0}", item.AmendingLcRegistry.沖銷存入保證金金額);
                    l4500.BPBKNO = item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.通知行;

                    l4500.TXTYPE = !String.IsNullOrEmpty(item.PaymentNotification.撥款方式) ? item.PaymentNotification.撥款方式.Substring(0, 1) : null;
                    l4500.PACTNO = !String.IsNullOrEmpty(item.PaymentNotification.入戶帳號) ? item.PaymentNotification.入戶帳號.Replace("-", "") : null;
                    l4500.GTXNO = item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.OpeningApplicationDocumentary.交易流水號.ToString();
                    l4500.VRATE = "";

                    bResult = l4500.Commit();

                    if (!bResult)
                    {
                        if (sbRspCode.Length > 0)
                            sbRspCode.Append(",");

                        sbRspCode.Append(l4500.RspCode).Append(":").Append(this.ReadErrCodeDis(l4500));
                        bRetrial = "3001".Equals(l4500.RspCode);
                    }

                    if (!bResult && l3700 != null)
                    {
                        //交易失敗,執行EC
                        l3700.HCODE = "1";
                        if (!l3700.Commit())
                        {
                            this.Context.AbortTransaction(amendingID, UserProfile.UserProfileRow.pid, (int)Naming.DocumentLevel.待註記);

                            sbRspCode.Append(",EC失敗[").Append(l3700.RspCode).Append(":")
                                .Append(this.ReadErrCodeDis(l3700)).Append("]");

                            bRetrial = true;
                        }
                    }

                }
                //檢查L3700是否有修改項目
                if (bResult && (!revised || newItem.CheckL5300Items(oldItem, newSN, oldSN,newNB,oldNB)))
                {
                    l5300 = new Txn_L5300();
                    l5300.KINBR = UserProfile.UserProfileRow.branch;
                    l5300.HCODE = "0";
                    l5300.ACTNO = item.LetterOfCreditVersion.LetterOfCredit.LcNo.Replace("-", "");
                    l5300.CIFKEY = item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo;
                    l5300.ITEM = "00"; //modify by tammy 04-->00
                    l5300.NOTEBK = "009" + newNB;
                    l5300.BCIFKEY = item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.BeneficiaryData.Organization.ReceiptNo;
                    l5300.FUNCD = !String.IsNullOrEmpty(item.PaymentNotification.用途別) ? item.PaymentNotification.用途別.Substring(0, 1) : null;
                    l5300.PAYNO = "009" + item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.付款行;
                    l5300.CONT1 = "";
                    l5300.CONT2 = "";
                    l5300.CONT3 = "";
                    if (item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.見票即付)
                    {
                        l5300.PAYTYPE = "1";
                        l5300.PAYDAY = "";
                        l5300.PAYDATE = item.LcItem.PaymentDate.HasValue ? String.Format("{0:yyyyMMdd}", item.LcItem.PaymentDate) : "00000000";
                    }
                    else
                    {
                        l5300.PAYTYPE = "2";
                        if (item.LcItem.PaymentDate.HasValue)
                        {
                            l5300.PAYDAY = "0000";
                            l5300.PAYDATE =  String.Format("{0:yyyyMMdd}", item.LcItem.PaymentDate);
                        }
                        else
                        {
                            Txn_LR017 txn = new EAI.Service.Transaction.Txn_LR017();
                            txn.Account = item.LetterOfCreditVersion.LetterOfCredit.LcNo.Replace("-", "").Trim();
                            if(txn.Commit())
                            {
                                l5300.PAYDAY = txn.Rs.EAIBody.MsgRs.SvcRs.RTDAY;
                            }
                            else
                            {
                                l5300.PAYDAY = $"{item.LcItem.定日付款-item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.LcItem.定日付款}";
                            }
                            l5300.PAYDATE = "00000000";
                        }
                    }

                    if (item.SpecificNote.原留印鑑相符 == true)
                    {
                        l5300.SEAL = "1";
                    }
                    else
                    {
                        l5300.SEAL = "2";
                    }

                    if (item.SpecificNote.分批交貨 == true)
                    {
                        l5300.DELIVER = "1";
                    }
                    else
                    {
                        l5300.DELIVER = "2";
                    }

                    String goods = String.Format("{0}{1}", newItem.Goods, String.Join("", newItem.GoodsDetails.Select(s => String.Format("{0} {1} {2} {3} {4}",
                        s.ProductName, s.ProductSize, s.UnitPrice, s.Quantity, s.Amount)).ToArray()));

                    goods = ValidityAgent.ConverttoFullWidthString(goods);

                    if (goods.Length > 0)
                    {
                        if (goods.Length <= MAX_GOODS_LENGTH_PER_LINE)
                        {
                            l5300.CONT1 = goods;										//貨品內容1
                            l5300.CONT2 = "";											//貨品內容2
                            l5300.CONT3 = "";											//貨品內容3
                        }
                        else if (goods.Length <= MAX_GOODS_LENGTH_PER_LINE * 2)
                        {
                            l5300.CONT1 = goods.Substring(0, MAX_GOODS_LENGTH_PER_LINE);
                            l5300.CONT2 = goods.Substring(MAX_GOODS_LENGTH_PER_LINE);
                            l5300.CONT3 = "";
                        }
                        else if (goods.Length <= MAX_GOODS_LENGTH_PER_LINE * 3)
                        {
                            l5300.CONT1 = goods.Substring(0, MAX_GOODS_LENGTH_PER_LINE);
                            l5300.CONT2 = goods.Substring(MAX_GOODS_LENGTH_PER_LINE, MAX_GOODS_LENGTH_PER_LINE);
                            l5300.CONT3 = goods.Substring(MAX_GOODS_LENGTH_PER_LINE * 2);
                        }
                        else
                        {
                            l5300.CONT1 = goods.Substring(0, MAX_GOODS_LENGTH_PER_LINE);
                            l5300.CONT2 = goods.Substring(MAX_GOODS_LENGTH_PER_LINE, MAX_GOODS_LENGTH_PER_LINE);
                            l5300.CONT3 = goods.Substring(MAX_GOODS_LENGTH_PER_LINE * 2, MAX_GOODS_LENGTH_PER_LINE);
                        }
                    }
                    l5300.DEADL = String.Format("{0:yyyyMMdd}", item.SpecificNote.最後交貨日);
                    l5300.GTXNO = item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.OpeningApplicationDocumentary.交易流水號.ToString();
                    l5300.CHFEE = l3700 == null ? item.AmendingLcRegistry.改狀費金額.ToString() : "";
                    l5300.TXTYPE = !String.IsNullOrEmpty(item.PaymentNotification.現轉別) ? item.PaymentNotification.現轉別.Substring(0, 1) : null;
                    if (l5300.TXTYPE == "4")
                    {
                        if (!String.IsNullOrEmpty(item.PaymentNotification.銷帳編號))
                        {
                            l5300.RVSYEAR = item.PaymentNotification.銷帳編號.Substring(0, 1);
                            l5300.RVSSRNO = item.PaymentNotification.銷帳編號.Substring(1);
                        }
                    }

                    l5300.PACTNO = !String.IsNullOrEmpty(item.PaymentNotification.扣款帳號) ? item.PaymentNotification.扣款帳號.Replace("-", "") : "";

                    l5300.ELCDTLFG = item.ForFpgService(BeneficiaryServiceGroup.ServiceDefinition.Fpg)
                                    ? "2"
                                    : item.ForFpgService(BeneficiaryServiceGroup.ServiceDefinition.Chimei)
                                        ? "3"
                                        : "1";              //申請種類
                    l5300.CRGRRATE = String.Format("{0:.}", item.PaymentNotification.信保成數);
                    l5300.CRGRTYPE = item.PaymentNotification.信保種類;
                    l5300.CRGRAMT = String.Format("{0}", item.PaymentNotification.實際送保金額);
                    l5300.Rq.EAIBody.MsgRq.SvcRq.CRGRDT = String.Format("{0:yyyyMMdd}", item.PaymentNotification.送保到期日);        //送保到期日
                    if (item.PaymentNotification.聯貸流水號.HasValue)
                    {
                        l5300.Rq.EAIBody.MsgRq.SvcRq.SINO = item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.開狀行 + item.PaymentNotification.CustomerID + String.Format("{0:0000}", item.PaymentNotification.聯貸流水號);
                    }
                    l5300.Rq.EAIBody.MsgRq.SvcRq.GUNO = item.AmendingLcRegistry.交易憑證編號;

                    bResult = l5300.Commit();
                    if (!bResult)
                    {
                        if (sbRspCode.Length > 0)
                            sbRspCode.Append(",");

                        sbRspCode.Append(l5300.RspCode).Append(":").Append(this.ReadErrCodeDis(l5300));
                        bRetrial = "3001".Equals(l5300.RspCode);

                        if (l4500 != null)
                        {
                            //交易失敗,執行EC
                            l4500.HCODE = "1";
                            if (!l4500.Commit())
                            {
                                sbRspCode.Append(",EC失敗[").Append(l4500.RspCode).Append(":")
                                    .Append(this.ReadErrCodeDis(l4500)).Append("]");
                                bRetrial = true;
                            }
                        }

                        if (l3700 != null)
                        {
                            //交易失敗,執行EC
                            l3700.HCODE = "1";
                            if (!l3700.Commit())
                            {
                                this.Context.AbortTransaction(amendingID, UserProfile.UserProfileRow.pid, (int)Naming.DocumentLevel.待註記);
                                sbRspCode.Append(",EC失敗[").Append(l3700.RspCode).Append(":")
                                    .Append(this.ReadErrCodeDis(l3700)).Append("]");
                                bRetrial = true;
                            }
                        }
                    }
                    //}


                }

                if (bRetrial)
                {
                    this.Context.AbortTransaction(amendingID, UserProfile.UserProfileRow.pid, (int)Naming.DocumentLevel.待註記);
                }

            }

            if (!bResult && sbRspCode.Length > 0)
            {
                if (bRetrial)
                    sbRspCode.Append(",本筆已轉待註記");
                rspCode = sbRspCode.ToString();
            }

            return bResult;

        }

        private bool doNegoL4500EAI(NegoDraft row, out string rspCode, out bool bRetrial, out Txn_L4500 txn)
        {
            string str;

            txn = new Txn_L4500();
            //帶入參數			
            str = row.DraftNo;    //新增匯票申請書號碼 MIN-YU-081201 add
            //if (str.Trim().Length > 7)
            //{
            //    L4500.LNCKNO = str.Substring(str.Trim().Length - 7, 7);  //匯票申請書號碼末七碼

            //}
            //else
            //{
            //    L4500.LNCKNO = str.Trim();
            //}
            txn.LNCKNO = String.Format("{0}", row.DraftID % 10000000);
            txn.KINBR = UserProfile.UserProfileRow.branch;
            txn.HCODE = "0";							//更正記號
            txn.ACTNO = (row.LcID.HasValue ? row.LetterOfCreditVersion.LetterOfCredit.LcNo : row.ACTNO()).Replace("-", "");
            txn.CIFKEY = row.LcID.HasValue?row.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo : row.NegoLC.ApplicantReceiptNo;				//買方統編
            str = row.NegoDraftRegistry.沖銷原因;
            txn.CAUSE = (str != null && str.Length >= 2) ? str.Substring(0, 2) : null; 					//沖銷原因
            txn.TXAMT = String.Format("{0:0.00}", row.Amount);				//沖銷信用狀金額            
            txn.EXRATE = row.NegoDraftRegistry.匯率.ToString();				//匯率
            txn.GRAMT = row.NegoDraftRegistry.沖銷存入保證金金額.ToString();				//沖銷存入保證金金額
            txn.TXTYPE = row.NegoDraftRegistry.撥款方式.Substring(0, 1);				//撥款方式            
            txn.BPBKNO = (txn.TXTYPE == "6" || txn.TXTYPE == "7" ) ? row.NegoDraftExtension.NegoBranch : row.NegoDraftExtension.LcBranch;//押匯行 開狀行 <> 押匯行 6 7 優利會科才會產生聯行往來
            //txn.BPBKNO = row.LcID.HasValue ? row.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.開狀行 : row.NegoLC.IssuingBank;
            txn.PACTNO = !String.IsNullOrEmpty(row.NegoDraftRegistry.撥款帳號科目) ? row.NegoDraftRegistry.撥款帳號科目.Replace("-", "") : "";				//扣款帳號/科目							
            txn.GTXNO = row.NegoDraftRegistry.GTXNO.ToString();						//信用狀流水號,共10碼
            decimal? val = row.NegoDraftRegistry.加減碼 + row.NegoDraftRegistry.掛牌利率;
            txn.VRATE = val.HasValue ? val.Value.ToString() : "0";				//墊款利率
            txn.IRTKD = row.NegoDraftRegistry.利率種類?.Substring(0, 2);                       //利率種類 固定為 04
            txn.RATECD = row.NegoDraftRegistry.利率型態?.Substring(0, 2);                //利率型態 01 or 04
            txn.IRTCD = row.NegoDraftRegistry.利率別?.Substring(0, 4);								//利率別
            txn.FITIRT = string.Format("{0:00.00000}", row.NegoDraftRegistry.掛牌利率 ?? 0);
            txn.IXPR = string.Format("{0:00.00000}", row.NegoDraftRegistry.加減碼 ?? 0);
            rspCode = null;
            bRetrial = false;

            bool bResult = txn.Commit();


            System.Text.StringBuilder sbRspCode = new System.Text.StringBuilder(txn.RspCode);

            if (!bResult)
            {
                sbRspCode.Append(":").Append(this.ReadErrCodeDis(txn));
                bRetrial = "3001".Equals(txn.RspCode);
            }

            if (!bResult && sbRspCode.Length > 0)
            {
                if (bRetrial)
                    sbRspCode.Append(",本筆已轉待註記");
            }

            rspCode = sbRspCode.ToString();
            return bResult;

        }

        private bool doNegoL1201EAI(NegoDraft draft, out string rspCode, out bool bRetrial, out Txn_L1201 txn)
        {
            txn = new Txn_L1201();
            //帶入參數
            txn.Rq.EAIBody.MsgRq.SvcRq.KINBR = UserProfile.UserProfileRow.branch;	//輸入行
            txn.Rq.EAIBody.MsgRq.SvcRq.HCODE = "0";				//更正記號
            txn.Rq.EAIBody.MsgRq.SvcRq.LCACTNO = draft.LcID.HasValue ? draft.LetterOfCreditVersion.LetterOfCredit.LcNo : draft.ACTNO();		//信用狀帳支號,共16碼
            txn.Rq.EAIBody.MsgRq.SvcRq.LCGTXNO = draft.NegoDraftRegistry.GTXNO.ToString();			//信用狀流水號,共10碼
            txn.Rq.EAIBody.MsgRq.SvcRq.RBRNO = draft.LcID.HasValue ? draft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.OpeningApplicationDocumentary.實績行 : draft.NegoLC.IssuingBank;	//實績行
            txn.Rq.EAIBody.MsgRq.SvcRq.CIFKEY = draft.LcID.HasValue ? draft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo : draft.NegoLC.CustomerOfBranch.Organization.ReceiptNo;	//買方統編
            txn.Rq.EAIBody.MsgRq.SvcRq.ACBRNO = draft.LcID.HasValue ? draft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.OpeningApplicationDocumentary.掛帳行 : draft.NegoLC.IssuingBank;	//掛帳行
            txn.Rq.EAIBody.MsgRq.SvcRq.MGDATA = draft.PaymentNotification.額度號碼.Replace("-", "");		//額度號碼
            txn.Rq.EAIBody.MsgRq.SvcRq.ACTNO = (draft.LcID.HasValue ? draft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.開狀行 : draft.NegoLC.IssuingBank) + "470000000000";		//帳號
            //20050827 Andy 修改 EAI交易(L1201)時 匯票號碼欄位(L1201.IBGTXNO)帶入押匯申請書號碼(原帶入押匯號碼)
            //L1201.IBGTXNO=_strFuDraftNo;	//匯票號碼
            txn.Rq.EAIBody.MsgRq.SvcRq.IBGTXNO = draft.AppNo();	//押匯申請書號碼
            txn.Rq.EAIBody.MsgRq.SvcRq.TXAMT = String.Format("{0:0.00}", draft.Amount);	//匯票金額			
            txn.Rq.EAIBody.MsgRq.SvcRq.CRGRRATE = String.Format("{0:.}", draft.NegoDraftRegistry.信保成數);	//信保成數
            txn.Rq.EAIBody.MsgRq.SvcRq.CRGRTYPE = draft.NegoDraftRegistry.信保種類;	//信保種類
            txn.Rq.EAIBody.MsgRq.SvcRq.CRGRDT = String.Format("{0:yyyyMMdd}", draft.NegoDraftRegistry.送保到期日);        //送保到期日
            txn.Rq.EAIBody.MsgRq.SvcRq.EDAY = String.Format("{0:yyyyMMdd}", draft.UsanceDate());	//匯票到期日
            txn.Rq.EAIBody.MsgRq.SvcRq.SDAY = String.Format("{0:yyyyMMdd}", draft.ImportDate);	//發票日
            if ( !String.IsNullOrEmpty(draft.PaymentNotification.帳戶性質))
            {
                string[] accountType = draft.PaymentNotification.帳戶性質.Split('-');

                txn.Rq.EAIBody.MsgRq.SvcRq.CHARCD1 = accountType[0];	//帳戶性質前3碼
                txn.Rq.EAIBody.MsgRq.SvcRq.CHARCD2 = accountType.Length > 0 ? accountType[1] : "";	//帳戶性質後3碼
            }
            else
            {
                txn.Rq.EAIBody.MsgRq.SvcRq.CHARCD1 = "";				//帳戶性質前3碼
                txn.Rq.EAIBody.MsgRq.SvcRq.CHARCD2 = "";				//帳戶性質後3碼
            }
            txn.Rq.EAIBody.MsgRq.SvcRq.ACNO = draft.NegoDraftRegistry.會計科目;			//會計科目
            txn.Rq.EAIBody.MsgRq.SvcRq.SBNO = draft.NegoDraftRegistry.會計子目;			//會計子目
            txn.Rq.EAIBody.MsgRq.SvcRq.BUNINO = draft.LcID.HasValue ? draft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.BeneficiaryData.Organization.ReceiptNo
                : draft.NegoLC.BeneficiaryData.Organization.ReceiptNo;		//發票人(賣方統編)
            txn.Rq.EAIBody.MsgRq.SvcRq.IBKNO = "009";
            txn.Rq.EAIBody.MsgRq.SvcRq.IBRNO = draft.LcID.HasValue ? draft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.通知行 : draft.NegoLC.AdvisingBank;
            txn.Rq.EAIBody.MsgRq.SvcRq.HCRATE = draft.NegoDraftRegistry.承兌手續費率.ToString();		//承兌手續費率
            txn.Rq.EAIBody.MsgRq.SvcRq.HCFEE = draft.NegoDraftRegistry.承兌手續費金額.ToString();			//承兌手續費金額
            txn.Rq.EAIBody.MsgRq.SvcRq.TXTYPE = draft.NegoDraftRegistry.現轉別 != null ? draft.NegoDraftRegistry.現轉別.Substring(0, 1) : null;		//現轉別
            txn.Rq.EAIBody.MsgRq.SvcRq.PACTNO = !String.IsNullOrEmpty(draft.PaymentNotification.扣款帳號) ? draft.PaymentNotification.扣款帳號.Replace("-", "") : null;			//扣款帳支號 Thomas
            txn.Rq.EAIBody.MsgRq.SvcRq.CKAMT = draft.NegoDraftRegistry.票繳金額.ToString();			//票繳金額
            txn.Rq.EAIBody.MsgRq.SvcRq.CKNO = draft.NegoDraftRegistry.支票號碼;			//支票號碼
            txn.Rq.EAIBody.MsgRq.SvcRq.CKBKNO7 = "";		//付款行
            txn.Rq.EAIBody.MsgRq.SvcRq.CKACTNO = "";	//發票人帳號
            txn.Rq.EAIBody.MsgRq.SvcRq.MGENO = "";			//貸放主管編號
            txn.Rq.EAIBody.MsgRq.SvcRq.SPENO = "";			//貸放負責人編號
            txn.Rq.EAIBody.MsgRq.SvcRq.FUNCD = !String.IsNullOrEmpty(draft.NegoDraftRegistry.用途別) ? draft.NegoDraftRegistry.用途別.Substring(0, 1) : null;			//用途別
            txn.Rq.EAIBody.MsgRq.SvcRq.RVSNO = draft.PaymentNotification.銷帳編號;		//銷帳編號
            txn.Rq.EAIBody.MsgRq.SvcRq.LOANTYP = !String.IsNullOrEmpty(draft.NegoDraftRegistry.融資業務分類) ? draft.NegoDraftRegistry.融資業務分類.Substring(0, 1) : null;	//融資業務分類
            txn.Rq.EAIBody.MsgRq.SvcRq.GOVSTYP = !String.IsNullOrEmpty(draft.NegoDraftRegistry.政府專案補助貸款分類) ? draft.NegoDraftRegistry.政府專案補助貸款分類.Substring(0, 2) : null;	//政府專案補助貸款分類
            txn.Rq.EAIBody.MsgRq.SvcRq.SETGFG = draft.NegoDraftRegistry.十足擔保記號 == true ? "1" : "0";		//十足擔保記號											
            txn.Rq.EAIBody.MsgRq.SvcRq.GRAMT = draft.NegoDraftRegistry.使用存入保證金金額.ToString();   //使用存入保證金金額
            txn.Rq.EAIBody.MsgRq.SvcRq.ELCDTLFG = "1";  // draft.NegoDraftRegistry.申請種類;		//申請種類

            rspCode = null;
            bRetrial = false;

            bool bResult = txn.Commit();

            System.Text.StringBuilder sbRspCode = new System.Text.StringBuilder(txn.RspCode);

            if (!bResult)
            {
                sbRspCode.Append(":").Append(this.ReadErrCodeDis(txn));
                bRetrial = "3001".Equals(txn.RspCode);
            }

            if (!bResult && sbRspCode.Length > 0)
            {
                if (bRetrial)
                    sbRspCode.Append(",本筆已轉待註記");
            }

            rspCode = sbRspCode.ToString();
            return bResult;
        }

        private bool doL2200EAI(NegoLoanRepayment repayment, EAI.Service.Transaction.LR006_Rs.IFX rs, System.Text.StringBuilder response, out bool bRetrial, out Txn_L2200 txn, bool forCreditInsurance = false)
        {
            NegoLoan loan = repayment.NegoLoan;
            CreditInsuranceLoan insuranceLoan = loan.CreditInsuranceLoan;

            txn = new Txn_L2200();
            //帶入參數
            txn.Rq.EAIBody.MsgRq.SvcRq.KINBR = loan.Reimbursement.NegoDraft.LetterOfCreditVersion.LetterOfCredit?.CreditApplicationDocumentary.開狀行 ?? loan.Reimbursement.NegoDraft.NegoLC?.IssuingBank;	//輸入行
            txn.Rq.EAIBody.MsgRq.SvcRq.RBRNO = txn.Rq.EAIBody.MsgRq.SvcRq.KINBR;
            txn.Rq.EAIBody.MsgRq.SvcRq.ACBRNO = txn.Rq.EAIBody.MsgRq.SvcRq.KINBR;
            txn.Rq.EAIBody.MsgRq.SvcRq.INTFLG = "1";
            txn.Rq.EAIBody.MsgRq.SvcRq.DFCD = "1";
            txn.Rq.EAIBody.MsgRq.SvcRq.EAMTCD = "1";
            txn.Rq.EAIBody.MsgRq.SvcRq.DLCD = "1";
            txn.Rq.EAIBody.MsgRq.SvcRq.HCODE = "0";				//更正記號

            decimal interestAmt = 0;
            if (rs?.EAIBody?.MsgRs?.SvcRs?.INTAMT != null)
            {
                interestAmt = decimal.Parse(rs.EAIBody.MsgRs.SvcRs.INTAMT);
                txn.Rq.EAIBody.MsgRq.SvcRq.INTAMT = $"{interestAmt:0.}";
                txn.Rq.EAIBody.MsgRq.SvcRq.RINTAMT = txn.Rq.EAIBody.MsgRq.SvcRq.INTAMT;
            }

            decimal DIAMT = 0;
            if (rs?.EAIBody?.MsgRs?.SvcRs?.DIAMT != null)
            {
                DIAMT = decimal.Parse(rs.EAIBody.MsgRs.SvcRs.DIAMT);
                txn.Rq.EAIBody.MsgRq.SvcRq.INTAMT = $"{interestAmt + DIAMT:0.}";
                txn.Rq.EAIBody.MsgRq.SvcRq.RINTAMT = txn.Rq.EAIBody.MsgRq.SvcRq.INTAMT;
            }

            decimal repaymentAmt;
            if (forCreditInsurance)
            {
                repaymentAmt = Math.Round((repayment.RepaymentAmount * insuranceLoan.LoanPercentage / 100) ?? 0);
                txn.Rq.EAIBody.MsgRq.SvcRq.ACTNO = insuranceLoan.ACTNO;
            }
            else
            {
                repaymentAmt = Math.Round((repayment.RepaymentAmount * (100 - (insuranceLoan?.LoanPercentage ?? 0)) / 100) ?? 0);
                txn.Rq.EAIBody.MsgRq.SvcRq.ACTNO = loan.ACTNO;
            }

            txn.Rq.EAIBody.MsgRq.SvcRq.TXAMT = $"{repaymentAmt + interestAmt:0.}";
            txn.Rq.EAIBody.MsgRq.SvcRq.PRIAMT = $"{repaymentAmt}";

            txn.Rq.EAIBody.MsgRq.SvcRq.VLDATE = $"{repayment.RepaymentDate:yyyy-MM-dd}";

            txn.Rq.EAIBody.MsgRq.SvcRq.CIFKEY = loan.Reimbursement.NegoDraft.LetterOfCreditVersion.LetterOfCredit?.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo ?? loan.Reimbursement.NegoDraft.NegoLC?.CustomerOfBranch.Organization.ReceiptNo;
            txn.Rq.EAIBody.MsgRq.SvcRq.TXTYPE = $"{(int)PaymentNotification.TransferenceType.連動轉}"; //loan.PaymentNotification.現轉別.GetEfficientString(0, 1);
            txn.Rq.EAIBody.MsgRq.SvcRq.PAYACTNO = loan.Reimbursement.PayableAccount.Replace("-", "");
            bRetrial = false;

            bool bResult = txn.Commit();

            if (!bResult)
            {
                response.Append(txn.RspCode)
                    .Append(":").Append(this.ReadErrCodeDis(txn));
                bRetrial = "3001".Equals(txn.RspCode);
                if (bRetrial)
                    response.Append(",本筆已轉待註記");
            }

            return bResult;
        }

        private bool doL4200EAI(NegoLoanRepayment repayment, EAI.Service.Transaction.LR006_Rs.IFX rs, System.Text.StringBuilder response, out bool bRetrial, out Txn_L4200 txn, bool forCreditInsurance = false)
        {
            NegoLoan loan = repayment.NegoLoan;
            CreditInsuranceLoan insuranceLoan = loan.CreditInsuranceLoan;

            txn = new Txn_L4200();
            //帶入參數
            txn.Rq.EAIBody.MsgRq.SvcRq.KINBR = loan.Reimbursement.NegoDraft.LetterOfCreditVersion.LetterOfCredit?.CreditApplicationDocumentary.開狀行 ?? loan.Reimbursement.NegoDraft.NegoLC?.IssuingBank; ;	//輸入行
            txn.Rq.EAIBody.MsgRq.SvcRq.ACBRNO = txn.Rq.EAIBody.MsgRq.SvcRq.KINBR;
            txn.Rq.EAIBody.MsgRq.SvcRq.HCODE = "0";				//更正記號
            txn.Rq.EAIBody.MsgRq.SvcRq.DFCD = "1";
            txn.Rq.EAIBody.MsgRq.SvcRq.EAMTCD = "1";
            txn.Rq.EAIBody.MsgRq.SvcRq.DLCD = "1";
            txn.Rq.EAIBody.MsgRq.SvcRq.CANCEL = "2";
            txn.Rq.EAIBody.MsgRq.SvcRq.BLRECFG = "3";

            decimal interestAmt = 0;
            if (rs?.EAIBody?.MsgRs?.SvcRs?.INTAMT != null)
            {
                interestAmt = decimal.Parse(rs.EAIBody.MsgRs.SvcRs.INTAMT);
                txn.Rq.EAIBody.MsgRq.SvcRq.INTAMT = $"{interestAmt:0.}";
            }

            decimal DIAMT = 0;
            if (rs?.EAIBody?.MsgRs?.SvcRs?.DIAMT != null)
            {
                DIAMT = decimal.Parse(rs.EAIBody.MsgRs.SvcRs.DIAMT);
                txn.Rq.EAIBody.MsgRq.SvcRq.INTAMT = $"{interestAmt + DIAMT:0.}";
            }

            decimal repaymentAmt;
            if (forCreditInsurance)
            {
                txn.Rq.EAIBody.MsgRq.SvcRq.ACTNO = insuranceLoan.ACTNO;
                //repaymentAmt = Math.Round((repayment.RepaymentAmount * insuranceLoan?.LoanPercentage / 100) ?? 0);
                repaymentAmt = insuranceLoan.ACTBAL ?? 0;
            }
            else
            {
                txn.Rq.EAIBody.MsgRq.SvcRq.ACTNO = loan.ACTNO;
                //repaymentAmt = Math.Round((repayment.RepaymentAmount * (100 - (insuranceLoan?.LoanPercentage ?? 0)) / 100) ?? 0);
                repaymentAmt = loan.ACTBAL ?? 0;
            }


            txn.Rq.EAIBody.MsgRq.SvcRq.RECAMT = $"{repaymentAmt + interestAmt:0.}";
            txn.Rq.EAIBody.MsgRq.SvcRq.PRIAMT = $"{repaymentAmt}";

            txn.Rq.EAIBody.MsgRq.SvcRq.VLDATE = $"{repayment.RepaymentDate:yyyy-MM-dd}";

            txn.Rq.EAIBody.MsgRq.SvcRq.CIFKEY = loan.Reimbursement.NegoDraft.LetterOfCreditVersion.LetterOfCredit?.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo ?? loan.Reimbursement.NegoDraft.NegoLC?.CustomerOfBranch.Organization.ReceiptNo;
            txn.Rq.EAIBody.MsgRq.SvcRq.TXTYPE = loan.PaymentNotification.現轉別.GetEfficientString(0, 1);
            txn.Rq.EAIBody.MsgRq.SvcRq.PAYACTNO = loan.Reimbursement.PayableAccount.Replace("-", "");
            bRetrial = false;

            if (repaymentAmt <= 0 && interestAmt <= 0)
            {
                return true;
            }

            bool bResult = txn.Commit();

            if (!bResult)
            {
                response
                    .Append(txn.RspCode)
                    .Append(":").Append(this.ReadErrCodeDis(txn));
                bRetrial = "3001".Equals(txn.RspCode);
                if (bRetrial)
                    response.Append(",本筆已轉待註記");
            }

            return bResult;
        }


        public bool AllowNegoDraftWithEAI(int? draftID, out string rspCode)
        {
            rspCode = null;
            using (LcEntityManager<NegoDraft> mgr = new LcEntityManager<NegoDraft>())
            {
                var item = mgr.EntityList.Where(n => n.DraftID == draftID).FirstOrDefault();
                return AllowNegoDraftWithEAI(item, out rspCode, mgr);
            }
        }

        public bool AllowNegoDraftWithEAI(NegoDraft item, out string rspCode, GenericManager<LcEntityDataContext> mgr = null)
        {
            if (mgr == null)
            {
                mgr = this;
            }

            rspCode = null;
            bool bResult = false;
            bool bRetrial;
            if (item != null)
            {
                if (item.AtSight())
                {
                    Txn_L4500 txn;
                    bResult = doNegoL4500EAI(item, out rspCode, out bRetrial, out txn);
                    if (bResult)
                    {
                        item.貸放支號 = txn.Rs.EAIBody.MsgRs.SvcRs.COUNT;
                        mgr.SubmitChanges();
                    }
                }
                else
                {
                    Txn_L1201 txn;
                    bResult = doNegoL1201EAI(item, out rspCode, out bRetrial, out txn);
                    if (bResult)
                    {
                        item.貸放支號 = txn.Rs.EAIBody.MsgRs.SvcRs.ACTNO + txn.Rs.EAIBody.MsgRs.SvcRs.SQNO;
                        mgr.SubmitChanges();
                    }
                }

                if (!bResult && bRetrial)
                {
                    mgr.DataContext.AbortNegoApplication(item.DraftID, UserProfile.UserName);
                }
                return bResult;

            }
            else
            {
                rspCode = "匯票資料不存在!!";
                return false;
            }
        }


        public bool CancelLcWithEAI(CreditCancellation item,out string rspCode)
        {
            bool bResult = true;
            bool bRetrial = false;
            System.Text.StringBuilder sbRspCode = new System.Text.StringBuilder();

            rspCode = null;

            Txn_L4500 l4500 = null;

            if (item != null)
            {

                l4500 = new Txn_L4500();
                l4500.KINBR = UserProfile.UserProfileRow.branch;
                l4500.HCODE = "0";
                l4500.ACTNO = item.LetterOfCredit.LcNo;
                l4500.CIFKEY = item.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo;
                l4500.CAUSE = !String.IsNullOrEmpty(item.CancellationRegistry.沖銷原因) ? item.CancellationRegistry.沖銷原因.Substring(0, 2) : null;
                l4500.TXAMT = String.Format("{0:0}", item.LetterOfCredit.可用餘額);
                l4500.GRAMT = item.CancellationRegistry.沖銷存入保證金金額.ToString();
                l4500.BPBKNO = item.LetterOfCredit.CreditApplicationDocumentary.通知行;
                l4500.TXTYPE = !String.IsNullOrEmpty(item.CancellationRegistry.撥款方式) ? item.CancellationRegistry.撥款方式.Substring(0, 1) : null;
                l4500.PACTNO = !String.IsNullOrEmpty(item.CancellationRegistry.入戶帳號) ? item.CancellationRegistry.入戶帳號.Replace("-", "") : null;
                l4500.GTXNO = item.LetterOfCredit.CreditApplicationDocumentary.OpeningApplicationDocumentary.交易流水號.ToString();
                l4500.VRATE = "";

                bResult = l4500.Commit();
                if (!bResult)
                {
                    sbRspCode.Append(l4500.RspCode).Append(":").Append(this.ReadErrCodeDis(l4500));
                    bRetrial = "3001".Equals(l4500.RspCode);
                }

            }

            if (bRetrial)
            {
                this.Context.AbortTransaction(item.CancellationID, UserProfile.UserProfileRow.pid, (int)Naming.DocumentLevel.待註記);
            }

            if (!bResult && sbRspCode.Length > 0)
            {
                if (bRetrial)
                    sbRspCode.Append(",本筆已轉待註記");
                rspCode = sbRspCode.ToString();
            }

            return bResult;

        }


        public bool CancelLcWithEAI(int cancellationID, out string rspCode, out CreditCancellation item)
        {
            bool bResult = false;
            rspCode = null;
            item = this.GetTable<CreditCancellation>().Where(c => c.CancellationID == cancellationID).FirstOrDefault();
            if (item != null)
            {
                bResult = CancelLcWithEAI(item, out rspCode);
            }

            return bResult;
        }

        public eAuth QueryTaskFlowSchema(string taskID)
        {
            if (CHBAuthorityService.QueryTaskFlowSchema != null)
            {
                return CHBAuthorityService.QueryTaskFlowSchema(UserProfile, taskID);
            }
            else
            {
                string url = new System.Text.StringBuilder(Settings.Default.AAUrl)
                    .Append("?uid=").Append(UserProfile.Auth.Company)
                    .Append("&uuid=").Append(UserProfile.PID)
                    .Append("&type=schemas&taskID=").Append(taskID).ToString();
                return getPortalAuth(url);
            }
        }

        public eAuth QueryAssumeNextUsers(string stepID)
        {
            if (CHBAuthorityService.QueryAssumeNextUsers != null)
            {
                return CHBAuthorityService.QueryAssumeNextUsers(UserProfile, stepID);
            }
            else
            {
                string url = new System.Text.StringBuilder(Settings.Default.AAUrl)
                    .Append("?uid=").Append(UserProfile.Auth.Company)
                    .Append("&uuid=").Append(UserProfile.PID)
                    .Append("&type=next&stepID=").Append(stepID).ToString();
                return getPortalAuth(url);
            }
        }


        private eAuth getPortalAuth(string url)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(url);
                return doc.ConvertTo<eAuth>();
            }
            catch (Exception ex)
            {
                ModelCore.Helper.Logger.Error("無法讀取CHB授權中心 => " + url + "\r\n" + ex);
            }
            return new eAuth { };
        }


        public eAuth CheckCertificate(XmlDocument doc)
        {
            //string uri = String.Format("{0}?uid={1}&uuid={2}",
            //    Settings.Default.CheckCertificateUrl, UserProfile.Auth.Company, UserProfile.PID);
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            //request.Method = "POST";
            //Stream output = request.GetRequestStream();
            //XmlTextWriter xtw = new XmlTextWriter(output, null);
            //doc.WriteTo(xtw);
            //xtw.Flush();
            //xtw.Close();

            //output.Flush();

            //XmlDocument authDoc = new XmlDocument();
            //authDoc.Load(request.GetResponse().GetResponseStream());
            //return authDoc.ConvertTo<eAuth>();

            return new eAuth
            {
                userSession = new eAuthUserSession
                {
                    isCertAuthorized = true,
                    isCertAuthorizedSpecified = true,
                }
            };
        }

        public bool AllowDraftAcceptanceWithEAI(NegoDraftAcceptance item, out string rspCode)
        {
            rspCode = null;

            bool bResult = false;
            bool bRetrial;
            StringBuilder msg = new StringBuilder();

            if (item != null)
            {

                Txn_L4700 l4700;
                bResult = doL4700EAI(item, msg, out bRetrial, out l4700);

                 if (!bResult && bRetrial)
                {
                    this.Context.AbortTransaction(item.AcceptanceID, UserProfile.UserProfileRow.pid, (int)Naming.DocumentLevel.待註記);
                }

                rspCode = msg.ToString();
                return bResult;
            }
            else
            {
                rspCode = "承兌匯票到期付款資料不存在!!";
                return false;
            }

        }


        public bool AllowReimbursementWithEAI(Reimbursement item, out string cause)
        {
            cause = null;

            bool bResult = false;
            bool bRetrial;
            StringBuilder msg = new StringBuilder();

            if (item != null)
            {
                if (item.NegoDraft.AtSight())
                {
                    bResult = SendL8600EAI(item, msg, out bRetrial);
                }
                else
                {
                    Txn_L4600 l4600;
                    bResult = SendL4600EAI(item, msg, out bRetrial, out l4600);
                }

                if(bResult)
                {
                    if(item.ImposeHandlingCharge>0)
                    {
                        Txn_L2300 l2300;
                        this.doL2300EAI(item, msg, out bRetrial, out l2300);
                    }
                }
                else if (!bResult && bRetrial)
                {
                    this.Context.AbortTransaction(item.ReimID, UserProfile.UserProfileRow.pid, (int)Naming.DocumentLevel.待註記);
                }

                cause = msg.ToString();
                return bResult;
            }
            else
            {
                cause = "還款資料不存在!!";
                return false;
            }

        }

        public bool AllowNegoLoanWithEAI(Reimbursement item, out string rspCode)
        {
            rspCode = null;
            bool bResult = false;
            bool bRetrial = false;
            StringBuilder msg = new StringBuilder();

            if (item != null)
            {
                if (item.TxnCode != (int)Naming.DocumentLevel.已開立)
                {
                    if (item.NegoDraft.AtSight())
                    {
                        bResult = SendL8600EAI(item, msg, out bRetrial);
                    }
                    else
                    {
                        Txn_L4600 l4600;
                        bResult = SendL4600EAI(item, msg, out bRetrial, out l4600);
                    }

                    if (bResult)
                    {
                        item.TxnCode = (int)Naming.DocumentLevel.已開立;
                        this.SubmitChanges();
                    }
                }
                else
                {
                    bResult = true;
                }


                if (bResult)
                {
                    NegoLoan loanItem = item.NegoLoan;
                    if (loanItem.TxnCode != (int)Naming.DocumentLevel.已開立)
                    {
                        Txn_L1000 txn;

                        if (item.NegoLoan.CreditInsuranceLoan?.LoanPercentage >= 100m)
                        {
                            loanItem.TxnCode = (int)Naming.DocumentLevel.已開立;
                            this.SubmitChanges();
                        }
                        else
                        {
                            bResult = SendL1000EAI(item.NegoLoan, msg, out bRetrial, out txn);

                            if (bResult)
                            {
                                loanItem.TxnCode = (int)Naming.DocumentLevel.已開立;
                                this.SubmitChanges();

                                loanItem.ACTNO = txn.Rs.EAIBody.MsgRs.SvcRs.ACTNO;
                                if (item.NegoLoan.CreditInsuranceLoan != null && item.NegoLoan.CreditInsuranceLoan.L1000ID.HasValue)
                                {
                                    item.NegoLoan.CreditInsuranceLoan.L1000.送信保對應帳支號 = txn.Rs.EAIBody.MsgRs.SvcRs.ACTNO;
                                }
                                this.SubmitChanges();
                            }
                        }
                    }
                }

                if (bResult && item.NegoLoan.CreditInsuranceLoan != null)
                {
                    Txn_L1000 txn;
                    bResult = SendL1000EAI(item.NegoLoan, msg, out bRetrial, out txn, true);

                    if (bResult)
                    {
                        item.NegoLoan.CreditInsuranceLoan.ACTNO = txn.Rs.EAIBody.MsgRs.SvcRs.ACTNO;
                        item.NegoLoan.CreditInsuranceLoan.MGNO = txn.Rq.EAIBody.MsgRq.SvcRq.MGNO;
                        this.SubmitChanges();
                    }
                }


                if (!bResult && bRetrial)
                {
                    this.Context.AbortTransaction(item.ReimID, UserProfile.UserProfileRow.pid, (int)Naming.DocumentLevel.待註記);
                }

                rspCode = msg.ToString();
                return bResult;
            }
            else
            {
                rspCode = "還款改貸資料不存在!!";
                return false;
            }

        }

        public bool AllowNegoLoanRepaymentWithEAI(NegoLoanRepayment item, out string rspCode)
        {
            bool bResult = false;
            bool bRetrial = false;

            StringBuilder response = new StringBuilder();

            if (item != null)
            {
                NegoLoan loan = item.NegoLoan;
                CreditInsuranceLoan insuranceLoan = loan.CreditInsuranceLoan;
                bool repayWhole = item.RepaymentAmount >= (loan.Reimbursement.Amount + item.InterestAmount) || item.RepaymentAmount >= (loan.BalanceAmount + item.InterestAmount);

                var rsLR006 = item.LoadLR006_Rs(out EAI.Service.Transaction.LR006_Rs.IFX insuredRs);
                if (item.TxnCode != (int)Naming.DocumentLevel.已開立)
                {
                    if (repayWhole)
                    {
                        bResult = doL4200EAI(item, rsLR006, response, out bRetrial, out Txn_L4200 txn);
                    }
                    else
                    {
                        bResult = doL2200EAI(item, rsLR006, response, out bRetrial, out Txn_L2200 txn);
                    }

                    if (bResult)
                    {
                        item.TxnCode = (int)Naming.DocumentLevel.已開立;
                        this.SubmitChanges();
                    }
                }
                else
                {
                    bResult = true;
                }

                if (bResult && insuranceLoan != null)
                {
                    if (repayWhole)
                    {
                        bResult = doL4200EAI(item, insuredRs, response, out bRetrial, out Txn_L4200 txn, true);
                    }
                    else
                    {
                        bResult = doL2200EAI(item, insuredRs, response, out bRetrial, out Txn_L2200 txn, true);
                    }
                }


                if (!bResult && bRetrial)
                {
                    this.Context.AbortTransaction(item.RepaymentID, UserProfile.UserProfileRow.pid, (int)Naming.DocumentLevel.待註記);
                }

                rspCode = response.ToString();
                return bResult;
            }
            else
            {
                rspCode = "改貸還款資料不存在!!";
                return false;
            }

        }

        public bool SendL8600EAI(Reimbursement item, StringBuilder msg, out bool bRetrial)
        {

            Txn_L8600 txn = new Txn_L8600();

            txn.Rq.EAIBody.MsgRq.SvcRq.SEQFG = "1";//	SEQFG	編列序號記號, 9(01)
            txn.Rq.EAIBody.MsgRq.SvcRq.KINBR = UserProfile.UserProfileRow.branch;//	KINBR	輸入行, 9(04)
            txn.Rq.EAIBody.MsgRq.SvcRq.HCODE = "0";//	HCODE	更正記號, 9(01)
            //	ECKIN	更正輸入行別, 9(04)
            //	ECTRM	更正櫃檯機序號, 9(04)
            //	ECTNO	更正交易序號, 9(08)
            txn.Rq.EAIBody.MsgRq.SvcRq.ACTNO = item.NegoDraft.LcID.HasValue ? item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.LcNo : item.NegoDraft.NegoLC.LCNo.Replace("-","");//	ACTNO	帳號, X(16)
            txn.Rq.EAIBody.MsgRq.SvcRq.CIFKEY = item.NegoDraft.LcID.HasValue ? item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo
                : item.NegoDraft.NegoLC.CustomerOfBranch.Organization.ReceiptNo; //	CIFKEY	統一編號, X(10)
            //	CIFERR	統一編號錯誤代碼, X(1)
            txn.Rq.EAIBody.MsgRq.SvcRq.CAUSE = !String.IsNullOrEmpty(item.L8600.沖銷原因) ? item.L8600.沖銷原因.Substring(0,2) : ""; //	CAUSE	沖銷原因, 9(02)
            txn.Rq.EAIBody.MsgRq.SvcRq.TXAMT = item.L8600.沖銷信用狀金額.ToString();   //	TXAMT	沖銷金額, 9(12)V99
            txn.Rq.EAIBody.MsgRq.SvcRq.GRAMT = item.L8600.沖銷存入保證金金額.ToString(); //	GRAMT	沖銷存入保證金金額, 9(12)V99
            txn.Rq.EAIBody.MsgRq.SvcRq.VDATE = String.Format("{0:yyyy-MM-dd}", item.L8600.墊款日); //	VDATE	墊款日, (YYYY-MM-DD)
            txn.Rq.EAIBody.MsgRq.SvcRq.VRATE = item.L8600.墊款利率.ToString();   //	VRATE	墊款利率, 9(02)V9(05)
            txn.Rq.EAIBody.MsgRq.SvcRq.VINT = item.L8600.墊款息金額.ToString();  //	VINT	墊款息金額, 9(12)V99
            txn.Rq.EAIBody.MsgRq.SvcRq.DFCD = !String.IsNullOrEmpty(item.L8600.違約金計收方式) ? item.L8600.違約金計收方式.Substring(0, 1) : "";    //	DFCD	違約金計收方式, 9(1)
            txn.Rq.EAIBody.MsgRq.SvcRq.DFAMT = item.L8600.違約金金額.ToString(); //	DFAMT	違約金金額, 9(12)V99
            //	SVEFEE	改貸手續費金額, 9(12)V99
            txn.Rq.EAIBody.MsgRq.SvcRq.TXTYPE = !String.IsNullOrEmpty(item.L8600.現轉別) ? item.L8600.現轉別.Substring(0, 1) : ""; //	TXTYPE	現轉別, 9(01)
            txn.Rq.EAIBody.MsgRq.SvcRq.PAYACTNO = !String.IsNullOrEmpty(item.L8600.扣款帳號) ? item.L8600.扣款帳號.Replace("-","") : "";  //	PAYACTNO	扣款帳號, 9(14)
            txn.Rq.EAIBody.MsgRq.SvcRq.CKAMT = item.L8600.票繳金額.ToString();  //	CKAMT	票繳金額, 9(12)V99
            txn.Rq.EAIBody.MsgRq.SvcRq.CKNO = item.L8600.支票號碼;  //	CKNO	支票號碼, 9(10)
            txn.Rq.EAIBody.MsgRq.SvcRq.CKBKNO7 = item.L8600.付款行;    //	CKBKNO7	付款行, 9(07)
            txn.Rq.EAIBody.MsgRq.SvcRq.CKACTNO = item.L8600.出票人帳號;//	CKACTNO	出票人帳號, 9(16)
            txn.Rq.EAIBody.MsgRq.SvcRq.CLSNOTE = !String.IsNullOrEmpty(item.L8600.不良債權處理註記) ? item.L8600.不良債權處理註記.Substring(0, 3) : ""; //	CLSNOTE	不良債權處理註記, X(3)
            txn.Rq.EAIBody.MsgRq.SvcRq.COUNT = item.L8600.墊款流水號.ToString();
            txn.Rq.EAIBody.MsgRq.SvcRq.RECAMT = item.L8600.實收總金額.ToString();

            bRetrial = false;

            bool bResult = txn.Commit();

            if (!bResult)
            {
                msg.Append("L8600交易失敗:")
                    .Append(this.ReadErrCodeDis(txn));
                bRetrial = "3001".Equals(txn.RspCode);

                if(bRetrial)
                    msg.Append(",本筆已轉待註記");
            }

            return bResult;

        }

        public bool SendL4600EAI(Reimbursement item, StringBuilder msg, out bool bRetrial, out Txn_L4600 txn)
        {
            txn = new Txn_L4600();

            txn.Rq.EAIBody.MsgRq.SvcRq.SEQFG = "1";//	SEQFG	編列序號記號, 9(01)
            txn.Rq.EAIBody.MsgRq.SvcRq.KINBR = UserProfile.UserProfileRow.branch;//	KINBR	輸入行, 9(04)
            txn.Rq.EAIBody.MsgRq.SvcRq.HCODE = "0";//	HCODE	更正記號, 9(01)
            txn.Rq.EAIBody.MsgRq.SvcRq.ACTNO = item.L4600.ACTNO;//	ACTNO	帳號, X(16)
            txn.Rq.EAIBody.MsgRq.SvcRq.CURCD = "00";
            txn.Rq.EAIBody.MsgRq.SvcRq.TXAMT = item.Amount.ToString();
            //	ECKIN	更正輸入行別, 9(04)
            //	ECTRM	更正櫃檯機序號, 9(04)
            //	ECTNO	更正交易序號, 9(08)
            txn.Rq.EAIBody.MsgRq.SvcRq.RBRNO = item.NegoDraft.LcID.HasValue ? item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.OpeningApplicationDocumentary.實績行 : item.NegoDraft.NegoLC.IssuingBank;	//實績行
            txn.Rq.EAIBody.MsgRq.SvcRq.CIFKEY = item.NegoDraft.LcID.HasValue ? item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo
                : item.NegoDraft.NegoLC.CustomerOfBranch.Organization.ReceiptNo; //	CIFKEY	統一編號, X(10)
            //	CIFERR	統一編號錯誤代碼, X(1)

            txn.Rq.EAIBody.MsgRq.SvcRq.GRBAL = item.L4600.抵存入保證金金額.ToString(); //	<GRBAL>抵存入保證金金額, 9(12)V99</GRBAL>
            txn.Rq.EAIBody.MsgRq.SvcRq.VRATE = item.L4600.墊款利率.ToString();   //	VRATE	墊款利率, 9(02)V9(05)
            txn.Rq.EAIBody.MsgRq.SvcRq.VINT = item.L4600.墊款息金額.ToString();  //	VINT	墊款息金額, 9(12)V99
            txn.Rq.EAIBody.MsgRq.SvcRq.CAUSE = !String.IsNullOrEmpty(item.L4600.還款原因) ? item.L4600.還款原因.Substring(0, 2) : null; //	CAUSE	還款原因, 9(02)
            txn.Rq.EAIBody.MsgRq.SvcRq.DFCD = !String.IsNullOrEmpty(item.L4600.違約金計收方式) ? item.L4600.違約金計收方式.Substring(0, 1) : null;    //	DFCD	違約金計收方式, 9(1)
            txn.Rq.EAIBody.MsgRq.SvcRq.DFAMT = item.L4600.違約金金額.ToString(); //	DFAMT	違約金金額, 9(12)V99
            txn.Rq.EAIBody.MsgRq.SvcRq.GTXNO = item.NegoDraft.AppNo(); //	<GTXNO>匯票號碼, 9(10)</GTXNO>
            txn.Rq.EAIBody.MsgRq.SvcRq.TXTYPE = !String.IsNullOrEmpty(item.L4600.現轉別) ? item.L4600.現轉別.Substring(0, 1) : null; //	TXTYPE	現轉別, 9(01)
            txn.Rq.EAIBody.MsgRq.SvcRq.PACTNO = !String.IsNullOrEmpty(item.L4600.扣款帳號) ? item.L4600.扣款帳號.Replace("-","") : null;  //	PAYACTNO	扣款帳號, 9(14)
            txn.Rq.EAIBody.MsgRq.SvcRq.CKAMT = item.L4600.票繳金額.ToString();  //	CKAMT	票繳金額, 9(12)V99
            txn.Rq.EAIBody.MsgRq.SvcRq.CKNO = item.L4600.支票號碼;  //	CKNO	支票號碼, 9(10)
            txn.Rq.EAIBody.MsgRq.SvcRq.CKBKNO7 = "";    //	CKBKNO7	付款行, 9(07)
            txn.Rq.EAIBody.MsgRq.SvcRq.CKACTNO = item.L4600.出票人帳號;//	CKACTNO	發票人帳號, 9(16)
            txn.Rq.EAIBody.MsgRq.SvcRq.CLSNOTE = !String.IsNullOrEmpty(item.L4600.不良債權處理註記) ? item.L4600.不良債權處理註記.Substring(0, 2) : null; //	CLSNOTE	不良債權處理註記, X(3)

            bRetrial = false;

            bool bResult = txn.Commit();

            if (!bResult)
            {
                msg.Append("L4600交易失敗:")
                    .Append(this.ReadErrCodeDis(txn));
                bRetrial = "3001".Equals(txn.RspCode);

                if (bRetrial)
                    msg.Append(",本筆已轉待註記");
            }

            return bResult;

        }

        private bool doL4700EAI(NegoDraftAcceptance item, StringBuilder msg, out bool bRetrial,out Txn_L4700 txn)
        {
            txn = new Txn_L4700();

            txn.Rq.EAIBody.MsgRq.SvcRq.SEQFG = "1";//	SEQFG	編列序號記號, 9(01)
            txn.Rq.EAIBody.MsgRq.SvcRq.KINBR = UserProfile.UserProfileRow.branch;//	KINBR	輸入行, 9(04)
            txn.Rq.EAIBody.MsgRq.SvcRq.ACTNO = item.L4700.ACTNO;
            txn.Rq.EAIBody.MsgRq.SvcRq.HCODE = "0";//	HCODE	更正記號, 9(01)
            txn.Rq.EAIBody.MsgRq.SvcRq.CURCD = String.Format("{0:00}", item.L4700.幣別);            //<CURCD>幣別,9(02)</CURCD>
            txn.Rq.EAIBody.MsgRq.SvcRq.TXAMT = item.L4700.承兌金額.ToString();  //<TXAMT>金額, 9(12)V99</TXAMT>
            //<ACFLG>配合金資軋帳記號, X(01)</ACFLG>
            //<!-- 
            //0   不配合金資軋帳記號。
            //1   配合金資軋帳記號，並不做換日。
            //2   配合金資軋帳記號，且由ITFS做換日。
            //-->
            //<ECKIN>更正輸入行別, 9(04)</ECKIN>
            //<ECTRM>更正櫃檯機序號, 9(04)</ECTRM>
            //<ECTNO>更正交易序號, 9(08)</ECTNO>
            txn.Rq.EAIBody.MsgRq.SvcRq.RBRNO = item.NegoDraft.LcID.HasValue
                ? item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.OpeningApplicationDocumentary.實績行
                : item.NegoDraft.NegoLC.IssuingBank;    //<RBRNO>實蹟行</RBRNO>
            txn.Rq.EAIBody.MsgRq.SvcRq.CIFKEY = txn.Rq.EAIBody.MsgRq.SvcRq.CIFKEY = item.NegoDraft.LcID.HasValue ? item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo
                : item.NegoDraft.NegoLC.CustomerOfBranch.Organization.ReceiptNo;  //<CIFKEY>統一編號, X(10)</CIFKEY>
            //<CIFERR>統一編號錯誤代碼, X(1)</CIFERR>
            //<!-- IT-TITA-TEXT -->
            txn.Rq.EAIBody.MsgRq.SvcRq.APTYPE = !String.IsNullOrEmpty(item.L4700.撥款方式) ? item.L4700.撥款方式.Substring(0, 1)
                : null; //<APTYPE>撥款方式,9(01)</APTYPE>
            txn.Rq.EAIBody.MsgRq.SvcRq.INACTNO = item.L4700.入戶帳號.Replace("-","");   //<INACTNO>入戶帳號,9(14)</INACTNO>
            txn.Rq.EAIBody.MsgRq.SvcRq.GTXNO = item.NegoDraft.AppNo();  //<GTXNO>匯票號碼,9(10)</GTXNO>
            txn.Rq.EAIBody.MsgRq.SvcRq.VRATE = item.L4700.墊款利率.ToString();  //<VRATE>墊款利率,9(02)V9(05)</VRATE>
            txn.Rq.EAIBody.MsgRq.SvcRq.IRTKD = !String.IsNullOrEmpty(item.L4700.利率種類) ? item.L4700.利率種類.Substring(0, 2)
                : null; //<IRTKD>利率種類,9(02)</IRTKD>
            txn.Rq.EAIBody.MsgRq.SvcRq.RATECD = !String.IsNullOrEmpty(item.L4700.利率型態) ? item.L4700.利率型態.Substring(0, 2)
                : null;//<RATECD>利率型態,9(02)</RATECD>
            txn.Rq.EAIBody.MsgRq.SvcRq.IRTCD = !String.IsNullOrEmpty(item.L4700.利率別) ? item.L4700.利率別.Substring(0, 4)
                : null; //<IRTCD>利率別,9(04)</IRTCD>
            txn.Rq.EAIBody.MsgRq.SvcRq.FITIRT = item.L4700.承作利率.ToString(); //<FITIRT>承作利率,9(02)V9(05)</FITIRT>
            txn.Rq.EAIBody.MsgRq.SvcRq.SIGN = item.L4700.加減碼.HasValue
                ? item.L4700.加減碼 >= 0 ? "+" : "-"
                : null;//<SIGN>加減碼正負號,X(01)</SIGN>
            txn.Rq.EAIBody.MsgRq.SvcRq.IXPR = item.L4700.加減碼.ToString();    //<IXPR>加減碼,9(02)V9(05)</IXPR>            

            bRetrial = false;

            bool bResult = txn.Commit();


            if (!bResult)
            {
                msg.Append("L4700交易失敗:")
                    .Append(this.ReadErrCodeDis(txn));
                bRetrial = "3001".Equals(txn.RspCode);

                if (bRetrial)
                    msg.Append(",本筆已轉待註記");
            }

            return bResult;

        }

        private bool doL2300EAI(Reimbursement item, StringBuilder msg, out bool bRetrial, out Txn_L2300 txn)
        {
            txn = new Txn_L2300();
            txn.Rq.EAIBody.MsgRq.SvcRq.KINBR = UserProfile.UserProfileRow.branch;//	KINBR	輸入行, 9(04)
            txn.Rq.EAIBody.MsgRq.SvcRq.HCODE = "0";//	HCODE	更正記號, 9(01)
            txn.Rq.EAIBody.MsgRq.SvcRq.ECKIN = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.ECTRM = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.ECTNO = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.ACTNO = item.NegoDraft.ACTNO();//	ACTNO	帳號, X(16)
            txn.Rq.EAIBody.MsgRq.SvcRq.CIFKEY = item.NegoDraft.LcID.HasValue ? item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo
                : item.NegoDraft.NegoLC.CustomerOfBranch.Organization.ReceiptNo; //	CIFKEY	統一編號, X(10)
            //txn.Rq.EAIBody.MsgRq.SvcRq.CAUSE = !String.IsNullOrEmpty(item.L4600.還款原因) ? item.L4600.還款原因.Substring(0, 2) : null; //	CAUSE	還款原因, 9(02)
            txn.Rq.EAIBody.MsgRq.SvcRq.CAUSE = "4"; //	CAUSE	補收原因, 9(01)
            txn.Rq.EAIBody.MsgRq.SvcRq.INTFLG = "08"; //  INTFLG  補收代碼, 9(02)
            txn.Rq.EAIBody.MsgRq.SvcRq.INTAMT = item.ImposeHandlingCharge.ToString();
            txn.Rq.EAIBody.MsgRq.SvcRq.SINTAMT = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.RSINTAMT = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.CREAMT = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.REFAMT = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.RREFAMT = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.RECAMT = item.ImposeHandlingCharge.ToString();
            txn.Rq.EAIBody.MsgRq.SvcRq.TXTYPE = item.L8600ID.HasValue
                ? (!String.IsNullOrEmpty(item.L8600.現轉別) ? item.L8600.現轉別.Substring(0, 1) : "")
                : !String.IsNullOrEmpty(item.L4600.現轉別) ? item.L4600.現轉別.Substring(0, 1) : ""; //	TXTYPE	現轉別, 9(01)
            txn.Rq.EAIBody.MsgRq.SvcRq.PAYACTNO = item.PayableAccount.Replace("-", "");
            txn.Rq.EAIBody.MsgRq.SvcRq.CKAMT = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.CKNO = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.CKBKNO = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.CKACTNO = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.TNINTAMT = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.IBRNO = item.NegoDraft.LcID.HasValue ? item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.通知行
                : item.NegoDraft.NegoLC.AdvisingBank;
            txn.Rq.EAIBody.MsgRq.SvcRq.COUNT = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.REASON1 = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.REASON2 = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.REASON3 = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.BLRECFG = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.UCIFKEY = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.FEEID = "";

            bRetrial = false;

            bool bResult = txn.Commit();

            if (!bResult)
            {
                msg.Append("L2300交易失敗:")
                    .Append(this.ReadErrCodeDis(txn));
                bRetrial = "3001".Equals(txn.RspCode);
                if (bRetrial)
                    msg.Append(",主機交易逾時");
            }

            return bResult;

        }


        public bool SendL1000EAI(NegoLoan item, StringBuilder msg, out bool bRetrial, out Txn_L1000 txn,bool forCreditInsurance = false)
        {
            txn = new Txn_L1000();

            L1000 dataItem = forCreditInsurance
                ? item.CreditInsuranceLoan.L1000
                : item.L1000;

            txn.Rq.EAIBody.MsgRq.SvcRq.KINBR = UserProfile.UserProfileRow.branch;	//輸入行, 9(04)-->
            txn.Rq.EAIBody.MsgRq.SvcRq.ACTNO = dataItem.ACTNO;	//帳支號, X(16)-->
            txn.Rq.EAIBody.MsgRq.SvcRq.HCODE = "0";	//更正記號, 9(01)-->
            //txn.Rq.EAIBody.MsgRq.SvcRq.ECKIN = "";	//更正輸入行別, 9(04)-->
            //txn.Rq.EAIBody.MsgRq.SvcRq.ECTRM = "";	//更正櫃檯機序號, 9(04)-->
            //txn.Rq.EAIBody.MsgRq.SvcRq.ECTNO = "";	//更正交易序號, 9(08)-->
            txn.Rq.EAIBody.MsgRq.SvcRq.RBRNO = item.Reimbursement.NegoDraft.LcID.HasValue ? item.Reimbursement.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.OpeningApplicationDocumentary.實績行 : item.Reimbursement.NegoDraft.NegoLC.IssuingBank;	//實績行, 9(04)-->
            txn.Rq.EAIBody.MsgRq.SvcRq.CIFKEY = item.Reimbursement.NegoDraft.LcID.HasValue ? item.Reimbursement.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo
                : item.Reimbursement.NegoDraft.NegoLC.CustomerOfBranch.Organization.ReceiptNo;	//統一編號, X(10)-->
            //txn.Rq.EAIBody.MsgRq.SvcRq.CIFERR = "";	//統一編號錯誤代碼, X(01)-->
            if(forCreditInsurance)
            {
                txn.Rq.EAIBody.MsgRq.SvcRq.TXAMT = (item.CreditInsuranceLoan.LoanAmount ?? 0).ToString();	//貸放金額, 9(12)V99-->
                txn.Rq.EAIBody.MsgRq.SvcRq.MGNO = item.Reimbursement.NegoDraft.LcID.HasValue 
                    ? item.Reimbursement.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.CustomerOfBranchExtension.InsuranceCreditNo
                    : item.Reimbursement.NegoDraft.NegoLC.CustomerOfBranch.CustomerOfBranchExtension.InsuranceCreditNo;	//額度號碼, 9(14)-->
            }
            else
            {
                txn.Rq.EAIBody.MsgRq.SvcRq.TXAMT = (item.Reimbursement.Amount - (item.CreditInsuranceLoan?.LoanAmount ?? 0)).ToString();	//貸放金額, 9(12)V99-->
                txn.Rq.EAIBody.MsgRq.SvcRq.MGNO = item.PaymentNotification.額度號碼.Replace("-", "");	//額度號碼, 9(14)-->
            }
            txn.Rq.EAIBody.MsgRq.SvcRq.ACNO = dataItem.會計科目;    //<ACNO>會計科目, X(04)</ACNO>
            txn.Rq.EAIBody.MsgRq.SvcRq.SBNO = dataItem.會計子目;  //<SBNO>會計子目, 9(03)</SBNO>
            txn.Rq.EAIBody.MsgRq.SvcRq.GUNO = item.PaymentNotification.交易憑證號碼;	//<GUNO>交易憑證號碼	 X(20)</GUNO>	
            txn.Rq.EAIBody.MsgRq.SvcRq.FUNCD = item.PaymentNotification.用途別.GetEfficientString(0, 1);	//<FUNCD>用途別	 9(02)</FUNCD>	
            var seg = item.PaymentNotification.帳戶性質.Split('-');
            txn.Rq.EAIBody.MsgRq.SvcRq.CHARCD1 = seg[0];	//<CHARCD1>帳戶性質1	 9(03)</CHARCD1>	
            txn.Rq.EAIBody.MsgRq.SvcRq.CHARCD2 = seg[1];	//<CHARCD2>帳戶性質2	 9(03)</CHARCD2>	
            txn.Rq.EAIBody.MsgRq.SvcRq.APSDAY = String.Format("{0:yyyyMMdd}", item.PaymentNotification.初放貸放日);	//<APSDAY>貸放起日	 9(08)</APSDAY>	
            txn.Rq.EAIBody.MsgRq.SvcRq.APEDAY = String.Format("{0:yyyyMMdd}", item.PaymentNotification.貸放到期日);	//<APEDAY>到期日	 9(08)</APEDAY>	
            txn.Rq.EAIBody.MsgRq.SvcRq.IMDAYS = item.PaymentNotification.貸放期限天.ToString();	//<IMDAYS>貸放天數	 9(03)</IMDAYS>	
            txn.Rq.EAIBody.MsgRq.SvcRq.IMMONS = item.PaymentNotification.貸放期限月.ToString();	//<IMMONS>貸放月數	 9(03)</IMMONS>	
            txn.Rq.EAIBody.MsgRq.SvcRq.PRCD = item.PaymentNotification.還本方式.GetEfficientString(0, 2);	//<PRCD>還本方式	 9(02)</PRCD>	
            txn.Rq.EAIBody.MsgRq.SvcRq.PRRATE = dataItem.最後一期還本比率.ToString();	//<PRRATE>最後一期還本比率	 9(03)</PRRATE>	
            txn.Rq.EAIBody.MsgRq.SvcRq.INTTM = dataItem.繳息週期月.ToString();	//<INTTM>繳息週期(月)	 9(02)</INTTM>	
            txn.Rq.EAIBody.MsgRq.SvcRq.INTTW = dataItem.繳息週期週.ToString();	//<INTTW>繳息週期(週)	 9(02)</INTTW>	
            txn.Rq.EAIBody.MsgRq.SvcRq.RATECD = dataItem.利率型態.GetEfficientString(0, 2);	//<RATECD>利率型態	 9(02)</RATECD>	
            txn.Rq.EAIBody.MsgRq.SvcRq.IRTCD = dataItem.利率別.GetEfficientString(0, 4); //<IRTCD>利率別	 9(04)</IRTCD>	
            txn.Rq.EAIBody.MsgRq.SvcRq.IRTFT = (dataItem.承作利率 - dataItem.加減碼).ToString();	//<IRTFT>利率別利率	 9(2)V9(5)</IRTFT>	
            txn.Rq.EAIBody.MsgRq.SvcRq.SIGN = dataItem.加減碼 >= 0 ? "+" : "-";	//<SIGN>正負號	 X(01)</SIGN>	
            txn.Rq.EAIBody.MsgRq.SvcRq.IXPR = Math.Abs(dataItem.加減碼.HasValue ? dataItem.加減碼.Value : 0m).ToString();	//<TIXPR>加減碼	 9(02)V9(05)	</TIXPR>
            txn.Rq.EAIBody.MsgRq.SvcRq.FITIRT = dataItem.承作利率.ToString();	//<FITIRT>承作利率	 9(02)V9(05)</FITIRT>	
            txn.Rq.EAIBody.MsgRq.SvcRq.IRTADJ = dataItem.利率調整生效方式.GetEfficientString(0, 1);	//<IRTADJ>利率調整生效方式	 9(01)</IRTADJ>	
            txn.Rq.EAIBody.MsgRq.SvcRq.IRTDM = dataItem.計息週期單位.GetEfficientString(0, 1);	//<IRTDM>按日/月計息	 9(01)</IRTDM>	
            txn.Rq.EAIBody.MsgRq.SvcRq.ISTFLG = dataItem.是否重算期金.GetEfficientString(0, 1); 	//<ISTFLG>是否重算期金	 9(01)</ISTFLG>	
            txn.Rq.EAIBody.MsgRq.SvcRq.PRTM = dataItem.還本週期月.ToString();	//<PRTM>還本週期(月)	 9(02)</PRTM>	
            txn.Rq.EAIBody.MsgRq.SvcRq.PRTW = dataItem.還本週期週.ToString();	//<PRTW>還本週期(週)	 9(02)</PRTW>	
            txn.Rq.EAIBody.MsgRq.SvcRq.IRTTM = dataItem.調整利率週期.ToString();	//<IRTTM>調整利率週期(月)	 9(02)</IRTTM>	

            txn.Rq.EAIBody.MsgRq.SvcRq.CNIRDT = dataItem.指定繳息日.ToString();	//<CNIRDT>指定繳息日	 9(02)</CNIRDT>	
            txn.Rq.EAIBody.MsgRq.SvcRq.GRACEDAY = dataItem.寬限期.ToString();	//<GRACEDAY>寬限期	 9(03)</GRACEDAY>	
            txn.Rq.EAIBody.MsgRq.SvcRq.PRDATE = String.Format("{0:yyyyMMdd}", dataItem.下次攤還日);	//<PRDATE>預定還本日	 9(08)</PRDATE>	
            txn.Rq.EAIBody.MsgRq.SvcRq.APTYPE = item.PaymentNotification.撥款方式.GetEfficientString(0, 1);	//<APTYPE>撥款方式	 9(01)</APTYPE>	
            txn.Rq.EAIBody.MsgRq.SvcRq.APACTNO = !String.IsNullOrEmpty(item.PaymentNotification.入戶帳號) ? item.PaymentNotification.入戶帳號.Replace("-", "") : null;	//<APACTNO>入戶帳號	 9(14)</APACTNO>	
            txn.Rq.EAIBody.MsgRq.SvcRq.AUTCD = dataItem.自動扣繳記號.GetEfficientString(0, 1); 	//<AUTCD>自動扣繳記號	 9(01)</AUTCD>	
            txn.Rq.EAIBody.MsgRq.SvcRq.PAYACTNO = !String.IsNullOrEmpty(dataItem.自動扣繳帳號) ? dataItem.自動扣繳帳號.Replace("-", "") : null;	//<PAYACTNO>繳款帳號	 9(14)</PAYACTNO>	
            txn.Rq.EAIBody.MsgRq.SvcRq.MGENO = item.Reimbursement.Documentary.BankingAudit.貸放主管編號;	//<MGENO>貸放主管	 9(06)</MGENO>	
            txn.Rq.EAIBody.MsgRq.SvcRq.SPENO = item.Reimbursement.Documentary.BankingAudit.貸放負責人編號;	//<SPENO>貸放負責人	 9(06)</SPENO>	
            txn.Rq.EAIBody.MsgRq.SvcRq.LNCD = dataItem.貸放型態.GetEfficientString(0, 1); 	//<LNCD>貸放型態	 X(01)</LNCD>	
            txn.Rq.EAIBody.MsgRq.SvcRq.NOTE = dataItem.備註.GetEfficientString(0, 30); 	//<NOTE>備註	 X(30)</NOTE>	
            if (forCreditInsurance)
            {
                txn.Rq.EAIBody.MsgRq.SvcRq.CRGRRATE = String.Format("{0:.}", item.PaymentNotification.信保成數);    //<CRGRRATE>信保成數	 9(03)</CRGRRATE>	
                txn.Rq.EAIBody.MsgRq.SvcRq.CRGRTYPE = item.PaymentNotification.信保種類.GetEfficientString(0, 2);   //<CRGRTYPE>信保種類	 9(02)</CRGRTYPE>	
                txn.Rq.EAIBody.MsgRq.SvcRq.CRACTNO = dataItem.送信保對應帳支號; //<CRACTNO>送信保對應帳支號	 9(16)</CRACTNO>	
            }
            else
            {
                txn.Rq.EAIBody.MsgRq.SvcRq.CRGRRATE = "";
                txn.Rq.EAIBody.MsgRq.SvcRq.CRGRTYPE = "";
                txn.Rq.EAIBody.MsgRq.SvcRq.CRACTNO = "";
            }
            txn.Rq.EAIBody.MsgRq.SvcRq.YEAR = dataItem.專案年度.ToString();	//<YEAR>專案年度	 9(02)</YEAR>	
            txn.Rq.EAIBody.MsgRq.SvcRq.SIFLG = dataItem.聯貸記號.GetEfficientString(0, 1); 	//<SIFLG>聯貸記號	 9(01)</SIFLG>	
            txn.Rq.EAIBody.MsgRq.SvcRq.SINO = dataItem.聯貸編號;	//<SINO>聯貸編號	 9(13)</SINO>	
            //txn.Rq.EAIBody.MsgRq.SvcRq.INTRQSP = dataItem.是否為固定利率;	//<INTRQSP>是否為固定利率	 9(01)</INTRQSP>	
            //txn.Rq.EAIBody.MsgRq.SvcRq.LABSTAT = dataItem.勞貸貸款狀態;	//<LABSTAT>勞貸貸款狀態	 9(01)</LABSTAT>	
            txn.Rq.EAIBody.MsgRq.SvcRq.RVSNO = item.PaymentNotification.銷帳編號;	//<RVSNO>銷帳編號	 X(10)</RVSNO>	
            txn.Rq.EAIBody.MsgRq.SvcRq.INTEXT = dataItem.繳息寬限期.ToString();	//<INTEXT>繳息寬限期	 9(03)</INTEXT>	
            txn.Rq.EAIBody.MsgRq.SvcRq.BEGINFEE = dataItem.帳戶管理費.ToString();	//<BEGINFEE>房貸帳戶管理費	 9(12)V9(02)</BEGINFEE>	
            txn.Rq.EAIBody.MsgRq.SvcRq.TXTYPE = item.PaymentNotification.現轉別.GetEfficientString(0, 1); 	//<TXTYPE>現轉別	 9(01)</TXTYPE>	
            txn.Rq.EAIBody.MsgRq.SvcRq.TAYACTNO = item.PaymentNotification.扣款帳號.Replace("-", "");	//<TAYACTNO>扣款帳號	 X(14)</TAYACTNO>	
            txn.Rq.EAIBody.MsgRq.SvcRq.LOANTYP = item.PaymentNotification.融資業務分類.GetEfficientString(0, 1);	//<LOANTYP>融資業務分類	 X(01)</LOANTYP>	
            txn.Rq.EAIBody.MsgRq.SvcRq.GOVSTYP = item.PaymentNotification.政府專案補助貸款分類.GetEfficientString(0, 2);	//<GOVSTYP>政府專案補助貸款	 X(02)</GOVSTYP>	
            //txn.Rq.EAIBody.MsgRq.SvcRq.ZEDFRATE = dataItem.零利率違約金利率;	//<ZEDFRATE>零利率違約金利率	 9(02)V9(05)</ZEDFRATE>	
            txn.Rq.EAIBody.MsgRq.SvcRq.IRTKD = dataItem.利率種類.GetEfficientString(0, 2);	//<IRTKD>利率種類	 9(02)</IRTKD>	

            if (item.Reimbursement.L8600ID.HasValue)
            {
                txn.Rq.EAIBody.MsgRq.SvcRq.LCACTNO = item.Reimbursement.NegoDraft.LcID.HasValue
                    ? item.Reimbursement.NegoDraft.LetterOfCreditVersion.LetterOfCredit.LcNo
                    : item.Reimbursement.NegoDraft.NegoLC.LCNo.Replace("-", "");	//信用狀改貸帳支號, 9(16)-->
                txn.Rq.EAIBody.MsgRq.SvcRq.LCDCNT = item.Reimbursement.L8600.墊款流水號.ToString();	//信用狀墊款流水號, 9(04)-->
            }
            else
            {
                txn.Rq.EAIBody.MsgRq.SvcRq.LCACTNO = "";	//信用狀改貸帳支號, 9(16)-->
                txn.Rq.EAIBody.MsgRq.SvcRq.LCDCNT = "";	//信用狀墊款流水號, 9(04)-->
            }
            txn.Rq.EAIBody.MsgRq.SvcRq.IDATE = "";	//聯貸下次收息日,(YYYY-MM-DD)-->
            txn.Rq.EAIBody.MsgRq.SvcRq.SVEINT = "";	//十成融資預收利息(台塑網十成融資使用，帳戶性質352-141), 9(12)V99-->
            txn.Rq.EAIBody.MsgRq.SvcRq.PERINRATE = "";	//基金搭配款比例, 9(03)-->
            txn.Rq.EAIBody.MsgRq.SvcRq.OFUNDAMT = "";	//原始應收代放款/受託代放款金額, 9(12)V99-->
            txn.Rq.EAIBody.MsgRq.SvcRq.REPYCD = "";	//外銷貸款預計還款來源,9(02)-->
            txn.Rq.EAIBody.MsgRq.SvcRq.REPYCDX = "";	//外銷貸款預計還款來源-其他說明,X(22)-->
            txn.Rq.EAIBody.MsgRq.SvcRq.CONRATECD = "";	//聯貸案訂約利率型態, 9(02)-->
            txn.Rq.EAIBody.MsgRq.SvcRq.FFIDATE = "";	//下次利率調整日,(YYYY-MM-DD)-->
            txn.Rq.EAIBody.MsgRq.SvcRq.MORPRCD = "";	//寬限期約定還本記號, 9(01)-->
            txn.Rq.EAIBody.MsgRq.SvcRq.ISHLD = "";	//是否入戶圈存記號, 9(01)-->
            txn.Rq.EAIBody.MsgRq.SvcRq.HLDAMT = "";	//圈存金額, 9(12)V99-->
            txn.Rq.EAIBody.MsgRq.SvcRq.DFFLAG = "";	//提前清償違約金記號, 9(01)-->
            txn.Rq.EAIBody.MsgRq.SvcRq.NXREFNO = "";	//記錄出口交易參考編號, X(16)-->
            txn.Rq.EAIBody.MsgRq.SvcRq.BTFLG = "";	//是否為代償案件(0：否；1：是),9(01)-->
            txn.Rq.EAIBody.MsgRq.SvcRq.RULE = "";	//是否符合銀行法72條之2,9(01)-->
            txn.Rq.EAIBody.MsgRq.SvcRq.RULEX = "";	//原因,9(01)-->
            txn.Rq.EAIBody.MsgRq.SvcRq.FITLOW = dataItem.利率下限.ToString();

            bRetrial = false;

            bool bResult = txn.Commit();

            if (!bResult)
            {
                msg.Append("L1000交易失敗:")
                    .Append(this.ReadErrCodeDis(txn));
                bRetrial = "3001".Equals(txn.RspCode);
                if (bRetrial)
                    msg.Append(",本筆已轉待註記");
            }

            return bResult;

        }


        #region 文件流程控管--啟動

        public IDictionary StartDocumentFlow(String taskID, int? docID, String schemaKey, String docDesc)
        {
            if (CHBAuthorityService.StartDocumentFlow != null)
            {
                return CHBAuthorityService.StartDocumentFlow(UserProfile, taskID, docID, schemaKey, docDesc);
            }
            else
            {
                String[] separator = { "\r\n" };
                string url = new System.Text.StringBuilder(Settings.Default.AAUrl)
                    .Append("?uid=").Append(UserProfile.Auth.Company)
                    .Append("&uuid=").Append(UserProfile.PID)
                    .Append("&type=start&taskID=").Append(taskID)
                    .Append("&docID=").Append(docID)
                    .Append("&schemaKey=").Append(schemaKey).ToString();

                WebClient client = new WebClient();
                String result = client.UploadString(url, Convert.ToBase64String(Encoding.UTF8.GetBytes(docDesc)));

                Dictionary<String, String> items = new Dictionary<string, string>();
                foreach (var item in result.Split(separator, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Split('=')).Where(i => i.Count() > 1))
                {
                    items.Add(item[0], item[1]);
                }
                return items;
            }
        }

        #endregion

        #region 文件流程控管--審核

        public IDictionary VerifyDocumentFlow(String stepID)
        {
            if (CHBAuthorityService.VerifyDocumentFlow != null)
            {
                return CHBAuthorityService.VerifyDocumentFlow(UserProfile, stepID);
            }
            else
            {
                string url = new System.Text.StringBuilder(Settings.Default.AAUrl)
                    .Append("?uid=").Append(UserProfile.Auth.Company)
                    .Append("&uuid=").Append(UserProfile.PID)
                    .Append("&type=verify&stepID=").Append(stepID).ToString();

                return Service.OutboundSvc.SendDocumentFlowControlRequest(url);
            }
        }

        #endregion

        #region 文件流程控管--放行

        public IDictionary PassDocumentFlow(String stepID, decimal limitedAmt)
        {
            if (CHBAuthorityService.PassDocumentFlow != null)
            {
                return CHBAuthorityService.PassDocumentFlow(UserProfile, stepID, limitedAmt);
            }
            else
            {
                string url = new System.Text.StringBuilder(Settings.Default.AAUrl)
                    .Append("?uid=").Append(UserProfile.Auth.Company)
                    .Append("&uuid=").Append(UserProfile.PID)
                    .Append("&type=pass&stepID=").Append(stepID)
                    .Append("&money=").Append(limitedAmt).ToString();

                return Service.OutboundSvc.SendDocumentFlowControlRequest(url);
            }
        }

        #endregion

        #region 文件流程控管--取消

        public IDictionary AbandonDocumentFlow(String stepID, String reason)
        {
            if (CHBAuthorityService.AbandonDocumentFlow != null)
            {
                return CHBAuthorityService.AbandonDocumentFlow(UserProfile, stepID, reason);
            }
            else
            {
                string url = new System.Text.StringBuilder(Settings.Default.AAUrl)
                    .Append("?uid=").Append(UserProfile.Auth.Company)
                    .Append("&uuid=").Append(UserProfile.PID)
                    .Append("&type=abandon&stepID=").Append(stepID)
                    .Append("&reason=").Append(reason).ToString();

                return Service.OutboundSvc.SendDocumentFlowControlRequest(url);
            }

        }

        #endregion

        #region 文件流程控管--刪除

        public IDictionary DeleteDocumentFlow(String stepID)
        {
            if (CHBAuthorityService.DeleteDocumentFlow != null)
            {
                return CHBAuthorityService.DeleteDocumentFlow(UserProfile, stepID);
            }
            else
            {
                string url = new System.Text.StringBuilder(Settings.Default.AAUrl)
                    .Append("?uid=").Append(UserProfile.Auth.Company)
                    .Append("&uuid=").Append(UserProfile.PID)
                    .Append("&type=delete&stepID=").Append(stepID).ToString();

                return Service.OutboundSvc.SendDocumentFlowControlRequest(url);
            }
        }

        #endregion


        #region 重新載入待辦事項

        public eAuth ReloadTodoFlow()
        {
            if (CHBAuthorityService.ReloadTodo != null)
            {
                return CHBAuthorityService.ReloadTodo(UserProfile);
            }
            else
            {
                string url = new System.Text.StringBuilder(Settings.Default.AAUrl)
                    .Append("?uid=").Append(UserProfile.Auth.Company)
                    .Append("&uuid=").Append(UserProfile.PID)
                    .Append("&epsk=").Append(UserProfile.Auth.EPSK)
                    .Append("&apid=").Append(UserProfile.GetAPID())
                    .Append("&type=reload").ToString();
                return getPortalAuth(url);
            }
        }
        #endregion

        #region 重新載入待辦事項

        public XmlDocument DownloadUserInfo(string uid, string uuid)
        {
            if (CHBAuthorityService.DownloadUserInfo != null)
            {
                return CHBAuthorityService.DownloadUserInfo(uid, uuid);
            }
            else
            {
                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    string url = new System.Text.StringBuilder(Settings.Default.AAUrl)
                        .Append("?uid=").Append(uid)
                        .Append("&uuid=").Append(uuid)
                        .Append("&type=userInfo").ToString();

                    xmlDoc.Load(url);
                }
                catch (Exception ex)
                {
                    ModelCore.Helper.Logger.Error(ex);
                }

                return xmlDoc;
            }
        }
        #endregion

    }
}
