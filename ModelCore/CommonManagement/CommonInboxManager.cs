using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using CommonLib.DataAccess;
using ModelCore.DataModel;
using ModelCore.Helper;
using ModelCore.Locale;

namespace ModelCore.CommonManagement
{
	/// <summary>
	/// InboxManagerDALC 的摘要描述。
	/// </summary>
	public class CommonInboxManager : LcEntityManager<MessageType>
	{
		public CommonInboxManager() : base()
		{
			//
			// TODO: 在此加入建構函式的程式碼
			//
		}

        public CommonInboxManager(GenericManager<LcEntityDataContext> mgr) : base(mgr) { }

        public String GetMailReceipentOfApplicant(int? docID)
        {
            var item = this.GetTable<Documentary>().Where(d => d.DocID == docID).FirstOrDefault();

            if (item != null)
            {
                switch (item.DocType)
                {
                    case (int)Naming.DocumentTypeDefinition.開狀申請書:
                        return item.CreditApplicationDocumentary.ApplicantDetails.ContactEmail;
                    case (int)Naming.DocumentTypeDefinition.修狀申請書:
                        return item.AmendingLcApplication.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.ApplicantDetails.ContactEmail;
                    case (int)Naming.DocumentTypeDefinition.信用狀註銷申請書:
                        return item.CreditCancellation.LetterOfCredit.CreditApplicationDocumentary.ApplicantDetails.ContactEmail;
                    case (int)Naming.DocumentTypeDefinition.押匯申請書:
                        if (item.NegoDraft.LC_ID.HasValue)
                        {
                            return item.NegoDraft.NegoLC.ApplicantDetails.ContactEmail;
                        }
                        else
                        {
                            return item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.ApplicantDetails.ContactEmail;
                        }
                    case (int)Naming.DocumentTypeDefinition.還款改貸申請書:
                        return item.Reimbursement.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.ApplicantDetails.ContactEmail;

                }
            }
            return null;
        }

        public String GetMailReceipentOfBank(int? docID)
        {
            var item = this.GetTable<Documentary>().Where(d => d.DocID == docID).FirstOrDefault();

            if (item != null)
            {
                switch (item.DocType)
                {
                    case (int)Naming.DocumentTypeDefinition.開狀申請書:
                        return String.Format("chb{0}@ms1.chb.com.tw,chb{1}@ms1.chb.com.tw",
                            item.CreditApplicationDocumentary.開狀行, item.CreditApplicationDocumentary.通知行);
                    case (int)Naming.DocumentTypeDefinition.修狀申請書:
                        return String.Format("chb{0}@ms1.chb.com.tw,chb{1}@ms1.chb.com.tw",
                            item.AmendingLcApplication.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.開狀行, item.AmendingLcApplication.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.通知行);
                    case (int)Naming.DocumentTypeDefinition.信用狀註銷申請書:
                        return String.Format("chb{0}@ms1.chb.com.tw",
                            item.CreditCancellation.LetterOfCredit.CreditApplicationDocumentary.開狀行);
                    case (int)Naming.DocumentTypeDefinition.押匯申請書:
                        if (item.NegoDraft.LC_ID.HasValue)
                        {
                            return String.Format("chb{0}@ms1.chb.com.tw,chb{1}@ms1.chb.com.tw",
                                item.NegoDraft.NegoLC.IssuingBank, item.NegoDraft.NegoLC.AdvisingBank);
                        }
                        else
                        {
                            return String.Format("chb{0}@ms1.chb.com.tw,chb{1}@ms1.chb.com.tw",
                                item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.開狀行, item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.通知行);
                        }
                    case (int)Naming.DocumentTypeDefinition.還款改貸申請書:
                        return String.Format("chb{0}@ms1.chb.com.tw,chb{1}@ms1.chb.com.tw",
                            item.Reimbursement.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.開狀行, item.Reimbursement.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.通知行);
                }
            }
            return null;
        }

        public string GetMailReceipentOfBeneficiary(int? docID)
        {
            var item = this.GetTable<Documentary>().Where(d => d.DocID == docID).FirstOrDefault();

            if (item != null)
            {
                switch (item.DocType)
                {
                    case (int)Naming.DocumentTypeDefinition.開狀申請書:
                        return item.CreditApplicationDocumentary.BeneDetails.ContactEmail;
                    case (int)Naming.DocumentTypeDefinition.修狀申請書:
                        return item.AmendingLcApplication.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.BeneDetails.ContactEmail;
                    case (int)Naming.DocumentTypeDefinition.信用狀註銷申請書:
                        return item.CreditCancellation.LetterOfCredit.CreditApplicationDocumentary.BeneDetails.ContactEmail;
                    case (int)Naming.DocumentTypeDefinition.押匯申請書:
                        if (item.NegoDraft.LC_ID.HasValue)
                        {
                            return item.NegoDraft.NegoLC.BeneDetails.ContactEmail;
                        }
                        else
                        {
                            return item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.BeneDetails.ContactEmail;
                        }
                    case (int)Naming.DocumentTypeDefinition.還款改貸申請書:
                        return item.Reimbursement.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.BeneDetails.ContactEmail;

                }
            }
            return null;
        }

