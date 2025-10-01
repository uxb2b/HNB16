using System;
using System.Xml;
using System.Data;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>
	/// Txn_B8500 的摘要描述。
	/// </summary>
    public class Txn_B8500 : EAITransaction<B8500_Rq.IFX, B8500_Rs.IFX>
	{

        public Txn_B8500()
            : base("B8500")
        {
			//
			// TODO: 在此加入建構函式的程式碼
			//
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "B8500_Rq.xml"));
            _rq = doc.ConvertTo<B8500_Rq.IFX>();
            
            _rq.LogTxn.PrcDt = String.Format("{0:yyyy-MM-dd}", DateTime.Today);

		}


	}
}
