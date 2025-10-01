using System;
using System.Xml;
using System.Data;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>
	/// Txn_L3700 的摘要描述。
	/// </summary>
    public class Txn_L3700 : EAITransaction<L3700_Rq.IFX, L3700_Rs.IFX>
	{

		public Txn_L3700()
            : base("L3700")
        {
			//
			// TODO: 在此加入建構函式的程式碼
			//
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "L3700_Rq.xml"));
            _rq = doc.ConvertTo<L3700_Rq.IFX>();
            
            _rq.EAIBody.MsgRq.SvcRq.ACFLG = "0";
            _rq.EAIBody.MsgRq.SvcRq.SEQFG = "1";

		}

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
		public string HCODE
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.HCODE;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.HCODE = value;
			}
		}
		public string ECKIN
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.ECKIN;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.ECKIN = value;
			}
		}
		public string ECTRM
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.ECTRM;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.ECTRM = value;
			}
		}
		public string ECTNO
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.ECTNO;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.ECTNO = value;
			}
		}
		public string ACTNO
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.ACTNO;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.ACTNO = value;
			}
		}
		public string CIFKEY
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.CIFKEY;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.CIFKEY = value;
			}
		}
		public string CIFERR
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.CIFERR;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.CIFERR = value;
			}
		}
		public string ITEM
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.ITEM;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.ITEM = value;
			}
		}
		public string SQAMT
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.SQAMT;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.SQAMT = value;
			}
		}
		public string DATE
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.DATE;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.DATE = value;
			}
		}
		public string CAUSE
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.CAUSE;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.CAUSE = value;
			}
		}
		public string RTDATE
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.RTDATE;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.RTDATE = value;
			}
		}
		public string RTCAUSE
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.RTCAUSE;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.RTCAUSE = value;
			}
		}
		public string GRAMT
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.GRAMT;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.GRAMT = value;
			}
		}
		public string CHRATE
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.CHRATE;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.CHRATE = value;
			}
		}
		public string FEE
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.FEE;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.FEE = value;
			}
		}
		public string HCFEE
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.HCFEE;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.HCFEE = value;
			}
		}
		public string TXTYPE
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.TXTYPE;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.TXTYPE = value;
			}
		}
		public string PACTNO
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.PACTNO;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.PACTNO = value;
			}
		}
		public string FSQAMT
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.FSQAMT;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.FSQAMT = value;
			}
		}
		public string FCURCD
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.FCURCD;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.FCURCD = value;
			}
		}
		public string EXRATE
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.EXRATE;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.EXRATE = value;
			}
		}
		public string CKAMT
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.CKAMT;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.CKAMT = value;
			}
		}
		public string CKNO
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.CKNO;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.CKNO = value;
			}
		}
		public string CKBKNO7
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.CKBKNO7;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.CKBKNO7 = value;
			}
		}
		public string CKACTNO
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.CKACTNO;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.CKACTNO = value;
			}
		}

		public string RVSYEAR
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.RVSYEAR;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.RVSYEAR = value;
			}
		}

		public string RVSSRNO
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.RVSSRNO;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.RVSSRNO = value;
			}
		}

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


	}
}
