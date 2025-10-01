using System;
using System.Data;
using System.Collections;
using System.IO;
using System.Xml;
using System.Net;
using System.Web;
using System.Linq;
using System.Collections.Generic;



using CommonLib.Utility;
//
using ModelCore.Properties;
using ModelCore.DataModel;
using ModelCore.Schema;
using System.Threading;
using ModelCore.Locale;
using ModelCore.Helper;
using System.Text;
using CommonLib.Security.UseCrypto;

namespace ModelCore.Service
{
	/// <summary>
	/// OutboundSvc 的摘要描述。
	/// </summary>
	public static class OutboundSvc
	{
        public static void SendLcToCDS(this CreditApplicationDocumentary item)
        {
            string uri = new System.Text.StringBuilder(Settings.Default.urlToCDS)
                .Append("?type=LC&fileName=").Append(HttpUtility.UrlEncode("LC-" + item.LetterOfCredit[0].LcNo + ".xml"))
                .Append("&no=").Append(HttpUtility.UrlEncode(item.開狀申請書號碼)).ToString();

            LLC lcObj = new LLC
            {
                LC = new LLCLC
                {
                    AdvisingBank = item.通知行,
                    AdvisingBankName = item.AdvisingBank.BranchName,
                    AppDate = item.開狀申請日期.ToString("yyyy/MM/dd"),
                    Applicant = item.ApplicantDetails.CompanyName,
                    ApplicantAddr = item.ApplicantDetails.Addr,
                    AppNo = item.開狀申請書號碼,
                    AtSight = item.見票即付.ToString(),
                    AttachAcceptance = item.AttachableDocument.匯票承兌申請書.ToString(),
                    AttachInv = item.AttachableDocument.統一發票.ToString(),
                    AttachPaying = item.AttachableDocument.匯票付款申請書.ToString(),
                    AtUsance = (!item.見票即付).ToString(),
                    Beneficiary = item.BeneDetails.CompanyName,
                    BeneSeal = item.SpecificNote.受益人單獨蓋章.ToString(),
                    EarlyInvDate = item.SpecificNote.接受發票早於開狀日.ToString(),
                    Goods = item.LcItem.Goods,
                    IssuingDate = item.LetterOfCredit[0].LcDate.ToString("yyyy/MM/dd"),
                    LargerInvAmt = item.SpecificNote.接受發票金額大於開狀金額.ToString(),
                    LcAmt = item.LcItem.開狀金額.ToString(),
                    LcExpiry = String.Format("{0:yyyy/MM/dd}",item.LcItem.有效期限),
                    LcNo = item.LetterOfCredit[0].LcNo,
                    NoAfterThan = String.Format("{0:yyyy/MM/dd}",item.SpecificNote.最後交貨日),
                    NonPatial = (!item.SpecificNote.分批交貨).ToString(),
                    Partial = item.SpecificNote.分批交貨.ToString(),
                    PayableBankAddr = item.PayableBank.Address,
                    PayableBankName = item.PayableBank.BranchName,
                    ReceiptNo = item.BeneficiaryData.CustomerOfBranch.Organization.ReceiptNo,
                    Seal = item.SpecificNote.原留印鑑相符.ToString(),
                    Usance = item.LcItem.定日付款.ToString()
                }
            };

            asyncSendLLC(uri, lcObj);
        }

