using System;
using System.Xml;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>
	/// Txn_LR003 的摘要描述。
	/// </summary>
    public class Txn_LR003 : EAITransaction<LR003_Rq.IFX, LR003_Rs.IFX>
	{
		public Txn_LR003()
            : base("LR003")
        {
			//
			// TODO: 在此加入建構函式的程式碼
			//
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "LR003_Rq.xml"));
            _rq = doc.ConvertTo<LR003_Rq.IFX>();
            
            _rq.EAIBody.MsgRq.SvcRq.ACFLG = "0";

		}

		#region LR003_Rq 成員
		public string GUNO
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.GUNO;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.GUNO = value;
			}
		}
		public string APSDAY
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.APSDAY;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.APSDAY = value;
			}
		}
		public string IMDAY
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.IMDAY;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.IMDAY = value;
			}
		}
		public string IMMONS
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.IMMONS;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.IMMONS = value;
			}
		}
		public string APNO
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.APNO;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.APNO = value;
			}
		}
		public string APEDAY
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.APEDAY;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.APEDAY = value;
			}
		}
		public string FLAG
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.FLAG;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.FLAG = value;
			}
		}
		public string OAPSDAY
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.OAPSDAY;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.OAPSDAY = value;
			}
		}
		public string OAPEDAY
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.OAPEDAY;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.OAPEDAY = value;
			}
		}
		public string OTXCD
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.OTXCD;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.OTXCD = value;
			}
		}

		

		
		#endregion

		#region LR003_Rs 成員
		
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
		public string APEDAY_Rs
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.APEDAY;
			}
		}
		public string IMDAYS
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.IMDAYS;
			}
		}
		public string IMMONS_Rs
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.IMMONS;
			}
		}
		public string WEEKDY
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.WEEKDY;
			}
		}




		#endregion


	}
}
