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
    /// BankManager ���K�n�y�z�C
    /// </summary>
    public class BankManager : LcEntityManager<CreditApplicationDocumentary>
	{
		public BankManager() : base()
		{
			//
			// TODO: �b���[�J�غc�禡���{���X
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
            // TODO: �b���[�J�غc�禡���{���X
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
                        txn.RCVBK = "009" + item.�}����;
                        txn.REMTYPE = "04";
                        txn.MSG1 = "�q�l�H�Ϊ��}���ݨ��z�q��";
                        txn.MSG2 = "�ӽФH�νs:" + item.CustomerOfBranch.Organization.ReceiptNo + ";"
                                 + "�ӽЮѽs��:" + item.ApplicationNo;
                        //txn.MSG3 = item.Documentary.CustomerCreditAlert.Count > 0
                        //    ? String.Concat("�Ӥᦳ�H�ζS�����СG",
                        //            String.Join("�B",item.Documentary.CustomerCreditAlert.Select(a=>a.AlertCode + " " + a.Description)),
                        //            "�C�Х���T�{�C") 
                        //    : "";
                        txn.MSG3 = item.Documentary.CustomerCreditAlert.Count > 0
                            ? String.Concat("�Ӥᦳ�H�ζS�����СG",
                                    String.Join("�B", item.Documentary.CustomerCreditAlert.Select(a => a.AlertCode)))
                            : "";

                        txn.RECVER = "��ڧ@�~�M��";
                        txn.SENDER = "�`��";
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
                        if (item.�q����!=item.�}����)
                        {
                            Txn_R3801 txn = new EAI.Service.Transaction.Txn_R3801();
                            txn.REMDAY = DateTime.Now.ToString("yyyy-MM-dd");
                            txn.RCVBK = "009" + item.�q����;
                            txn.REMTYPE = "04";
                            //txn.MSG1 = "�q�l�H�Ϊ��}���w�}�߳q��";
                            txn.MSG1 = "�q�l�H�Ϊ��}���w�}�߳q��:�ӽФH�νs:" + item.CustomerOfBranch.Organization.ReceiptNo;
                            //txn.MSG2 = "�ӽФH�νs:" + item.CustomerOfBranch.Organization.ReceiptNo + "��" + DateTime.Now.ToString("MM/dd hh:mm") + "�q" + item.CustomerOfBranch.BankData.BranchName + "�}��";
                            txn.MSG2 = DateTime.Now.ToString("MM/dd HH:mm") + item.CustomerOfBranch.BankData.BranchName + "�}�ߨ��q�H�νs:" + item.BeneficiaryData.Organization.ReceiptNo + "���H�Ϊ�";
                            //txn.MSG3 = "���q�H�νs:" + item.BeneficiaryData.Organization.ReceiptNo + "���H�Ϊ��A�Ш��z�óq�����q�H�C"
                            //        + (item.Documentary.CustomerCreditAlert.Count > 0
                            //            ? String.Concat("�Ӥᦳ�H�ζS�����СG",
                            //                    String.Join("�B", item.Documentary.CustomerCreditAlert.Select(a => a.AlertCode + " " + a.Description)),
                            //                    "�C�Х���T�{�C")
                            //            : "");
                            //txn.MSG3 = "���q�H�νs:" + item.BeneficiaryData.Organization.ReceiptNo + "���H�Ϊ��A�Ш��z�óq�����q�H�C"
                            txn.MSG3 = "�Ш��z�q�����q�H�C"
                                    + (item.Documentary.CustomerCreditAlert.Count > 0
                                        ? String.Concat("�H�ζS���G",
                                                String.Join("�B", item.Documentary.CustomerCreditAlert.Select(a => a.AlertCode)))
                                        : "");
                            txn.RECVER = "�«H�䴩�D�ޤΩ�ڧ@�~�M��";
                            txn.SENDER = "�`��";
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
                            if (item.Documentary.CurrentLevel == (int)Naming.DocumentLevel.�w�}��)
                            {
                                txn.RCVBK = "009" + item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.�}����;
                                txn.MSG1 = "�q�l��פw����"
                                         + string.Format("({0}{1:MMdd})",new TaiwanCalendar().GetYear(DateTime.Now), DateTime.Now)
                                         +"�q��";
                                txn.MSG2 = "�ӽФH�νs:" + item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo + ";"
                                         + "�H�Ϊ����X:" + item.LetterOfCreditVersion.LetterOfCredit.LcNo + ";";

                                txn.MSG3 = "�ײ����X:" + item.DraftNo + ";"
                                         + "��ת��B:" + item.Amount.ToString() + ";";
                            }
                            else
                            {
                                txn.RCVBK = "009" + item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.�q����;
                                txn.MSG1 = "�q�l�H�Ϊ���׫ݨ��z�q��";
                                if (item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.Documentary.CustomerCreditAlert.Count > 0)
                                {
                                    txn.MSG2 = String.Concat("�H�Ϊ����X:", item.LetterOfCreditVersion.LetterOfCredit.LcNo, ";",
                                                    "�ײ����X:", item.DraftNo, ";");

                                    txn.MSG3 = String.Concat("�Ӥᦳ�H�ζS�����СG",
                                                    String.Join("�B", item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.Documentary.CustomerCreditAlert.Select(a => a.AlertCode)),
                                                    "�C�Цܤ����d�߽T�{�C");
                                }
                                else
                                {
                                    txn.MSG2 = String.Concat("�ӽФH�νs:", item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo, ";",
                                                    "�H�Ϊ����X:", item.LetterOfCreditVersion.LetterOfCredit.LcNo, ";");
                                    txn.MSG3 = String.Concat("�ײ����X:", item.DraftNo, ";",
                                                    "��ת��B:", item.Amount.ToString(), ";");

                                }
                            }
                            txn.RECVER = "��ڧ@�~�M��";
                            txn.SENDER = "�`��";
                            txn.Commit();

                        }
                        else
                        {
                            Txn_R3801 txn = new EAI.Service.Transaction.Txn_R3801();
                            txn.REMDAY = DateTime.Now.ToString("yyyy-MM-dd");
                            txn.REMTYPE = "04";
                            if (item.Documentary.CurrentLevel == (int)Naming.DocumentLevel.�w�}��)
                            {
                                txn.RCVBK = "009" + item.NegoLC.IssuingBank;
                                txn.MSG1 = "�q�l��פw����"
                                         + string.Format("({0}{1:MMdd})", new TaiwanCalendar().GetYear(DateTime.Now), DateTime.Now)
                                         + "�q��";
                            }
                            else
                            {
                                txn.RCVBK = "009" + item.NegoLC.AdvisingBank;
                                txn.MSG1 = "�q�l��׫ݨ��z�q��";
                            }
                            txn.MSG2 = "�ӽФH�νs:" + item.NegoLC.CustomerOfBranch.Organization.ReceiptNo + ";"
                                     + "�H�Ϊ����X:" + item.NegoLC.LCNo + ";";

                            txn.MSG3 = "�ײ����X:" + item.DraftNo + ";"
                                     + "��ת��B:" + item.Amount.ToString() + ";";

                            txn.RECVER = "��ڧ@�~�M��";
                            txn.SENDER = "�`��";
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
                        txn.RCVBK = "009" + item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.�}����;
                        txn.REMTYPE = "04";
                        txn.MSG1 = "�q�l�H�Ϊ��ת��ӽФw�}�߳q��";
                        txn.MSG2 = "�ӽФH�νs:" + item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo + ";"
                                 + "�ת��ӽЮѸ��X:" + item.AmendmentNo + ";";
                        //txn.MSG3 = item.Documentary.CustomerCreditAlert.Count > 0
                        //    ? String.Concat("�Ӥᦳ�H�ζS�����СG",
                        //            String.Join("�B", item.Documentary.CustomerCreditAlert.Select(a => a.AlertCode + " " + a.Description)),
                        //            "�C�Х���T�{�C")
                        //    : "";
                        txn.MSG3 = item.Documentary.CustomerCreditAlert.Count > 0
                            ? String.Concat("�H�ζS���G",
                                    String.Join("�B", item.Documentary.CustomerCreditAlert.Select(a => a.AlertCode)))
                            : "";
                        txn.RECVER = "��ڧ@�~�M��";
                        txn.SENDER = "�`��";
                        txn.Commit();

                        if (item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.�}���� != item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.�q����)
                        {
                            txn.RCVBK = $"009{item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.�q����}";
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
                        txn.RCVBK = "009" + item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.�}����;
                        txn.REMTYPE = "04";
                        txn.MSG1 = "�q�l�H�Ϊ��ת��ӽЫݨ��z�q��";
                        txn.MSG2 = "�ӽФH�νs:" + item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo + ";"
                                 + "�ת��ӽЮѸ��X:" + item.AmendmentNo + ";";
                        //txn.MSG3 = item.Documentary.CustomerCreditAlert.Count > 0
                        //    ? String.Concat("�Ӥᦳ�H�ζS�����СG",
                        //            String.Join("�B", item.Documentary.CustomerCreditAlert.Select(a => a.AlertCode + " " + a.Description)),
                        //            "�C�Х���T�{�C")
                        //    : "";
                        txn.MSG3 = item.Documentary.CustomerCreditAlert.Count > 0
                            ? String.Concat("�H�ζS���G",
                                    String.Join("�B", item.Documentary.CustomerCreditAlert.Select(a => a.AlertCode)))
                            : "";

                        txn.RECVER = "��ڧ@�~�M��";
                        txn.SENDER = "�`��";
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
                        txn.RCVBK = "009" + item.LetterOfCredit.CreditApplicationDocumentary.�}����;
                        txn.REMTYPE = "04";
                        txn.MSG1 = "�q�l�H�Ϊ����P�ӽЫݨ��z�q��";
                        txn.MSG2 = "�ӽФH�νs:" + item.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo + ";"
                                 + "���P�ӽЮѸ��X:" + item.���P�ӽи��X + ";";
                        txn.MSG3 =  "";

                        txn.RECVER = "��ڧ@�~�M��";
                        txn.SENDER = "�`��";
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
                        String subject = item.Documentary.CurrentLevel == (int)Naming.DocumentLevel.�ݸg��f��
                            ? "�ײ��ٴګݨ��z�q��"
                            : "�ײ��ٴڤw�����q��";

                        if (item.NegoDraft.LcID.HasValue)
                        {
                            Txn_R3801 txn = new EAI.Service.Transaction.Txn_R3801();
                            txn.REMDAY = DateTime.Now.ToString("yyyy-MM-dd");
                            txn.RCVBK = "009" + item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.�}����;
                            txn.REMTYPE = "04";
                            txn.MSG1 = subject;
                            txn.MSG2 = "�ӽФH�νs:" + item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo + ";"
                                     + "�H�Ϊ����X:" + item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.LcNo + ";";

                            txn.MSG3 = "�ٴڥӽи��X:" + item.ReimbursementNo + ";"
                                     + "�ٴڪ��B:" + item.Amount.ToString() + ";";
                            txn.RECVER = "��ڧ@�~�M��";
                            txn.SENDER = "�`��";
                            txn.Commit();

                        }
                        else
                        {
                            Txn_R3801 txn = new EAI.Service.Transaction.Txn_R3801();
                            txn.REMDAY = DateTime.Now.ToString("yyyy-MM-dd");
                            txn.RCVBK = "009" + item.NegoDraft.NegoLC.IssuingBank;
                            txn.REMTYPE = "04";
                            txn.MSG1 = subject;
                            txn.MSG2 = "�ӽФH�νs:" + item.NegoDraft.NegoLC.CustomerOfBranch.Organization.ReceiptNo + ";"
                                     + "�H�Ϊ����X:" + item.NegoDraft.NegoLC.LCNo + ";";

                            txn.MSG3 = "�ٴڥӽи��X:" + item.ReimbursementNo + ";"
                                     + "�ٴڪ��B:" + item.Amount.ToString() + ";";
                            txn.RECVER = "��ڧ@�~�M��";
                            txn.SENDER = "�`��";
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
                        String subject = item.Documentary.CurrentLevel == (int)Naming.DocumentLevel.�ݸg��f��
                            ? "�ײ��ٴڧ�U�ݨ��z�q��"
                            : "�ײ��ٴڧ�U�w�����q��";

                        if (item.NegoDraft.LcID.HasValue)
                        {
                            Txn_R3801 txn = new EAI.Service.Transaction.Txn_R3801();
                            txn.REMDAY = DateTime.Now.ToString("yyyy-MM-dd");
                            txn.RCVBK = "009" + item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.�}����;
                            txn.REMTYPE = "04";
                            txn.MSG1 = subject;
                            txn.MSG2 = "�ӽФH�νs:" + item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo + ";"
                                     + "�H�Ϊ����X:" + item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.LcNo + ";";

                            //txn.MSG3 = "�ٴڧ�U�ӽи��X:" + item.ReimbursementNo + ";"
                            //         + "�ٴڧ�U���B:" + item.Amount.ToString() + ";"
                            //         + (item.Documentary.CustomerCreditAlert.Count > 0
                            //            ? String.Concat("�Ӥᦳ�H�ζS�����СG",
                            //                    String.Join("�B", item.Documentary.CustomerCreditAlert.Select(a => a.AlertCode + " " + a.Description)),
                            //                    "�C�Х���T�{�C")
                            //            : "");
                            txn.MSG3 = "�ٴڧ�U�ӽи��X:" + item.ReimbursementNo + ";"
                                     + "�ٴڧ�U���B:" + item.Amount.ToString() + ";"
                                     + (item.Documentary.CustomerCreditAlert.Count > 0
                                        ? String.Concat("�H�ζS���G",
                                                String.Join("�B", item.Documentary.CustomerCreditAlert.Select(a => a.AlertCode)))
                                        : "");
                            txn.RECVER = "��ڧ@�~�M��";
                            txn.SENDER = "�`��";
                            txn.Commit();

                        }
                        else
                        {
                            Txn_R3801 txn = new EAI.Service.Transaction.Txn_R3801();
                            txn.REMDAY = DateTime.Now.ToString("yyyy-MM-dd");
                            txn.RCVBK = "009" + item.NegoDraft.NegoLC.IssuingBank;
                            txn.REMTYPE = "04";
                            txn.MSG1 = subject;
                            txn.MSG2 = "�ӽФH�νs:" + item.NegoDraft.NegoLC.CustomerOfBranch.Organization.ReceiptNo + ";"
                                     + "�H�Ϊ����X:" + item.NegoDraft.NegoLC.LCNo + ";";

                            txn.MSG3 = "�ٴڧ�U�ӽи��X:" + item.ReimbursementNo + ";"
                                     + "�ٴڧ�U���B:" + item.Amount.ToString() + ";";
                            txn.RECVER = "��ڧ@�~�M��";
                            txn.SENDER = "�`��";
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
                            txn.RCVBK = "009" + item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.�}����;
                            txn.REMTYPE = "04";
                            txn.MSG1 = "�ӧI�ײ��w�����q��";
                            txn.MSG2 = "�ӽФH�νs:" + item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo + ";"
                                     + "�H�Ϊ����X:" + item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.LcNo + ";";

                            txn.MSG3 = "�ײ����X:" + item.NegoDraft.DraftNo + ";"
                                     + "��ת��B:" + item.NegoDraft.Amount.ToString() + ";";
                            txn.RECVER = "��ڧ@�~�M��";
                            txn.SENDER = "�`��";
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
                        String subject = "�Ȥ��U�ٴګݩ��q��";

                        if (item.NegoDraft.LcID.HasValue)
                        {
                            Txn_R3801 txn = new EAI.Service.Transaction.Txn_R3801();
                            txn.REMDAY = DateTime.Now.ToString("yyyy-MM-dd");
                            txn.RCVBK = "009" + item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.�}����;
                            txn.REMTYPE = "04";
                            txn.MSG1 = subject;
                            txn.MSG2 = "�ӽФH�νs:" + item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo + ";"
                                     + "�H�Ϊ����X:" + item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.LcNo + ";";

                            txn.MSG3 = "��U���ٴڥӽи��X:" + item.ReimbursementNo + ";"
                                     + "��U�ٴڪ��B:" + repayment.RepaymentAmount.ToString() + ";";
                            txn.RECVER = "��ڧ@�~�M��";
                            txn.SENDER = "�`��";
                            txn.Commit();

                        }
                        else
                        {
                            Txn_R3801 txn = new EAI.Service.Transaction.Txn_R3801();
                            txn.REMDAY = DateTime.Now.ToString("yyyy-MM-dd");
                            txn.RCVBK = "009" + item.NegoDraft.NegoLC.IssuingBank;
                            txn.REMTYPE = "04";
                            txn.MSG1 = subject;
                            txn.MSG2 = "�ӽФH�νs:" + item.NegoDraft.NegoLC.CustomerOfBranch.Organization.ReceiptNo + ";"
                                     + "�H�Ϊ����X:" + item.NegoDraft.NegoLC.LCNo + ";";

                            txn.MSG3 = "��U���ٴڥӽи��X:" + item.ReimbursementNo + ";"
                                     + "��U�ٴڪ��B:" + repayment.RepaymentAmount.ToString() + ";";
                            txn.RECVER = "��ڧ@�~�M��";
                            txn.SENDER = "�`��";
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
                        txn.RCVBK = "009" + item.LetterOfCredit.CreditApplicationDocumentary.�}����;
                        txn.REMTYPE = "04";
                        txn.MSG1 = "�q�l�H�Ϊ��D�ʾl�B���P�ݵn���q��";
                        txn.MSG2 = "�ӽФH�νs:" + item.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo + ";"
                                 + "�H�Ϊ����X:" + item.LetterOfCredit.LcNo;

                        txn.MSG3 =  "";

                        txn.RECVER = "��ڧ@�~�M��";
                        txn.SENDER = "�`��";
                        txn.Commit();
                    }
                }

            });
        }

        public static void DoReadyTodoR3801(String branch, String branchCRC, Naming.DocumentTypeDefinition docType)
        {
            DoReadyTodoR3801(branch, branchCRC, $"{docType}����g�쥼���z");
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
                txn.MSG2 = $"���z����G{branch}";
                if (reason != null)
                {
                    txn.MSG3 = reason;
                }

                txn.RECVER = "��ڧ@�~�M��";
                txn.SENDER = "�`��";
                txn.Commit();

                txn.RCVBK = $"009{branch}";
                txn.Commit();
            });
        }


        // add �T��out �Ѽ� 980603
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
                txn.RBRNO = item.OpeningApplicationDocumentary.���Z��;
                txn.SBRNO = item.OpeningApplicationDocumentary.���b��;
                txn.CIFKEY = item.CustomerOfBranch.Organization.ReceiptNo;
                txn.MGDATA = item.PaymentNotification.�B�׸��X.Replace("-", "");
                txn.ACTNO = item.OpeningApplicationDocumentary.�b��.Replace("-", "");
                var seg = item.PaymentNotification.�b��ʽ�.Split('-');
                txn.CHARCD1 = seg[0];
                txn.CHARCD2 = seg[1];
                txn.ACNO = item.PaymentNotification.�|�p���;
                txn.SBNO = item.OpeningApplicationDocumentary.�|�p�l��;
                txn.SDAY = String.Format("{0:yyyyMMdd}", System.DateTime.Now);
                txn.GRAMT = item.PaymentNotification.�O�Ҫ�.ToString();
                txn.EDAY = String.Format("{0:yyyyMMdd}", item.LcItem.���Ĵ���);
                txn.RTDAY = item.OpeningApplicationDocumentary.�ײ�����.ToString();
                txn.NOTEBK = "009" + item.�q����;
                txn.BUNINO = item.BeneficiaryData.Organization.ReceiptNo;
                txn.ISDAYCNT = item.OpeningApplicationDocumentary.�s�P�Ѽƭp�� == true ? "2" : "1";
                txn.RATE = item.OpeningApplicationDocumentary.����O�v.ToString();

                //***************************************************
                //***************************************************
                txn.FEE = item.OpeningApplicationDocumentary.����O���B.ToString();          //����O���B
                txn.HCRATE = item.OpeningApplicationDocumentary.�ӧI�O�v.ToString();      //�ӧI�O�O�v
                txn.HCFEE = item.PaymentNotification.�ӧI����O.ToString();           //�ӧI����O
                txn.TXTYPE = item.PaymentNotification.�{��O.Substring(0, 1);           //�{��O
                txn.PACTNO = !String.IsNullOrEmpty(item.PaymentNotification.���ڱb��) ? item.PaymentNotification.���ڱb��.Replace("-", "") : null;         //���ڱb��
                txn.CKAMT = item.OpeningApplicationDocumentary.��ú���B.ToString();              //��ú���B
                txn.CKNO = item.OpeningApplicationDocumentary.�䲼���X;                 //�䲼���X
                txn.CKBKNO7 = !String.IsNullOrEmpty(item.OpeningApplicationDocumentary.�䲼�I�ڦ�) ? item.OpeningApplicationDocumentary.�䲼�I�ڦ�.Replace("-", "") : null;                  //�䲼�I�ڦ�
                txn.CKACTNO = item.OpeningApplicationDocumentary.�o���H�b��; //�X���H�b��
                txn.FUNCD = !String.IsNullOrEmpty(item.PaymentNotification.�γ~�O) ? item.PaymentNotification.�γ~�O.Substring(0, 1) : null;                //�γ~�O
                txn.MGENO = item.OpeningApplicationDocumentary.�U��D�޽s��;           //�U��D�޽s��
                txn.SPENO = item.OpeningApplicationDocumentary.�U��t�d�H�s��;           //�U��t�d�H�s��
                txn.PAYNO = item.OpeningApplicationDocumentary.�I�ڤH.Replace("-", "");       //�I�ڤH
                txn.PAYTYPE = item.�����Y�I ? "1" : "2";           //�I�ڴ���-�Ѽ�
                txn.PAYDAY = item.LcItem.�w��I��.ToString();            //�I�ڴ���-�Ѽ�
                txn.SEAL = item.SpecificNote.��d�LŲ�۲� == true ? "1" : "2";                     //�ײ��I�کΩӧI�ӽЮѤW�LŲ�覡
                txn.DELIVER = item.SpecificNote.�����f == true ? "1" : "2";                  //��f�覡
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

                    goods =  ValidityAgent.ConverttoFullWidthString(String.Join(" ", item.LcItem.GoodsDetails.Select(s => String.Format("�~�W:{0} �W��:{1} ���:{2} �ƶq:{3} ���B:{4} �Ƶ�:{5}",
                            s.ProductName, s.ProductSize, s.UnitPrice, s.Quantity, s.Amount,s.Remark))));
                }

                buildGoodsContent(txn, goods);

                txn.DEADLE = String.Format("{0:yyyyMMdd}", item.SpecificNote.�̫��f��);        //�̫��f���
                //					txn.DEADLE = System.DateTime.ParseExact(txn.DEADLE,"yyyy.MM.dd",System.Globalization.CultureInfo.CurrentCulture).ToString();
                txn.GTXNO = item.OpeningApplicationDocumentary.����y����.ToString();                    //**�H�Ϊ����X
                txn.RVSNO = item.PaymentNotification.�P�b�s��;               //�P�b�s��
                txn.LOANTYP = !String.IsNullOrEmpty(item.PaymentNotification.�ĸ�~�Ȥ���) ? item.PaymentNotification.�ĸ�~�Ȥ���.Substring(0, 1) : null;         //�ĸ�~�Ȥ���

                txn.GOVSTYP = !String.IsNullOrEmpty(item.PaymentNotification.�F���M�׸ɧU�U�ڤ���) ? item.PaymentNotification.�F���M�׸ɧU�U�ڤ���.Substring(0, 2) : null;          //�F���M�׸ɧU�U�ڤ���
                txn.SETGFG = item.OpeningApplicationDocumentary.�Q����O�O�� == true ? "2" : "1";         //�Q����O�O��
                //					txn.TYPE = rowDetail["CreditKind"].ToString();              //�H�Ϊ�����
                txn.TYPE = "1";                                             //�H�Ϊ�����
                txn.FSQAMT = item.LcItem.�}�����B.ToString();            //�}�����B
                //					txn.FCURCD = rowDetail["CreditKind"].ToString();            //�}�����O
                txn.FCURCD = "00";           //�}�����O
                txn.EXRATE = item.OpeningApplicationDocumentary.�}���ײv.ToString();          //�}���ײv
                txn.TXAMT = item.OpeningApplicationDocumentary.�O�b���B.ToString();         //�O�b���B
                txn.ELCDTLFG = item.ForFpgService(BeneficiaryServiceGroup.ServiceDefinition.Fpg) 
                                    ? "2" 
                                    : item.ForFpgService(BeneficiaryServiceGroup.ServiceDefinition.Chimei)
                                        ? "3"
                                        : "1";              //�ӽк���
                txn.CRGRRATE = String.Format("{0:.}", item.PaymentNotification.�H�O����);      //�H�O����
                txn.CRGRTYPE = item.PaymentNotification.�H�O����;            //�H�O����
                txn.CRGRAMT = item.PaymentNotification.��ڰe�O���B.ToString();//��ڰe�O���B
                txn.Rq.EAIBody.MsgRq.SvcRq.CRGRDT = String.Format("{0:yyyyMMdd}", item.PaymentNotification.�e�O�����);        //�e�O�����
                if(item.PaymentNotification.�p�U�y����.HasValue)
                {
                    txn.Rq.EAIBody.MsgRq.SvcRq.SINO = item.�}���� + item.PaymentNotification.CustomerID + String.Format("{0:0000}", item.PaymentNotification.�p�U�y����);
                }
                txn.Rq.EAIBody.MsgRq.SvcRq.GUNO = item.OpeningApplicationDocumentary.������ҽs��;

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
                txn.CONT1 = goods;										//�f�~���e1
                txn.CONT2 = "";											//�f�~���e2
                txn.CONT3 = "";											//�f�~���e3
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
                    if (newItem.�}�����B > oldItem.�}�����B)
                    {
                        l3700.SQAMT = (newItem.�}�����B - oldItem.�}�����B).ToString();
                    }
                    else
                    {
                        l3700.SQAMT = "";
                    }
                    //MiYu 2006-10-31
                    //l3700.DATE = String.Format("{0:yyyyMMdd}",rowDetail["LcExpiry"]);
                    l3700.DATE = String.Format("{0:yyyyMMdd}", newItem.���Ĵ���);
                    l3700.CAUSE = !String.IsNullOrEmpty(item.AmendingLcRegistry.�����H�Ϊ���]) ? item.AmendingLcRegistry.�����H�Ϊ���].Substring(0, 1) : null;
                    l3700.RTDATE = item.AmendingLcRegistry.�����ײ�����.ToString();
                    l3700.RTCAUSE = !String.IsNullOrEmpty(item.AmendingLcRegistry.�����ײ�������]) ? item.AmendingLcRegistry.�����ײ�������].Substring(0, 1) : null;
                    l3700.GRAMT = String.Format("{0}", item.PaymentNotification.�W�[�O�Ҫ����B);
                    l3700.CHRATE = String.Format("{0}", item.PaymentNotification.����O�v);
                    l3700.FEE = String.Format("{0}", item.AmendingLcRegistry.�窱�O���B);
                    l3700.HCFEE = String.Format("{0}", item.PaymentNotification.�ӧI����O);
                    l3700.TXTYPE = !String.IsNullOrEmpty(item.PaymentNotification.�{��O) ? item.PaymentNotification.�{��O.Substring(0, 1) : null;
                    if (l3700.TXTYPE == "4")
                    {
                        if (!String.IsNullOrEmpty(item.PaymentNotification.�P�b�s��))
                        {
                            l3700.RVSYEAR = item.PaymentNotification.�P�b�s��.Substring(0, 1);
                            l3700.RVSSRNO = item.PaymentNotification.�P�b�s��.Substring(1);
                        }
                    }


                    l3700.PACTNO = !String.IsNullOrEmpty(item.PaymentNotification.���ڱb��) ? item.PaymentNotification.���ڱb��.Replace("-", "") : "";

                    if (item.AmendingLcRegistry.�W�[�H�Ϊ��O�b���B > 0)
                    {
                        l3700.FSQAMT = String.Format("{0}", item.AmendingLcRegistry.�W�[�H�Ϊ��O�b���B);
                    }
                    else
                    {
                        l3700.FSQAMT = "";
                    }

                    l3700.FCURCD = String.Format("{0:00}", item.LcItem.���O);
                    l3700.EXRATE = String.Format("{0}", item.AmendingLcRegistry.�ײv);

                    bResult = l3700.Commit();
                    if (!bResult)
                    {
                        sbRspCode.Append(l3700.RspCode).Append(":").Append(this.ReadErrCodeDis(l3700));
                        bRetrial = "3001".Equals(l3700.RspCode);
                    }

                }
                //�ˬdL4500�O�_���קﶵ��
                if (bResult && newItem.CheckL4500Items(oldItem))
                {
                    revised = true;
                    l4500 = new Txn_L4500();
                    l4500.KINBR = UserProfile.UserProfileRow.branch;
                    l4500.HCODE = "0";
                    l4500.ACTNO = item.LetterOfCreditVersion.LetterOfCredit.LcNo.Replace("-", "");
                    l4500.CIFKEY = item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo;
                    l4500.CAUSE = !String.IsNullOrEmpty(item.AmendingLcRegistry.�R�P��]) ? item.AmendingLcRegistry.�R�P��].Substring(0, 2) : null;
                    l4500.TXAMT = String.Format("{0}", Math.Abs(newItem.�}�����B.Value - oldItem.�}�����B.Value));
                    l4500.EXRATE = String.Format("{0}", item.AmendingLcRegistry.�ײv);
                    l4500.GRAMT = String.Format("{0}", item.AmendingLcRegistry.�R�P�s�J�O�Ҫ����B);
                    l4500.BPBKNO = item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.�q����;

                    l4500.TXTYPE = !String.IsNullOrEmpty(item.PaymentNotification.���ڤ覡) ? item.PaymentNotification.���ڤ覡.Substring(0, 1) : null;
                    l4500.PACTNO = !String.IsNullOrEmpty(item.PaymentNotification.�J��b��) ? item.PaymentNotification.�J��b��.Replace("-", "") : null;
                    l4500.GTXNO = item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.OpeningApplicationDocumentary.����y����.ToString();
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
                        //�������,����EC
                        l3700.HCODE = "1";
                        if (!l3700.Commit())
                        {
                            this.Context.AbortTransaction(amendingID, UserProfile.UserProfileRow.pid, (int)Naming.DocumentLevel.�ݵ��O);

                            sbRspCode.Append(",EC����[").Append(l3700.RspCode).Append(":")
                                .Append(this.ReadErrCodeDis(l3700)).Append("]");

                            bRetrial = true;
                        }
                    }

                }
                //�ˬdL3700�O�_���קﶵ��
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
                    l5300.FUNCD = !String.IsNullOrEmpty(item.PaymentNotification.�γ~�O) ? item.PaymentNotification.�γ~�O.Substring(0, 1) : null;
                    l5300.PAYNO = "009" + item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.�I�ڦ�;
                    l5300.CONT1 = "";
                    l5300.CONT2 = "";
                    l5300.CONT3 = "";
                    if (item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.�����Y�I)
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
                                l5300.PAYDAY = $"{item.LcItem.�w��I��-item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.LcItem.�w��I��}";
                            }
                            l5300.PAYDATE = "00000000";
                        }
                    }

                    if (item.SpecificNote.��d�LŲ�۲� == true)
                    {
                        l5300.SEAL = "1";
                    }
                    else
                    {
                        l5300.SEAL = "2";
                    }

                    if (item.SpecificNote.�����f == true)
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
                            l5300.CONT1 = goods;										//�f�~���e1
                            l5300.CONT2 = "";											//�f�~���e2
                            l5300.CONT3 = "";											//�f�~���e3
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
                    l5300.DEADL = String.Format("{0:yyyyMMdd}", item.SpecificNote.�̫��f��);
                    l5300.GTXNO = item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.OpeningApplicationDocumentary.����y����.ToString();
                    l5300.CHFEE = l3700 == null ? item.AmendingLcRegistry.�窱�O���B.ToString() : "";
                    l5300.TXTYPE = !String.IsNullOrEmpty(item.PaymentNotification.�{��O) ? item.PaymentNotification.�{��O.Substring(0, 1) : null;
                    if (l5300.TXTYPE == "4")
                    {
                        if (!String.IsNullOrEmpty(item.PaymentNotification.�P�b�s��))
                        {
                            l5300.RVSYEAR = item.PaymentNotification.�P�b�s��.Substring(0, 1);
                            l5300.RVSSRNO = item.PaymentNotification.�P�b�s��.Substring(1);
                        }
                    }

                    l5300.PACTNO = !String.IsNullOrEmpty(item.PaymentNotification.���ڱb��) ? item.PaymentNotification.���ڱb��.Replace("-", "") : "";

                    l5300.ELCDTLFG = item.ForFpgService(BeneficiaryServiceGroup.ServiceDefinition.Fpg)
                                    ? "2"
                                    : item.ForFpgService(BeneficiaryServiceGroup.ServiceDefinition.Chimei)
                                        ? "3"
                                        : "1";              //�ӽк���
                    l5300.CRGRRATE = String.Format("{0:.}", item.PaymentNotification.�H�O����);
                    l5300.CRGRTYPE = item.PaymentNotification.�H�O����;
                    l5300.CRGRAMT = String.Format("{0}", item.PaymentNotification.��ڰe�O���B);
                    l5300.Rq.EAIBody.MsgRq.SvcRq.CRGRDT = String.Format("{0:yyyyMMdd}", item.PaymentNotification.�e�O�����);        //�e�O�����
                    if (item.PaymentNotification.�p�U�y����.HasValue)
                    {
                        l5300.Rq.EAIBody.MsgRq.SvcRq.SINO = item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.�}���� + item.PaymentNotification.CustomerID + String.Format("{0:0000}", item.PaymentNotification.�p�U�y����);
                    }
                    l5300.Rq.EAIBody.MsgRq.SvcRq.GUNO = item.AmendingLcRegistry.������ҽs��;

                    bResult = l5300.Commit();
                    if (!bResult)
                    {
                        if (sbRspCode.Length > 0)
                            sbRspCode.Append(",");

                        sbRspCode.Append(l5300.RspCode).Append(":").Append(this.ReadErrCodeDis(l5300));
                        bRetrial = "3001".Equals(l5300.RspCode);

                        if (l4500 != null)
                        {
                            //�������,����EC
                            l4500.HCODE = "1";
                            if (!l4500.Commit())
                            {
                                sbRspCode.Append(",EC����[").Append(l4500.RspCode).Append(":")
                                    .Append(this.ReadErrCodeDis(l4500)).Append("]");
                                bRetrial = true;
                            }
                        }

                        if (l3700 != null)
                        {
                            //�������,����EC
                            l3700.HCODE = "1";
                            if (!l3700.Commit())
                            {
                                this.Context.AbortTransaction(amendingID, UserProfile.UserProfileRow.pid, (int)Naming.DocumentLevel.�ݵ��O);
                                sbRspCode.Append(",EC����[").Append(l3700.RspCode).Append(":")
                                    .Append(this.ReadErrCodeDis(l3700)).Append("]");
                                bRetrial = true;
                            }
                        }
                    }
                    //}


                }

                if (bRetrial)
                {
                    this.Context.AbortTransaction(amendingID, UserProfile.UserProfileRow.pid, (int)Naming.DocumentLevel.�ݵ��O);
                }

            }

            if (!bResult && sbRspCode.Length > 0)
            {
                if (bRetrial)
                    sbRspCode.Append(",�����w��ݵ��O");
                rspCode = sbRspCode.ToString();
            }

            return bResult;

        }

        private bool doNegoL4500EAI(NegoDraft row, out string rspCode, out bool bRetrial, out Txn_L4500 txn)
        {
            string str;

            txn = new Txn_L4500();
            //�a�J�Ѽ�			
            str = row.DraftNo;    //�s�W�ײ��ӽЮѸ��X MIN-YU-081201 add
            //if (str.Trim().Length > 7)
            //{
            //    L4500.LNCKNO = str.Substring(str.Trim().Length - 7, 7);  //�ײ��ӽЮѸ��X���C�X

            //}
            //else
            //{
            //    L4500.LNCKNO = str.Trim();
            //}
            txn.LNCKNO = String.Format("{0}", row.DraftID % 10000000);
            txn.KINBR = UserProfile.UserProfileRow.branch;
            txn.HCODE = "0";							//�󥿰O��
            txn.ACTNO = (row.LcID.HasValue ? row.LetterOfCreditVersion.LetterOfCredit.LcNo : row.ACTNO()).Replace("-", "");
            txn.CIFKEY = row.LcID.HasValue?row.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo : row.NegoLC.ApplicantReceiptNo;				//�R��νs
            str = row.NegoDraftRegistry.�R�P��];
            txn.CAUSE = (str != null && str.Length >= 2) ? str.Substring(0, 2) : null; 					//�R�P��]
            txn.TXAMT = String.Format("{0:0.00}", row.Amount);				//�R�P�H�Ϊ����B            
            txn.EXRATE = row.NegoDraftRegistry.�ײv.ToString();				//�ײv
            txn.GRAMT = row.NegoDraftRegistry.�R�P�s�J�O�Ҫ����B.ToString();				//�R�P�s�J�O�Ҫ����B
            txn.TXTYPE = row.NegoDraftRegistry.���ڤ覡.Substring(0, 1);				//���ڤ覡            
            txn.BPBKNO = (txn.TXTYPE == "6" || txn.TXTYPE == "7" ) ? row.NegoDraftExtension.NegoBranch : row.NegoDraftExtension.LcBranch;//��צ� �}���� <> ��צ� 6 7 �u�Q�|��~�|�����p�橹��
            //txn.BPBKNO = row.LcID.HasValue ? row.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.�}���� : row.NegoLC.IssuingBank;
            txn.PACTNO = !String.IsNullOrEmpty(row.NegoDraftRegistry.���ڱb�����) ? row.NegoDraftRegistry.���ڱb�����.Replace("-", "") : "";				//���ڱb��/���							
            txn.GTXNO = row.NegoDraftRegistry.GTXNO.ToString();						//�H�Ϊ��y����,�@10�X
            decimal? val = row.NegoDraftRegistry.�[��X + row.NegoDraftRegistry.���P�Q�v;
            txn.VRATE = val.HasValue ? val.Value.ToString() : "0";				//�ԴڧQ�v
            txn.IRTKD = row.NegoDraftRegistry.�Q�v����?.Substring(0, 2);                       //�Q�v���� �T�w�� 04
            txn.RATECD = row.NegoDraftRegistry.�Q�v���A?.Substring(0, 2);                //�Q�v���A 01 or 04
            txn.IRTCD = row.NegoDraftRegistry.�Q�v�O?.Substring(0, 4);								//�Q�v�O
            txn.FITIRT = string.Format("{0:00.00000}", row.NegoDraftRegistry.���P�Q�v ?? 0);
            txn.IXPR = string.Format("{0:00.00000}", row.NegoDraftRegistry.�[��X ?? 0);
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
                    sbRspCode.Append(",�����w��ݵ��O");
            }

            rspCode = sbRspCode.ToString();
            return bResult;

        }

        private bool doNegoL1201EAI(NegoDraft draft, out string rspCode, out bool bRetrial, out Txn_L1201 txn)
        {
            txn = new Txn_L1201();
            //�a�J�Ѽ�
            txn.Rq.EAIBody.MsgRq.SvcRq.KINBR = UserProfile.UserProfileRow.branch;	//��J��
            txn.Rq.EAIBody.MsgRq.SvcRq.HCODE = "0";				//�󥿰O��
            txn.Rq.EAIBody.MsgRq.SvcRq.LCACTNO = draft.LcID.HasValue ? draft.LetterOfCreditVersion.LetterOfCredit.LcNo : draft.ACTNO();		//�H�Ϊ��b�丹,�@16�X
            txn.Rq.EAIBody.MsgRq.SvcRq.LCGTXNO = draft.NegoDraftRegistry.GTXNO.ToString();			//�H�Ϊ��y����,�@10�X
            txn.Rq.EAIBody.MsgRq.SvcRq.RBRNO = draft.LcID.HasValue ? draft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.OpeningApplicationDocumentary.���Z�� : draft.NegoLC.IssuingBank;	//���Z��
            txn.Rq.EAIBody.MsgRq.SvcRq.CIFKEY = draft.LcID.HasValue ? draft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo : draft.NegoLC.CustomerOfBranch.Organization.ReceiptNo;	//�R��νs
            txn.Rq.EAIBody.MsgRq.SvcRq.ACBRNO = draft.LcID.HasValue ? draft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.OpeningApplicationDocumentary.���b�� : draft.NegoLC.IssuingBank;	//���b��
            txn.Rq.EAIBody.MsgRq.SvcRq.MGDATA = draft.PaymentNotification.�B�׸��X.Replace("-", "");		//�B�׸��X
            txn.Rq.EAIBody.MsgRq.SvcRq.ACTNO = (draft.LcID.HasValue ? draft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.�}���� : draft.NegoLC.IssuingBank) + "470000000000";		//�b��
            //20050827 Andy �ק� EAI���(L1201)�� �ײ����X���(L1201.IBGTXNO)�a�J��ץӽЮѸ��X(��a�J��׸��X)
            //L1201.IBGTXNO=_strFuDraftNo;	//�ײ����X
            txn.Rq.EAIBody.MsgRq.SvcRq.IBGTXNO = draft.AppNo();	//��ץӽЮѸ��X
            txn.Rq.EAIBody.MsgRq.SvcRq.TXAMT = String.Format("{0:0.00}", draft.Amount);	//�ײ����B			
            txn.Rq.EAIBody.MsgRq.SvcRq.CRGRRATE = String.Format("{0:.}", draft.NegoDraftRegistry.�H�O����);	//�H�O����
            txn.Rq.EAIBody.MsgRq.SvcRq.CRGRTYPE = draft.NegoDraftRegistry.�H�O����;	//�H�O����
            txn.Rq.EAIBody.MsgRq.SvcRq.CRGRDT = String.Format("{0:yyyyMMdd}", draft.NegoDraftRegistry.�e�O�����);        //�e�O�����
            txn.Rq.EAIBody.MsgRq.SvcRq.EDAY = String.Format("{0:yyyyMMdd}", draft.UsanceDate());	//�ײ������
            txn.Rq.EAIBody.MsgRq.SvcRq.SDAY = String.Format("{0:yyyyMMdd}", draft.ImportDate);	//�o����
            if ( !String.IsNullOrEmpty(draft.PaymentNotification.�b��ʽ�))
            {
                string[] accountType = draft.PaymentNotification.�b��ʽ�.Split('-');

                txn.Rq.EAIBody.MsgRq.SvcRq.CHARCD1 = accountType[0];	//�b��ʽ�e3�X
                txn.Rq.EAIBody.MsgRq.SvcRq.CHARCD2 = accountType.Length > 0 ? accountType[1] : "";	//�b��ʽ��3�X
            }
            else
            {
                txn.Rq.EAIBody.MsgRq.SvcRq.CHARCD1 = "";				//�b��ʽ�e3�X
                txn.Rq.EAIBody.MsgRq.SvcRq.CHARCD2 = "";				//�b��ʽ��3�X
            }
            txn.Rq.EAIBody.MsgRq.SvcRq.ACNO = draft.NegoDraftRegistry.�|�p���;			//�|�p���
            txn.Rq.EAIBody.MsgRq.SvcRq.SBNO = draft.NegoDraftRegistry.�|�p�l��;			//�|�p�l��
            txn.Rq.EAIBody.MsgRq.SvcRq.BUNINO = draft.LcID.HasValue ? draft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.BeneficiaryData.Organization.ReceiptNo
                : draft.NegoLC.BeneficiaryData.Organization.ReceiptNo;		//�o���H(���νs)
            txn.Rq.EAIBody.MsgRq.SvcRq.IBKNO = "009";
            txn.Rq.EAIBody.MsgRq.SvcRq.IBRNO = draft.LcID.HasValue ? draft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.�q���� : draft.NegoLC.AdvisingBank;
            txn.Rq.EAIBody.MsgRq.SvcRq.HCRATE = draft.NegoDraftRegistry.�ӧI����O�v.ToString();		//�ӧI����O�v
            txn.Rq.EAIBody.MsgRq.SvcRq.HCFEE = draft.NegoDraftRegistry.�ӧI����O���B.ToString();			//�ӧI����O���B
            txn.Rq.EAIBody.MsgRq.SvcRq.TXTYPE = draft.NegoDraftRegistry.�{��O != null ? draft.NegoDraftRegistry.�{��O.Substring(0, 1) : null;		//�{��O
            txn.Rq.EAIBody.MsgRq.SvcRq.PACTNO = !String.IsNullOrEmpty(draft.PaymentNotification.���ڱb��) ? draft.PaymentNotification.���ڱb��.Replace("-", "") : null;			//���ڱb�丹 Thomas
            txn.Rq.EAIBody.MsgRq.SvcRq.CKAMT = draft.NegoDraftRegistry.��ú���B.ToString();			//��ú���B
            txn.Rq.EAIBody.MsgRq.SvcRq.CKNO = draft.NegoDraftRegistry.�䲼���X;			//�䲼���X
            txn.Rq.EAIBody.MsgRq.SvcRq.CKBKNO7 = "";		//�I�ڦ�
            txn.Rq.EAIBody.MsgRq.SvcRq.CKACTNO = "";	//�o���H�b��
            txn.Rq.EAIBody.MsgRq.SvcRq.MGENO = "";			//�U��D�޽s��
            txn.Rq.EAIBody.MsgRq.SvcRq.SPENO = "";			//�U��t�d�H�s��
            txn.Rq.EAIBody.MsgRq.SvcRq.FUNCD = !String.IsNullOrEmpty(draft.NegoDraftRegistry.�γ~�O) ? draft.NegoDraftRegistry.�γ~�O.Substring(0, 1) : null;			//�γ~�O
            txn.Rq.EAIBody.MsgRq.SvcRq.RVSNO = draft.PaymentNotification.�P�b�s��;		//�P�b�s��
            txn.Rq.EAIBody.MsgRq.SvcRq.LOANTYP = !String.IsNullOrEmpty(draft.NegoDraftRegistry.�ĸ�~�Ȥ���) ? draft.NegoDraftRegistry.�ĸ�~�Ȥ���.Substring(0, 1) : null;	//�ĸ�~�Ȥ���
            txn.Rq.EAIBody.MsgRq.SvcRq.GOVSTYP = !String.IsNullOrEmpty(draft.NegoDraftRegistry.�F���M�׸ɧU�U�ڤ���) ? draft.NegoDraftRegistry.�F���M�׸ɧU�U�ڤ���.Substring(0, 2) : null;	//�F���M�׸ɧU�U�ڤ���
            txn.Rq.EAIBody.MsgRq.SvcRq.SETGFG = draft.NegoDraftRegistry.�Q����O�O�� == true ? "1" : "0";		//�Q����O�O��											
            txn.Rq.EAIBody.MsgRq.SvcRq.GRAMT = draft.NegoDraftRegistry.�ϥΦs�J�O�Ҫ����B.ToString();   //�ϥΦs�J�O�Ҫ����B
            txn.Rq.EAIBody.MsgRq.SvcRq.ELCDTLFG = "1";  // draft.NegoDraftRegistry.�ӽк���;		//�ӽк���

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
                    sbRspCode.Append(",�����w��ݵ��O");
            }

            rspCode = sbRspCode.ToString();
            return bResult;
        }

        private bool doL2200EAI(NegoLoanRepayment repayment, EAI.Service.Transaction.LR006_Rs.IFX rs, System.Text.StringBuilder response, out bool bRetrial, out Txn_L2200 txn, bool forCreditInsurance = false)
        {
            NegoLoan loan = repayment.NegoLoan;
            CreditInsuranceLoan insuranceLoan = loan.CreditInsuranceLoan;

            txn = new Txn_L2200();
            //�a�J�Ѽ�
            txn.Rq.EAIBody.MsgRq.SvcRq.KINBR = loan.Reimbursement.NegoDraft.LetterOfCreditVersion.LetterOfCredit?.CreditApplicationDocumentary.�}���� ?? loan.Reimbursement.NegoDraft.NegoLC?.IssuingBank;	//��J��
            txn.Rq.EAIBody.MsgRq.SvcRq.RBRNO = txn.Rq.EAIBody.MsgRq.SvcRq.KINBR;
            txn.Rq.EAIBody.MsgRq.SvcRq.ACBRNO = txn.Rq.EAIBody.MsgRq.SvcRq.KINBR;
            txn.Rq.EAIBody.MsgRq.SvcRq.INTFLG = "1";
            txn.Rq.EAIBody.MsgRq.SvcRq.DFCD = "1";
            txn.Rq.EAIBody.MsgRq.SvcRq.EAMTCD = "1";
            txn.Rq.EAIBody.MsgRq.SvcRq.DLCD = "1";
            txn.Rq.EAIBody.MsgRq.SvcRq.HCODE = "0";				//�󥿰O��

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
            txn.Rq.EAIBody.MsgRq.SvcRq.TXTYPE = $"{(int)PaymentNotification.TransferenceType.�s����}"; //loan.PaymentNotification.�{��O.GetEfficientString(0, 1);
            txn.Rq.EAIBody.MsgRq.SvcRq.PAYACTNO = loan.Reimbursement.PayableAccount.Replace("-", "");
            bRetrial = false;

            bool bResult = txn.Commit();

            if (!bResult)
            {
                response.Append(txn.RspCode)
                    .Append(":").Append(this.ReadErrCodeDis(txn));
                bRetrial = "3001".Equals(txn.RspCode);
                if (bRetrial)
                    response.Append(",�����w��ݵ��O");
            }

            return bResult;
        }

        private bool doL4200EAI(NegoLoanRepayment repayment, EAI.Service.Transaction.LR006_Rs.IFX rs, System.Text.StringBuilder response, out bool bRetrial, out Txn_L4200 txn, bool forCreditInsurance = false)
        {
            NegoLoan loan = repayment.NegoLoan;
            CreditInsuranceLoan insuranceLoan = loan.CreditInsuranceLoan;

            txn = new Txn_L4200();
            //�a�J�Ѽ�
            txn.Rq.EAIBody.MsgRq.SvcRq.KINBR = loan.Reimbursement.NegoDraft.LetterOfCreditVersion.LetterOfCredit?.CreditApplicationDocumentary.�}���� ?? loan.Reimbursement.NegoDraft.NegoLC?.IssuingBank; ;	//��J��
            txn.Rq.EAIBody.MsgRq.SvcRq.ACBRNO = txn.Rq.EAIBody.MsgRq.SvcRq.KINBR;
            txn.Rq.EAIBody.MsgRq.SvcRq.HCODE = "0";				//�󥿰O��
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
            txn.Rq.EAIBody.MsgRq.SvcRq.TXTYPE = loan.PaymentNotification.�{��O.GetEfficientString(0, 1);
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
                    response.Append(",�����w��ݵ��O");
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
                        item.�U��丹 = txn.Rs.EAIBody.MsgRs.SvcRs.COUNT;
                        mgr.SubmitChanges();
                    }
                }
                else
                {
                    Txn_L1201 txn;
                    bResult = doNegoL1201EAI(item, out rspCode, out bRetrial, out txn);
                    if (bResult)
                    {
                        item.�U��丹 = txn.Rs.EAIBody.MsgRs.SvcRs.ACTNO + txn.Rs.EAIBody.MsgRs.SvcRs.SQNO;
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
                rspCode = "�ײ���Ƥ��s�b!!";
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
                l4500.CAUSE = !String.IsNullOrEmpty(item.CancellationRegistry.�R�P��]) ? item.CancellationRegistry.�R�P��].Substring(0, 2) : null;
                l4500.TXAMT = String.Format("{0:0}", item.LetterOfCredit.�i�ξl�B);
                l4500.GRAMT = item.CancellationRegistry.�R�P�s�J�O�Ҫ����B.ToString();
                l4500.BPBKNO = item.LetterOfCredit.CreditApplicationDocumentary.�q����;
                l4500.TXTYPE = !String.IsNullOrEmpty(item.CancellationRegistry.���ڤ覡) ? item.CancellationRegistry.���ڤ覡.Substring(0, 1) : null;
                l4500.PACTNO = !String.IsNullOrEmpty(item.CancellationRegistry.�J��b��) ? item.CancellationRegistry.�J��b��.Replace("-", "") : null;
                l4500.GTXNO = item.LetterOfCredit.CreditApplicationDocumentary.OpeningApplicationDocumentary.����y����.ToString();
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
                this.Context.AbortTransaction(item.CancellationID, UserProfile.UserProfileRow.pid, (int)Naming.DocumentLevel.�ݵ��O);
            }

            if (!bResult && sbRspCode.Length > 0)
            {
                if (bRetrial)
                    sbRspCode.Append(",�����w��ݵ��O");
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
                ModelCore.Helper.Logger.Error("�L�kŪ��CHB���v���� => " + url + "\r\n" + ex);
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
                    this.Context.AbortTransaction(item.AcceptanceID, UserProfile.UserProfileRow.pid, (int)Naming.DocumentLevel.�ݵ��O);
                }

                rspCode = msg.ToString();
                return bResult;
            }
            else
            {
                rspCode = "�ӧI�ײ�����I�ڸ�Ƥ��s�b!!";
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
                    this.Context.AbortTransaction(item.ReimID, UserProfile.UserProfileRow.pid, (int)Naming.DocumentLevel.�ݵ��O);
                }

                cause = msg.ToString();
                return bResult;
            }
            else
            {
                cause = "�ٴڸ�Ƥ��s�b!!";
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
                if (item.TxnCode != (int)Naming.DocumentLevel.�w�}��)
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
                        item.TxnCode = (int)Naming.DocumentLevel.�w�}��;
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
                    if (loanItem.TxnCode != (int)Naming.DocumentLevel.�w�}��)
                    {
                        Txn_L1000 txn;

                        if (item.NegoLoan.CreditInsuranceLoan?.LoanPercentage >= 100m)
                        {
                            loanItem.TxnCode = (int)Naming.DocumentLevel.�w�}��;
                            this.SubmitChanges();
                        }
                        else
                        {
                            bResult = SendL1000EAI(item.NegoLoan, msg, out bRetrial, out txn);

                            if (bResult)
                            {
                                loanItem.TxnCode = (int)Naming.DocumentLevel.�w�}��;
                                this.SubmitChanges();

                                loanItem.ACTNO = txn.Rs.EAIBody.MsgRs.SvcRs.ACTNO;
                                if (item.NegoLoan.CreditInsuranceLoan != null && item.NegoLoan.CreditInsuranceLoan.L1000ID.HasValue)
                                {
                                    item.NegoLoan.CreditInsuranceLoan.L1000.�e�H�O�����b�丹 = txn.Rs.EAIBody.MsgRs.SvcRs.ACTNO;
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
                    this.Context.AbortTransaction(item.ReimID, UserProfile.UserProfileRow.pid, (int)Naming.DocumentLevel.�ݵ��O);
                }

                rspCode = msg.ToString();
                return bResult;
            }
            else
            {
                rspCode = "�ٴڧ�U��Ƥ��s�b!!";
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
                if (item.TxnCode != (int)Naming.DocumentLevel.�w�}��)
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
                        item.TxnCode = (int)Naming.DocumentLevel.�w�}��;
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
                    this.Context.AbortTransaction(item.RepaymentID, UserProfile.UserProfileRow.pid, (int)Naming.DocumentLevel.�ݵ��O);
                }

                rspCode = response.ToString();
                return bResult;
            }
            else
            {
                rspCode = "��U�ٴڸ�Ƥ��s�b!!";
                return false;
            }

        }

        public bool SendL8600EAI(Reimbursement item, StringBuilder msg, out bool bRetrial)
        {

            Txn_L8600 txn = new Txn_L8600();

            txn.Rq.EAIBody.MsgRq.SvcRq.SEQFG = "1";//	SEQFG	�s�C�Ǹ��O��, 9(01)
            txn.Rq.EAIBody.MsgRq.SvcRq.KINBR = UserProfile.UserProfileRow.branch;//	KINBR	��J��, 9(04)
            txn.Rq.EAIBody.MsgRq.SvcRq.HCODE = "0";//	HCODE	�󥿰O��, 9(01)
            //	ECKIN	�󥿿�J��O, 9(04)
            //	ECTRM	���d�i���Ǹ�, 9(04)
            //	ECTNO	�󥿥���Ǹ�, 9(08)
            txn.Rq.EAIBody.MsgRq.SvcRq.ACTNO = item.NegoDraft.LcID.HasValue ? item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.LcNo : item.NegoDraft.NegoLC.LCNo.Replace("-","");//	ACTNO	�b��, X(16)
            txn.Rq.EAIBody.MsgRq.SvcRq.CIFKEY = item.NegoDraft.LcID.HasValue ? item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo
                : item.NegoDraft.NegoLC.CustomerOfBranch.Organization.ReceiptNo; //	CIFKEY	�Τ@�s��, X(10)
            //	CIFERR	�Τ@�s�����~�N�X, X(1)
            txn.Rq.EAIBody.MsgRq.SvcRq.CAUSE = !String.IsNullOrEmpty(item.L8600.�R�P��]) ? item.L8600.�R�P��].Substring(0,2) : ""; //	CAUSE	�R�P��], 9(02)
            txn.Rq.EAIBody.MsgRq.SvcRq.TXAMT = item.L8600.�R�P�H�Ϊ����B.ToString();   //	TXAMT	�R�P���B, 9(12)V99
            txn.Rq.EAIBody.MsgRq.SvcRq.GRAMT = item.L8600.�R�P�s�J�O�Ҫ����B.ToString(); //	GRAMT	�R�P�s�J�O�Ҫ����B, 9(12)V99
            txn.Rq.EAIBody.MsgRq.SvcRq.VDATE = String.Format("{0:yyyy-MM-dd}", item.L8600.�Դڤ�); //	VDATE	�Դڤ�, (YYYY-MM-DD)
            txn.Rq.EAIBody.MsgRq.SvcRq.VRATE = item.L8600.�ԴڧQ�v.ToString();   //	VRATE	�ԴڧQ�v, 9(02)V9(05)
            txn.Rq.EAIBody.MsgRq.SvcRq.VINT = item.L8600.�Դڮ����B.ToString();  //	VINT	�Դڮ����B, 9(12)V99
            txn.Rq.EAIBody.MsgRq.SvcRq.DFCD = !String.IsNullOrEmpty(item.L8600.�H�����p���覡) ? item.L8600.�H�����p���覡.Substring(0, 1) : "";    //	DFCD	�H�����p���覡, 9(1)
            txn.Rq.EAIBody.MsgRq.SvcRq.DFAMT = item.L8600.�H�������B.ToString(); //	DFAMT	�H�������B, 9(12)V99
            //	SVEFEE	��U����O���B, 9(12)V99
            txn.Rq.EAIBody.MsgRq.SvcRq.TXTYPE = !String.IsNullOrEmpty(item.L8600.�{��O) ? item.L8600.�{��O.Substring(0, 1) : ""; //	TXTYPE	�{��O, 9(01)
            txn.Rq.EAIBody.MsgRq.SvcRq.PAYACTNO = !String.IsNullOrEmpty(item.L8600.���ڱb��) ? item.L8600.���ڱb��.Replace("-","") : "";  //	PAYACTNO	���ڱb��, 9(14)
            txn.Rq.EAIBody.MsgRq.SvcRq.CKAMT = item.L8600.��ú���B.ToString();  //	CKAMT	��ú���B, 9(12)V99
            txn.Rq.EAIBody.MsgRq.SvcRq.CKNO = item.L8600.�䲼���X;  //	CKNO	�䲼���X, 9(10)
            txn.Rq.EAIBody.MsgRq.SvcRq.CKBKNO7 = item.L8600.�I�ڦ�;    //	CKBKNO7	�I�ڦ�, 9(07)
            txn.Rq.EAIBody.MsgRq.SvcRq.CKACTNO = item.L8600.�X���H�b��;//	CKACTNO	�X���H�b��, 9(16)
            txn.Rq.EAIBody.MsgRq.SvcRq.CLSNOTE = !String.IsNullOrEmpty(item.L8600.���}���v�B�z���O) ? item.L8600.���}���v�B�z���O.Substring(0, 3) : ""; //	CLSNOTE	���}���v�B�z���O, X(3)
            txn.Rq.EAIBody.MsgRq.SvcRq.COUNT = item.L8600.�Դڬy����.ToString();
            txn.Rq.EAIBody.MsgRq.SvcRq.RECAMT = item.L8600.�ꦬ�`���B.ToString();

            bRetrial = false;

            bool bResult = txn.Commit();

            if (!bResult)
            {
                msg.Append("L8600�������:")
                    .Append(this.ReadErrCodeDis(txn));
                bRetrial = "3001".Equals(txn.RspCode);

                if(bRetrial)
                    msg.Append(",�����w��ݵ��O");
            }

            return bResult;

        }

        public bool SendL4600EAI(Reimbursement item, StringBuilder msg, out bool bRetrial, out Txn_L4600 txn)
        {
            txn = new Txn_L4600();

            txn.Rq.EAIBody.MsgRq.SvcRq.SEQFG = "1";//	SEQFG	�s�C�Ǹ��O��, 9(01)
            txn.Rq.EAIBody.MsgRq.SvcRq.KINBR = UserProfile.UserProfileRow.branch;//	KINBR	��J��, 9(04)
            txn.Rq.EAIBody.MsgRq.SvcRq.HCODE = "0";//	HCODE	�󥿰O��, 9(01)
            txn.Rq.EAIBody.MsgRq.SvcRq.ACTNO = item.L4600.ACTNO;//	ACTNO	�b��, X(16)
            txn.Rq.EAIBody.MsgRq.SvcRq.CURCD = "00";
            txn.Rq.EAIBody.MsgRq.SvcRq.TXAMT = item.Amount.ToString();
            //	ECKIN	�󥿿�J��O, 9(04)
            //	ECTRM	���d�i���Ǹ�, 9(04)
            //	ECTNO	�󥿥���Ǹ�, 9(08)
            txn.Rq.EAIBody.MsgRq.SvcRq.RBRNO = item.NegoDraft.LcID.HasValue ? item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.OpeningApplicationDocumentary.���Z�� : item.NegoDraft.NegoLC.IssuingBank;	//���Z��
            txn.Rq.EAIBody.MsgRq.SvcRq.CIFKEY = item.NegoDraft.LcID.HasValue ? item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo
                : item.NegoDraft.NegoLC.CustomerOfBranch.Organization.ReceiptNo; //	CIFKEY	�Τ@�s��, X(10)
            //	CIFERR	�Τ@�s�����~�N�X, X(1)

            txn.Rq.EAIBody.MsgRq.SvcRq.GRBAL = item.L4600.��s�J�O�Ҫ����B.ToString(); //	<GRBAL>��s�J�O�Ҫ����B, 9(12)V99</GRBAL>
            txn.Rq.EAIBody.MsgRq.SvcRq.VRATE = item.L4600.�ԴڧQ�v.ToString();   //	VRATE	�ԴڧQ�v, 9(02)V9(05)
            txn.Rq.EAIBody.MsgRq.SvcRq.VINT = item.L4600.�Դڮ����B.ToString();  //	VINT	�Դڮ����B, 9(12)V99
            txn.Rq.EAIBody.MsgRq.SvcRq.CAUSE = !String.IsNullOrEmpty(item.L4600.�ٴڭ�]) ? item.L4600.�ٴڭ�].Substring(0, 2) : null; //	CAUSE	�ٴڭ�], 9(02)
            txn.Rq.EAIBody.MsgRq.SvcRq.DFCD = !String.IsNullOrEmpty(item.L4600.�H�����p���覡) ? item.L4600.�H�����p���覡.Substring(0, 1) : null;    //	DFCD	�H�����p���覡, 9(1)
            txn.Rq.EAIBody.MsgRq.SvcRq.DFAMT = item.L4600.�H�������B.ToString(); //	DFAMT	�H�������B, 9(12)V99
            txn.Rq.EAIBody.MsgRq.SvcRq.GTXNO = item.NegoDraft.AppNo(); //	<GTXNO>�ײ����X, 9(10)</GTXNO>
            txn.Rq.EAIBody.MsgRq.SvcRq.TXTYPE = !String.IsNullOrEmpty(item.L4600.�{��O) ? item.L4600.�{��O.Substring(0, 1) : null; //	TXTYPE	�{��O, 9(01)
            txn.Rq.EAIBody.MsgRq.SvcRq.PACTNO = !String.IsNullOrEmpty(item.L4600.���ڱb��) ? item.L4600.���ڱb��.Replace("-","") : null;  //	PAYACTNO	���ڱb��, 9(14)
            txn.Rq.EAIBody.MsgRq.SvcRq.CKAMT = item.L4600.��ú���B.ToString();  //	CKAMT	��ú���B, 9(12)V99
            txn.Rq.EAIBody.MsgRq.SvcRq.CKNO = item.L4600.�䲼���X;  //	CKNO	�䲼���X, 9(10)
            txn.Rq.EAIBody.MsgRq.SvcRq.CKBKNO7 = "";    //	CKBKNO7	�I�ڦ�, 9(07)
            txn.Rq.EAIBody.MsgRq.SvcRq.CKACTNO = item.L4600.�X���H�b��;//	CKACTNO	�o���H�b��, 9(16)
            txn.Rq.EAIBody.MsgRq.SvcRq.CLSNOTE = !String.IsNullOrEmpty(item.L4600.���}���v�B�z���O) ? item.L4600.���}���v�B�z���O.Substring(0, 2) : null; //	CLSNOTE	���}���v�B�z���O, X(3)

            bRetrial = false;

            bool bResult = txn.Commit();

            if (!bResult)
            {
                msg.Append("L4600�������:")
                    .Append(this.ReadErrCodeDis(txn));
                bRetrial = "3001".Equals(txn.RspCode);

                if (bRetrial)
                    msg.Append(",�����w��ݵ��O");
            }

            return bResult;

        }

        private bool doL4700EAI(NegoDraftAcceptance item, StringBuilder msg, out bool bRetrial,out Txn_L4700 txn)
        {
            txn = new Txn_L4700();

            txn.Rq.EAIBody.MsgRq.SvcRq.SEQFG = "1";//	SEQFG	�s�C�Ǹ��O��, 9(01)
            txn.Rq.EAIBody.MsgRq.SvcRq.KINBR = UserProfile.UserProfileRow.branch;//	KINBR	��J��, 9(04)
            txn.Rq.EAIBody.MsgRq.SvcRq.ACTNO = item.L4700.ACTNO;
            txn.Rq.EAIBody.MsgRq.SvcRq.HCODE = "0";//	HCODE	�󥿰O��, 9(01)
            txn.Rq.EAIBody.MsgRq.SvcRq.CURCD = String.Format("{0:00}", item.L4700.���O);            //<CURCD>���O,9(02)</CURCD>
            txn.Rq.EAIBody.MsgRq.SvcRq.TXAMT = item.L4700.�ӧI���B.ToString();  //<TXAMT>���B, 9(12)V99</TXAMT>
            //<ACFLG>�t�X�����b�O��, X(01)</ACFLG>
            //<!-- 
            //0   ���t�X�����b�O���C
            //1   �t�X�����b�O���A�ä�������C
            //2   �t�X�����b�O���A�B��ITFS������C
            //-->
            //<ECKIN>�󥿿�J��O, 9(04)</ECKIN>
            //<ECTRM>���d�i���Ǹ�, 9(04)</ECTRM>
            //<ECTNO>�󥿥���Ǹ�, 9(08)</ECTNO>
            txn.Rq.EAIBody.MsgRq.SvcRq.RBRNO = item.NegoDraft.LcID.HasValue
                ? item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.OpeningApplicationDocumentary.���Z��
                : item.NegoDraft.NegoLC.IssuingBank;    //<RBRNO>���ݦ�</RBRNO>
            txn.Rq.EAIBody.MsgRq.SvcRq.CIFKEY = txn.Rq.EAIBody.MsgRq.SvcRq.CIFKEY = item.NegoDraft.LcID.HasValue ? item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo
                : item.NegoDraft.NegoLC.CustomerOfBranch.Organization.ReceiptNo;  //<CIFKEY>�Τ@�s��, X(10)</CIFKEY>
            //<CIFERR>�Τ@�s�����~�N�X, X(1)</CIFERR>
            //<!-- IT-TITA-TEXT -->
            txn.Rq.EAIBody.MsgRq.SvcRq.APTYPE = !String.IsNullOrEmpty(item.L4700.���ڤ覡) ? item.L4700.���ڤ覡.Substring(0, 1)
                : null; //<APTYPE>���ڤ覡,9(01)</APTYPE>
            txn.Rq.EAIBody.MsgRq.SvcRq.INACTNO = item.L4700.�J��b��.Replace("-","");   //<INACTNO>�J��b��,9(14)</INACTNO>
            txn.Rq.EAIBody.MsgRq.SvcRq.GTXNO = item.NegoDraft.AppNo();  //<GTXNO>�ײ����X,9(10)</GTXNO>
            txn.Rq.EAIBody.MsgRq.SvcRq.VRATE = item.L4700.�ԴڧQ�v.ToString();  //<VRATE>�ԴڧQ�v,9(02)V9(05)</VRATE>
            txn.Rq.EAIBody.MsgRq.SvcRq.IRTKD = !String.IsNullOrEmpty(item.L4700.�Q�v����) ? item.L4700.�Q�v����.Substring(0, 2)
                : null; //<IRTKD>�Q�v����,9(02)</IRTKD>
            txn.Rq.EAIBody.MsgRq.SvcRq.RATECD = !String.IsNullOrEmpty(item.L4700.�Q�v���A) ? item.L4700.�Q�v���A.Substring(0, 2)
                : null;//<RATECD>�Q�v���A,9(02)</RATECD>
            txn.Rq.EAIBody.MsgRq.SvcRq.IRTCD = !String.IsNullOrEmpty(item.L4700.�Q�v�O) ? item.L4700.�Q�v�O.Substring(0, 4)
                : null; //<IRTCD>�Q�v�O,9(04)</IRTCD>
            txn.Rq.EAIBody.MsgRq.SvcRq.FITIRT = item.L4700.�ӧ@�Q�v.ToString(); //<FITIRT>�ӧ@�Q�v,9(02)V9(05)</FITIRT>
            txn.Rq.EAIBody.MsgRq.SvcRq.SIGN = item.L4700.�[��X.HasValue
                ? item.L4700.�[��X >= 0 ? "+" : "-"
                : null;//<SIGN>�[��X���t��,X(01)</SIGN>
            txn.Rq.EAIBody.MsgRq.SvcRq.IXPR = item.L4700.�[��X.ToString();    //<IXPR>�[��X,9(02)V9(05)</IXPR>            

            bRetrial = false;

            bool bResult = txn.Commit();


            if (!bResult)
            {
                msg.Append("L4700�������:")
                    .Append(this.ReadErrCodeDis(txn));
                bRetrial = "3001".Equals(txn.RspCode);

                if (bRetrial)
                    msg.Append(",�����w��ݵ��O");
            }

            return bResult;

        }

        private bool doL2300EAI(Reimbursement item, StringBuilder msg, out bool bRetrial, out Txn_L2300 txn)
        {
            txn = new Txn_L2300();
            txn.Rq.EAIBody.MsgRq.SvcRq.KINBR = UserProfile.UserProfileRow.branch;//	KINBR	��J��, 9(04)
            txn.Rq.EAIBody.MsgRq.SvcRq.HCODE = "0";//	HCODE	�󥿰O��, 9(01)
            txn.Rq.EAIBody.MsgRq.SvcRq.ECKIN = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.ECTRM = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.ECTNO = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.ACTNO = item.NegoDraft.ACTNO();//	ACTNO	�b��, X(16)
            txn.Rq.EAIBody.MsgRq.SvcRq.CIFKEY = item.NegoDraft.LcID.HasValue ? item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo
                : item.NegoDraft.NegoLC.CustomerOfBranch.Organization.ReceiptNo; //	CIFKEY	�Τ@�s��, X(10)
            //txn.Rq.EAIBody.MsgRq.SvcRq.CAUSE = !String.IsNullOrEmpty(item.L4600.�ٴڭ�]) ? item.L4600.�ٴڭ�].Substring(0, 2) : null; //	CAUSE	�ٴڭ�], 9(02)
            txn.Rq.EAIBody.MsgRq.SvcRq.CAUSE = "4"; //	CAUSE	�ɦ���], 9(01)
            txn.Rq.EAIBody.MsgRq.SvcRq.INTFLG = "08"; //  INTFLG  �ɦ��N�X, 9(02)
            txn.Rq.EAIBody.MsgRq.SvcRq.INTAMT = item.ImposeHandlingCharge.ToString();
            txn.Rq.EAIBody.MsgRq.SvcRq.SINTAMT = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.RSINTAMT = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.CREAMT = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.REFAMT = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.RREFAMT = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.RECAMT = item.ImposeHandlingCharge.ToString();
            txn.Rq.EAIBody.MsgRq.SvcRq.TXTYPE = item.L8600ID.HasValue
                ? (!String.IsNullOrEmpty(item.L8600.�{��O) ? item.L8600.�{��O.Substring(0, 1) : "")
                : !String.IsNullOrEmpty(item.L4600.�{��O) ? item.L4600.�{��O.Substring(0, 1) : ""; //	TXTYPE	�{��O, 9(01)
            txn.Rq.EAIBody.MsgRq.SvcRq.PAYACTNO = item.PayableAccount.Replace("-", "");
            txn.Rq.EAIBody.MsgRq.SvcRq.CKAMT = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.CKNO = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.CKBKNO = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.CKACTNO = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.TNINTAMT = "";
            txn.Rq.EAIBody.MsgRq.SvcRq.IBRNO = item.NegoDraft.LcID.HasValue ? item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.�q����
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
                msg.Append("L2300�������:")
                    .Append(this.ReadErrCodeDis(txn));
                bRetrial = "3001".Equals(txn.RspCode);
                if (bRetrial)
                    msg.Append(",�D������O��");
            }

            return bResult;

        }


        public bool SendL1000EAI(NegoLoan item, StringBuilder msg, out bool bRetrial, out Txn_L1000 txn,bool forCreditInsurance = false)
        {
            txn = new Txn_L1000();

            L1000 dataItem = forCreditInsurance
                ? item.CreditInsuranceLoan.L1000
                : item.L1000;

            txn.Rq.EAIBody.MsgRq.SvcRq.KINBR = UserProfile.UserProfileRow.branch;	//��J��, 9(04)-->
            txn.Rq.EAIBody.MsgRq.SvcRq.ACTNO = dataItem.ACTNO;	//�b�丹, X(16)-->
            txn.Rq.EAIBody.MsgRq.SvcRq.HCODE = "0";	//�󥿰O��, 9(01)-->
            //txn.Rq.EAIBody.MsgRq.SvcRq.ECKIN = "";	//�󥿿�J��O, 9(04)-->
            //txn.Rq.EAIBody.MsgRq.SvcRq.ECTRM = "";	//���d�i���Ǹ�, 9(04)-->
            //txn.Rq.EAIBody.MsgRq.SvcRq.ECTNO = "";	//�󥿥���Ǹ�, 9(08)-->
            txn.Rq.EAIBody.MsgRq.SvcRq.RBRNO = item.Reimbursement.NegoDraft.LcID.HasValue ? item.Reimbursement.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.OpeningApplicationDocumentary.���Z�� : item.Reimbursement.NegoDraft.NegoLC.IssuingBank;	//���Z��, 9(04)-->
            txn.Rq.EAIBody.MsgRq.SvcRq.CIFKEY = item.Reimbursement.NegoDraft.LcID.HasValue ? item.Reimbursement.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo
                : item.Reimbursement.NegoDraft.NegoLC.CustomerOfBranch.Organization.ReceiptNo;	//�Τ@�s��, X(10)-->
            //txn.Rq.EAIBody.MsgRq.SvcRq.CIFERR = "";	//�Τ@�s�����~�N�X, X(01)-->
            if(forCreditInsurance)
            {
                txn.Rq.EAIBody.MsgRq.SvcRq.TXAMT = (item.CreditInsuranceLoan.LoanAmount ?? 0).ToString();	//�U����B, 9(12)V99-->
                txn.Rq.EAIBody.MsgRq.SvcRq.MGNO = item.Reimbursement.NegoDraft.LcID.HasValue 
                    ? item.Reimbursement.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.CustomerOfBranchExtension.InsuranceCreditNo
                    : item.Reimbursement.NegoDraft.NegoLC.CustomerOfBranch.CustomerOfBranchExtension.InsuranceCreditNo;	//�B�׸��X, 9(14)-->
            }
            else
            {
                txn.Rq.EAIBody.MsgRq.SvcRq.TXAMT = (item.Reimbursement.Amount - (item.CreditInsuranceLoan?.LoanAmount ?? 0)).ToString();	//�U����B, 9(12)V99-->
                txn.Rq.EAIBody.MsgRq.SvcRq.MGNO = item.PaymentNotification.�B�׸��X.Replace("-", "");	//�B�׸��X, 9(14)-->
            }
            txn.Rq.EAIBody.MsgRq.SvcRq.ACNO = dataItem.�|�p���;    //<ACNO>�|�p���, X(04)</ACNO>
            txn.Rq.EAIBody.MsgRq.SvcRq.SBNO = dataItem.�|�p�l��;  //<SBNO>�|�p�l��, 9(03)</SBNO>
            txn.Rq.EAIBody.MsgRq.SvcRq.GUNO = item.PaymentNotification.������Ҹ��X;	//<GUNO>������Ҹ��X	 X(20)</GUNO>	
            txn.Rq.EAIBody.MsgRq.SvcRq.FUNCD = item.PaymentNotification.�γ~�O.GetEfficientString(0, 1);	//<FUNCD>�γ~�O	 9(02)</FUNCD>	
            var seg = item.PaymentNotification.�b��ʽ�.Split('-');
            txn.Rq.EAIBody.MsgRq.SvcRq.CHARCD1 = seg[0];	//<CHARCD1>�b��ʽ�1	 9(03)</CHARCD1>	
            txn.Rq.EAIBody.MsgRq.SvcRq.CHARCD2 = seg[1];	//<CHARCD2>�b��ʽ�2	 9(03)</CHARCD2>	
            txn.Rq.EAIBody.MsgRq.SvcRq.APSDAY = String.Format("{0:yyyyMMdd}", item.PaymentNotification.���U���);	//<APSDAY>�U��_��	 9(08)</APSDAY>	
            txn.Rq.EAIBody.MsgRq.SvcRq.APEDAY = String.Format("{0:yyyyMMdd}", item.PaymentNotification.�U������);	//<APEDAY>�����	 9(08)</APEDAY>	
            txn.Rq.EAIBody.MsgRq.SvcRq.IMDAYS = item.PaymentNotification.�U�������.ToString();	//<IMDAYS>�U��Ѽ�	 9(03)</IMDAYS>	
            txn.Rq.EAIBody.MsgRq.SvcRq.IMMONS = item.PaymentNotification.�U�������.ToString();	//<IMMONS>�U����	 9(03)</IMMONS>	
            txn.Rq.EAIBody.MsgRq.SvcRq.PRCD = item.PaymentNotification.�٥��覡.GetEfficientString(0, 2);	//<PRCD>�٥��覡	 9(02)</PRCD>	
            txn.Rq.EAIBody.MsgRq.SvcRq.PRRATE = dataItem.�̫�@���٥���v.ToString();	//<PRRATE>�̫�@���٥���v	 9(03)</PRRATE>	
            txn.Rq.EAIBody.MsgRq.SvcRq.INTTM = dataItem.ú���g����.ToString();	//<INTTM>ú���g��(��)	 9(02)</INTTM>	
            txn.Rq.EAIBody.MsgRq.SvcRq.INTTW = dataItem.ú���g���g.ToString();	//<INTTW>ú���g��(�g)	 9(02)</INTTW>	
            txn.Rq.EAIBody.MsgRq.SvcRq.RATECD = dataItem.�Q�v���A.GetEfficientString(0, 2);	//<RATECD>�Q�v���A	 9(02)</RATECD>	
            txn.Rq.EAIBody.MsgRq.SvcRq.IRTCD = dataItem.�Q�v�O.GetEfficientString(0, 4); //<IRTCD>�Q�v�O	 9(04)</IRTCD>	
            txn.Rq.EAIBody.MsgRq.SvcRq.IRTFT = (dataItem.�ӧ@�Q�v - dataItem.�[��X).ToString();	//<IRTFT>�Q�v�O�Q�v	 9(2)V9(5)</IRTFT>	
            txn.Rq.EAIBody.MsgRq.SvcRq.SIGN = dataItem.�[��X >= 0 ? "+" : "-";	//<SIGN>���t��	 X(01)</SIGN>	
            txn.Rq.EAIBody.MsgRq.SvcRq.IXPR = Math.Abs(dataItem.�[��X.HasValue ? dataItem.�[��X.Value : 0m).ToString();	//<TIXPR>�[��X	 9(02)V9(05)	</TIXPR>
            txn.Rq.EAIBody.MsgRq.SvcRq.FITIRT = dataItem.�ӧ@�Q�v.ToString();	//<FITIRT>�ӧ@�Q�v	 9(02)V9(05)</FITIRT>	
            txn.Rq.EAIBody.MsgRq.SvcRq.IRTADJ = dataItem.�Q�v�վ�ͮĤ覡.GetEfficientString(0, 1);	//<IRTADJ>�Q�v�վ�ͮĤ覡	 9(01)</IRTADJ>	
            txn.Rq.EAIBody.MsgRq.SvcRq.IRTDM = dataItem.�p���g�����.GetEfficientString(0, 1);	//<IRTDM>����/��p��	 9(01)</IRTDM>	
            txn.Rq.EAIBody.MsgRq.SvcRq.ISTFLG = dataItem.�O�_�������.GetEfficientString(0, 1); 	//<ISTFLG>�O�_�������	 9(01)</ISTFLG>	
            txn.Rq.EAIBody.MsgRq.SvcRq.PRTM = dataItem.�٥��g����.ToString();	//<PRTM>�٥��g��(��)	 9(02)</PRTM>	
            txn.Rq.EAIBody.MsgRq.SvcRq.PRTW = dataItem.�٥��g���g.ToString();	//<PRTW>�٥��g��(�g)	 9(02)</PRTW>	
            txn.Rq.EAIBody.MsgRq.SvcRq.IRTTM = dataItem.�վ�Q�v�g��.ToString();	//<IRTTM>�վ�Q�v�g��(��)	 9(02)</IRTTM>	

            txn.Rq.EAIBody.MsgRq.SvcRq.CNIRDT = dataItem.���wú����.ToString();	//<CNIRDT>���wú����	 9(02)</CNIRDT>	
            txn.Rq.EAIBody.MsgRq.SvcRq.GRACEDAY = dataItem.�e����.ToString();	//<GRACEDAY>�e����	 9(03)</GRACEDAY>	
            txn.Rq.EAIBody.MsgRq.SvcRq.PRDATE = String.Format("{0:yyyyMMdd}", dataItem.�U���u�٤�);	//<PRDATE>�w�w�٥���	 9(08)</PRDATE>	
            txn.Rq.EAIBody.MsgRq.SvcRq.APTYPE = item.PaymentNotification.���ڤ覡.GetEfficientString(0, 1);	//<APTYPE>���ڤ覡	 9(01)</APTYPE>	
            txn.Rq.EAIBody.MsgRq.SvcRq.APACTNO = !String.IsNullOrEmpty(item.PaymentNotification.�J��b��) ? item.PaymentNotification.�J��b��.Replace("-", "") : null;	//<APACTNO>�J��b��	 9(14)</APACTNO>	
            txn.Rq.EAIBody.MsgRq.SvcRq.AUTCD = dataItem.�۰ʦ�ú�O��.GetEfficientString(0, 1); 	//<AUTCD>�۰ʦ�ú�O��	 9(01)</AUTCD>	
            txn.Rq.EAIBody.MsgRq.SvcRq.PAYACTNO = !String.IsNullOrEmpty(dataItem.�۰ʦ�ú�b��) ? dataItem.�۰ʦ�ú�b��.Replace("-", "") : null;	//<PAYACTNO>ú�ڱb��	 9(14)</PAYACTNO>	
            txn.Rq.EAIBody.MsgRq.SvcRq.MGENO = item.Reimbursement.Documentary.BankingAudit.�U��D�޽s��;	//<MGENO>�U��D��	 9(06)</MGENO>	
            txn.Rq.EAIBody.MsgRq.SvcRq.SPENO = item.Reimbursement.Documentary.BankingAudit.�U��t�d�H�s��;	//<SPENO>�U��t�d�H	 9(06)</SPENO>	
            txn.Rq.EAIBody.MsgRq.SvcRq.LNCD = dataItem.�U�񫬺A.GetEfficientString(0, 1); 	//<LNCD>�U�񫬺A	 X(01)</LNCD>	
            txn.Rq.EAIBody.MsgRq.SvcRq.NOTE = dataItem.�Ƶ�.GetEfficientString(0, 30); 	//<NOTE>�Ƶ�	 X(30)</NOTE>	
            if (forCreditInsurance)
            {
                txn.Rq.EAIBody.MsgRq.SvcRq.CRGRRATE = String.Format("{0:.}", item.PaymentNotification.�H�O����);    //<CRGRRATE>�H�O����	 9(03)</CRGRRATE>	
                txn.Rq.EAIBody.MsgRq.SvcRq.CRGRTYPE = item.PaymentNotification.�H�O����.GetEfficientString(0, 2);   //<CRGRTYPE>�H�O����	 9(02)</CRGRTYPE>	
                txn.Rq.EAIBody.MsgRq.SvcRq.CRACTNO = dataItem.�e�H�O�����b�丹; //<CRACTNO>�e�H�O�����b�丹	 9(16)</CRACTNO>	
            }
            else
            {
                txn.Rq.EAIBody.MsgRq.SvcRq.CRGRRATE = "";
                txn.Rq.EAIBody.MsgRq.SvcRq.CRGRTYPE = "";
                txn.Rq.EAIBody.MsgRq.SvcRq.CRACTNO = "";
            }
            txn.Rq.EAIBody.MsgRq.SvcRq.YEAR = dataItem.�M�צ~��.ToString();	//<YEAR>�M�צ~��	 9(02)</YEAR>	
            txn.Rq.EAIBody.MsgRq.SvcRq.SIFLG = dataItem.�p�U�O��.GetEfficientString(0, 1); 	//<SIFLG>�p�U�O��	 9(01)</SIFLG>	
            txn.Rq.EAIBody.MsgRq.SvcRq.SINO = dataItem.�p�U�s��;	//<SINO>�p�U�s��	 9(13)</SINO>	
            //txn.Rq.EAIBody.MsgRq.SvcRq.INTRQSP = dataItem.�O�_���T�w�Q�v;	//<INTRQSP>�O�_���T�w�Q�v	 9(01)</INTRQSP>	
            //txn.Rq.EAIBody.MsgRq.SvcRq.LABSTAT = dataItem.�ҶU�U�ڪ��A;	//<LABSTAT>�ҶU�U�ڪ��A	 9(01)</LABSTAT>	
            txn.Rq.EAIBody.MsgRq.SvcRq.RVSNO = item.PaymentNotification.�P�b�s��;	//<RVSNO>�P�b�s��	 X(10)</RVSNO>	
            txn.Rq.EAIBody.MsgRq.SvcRq.INTEXT = dataItem.ú���e����.ToString();	//<INTEXT>ú���e����	 9(03)</INTEXT>	
            txn.Rq.EAIBody.MsgRq.SvcRq.BEGINFEE = dataItem.�b��޲z�O.ToString();	//<BEGINFEE>�жU�b��޲z�O	 9(12)V9(02)</BEGINFEE>	
            txn.Rq.EAIBody.MsgRq.SvcRq.TXTYPE = item.PaymentNotification.�{��O.GetEfficientString(0, 1); 	//<TXTYPE>�{��O	 9(01)</TXTYPE>	
            txn.Rq.EAIBody.MsgRq.SvcRq.TAYACTNO = item.PaymentNotification.���ڱb��.Replace("-", "");	//<TAYACTNO>���ڱb��	 X(14)</TAYACTNO>	
            txn.Rq.EAIBody.MsgRq.SvcRq.LOANTYP = item.PaymentNotification.�ĸ�~�Ȥ���.GetEfficientString(0, 1);	//<LOANTYP>�ĸ�~�Ȥ���	 X(01)</LOANTYP>	
            txn.Rq.EAIBody.MsgRq.SvcRq.GOVSTYP = item.PaymentNotification.�F���M�׸ɧU�U�ڤ���.GetEfficientString(0, 2);	//<GOVSTYP>�F���M�׸ɧU�U��	 X(02)</GOVSTYP>	
            //txn.Rq.EAIBody.MsgRq.SvcRq.ZEDFRATE = dataItem.�s�Q�v�H�����Q�v;	//<ZEDFRATE>�s�Q�v�H�����Q�v	 9(02)V9(05)</ZEDFRATE>	
            txn.Rq.EAIBody.MsgRq.SvcRq.IRTKD = dataItem.�Q�v����.GetEfficientString(0, 2);	//<IRTKD>�Q�v����	 9(02)</IRTKD>	

            if (item.Reimbursement.L8600ID.HasValue)
            {
                txn.Rq.EAIBody.MsgRq.SvcRq.LCACTNO = item.Reimbursement.NegoDraft.LcID.HasValue
                    ? item.Reimbursement.NegoDraft.LetterOfCreditVersion.LetterOfCredit.LcNo
                    : item.Reimbursement.NegoDraft.NegoLC.LCNo.Replace("-", "");	//�H�Ϊ���U�b�丹, 9(16)-->
                txn.Rq.EAIBody.MsgRq.SvcRq.LCDCNT = item.Reimbursement.L8600.�Դڬy����.ToString();	//�H�Ϊ��Դڬy����, 9(04)-->
            }
            else
            {
                txn.Rq.EAIBody.MsgRq.SvcRq.LCACTNO = "";	//�H�Ϊ���U�b�丹, 9(16)-->
                txn.Rq.EAIBody.MsgRq.SvcRq.LCDCNT = "";	//�H�Ϊ��Դڬy����, 9(04)-->
            }
            txn.Rq.EAIBody.MsgRq.SvcRq.IDATE = "";	//�p�U�U��������,(YYYY-MM-DD)-->
            txn.Rq.EAIBody.MsgRq.SvcRq.SVEINT = "";	//�Q���ĸ�w���Q��(�x����Q���ĸ�ϥΡA�b��ʽ�352-141), 9(12)V99-->
            txn.Rq.EAIBody.MsgRq.SvcRq.PERINRATE = "";	//����f�t�ڤ��, 9(03)-->
            txn.Rq.EAIBody.MsgRq.SvcRq.OFUNDAMT = "";	//��l�����N���/���U�N��ڪ��B, 9(12)V99-->
            txn.Rq.EAIBody.MsgRq.SvcRq.REPYCD = "";	//�~�P�U�ڹw�p�ٴڨӷ�,9(02)-->
            txn.Rq.EAIBody.MsgRq.SvcRq.REPYCDX = "";	//�~�P�U�ڹw�p�ٴڨӷ�-��L����,X(22)-->
            txn.Rq.EAIBody.MsgRq.SvcRq.CONRATECD = "";	//�p�U�׭q���Q�v���A, 9(02)-->
            txn.Rq.EAIBody.MsgRq.SvcRq.FFIDATE = "";	//�U���Q�v�վ��,(YYYY-MM-DD)-->
            txn.Rq.EAIBody.MsgRq.SvcRq.MORPRCD = "";	//�e�������w�٥��O��, 9(01)-->
            txn.Rq.EAIBody.MsgRq.SvcRq.ISHLD = "";	//�O�_�J���s�O��, 9(01)-->
            txn.Rq.EAIBody.MsgRq.SvcRq.HLDAMT = "";	//��s���B, 9(12)V99-->
            txn.Rq.EAIBody.MsgRq.SvcRq.DFFLAG = "";	//���e�M�v�H�����O��, 9(01)-->
            txn.Rq.EAIBody.MsgRq.SvcRq.NXREFNO = "";	//�O���X�f����Ѧҽs��, X(16)-->
            txn.Rq.EAIBody.MsgRq.SvcRq.BTFLG = "";	//�O�_���N�v�ץ�(0�G�_�F1�G�O),9(01)-->
            txn.Rq.EAIBody.MsgRq.SvcRq.RULE = "";	//�O�_�ŦX�Ȧ�k72����2,9(01)-->
            txn.Rq.EAIBody.MsgRq.SvcRq.RULEX = "";	//��],9(01)-->
            txn.Rq.EAIBody.MsgRq.SvcRq.FITLOW = dataItem.�Q�v�U��.ToString();

            bRetrial = false;

            bool bResult = txn.Commit();

            if (!bResult)
            {
                msg.Append("L1000�������:")
                    .Append(this.ReadErrCodeDis(txn));
                bRetrial = "3001".Equals(txn.RspCode);
                if (bRetrial)
                    msg.Append(",�����w��ݵ��O");
            }

            return bResult;

        }


        #region ���y�{����--�Ұ�

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

        #region ���y�{����--�f��

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

        #region ���y�{����--���

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

        #region ���y�{����--����

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

        #region ���y�{����--�R��

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


        #region ���s���J�ݿ�ƶ�

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

        #region ���s���J�ݿ�ƶ�

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
