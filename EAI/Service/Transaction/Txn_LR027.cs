using System;
using System.Xml;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
    public class Txn_LR027 : EAITransaction<LR027_Rq.IFX, LR027_Rs.IFX>
	{

		//cont'r
		public Txn_LR027()
            : base("LR027")
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "LR027_Rq.xml"));
            _rq = doc.ConvertTo<LR027_Rq.IFX>();
            
            _rq.EAIBody.MsgRq.SvcRq.ACFLG = "0";

		}


		#region LR027_Rq 成員
		public string KINBR
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.KINBR;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.KINBR = value;
			}
		}
		public string ACFLG
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.ACFLG;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.ACFLG = value;
			}
		}
		public string KIND
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.KIND;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.KIND = value;
			}
		}
		public string AMT
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.AMT;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.AMT = value;
			}
		}
		public string SDAY
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.SDAY;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.SDAY = value;
			}
		}
		public string EDAY
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.EDAY;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.EDAY = value;
			}
		}
		public string RATE
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.RATE;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.RATE = value;
			}
		}
		public string IRTKD
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.IRTKD;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.IRTKD = value;
			}
		}
		public string RATECD
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.RATECD;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.RATECD = value;
			}
		}
		public string IRTCD
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.IRTCD;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.IRTCD = value;
			}
		}
		public string IXPR
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.IXPR;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.IXPR = value;
			}
		}
		public string DFDATE
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.DFDATE;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.DFDATE = value;
			}
		}

		
		#endregion

		#region LR027_Rs 成員
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
		public string VINT
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.VINT;
			}
		}
		public string DFAMT1
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.DFAMT1==null ? "NULL" : _rs.EAIBody.MsgRs.SvcRs.DFAMT1;
			}
		}
		public string DFAMT2
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.DFAMT2==null ? "NULL" : _rs.EAIBody.MsgRs.SvcRs.DFAMT2;
			}
		}

		#endregion
	}
}