        public String GetMailReceipentForCustomer(int? docID)
        {
            return this.GetMailReceipentOfApplicant(docID)
                .JoinEmail(this.GetMailReceipentOfBeneficiary(docID));
        }

        public void CreateInboxForApplicant(int? docID,Naming.MessageTypeDefinition typeID)
        {
            var item = this.GetTable<Documentary>().Where(d => d.DocID == docID).FirstOrDefault();

            if (item != null)
            {
                String receiptNo = null;
                int? companyID = null;
                switch (item.DocType)
                {
                    case (int)Naming.DocumentTypeDefinition.開狀申請書:
                        receiptNo = item.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo;
                        companyID = item.CreditApplicationDocumentary.申請人;
                        break;
                    case (int)Naming.DocumentTypeDefinition.修狀申請書:
                        receiptNo =  item.AmendingLcApplication.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo;
                        companyID = item.AmendingLcApplication.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.申請人;
                        break;
                    case (int)Naming.DocumentTypeDefinition.信用狀註銷申請書:
                        receiptNo = item.CreditCancellation.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo;
                        companyID = item.CreditCancellation.LetterOfCredit.CreditApplicationDocumentary.申請人;
                        break;
                    case (int)Naming.DocumentTypeDefinition.押匯申請書:
                        if (item.NegoDraft.LC_ID.HasValue)
                        {
                            receiptNo = item.NegoDraft.NegoLC.ApplicantReceiptNo;
                            companyID = item.NegoDraft.NegoLC.CompanyID;
                        }
                        else
                        {
                            receiptNo = item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo;
                            companyID = item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.申請人;
                        }
                        break;
                    case (int)Naming.DocumentTypeDefinition.還款改貸申請書:
                        receiptNo = item.Reimbursement.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo;
                        companyID = item.Reimbursement.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.申請人;
                        break;

                }

                var table = this.GetTable<CustomerInbox>();
                table.InsertOnSubmit(new CustomerInbox
                {
                    DocID = docID.Value,
                    MsgDate = DateTime.Now,
                    ReceiptNo = receiptNo,
                    TypeID = (int)typeID,
                    CompanyID = companyID
                });

                this.SubmitChanges();
            }
        }

        public void CreateInboxForBeneciary(int? docID, Naming.MessageTypeDefinition typeID)
        {
            var item = this.GetTable<Documentary>().Where(d => d.DocID == docID).FirstOrDefault();

            if (item != null)
            {
                String receiptNo = null;
                int? companyID = null; 
                switch (item.DocType)
                {
                    case (int)Naming.DocumentTypeDefinition.開狀申請書:
                        receiptNo = item.CreditApplicationDocumentary.BeneficiaryData.Organization.ReceiptNo;
                        companyID = item.CreditApplicationDocumentary.受益人;
                        break;
                    case (int)Naming.DocumentTypeDefinition.修狀申請書:
                        receiptNo = item.AmendingLcApplication.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.BeneficiaryData.Organization.ReceiptNo;
                        companyID = item.AmendingLcApplication.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.受益人;
                        break;
                    case (int)Naming.DocumentTypeDefinition.信用狀註銷申請書:
                        receiptNo = item.CreditCancellation.LetterOfCredit.CreditApplicationDocumentary.BeneficiaryData.Organization.ReceiptNo;
                        companyID = item.CreditCancellation.LetterOfCredit.CreditApplicationDocumentary.受益人;
                        break;
                    case (int)Naming.DocumentTypeDefinition.押匯申請書:
                        if (item.NegoDraft.LC_ID.HasValue)
                        {
                            receiptNo = item.NegoDraft.NegoLC.BeneficiaryReceiptNo;
                            companyID = item.NegoDraft.NegoLC.BeneficiaryID;
                        }
                        else
                        {
                            receiptNo = item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.BeneficiaryData.Organization.ReceiptNo;
                            companyID = item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.受益人;
                        }
                        break;
                    case (int)Naming.DocumentTypeDefinition.還款改貸申請書:
                        receiptNo = item.Reimbursement.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.BeneficiaryData.Organization.ReceiptNo;
                        companyID = item.Reimbursement.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.受益人;
                        break;

                }

                var table = this.GetTable<CustomerInbox>();
                table.InsertOnSubmit(new CustomerInbox
                {
                    DocID = docID.Value,
                    MsgDate = DateTime.Now,
                    ReceiptNo = receiptNo,
                    TypeID = (int)typeID,
                    CompanyID = companyID
                });

                this.SubmitChanges();
            }
        }

