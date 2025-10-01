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
using EAI.Service.Transaction.BR758;
using CommonLib.Core.Utility;
using System.Text;
using EAI.Helper;
using System.Linq;
using CommonLib.Utility;

namespace EAI.Service
{
	/// <summary>
	/// Summary description for TransactionSvc.
	/// </summary>
    public class BR758Service
    {
        private static BR758Service _instance = new BR758Service();

        private BR758Service()
        {
            //
            // TODO: Add constructor logic here
            //
            //			System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy();
            System.Net.ServicePointManager.ServerCertificateValidationCallback =
                (sender, certificate, chain, sslPolicyErrors) => { return true; };

            //MQEnvironment.addConnectionPoolToken();

            MQEnvironment.properties[MQC.TRANSPORT_PROPERTY] = MQC.TRANSPORT_MQSERIES_CLIENT;

        }


        public static XmlDocument GetData()
        {
            return _instance.doInvokeOutboundEAI();
        }

        public static IFX GetBR758()
        {
            var doc = _instance.doInvokeOutboundEAI();
            if (doc != null)
            {
                var ifx = doc.ConvertTo<IFX>();
                return ifx;
            }

            return null;
        }

        private static readonly String[] __RejectReason = {
            "03:帳號錯誤",
            "04:帳號與戶名不符",
            "05:疑似制裁名單確認為是",
            "06:疑似制裁名單更正",
            "07:疑似制裁名單確認為否",
            "99:其他原因"
        };
        public static String GetRejectReason(IFX eaiBR758)
        {
            var code = eaiBR758.EAIBody?.MsgRq?.SvcRq?.REJCD;
            return code != null
                ? (__RejectReason.Where(r => r.StartsWith(code)).FirstOrDefault() ?? $"代碼:{code}")
                : null;
        }


        private XmlDocument doInvokeOutboundEAI()
        {
            XmlDocument doc = null;

            if (Settings.Default.UseEAI)
            {

                bool check = false;
                String line = "";

                try
                {
                    MQQueueManager qMgr = TransactionSvc.InvokeQueueManager(
                        Settings.Default.BR_Port,
                        Settings.Default.BR_Host,
                        Settings.Default.BR_QChannelName,
                        Settings.Default.BR_QManagerName);

                    check = qMgr.OpenStatus;
                    if (check)
                    {
                        try
                        {

                            MQQueue resQ = qMgr.AccessQueue(Settings.Default.BR_RCVQName, MQC.MQOO_INPUT_AS_Q_DEF | MQC.MQOO_FAIL_IF_QUIESCING | MQC.MQOO_OUTPUT);

                            MQMessage responseMessage = new MQMessage();
                            responseMessage.CharacterSet = 1208;
                            //responseMessage.MessageId = requestMessage.MessageId;     // response correlation id has to match
                            MQGetMessageOptions gmo = new MQGetMessageOptions();
                            gmo.Options = MQC.MQGMO_WAIT;  //| MQC.MQGMO_CONVERT;         // tell get to wait 
                            gmo.WaitInterval = 5 * 1000;                             // tell get to wait up to 5 seconds
                            gmo.MatchOptions = MQC.MQMO_NONE; //MQC.MQMO_MATCH_MSG_ID;
                            resQ.Get(responseMessage, gmo);  // get response message
                            line = responseMessage.ReadString(responseMessage.MessageLength);
                            doc = new XmlDocument();
                            doc.LoadXml(line.RegularizeXmlString());
                            resQ.Close();
                            qMgr.Disconnect();

                            Logger.Info(new EAILog { EAIDoc = doc, TxnID = "BR758", IsRs = true });

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

            }
            else
            {
                return null;
            }

            return doc;
        }

    }

}

