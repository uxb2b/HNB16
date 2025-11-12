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
    public class LcApplicationManager : LcEntityManager<CreditApplicationDocumentary>
	{
		public LcApplicationManager() : base()
		{
			//
			// TODO: 在此加入建構函式的程式碼
			//
		}

        public LcApplicationManager(GenericManager<LcEntityDbContext> mgr) : base(mgr) { }

        public bool ApproveLcApplication(int? appID,UserProfile profile,String memo,String instruction)
        {
            var item = this.EntityList.Where(a => a.DocumentaryID == appID).FirstOrDefault();
            if (item != null)
            {
                item.Documentary.DoApprove(Naming.DocumentLevel.待主管審核, profile.ProfileData.PID, memo);
                item.Instrunction = instruction;

                this.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool VerifyLcApplication(int? appID, UserProfile profile,String memo = null)
        {
            return VerifyLcApplication(appID, profile, out CreditApplicationDocumentary item, memo);
        }

        public bool VerifyLcApplication(int? appID, UserProfile profile,out CreditApplicationDocumentary item, String memo = null)
        {
            item = this.EntityList.Where(a => a.DocumentaryID == appID).FirstOrDefault();
            if (item != null)
            {
                item.Documentary.DoApprove(Naming.DocumentLevel.待CRC登錄, profile.ProfileData.PID, memo);
                this.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool RegisterLcApplication(int? appID, UserProfile profile, String memo, String instruction = null)
        {
            var item = this.EntityList.Where(a => a.DocumentaryID == appID).FirstOrDefault();
            if (item != null)
            {
                item.Documentary.DoApprove(Naming.DocumentLevel.待放行, profile.ProfileData.PID, memo);
                if (instruction != null)
                {
                    item.Instrunction = instruction;
                }

                this.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool AllowLcApplication(int? appID, UserProfile profile, String lcNo)
        {
            var item = this.EntityList.Where(a => a.DocumentaryID == appID).FirstOrDefault();
            return AllowLcApplication(item, profile, lcNo);
        }

        public bool AllowLcApplication(CreditApplicationDocumentary item, UserProfile profile, String lcNo, String memo = null)
        {
            if (item != null)
            {
                //this.Context.CreateLetterOfCredit(item.DocumentaryID, lcNo);

                item.Documentary.DoApprove(Naming.DocumentLevel.已開立, profile.ProfileData.PID, String.Join(",", String.Format("LCNo:{0}", lcNo), memo));
                //item.OpeningApplicationDocumentary.作業資訊組負責人 = profile.USER_NAME;

                this.SubmitChanges();

                try
                {
                    if (!String.IsNullOrEmpty(item.FileName))
                    {
                        OutboundSvc.SendLcToCDS(item);
                    }
                }
                catch (Exception ex)
                {
                    ModelCore.Helper.Logger.Error(ex, ModelCore.Helper.Logger.LogLevel.Error);
                }

                return true;
            }
            return false;
        }

        public bool DenyLcApplicationWhenApproving(int? appID, UserProfile profile, string rejectReason,String instruction)
        {
            CreditApplicationDocumentary item;
            if (denyApplication(appID, profile, rejectReason, Naming.DocumentLevel.銀行已拒絕, instruction, out item))
            {
                try
                {
                    if (item.FileName != null)
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

        public bool DenyLcApplicationWhenVerifying(int? appID, UserProfile profile, string rejectReason,out CreditApplicationDocumentary item)
        {
            if (denyApplication(appID, profile, rejectReason, Naming.DocumentLevel.已退回_主管退回, out item))
            {
                //int? paymentID = item.PaymentID;
                //item.PaymentID = null;
                //this.DeleteAnyOnSubmit<PaymentNotification>(p => p.PaymentID == paymentID);
                //this.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool DenyLcApplicationWhenRegistering(int? appID, UserProfile profile, string rejectReason)
        {
            CreditApplicationDocumentary item;
            if (denyApplication(appID, profile, rejectReason, Naming.DocumentLevel.已退回_CRC退回, out item))
            {
                //int? paymentID = item.PaymentID;
                //item.PaymentID = null;
                //this.DeleteAnyOnSubmit<PaymentNotification>(p => p.PaymentID == paymentID);

                this.SubmitChanges();

                return true;
            }
            return false;
        }

        public bool DenyLcApplicationWhenAllowing(int? appID, UserProfile profile, string rejectReason)
        {
            var item = this.EntityList.Where(a => a.DocumentaryID == appID).FirstOrDefault();
            if (item != null)
            {
                if (item.OverTheCounter == true)
                {
                    item.Documentary.DoDeny(Naming.DocumentLevel.臨櫃申請CRC主管退回, profile.ProfileData.PID, rejectReason);
                }
                else
                {
                    item.Documentary.DoDeny(Naming.DocumentLevel.已退回_CRC主管退回, profile.ProfileData.PID, rejectReason);

                }
                this.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool DenyLcApplicationOverTheCounter(int? appID, UserProfile profile, string rejectReason)
        {
            CreditApplicationDocumentary item;
            return denyApplication(appID, profile, rejectReason, Naming.DocumentLevel.臨櫃申請銀行已拒絕,out item);
        }


        protected bool denyApplication(int? appID, UserProfile profile, string rejectReason, Naming.DocumentLevel denyLevel,String instruction,out CreditApplicationDocumentary item)
        {
            item = this.EntityList.Where(a => a.DocumentaryID == appID).FirstOrDefault();
            if (item != null)
            {
                item.Documentary.DoDeny(denyLevel, profile.ProfileData.PID, rejectReason);
                item.Instrunction = instruction;

                this.SubmitChanges();

                return true;
            }
            return false;
        }

        protected bool denyApplication(int? appID, UserProfile profile, string rejectReason,Naming.DocumentLevel denyLevel,out CreditApplicationDocumentary item)
        {
            item = this.EntityList.Where(a => a.DocumentaryID == appID).FirstOrDefault();
            if (item != null)
            {
                item.Documentary.DoDeny(denyLevel, profile.ProfileData.PID, rejectReason);

                this.SubmitChanges();

                return true;
            }
            return false;
        }
    }
}
