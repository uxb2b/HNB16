using System;
using System.Xml;
using System.Data;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>
	/// Txn_L0105 的摘要描述。
	/// </summary>
	public class Txn_L0105 : EAITransaction<L0105_Rq.IFX,L0105_Rs.IFX>
	{

        public Txn_L0105()
            : base("L0105")
        {
			//
			// TODO: 在此加入建構函式的程式碼
			//
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "L0105_Rq.xml"));
            _rq = doc.ConvertTo<L0105_Rq.IFX>();
            
            _rq.LogTxn.PrcDt = String.Format("{0:yyyy-MM-dd}", DateTime.Today);

            initializeData();

		}

        private void initializeData()
        {
            _rq.EAIBody.MsgRq.SvcRq.ACTNO = "";
            _rq.EAIBody.MsgRq.SvcRq.IBRNO = "";
            _rq.EAIBody.MsgRq.SvcRq.BALFG = "1";
            _rq.EAIBody.MsgRq.SvcRq.CIFERR = "";
            _rq.EAIBody.MsgRq.SvcRq.CIFKEY = "";
        }

	

	}
}
