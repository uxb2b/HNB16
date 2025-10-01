using System;
using System.Xml;
using System.Data;

using EAI.Properties;
using CommonLib.Core.Utility;
using EAI.Helper;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>
	/// Txn_L4500 的摘要描述。
	/// </summary>
    public class Txn_L4500 : EAITransaction<L4500_Rq.IFX, L4500_Rs.IFX>
	{

        public Txn_L4500()
            : base("L4500")
        {
			//
			// TODO: 在此加入建構函式的程式碼
			//
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "L4500_Rq.xml"));
            _rq = doc.ConvertTo<L4500_Rq.IFX>();
            
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
		public string TXAMT
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.TXAMT;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.TXAMT = value;
			}
		}
		public string SEQFG
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.SEQFG;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.SEQFG = value;
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
		public string BPBKNO
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.BPBKNO;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.BPBKNO = value;
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
		public string OTXNO
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.OTXNO;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.OTXNO = value;
			}
		}
		public string GTXNO
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.GTXNO;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.GTXNO = value;
			}
		}
		public string VRATE
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.VRATE;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.VRATE = value;
			}
		}
		public string NEGRATE
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.NEGRATE;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.NEGRATE = value;
			}
		}
		public string NEGFEE
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.NEGFEE;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.NEGFEE = value;
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
		public string FITIRT
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.FITIRT;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.FITIRT = value;
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

		public string LNCKNO
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.LNCKNO;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.LNCKNO = value;
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

		public string COUNT
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.COUNT;
			}
		}

    }
}
