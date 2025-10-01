using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;



using CommonLib.DataAccess;
using ModelCore.DataModel;
using ModelCore.EventMessageApp;
using ModelCore.Locale;
using ModelCore.Properties;
using CommonLib.Utility;
using ModelCore.BankManagement;
using ModelCore.Helper;

using System.Threading;
using System.Text;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Net;

namespace ModelCore.NegoManagement
{
	/// <summary>
	/// NegoDraftDALC 的摘要描述。
	/// </summary>
    public class UXCDSNegoDraftManager : LcEntityManager<NegoDraft>
	{
		public UXCDSNegoDraftManager() : base()
		{
			//
			// TODO: 在此加入建構函式的程式碼
			//
		}

        public UXCDSNegoDraftManager(GenericManager<LcEntityDataContext> mgr) : base(mgr) { }

		public bool AllowMarkedNegoApplication(string approver,int[] draftID,String[] branchNo)
		{
			bool bResult = false;
            if (draftID != null && draftID.Length > 0 && branchNo != null && branchNo.Length==draftID.Length )
			{
                DbTransaction sqlTran = null;

				try
				{
					Context.Connection.Open();

                    sqlTran = Context.Connection.BeginTransaction();
                    Context.Transaction = sqlTran;

					for(int i=0;i<draftID.Length;i++)
					{
                        Context.AllowNegoApplication(draftID[i], approver, null, null, branchNo[i]);
					}

					sqlTran.Commit();
					bResult = true;
				}
				catch(Exception ex)
				{
					if(sqlTran!=null)
					{
						sqlTran.Rollback();
					}
					ModelCore.Helper.Logger.Error(ex, ModelCore.Helper.Logger.LogLevel.Error);
				}
				finally
				{
                    Context.Connection.Close();
				}
			}

			return bResult;
		}


