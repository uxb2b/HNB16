using System;
using System.Xml;
using System.Data;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>
	/// Txn_L4700 的摘要描述。
	/// </summary>
	public class Txn_L4700 : EAITransaction<L4700_Rq.IFX,L4700_Rs.IFX>
	{

        public Txn_L4700()
            : base("L4700")
        {
			//
			// TODO: 在此加入建構函式的程式碼
			//
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "L4700_Rq.xml"));
            _rq = doc.ConvertTo<L4700_Rq.IFX>();
            
            _rq.LogTxn.PrcDt = String.Format("{0:yyyy-MM-dd}", DateTime.Today);

            _rq.EAIBody.MsgRq.SvcRq.ACFLG = "0";
            _rq.EAIBody.MsgRq.SvcRq.SEQFG = "1";

		}

	}
}
