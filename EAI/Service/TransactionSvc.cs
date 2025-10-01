using System;
using System.Data;
using System.Threading;
using System.IO;
using System.Net;
using System.Xml;
using System.Security.Cryptography.X509Certificates;

using EAI.Properties;
using IBM.WMQ;
using EAI.Service.Transaction;
using CommonLib.Core.Utility;
using System.Text;
using EAI.Helper;
using CommonLib.Utility;

namespace EAI.Service
{
	/// <summary>
	/// Summary description for TransactionSvc.
	/// </summary>
    public class TransactionSvc
    {
        private static TransactionSvc _instance = new TransactionSvc();
        private static Transaction.R0060_Rs.IFX __InBusiness;

        private static int __CountToRetry = 0;


        private TransactionSvc()
        {
            //
            // TODO: Add constructor logic here
            //
            //			System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy();
            System.Net.ServicePointManager.ServerCertificateValidationCallback =
                (sender, certificate, chain, sslPolicyErrors) => { return true; };

            //var mqConnMgr = new MQSimpleConnectionManager();
            //mqConnMgr.setActive(MQSimpleConnectionManager.MODE_AUTO);
            ////in pool connection keep alive 600 second
            ////mqConnMgr.setTimeout(5000);
            //mqConnMgr.setTimeout(600000);
            ////max pool connection count 512
            //mqConnMgr.setHighThreshold(512);
            ////MODE_AUTO must use addConnectionPoolToken to enable connection pool
            //MQEnvironment.addConnectionPoolToken();

            MQEnvironment.properties[MQC.TRANSPORT_PROPERTY] = MQC.TRANSPORT_MQSERIES_CLIENT;

        }

        public static void StartUp()
        {
            Action job = () => 
            {
                if (DateTime.Now.TimeOfDay >= Settings.Default.EAIRetryStartAt && DateTime.Now.TimeOfDay < Settings.Default.R3801TimeUp)
                    RetryEAI();
            };
            job.RecycleJob(30 * 60 * 1000);
        }

        public static XmlDocument InvokeOutboundEAI(string recNo, XmlDocument docRq)
        {
            return Settings.Default.UseEAIProxy ? _instance.doInvokeEAIProxy(recNo, docRq) : _instance.doInvokeOutboundEAI(recNo, docRq);
        }

