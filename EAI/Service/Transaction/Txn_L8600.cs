using System;
using System.Xml;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>Txn_L8600 的摘要描述。</summary>
	public class Txn_L8600 : EAITransaction<L8600_Rq.IFX,L8600_Rs.IFX>
	{
		/// <summary>建構函式</summary>
		public Txn_L8600()
            : base("L8600")
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "L8600_Rq.xml"));
            _rq = doc.ConvertTo<L8600_Rq.IFX>();
            
            _rq.EAIBody.MsgRq.SvcRq.ACFLG = "0";
            _rq.EAIBody.MsgRq.SvcRq.SEQFG = "1";
		}

		#region codegen
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
		public string VINT
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.VINT;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.VINT = value;
			}
		}
		public string DFCD
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.DFCD;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.DFCD = value;
			}
		}
		public string DFAMT
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.DFAMT;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.DFAMT = value;
			}
		}
		public string SVEFEE
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.SVEFEE;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.SVEFEE = value;
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
		public string PAYACTNO
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.PAYACTNO;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.PAYACTNO = value;
			}
		}
		public string CLSNOTE
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.CLSNOTE;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.CLSNOTE = value;
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
		public string VDATE
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.VDATE;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.VDATE = value;
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
		public string COUNT
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.COUNT;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.COUNT = value;
			}
		}
		public string RECAMT
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.RECAMT;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.RECAMT = value;
			}
		}
		public string DFIRTKD
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.DFIRTKD;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.DFIRTKD = value;
			}
		}
		public string DFRATECD
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.DFRATECD;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.DFRATECD = value;
			}
		}
		public string DFIRTCD
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.DFIRTCD;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.DFIRTCD = value;
			}
		}
		public string DFFITIRT
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.DFFITIRT;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.DFFITIRT = value;
			}
		}
		public string DFIXPR
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.DFIXPR;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.DFIXPR = value;
			}
		}
		#endregion

		#region L8600_Rs 成員

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
		public string IEDATE
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.IEDATE;
			}
		}
		public string SINTAMT
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.SINTAMT;
			}
		}
		public string Rs_VINT
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.VINT;
			}
		}
		public string PAYAMT
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.PAYAMT;
			}
		}
		#endregion

		#endregion



	}
}