using System;
using System.Xml;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>
	/// Txn_L3400 的摘要描述。
	/// </summary>
    public class Txn_L3400 : EAITransaction<L3400_Rq.IFX, L3400_Rs.IFX>
	{

		public Txn_L3400()
            : base("L3400")
        {
			//
			// TODO: 在此加入建構函式的程式碼
			//
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "L3400_Rq.xml"));
            _rq = doc.ConvertTo<L3400_Rq.IFX>();
            
            _rq.EAIBody.MsgRq.SvcRq.ACFLG = "0";
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
		public string HCDE
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.HCDE;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.HCDE = value;
			}
		}
		public string BRNO
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.BRNO;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.BRNO = value;
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
		public string BCIFKEY
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.BCIFKEY;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.BCIFKEY = value;
			}
		}
		public string BCIFERR
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.BCIFERR;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.BCIFERR = value;
			}
		}
		public string BNAME
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.BNAME;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.BNAME = value;
			}
		}
		public string BADDR
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.BADDR;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.BADDR = value;
			}
		}
		public string EMAIL
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.EMAIL;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.EMAIL = value;
			}
		}

		public string BRNO_Rs
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

	}
}