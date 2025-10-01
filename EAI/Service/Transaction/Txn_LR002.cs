using System;
using System.Xml;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>
	/// Txn_LR002 的摘要描述。
	/// </summary>
    public class Txn_LR002 : EAITransaction<LR002_Rq.IFX, LR002_Rs.IFX>
	{

		public Txn_LR002()
            : base("LR002")
        {
			//
			// TODO: 在此加入建構函式的程式碼
			//
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "LR002_Rq.xml"));
            _rq = doc.ConvertTo<LR002_Rq.IFX>();
            
            _rq.EAIBody.MsgRq.SvcRq.ACFLG = "0";
        }


	}
}