        public void CreateInboxForBank(String content, String bankCode, Naming.MessageTypeDefinition tid)
        {
            var table = this.GetTable<BankInbox>();
            table.InsertOnSubmit(new BankInbox
            {
                BankCode = bankCode,
                MsgDate = DateTime.Now,
                TypeID = (int)tid,
                MsgContent = content
            });
            this.SubmitChanges();
        }


        public void CreateInboxForBank(int? docID, Naming.MessageTypeDefinition typeID)
        {
            var item = this.GetTable<Documentary>().Where(d => d.DocID == docID).FirstOrDefault();
            if (item != null)
            {
             String bankCode =null;
             switch (item.DocType)
             {
                 case (int)Naming.DocumentTypeDefinition.開狀申請書:
                     bankCode = item.CreditApplicationDocumentary.開狀行;
                     break;
                 case (int)Naming.DocumentTypeDefinition.修狀申請書:
                     bankCode = item.AmendingLcApplication.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.開狀行;
                     break;
                 case (int)Naming.DocumentTypeDefinition.信用狀註銷申請書:
                     bankCode = item.CreditCancellation.LetterOfCredit.CreditApplicationDocumentary.開狀行;
                     break;
                 case (int)Naming.DocumentTypeDefinition.押匯申請書:
                     if (item.NegoDraft.LC_ID.HasValue)
                     {
                         bankCode = item.NegoDraft.NegoLC.AdvisingBank;
                     }
                     else
                     {
                         bankCode = item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.通知行;
                     }
                     break;
                 case (int)Naming.DocumentTypeDefinition.還款改貸申請書:
                     bankCode = item.Reimbursement.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.通知行;
                     break;

             }

                var table = this.GetTable<BankInbox>();

                if (item.CurrentLevel == (int)Naming.DocumentLevel.待經辦審核
                    || item.CurrentLevel == (int)Naming.DocumentLevel.待主管審核
                    || item.CurrentLevel == (int)Naming.DocumentLevel.已開立
                    || item.CurrentLevel == (int)Naming.DocumentLevel.已註銷
                    || item.CurrentLevel == (int)Naming.DocumentLevel.已退回_CRC退回
                    || item.CurrentLevel == (int)Naming.DocumentLevel.已退回_主管退回
                    || item.CurrentLevel == (int)Naming.DocumentLevel.銀行已拒絕)
                {
                    table.InsertOnSubmit(new BankInbox
                    {
                        BankCode = bankCode,
                        DocID = docID.Value,
                        MsgDate = DateTime.Now,
                        TypeID = (int)typeID
                    });
                    this.SubmitChanges();
                }
                else if (item.CurrentLevel == (int)Naming.DocumentLevel.待CRC登錄
                    || item.CurrentLevel == (int)Naming.DocumentLevel.待放行
                    || item.CurrentLevel == (int)Naming.DocumentLevel.待註記
                    || item.CurrentLevel == (int)Naming.DocumentLevel.已退回_CRC主管退回)
                {
                    var bank = this.GetTable<BankData>().Where(b => b.BankCode == bankCode).First();
                    table.InsertOnSubmit(new BankInbox
                    {
                        BankCode = bank.CRC_Branch,
                        DocID = docID.Value,
                        MsgDate = DateTime.Now,
                        TypeID = (int)typeID
                    });
                    this.SubmitChanges();
                }
            }
        }


        public void CreateInboxOfLcForCustomer(LetterOfCredit item, Naming.MessageTypeDefinition typeID)
        {
            var table = this.GetTable<CustomerInbox>();
            table.InsertOnSubmit(new CustomerInbox
            {
                DocID = item.AppID.Value,
                MsgDate = DateTime.Now,
                ReceiptNo = item.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo,
                TypeID = (int)typeID,
                CompanyID = item.CreditApplicationDocumentary.申請人
            });

            table.InsertOnSubmit(new CustomerInbox
            {
                DocID = item.AppID.Value,
                MsgDate = DateTime.Now,
                ReceiptNo = item.CreditApplicationDocumentary.BeneficiaryData.Organization.ReceiptNo,
                TypeID = (int)typeID,
                CompanyID = item.CreditApplicationDocumentary.受益人
            });

            this.SubmitChanges();
        }
    }
}