        public static LLC SendLc(this CreditApplicationDocumentary item,String pdfBase64)
        {

            string uri = item.BeneficiaryData.Organization.OrganizationStatus?.BeneficiaryGroup?.BeneficiaryServiceGroup?.PostUrl;
            if (uri == null)
            {
                return null;
            }

            LLC lcObj = new LLC
            {
                LC = new LLCLC
                {
                    AdvisingBank = item.通知行,
                    AdvisingBankName = item.AdvisingBank.BranchName,
                    AppDate = item.開狀申請日期.ToString("yyyy/MM/dd"),
                    Applicant = item.ApplicantDetails.CompanyName,
                    ApplicantAddr = item.ApplicantDetails.Addr,
                    AppNo = item.開狀申請書號碼,
                    AtSight = item.見票即付.ToString(),
                    AttachAcceptance = item.AttachableDocument.匯票承兌申請書.ToString(),
                    AttachInv = item.AttachableDocument.統一發票.ToString(),
                    AttachPaying = item.AttachableDocument.匯票付款申請書.ToString(),
                    AtUsance = (!item.見票即付).ToString(),
                    Beneficiary = item.BeneDetails.CompanyName,
                    BeneSeal = item.SpecificNote.受益人單獨蓋章.ToString(),
                    EarlyInvDate = item.SpecificNote.接受發票早於開狀日.ToString(),
                    Goods = item.LcItem.BuildGoodsDetails(),
                    IssuingDate = item.LetterOfCredit[0].LcDate.ToString("yyyy/MM/dd"),
                    LargerInvAmt = item.SpecificNote.接受發票金額大於開狀金額.ToString(),
                    LcAmt = item.LcItem.開狀金額.ToString(),
                    LcExpiry = String.Format("{0:yyyy/MM/dd}", item.LcItem.有效期限),
                    LcNo = item.LetterOfCredit[0].LcNo,
                    NoAfterThan = String.Format("{0:yyyy/MM/dd}", item.SpecificNote.最後交貨日),
                    NonPatial = (!item.SpecificNote.分批交貨).ToString(),
                    Partial = item.SpecificNote.分批交貨.ToString(),
                    PayableBankAddr = item.PayableBank.Address,
                    PayableBankName = item.PayableBank.BranchName,
                    ReceiptNo = item.BeneficiaryData.CustomerOfBranch.Organization.ReceiptNo,
                    Seal = item.SpecificNote.原留印鑑相符.ToString(),
                    Usance = item.LcItem.定日付款.ToString(),
                    PaymentDate = item.LcItem.PaymentDate?.ToString("yyyy/MM/dd"),
                    AttachEInv = item.AttachableDocument.電子發票證明聯?.ToString(),
                    BeneficiaryNo = item.BeneficiaryData.Organization.ReceiptNo,
                    AttachOthers = item.AttachableDocument.其他,
                    InvDateStart = item.SpecificNote.押匯發票起始日?.ToString("yyyy/MM/dd"),
                    NegoDateStart = item.SpecificNote.押匯起始日?.ToString("yyyy/MM/dd"),
                    AcceptInvAddr = item.SpecificNote.接受發票人地址與受益人地址不符?.ToString(),
                    AcceptInvDetails = item.SpecificNote.貨品明細以發票為準?.ToString(),
                    AcceptEInv = item.SpecificNote.接受發票電子訊息?.ToString(),
                    LargerInvNego = item.SpecificNote.接受發票金額大於匯票金額?.ToString(),
                    SpecificOthers = item.SpecificNote.其他,
                    BankID = "009",
                    IssuingBank = item.開狀行,
                    PayableBank = item.付款行,
                    PdfBase64 = pdfBase64,
                    ToConfirm = item.AppID.EncryptKey(),
                }
            };

            SendLLC(uri, lcObj);
            return lcObj;
        }

        public static void SendLLC(this string uri, LLC lcObj,bool async = false)
        {
            var docMsg = lcObj.ConvertToXml();
            CryptoUtility.SignXmlSHA256(docMsg, null,
                null, AppSigner.SignerCertificate);

            if(async)
            {
                ThreadPool.QueueUserWorkItem(state =>
                {
                    try
                    {
                        DoSend(uri, docMsg);
                    } 
                    catch(Exception ex) 
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("Send:\r\n").Append(uri).Append("\r\nContent:\r\n")
                            .Append(docMsg.OuterXml).Append("\r\n")
                            .Append(ex);

                        CommonLib.Core.Utility.Logger.Warn(sb);
                    }
                });
            }
            else
            {
                DoSend(uri, docMsg);
            }
        }

