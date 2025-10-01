using System;
using System.Xml;
using System.Data;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>
	/// Txn_A1000 ���K�n�y�z�C
	/// </summary>
	public class Txn_A1000 : EAITransaction<A1000_Rq.IFX,A1000_Rs.IFX>
	{

        public Txn_A1000()
            : base("A1000")
        {
			//
			// TODO: �b���[�J�غc�禡���{���X
			//
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "A1000_Rq.xml"));
            _rq = doc.ConvertTo<A1000_Rq.IFX>();
            
            _rq.LogTxn.PrcDt = String.Format("{0:yyyy-MM-dd}", DateTime.Today);

		}
	

	}
}