        public void SavePromptInfo(ModelCore.Schema.UXCDS.NegoData negoData)
        {
            DateTime now = DateTime.Now;

            NegoPrompt prompt = new NegoPrompt
            {
                PromptContent = negoData.JsonStringify(),
                ImportDate = now
            };

            this.GetTable<NegoPrompt>().InsertOnSubmit(prompt);
            this.SubmitChanges();

            LetterOfCredit lc;
            NegoLC nlc;
            if (!CheckNegoLC(negoData.LC, out lc, out nlc))
            {
                return;
            }

            var draftRow = negoData.Draft;
            if (draftRow == null)
            {
                return;
            }
            else
            {
                NegoDraft draft = this.EntityList.Where(d => d.DraftNo == draftRow.DraftNo)
                                            .OrderByDescending(d => d.DraftID).FirstOrDefault();
                if (draft != null)
                {
                    if (draft.LcID == lc?.LcID
                        || draft.LC_ID == nlc?.LC_ID)
                    {
                        if (draft.Documentary.CurrentLevel == (int)Naming.DocumentLevel.銀行已拒絕)
                        {
                            draft.DraftNo = draft.DraftNo + "(銀行已拒絕)";
                            draft = null;
                        }
                    }
                }


                if (draft == null)
                {
                    draft = new NegoDraft
                    {
                        Documentary = new Documentary
                        {
                            DocType = (int)Naming.DocumentTypeDefinition.押匯申請書,
                            CurrentLevel = (int)Naming.DocumentLevel.待經辦審核,
                            DocDate = now
                        },
                        NegoDraftExtension = new NegoDraftExtension
                        {
                            DraftType = (int)Naming.DraftType.CDS_CSC
                        },
                        NegoPrompt = prompt
                    };

                    new DocumentaryLevel
                    {
                        Documentary = draft.Documentary,
                        LevelDate = now,
                        DocLevel = (int)Naming.DocumentLevel.待經辦審核
                    };

                    if (lc != null)
                    {
                        draft.LcID = lc.LcID;
                        draft.NegoDraftExtension.NegoBranch = lc.CreditApplicationDocumentary.通知行;
                        draft.NegoDraftExtension.LcBranch = lc.CreditApplicationDocumentary.開狀行;
                        if (lc.CreditApplicationDocumentary.BeneficiaryData.DraftType == (int)Naming.DraftType.WASIN)
                        {
                            draft.NegoDraftExtension.DraftType = (int)Naming.DraftType.WASIN;
                        }
                        //draft.NegoDraftExtension.入戶帳號 = lc.CreditApplicationDocumentary.BeneficiaryData.Organization.BeneficiaryTransferInto
                        //                                        .Where(b => b.Status == (int)Naming.BeneficiaryStatus.已核准)
                        //                                        .FirstOrDefault()?.AccountNo;
                    }
                    else
                    {
                        draft.LC_ID = nlc.LC_ID;
                        draft.NegoDraftExtension.NegoBranch = nlc.AdvisingBank;
                        draft.NegoDraftExtension.LcBranch = nlc.IssuingBank;
                        var draftType = this.GetTable<BeneficiaryData>().Where(b => b.BeneID == nlc.BeneficiaryID)
                            .Select(b => b.DraftType).FirstOrDefault();
                        if (draftType == (int)Naming.DraftType.WASIN)
                        {
                            draft.NegoDraftExtension.DraftType = (int)Naming.DraftType.WASIN;
                        }
                    }

                    UpdateNegoDraft(draft, draftRow, negoData.Invoice);
                    draft.DraftContent = draftRow.GetXml();

                    this.EntityList.InsertOnSubmit(draft);
                    this.SubmitChanges();
                    draft.UpdateAppSeq(this);

                    MessageNotification.CreateInboxMessage(draft.DraftID, Naming.MessageTypeDefinition.MSG_NEGO_APP_READY, Naming.MessageReceipent.ForBank);
                    MessageNotification.CreateMailMessage(this, draft.DraftID, Naming.MessageTypeDefinition.MSG_NEGO_APP_READY, Naming.MessageReceipent.ForBank);

                }

                var organization = lc?.CreditApplicationDocumentary.BeneficiaryData.Organization ?? nlc.BeneficiaryData.Organization;
                CommitNegoPrompt(organization?.OrganizationStatus?.BeneficiaryGroup?.BeneficiaryServiceGroup?.ConfirmUrl, draftRow.KeyID);

            }


        }

        private void CommitNegoPrompt(String confirmUrl, string draftKey)
        {
            if (confirmUrl != null)
            {
                Task.Run(() => 
                {
                    using (WebClientEx client = new WebClientEx()
                    {
                        Timeout = Timeout.Infinite,
                    })
                    {
                        if (!String.IsNullOrEmpty(Settings.Default.ProxyUrlToUXCDS))
                        {
                            client.Proxy = new WebProxy(Settings.Default.ProxyUrlToUXCDS, true);
                        }
                        client.Encoding = Encoding.UTF8;
                        NameValueCollection values = new NameValueCollection
                        {
                            ["KeyID"] = ""
                        };

                        values["KeyID"] = draftKey;
                        client.UploadValues(confirmUrl, values);
                    }
                });
            }
        }

