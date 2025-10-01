using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;

using ModelCore.DataModel;
using ModelCore.Helper;
using ModelCore.LcManagement;
using ModelCore.Locale;
using ModelCore.Properties;
using CommonLib.Utility;
using CommonLib.Security.UseCrypto;
using EAI.Helper;
using EAI.Service;

namespace ModelCore.Service.FPG
{
    public static partial class FpgAgent
    {
        private static int __CountToConfirm = 0;
        private static int __CountToService = 0;
        private static int __CountToResponse = 0;
        private static int __CountToSendLc = 0;
        private static int __CountToSendDraftStatus = 0;
        private static int __CountToSendNegoPromptRequest = 0;
        private static int __CountToSendNegoRemittance = 0;
        private static int __CountToCheckNegoRemittance = 0;

        static FpgAgent()
        {
            CreateCryptoInstance = () =>
                {
                    return new CryptoUtility() { VerifySignatureOnly = true };
                };
        }

        public static void StartUp()
        {

        }

        public static Func<CryptoUtility> CreateCryptoInstance
        {
            get;
            set;
        }


        public static void SendLc()
        {
            if (Interlocked.Increment(ref __CountToSendLc) == 1)
            {
                ThreadPool.QueueUserWorkItem(stateInfo =>
                {
                    do
                    {
                        try
                        {
                            doSendLc();
                        }
                        catch (Exception ex)
                        {
                            ModelCore.Helper.Logger.Error(ex, ModelCore.Helper.Logger.LogLevel.Error);
                        }
                    } while (Interlocked.Decrement(ref __CountToSendLc) > 0);
                });
            }
        }

        public static void SendDraftProcessStatus()
        {
            if (Interlocked.Increment(ref __CountToSendDraftStatus) == 1)
            {
                ThreadPool.QueueUserWorkItem(stateInfo =>
                {
                    do
                    {
                        try
                        {
                            doSendNegoDraftStatus();
                        }
                        catch (Exception ex)
                        {
                            ModelCore.Helper.Logger.Error(ex, ModelCore.Helper.Logger.LogLevel.Error);
                        }
                    } while (Interlocked.Decrement(ref __CountToSendDraftStatus) > 0);
                });
            }
        }

        public static void SendNegoPromptRequest()
        {
            if (Interlocked.Increment(ref __CountToSendNegoPromptRequest) == 1)
            {
                ThreadPool.QueueUserWorkItem(stateInfo =>
                {
                    do
                    {
                        try
                        {
                            doSendNegoPromptRequest();
                        }
                        catch (Exception ex)
                        {
                            ModelCore.Helper.Logger.Error(ex, ModelCore.Helper.Logger.LogLevel.Error);
                        }
                    } while (Interlocked.Decrement(ref __CountToSendNegoPromptRequest) > 0);
                });
            }
        }

        public static void SendNegoRemittance()
        {
            if (Interlocked.Increment(ref __CountToSendNegoRemittance) == 1)
            {
                ThreadPool.QueueUserWorkItem(stateInfo =>
                {
                    do
                    {
                        try
                        {
                            doSendNegoRemittance();
                        }
                        catch (Exception ex)
                        {
                            ModelCore.Helper.Logger.Error(ex, ModelCore.Helper.Logger.LogLevel.Error);
                        }
                    } while (Interlocked.Decrement(ref __CountToSendNegoRemittance) > 0);

                    CheckNegoRemittance();

                });
            }
        }

        public static void CheckNegoRemittance()
        {
            if (Interlocked.Increment(ref __CountToCheckNegoRemittance) == 1)
            {
                ThreadPool.QueueUserWorkItem(stateInfo =>
                {
                    do
                    {
                        try
                        {
                            doCheckBR758();
                        }
                        catch (Exception ex)
                        {
                            ModelCore.Helper.Logger.Error(ex, ModelCore.Helper.Logger.LogLevel.Error);
                        }
                    } while (Interlocked.Decrement(ref __CountToCheckNegoRemittance) > 0);
                });
            }
        }

        public static void RespondToConfirmation()
        {
            if (Interlocked.Increment(ref __CountToConfirm) == 1)
            {
                ThreadPool.QueueUserWorkItem(stateInfo =>
                {
                    do
                    {
                        try
                        {
                            doConfirm();
                        }
                        catch (Exception ex)
                        {
                            ModelCore.Helper.Logger.Error(ex, ModelCore.Helper.Logger.LogLevel.Error);
                        }
                    } while (Interlocked.Decrement(ref __CountToConfirm) > 0);
                });
            }
        }

