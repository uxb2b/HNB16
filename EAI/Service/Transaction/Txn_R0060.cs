using System;
using System.Xml;
using System.Data;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>
	/// Txn_R0060 的摘要描述。
	/// </summary>
	public class Txn_R0060 : EAITransaction<R0060_Rq.IFX,R0060_Rs.IFX>
	{

        public Txn_R0060()
            : base("R0060")
        {
			//
			// TODO: 在此加入建構函式的程式碼
			//
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "R0060_Rq.xml"));
            _rq = doc.ConvertTo<R0060_Rq.IFX>();
            
            _rq.LogTxn.PrcDt = String.Format("{0:yyyy-MM-dd}", DateTime.Today);

            _rq.EAIBody.MsgRq.SvcRq.ACFLG = "0";

		}

	}
}
