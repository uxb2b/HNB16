using System;
using System.Xml;
using System.Data;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>
	/// Txn_F0390 的摘要描述。
	/// </summary>
    public class Txn_F0390 : EAITransaction<F0390_Rq.IFX, F0390_Rs.IFX>
	{

        public Txn_F0390()
            : base("F0390")
        {
			//
			// TODO: 在此加入建構函式的程式碼
			//
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "F0390_Rq.xml"));
            _rq = doc.ConvertTo<F0390_Rq.IFX>();
            
            _rq.LogTxn.PrcDt = String.Format("{0:yyyy-MM-dd}", DateTime.Today);

            initializeData();

		}

        private void initializeData()
        {
            _rq.EAIBody.MsgRq.SvcRq.CIFERR = "";
            _rq.EAIBody.MsgRq.SvcRq.CIFKEY = "";
            _rq.EAIBody.MsgRq.SvcRq.NAL = "TW";
        }


    }
}