        public static void ProcessServiceRequest()
        {
            if (Interlocked.Increment(ref __CountToService) == 1)
            {
                ThreadPool.QueueUserWorkItem(stateInfo =>
                {
                    do
                    {
                        try
                        {
                            doService();
                        }
                        catch (Exception ex)
                        {
                            ModelCore.Helper.Logger.Error(ex, ModelCore.Helper.Logger.LogLevel.Error);
                        }

                    } while (Interlocked.Decrement(ref __CountToService) > 0);

                    CommitServiceRequest();
                });
            }
        }

        public static void CommitServiceRequest()
        {
            if (Interlocked.Increment(ref __CountToResponse) == 1)
            {
                ThreadPool.QueueUserWorkItem(stateInfo =>
                {
                    do
                    {
                        try
                        {
                            doCommit();
                        }
                        catch (Exception ex)
                        {
                            ModelCore.Helper.Logger.Error(ex, ModelCore.Helper.Logger.LogLevel.Error);
                        }
                    } while (Interlocked.Decrement(ref __CountToResponse) > 0);
                });
            }
        }

        private static void doSendLc()
        {
            DateTime now = DateTime.Now;

            ModelCore.Schema.FPG.J2SP j2sp = new ModelCore.Schema.FPG.J2SP
            {
                BkId = "009",
                FILENAME = String.Format("{0:yyyyMMddHHmmssfff}009_J2SP_TXD2SB29.xml", now),
                FLAG = 1,
                FLAGSpecified = true,
                TABLENAME = "TXD2SB29",
                ITEM = new ModelCore.Schema.FPG.J2SPITEM
                {
                    id = "sign"
                }
            };

            using (LcManager mgr = new LcManager())
            {
                List<ModelCore.Schema.FPG.J2SPITEMTXD2SB29> fpgItems = new List<ModelCore.Schema.FPG.J2SPITEMTXD2SB29>();
                int idx = 0;
                foreach(var service in mgr.GetTable<BeneficiaryServiceGroup>().ToList())
                {
                    fpgItems.Clear();
                    j2sp.FILENAME = String.Format("{0:yyyyMMddHHmmssfff}_{1}_009_J2SP_TXD2SB29.xml", DateTime.Now, idx++);
                    try
                    {
                        var items = mgr.GetCreditApplicationReadyToSend(service.ServiceID);
                        if (items.Count() > 0)
                        {
                            fpgItems.AddRange(items.Select(c => c.BuildTXD2SB29()).ToArray());
                        }
                        var amendingItems = mgr.GetLcAmendmentReadyToSend(service.ServiceID);
                        if (amendingItems.Count() > 0)
                        {
                            fpgItems.AddRange(amendingItems.Select(c => c.BuildTXD2SB29()).ToArray());
                        }
                        if (fpgItems.Count > 0)
                        {
                            j2sp.ITEM.TXD2SB29 = fpgItems.ToArray();
                            mgr.SendToFPG(j2sp, service.PostUrl, (BeneficiaryServiceGroup.ServiceDefinition)service.ServiceID);
                        }
                    }
                    catch(Exception ex)
                    {
                        ModelCore.Helper.Logger.Error(ex, ModelCore.Helper.Logger.LogLevel.Error);
                    }
                }
            }
        }

