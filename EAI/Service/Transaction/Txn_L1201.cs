using System;
using System.Xml;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>
	/// Txn_L1201 ���K�n�y�z�C
	/// </summary>
    public class Txn_L1201 : EAITransaction<L1201_Rq.IFX, L1201_Rs.IFX>
	{
		public Txn_L1201()
            : base("L1201")
        {
			//
			// TODO: �b���[�J�غc�禡���{���X
			//
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "L1201_Rq.xml"));
            _rq = doc.ConvertTo<L1201_Rq.IFX>();
            
            _rq.EAIBody.MsgRq.SvcRq.ACFLG = "0";

		}


	}
}
