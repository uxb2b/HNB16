using System;
using System.Xml;
using System.Data;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>
	/// Txn_P1002 ���K�n�y�z�C
	/// </summary>
	public class Txn_P1002 : EAITransaction<P1002_Rq.IFX,P1002_Rs.IFX>
	{

        public Txn_P1002()
            : base("P1002")
        {
			//
			// TODO: �b���[�J�غc�禡���{���X
			//
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "P1002_Rq.xml"));
            _rq = doc.ConvertTo<P1002_Rq.IFX>();
            
            _rq.LogTxn.PrcDt = String.Format("{0:yyyy-MM-dd}", DateTime.Today);

            _rq.EAIBody.MsgRq.SvcRq.ACFLG = "0";

		}
	

	}
}