        public bool UpdateNegoDraft(NegoDraft draft, ModelCore.Schema.UXCDS.NegoDraft draftRow, ModelCore.Schema.UXCDS.BusinessInvoice[] invoiceDetails)
        {

            int year = ((DateTime)draftRow.ShipmentDate).Year - 1911;

            draft.DraftNo = draftRow.DraftNo.ToString();
            draft.NegoDraftExtension.NegotiateDate = draft.NegoDraftExtension.DueDate 
                = draft.ImportDate = DateTime.Now;
            draft.ShipmentDate = draftRow.ShipmentDate;
            draft.Amount = draftRow.Amount;
            draft.NegoDraftExtension.入戶帳號 = draftRow.DepositAccount;
            draft.ItemName = draftRow.ItemName.ToString();
                draft.ItemQuantity = draftRow.ItemQuantity;
                draft.ItemSubtotal = draftRow.ItemSubtotal;
            if (draftRow.FrontSeal?.Content != null)
            {
                if(draft.FrontSeal!=draftRow.FrontSeal.SealPath)
                {
                    draft.FrontSeal = draftRow.FrontSeal.SealPath;
                    String sealPath = Path.Combine(AppSettings.Default.TempPath, draftRow.FrontSeal.SealPath);
                    File.WriteAllBytes(sealPath, Convert.FromBase64String(draftRow.FrontSeal.Content));
                }
            }
            if (draftRow.BackSeal?.Content != null)
            {
                if (draft.BackSeal != draftRow.BackSeal.SealPath)
                {
                    draft.BackSeal = draftRow.BackSeal.SealPath;
                    String sealPath = Path.Combine(AppSettings.Default.TempPath, draftRow.BackSeal.SealPath);
                    File.WriteAllBytes(sealPath, Convert.FromBase64String(draftRow.BackSeal.Content));
                }
            }

            draft.AppYear = year;
            draft.AppSeq = 0;
            draft.DownloadFlag = 1;

            UpdateNegoInvoice(draft, draftRow.NegoInvoice, invoiceDetails);

            return true;
        }

        private void UpdateNegoInvoice(NegoDraft draft, List<ModelCore.Schema.UXCDS.NegoInvoice> draftInvoice, ModelCore.Schema.UXCDS.BusinessInvoice[] invoiceDetails)
        {
            if (draftInvoice != null && draftInvoice.Count > 0)
            {
                draft.InvoiceCount = draftInvoice.Count;
                foreach (var invoiceRow in draftInvoice)
                {
                    NegoInvoice invoice = new NegoInvoice
                    {
                        InvoiceNo = invoiceRow.InvoiceNo,
                        LadingNo = invoiceRow.LadingNo,
                        InvoiceAmount = invoiceRow.InvoiceAmount,
                        InvoiceDate = invoiceRow.InvoiceDate,
                        ImportDate = DateTime.Now,
                        ContractNo = invoiceRow.ContractNo,
                        TaxNo = invoiceRow.TaxNo,
                        DownloadFlag = 1
                    };

                    invoice.InvoiceContent = invoiceDetails?.Where(v => v.InvoiceNo == invoiceRow.InvoiceNo)
                                                .FirstOrDefault()?.DataContent;
                    if (!draft.InvoiceDate.HasValue || draft.InvoiceDate > invoice.InvoiceDate)
                    {
                        draft.InvoiceDate = invoice.InvoiceDate;
                    }
                    draft.NegoInvoice.Add(invoice);
                }
            }
        }

