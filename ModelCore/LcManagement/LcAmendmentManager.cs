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
    public class LcAmendmentManager : LcEntityManager<AmendingLcApplication>
	{
		public LcAmendmentManager() : base()
		{
			//
			// TODO: 在此加入建構函式的程式碼
			//
		}

        public LcAmendmentManager(GenericManager<LcEntityDbContext> mgr) : base(mgr) { }

        public bool ApproveAmendment(int? amendingID,UserProfile profile,String memo,String instruction)
        {
            var item = this.EntityList.Where(a => a.DocumentaryID == amendingID).FirstOrDefault();
            if (item != null)
            {
                item.Documentary.DoApprove(Naming.DocumentLevel.待主管審核, profile.ProfileData.PID, memo);
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
            item = this.EntityList.Where(a => a.DocumentaryID == amendingID).FirstOrDefault();
            if (item != null)
            {
                item.Documentary.DoApprove(Naming.DocumentLevel.待CRC登錄, profile.ProfileData.PID, memo);

                this.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool RegisterAmendment(int? amendingID, UserProfile profile, String memo)
        {
            var item = this.EntityList.Where(a => a.DocumentaryID == amendingID).FirstOrDefault();
            if (item != null)
            {
                item.Documentary.DoApprove(Naming.DocumentLevel.待放行, profile.ProfileData.PID, memo);
//                item.AmendingLcRegistry.放款作業專員 = profile.ProfileData.USER_NAME;

                this.SubmitChanges();

                return true;
            }
            return false;
        }

        public bool AllowAmendment(int? amendingID, UserProfile profile, out AmendingLcApplication item, String memo = null)
        {
            item = this.EntityList.Where(a => a.DocumentaryID == amendingID).FirstOrDefault();
            return AllowAmendment(item, profile);
        }

        public bool AllowAmendment(AmendingLcApplication item, UserProfile profile, String memo = null)
        {
            if (item != null)
            {
                var lcItem = new LetterOfCreditVersion
                {
                    VersionNo = item.Source.VersionNo + 1,
                    Lc = item.Source.Lc,
                };
                item.Source.Lc.LetterOfCreditVersion.Add(lcItem);

                decimal? availableAmt = null;
                try
                {
                    availableAmt = checkAvailableAmount(item.Source.Lc, profile);
                }
                catch (Exception ex)
                {
                    ModelCore.Helper.Logger.Error(ex, ModelCore.Helper.Logger.LogLevel.Error);
                }

                item.Documentary.DoApprove(Naming.DocumentLevel.已開立, profile.ProfileData.PID, String.Join(",", String.Format("LCNo:{0}", lcItem.Lc.LcNo), memo));
                //item.AmendingLcRegistry.作業資訊組負責人 = profile.ProfileData.USER_NAME;
                item.PrintNotice = 1;

                DateTime now = DateTime.Now;

                item.AmendingLcInformation = new AmendingLcInformation
                {
                    AmendingDate = now,
                    InformationNo = String.Format("{0}-{1}", item.Source.Lc.LcNo, lcItem.VersionNo),
                };

                lcItem.SpecificNotesID = item.SpecificNotesID;
                lcItem.AttachableDocumentID = item.AttachableDocumentID;
                lcItem.LcItemsID = item.LcItemsID;
                lcItem.NotifyingBank = item.Source.NotifyingBank;

                if (availableAmt.HasValue)
                {
                    lcItem.Lc.可用餘額 = availableAmt;
                }

                this.SubmitChanges();

                try
                {
                    if (item.Source.Lc.Application.FileName != null)
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
                    if (item.Source.Lc.Application.FileName != null)
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
                this.SubmitChanges();
                return true;
            }
            return false;
        }


        public bool DenyAmendmentWhenAllowing(int? appID, UserProfile profile, string rejectReason,out AmendingLcApplication item)
        {
            item = this.EntityList.Where(a => a.DocumentaryID == appID).FirstOrDefault();
            if (item != null)
            {
                if (item.Source.Lc.Application.OverTheCounter == true)
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

        public bool DenyAmendmentOverTheCounter(int? appID, UserProfile profile, string rejectReason)
        {
            AmendingLcApplication item;
            return denyApplication(appID, profile, rejectReason, Naming.DocumentLevel.臨櫃申請銀行已拒絕,out item);
        }

        protected bool denyApplication(int? amendingID, UserProfile profile, string rejectReason, Naming.DocumentLevel denyLevel, String instruction, out AmendingLcApplication item)
        {
            item = this.EntityList.Where(a => a.DocumentaryID == amendingID).FirstOrDefault();
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
            item = this.EntityList.Where(a => a.DocumentaryID == amendingID).FirstOrDefault();
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
