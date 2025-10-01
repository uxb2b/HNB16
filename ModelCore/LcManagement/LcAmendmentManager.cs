using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModelCore.DataModel;
using CommonLib.DataAccess;
using ModelCore.Locale;
using ModelCore.UserManagement;
using EAI.Service.Transaction;
using CommonLib.Utility;
using ModelCore.Service;

namespace ModelCore.LcManagement
{
    public class LcAmendmentManager : LcEntityManager<AmendingLcApplication>
	{
		public LcAmendmentManager() : base()
		{
			//
			// TODO: 在此加入建構函式的程式碼
			//
		}

        public LcAmendmentManager(GenericManager<LcEntityDataContext> mgr) : base(mgr) { }

        public bool ApproveAmendment(int? amendingID,UserProfile profile,String memo,String instruction)
        {
            var item = this.EntityList.Where(a => a.AmendingID == amendingID).FirstOrDefault();
            if (item != null && item.PaymentID.HasValue)
            {
                item.Documentary.DoApprove(Naming.DocumentLevel.待主管審核, profile.ProfileData.PID, memo);
                item.PaymentNotification.放款作業專員 = profile.ProfileData.USER_NAME;
                item.Instruction = instruction;

                this.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool VerifyAmendment(int? amendingID, UserProfile profile, String memo = null)
        {
            return VerifyAmendment(amendingID, profile, out AmendingLcApplication item, memo);
        }

        public bool VerifyAmendment(int? amendingID, UserProfile profile,out AmendingLcApplication item, String memo = null)
        {
            item = this.EntityList.Where(a => a.AmendingID == amendingID).FirstOrDefault();
            if (item != null && item.PaymentID.HasValue)
            {
                item.Documentary.DoApprove(Naming.DocumentLevel.待CRC登錄, profile.ProfileData.PID, memo);
                item.PaymentNotification.授信支援主管 = profile.ProfileData.USER_NAME;

                this.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool RegisterAmendment(int? amendingID, UserProfile profile, String memo)
        {
            var item = this.EntityList.Where(a => a.AmendingID == amendingID).FirstOrDefault();
            if (item != null && item.RegistrationID.HasValue)
            {
                item.Documentary.DoApprove(Naming.DocumentLevel.待放行, profile.ProfileData.PID, memo);
//                item.AmendingLcRegistry.放款作業專員 = profile.ProfileData.USER_NAME;

                this.SubmitChanges();

                this.ExecuteCommand("update AmendingLcRegistry set 放款作業專員 = {0} where RegistrationID = {1}", profile.ProfileData.USER_NAME, item.RegistrationID);

                return true;
            }
            return false;
        }

        public bool AllowAmendment(int? amendingID, UserProfile profile, out AmendingLcApplication item, String memo = null)
        {
            item = this.EntityList.Where(a => a.AmendingID == amendingID).FirstOrDefault();
            return AllowAmendment(item, profile);
        }

        public bool AllowAmendment(AmendingLcApplication item, UserProfile profile, String memo = null)
        {
            if (item != null && item.RegistrationID.HasValue)
            {
                var lcItem = new LetterOfCreditVersion
                {
                    VersionNo = item.LetterOfCreditVersion.VersionNo + 1,
                    LetterOfCredit = item.LetterOfCreditVersion.LetterOfCredit,
                };
                item.LetterOfCreditVersion.LetterOfCredit.LetterOfCreditVersion.Add(lcItem);

                decimal? availableAmt = null;
                try
                {
                    availableAmt = checkAvailableAmount(item.LetterOfCreditVersion.LetterOfCredit, profile);
                }
                catch (Exception ex)
                {
                    ModelCore.Helper.Logger.Error(ex, ModelCore.Helper.Logger.LogLevel.Error);
                }

                item.Documentary.DoApprove(Naming.DocumentLevel.已開立, profile.ProfileData.PID, String.Join(",", String.Format("LCNo:{0}", lcItem.LetterOfCredit.LcNo), memo));
                //item.AmendingLcRegistry.作業資訊組負責人 = profile.ProfileData.USER_NAME;
                item.PrintNotice = 1;

                DateTime now = DateTime.Now;

                item.AmendingLcInformation = new AmendingLcInformation
                {
                    AmendingDate = now,
                    InformationNo = String.Format("{0}-{1}", item.LetterOfCreditVersion.LetterOfCredit.LcNo, lcItem.VersionNo),
                };

                lcItem.NoteID = item.NoteID;
                lcItem.AttachmentID = item.AttachmentID;
                lcItem.ItemID = item.ItemID;
                lcItem.NotifyingBank = item.LetterOfCreditVersion.NotifyingBank;

                if (availableAmt.HasValue)
                {
                    lcItem.LetterOfCredit.可用餘額 = availableAmt;
                }

                this.SubmitChanges();

                this.ExecuteCommand("update AmendingLcRegistry set 作業資訊組負責人 = {0} where RegistrationID = {1}", profile.ProfileData.USER_NAME, item.RegistrationID);

                try
                {
                    if (item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.FileName != null)
                        OutboundSvc.SendAmendmentInfoToCDS(item);
                }
                catch (Exception ex)
                {
                    ModelCore.Helper.Logger.Error(ex, ModelCore.Helper.Logger.LogLevel.Error);
                }

                return true;
            }
            return false;
        }

        public bool DenyAmendmentWhenApproving(int? amendingID, UserProfile profile, string rejectReason, String instruction)
        {
            AmendingLcApplication item;
            if (denyApplication(amendingID, profile, rejectReason, Naming.DocumentLevel.銀行已拒絕, instruction, out item))
            {
                try
                {
                    if (item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.FileName != null)
                        OutboundSvc.SendRejection(item.Documentary, rejectReason);
                }
                catch (Exception ex)
                {
                    ModelCore.Helper.Logger.Error(ex, ModelCore.Helper.Logger.LogLevel.Error);
                }

                return true;
            }
            return false;
        }

        public bool DenyAmendmentWhenVerifying(int? appID, UserProfile profile, string rejectReason,out AmendingLcApplication item)
        {
            if (denyApplication(appID, profile, rejectReason, Naming.DocumentLevel.已退回_主管退回, out item))
            {
                int? paymentID = item.PaymentID;
                item.PaymentID = null;
                this.DeleteAnyOnSubmit<PaymentNotification>(p => p.PaymentID == paymentID);
                this.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool DenyAmendmentWhenRegistering(int? appID, UserProfile profile, string rejectReason)
        {
            AmendingLcApplication item;
            if (denyApplication(appID, profile, rejectReason, Naming.DocumentLevel.已退回_CRC退回, out item))
            {
                int? paymentID = item.PaymentID;
                item.PaymentID = null;
                this.DeleteAnyOnSubmit<PaymentNotification>(p => p.PaymentID == paymentID);

                int? registrationID = item.RegistrationID;
                item.RegistrationID = null;
                this.DeleteAllOnSubmit<AmendingLcRegistry>(r => r.RegistrationID == registrationID);
                this.SubmitChanges();

                return true;
            }
            return false;
        }

        public bool DenyAmendmentWhenAllowing(int? appID, UserProfile profile, string rejectReason,out AmendingLcApplication item)
        {
            item = this.EntityList.Where(a => a.AmendingID == appID).FirstOrDefault();
            if (item != null)
            {
                if (item.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.OverTheCounter == true)
                {
                    item.Documentary.DoDeny(Naming.DocumentLevel.臨櫃申請CRC主管退回, profile.ProfileData.PID, rejectReason);

                    int? paymentID = item.PaymentID;
                    item.PaymentID = null;
                    this.DeleteAnyOnSubmit<PaymentNotification>(p => p.PaymentID == paymentID);
                }
                else
                {
                    item.Documentary.DoDeny(Naming.DocumentLevel.已退回_CRC主管退回, profile.ProfileData.PID, rejectReason);

                }

                int? registrationID = item.RegistrationID;
                item.RegistrationID = null;
                this.DeleteAllOnSubmit<AmendingLcRegistry>(r => r.RegistrationID == registrationID);

                this.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool DenyAmendmentOverTheCounter(int? appID, UserProfile profile, string rejectReason)
        {
            AmendingLcApplication item;
            return denyApplication(appID, profile, rejectReason, Naming.DocumentLevel.臨櫃申請銀行已拒絕,out item);
        }

        protected bool denyApplication(int? amendingID, UserProfile profile, string rejectReason, Naming.DocumentLevel denyLevel, String instruction, out AmendingLcApplication item)
        {
            item = this.EntityList.Where(a => a.AmendingID == amendingID).FirstOrDefault();
            if (item != null)
            {
                item.Documentary.DoDeny(denyLevel, profile.ProfileData.PID, rejectReason);
                item.Instruction = instruction;

                this.SubmitChanges();

                return true;
            }
            return false;
        }

        protected bool denyApplication(int? amendingID, UserProfile profile, string rejectReason,Naming.DocumentLevel denyLevel,out AmendingLcApplication item)
        {
            item = this.EntityList.Where(a => a.AmendingID == amendingID).FirstOrDefault();
            if (item != null)
            {
                item.Documentary.DoDeny(denyLevel, profile.ProfileData.PID, rejectReason);

                this.SubmitChanges();

                return true;
            }
            return false;
        }

        private decimal? checkAvailableAmount(LetterOfCredit item,UserProfile profile)
        {
            decimal? availableAmt = null;
            Txn_LR017 txn = new Txn_LR017();
            txn.Account = item.LcNo.Replace("-", "").Trim();
            if (txn.Commit())
            {
                availableAmt = decimal.Parse(txn.SQBAL);
            }
            return availableAmt;
        }
    }
}
