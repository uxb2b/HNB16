using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;

using CommonLib.DataAccess;
using EAI.Service.Transaction;
using ModelCore.DataModel;
using ModelCore.Helper;
using ModelCore.Locale;
using ModelCore.Properties;
using ModelCore.UserManagement;
using CommonLib.Utility;
using CommonLib.Security.UseCrypto;

namespace ModelCore.Service.FPG
{
    public static partial class ExtensionMethods
    {
        public static IQueryable<CreditApplicationDocumentary> GetCreditApplicationReadyToSend(this GenericManager<LcEntityDataContext> mgr,int? serviceID = null)
        {
            var items = mgr.GetTable<DocumentDispatch>().Select(d => d.Documentary.CreditApplicationDocumentary)
                    .Where(c => c.FpgLcItem != null);

            if(serviceID.HasValue)
            {
                var groups = mgr.GetTable<BeneficiaryGroup>().Where(b => b.ServiceID == serviceID);
                items = items.Where(c => groups.Any(g => c.FpgLcItem.GroupID == g.GroupID));
            }

            return items;
        }

        public static IQueryable<AmendingLcApplication> GetLcAmendmentReadyToSend(this GenericManager<LcEntityDataContext> mgr, int? serviceID = null)
        {
            var items =  mgr.GetTable<DocumentDispatch>().Select(d => d.Documentary.AmendingLcApplication)
                    .Where(c => c.LetterOfCredit.CreditApplicationDocumentary.FpgLcItem != null);

            if (serviceID.HasValue)
            {
                var groups = mgr.GetTable<BeneficiaryGroup>().Where(b => b.ServiceID == serviceID);
                items = items.Where(c => groups.Any(g => c.LetterOfCredit.CreditApplicationDocumentary.FpgLcItem.GroupID == g.GroupID));
            }

            return items;
        }

        public static IQueryable<NegoDraft> GetNegoDraftReadyToSend(this GenericManager<LcEntityDataContext> mgr, Naming.DocumentLevel level, int? serviceID = null)
        {
            var items =  mgr.GetTable<DocumentDispatch>()
                .Select(d => d.Documentary)
                .Where(d => d.CurrentLevel == (int)level)
                .Select(d => d.NegoDraft)
                .Where(c => c.LetterOfCredit.CreditApplicationDocumentary.FpgLcItem != null);

            if (serviceID.HasValue)
            {
                var groups = mgr.GetTable<BeneficiaryGroup>().Where(b => b.ServiceID == serviceID);
                items = items.Where(c => groups.Any(g => c.LetterOfCredit.CreditApplicationDocumentary.FpgLcItem.GroupID == g.GroupID));
            }

            return items;
        }

        public static IQueryable<NegoDraft> GetFpgNegoRemittanceCheckList(this GenericManager<LcEntityDataContext> mgr, IQueryable<NegoDraft> items)
        {
            return items.Join(mgr.GetTable<FpgNegoDraft>()
                    .Where(f => f.FpgNegoRemittance == null || f.FpgNegoRemittance.Status == (int)Naming.RemittanceStatusDefinition.匯款退回)
                    , n => n.DraftID, f => f.DraftID, (n, f) => n)
                .Join(mgr.GetTable<Documentary>().Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.已開立)
                    , n => n.DraftID, d => d.DocID, (n, d) => n);
        }

