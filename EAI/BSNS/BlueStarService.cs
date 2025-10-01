using System;
using System.Data;
using System.Threading;
using System.IO;
using System.Net;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using System.Collections.Generic;

using EAI.Properties;
using IBM.WMQ;
using EAI.Service.Transaction;
using CommonLib.Core.Utility;
using System.Text;
using EAI.Helper;

namespace EAI.BSNS
{
	/// <summary>
	/// Summary description for TransactionSvc.
	/// </summary>
    public class BlueStarService
    {
        private static BlueStarService _instance = new BlueStarService();

        private BlueStarService()
        {
            //
            // TODO: Add constructor logic here
            //
            //			System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy();
            System.Net.ServicePointManager.ServerCertificateValidationCallback =
                (sender, certificate, chain, sslPolicyErrors) => { return true; };

            MQEnvironment.properties[MQC.TRANSPORT_PROPERTY] = MQC.TRANSPORT_MQSERIES_CLIENT;

        }

        public static void StartUp()
        {
            //
        }

        public static XmlDocument InvokeOutboundEAI(XmlDocument docRq)
        {
            return Settings.Default.BS_UseEAIProxy ? _instance.doInvokeEAIProxy(docRq) : _instance.doInvokeOutboundEAI(docRq);
        }

        private XmlDocument doInvokeEAIProxy(XmlDocument docRq)
        {
            XmlDocument doc = new XmlDocument();

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Settings.Default.BS_EAIProxy);
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
            }
            finally
            {
                Logger.Info(new EAILog { EAIDoc = docRq, TxnID = "BS" });
                Logger.Info(new EAILog { EAIDoc = doc, TxnID = "BS", IsRs = true });
            }

            return doc;
        }

        private XmlDocument doInvokeOutboundEAI(XmlDocument docRq)
        {
            XmlDocument doc = new XmlDocument();


            bool check = false;
            String line = "";
            MQQueue reqQ;

            byte[] data;
            byte[] messageId;
            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(ms, new UTF8Encoding()))
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

                MQQueueManager qMgr = EAI.Service.TransactionSvc.InvokeQueueManager(
                        Settings.Default.BS_Port,
                        Settings.Default.BS_Host,
                        Settings.Default.BS_QChannelName,
                        Settings.Default.BS_QManagerName);

                check = qMgr.OpenStatus;
                if (check)
                {
                    try
                    {
                        reqQ = qMgr.AccessQueue(Settings.Default.BS_SendQName, MQC.MQOO_OUTPUT | MQC.MQOO_FAIL_IF_QUIESCING | MQC.MQOO_SET_IDENTITY_CONTEXT);

                        MQMessage requestMessage = new MQMessage();

                        requestMessage.CharacterSet = 1208;
                        //messageId = System.Guid.NewGuid().ToByteArray();
                        //Logger.Info(String.Join("", messageId.Select(c => String.Format("{0:X}", c))));
                        //requestMessage.MessageId = messageId;
                        requestMessage.MessageId = MQC.MQMI_NONE;
                        requestMessage.Write(data);
                        requestMessage.Format = MQC.MQFMT_STRING;
                        //requestMessage.MessageType = MQC.MQMT_REQUEST;
                        //requestMessage.ReplyToQueueName = this._rcvQName;

                        reqQ.Put(requestMessage, new MQPutMessageOptions());                             // put request message
                        messageId = requestMessage.MessageId;
                        reqQ.Close();
                        //qMgr.Disconnect();

                        //qMgr = new MQQueueManager(this._qManagerName);
                        MQQueue resQ = qMgr.AccessQueue(Settings.Default.BS_RCVQName, MQC.MQOO_INPUT_AS_Q_DEF | MQC.MQOO_FAIL_IF_QUIESCING | MQC.MQOO_OUTPUT);

                        MQMessage responseMessage = new MQMessage();
                        responseMessage.ClearMessage();
                        responseMessage.CharacterSet = 1208;
                        responseMessage.MessageId = messageId;     // response correlation id has to match
                        MQGetMessageOptions gmo = new MQGetMessageOptions();
                        gmo.Options = MQC.MQGMO_WAIT;// | MQC.MQGMO_CONVERT;         // tell get to wait 
                        gmo.WaitInterval = 60 * 1000;                             // tell get to wait up to 60 seconds
                        //gmo.MatchOptions = MQC.MQMO_MATCH_MSG_ID;
                        resQ.Get(responseMessage, gmo);  // get response message
                        line = responseMessage.ReadString(responseMessage.MessageLength);
                        doc.LoadXml(line);
                        resQ.Close();
                        qMgr.Disconnect();
                    }
                    catch (MQException ex)
                    {
                        Logger.Error(ex);
                        qMgr.Disconnect();
                    }

                }
                else
                {
                    qMgr.Disconnect();
                }


            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            finally
            {
                Logger.Info(new EAILog { EAIDoc = docRq, TxnID = "BS" });
                Logger.Info(new EAILog { EAIDoc = doc, TxnID = "BS", IsRs = true });
            }

            return doc;
        }

    }
}