        private static void doSendNegoDraftStatus()
        {
            DateTime now = DateTime.Now;

            ModelCore.Schema.FPG.J2SP j2sp = new ModelCore.Schema.FPG.J2SP
            {
                BkId = "009",
                FILENAME = String.Format("{0:yyyyMMddHHmmssfff}009_J2SP_TXD2SB31.xml", now),
                FLAG = 1,
                FLAGSpecified = true,
                TABLENAME = "TXD2SB31",
                ITEM = new ModelCore.Schema.FPG.J2SPITEM
                {
                    id = "sign"
                }
            };

            using (LcManager mgr = new LcManager())
            {
                List<ModelCore.Schema.FPG.J2SPITEMTXD2SB31> fpgItems = new List<ModelCore.Schema.FPG.J2SPITEMTXD2SB31>();
                int idx = 0;
                foreach(var service in mgr.GetTable<BeneficiaryServiceGroup>())
                {
                    fpgItems.Clear();
                    j2sp.FILENAME = String.Format("{0:yyyyMMddHHmmssfff}_{1}_009_J2SP_TXD2SB31.xml", DateTime.Now, idx++);

                    var items = mgr.GetNegoDraftReadyToSend(Naming.DocumentLevel.銀行已拒絕, service.ServiceID);
                    if (items.Count() > 0)
                    {
                        fpgItems.AddRange(items.Select(c => c.BuildTXD2SB31("C")).ToArray());
                    }
                    items = mgr.GetNegoDraftReadyToSend(Naming.DocumentLevel.拒絕押匯_自動退回, service.ServiceID);
                    if (items.Count() > 0)
                    {
                        fpgItems.AddRange(items.Select(c => c.BuildTXD2SB31("C")).ToArray());
                    }
                    items = mgr.GetNegoDraftReadyToSend(Naming.DocumentLevel.已開立, service.ServiceID);
                    if (items.Count() > 0)
                    {
                        fpgItems.AddRange(items.Select(c => c.BuildTXD2SB31("D")).ToArray());
                    }
                    items = mgr.GetNegoDraftReadyToSend(Naming.DocumentLevel.押匯申請已轉帳, service.ServiceID);
                    if (items.Count() > 0)
                    {
                        fpgItems.AddRange(items.Select(c => c.BuildTXD2SB31("E")).ToArray());
                    }
                    if (fpgItems.Count > 0)
                    {
                        j2sp.ITEM.TXD2SB31 = fpgItems.ToArray();
                        mgr.SendToFPG(j2sp, service.PostUrl, (BeneficiaryServiceGroup.ServiceDefinition)service.ServiceID);
                    }

                }
            }
        }