        public static IQueryable<NegoDraft> GetFpgNegoRemittanceAllowingList(this GenericManager<LcEntityDataContext> mgr, IQueryable<NegoDraft> items)
        {
            return items.Join(mgr.GetTable<FpgNegoDraft>()
                    .Where(f => f.FpgNegoRemittance.Status == (int)Naming.RemittanceStatusDefinition.匯款待審核)
                    , n => n.DraftID, f => f.DraftID, (n, f) => n)
                .Join(mgr.GetTable<Documentary>().Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.已開立)
                    , n => n.DraftID, d => d.DocID, (n, d) => n);
        }

        public static IQueryable<FpgNegoRemittance> GetFpgNegoRemittanceMarkingList(this GenericManager<LcEntityDataContext> mgr, IQueryable<NegoDraft> items)
        {
            return items.Join(mgr.GetTable<FpgNegoRemittance>()
                    .Where(f => /*f.Status == (int)Naming.RemittanceStatusDefinition.匯款資料已送出
                        ||*/ f.Status == (int)Naming.RemittanceStatusDefinition.匯款失敗
                        || f.Status == (int)Naming.RemittanceStatusDefinition.匯款資料已送出)
                    , n => n.DraftID, f => f.DraftID, (n, f) => f);
        }


        public static IQueryable<NegoDraft> GetFpgNegoDraftByLevel(this GenericManager<LcEntityDataContext> mgr, IQueryable<NegoDraft> items, Naming.DocumentLevel draftLevel)
        {
            return items.Join(mgr.GetTable<FpgNegoDraft>()
                    , n => n.DraftID, f => f.DraftID, (n, f) => n)
                .Join(mgr.GetTable<Documentary>().Where(d => d.CurrentLevel == (int)draftLevel)
                    , n => n.DraftID, d => d.DocID, (n, d) => n);
        }

        public static IQueryable<NegoDraft> GetFpgNegoDraftDefectsTodo(this GenericManager<LcEntityDataContext> mgr, IQueryable<NegoDraft> items)
        {
            return mgr.GetFpgNegoDraftByLevel(items, Naming.DocumentLevel.瑕疵押匯)
                .Where(d => d.DownloadFlag == 1);
        }


        public static IQueryable<FpgNegoRemittance> GetFpgNegoRemittanceReadyToSend(this GenericManager<LcEntityDataContext> mgr)
        {
            DateTime readyDate = DateTime.Today.AddDays(1);
            //return mgr.GetTable<DocumentDispatch>()
            //    .Join(mgr.GetTable<NegoDraft>(), d => d.DocID, n => n.DraftID, (d, n) => n)
            //    .Join(mgr.GetTable<FpgNegoRemittance>()
            //            .Where(f => f.RemittanceDate < readyDate)
            //            .Where(r => r.Status == (int)Naming.RemittanceStatusDefinition.匯款資料準備中),
            //        n => n.DraftID, r => r.DraftID, (n, r) => r);

            return mgr.GetTable<NegoDraft>()
                    .Join(mgr.GetTable<FpgNegoRemittance>()
                            .Where(f => f.RemittanceDate < readyDate)
                            .Where(r => r.Status == (int)Naming.RemittanceStatusDefinition.匯款資料準備中),
                        n => n.DraftID, r => r.DraftID, (n, r) => r);
        }

        public static IQueryable<FpgNegoRemittanceLog> GetReadyToSendRemittanceEAI(this GenericManager<LcEntityDataContext> mgr)
        {
            return mgr.GetTable<FpgNegoRemittanceDispatch>()
                .Join(mgr.GetTable<FpgNegoRemittanceLog>()
                        .Where(r => r.Status == (int)Naming.RemittanceStatusDefinition.匯款資料已送出),
                    n => n.RemittanceID, r => r.RemittanceID, (n, r) => r)
                .Join(mgr.GetTable<FpgNegoRemittance>(),
                    r => r.DraftID, f => f.DraftID, (r, f) => r);
        }

        public static IQueryable<FpgNegoRemittanceLog> GetReadyToSendA1000(this GenericManager<LcEntityDataContext> mgr)
        {
            return mgr.GetTable<FpgNegoRemittanceDispatch>()
                .Join(mgr.GetTable<FpgNegoRemittanceLog>()
                        .Where(r => r.Status == (int)Naming.RemittanceStatusDefinition.匯款資料已送出),
                    n => n.RemittanceID, r => r.RemittanceID, (n, r) => r)
                .Join(mgr.GetTable<FpgNegoRemittance>(),
                    r => r.DraftID, f => f.DraftID, (r, f) => r);
        }

        public static DataPortLog SendToFPG(this GenericManager<LcEntityDataContext> mgr, ModelCore.Schema.FPG.J2SP item, BeneficiaryServiceGroup.ServiceDefinition? serviceID)
        {
            String fpgPost = mgr.GetTable<BeneficiaryServiceGroup>().Where(s => s.ServiceID == (int?)serviceID)
                    .FirstOrDefault()?.PostUrl;

            return SendToFPG(mgr, item, fpgPost, serviceID);
        }


        public static DataPortLog SendToFPG(this GenericManager<LcEntityDataContext> mgr, ModelCore.Schema.FPG.J2SP item, String fpgPost, BeneficiaryServiceGroup.ServiceDefinition? serviceID = null)
        {
            var docMsg = item.ConvertToXml();
            CryptoUtility.SignXmlSHA256(docMsg, null,
                null, AppSigner.SignerCertificate, "#sign");

            String fileName = Path.Combine(CommonLib.Core.Utility.Logger.LogDailyPath, item.FILENAME);
            var utf8Encoding = new UTF8Encoding();
            using (StreamWriter sw = new StreamWriter(fileName,false,utf8Encoding))
            {
                sw.Write(docMsg.OuterXml);
                sw.Flush();
                sw.Close();
            }
            //docMsg.Save(fileName);

            DataPortLog log = new DataPortLog
            {
                ContentPath = fileName,
                Direction = (int)Naming.TransportDirection.Outbound,
                TransportTime = DateTime.Now,
                FpgTableName = item.TABLENAME,
                FpgFileName = String.Format("{0}{1}", item.FILENAME, item.INITIALFILE),
                ServiceID = (int?)serviceID,
            };

            mgr.GetTable<DataPortLog>().InsertOnSubmit(log);

            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Encoding = utf8Encoding;
                    var stream = client.OpenWrite(fpgPost ?? Settings.Default.FpgPost);
                    using (StreamWriter sw = new StreamWriter(stream, utf8Encoding))
                    {
                        sw.Write(docMsg.OuterXml);
                        sw.Flush();
                    }
                    stream.Flush();
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                ModelCore.Helper.Logger.Error(ex, ModelCore.Helper.Logger.LogLevel.Error);
                log.ExceptionLog = new ExceptionLog
                {
                    LogTime = DateTime.Now,
                    Message = ex.Message
                };
            }
            finally
            {
                mgr.SubmitChanges();
            }
            return log;
        }

        public static void CommitToFPG(this ModelCore.Schema.FPG.J2SP item,String fpgConfirm)
        {
            if (item.ITEM != null)
            {
                item.ITEM.id = "sign";
            }

            var docMsg = item.ConvertToXml();
            CryptoUtility.SignXmlSHA256(docMsg, null,
                null, AppSigner.SignerCertificate, item?.ITEM?.id == "sign" ? "#sign" : "");

            using (WebClient client = new WebClient())
            {
                Encoding utf8 = new UTF8Encoding();
                client.Encoding = utf8;
                var stream = client.OpenWrite(fpgConfirm ?? Settings.Default.FpgConfirm);
                using (StreamWriter sw = new StreamWriter(stream, utf8))
                {
                    sw.Write(docMsg.OuterXml);
                    sw.Flush();
                }
                //                docMsg.Save(stream);
                stream.Flush();
                stream.Close();
            }

        }


        public static void ProcessConfirmation(this GenericManager<LcEntityDataContext> mgr, XmlDocument docMsg, BeneficiaryServiceGroup.ServiceDefinition? serviceID)
        {
            DataPortLog log = mgr.processInput(docMsg);

            try
            {
                if (docMsg.GetXmlSignature() != null)
                {
                    CryptoUtility crypto = FpgAgent.CreateCryptoInstance();
                    if (!crypto.VerifyXmlSignature(docMsg))
                    {
                        throw new Exception("驗簽失敗!!");
                    }
                }

                log.ReceivedDataQueue = new ReceivedDataQueue { };
                log.ServiceID = (int?)serviceID;

            }
            catch (Exception ex)
            {
                ModelCore.Helper.Logger.Error(ex, ModelCore.Helper.Logger.LogLevel.Error);
                log.ExceptionLog = new ExceptionLog
                {
                    LogTime = DateTime.Now,
                    Message = ex.Message
                };
            }
            finally
            {
                mgr.SubmitChanges();
            }
        }

        private static DataPortLog processInput(this GenericManager<LcEntityDataContext> mgr, XmlDocument docMsg)
        {
            DateTime now = DateTime.Now;
            String fileName = Path.Combine(CommonLib.Core.Utility.Logger.LogDailyPath, String.Format("{0:yyyyMMdd-HHmmssfff}-{1}.xml", now, Guid.NewGuid()));
            docMsg.Save(fileName);

            DataPortLog log = new DataPortLog
            {
                ContentPath = fileName,
                Direction = (int)Naming.TransportDirection.Inbound,
                TransportTime = now,
            };

            mgr.GetTable<DataPortLog>().InsertOnSubmit(log);

            return log;
        }

        public static void PrepareServiceRequest(this GenericManager<LcEntityDataContext> mgr, XmlDocument docMsg,BeneficiaryServiceGroup.ServiceDefinition serviceID = BeneficiaryServiceGroup.ServiceDefinition.Fpg)
        {
            DataPortLog log = mgr.processInput(docMsg);

            try
            {
                if (docMsg.GetXmlSignature() != null)
                {
                    CryptoUtility crypto = FpgAgent.CreateCryptoInstance();
                    if (!crypto.VerifyXmlSignature(docMsg))
                    {
                        throw new Exception("驗簽失敗!!");
                    }
                }

                log.ServiceDataQueue = new ServiceDataQueue
                {
                    ServiceID = (int)serviceID,
                };
                log.ServiceID = (int)serviceID;
            }
            catch (Exception ex)
            {
                ModelCore.Helper.Logger.Error(ex);
                log.ExceptionLog = new ExceptionLog
                {
                    LogTime = DateTime.Now,
                    Message = ex.Message
                };
            }
            finally
            {
                mgr.SubmitChanges();
            }
        }


        public static void ConfirmFpgResponse(this GenericManager<LcEntityDataContext> mgr, ModelCore.Schema.FPG.J2SP j2sp)
        {
            switch (j2sp.TABLENAME)
            {
                case "TXD2SB29":
                    mgr.ConfirmSentLc(j2sp);
                    break;
                case "TXD2SB31":
                    mgr.ConfirmSentNegoDraftStatus(j2sp);
                    break;
                case "TXD2SB40":
                    mgr.ConfirmSentNegoPromptRequest(j2sp);
                    break;
                default:
                    break;
            }
        }

        public static void ProcessFpgRequest(this GenericManager<LcEntityDataContext> mgr, ModelCore.Schema.FPG.J2SP j2sp, DataPortLog logItem, BeneficiaryServiceGroup.ServiceDefinition? serviceID)
        {
            switch (j2sp.TABLENAME)
            {
                case "TXD2SB38":
                    mgr.AcceptLcAmendment(j2sp, logItem);
                    break;

                case "TXD2SB30":
                    mgr.PromptDraftNegotiation(j2sp, logItem, serviceID);
                    break;

                case "TXD2SB32":
                    mgr.ReceiveNegoInvoice(j2sp, logItem);
                    break;

                case "TXD2SB33":
                    mgr.ReceiveInvoiceDetail(j2sp, logItem);
                    mgr.CheckDraftInvoice();
                    break;

                case "TXD2SB39":
                    var items = mgr.ReviewFpgDraftNegotiation(j2sp, logItem);
                    mgr.SendNegotiationPromptRequest(items, serviceID);
                    break;
                default:
                    break;
            }
        }

        public static void LogException(this GenericManager<LcEntityDataContext> mgr, Exception ex, int logID)
        {
            //ModelCore.Helper.Logger.Error(ex, ModelCore.Helper.Logger.LogLevel.Error);
            using(GenericManager<LcEntityDataContext> models = new GenericManager<LcEntityDataContext>())
            {
                models.GetTable<DataProcessLog>().InsertOnSubmit(new DataProcessLog
                {
                    LogID = logID,
                    ProcessDate = DateTime.Now,
                    ExceptionLog = new ExceptionLog
                    {
                        LogTime = DateTime.Now,
                        Message = ex.Message
                    }
                });

                models.SubmitChanges();

            }
        }

        public static void LogMessage(this GenericManager<LcEntityDataContext> mgr, String message, int logID)
        {
            mgr.GetTable<DataProcessLog>().InsertOnSubmit(new DataProcessLog
            {
                LogID = logID,
                ProcessDate = DateTime.Now,
                ExceptionLog = new ExceptionLog
                {
                    LogTime = DateTime.Now,
                    Message = message
                }
            });

            mgr.SubmitChanges();
        }

        public static bool FindLc(this GenericManager<LcEntityDataContext> mgr, String lcNo, String lcSqNo, out LetterOfCredit lcItem, out AmendingLcInformation amendmentInfo)
        {
            lcItem = mgr.GetTable<LetterOfCredit>().Where(l => l.LcNo == lcNo).FirstOrDefault();
            amendmentInfo = null;

            if (lcItem != null)
            {
                int verNo = 0;
                if (!String.IsNullOrEmpty(lcSqNo) && int.TryParse(lcSqNo, out verNo))
                {
                    verNo--;
                }

                if (verNo > 0)
                {
                    int lcID = lcItem.LcID;
                    amendmentInfo = mgr.GetTable<AmendingLcInformation>()
                        .Join(mgr.GetTable<AmendingLcApplication>().Where(a => a.LcID == lcID), 
                            i => i.AmendingID, a => a.AmendingID, (i, a) => i)
                        .Where(i => i.LcVersionNo == verNo).FirstOrDefault();
                    if (amendmentInfo != null)
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        public static bool FindDraft(this GenericManager<LcEntityDataContext> mgr, String lcNo, String lcSqNo, String sqNo, out LetterOfCredit lcItem, out AmendingLcInformation amendmentInfo, out NegoDraft draft)
        {
            draft = null;
            if (mgr.FindLc(lcNo, lcSqNo, out lcItem, out amendmentInfo))
            {
                if (amendmentInfo != null)
                {
                    draft = amendmentInfo.NegoDraft.Where(d => d.DraftNo == sqNo)
                        .OrderByDescending(d => d.DraftID).FirstOrDefault();
                }
                else
                {
                    draft = lcItem.NegoDraft.Where(d => d.DraftNo == sqNo)
                        .OrderByDescending(d => d.DraftID).FirstOrDefault();
                }
            }
            return draft != null;
        }


        public static void ConfirmFpgNegoDraftReject(this GenericManager<LcEntityDataContext> mgr,UserProfile userProfile)
        {
            foreach (var item in userProfile.GetEffectiveNegoDraft(mgr)
                .Join(mgr.GetTable<FpgNegoDraft>()
                    , n => n.DraftID, f => f.DraftID, (n, f) => n)
                .Join(mgr.GetTable<Documentary>().Where(d => d.CurrentLevel == (int)Naming.DocumentLevel.拒絕押匯_自動退回)
                    , n => n.DraftID, d => d.DocID, (n, d) => n))
            {
                item.Documentary.CurrentLevel = (int)Naming.DocumentLevel.銀行已拒絕;
            }
            mgr.SubmitChanges();
        }

        public static void AnnotateFpgNegoAsRemitted(this GenericManager<LcEntityDataContext> mgr,UserProfile userProfile, IEnumerable<int> draftID)
        {
            var remittance = mgr.GetTable<FpgNegoRemittance>();

            foreach (var id in draftID)
            {
                var item = remittance.Where(l => l.DraftID == id).FirstOrDefault();
                if (item != null)
                {
                    item.Status = (int)Naming.RemittanceStatusDefinition.匯款已完成;
                    item.FpgNegoDraft.NegoDraft.Documentary.DoApprove(Naming.DocumentLevel.押匯申請已轉帳, userProfile.UserProfileRow.pid, null);
                    item.FpgNegoDraft.NegoDraft.IntentToDispatch();
                    mgr.SubmitChanges();

                    mgr.ExecuteCommand("UPDATE FpgNegoRemittanceLog SET Status = 5 WHERE DraftID = {0}", item.DraftID);
                }
            }

        }

        public static void AnnotateFpgNegoAsReturned(this GenericManager<LcEntityDataContext> mgr, UserProfile userProfile, IEnumerable<int> draftID)
        {
            var remittance = mgr.GetTable<FpgNegoRemittance>();

            foreach (var id in draftID)
            {
                var item = remittance.Where(l => l.DraftID == id).FirstOrDefault();
                if (item != null)
                {
                    item.Status = (int)Naming.RemittanceStatusDefinition.匯款退回;
                    item.FpgNegoDraft.NegoDraft.Documentary.DoDeny(Naming.DocumentLevel.已開立, userProfile.UserProfileRow.pid, "註記押匯款退回");
                    mgr.SubmitChanges();
                }
            }

        }
    }
}
