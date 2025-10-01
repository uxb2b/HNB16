using System;
using System.Xml;
using System.Data;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>
	/// Txn_P1000 的摘要描述。
	/// </summary>
	public class Txn_P1000 : EAITransaction<P1000_Rq.IFX,P1000_Rs.IFX>
	{

        public Txn_P1000()
            : base("P1000")
        {
			//
			// TODO: 在此加入建構函式的程式碼
			//
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "P1000_Rq.xml"));
            _rq = doc.ConvertTo<P1000_Rq.IFX>();
            
            _rq.LogTxn.PrcDt = String.Format("{0:yyyy-MM-dd}", DateTime.Today);

            _rq.EAIBody.MsgRq.SvcRq.ACFLG = "0";

		}
	

	}
}
