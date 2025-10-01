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
	/// OutboundSvc ���K�n�y�z�C
	/// </summary>
	public static class OutboundSvc
	{
        public static void SendLcToCDS(this CreditApplicationDocumentary item)
        {
            string uri = new System.Text.StringBuilder(Settings.Default.urlToCDS)
                .Append("?type=LC&fileName=").Append(HttpUtility.UrlEncode("LC-" + item.LetterOfCredit[0].LcNo + ".xml"))
                .Append("&no=").Append(HttpUtility.UrlEncode(item.�}���ӽЮѸ��X)).ToString();

            LLC lcObj = new LLC
            {
                LC = new LLCLC
                {
                    AdvisingBank = item.�q����,
                    AdvisingBankName = item.AdvisingBank.BranchName,
                    AppDate = item.�}���ӽФ��.ToString("yyyy/MM/dd"),
                    Applicant = item.ApplicantDetails.CompanyName,
                    ApplicantAddr = item.ApplicantDetails.Addr,
                    AppNo = item.�}���ӽЮѸ��X,
                    AtSight = item.�����Y�I.ToString(),
                    AttachAcceptance = item.AttachableDocument.�ײ��ӧI�ӽЮ�.ToString(),
                    AttachInv = item.AttachableDocument.�Τ@�o��.ToString(),
                    AttachPaying = item.AttachableDocument.�ײ��I�ڥӽЮ�.ToString(),
                    AtUsance = (!item.�����Y�I).ToString(),
                    Beneficiary = item.BeneDetails.CompanyName,
                    BeneSeal = item.SpecificNote.���q�H��W�\��.ToString(),
                    EarlyInvDate = item.SpecificNote.�����o������}����.ToString(),
                    Goods = item.LcItem.Goods,
                    IssuingDate = item.LetterOfCredit[0].LcDate.ToString("yyyy/MM/dd"),
                    LargerInvAmt = item.SpecificNote.�����o�����B�j��}�����B.ToString(),
                    LcAmt = item.LcItem.�}�����B.ToString(),
                    LcExpiry = String.Format("{0:yyyy/MM/dd}",item.LcItem.���Ĵ���),
                    LcNo = item.LetterOfCredit[0].LcNo,
                    NoAfterThan = String.Format("{0:yyyy/MM/dd}",item.SpecificNote.�̫��f��),
                    NonPatial = (!item.SpecificNote.�����f).ToString(),
                    Partial = item.SpecificNote.�����f.ToString(),
                    PayableBankAddr = item.PayableBank.Address,
                    PayableBankName = item.PayableBank.BranchName,
                    ReceiptNo = item.BeneficiaryData.CustomerOfBranch.Organization.ReceiptNo,
                    Seal = item.SpecificNote.��d�LŲ�۲�.ToString(),
                    Usance = item.LcItem.�w��I��.ToString()
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
                    AdvisingBank = item.�q����,
                    AdvisingBankName = item.AdvisingBank.BranchName,
                    AppDate = item.�}���ӽФ��.ToString("yyyy/MM/dd"),
                    Applicant = item.ApplicantDetails.CompanyName,
                    ApplicantAddr = item.ApplicantDetails.Addr,
                    AppNo = item.�}���ӽЮѸ��X,
                    AtSight = item.�����Y�I.ToString(),
                    AttachAcceptance = item.AttachableDocument.�ײ��ӧI�ӽЮ�.ToString(),
                    AttachInv = item.AttachableDocument.�Τ@�o��.ToString(),
                    AttachPaying = item.AttachableDocument.�ײ��I�ڥӽЮ�.ToString(),
                    AtUsance = (!item.�����Y�I).ToString(),
                    Beneficiary = item.BeneDetails.CompanyName,
                    BeneSeal = item.SpecificNote.���q�H��W�\��.ToString(),
                    EarlyInvDate = item.SpecificNote.�����o������}����.ToString(),
                    Goods = item.LcItem.BuildGoodsDetails(),
                    IssuingDate = item.LetterOfCredit[0].LcDate.ToString("yyyy/MM/dd"),
                    LargerInvAmt = item.SpecificNote.�����o�����B�j��}�����B.ToString(),
                    LcAmt = item.LcItem.�}�����B.ToString(),
                    LcExpiry = String.Format("{0:yyyy/MM/dd}", item.LcItem.���Ĵ���),
                    LcNo = item.LetterOfCredit[0].LcNo,
                    NoAfterThan = String.Format("{0:yyyy/MM/dd}", item.SpecificNote.�̫��f��),
                    NonPatial = (!item.SpecificNote.�����f).ToString(),
                    Partial = item.SpecificNote.�����f.ToString(),
                    PayableBankAddr = item.PayableBank.Address,
                    PayableBankName = item.PayableBank.BranchName,
                    ReceiptNo = item.BeneficiaryData.CustomerOfBranch.Organization.ReceiptNo,
                    Seal = item.SpecificNote.��d�LŲ�۲�.ToString(),
                    Usance = item.LcItem.�w��I��.ToString(),
                    PaymentDate = item.LcItem.PaymentDate?.ToString("yyyy/MM/dd"),
                    AttachEInv = item.AttachableDocument.�q�l�o���ҩ��p?.ToString(),
                    BeneficiaryNo = item.BeneficiaryData.Organization.ReceiptNo,
                    AttachOthers = item.AttachableDocument.��L,
                    InvDateStart = item.SpecificNote.��׵o���_�l��?.ToString("yyyy/MM/dd"),
                    NegoDateStart = item.SpecificNote.��װ_�l��?.ToString("yyyy/MM/dd"),
                    AcceptInvAddr = item.SpecificNote.�����o���H�a�}�P���q�H�a�}����?.ToString(),
                    AcceptInvDetails = item.SpecificNote.�f�~���ӥH�o������?.ToString(),
                    AcceptEInv = item.SpecificNote.�����o���q�l�T��?.ToString(),
                    LargerInvNego = item.SpecificNote.�����o�����B�j��ײ����B?.ToString(),
                    SpecificOthers = item.SpecificNote.��L,
                    BankID = "009",
                    IssuingBank = item.�}����,
                    PayableBank = item.�I�ڦ�,
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
				.Append("?type=LCRN&fileName=").Append(HttpUtility.UrlEncode("Advice-"+ item.�ӽЮѸ��X + ".xml"))
				.Append("&no=").Append(HttpUtility.UrlEncode(item.�ӽЮѸ��X)).ToString();

            LLC lcObj = new LLC
            {
                LcAdvice = new LLCLcAdvice
                {
                    AdviceDate = String.Format("{0:yyyy/MM/dd}", item.�ӽФ��),
                    AdviceNo = item.AmendingLcInformation.InformationNo,
                    AmendNo = item.�ӽЮѸ��X,
                    AppNo = item.LetterOfCredit.CreditApplicationDocumentary.�}���ӽЮѸ��X,
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
                    AdviceDate = String.Format("{0:yyyy/MM/dd}", item.�ӽФ��),
                    AdviceNo = item.AmendingLcInformation?.InformationNo,
                    AmendNo = item.�ӽЮѸ��X,
                    AppNo = lc.CreditApplicationDocumentary.�}���ӽЮѸ��X,
                    LcNo = lc.LcNo,
                    BankID = "009",
                    IssuingBank = lc.CreditApplicationDocumentary.�}����,
                    ReceiptNo = lc.CreditApplicationDocumentary.BeneficiaryData.CustomerOfBranch.Organization.ReceiptNo,
                    PdfBase64 = pdfBase64,
                    ToConfirm = item.AmendingID.EncryptKey(),
                }
            };

            LcItem newItem, oldItem;
            AttachableDocument newAttach, oldAttach;
            SpecificNote newSN, oldSN;

            item.GetLcAmendmentItems(out newItem, out oldItem, out newAttach, out oldAttach, out newSN, out oldSN, out String newNotifyingBank, out String oldNotifyingBank);

            if (newItem.�}�����B != oldItem.�}�����B)
            {
                lcObj.LcAdvice.LcAmt = $"{newItem.�}�����B}";
            }

            if (newItem.���Ĵ��� != oldItem.���Ĵ���)
            {
                lcObj.LcAdvice.LcExpiry = $"{newItem.���Ĵ���:yyyy/MM/dd}";
            }

            if (newItem.�w��I�� != oldItem.�w��I��)
            {
                lcObj.LcAdvice.Usance = $"{newItem.�w��I��}";
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

            if (newAttach.�ײ��I�ڥӽЮ� != oldAttach.�ײ��I�ڥӽЮ�)
            {
                lcObj.LcAdvice.AttachPaying = $"{newAttach.�ײ��I�ڥӽЮ�}";
            }

            if (newAttach.�Τ@�o�� != oldAttach.�Τ@�o��)
            {
                lcObj.LcAdvice.AttachInv = $"{newAttach.�Τ@�o��}";
            }

            if (newAttach.�q�l�o���ҩ��p != oldAttach.�q�l�o���ҩ��p)
            {
                lcObj.LcAdvice.AcceptEInv = $"{newAttach.�q�l�o���ҩ��p}";
            }

            if (newAttach.��L != oldAttach.��L)
            {
                lcObj.LcAdvice.AttachOthers = newAttach.��L;
            }
            if (newSN.��d�LŲ�۲� != oldSN.��d�LŲ�۲�)
            {
                lcObj.LcAdvice.Seal = $"{newSN.��d�LŲ�۲�}";
            }
            if (newSN.���q�H��W�\�� != oldSN.���q�H��W�\��)
            {
                lcObj.LcAdvice.BeneSeal = $"{newSN.���q�H��W�\��}";
            }
            if (newSN.�����f != oldSN.�����f)
            {
                lcObj.LcAdvice.Partial = $"{newSN.�����f}";
            }
            if (newSN.�̫��f�� != oldSN.�̫��f��)
            {
                lcObj.LcAdvice.NoAfterThan = $"{newSN.�̫��f��:yyyy/MM/dd}";
            }
            ///////
            if (newSN.��׵o���_�l�� != oldSN.��׵o���_�l��)
            {
                lcObj.LcAdvice.InvDateStart = $"{newSN.��׵o���_�l��:yyyy/MM/dd}";
            }

            if (newSN.��װ_�l�� != oldSN.��װ_�l��)
            {
                lcObj.LcAdvice.NegoDateStart = $"{newSN.��װ_�l��:yyyy/MM/dd}";
            }
            ///////
            if (newSN.�����o�����B�j��}�����B != oldSN.�����o�����B�j��}�����B && newSN.�����o�����B�j��}�����B.HasValue && oldSN.�����o�����B�j��}�����B.HasValue)
            {
                lcObj.LcAdvice.LargerInvAmt = $"{newSN.�����o�����B�j��}�����B}";
            }

            if (newSN.�����o������}���� != oldSN.�����o������}���� && newSN.�����o������}����.HasValue && oldSN.�����o������}����.HasValue)
            {
                lcObj.LcAdvice.EarlyInvDate = $"{newSN.�����o������}����}";
            }

            if (newSN.�����o���H�a�}�P���q�H�a�}���� != oldSN.�����o���H�a�}�P���q�H�a�}���� && newSN.�����o���H�a�}�P���q�H�a�}����.HasValue && oldSN.�����o���H�a�}�P���q�H�a�}����.HasValue)
            {
                lcObj.LcAdvice.AcceptInvAddr = $"{newSN.�����o���H�a�}�P���q�H�a�}����}";
            }

            if (newSN.�f�~���ӥH�o������ != oldSN.�f�~���ӥH�o������ && newSN.�f�~���ӥH�o������.HasValue && oldSN.�f�~���ӥH�o������.HasValue)
            {
                lcObj.LcAdvice.AcceptInvDetails = newSN.�f�~���ӥH�o������?.ToString();
            }

            if (newSN.�����o���q�l�T�� != oldSN.�����o���q�l�T�� && newSN.�����o���q�l�T��.HasValue && oldSN.�����o���q�l�T��.HasValue)
            {
                lcObj.LcAdvice.AcceptEInv = newSN.�����o���q�l�T��?.ToString();
            }

            if (newSN.�����o�����B�j��ײ����B != oldSN.�����o�����B�j��ײ����B && newSN.�����o�����B�j��ײ����B.HasValue && oldSN.�����o�����B�j��ײ����B.HasValue)
            {
                lcObj.LcAdvice.LargerInvNego = newSN.�����o�����B�j��ײ����B?.ToString();
            }

            if (newSN.��L != oldSN.��L)
            {
                lcObj.LcAdvice.SpecificOthers = newSN.��L;
            }

            SendLLC(uri, lcObj);
            return lcObj;
        }

        public static void SendLcCancellationToCDS(CreditCancellation item)
		{
            string uri = new System.Text.StringBuilder(Settings.Default.urlToCDS)
                .Append("?type=Reply&fileName=").Append(HttpUtility.UrlEncode("LCD-" + item.���P�ӽи��X + ".xml"))
                .Append("&no=").Append(HttpUtility.UrlEncode(item.���P�ӽи��X)).ToString();

            LLC lcObj = new LLC
            {
                ApplyLcNo = new LLCApplyLcNo {
                    ApplNo = item.LetterOfCredit.CreditApplicationDocumentary.�}���ӽЮѸ��X,
                    LcNo = item.LetterOfCredit.LcNo,
                    CancelNo = item.���P�ӽи��X
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
                    ApplNo = lc.CreditApplicationDocumentary.�}���ӽЮѸ��X,
                    LcNo = lc.LcNo,
                    CancelNo = item.���P�ӽи��X,
                    PdfBase64 = pdfBase64,
                    BankID = "009",
                    IssuingBank = lc.CreditApplicationDocumentary.�}����,
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
                case Naming.DocumentTypeDefinition.�}���ӽЮ�:
                    dsAppl.Type = "LCA";
                    docNo = rejectData.CreditApplicationDocumentary.�}���ӽЮѸ��X;
                    dsAppl.ApplyLcNo.ApplNo = rejectData.CreditApplicationDocumentary.�}���ӽЮѸ��X;
                    break;
                case Naming.DocumentTypeDefinition.�ת��ӽЮ�:
                    dsAppl.Type = "LCR";
                    docNo = rejectData.AmendingLcApplication.LetterOfCredit.CreditApplicationDocumentary.�}���ӽЮѸ��X;
                    dsAppl.ApplyLcNo.ApplNo = rejectData.AmendingLcApplication.LetterOfCredit.CreditApplicationDocumentary.�}���ӽЮѸ��X;
                    dsAppl.ApplyLcNo.LcNo = rejectData.AmendingLcApplication.LetterOfCredit.LcNo;
                    dsAppl.ApplyLcNo.AmendNo = rejectData.AmendingLcApplication.�ӽЮѸ��X;
                    break;
                case Naming.DocumentTypeDefinition.�H�Ϊ����P�ӽЮ�:
                    dsAppl.Type = "LCD";
                    docNo = rejectData.CreditCancellation.���P�ӽи��X;
                    dsAppl.ApplyLcNo.ApplNo = rejectData.CreditCancellation.LetterOfCredit.CreditApplicationDocumentary.�}���ӽЮѸ��X;
                    dsAppl.ApplyLcNo.LcNo = rejectData.CreditCancellation.LetterOfCredit.LcNo;
                    dsAppl.ApplyLcNo.CancelNo = rejectData.CreditCancellation.���P�ӽи��X;
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