        private static void DoSend(string uri, XmlDocument docMsg)
        {
            using (WebClient client = new WebClient())
            {
                if (!String.IsNullOrEmpty(Settings.Default.ProxyUrlToUXCDS))
                {
                    client.Proxy = new WebProxy(Settings.Default.ProxyUrlToUXCDS, true);
                }
                Encoding utf8 = new UTF8Encoding();
                client.Encoding = utf8;
                var stream = client.OpenWrite(uri);
                using (StreamWriter sw = new StreamWriter(stream, utf8))
                {
                    sw.Write(docMsg.OuterXml);
                    sw.Flush();
                }
                //docMsg.Save(stream);
                stream.Flush();
                stream.Close();
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("Send:\r\n").Append(uri).Append("\r\nContent:\r\n")
                .Append(docMsg.OuterXml);

            CommonLib.Core.Utility.Logger.Info(sb);
        }

        private static void asyncSendLLC(this string uri, LLC lcObj)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                    request.Method = "POST";
                    Stream output = request.GetRequestStream();
                    XmlTextWriter xtw = new XmlTextWriter(output, null);
                    xtw.Formatting = Formatting.Indented;

                    lcObj.ConvertToXml().Save(xtw);

                    xtw.Flush();
                    xtw.Close();

                    output.Flush();

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    long length = response.ContentLength;
                    HttpStatusCode status = response.StatusCode;
                }
                catch (Exception ex)
                {
                    ModelCore.Helper.Logger.Error(ex);
                }
            });
        }

		public static void SendAmendmentInfoToCDS(this AmendingLcApplication item)
		{
			string uri = new System.Text.StringBuilder(Settings.Default.urlToCDS)
				.Append("?type=LCRN&fileName=").Append(HttpUtility.UrlEncode("Advice-"+ item.申請書號碼 + ".xml"))
				.Append("&no=").Append(HttpUtility.UrlEncode(item.申請書號碼)).ToString();

            LLC lcObj = new LLC
            {
                LcAdvice = new LLCLcAdvice
                {
                    AdviceDate = String.Format("{0:yyyy/MM/dd}", item.申請日期),
                    AdviceNo = item.AmendingLcInformation.InformationNo,
                    AmendNo = item.申請書號碼,
                    AppNo = item.LetterOfCredit.CreditApplicationDocumentary.開狀申請書號碼,
                    LcNo = item.LetterOfCredit.LcNo
                }
            };

            asyncSendLLC(uri, lcObj);

		}

        public static LLC SendAmendmentInfo(this AmendingLcApplication item,String pdfBase64)
        {
            LetterOfCredit lc = item.LetterOfCredit;
            string uri = lc.CreditApplicationDocumentary.BeneficiaryData.Organization.OrganizationStatus?.BeneficiaryGroup?.BeneficiaryServiceGroup?.PostUrl;
            if (uri == null)
            {
                return null;
            }

            LLC lcObj = new LLC
            {
                LcAdvice = new LLCLcAdvice
                {
                    AdviceDate = String.Format("{0:yyyy/MM/dd}", item.申請日期),
                    AdviceNo = item.AmendingLcInformation?.InformationNo,
                    AmendNo = item.申請書號碼,
                    AppNo = lc.CreditApplicationDocumentary.開狀申請書號碼,
                    LcNo = lc.LcNo,
                    BankID = "009",
                    IssuingBank = lc.CreditApplicationDocumentary.開狀行,
                    ReceiptNo = lc.CreditApplicationDocumentary.BeneficiaryData.CustomerOfBranch.Organization.ReceiptNo,
                    PdfBase64 = pdfBase64,
                    ToConfirm = item.AmendingID.EncryptKey(),
                }
            };

            LcItem newItem, oldItem;
            AttachableDocument newAttach, oldAttach;
            SpecificNote newSN, oldSN;

            item.GetLcAmendmentItems(out newItem, out oldItem, out newAttach, out oldAttach, out newSN, out oldSN, out String newNotifyingBank, out String oldNotifyingBank);

            if (newItem.開狀金額 != oldItem.開狀金額)
            {
                lcObj.LcAdvice.LcAmt = $"{newItem.開狀金額}";
            }

            if (newItem.有效期限 != oldItem.有效期限)
            {
                lcObj.LcAdvice.LcExpiry = $"{newItem.有效期限:yyyy/MM/dd}";
            }

            if (newItem.定日付款 != oldItem.定日付款)
            {
                lcObj.LcAdvice.Usance = $"{newItem.定日付款}";
            }

            if (newItem.PaymentDate != oldItem.PaymentDate)
            {
                lcObj.LcAdvice.PaymentDate = $"{newItem.PaymentDate:yyyy/MM/dd}";
            }

            String newGoodsStr = newItem.BuildGoodsDetails(), oldGoodsStr = oldItem.BuildGoodsDetails();
            if (newGoodsStr != oldGoodsStr)
            {
                lcObj.LcAdvice.Goods = newGoodsStr;
            }

            if (newAttach.匯票付款申請書 != oldAttach.匯票付款申請書)
            {
                lcObj.LcAdvice.AttachPaying = $"{newAttach.匯票付款申請書}";
            }

            if (newAttach.統一發票 != oldAttach.統一發票)
            {
                lcObj.LcAdvice.AttachInv = $"{newAttach.統一發票}";
            }

            if (newAttach.電子發票證明聯 != oldAttach.電子發票證明聯)
            {
                lcObj.LcAdvice.AcceptEInv = $"{newAttach.電子發票證明聯}";
            }

            if (newAttach.其他 != oldAttach.其他)
            {
                lcObj.LcAdvice.AttachOthers = newAttach.其他;
            }
            if (newSN.原留印鑑相符 != oldSN.原留印鑑相符)
            {
                lcObj.LcAdvice.Seal = $"{newSN.原留印鑑相符}";
            }
            if (newSN.受益人單獨蓋章 != oldSN.受益人單獨蓋章)
            {
                lcObj.LcAdvice.BeneSeal = $"{newSN.受益人單獨蓋章}";
            }
            if (newSN.分批交貨 != oldSN.分批交貨)
            {
                lcObj.LcAdvice.Partial = $"{newSN.分批交貨}";
            }
            if (newSN.最後交貨日 != oldSN.最後交貨日)
            {
                lcObj.LcAdvice.NoAfterThan = $"{newSN.最後交貨日:yyyy/MM/dd}";
            }
            ///////
            if (newSN.押匯發票起始日 != oldSN.押匯發票起始日)
            {
                lcObj.LcAdvice.InvDateStart = $"{newSN.押匯發票起始日:yyyy/MM/dd}";
            }

            if (newSN.押匯起始日 != oldSN.押匯起始日)
            {
                lcObj.LcAdvice.NegoDateStart = $"{newSN.押匯起始日:yyyy/MM/dd}";
            }
            ///////
            if (newSN.接受發票金額大於開狀金額 != oldSN.接受發票金額大於開狀金額 && newSN.接受發票金額大於開狀金額.HasValue && oldSN.接受發票金額大於開狀金額.HasValue)
            {
                lcObj.LcAdvice.LargerInvAmt = $"{newSN.接受發票金額大於開狀金額}";
            }

            if (newSN.接受發票早於開狀日 != oldSN.接受發票早於開狀日 && newSN.接受發票早於開狀日.HasValue && oldSN.接受發票早於開狀日.HasValue)
            {
                lcObj.LcAdvice.EarlyInvDate = $"{newSN.接受發票早於開狀日}";
            }

            if (newSN.接受發票人地址與受益人地址不符 != oldSN.接受發票人地址與受益人地址不符 && newSN.接受發票人地址與受益人地址不符.HasValue && oldSN.接受發票人地址與受益人地址不符.HasValue)
            {
                lcObj.LcAdvice.AcceptInvAddr = $"{newSN.接受發票人地址與受益人地址不符}";
            }

            if (newSN.貨品明細以發票為準 != oldSN.貨品明細以發票為準 && newSN.貨品明細以發票為準.HasValue && oldSN.貨品明細以發票為準.HasValue)
            {
                lcObj.LcAdvice.AcceptInvDetails = newSN.貨品明細以發票為準?.ToString();
            }

            if (newSN.接受發票電子訊息 != oldSN.接受發票電子訊息 && newSN.接受發票電子訊息.HasValue && oldSN.接受發票電子訊息.HasValue)
            {
                lcObj.LcAdvice.AcceptEInv = newSN.接受發票電子訊息?.ToString();
            }

            if (newSN.接受發票金額大於匯票金額 != oldSN.接受發票金額大於匯票金額 && newSN.接受發票金額大於匯票金額.HasValue && oldSN.接受發票金額大於匯票金額.HasValue)
            {
                lcObj.LcAdvice.LargerInvNego = newSN.接受發票金額大於匯票金額?.ToString();
            }

            if (newSN.其他 != oldSN.其他)
            {
                lcObj.LcAdvice.SpecificOthers = newSN.其他;
            }

            SendLLC(uri, lcObj);
            return lcObj;
        }

        public static void SendLcCancellationToCDS(CreditCancellation item)
		{
            string uri = new System.Text.StringBuilder(Settings.Default.urlToCDS)
                .Append("?type=Reply&fileName=").Append(HttpUtility.UrlEncode("LCD-" + item.註銷申請號碼 + ".xml"))
                .Append("&no=").Append(HttpUtility.UrlEncode(item.註銷申請號碼)).ToString();

            LLC lcObj = new LLC
            {
                ApplyLcNo = new LLCApplyLcNo {
                    ApplNo = item.LetterOfCredit.CreditApplicationDocumentary.開狀申請書號碼,
                    LcNo = item.LetterOfCredit.LcNo,
                    CancelNo = item.註銷申請號碼
                },
                Reject = new LLCReject
                {
                    Code = 5
                },
                Type = "LCD"
            };

            asyncSendLLC(uri, lcObj);

		}

        public static LLC SendLcCancellation(this CreditCancellation item, String pdfBase64)
        {
            LetterOfCredit lc = item.LetterOfCredit;
            string uri = lc.CreditApplicationDocumentary.BeneficiaryData.Organization.OrganizationStatus?.BeneficiaryGroup?.BeneficiaryServiceGroup?.PostUrl;
            if (uri == null)
            {
                return null;
            }

            LLC lcObj = new LLC
            {
                LcCancel = new LLCLcCancel
                {
                    ApplNo = lc.CreditApplicationDocumentary.開狀申請書號碼,
                    LcNo = lc.LcNo,
                    CancelNo = item.註銷申請號碼,
                    PdfBase64 = pdfBase64,
                    BankID = "009",
                    IssuingBank = lc.CreditApplicationDocumentary.開狀行,
                    CancelDate = $"{item.CreditCancellationInfo?.CancellationDate:yyyy/MM/dd}",
                    ReceiptNo = lc.CreditApplicationDocumentary.BeneficiaryData.CustomerOfBranch.Organization.ReceiptNo,
                    ToConfirm = item.CancellationID.EncryptKey(),
                }
            };

            SendLLC(uri, lcObj);
            return lcObj;
        }


        public static void SendRejection(Documentary rejectData, string reason)
        {
            LLC dsAppl = new LLC
            {
                ApplyLcNo = new LLCApplyLcNo { },
                Reject = new LLCReject
                {
                    Code = 6,
                    Reason = reason
                }
            };

            object docNo;

            switch ((Naming.DocumentTypeDefinition)rejectData.DocType)
            {
                case Naming.DocumentTypeDefinition.開狀申請書:
                    dsAppl.Type = "LCA";
                    docNo = rejectData.CreditApplicationDocumentary.開狀申請書號碼;
                    dsAppl.ApplyLcNo.ApplNo = rejectData.CreditApplicationDocumentary.開狀申請書號碼;
                    break;
                case Naming.DocumentTypeDefinition.修狀申請書:
                    dsAppl.Type = "LCR";
                    docNo = rejectData.AmendingLcApplication.LetterOfCredit.CreditApplicationDocumentary.開狀申請書號碼;
                    dsAppl.ApplyLcNo.ApplNo = rejectData.AmendingLcApplication.LetterOfCredit.CreditApplicationDocumentary.開狀申請書號碼;
                    dsAppl.ApplyLcNo.LcNo = rejectData.AmendingLcApplication.LetterOfCredit.LcNo;
                    dsAppl.ApplyLcNo.AmendNo = rejectData.AmendingLcApplication.申請書號碼;
                    break;
                case Naming.DocumentTypeDefinition.信用狀註銷申請書:
                    dsAppl.Type = "LCD";
                    docNo = rejectData.CreditCancellation.註銷申請號碼;
                    dsAppl.ApplyLcNo.ApplNo = rejectData.CreditCancellation.LetterOfCredit.CreditApplicationDocumentary.開狀申請書號碼;
                    dsAppl.ApplyLcNo.LcNo = rejectData.CreditCancellation.LetterOfCredit.LcNo;
                    dsAppl.ApplyLcNo.CancelNo = rejectData.CreditCancellation.註銷申請號碼;
                    break;
                default:
                    return;
            }

            string uri = new System.Text.StringBuilder(Settings.Default.urlToCDS)
                .Append("?type=Reply&fileName=").Append(HttpUtility.UrlEncode("Reject-" + docNo.ToString() + ".xml")).ToString();

            asyncSendLLC(uri, dsAppl);
        }

	
		public static IDictionary SendDocumentFlowControlRequest(string url)
		{
			Hashtable result = new Hashtable();
			try
			{

				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				request.Method = "GET";

				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				long length = response.ContentLength;
				HttpStatusCode status = response.StatusCode;

				string line;
				StreamReader sr = new StreamReader(response.GetResponseStream());
				while((line=sr.ReadLine())!=null)
				{
					string[] valuePair = line.Split('=');
					if(valuePair.Length>1)
					{
						result[valuePair[0].Trim()] = valuePair[1].Trim();
					}

				}

				sr.Close();

			}
			catch(Exception ex)
			{
				ModelCore.Helper.Logger.Error(ex);
				result.Clear();
			}

			return result;
		}

        public static LLC AllowNegoDraft(this NegoDraft item)
        {

            string uri = item.LetterOfCredit?.CreditApplicationDocumentary.BeneficiaryData.Organization.OrganizationStatus?.BeneficiaryGroup?.BeneficiaryServiceGroup?.PostUrl
                        ?? item.NegoLC?.BeneficiaryData.Organization.OrganizationStatus?.BeneficiaryGroup?.BeneficiaryServiceGroup?.PostUrl;

            if (uri == null)
            {
                return null;
            }

            LLC lcObj = new LLC
            {
                AllowedNego = new LLCAllowedNego
                {
                    DraftNo = item.DraftNo,
                    LcNo = item.LetterOfCredit?.LcNo ?? item.NegoLC?.LCNo,
                    BankID = "009",
                    ToConfirm = item.DraftID.EncryptKey(),
                }
            };

            SendLLC(uri, lcObj);
            return lcObj;
        }

        public static LLC RejectNegoDraft(this NegoDraft item)
        {

            string uri = item.LetterOfCredit?.CreditApplicationDocumentary.BeneficiaryData.Organization.OrganizationStatus?.BeneficiaryGroup?.BeneficiaryServiceGroup?.PostUrl
                        ?? item.NegoLC?.BeneficiaryData.Organization.OrganizationStatus?.BeneficiaryGroup?.BeneficiaryServiceGroup?.PostUrl;

            if (uri == null)
            {
                return null;
            }

            LLC lcObj = new LLC
            {
                RejectedNego = new LLCRejectedNego
                {
                    DraftNo = item.DraftNo,
                    LcNo = item.LetterOfCredit?.LcNo ?? item.NegoLC?.LCNo,
                    BankID = "009",
                    ToConfirm = item.DraftID.EncryptKey(),
                }
            };

            SendLLC(uri, lcObj, true);
            return lcObj;
        }

        public static void IntentToDispatch(this NegoDraft draft)
        {
            draft.Documentary.IntentToDispatch();
        }

        public static void IntentToDispatch(this Documentary doc)
        {
            if (doc.DocumentDispatch == null)
            {
                doc.DocumentDispatch = new DocumentDispatch { };
            }
        }
    }
}