        public bool CheckNegoLC(ModelCore.Schema.UXCDS.NegoLC lcData, out LetterOfCredit eLc, out NegoLC lc)
        {
            eLc = null;
            lc = null;

            var applicantItem = lcData?.Applicant;

            if (applicantItem == null || lcData?.BeneficiaryData?.ReceiptNo == null)
                return false;

            lcData.LCNo = lcData.LCNo.GetEfficientString();
            eLc = this.GetTable<LetterOfCredit>().Where(l => l.LcNo == lcData.LCNo).FirstOrDefault();
            if (eLc != null)
            {
                lc = null;
                return true;
            }
            else if ((lc = this.GetTable<NegoLC>().Where(l => l.LCNo == lcData.LCNo).FirstOrDefault()) != null)
            {
                return true;
            }

            String issuingBank = lcData.IssuingBank.Right(4);

            var applicant = this.GetTable<Organization>().Where(o => o.ReceiptNo == applicantItem.ReceiptNo).FirstOrDefault();
            if (applicant == null)
            {
                applicant = CreateApplicant(applicantItem);
            }

            var cust = applicant.CustomerOfBranch.Where(c => c.BankCode == issuingBank).FirstOrDefault();
            if (cust == null)
            {
                cust = new CustomerOfBranch
                {
                    Organization = applicant,
                    Addr = applicant.Addr,
                    BankCode = issuingBank,
                    CompanyName = applicant.CompanyName,
                    ContactEmail = applicant.ContactEmail,
                    Phone = applicant.Phone,
                    Undertaker = applicant.UndertakerName,
                    CustomerOfBranchVersion = new CustomerOfBranchVersion
                    {
                        Addr = applicant.Addr,
                        CompanyName = applicant.CompanyName,
                        ContactEmail = applicant.ContactEmail,
                        Phone = applicant.Phone,
                        Undertaker = applicant.UndertakerName
                    }
                };
                this.GetTable<CustomerOfBranch>().InsertOnSubmit(cust);
                this.SubmitChanges();
            }

            var bene = this.GetTable<Organization>().Where(o => o.ReceiptNo == lcData.BeneficiaryData.ReceiptNo).FirstOrDefault();
            if (bene == null)
            {
                bene = CreateApplicant(lcData.BeneficiaryData);
            }

            var beneData = bene.BeneficiaryData;
            if (beneData == null)
            {
                beneData = new BeneficiaryData
                {
                    Organization = bene,
                    CustomerOfBranchVersion = new CustomerOfBranchVersion
                    {
                        Addr = bene.Addr,
                        CompanyName = bene.CompanyName,
                        ContactEmail = bene.ContactEmail,
                        Phone = bene.Phone,
                        Undertaker = bene.UndertakerName
                    }
                };
                this.SubmitChanges();
            }

            lc = new NegoLC
            {
                LCNo = (String)lcData.LCNo,
                CompanyID = applicant.CompanyID,
                BeneficiaryID = bene.CompanyID,
                DateOfIssue = (DateTime)lcData.DateOfIssue,
                DateOfExpiry = (DateTime)lcData.DateOfExpiry,
                Amount = lcData.Amount,
                ApplicantReceiptNo = (String)lcData.ApplicantReceiptNo,
                AdvisingBank = ((String)lcData.NotifyingBank).Right(4),
                PayableBank = ((String)lcData.AdvisingBank).Right(4),
                ShipmentNoAfter = lcData.ShipmentNoAfter,
                LCType = lcData.LCType,
                AvailableAmount = lcData.AvailableAmount ?? lcData.Amount,
                DueDays = lcData.DueDays,
                BeneficiaryReceiptNo = bene.ReceiptNo,
                VersionID = cust.CurrentVersion,
                BeneDetailID = beneData.CurrentVersion,
                ImportDate = DateTime.Now,
                IssuingBank = issuingBank,
                Status = 0,
                DownloadFlag = 1
            };

            this.GetTable<NegoLC>().InsertOnSubmit(lc);
            this.SubmitChanges();

            return true;
        }

        private Organization CreateApplicant(ModelCore.Schema.UXCDS.Organization data)
        {
            Organization item = new Organization
            {
                ContactName = data.ContactName,
                Fax = data.Fax,
                LogoURL = data.LogoURL,
                CompanyName = data.CompanyName,
                ReceiptNo = data.ReceiptNo,
                Phone = data.Phone,
                ContactFax = data.ContactFax,
                ContactPhone = data.ContactPhone,
                ContactMobilePhone = data.ContactMobilePhone,
                RegAddr = data.RegAddr,
                UndertakerName = data.UndertakerName,
                Addr = data.Addr,
                EnglishName = data.EnglishName,
                EnglishAddr = data.EnglishAddr,
                EnglishRegAddr = data.EnglishRegAddr,
                ContactEmail = data.ContactEmail,
                UndertakerPhone = data.UndertakerPhone,
                UndertakerFax = data.UndertakerFax,
                UndertakerMobilePhone = data.UndertakerMobilePhone,
                InvoiceSignature = data.InvoiceSignature,
                OrganizationStatus = new OrganizationStatus { },
            };

            this.GetTable<Organization>().InsertOnSubmit(item);
            this.SubmitChanges();
            return item;
        }


    }
}
