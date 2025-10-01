using System;
using System.Xml;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>
	/// Txn_LR017 ���K�n�y�z�C
	/// </summary>
    public class Txn_LR004 : EAITransaction<LR004_Rq.IFX, LR004_Rs.IFX>
	{

        public Txn_LR004()
            : base("LR004")
		{
			//
			// TODO: �b���[�J�غc�禡���{���X
			//
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "LR004_Rq.xml"));
            _rq = doc.ConvertTo<LR004_Rq.IFX>();
            
            _rq.EAIBody.MsgRq.SvcRq.ACFLG = "0";


		}

		public string Rbrno
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.RBRNO;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.RBRNO = value;
			}
		}

		public string Mgeno
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.MGENO;
			}
		}

		public string Speno
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.SPENO;
			}
		}


	}
}