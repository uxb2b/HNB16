using System;
using System.Xml;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>Txn_L4600 的摘要描述。</summary>
    public class Txn_L4600 : EAITransaction<L4600_Rq.IFX, L4600_Rs.IFX>
	{
		/// <summary>建構函式</summary>
		public Txn_L4600()
            : base("L4600")
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "L4600_Rq.xml"));
            _rq = doc.ConvertTo<L4600_Rq.IFX>();
            
            _rq.EAIBody.MsgRq.SvcRq.ACFLG = "0";
		}

		#region L4600_Rs 成員

		#region codegen by Macro
		public string BRNO
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.BRNO;
			}
		}
		public string TRMSEQ
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.TRMSEQ;
			}
		}
		public string TXTNO
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.TXTNO;
			}
		}
		public string BRNAME
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.BRNAME;
			}
		}
		public string ACBRNAME
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.ACBRNAME;
			}
		}
		public string CADDR
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.CADDR;
			}
		}
		public string SUPCNM
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.SUPCNM;
			}
		}
		#endregion

		#endregion




	}
}

