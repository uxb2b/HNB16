using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;



using CommonLib.Core.DataWork;

using EAI.Service.Transaction;
using ModelCore.UserManagement;
using CommonLib.Utility;

using ModelCore.NegoManagement;
using ModelCore.DataModel;
using ModelCore.Helper;
using ModelCore.Properties;
using ModelCore.Locale;

namespace ModelCore.BankManagement
{
	/// <summary>
	/// Bussiness 的摘要描述。
	/// </summary>
	public  class BusinessManager  : LcEntityManager<CreditApplicationDocumentary>
	{
		public BusinessManager() : base()
		{
			//
			// TODO: 在此加入建構函式的程式碼
			//
		}

        public BusinessManager(GenericManager<LcEntityDbContext> mgr) : base(mgr) { }

        public UserManagement.UserProfile UserProfile { get; set; }

        public BusinessManager(UserProfile userProfile)
            : this()
        {
            //
            // TODO: 在此加入建構函式的程式碼
            //
            UserProfile = userProfile;
        }

		public Organization GetBusinessDetail(String receiptNo)
		{
            return this.GetTable<Organization>().Where(o => o.ReceiptNo == receiptNo).FirstOrDefault();
		}


        public bool ApproveLcApplication(int? appID, String approver, Naming.DocumentLevel docLevel)
        {
            var item = this.GetTable<CreditApplicationDocumentary>().Where(a => a.DocumentaryID == appID).FirstOrDefault();
            if (item != null)
            {
                item.ApplicationDate = DateTime.Now;
                item.Documentary.DoApprove(docLevel, approver, null);
                this.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool ApproveAmendmentApplication(int? amendingID, String approver, Naming.DocumentLevel docLevel)
        {
            var item = this.GetTable<AmendingLcApplication>().Where(a => a.DocumentaryID == amendingID).FirstOrDefault();
            if (item != null)
            {
                item.ApplicationDate = DateTime.Now;
                item.Documentary.DoApprove(docLevel, approver, null);
                this.SubmitChanges();
                return true;
            }
            return false;
        }


        public bool ApproveCreditCancellation(int? cancellationID, String approver, Naming.DocumentLevel docLevel)
        {
            var item = this.GetTable<CreditCancellation>().Where(a => a.DocumentaryID == cancellationID).FirstOrDefault();
            if (item != null)
            {
                item.申請日期 = DateTime.Now;
                item.Documentary.DoApprove(docLevel, approver, null);
                this.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool ApproveNegoDraft(int? draftID, String approver, Naming.DocumentLevel docLevel)
        {
            var item = this.GetTable<NegoDraft>().Where(a => a.DocumentaryID == draftID).FirstOrDefault();
            if (item != null)
            {
                item.NegoDate = DateTime.Now;
                item.Documentary.DoApprove(docLevel, approver, null);
                this.SubmitChanges();
                return true;
            }
            return false;
        }

    }


}
