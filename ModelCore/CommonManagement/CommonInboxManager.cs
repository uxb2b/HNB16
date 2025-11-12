using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using CommonLib.Core.DataWork;
using ModelCore.DataModel;
using ModelCore.Helper;
using ModelCore.Locale;

namespace ModelCore.CommonManagement
{
	/// <summary>
	/// InboxManagerDALC 的摘要描述。
	/// </summary>
	public class CommonInboxManager : ModelSource
    {
		public CommonInboxManager() : base()
		{
			//
			// TODO: 在此加入建構函式的程式碼
			//
		}

        public CommonInboxManager(GenericManager<LcEntityDbContext> mgr) : base(mgr) { }

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
                        return item.AmendingLcApplication.Source.Lc.Application.ApplicantDetails.ContactEmail;
                    case (int)Naming.DocumentTypeDefinition.信用狀註銷申請書:
                        return item.CreditCancellation.Lc.Application.ApplicantDetails.ContactEmail;
                    case (int)Naming.DocumentTypeDefinition.押匯申請書:
                        if (item.NegoDraft.NegoLcVersion.Lc.ApplicationID.HasValue)
                        {
                            return item.NegoDraft.NegoLcVersion.Lc.Application.ApplicantDetails.ContactEmail;
                        }
                        else
                        {
                            return item.NegoDraft.NegoLcVersion.Lc.NegoLC.ApplicantDetails.ContactEmail;
                        }

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
                            item.CreditApplicationDocumentary.IssuingBankCode, item.CreditApplicationDocumentary.AdvisingBankCode);
                    case (int)Naming.DocumentTypeDefinition.修狀申請書:
                        return String.Format("chb{0}@ms1.chb.com.tw,chb{1}@ms1.chb.com.tw",
                            item.AmendingLcApplication.Source.Lc.Application.IssuingBankCode, item.AmendingLcApplication.Source.Lc.Application.AdvisingBankCode);
                    case (int)Naming.DocumentTypeDefinition.信用狀註銷申請書:
                        return String.Format("chb{0}@ms1.chb.com.tw",
                            item.CreditCancellation.Lc.Application.IssuingBankCode);
                    case (int)Naming.DocumentTypeDefinition.押匯申請書:
                        if (!item.NegoDraft.NegoLcVersion.Lc.ApplicationID.HasValue)
                        {
                            return String.Format("chb{0}@ms1.chb.com.tw,chb{1}@ms1.chb.com.tw",
                                item.NegoDraft.NegoLcVersion.Lc.NegoLC.IssuingBank, item.NegoDraft.NegoLcVersion.Lc.NegoLC.AdvisingBank);
                        }
                        else
                        {
                            return String.Format("chb{0}@ms1.chb.com.tw,chb{1}@ms1.chb.com.tw",
                                item.NegoDraft.NegoLcVersion.Lc.Application.IssuingBankCode, item.NegoDraft.NegoLcVersion.Lc.Application.AdvisingBankCode);
                        }
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
                        return item.AmendingLcApplication.Source.Lc.Application.BeneDetails.ContactEmail;
                    case (int)Naming.DocumentTypeDefinition.信用狀註銷申請書:
                        return item.CreditCancellation.Lc.Application.BeneDetails.ContactEmail;
                    case (int)Naming.DocumentTypeDefinition.押匯申請書:
                        if (!item.NegoDraft.NegoLcVersion.Lc.ApplicationID.HasValue)
                        {
                            return item.NegoDraft.NegoLcVersion.Lc.NegoLC.BeneDetails.ContactEmail;
                        }
                        else
                        {
                            return item.NegoDraft.NegoLcVersion.Lc.Application.BeneDetails.ContactEmail;
                        }

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
                        companyID = item.CreditApplicationDocumentary.ApplicantID;
                        break;
                    case (int)Naming.DocumentTypeDefinition.修狀申請書:
                        receiptNo =  item.AmendingLcApplication.Source.Lc.Application.CustomerOfBranch.Organization.ReceiptNo;
                        companyID = item.AmendingLcApplication.Source.Lc.Application.ApplicantID;
                        break;
                    case (int)Naming.DocumentTypeDefinition.信用狀註銷申請書:
                        receiptNo = item.CreditCancellation.Lc.Application.CustomerOfBranch.Organization.ReceiptNo;
                        companyID = item.CreditCancellation.Lc.Application.ApplicantID;
                        break;
                    case (int)Naming.DocumentTypeDefinition.押匯申請書:
                        if (!item.NegoDraft.NegoLcVersion.Lc.ApplicationID.HasValue)
                        {
                            receiptNo = item.NegoDraft.NegoLcVersion.Lc.NegoLC.CustomerOfBranch.Organization.ReceiptNo;
                            companyID = item.NegoDraft.NegoLcVersion.Lc.NegoLC.CompanyID;
                        }
                        else
                        {
                            receiptNo = item.NegoDraft.NegoLcVersion.Lc.Application.CustomerOfBranch.Organization.ReceiptNo;
                            companyID = item.NegoDraft.NegoLcVersion.Lc.Application.ApplicantID;
                        }
                        break;

                }

                var table = this.GetTable<CustomerInbox>();
                table.Add(new CustomerInbox
                {
                    DocumentaryID = docID.Value,
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
                        receiptNo = item.CreditApplicationDocumentary.Beneficiary.Organization.ReceiptNo;
                        companyID = item.CreditApplicationDocumentary.BeneficiaryID;
                        break;
                    case (int)Naming.DocumentTypeDefinition.修狀申請書:
                        receiptNo = item.AmendingLcApplication.Source.Lc.Application.Beneficiary.Organization.ReceiptNo;
                        companyID = item.AmendingLcApplication.Source.Lc.Application.BeneficiaryID;
                        break;
                    case (int)Naming.DocumentTypeDefinition.信用狀註銷申請書:
                        receiptNo = item.CreditCancellation.Lc.Application.Beneficiary.Organization.ReceiptNo;
                        companyID = item.CreditCancellation.Lc.Application.BeneficiaryID;
                        break;
                    case (int)Naming.DocumentTypeDefinition.押匯申請書:
                        if (!item.NegoDraft.NegoLcVersion.Lc.ApplicationID.HasValue)
                        {
                            receiptNo = item.NegoDraft.NegoLcVersion.Lc.NegoLC.Beneficiary.Organization.ReceiptNo;
                            companyID = item.NegoDraft.NegoLcVersion.Lc.NegoLC.BeneficiaryID;
                        }
                        else
                        {
                            receiptNo = item.NegoDraft.NegoLcVersion.Lc.Application.Beneficiary.Organization.ReceiptNo;
                            companyID = item.NegoDraft.NegoLcVersion.Lc.Application.BeneficiaryID;
                        }
                        break;

                }

                var table = this.GetTable<CustomerInbox>();
                table.Add(new CustomerInbox
                {
                    DocumentaryID = docID.Value,
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
            table.Add(new BankInbox
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
                     bankCode = item.CreditApplicationDocumentary.IssuingBankCode;
                     break;
                 case (int)Naming.DocumentTypeDefinition.修狀申請書:
                     bankCode = item.AmendingLcApplication.Source.Lc.Application.IssuingBankCode;
                     break;
                 case (int)Naming.DocumentTypeDefinition.信用狀註銷申請書:
                     bankCode = item.CreditCancellation.Lc.Application.IssuingBankCode;
                     break;
                 case (int)Naming.DocumentTypeDefinition.押匯申請書:
                     if (!item.NegoDraft.NegoLcVersion.Lc.ApplicationID.HasValue)
                     {
                         bankCode = item.NegoDraft.NegoLcVersion.Lc.NegoLC.AdvisingBank;
                     }
                     else
                     {
                         bankCode = item.NegoDraft.NegoLcVersion.Lc.Application.AdvisingBankCode;
                     }
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
                    table.Add(new BankInbox
                    {
                        BankCode = bankCode,
                        DocumentaryID = docID.Value,
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
                    table.Add(new BankInbox
                    {
                        BankCode = bank.CRC_Branch,
                        DocumentaryID = docID.Value,
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
            table.Add(new CustomerInbox
            {
                DocumentaryID = item.ApplicationID.Value,
                MsgDate = DateTime.Now,
                ReceiptNo = item.Application.CustomerOfBranch.Organization.ReceiptNo,
                TypeID = (int)typeID,
                CompanyID = item.Application.ApplicantID
            });

            table.Add(new CustomerInbox
            {
                DocumentaryID = item.ApplicationID.Value,
                MsgDate = DateTime.Now,
                ReceiptNo = item.Application.Beneficiary.Organization.ReceiptNo,
                TypeID = (int)typeID,
                CompanyID = item.Application.BeneficiaryID
            });

            this.SubmitChanges();
        }
    }
}
