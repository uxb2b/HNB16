using System;
using System.Xml;
using System.Data;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>
	/// Txn_F0482 ���K�n�y�z�C
	/// </summary>
    public class Txn_F0482 : EAITransaction<F0482_Rq.IFX, F0482_Rs.IFX>
	{

        public Txn_F0482()
            : base("F0482")
        {
			//
			// TODO: �b���[�J�غc�禡���{���X
			//
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "F0482_Rq.xml"));
            _rq = doc.ConvertTo<F0482_Rq.IFX>();
            
            _rq.LogTxn.PrcDt = String.Format("{0:yyyy-MM-dd}", DateTime.Today);
            _rq.EAIBody.MsgRq.SvcRq.FUNCD = "2";

            initializeData();

		}

        private void initializeData()
        {
        }


    }
}
