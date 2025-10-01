using System;
using System.Xml;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>
	/// Txn_L1000 的摘要描述。
	/// </summary>
    public class Txn_L1000 : EAITransaction<L1000_Rq.IFX, L1000_Rs.IFX>
	{

		/// <summary>建構函式</summary>
		public Txn_L1000()
            : base("L1000")
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "L1000_Rq.xml"));
            _rq = doc.ConvertTo<L1000_Rq.IFX>();
            
            _rq.EAIBody.MsgRq.SvcRq.SEQFG = "1";
            _rq.EAIBody.MsgRq.SvcRq.ACFLG = "0";
		}




	}
}


