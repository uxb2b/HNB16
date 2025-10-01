using System;
using System.Xml;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>
	/// Txn_L1201 的摘要描述。
	/// </summary>
    public class Txn_L1201 : EAITransaction<L1201_Rq.IFX, L1201_Rs.IFX>
	{
		public Txn_L1201()
            : base("L1201")
        {
			//
			// TODO: 在此加入建構函式的程式碼
			//
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "L1201_Rq.xml"));
            _rq = doc.ConvertTo<L1201_Rq.IFX>();
            
            _rq.EAIBody.MsgRq.SvcRq.ACFLG = "0";

		}


	}
}
