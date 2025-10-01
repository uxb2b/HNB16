using System;
using System.Xml;
using System.Data;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>
	/// Txn_F0104 ���K�n�y�z�C
	/// </summary>
	public class Txn_F0104 : EAITransaction<F0104_Rq.IFX,F0104_Rs.IFX>
	{

        public Txn_F0104()
            : base("F0104")
        {
			//
			// TODO: �b���[�J�غc�禡���{���X
			//
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "F0104_Rq.xml"));
            _rq = doc.ConvertTo<F0104_Rq.IFX>();
            
            _rq.LogTxn.PrcDt = String.Format("{0:yyyy-MM-dd}", DateTime.Today);

            initializeData();

		}

        private void initializeData()
        {
            _rq.EAIBody.MsgRq.SvcRq.ACTNO = "";
            _rq.EAIBody.MsgRq.SvcRq.IBRNO = "9999";
            _rq.EAIBody.MsgRq.SvcRq.INQCD = "1";
            _rq.EAIBody.MsgRq.SvcRq.CIFERR = "";
            _rq.EAIBody.MsgRq.SvcRq.CIFKEY = "";
            _rq.EAIBody.MsgRq.SvcRq.IQKIND = "00";
            _rq.EAIBody.MsgRq.SvcRq.SFG = "1";
        }

	

	}
}
