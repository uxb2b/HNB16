using System;
using System.Xml;
using System.Data;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>
	/// Txn_C0066 的摘要描述。
	/// </summary>
	public class Txn_C0066 : EAITransaction<C0066_Rq.IFX,C0066_Rs.IFX>
	{

        public Txn_C0066()
            : base("C0066")
        {
			//
			// TODO: 在此加入建構函式的程式碼
			//
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "C0066_Rq.xml"));
            _rq = doc.ConvertTo<C0066_Rq.IFX>();
            
            _rq.LogTxn.PrcDt = String.Format("{0:yyyy-MM-dd}", DateTime.Today);

            _rq.EAIBody.MsgRq.SvcRq.ACFLG = "0";

		}

	}
}
