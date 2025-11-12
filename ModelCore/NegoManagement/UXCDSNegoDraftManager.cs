using CommonLib.Core.DataWork;
using CommonLib.Utility;
using Microsoft.EntityFrameworkCore.Storage;
using ModelCore.BankManagement;
using ModelCore.DataModel;
using ModelCore.EventMessageApp;
using ModelCore.Helper;
using ModelCore.Locale;
using ModelCore.Properties;
using ModelCore.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

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

        public UXCDSNegoDraftManager(GenericManager<LcEntityDbContext> mgr) : base(mgr) { }

        public bool AllowMarkedNegoApplication(string approver, int[] draftID, String[] branchNo)
        {
            bool bResult = false;
            if (draftID != null && draftID.Length > 0 && branchNo != null && branchNo.Length == draftID.Length)
            {
                IDbContextTransaction sqlTran = null;
                try
                {

                    sqlTran = Context.Database.BeginTransaction();

                    for (int i = 0; i < draftID.Length; i++)
                    {
                        //Context.AllowNegoApplication(draftID[i], approver, null, null, branchNo[i]);
                    }

                    sqlTran.Commit();
                    bResult = true;
                }
                catch (Exception ex)
                {
                    if (sqlTran != null)
                    {
                        sqlTran.Rollback();
                    }
                    ModelCore.Helper.Logger.Error(ex, ModelCore.Helper.Logger.LogLevel.Error);
                }
                finally
                {
                    if(sqlTran != null)
                    {
                        sqlTran.Dispose();
                    }   
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

            this.GetTable<NegoPrompt>().Add(prompt);
            this.SubmitChanges();

            LetterOfCreditVersion eLc;
            if (!CheckNegoLC(negoData.LC, out eLc))
            {
                return;
            }

            var lc = eLc.Lc;

            var draftRow = negoData.Draft;
            if (draftRow == null)
            {
                return;
            }
            else
            {
                NegoDraft draft = this.EntityList.Where(d => d.DraftNo == draftRow.DraftNo)
                                    .OrderByDescending(d => d.DocumentaryID).FirstOrDefault();
                if (draft != null)
                {
                    if (draft.NegoLcVersionID == eLc.VersionID)
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
                        Prompt = prompt,
                        NegoDate = draftRow.NegoDate ?? DateTime.Now,
                    };

                    new DocumentaryLevel
                    {
                        Documentary = draft.Documentary,
                        LevelDate = now,
                        DocLevel = (int)Naming.DocumentLevel.待經辦審核
                    };

                    draft.NegoLcVersionID = lc.LetterOfCreditVersion.OrderByDescending(v => v.VersionID).First().VersionID;
                    if (lc.Application != null)
                    {
                        draft.NegoDraftExtension.NegoBranch = lc.Application?.AdvisingBankCode;
                        draft.NegoDraftExtension.LcBranch = lc.Application.IssuingBankCode;
                        if (lc.Application.Beneficiary.DraftType == (int)Naming.DraftType.WASIN)
                        {
                            draft.NegoDraftExtension.DraftType = (int)Naming.DraftType.WASIN;
                        }
                    }
                    else
                    {
                        draft.NegoDraftExtension.NegoBranch = lc.NegoLC.AdvisingBank;
                        draft.NegoDraftExtension.LcBranch = lc.NegoLC.IssuingBank;
                        var draftType = this.GetTable<BeneficiaryData>().Where(b => b.OrganizationID == lc.NegoLC.BeneficiaryID)
                            .Select(b => b.DraftType).FirstOrDefault();
                        if (draftType == (int)Naming.DraftType.WASIN)
                        {
                            draft.NegoDraftExtension.DraftType = (int)Naming.DraftType.WASIN;
                        }
                    }

                    UpdateNegoDraft(draft, draftRow, negoData.Invoice);
                    draft.DraftContent = draftRow.GetXml();

                    this.EntityList.Add(draft);
                    this.SubmitChanges();
                    draft.UpdateAppSeq(this);

                    //MessageNotification.CreateInboxMessage(draft.DocumentaryID, Naming.MessageTypeDefinition.MSG_NEGO_APP_READY, Naming.MessageReceipent.ForBank);
                    //MessageNotification.CreateMailMessage(this, draft.DocumentaryID, Naming.MessageTypeDefinition.MSG_NEGO_APP_READY, Naming.MessageReceipent.ForBank);

                }

                var organization = lc?.Application?.Beneficiary.Organization ?? lc.NegoLC?.Beneficiary?.Organization;
                CommitNegoPrompt(organization?.OrganizationStatus?.Group?.Service?.ConfirmUrl, draftRow.KeyID);

            }


        }

        public bool UpdateNegoDraft(NegoDraft draft, ModelCore.Schema.UXCDS.NegoDraft draftRow, ModelCore.Schema.UXCDS.BusinessInvoice[] invoiceDetails)
        {

            int year = ((DateTime)draftRow.ShipmentDate).Year - 1911;

            draft.DraftNo = draftRow.DraftNo.ToString();
            draft.NegoDraftExtension.DueDate = draft.NegoDate = DateTime.Now;
            draft.ShipmentDate = draftRow.ShipmentDate;
            draft.Amount = draftRow.Amount;
            draft.NegoDraftExtension.BeneficiaryAccountNo = draftRow.DepositAccount;
            draft.ItemName = draftRow.ItemName.ToString();
            draft.ItemQuantity = draftRow.ItemQuantity;
            draft.ItemSubtotal = draftRow.ItemSubtotal;
            if (draftRow.FrontSeal?.Content != null)
            {
                if (draft.FrontSeal != draftRow.FrontSeal.SealPath)
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

        public bool CheckNegoLC(ModelCore.Schema.UXCDS.NegoLC lcData, out LetterOfCreditVersion eLc)
        {
            eLc = null;
            NegoLC nlc = null;

            var applicantItem = lcData?.Applicant;

            if (applicantItem == null || lcData?.BeneficiaryData?.ReceiptNo == null)
                return false;

            lcData.LCNo = lcData.LCNo.GetEfficientString();
            eLc = this.GetTable<LetterOfCreditVersion>().Where(l => l.Lc.LcNo == lcData.LCNo)
                .OrderByDescending(l => l.VersionID)
                .FirstOrDefault();
            if (eLc != null)
            {
                return true;
            }

            String issuingBank = lcData.IssuingBank.Right(4);
            String advisingBank = lcData.NotifyingBank?.Right(4);
            String payableBank = lcData.AdvisingBank?.Right(4);

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
                this.GetTable<CustomerOfBranch>().Add(cust);
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

            // Create AttachableDocument
            AttachableDocument attachableDoc = new AttachableDocument
            {
                匯票付款申請書 = null,
                匯票承兌申請書 = null,
                統一發票 = null,
                電子發票證明聯 = null,
                其他 = null
            };
            this.GetTable<AttachableDocument>().Add(attachableDoc);
            this.SubmitChanges();

            // Create SpecificNotes
            SpecificNotes specificNotes = new SpecificNotes
            {
                原留印鑑相符 = null,
                受益人單獨蓋章 = null,
                分批交貨 = null,
                最後交貨日 = null,
                接受發票早於開狀日 = null,
                接受發票金額大於開狀金額 = null,
                其他 = null,
                押匯起始日 = null,
                押匯發票起始日 = null,
                接受發票人地址與受益人地址不符 = null,
                接受發票電子訊息 = null,
                貨品明細以發票為準 = null,
                接受發票金額大於匯票金額 = null,
                以發票收執聯或扣抵聯正本押匯 = null,
                發票影本可接受 = null,
                NonCSCTerms = null,
                IsUsanceLcInterestPayByBuyer = null,
                IsAcceptanceChargePayByBuyer = null,
                SpecialMessageForCS = null,
                IsCSCTerms = null,
                CSCSalesDept = null
            };
            this.GetTable<SpecificNotes>().Add(specificNotes);
            this.SubmitChanges();

            // Get CurrencyID (assuming default or mapping needed)
            var currencyType = this.GetTable<CurrencyType>().FirstOrDefault();
            int currencyId = currencyType?.CurrencyID ?? 1; // Default to 1 if not found

            // Create LcItems
            LcItems lcItems = new LcItems
            {
                開狀金額 = lcData.Amount,
                有效期限 = lcData.DateOfExpiry,
                CurrencyTypeID = currencyId,
                Goods = null, // Can be populated from lcData if available
                定日付款 = lcData.DueDays ?? 0,
                PaymentDate = null,
                PaymentTerms = null
            };
            this.GetTable<LcItems>().Add(lcItems);
            this.SubmitChanges();

            // Create NegoLcVersion
            var lc = new LetterOfCredit
            {
                LcNo = lcData.LCNo,
                可用餘額 = lcData.AvailableAmount ?? lcData.Amount,
                LcDate = lcData.DateOfIssue,
                AppCountersign = false,
                PrintNotice = null,
                受益人匯票簽核認可 = null,
                NotifyingBank = advisingBank
            };
            this.GetTable<LetterOfCredit>().Add(lc);
            this.SubmitChanges();

            // Create Source
            eLc = new LetterOfCreditVersion
            {
                LcID = lc.LcID,
                VersionNo = 0, // Initial version
                LcItemsID = lcItems.ItemID,
                SpecificNotesID = specificNotes.NoteID,
                AttachableDocumentID = attachableDoc.AttachmentID,
                AmendingLcInformationID = null,
                NotifyingBank = advisingBank
            };
            this.GetTable<LetterOfCreditVersion>().Add(eLc);
            this.SubmitChanges();

            // Also create NegoLC for backward compatibility
            nlc = new NegoLC
            {
                LcID = eLc.LcID,
                CompanyID = applicant.CompanyID,
                BeneficiaryID = beneData.OrganizationID,
                ImportDate = DateTime.Now,
                IssuingBank = issuingBank,
                AdvisingBank = advisingBank,
                PayableBank = payableBank,
                LCType = lcData.LCType,
                Status = 0,
                DownloadFlag = 1,
                ApplicantDetailsID = cust.CustomerOfBranchVersionID,
                BeneDetailsID = beneData.CustomerOfBranchVersionID
            };

            this.GetTable<NegoLC>().Add(nlc);
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

            this.GetTable<Organization>().Add(item);
            this.SubmitChanges();
            return item;
        }

    }
}
