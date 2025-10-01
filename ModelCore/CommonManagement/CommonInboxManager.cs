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
	/// InboxManagerDALC ���K�n�y�z�C
	/// </summary>
	public class CommonInboxManager : LcEntityManager<MessageType>
	{
		public CommonInboxManager() : base()
		{
			//
			// TODO: �b���[�J�غc�禡���{���X
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
                    case (int)Naming.DocumentTypeDefinition.�}���ӽЮ�:
                        return item.CreditApplicationDocumentary.ApplicantDetails.ContactEmail;
                    case (int)Naming.DocumentTypeDefinition.�ת��ӽЮ�:
                        return item.AmendingLcApplication.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.ApplicantDetails.ContactEmail;
                    case (int)Naming.DocumentTypeDefinition.�H�Ϊ����P�ӽЮ�:
                        return item.CreditCancellation.LetterOfCredit.CreditApplicationDocumentary.ApplicantDetails.ContactEmail;
                    case (int)Naming.DocumentTypeDefinition.��ץӽЮ�:
                        if (item.NegoDraft.LC_ID.HasValue)
                        {
                            return item.NegoDraft.NegoLC.ApplicantDetails.ContactEmail;
                        }
                        else
                        {
                            return item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.ApplicantDetails.ContactEmail;
                        }
                    case (int)Naming.DocumentTypeDefinition.�ٴڧ�U�ӽЮ�:
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
                    case (int)Naming.DocumentTypeDefinition.�}���ӽЮ�:
                        return String.Format("chb{0}@ms1.chb.com.tw,chb{1}@ms1.chb.com.tw",
                            item.CreditApplicationDocumentary.�}����, item.CreditApplicationDocumentary.�q����);
                    case (int)Naming.DocumentTypeDefinition.�ת��ӽЮ�:
                        return String.Format("chb{0}@ms1.chb.com.tw,chb{1}@ms1.chb.com.tw",
                            item.AmendingLcApplication.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.�}����, item.AmendingLcApplication.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.�q����);
                    case (int)Naming.DocumentTypeDefinition.�H�Ϊ����P�ӽЮ�:
                        return String.Format("chb{0}@ms1.chb.com.tw",
                            item.CreditCancellation.LetterOfCredit.CreditApplicationDocumentary.�}����);
                    case (int)Naming.DocumentTypeDefinition.��ץӽЮ�:
                        if (item.NegoDraft.LC_ID.HasValue)
                        {
                            return String.Format("chb{0}@ms1.chb.com.tw,chb{1}@ms1.chb.com.tw",
                                item.NegoDraft.NegoLC.IssuingBank, item.NegoDraft.NegoLC.AdvisingBank);
                        }
                        else
                        {
                            return String.Format("chb{0}@ms1.chb.com.tw,chb{1}@ms1.chb.com.tw",
                                item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.�}����, item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.�q����);
                        }
                    case (int)Naming.DocumentTypeDefinition.�ٴڧ�U�ӽЮ�:
                        return String.Format("chb{0}@ms1.chb.com.tw,chb{1}@ms1.chb.com.tw",
                            item.Reimbursement.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.�}����, item.Reimbursement.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.�q����);
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
                    case (int)Naming.DocumentTypeDefinition.�}���ӽЮ�:
                        return item.CreditApplicationDocumentary.BeneDetails.ContactEmail;
                    case (int)Naming.DocumentTypeDefinition.�ת��ӽЮ�:
                        return item.AmendingLcApplication.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.BeneDetails.ContactEmail;
                    case (int)Naming.DocumentTypeDefinition.�H�Ϊ����P�ӽЮ�:
                        return item.CreditCancellation.LetterOfCredit.CreditApplicationDocumentary.BeneDetails.ContactEmail;
                    case (int)Naming.DocumentTypeDefinition.��ץӽЮ�:
                        if (item.NegoDraft.LC_ID.HasValue)
                        {
                            return item.NegoDraft.NegoLC.BeneDetails.ContactEmail;
                        }
                        else
                        {
                            return item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.BeneDetails.ContactEmail;
                        }
                    case (int)Naming.DocumentTypeDefinition.�ٴڧ�U�ӽЮ�:
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
                    case (int)Naming.DocumentTypeDefinition.�}���ӽЮ�:
                        receiptNo = item.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo;
                        companyID = item.CreditApplicationDocumentary.�ӽФH;
                        break;
                    case (int)Naming.DocumentTypeDefinition.�ת��ӽЮ�:
                        receiptNo =  item.AmendingLcApplication.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo;
                        companyID = item.AmendingLcApplication.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.�ӽФH;
                        break;
                    case (int)Naming.DocumentTypeDefinition.�H�Ϊ����P�ӽЮ�:
                        receiptNo = item.CreditCancellation.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo;
                        companyID = item.CreditCancellation.LetterOfCredit.CreditApplicationDocumentary.�ӽФH;
                        break;
                    case (int)Naming.DocumentTypeDefinition.��ץӽЮ�:
                        if (item.NegoDraft.LC_ID.HasValue)
                        {
                            receiptNo = item.NegoDraft.NegoLC.ApplicantReceiptNo;
                            companyID = item.NegoDraft.NegoLC.CompanyID;
                        }
                        else
                        {
                            receiptNo = item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo;
                            companyID = item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.�ӽФH;
                        }
                        break;
                    case (int)Naming.DocumentTypeDefinition.�ٴڧ�U�ӽЮ�:
                        receiptNo = item.Reimbursement.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.CustomerOfBranch.Organization.ReceiptNo;
                        companyID = item.Reimbursement.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.�ӽФH;
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
                    case (int)Naming.DocumentTypeDefinition.�}���ӽЮ�:
                        receiptNo = item.CreditApplicationDocumentary.BeneficiaryData.Organization.ReceiptNo;
                        companyID = item.CreditApplicationDocumentary.���q�H;
                        break;
                    case (int)Naming.DocumentTypeDefinition.�ת��ӽЮ�:
                        receiptNo = item.AmendingLcApplication.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.BeneficiaryData.Organization.ReceiptNo;
                        companyID = item.AmendingLcApplication.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.���q�H;
                        break;
                    case (int)Naming.DocumentTypeDefinition.�H�Ϊ����P�ӽЮ�:
                        receiptNo = item.CreditCancellation.LetterOfCredit.CreditApplicationDocumentary.BeneficiaryData.Organization.ReceiptNo;
                        companyID = item.CreditCancellation.LetterOfCredit.CreditApplicationDocumentary.���q�H;
                        break;
                    case (int)Naming.DocumentTypeDefinition.��ץӽЮ�:
                        if (item.NegoDraft.LC_ID.HasValue)
                        {
                            receiptNo = item.NegoDraft.NegoLC.BeneficiaryReceiptNo;
                            companyID = item.NegoDraft.NegoLC.BeneficiaryID;
                        }
                        else
                        {
                            receiptNo = item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.BeneficiaryData.Organization.ReceiptNo;
                            companyID = item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.���q�H;
                        }
                        break;
                    case (int)Naming.DocumentTypeDefinition.�ٴڧ�U�ӽЮ�:
                        receiptNo = item.Reimbursement.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.BeneficiaryData.Organization.ReceiptNo;
                        companyID = item.Reimbursement.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.���q�H;
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
                 case (int)Naming.DocumentTypeDefinition.�}���ӽЮ�:
                     bankCode = item.CreditApplicationDocumentary.�}����;
                     break;
                 case (int)Naming.DocumentTypeDefinition.�ת��ӽЮ�:
                     bankCode = item.AmendingLcApplication.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.�}����;
                     break;
                 case (int)Naming.DocumentTypeDefinition.�H�Ϊ����P�ӽЮ�:
                     bankCode = item.CreditCancellation.LetterOfCredit.CreditApplicationDocumentary.�}����;
                     break;
                 case (int)Naming.DocumentTypeDefinition.��ץӽЮ�:
                     if (item.NegoDraft.LC_ID.HasValue)
                     {
                         bankCode = item.NegoDraft.NegoLC.AdvisingBank;
                     }
                     else
                     {
                         bankCode = item.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.�q����;
                     }
                     break;
                 case (int)Naming.DocumentTypeDefinition.�ٴڧ�U�ӽЮ�:
                     bankCode = item.Reimbursement.NegoDraft.LetterOfCreditVersion.LetterOfCredit.CreditApplicationDocumentary.�q����;
                     break;

             }

                var table = this.GetTable<BankInbox>();

                if (item.CurrentLevel == (int)Naming.DocumentLevel.�ݸg��f��
                    || item.CurrentLevel == (int)Naming.DocumentLevel.�ݥD�޼f��
                    || item.CurrentLevel == (int)Naming.DocumentLevel.�w�}��
                    || item.CurrentLevel == (int)Naming.DocumentLevel.�w���P
                    || item.CurrentLevel == (int)Naming.DocumentLevel.�w�h�^_CRC�h�^
                    || item.CurrentLevel == (int)Naming.DocumentLevel.�w�h�^_�D�ްh�^
                    || item.CurrentLevel == (int)Naming.DocumentLevel.�Ȧ�w�ڵ�)
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
                else if (item.CurrentLevel == (int)Naming.DocumentLevel.��CRC�n��
                    || item.CurrentLevel == (int)Naming.DocumentLevel.�ݩ��
                    || item.CurrentLevel == (int)Naming.DocumentLevel.�ݵ��O
                    || item.CurrentLevel == (int)Naming.DocumentLevel.�w�h�^_CRC�D�ްh�^)
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
                CompanyID = item.CreditApplicationDocumentary.�ӽФH
            });

            table.InsertOnSubmit(new CustomerInbox
            {
                DocID = item.AppID.Value,
                MsgDate = DateTime.Now,
                ReceiptNo = item.CreditApplicationDocumentary.BeneficiaryData.Organization.ReceiptNo,
                TypeID = (int)typeID,
                CompanyID = item.CreditApplicationDocumentary.���q�H
            });

            this.SubmitChanges();
        }
    }
}