        private XmlDocument doInvokeEAIProxy(string recNo, XmlDocument docRq)
        {
            XmlDocument doc = new XmlDocument();
            String pmtID = docRq.DocumentElement["LogTxn"]["PmtID"].InnerText;
            String txnID = docRq.DocumentElement["EAIHeader"]["TxnID"].InnerText;

            if (Settings.Default.UseEAI)
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Settings.Default.EAIProxy);
                    request.Method = "POST";
                    Stream output = request.GetRequestStream();
                    docRq.Save(output);
                    output.Flush();

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    long length = response.ContentLength;
                    HttpStatusCode status = response.StatusCode;
                    doc.Load(response.GetResponseStream());
                    response.Close();
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                    getErrorXML(doc, "3001", pmtID, txnID);
                }
                finally
                {
                    Logger.Info(new EAILog { EAIDoc = docRq, TxnID = recNo });
                    Logger.Info(new EAILog { EAIDoc = doc, TxnID = recNo, IsRs = true });
                }
            }
            else
            {
                getErrorXML(doc, "3001", pmtID, txnID);
            }

            return doc;
        }

        private XmlDocument doInvokeOutboundEAI(string recNo, XmlDocument docRq)
        {
            XmlDocument doc = new XmlDocument();
            String pmtID = docRq.DocumentElement["LogTxn"]["PmtID"].InnerText;
            String txnID = docRq.DocumentElement["EAIHeader"]["TxnID"].InnerText;

            if (Settings.Default.UseEAI)
            {

                bool check = false;
                String line = "";
                MQQueue reqQ;

                byte[] data;
                using (MemoryStream ms = new MemoryStream())
                {
                    using(StreamWriter sw = new StreamWriter(ms,new UTF8Encoding()))
                    {
                        docRq.Save(sw);
                        sw.Flush();
                        sw.Close();
                    }
                    ms.Flush();
                    data = ms.ToArray();
                }

                try
                {

                    MQQueueManager qMgr = InvokeQueueManager(
                        Settings.Default.port,
                        Settings.Default.host,
                        Settings.Default.channelName,
                        Settings.Default.qManagerName);

                    check = qMgr.OpenStatus;
                    if (check)
                    {
                        try
                        {
                            reqQ = qMgr.AccessQueue(Settings.Default.sendQName, MQC.MQOO_OUTPUT | MQC.MQOO_FAIL_IF_QUIESCING | MQC.MQOO_SET_IDENTITY_CONTEXT);


                            MQMessage requestMessage = new MQMessage();

                            requestMessage.CharacterSet = 1208;
                            requestMessage.MessageId = System.Guid.NewGuid().ToByteArray();
                            requestMessage.Write(data);
                            requestMessage.Format = MQC.MQFMT_STRING;
                            requestMessage.MessageType = MQC.MQMT_REQUEST;
                            requestMessage.ReplyToQueueName = Settings.Default.rcvQName;

                            reqQ.Put(requestMessage, new MQPutMessageOptions());                             // put request message
                            reqQ.Close();

                            MQQueue resQ = qMgr.AccessQueue(Settings.Default.rcvQName, MQC.MQOO_INPUT_AS_Q_DEF | MQC.MQOO_FAIL_IF_QUIESCING | MQC.MQOO_OUTPUT);

                            MQMessage responseMessage = new MQMessage();
                            responseMessage.CharacterSet = 1208;
                            responseMessage.MessageId = requestMessage.MessageId;     // response correlation id has to match
                            MQGetMessageOptions gmo = new MQGetMessageOptions();
                            gmo.Options = MQC.MQGMO_WAIT ;  //| MQC.MQGMO_CONVERT;         // tell get to wait 
                            gmo.WaitInterval = 60 * 1000;                             // tell get to wait up to 60 seconds
                            gmo.MatchOptions = MQC.MQMO_MATCH_MSG_ID;
                            resQ.Get(responseMessage, gmo);  // get response message
                            line = responseMessage.ReadString(responseMessage.MessageLength);
                            doc.LoadXml(line.RegularizeXmlString());
                            resQ.Close();
                            qMgr.Disconnect();
                        }
                        catch (MQException ex)
                        {
                            Logger.Error(ex);
                            getErrorXML(doc, "3002", pmtID, txnID);
                            qMgr.Disconnect();
                        }

                    }
                    else
                    {
                        getErrorXML(doc, "3001", pmtID, txnID);
                        qMgr.Disconnect();
                    }


                }
                catch (Exception e)
                {
                    Logger.Error(e);
                    getErrorXML(doc, "3001", pmtID, txnID);
                }
                finally
                {
                    Logger.Info(new EAILog { EAIDoc = docRq, TxnID = recNo });
                    Logger.Info(new EAILog { EAIDoc = doc, TxnID = recNo, IsRs = true });
                }
            }
            else
            {
                getErrorXML(doc, "3001", pmtID, txnID);
            }

            return doc;
        }


        public XmlDocument getErrorXML(XmlDocument doc, string status, string pmtid, string txnid, String message = null)
        {
            doc.Load(Path.Combine(Settings.Default.PhysicalTxnFilePath, "ErrorResponse.xml"));
            doc.DocumentElement["EAIHeader"]["TxnID"].InnerText = txnid;
            doc.DocumentElement["EAIHeader"]["ErrCode"].InnerText = message == null ? status : $"{status}({message})";
            doc.DocumentElement["LogTxn"]["PmtID"].InnerText = pmtid;
            return doc;
        }

        //		private XmlDocument doInvokeOutboundEAI(string recNo,DataSet packet)
        //		{
        //			
        //			XmlDocument doc = new XmlDocument();
        //            
        //			//透過http傳輸,呼叫EAI Gateway
        //
        //			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Configuration.Values["EAIProxyUrl"] + "?recNo=" + recNo);
        //			request.Timeout = 86400000;
        //			request.ContentType = "text/xml; charset=Big5";//"application/x-www-form-urlencoded";
        //				
        //			packet.DataSetName = "IFX";
        //			
        //			MemoryStream ms = new MemoryStream();
        //			XmlTextWriter xtw = new XmlTextWriter(ms,System.Text.Encoding.Default);
        //			packet.WriteXml(xtw);
        //
        //			xtw.Flush();
        //
        //			request.Method = "POST";
        //			request.ContentLength = ms.Length;
        //			Stream output = request.GetRequestStream();
        //            ms.Seek(0,SeekOrigin.Begin);
        //
        //			byte[] buf = new byte[4096];
        //			int totalRead;
        //			while((totalRead=ms.Read(buf,0,4096))>0)
        //			{
        //				output.Write(buf,0,totalRead);
        //			}
        //			
        //			ms.Close();
        //			output.Flush();
        //			output.Close();
        //
        //
        //			// 取得回文
        //			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //			long content_length = response.ContentLength;
        //			int status_code = (int)response.StatusCode;
        //
        //			doc.Load(response.GetResponseStream());
        //
        //			return doc;
        //		}

        public static void RetryEAI()
        {
            if (Interlocked.Increment(ref __CountToRetry) == 1)
            {
                ThreadPool.QueueUserWorkItem(stateInfo =>
                {
                    do
                    {
                        try
                        {
                            _instance.doRetry();
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex);
                        }
                    } while (Interlocked.Decrement(ref __CountToRetry) > 0);
                });
            }
        }

        private void doRetry()
        {
            String retrialPath = Path.Combine(Logger.LogPath,  Txn_R3801.EAI_Retry_Queue);
            String[] files;

            if (Directory.Exists(retrialPath))
            {
                files = Directory.GetFiles(retrialPath);
                if(files!=null && files.Length>0 && isTimeToRetry())
                {
                    foreach (var f in files)
                    {
                        Txn_R3801 txn = new Txn_R3801(f);
                        //if (txn.REMDAY.CompareTo("2017-04-20") < 0)
                        //{
                        //    File.Delete(f);
                        //}
                        //else 
                        if (IsInBusiness(txn.RCVBK.Substring(3)))
                        {
                            txn.REMDAY = DateTime.Now.ToString("yyyy-MM-dd");
                            txn.Commit();
                            File.Delete(f);
                        }
                    }

                    __InBusiness = null;
                }
            }
           
        }

        private bool isTimeToRetry()
        {
            if (Settings.Default.TimeToRetryR3801ByTheDay != null)
            {
                foreach (var t in Settings.Default.TimeToRetryR3801ByTheDay)
                {
                    if (t.From <= DateTime.Now.TimeOfDay && DateTime.Now.TimeOfDay <= t.To)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static void LoadBranchInBusinessInfo()
        {
            lock (typeof(TransactionSvc))
            {
                Txn_R0060 txn = new Txn_R0060();
                txn.Rq.EAIBody.MsgRq.SvcRq.KINBR = "9999";
                txn.Rq.EAIBody.MsgRq.SvcRq.BKNO = "009";
                txn.Rq.EAIBody.MsgRq.SvcRq.BOFF = "0";
                txn.Rq.EAIBody.MsgRq.SvcRq.ZIPIN = "000";

                txn.Commit();
                __InBusiness = txn.Rs;
            }
        }

        public static bool IsInBusiness(String bankCode)
        {
            if (__InBusiness == null || String.Compare(__InBusiness.LogTxn.PrcDt, DateTime.Today.ToString("yyyy-MM-dd")) < 0)
            {
                LoadBranchInBusinessInfo();
            }

            if (__InBusiness.EAIHeader.ErrCode == "0"
                && __InBusiness.EAIBody.MsgRs.Header.TxnId == "R0060"
                && __InBusiness.EAIBody.MsgRs.Header.RspCode == "R0060")
            {
                if (__InBusiness.EAIBody.MsgRs.SvcRs.Detail != null)
                {
                    foreach (var d in __InBusiness.EAIBody.MsgRs.SvcRs.Detail)
                    {
                        if (d.BRNO == bankCode)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static MQQueueManager InvokeQueueManager(int port,String hostName,String channel,String managerName)
        {
            lock(typeof(TransactionSvc))
            {
                MQEnvironment.Port = port;
                MQEnvironment.Hostname = hostName;
                MQEnvironment.Channel = channel;
                MQQueueManager qMgr = new MQQueueManager(managerName);
                return qMgr;
            }
        }


    }

	//public class TrustAllCertificatePolicy : System.Net.ICertificatePolicy
	//{
	//	public TrustAllCertificatePolicy() 
	//	{}

	//	public bool CheckValidationResult(ServicePoint sp, 
	//		X509Certificate cert,WebRequest req, int problem)
	//	{
	//		return true;
	//	}
	//}

}
