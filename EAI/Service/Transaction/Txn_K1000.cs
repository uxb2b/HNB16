using System;
using System.Xml;
using System.Data;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>
	/// Txn_K1000 ���K�n�y�z�C
	/// </summary>
	public class Txn_K1000 : EAITransaction<K1000_Rq.IFX,K1000_Rs.IFX>
	{

        public Txn_K1000()
            : base("K1000")
        {
			//
			// TODO: �b���[�J�غc�禡���{���X
			//
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "K1000_Rq.xml"));
            _rq = doc.ConvertTo<K1000_Rq.IFX>();
            
            _rq.LogTxn.PrcDt = String.Format("{0:yyyy-MM-dd}", DateTime.Today);

            _rq.EAIBody.MsgRq.SvcRq.ACFLG = "0";

		}



	

	}
}