        private static void doSendNegoPromptRequest()
        {
            using (LcManager mgr = new LcManager())
            {
                foreach (var q in mgr.GetTable<NegoPromptRequestQueue>().ToList())
                {
                    var log = q.DataPortLog;
                    try
                    {
                        if (!File.Exists(log.ContentPath))
                        {
                            mgr.ExecuteCommand("delete NegoPromptRequestQueue where LogID = {0}", q.LogID);
                            continue;
                        }

                        XmlDocument docMsg = new XmlDocument();
                        docMsg.Load(log.ContentPath);

                        using (WebClient client = new WebClient())
                        {
                            Encoding utf8 = new UTF8Encoding();
                            client.Encoding = utf8;
                            var stream = client.OpenWrite(Settings.Default.FpgPost);
                            using (StreamWriter sw = new StreamWriter(stream, utf8))
                            {
                                sw.Write(docMsg.OuterXml);
                                sw.Flush();
                            }
//                            docMsg.Save(stream);
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
                        mgr.SubmitChanges();
                    }
                }
            }
        }

        private static void doSendNegoRemittance()
        {
            using (LcManager models = new LcManager())
            {
                var items = models.GetFpgNegoRemittanceReadyToSend();

                foreach (var item in items)
                {
                    try
                    {
                        if (TransactionSvc.IsInBusiness(item.FpgNegoDraft.NegoDraft.NegoDraftExtension.NegoBranch))
                        {
                            models.CreateBatchRemittance(item);
                        }
                        else
                        {
                            item.Status = (int)Naming.RemittanceStatusDefinition.匯款退回;
                            item.FpgNegoDraft.NegoDraft.Documentary.DoDeny(Naming.DocumentLevel.已開立, Settings.Default.SystemID, $"執行匯款日({DateTime.Today:yyyy/MM/dd})分行({item.FpgNegoDraft.NegoDraft.NegoDraftExtension.NegoBranch})未營業");
                            models.SubmitChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelCore.Helper.Logger.Error(ex, ModelCore.Helper.Logger.LogLevel.Error);
                    }
                }

                doSendRemittanceDispatch(models);
            }
        }


        private static void doSendRemittanceDispatch(LcManager models)
        {
            var items = models.GetReadyToSendRemittanceEAI();

            foreach (var item in items)
            {
                try
                {
                    if(item.FpgNegoRemittance.FpgNegoDraft.NegoDraft.NegoDraftExtension.DraftType == (int)Naming.DraftType.CHIMEI)
                    {
                        models.SendA1000(item);
                    }
                    else
                    {
                        models.SendP1002(item);
                    }
                }
                catch (Exception ex)
                {
                    ModelCore.Helper.Logger.Error(ex, ModelCore.Helper.Logger.LogLevel.Error);
                }
            }
        }

        private static void doSendNegoRemittanceByB8500()
        {
            using (LcManager mgr = new LcManager())
            {
                var groups = mgr.GetTable<FpgNegoRemittance>().Where(f => f.Status == (int)Naming.RemittanceStatusDefinition.匯款資料準備中)
                    .GroupBy(f => f.FpgNegoDraft.NegoDraft.LetterOfCredit.CreditApplicationDocumentary.受益人);

                var remittanceLog = mgr.GetTable<FpgNegoRemittanceLog>();

                foreach (var g in groups)
                {
                    try
                    {
                        DataPortLog log = new DataPortLog
                        {
                            Catalog = (int)Naming.TransportCatalogDefinition.B8500FpgNegoDraft,
                            Direction = (int)Naming.TransportDirection.Outbound
                        };

                        var bene = mgr.GetTable<Organization>().Where(o => o.CompanyID == g.Key).First();
                        var items = g.ToList();

                        log.ContentPath = mgr.CreateRemittanceDataFile(bene, items);
                        int idx = 0;
                        foreach (var item in items)
                        {
                            item.Status = (int)Naming.RemittanceStatusDefinition.匯款資料已送出;
                            remittanceLog.InsertOnSubmit(new FpgNegoRemittanceLog
                            {
                                DataPortLog = log,
                                DraftID = item.DraftID,
                                SeqNo = ++idx
                            });
                        }

                        mgr.SubmitChanges();
                        mgr.SendB8500(bene, items, log);

                    }
                    catch (Exception ex)
                    {
                        ModelCore.Helper.Logger.Error(ex, ModelCore.Helper.Logger.LogLevel.Error);
                    }
                }
            }
        }

        private static void doCheckBR758()
        {
            EAI.Service.Transaction.BR758.IFX eaiBR758 = null;
            while ((eaiBR758 = BR758Service.GetBR758()) != null)
            {
                WithdrawRemittanceByBR758(eaiBR758);
            }
        }

        public static void WithdrawRemittanceByBR758(EAI.Service.Transaction.BR758.IFX eaiBR758)
        {
            using (LcManager models = new LcManager())
            {
                var PMTID = eaiBR758.EAIBody?.MsgRq?.SvcRq?.PMTID;
                var item = models.GetTable<FpgNegoRemittanceLog>().Where(r => r.DPMTID == PMTID).FirstOrDefault();
                //CommonLib.Core.Utility.Logger.Debug($"Withdrawed Remittance PMTID: {PMTID}");
                if (item != null)
                {
                    //CommonLib.Core.Utility.Logger.Debug($"Withdrawed Remittance found: {item.RemittanceID}");
                    item.Description = BR758Service.GetRejectReason(eaiBR758);
                    item.Status = (int)Naming.RemittanceStatusDefinition.匯款失敗;
                    item.FpgNegoRemittance.Status = (int)Naming.RemittanceStatusDefinition.匯款退回;
                    if (item.FpgNegoRemittanceDispatch == null)
                    {
                        item.FpgNegoRemittanceDispatch = new FpgNegoRemittanceDispatch();
                    }
                    models.SubmitChanges();
                }
            }
        }

        private static void doConfirm()
        {
            using (LcManager mgr = new LcManager())
            {
                var queue = mgr.GetTable<ReceivedDataQueue>();
                var processLog = mgr.GetTable<DataProcessLog>();
                var items = queue.Select(q => q.DataPortLog).Where(d => d.Direction == (int)Naming.TransportDirection.Inbound);

                foreach (var item in items)
                {
                    processLog.InsertOnSubmit(new DataProcessLog
                    {
                        LogID = item.LogID,
                        ProcessDate = DateTime.Now
                    });

                    queue.DeleteOnSubmit(item.ReceivedDataQueue);
                    mgr.SubmitChanges();

                    try
                    {
                        XmlDocument docMsg = new XmlDocument();
                        docMsg.Load(item.ContentPath);
                        ModelCore.Schema.FPG.J2SP j2sp = docMsg.ConvertTo<ModelCore.Schema.FPG.J2SP>();
                        item.FpgTableName = j2sp.TABLENAME;
                        item.FpgFileName = j2sp.INITIALFILE;
                        mgr.SubmitChanges();
                        mgr.ConfirmFpgResponse(j2sp);
                    }
                    catch (Exception ex)
                    {
                        ModelCore.Helper.Logger.Error(ex, ModelCore.Helper.Logger.LogLevel.Error);
                        mgr.LogException(ex, item.LogID);
                    }
                }

            }
        }

        private static void doService()
        {
            using (LcManager mgr = new LcManager())
            {
                var queue = mgr.GetTable<ServiceDataQueue>();
                var processLog = mgr.GetTable<DataProcessLog>();
                var items = queue.Select(q => q.DataPortLog)
                                .Where(d => d.Direction == (int)Naming.TransportDirection.Inbound)
                                .OrderBy(d => d.FpgTableName);

                foreach (var item in items)
                {
                    processLog.InsertOnSubmit(new DataProcessLog
                    {
                        LogID = item.LogID,
                        ProcessDate = DateTime.Now
                    });

                    BeneficiaryServiceGroup.ServiceDefinition? serviceID = (BeneficiaryServiceGroup.ServiceDefinition?)item.ServiceDataQueue.ServiceID;
                    queue.DeleteOnSubmit(item.ServiceDataQueue);
                    mgr.SubmitChanges();

                    try
                    {
                        XmlDocument docMsg = new XmlDocument();
                        docMsg.Load(item.ContentPath);
                        ModelCore.Schema.FPG.J2SP j2sp = docMsg.ConvertTo<ModelCore.Schema.FPG.J2SP>();
                        item.FpgTableName = j2sp.TABLENAME;
                        item.FpgFileName = j2sp.FILENAME;
                        item.ResponseDataQueue = new ResponseDataQueue
                        {
                            ServiceID = (int?)serviceID,
                        };
                        mgr.SubmitChanges();

                        mgr.ProcessFpgRequest(j2sp, item, serviceID);
                    }
                    catch (Exception ex)
                    {
                        ModelCore.Helper.Logger.Error(ex, ModelCore.Helper.Logger.LogLevel.Error);
                        mgr.LogException(ex, item.LogID);
                    }
                }

            }
        }

        public static void doCommit()
        {
            using (LcManager mgr = new LcManager())
            {
                var processLog = mgr.GetTable<DataProcessLog>();
                var items = mgr.GetTable<DataPortLog>().Where(d => d.ResponseDataQueue != null);
                DataPortLog item;
                while ((item = items.FirstOrDefault()) != null)
                {
                    try
                    {
                        ModelCore.Schema.FPG.J2SP j2sp = item.LoadCommitment();
                        if (j2sp != null)
                        {
                            j2sp.CommitToFPG(item.ResponseDataQueue.BeneficiaryServiceGroup?.ConfirmUrl);
                        }
                        else
                        {
                            CommonLib.Core.Utility.Logger.Warn($"DataPortLog => {item.LogID} no commitment");
                        }

                        mgr.DeleteAny<ResponseDataQueue>(r => r.LogID == item.LogID);
                    }
                    catch (Exception ex)
                    {
                        ModelCore.Helper.Logger.Error(ex, ModelCore.Helper.Logger.LogLevel.Error);
                        mgr.LogException(ex, item.LogID);
                    }
                }
            }
        }

        public static void SendToFPG(this ModelCore.Schema.FPG.J2SP item)
        {
            var docMsg = item.ConvertToXml();
            CryptoUtility.SignXmlSHA256(docMsg, null,
                null, AppSigner.SignerCertificate, "#sign");

            String fileName = Path.Combine(CommonLib.Core.Utility.Logger.LogDailyPath, item.FILENAME);
            docMsg.Save(fileName);

            ThreadPool.QueueUserWorkItem(state =>
                {
                    using (WebClient client = new WebClient())
                    {
                        Encoding utf8 = new UTF8Encoding();
                        client.Encoding = utf8;
                        var stream = client.OpenWrite(Settings.Default.FpgPost);
                        using (StreamWriter sw = new StreamWriter(stream, utf8))
                        {
                            sw.Write(docMsg.OuterXml);
                            sw.Flush();
                        }
                        //docMsg.Save(stream);
                        stream.Flush();
                        stream.Close();
                    }
                });
        }
    }
}
