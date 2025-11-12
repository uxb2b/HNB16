using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModelCore.DataModel;
using CommonLib.Core.DataWork;
using ModelCore.Locale;
using ModelCore.UserManagement;
using EAI.Service.Transaction;
using CommonLib.Utility;
using ModelCore.Service;

namespace ModelCore.LcManagement
{
    public class CreditCancellationManager : LcEntityManager<CreditCancellation>
	{
		public CreditCancellationManager() : base()
		{
			//
			// TODO: 在此加入建構函式的程式碼
			//
		}

        public CreditCancellationManager(GenericManager<LcEntityDbContext> mgr) : base(mgr) { }

        public bool AcceptCancellation(int? cancellationID, UserProfile profile, String memo)
        {
            var item = this.EntityList.Where(a => a.DocumentaryID == cancellationID).FirstOrDefault();
            if (item != null && item.RegistrationID.HasValue)
            {
                item.Documentary.DoApprove(Naming.DocumentLevel.待主管審核, profile.ProfileData.PID, memo);

                this.SubmitChanges();
                return true;
            }
            return false;
        }

        //public bool VerifyAmendment(int? amendingID, UserProfile profile)
        //{
        //    var item = this.EntityList.Where(a => a.DocumentaryID == amendingID).FirstOrDefault();
        //    if (item != null && item.PaymentID.HasValue)
        //    {
        //        DateTime now = DateTime.Now;
        //        item.Documentary.DocumentaryAllowance.Add(
        //            new DocumentaryAllowance
        //            {
        //                Approver = profile.ProfileData.PID,
        //                ApprovalDate = now
        //            });

        //        item.Documentary.DocumentaryLevel.Add(new DocumentaryLevel
        //        {
        //            DocLevel = (int)Naming.DocumentLevel.待CRC登錄,
        //            LevelDate = now
        //        });

        //        item.Documentary.CurrentLevel = (int)Naming.DocumentLevel.待CRC登錄;
        //        item.PaymentNotification.授信支援主管 = profile.ProfileData.USER_NAME;

        //        this.SubmitChanges();
        //        return true;
        //    }
        //    return false;
        //}

        //public bool RegisterAmendment(int? amendingID, UserProfile profile, String memo)
        //{
        //    var item = this.EntityList.Where(a => a.DocumentaryID == amendingID).FirstOrDefault();
        //    if (item != null && item.RegistrationID.HasValue)
        //    {
        //        DateTime now = DateTime.Now;
        //        item.Documentary.DocumentaryAllowance.Add(
        //            new DocumentaryAllowance
        //            {
        //                Approver = profile.ProfileData.PID,
        //                ApprovalDate = now,
        //                Memo = memo
        //            });

        //        item.Documentary.DocumentaryLevel.Add(new DocumentaryLevel
        //        {
        //            DocLevel = (int)Naming.DocumentLevel.待放行,
        //            LevelDate = now
        //        });

        //        item.Documentary.CurrentLevel = (int)Naming.DocumentLevel.待放行;

        //        this.SubmitChanges();
        //        return true;
        //    }
        //    return false;
        //}

        public bool AllowCancellation(int? cancellationID, UserProfile profile, out CreditCancellation item)
        {
            item = this.EntityList.Where(a => a.DocumentaryID == cancellationID).FirstOrDefault();
            return AllowCancellation(item, profile);
        }

        public bool AllowCancellation(CreditCancellation item, UserProfile profile, String memo = null)
        {
            if (item != null && item.RegistrationID.HasValue)
            {

                item.Documentary.DoApprove(Naming.DocumentLevel.已註銷, profile.ProfileData.PID, memo);
                //item.CancellationRegistry.作業資訊組負責人 = profile.ProfileData.USER_NAME;

                var info = item.CreditCancellationInfo;
                if (info == null)
                {
                    info = new CreditCancellationInfo
                    {
                        CancellationID = item.DocumentaryID,
                        CancellationDate = DateTime.Now
                    };

                    this.GetTable<CreditCancellationInfo>().Add(info);
                }

                this.SubmitChanges();

                this.ExecuteCommand("update NegoDraftRegistry set 作業資訊組負責人 = {0} where RegisterID = {1}", profile.ProfileData.USER_NAME, item.DocumentaryID);

                try
                {
                    if (item.Lc.Application.FileName != null)
                        OutboundSvc.SendLcCancellationToCDS(item);
                }
                catch (Exception ex)
                {
                    ModelCore.Helper.Logger.Error(ex, ModelCore.Helper.Logger.LogLevel.Error);
                }

                return true;
            }
            return false;
        }

        public bool DenyCancellationWhenApproving(int? cancellationID, UserProfile profile, string rejectReason)
        {
            CreditCancellation item;
            if (denyApplication(cancellationID, profile, rejectReason, Naming.DocumentLevel.銀行已拒絕, out item))
            {
                try
                {
                    if (item.Lc.Application.FileName != null)
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

        //public bool DenyAmendmentWhenVerifying(int? appID, UserProfile profile, string rejectReason)
        //{
        //    return denyApplication(appID, profile, rejectReason, Naming.DocumentLevel.已退回_主管退回);
        //}

        //public bool DenyAmendmentWhenRegistering(int? appID, UserProfile profile, string rejectReason)
        //{
        //    return denyApplication(appID, profile, rejectReason, Naming.DocumentLevel.已退回_CRC退回);
        //}


        protected bool denyApplication(int? cancellationID, UserProfile profile, string rejectReason, Naming.DocumentLevel denyLevel, out CreditCancellation item)
        {
            item = this.EntityList.Where(a => a.DocumentaryID == cancellationID).FirstOrDefault();
            if (item != null)
            {
                item.Documentary.DoDeny(denyLevel, profile.ProfileData.PID, rejectReason);

                this.SubmitChanges();

                return true;
            }
            return false;
        }

        //protected bool denyApplication(int? amendingID, UserProfile profile, string rejectReason,Naming.DocumentLevel denyLevel)
        //{
        //    var item = this.EntityList.Where(a => a.DocumentaryID == amendingID).FirstOrDefault();
        //    if (item != null)
        //    {
        //        doDeny(profile, rejectReason, denyLevel, item);

        //        this.SubmitChanges();

        //        return true;
        //    }
        //    return false;
        //}


        //private decimal? checkAvailableAmount(NegoLcVersion item,UserProfile profile)
        //{
        //    decimal? availableAmt = null;
        //    Txn_LR017 txn = new Txn_LR017();
        //    txn.Account = item.LcNo.Replace("-", "").Trim();
        //    if (txn.Commit())
        //    {
        //        availableAmt = decimal.Parse(txn.SQBAL);
        //    }
        //    return availableAmt;
        //}
    }
}
