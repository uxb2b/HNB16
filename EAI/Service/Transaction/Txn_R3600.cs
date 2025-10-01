using System;
using System.Xml;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>
	/// Txn_LR001 的摘要描述。
	/// </summary>
    public class Txn_R3600 : EAITransaction<R3600_Rq.IFX, R3600_Rs.IFX>
	{

		public Txn_R3600()
            : base("R3600")
        {
			//
			// TODO: 在此加入建構函式的程式碼
			//
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "R3600_Rq.xml"));
            _rq = doc.ConvertTo<R3600_Rq.IFX>();
            

		}

		public string REMDAY
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.REMDAY;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.REMDAY = value;
			}
		}
		public string RCVBK
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.RCVBK;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.RCVBK = value;
			}
		}
		public string REMTYPE
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.REMTYPE;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.REMTYPE = value;
			}
		}
		public string CHIMSG
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.CHIMSG;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.CHIMSG = value;
			}
		}
		public string ENGMSG
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.ENGMSG;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.ENGMSG = value;
			}
		}
		public string MSG1
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.MSG1;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.MSG1 = value;
			}
		}
		public string MSG2
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.MSG2;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.MSG2 = value;
			}
		}
		public string MSG3
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.MSG3;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.MSG3 = value;
			}
		}
		public string RECVER
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.RECVER;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.RECVER = value;
			}
		}
		public string SENDER
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.SENDER;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.SENDER = value;
			}
		}
		public string PRECD
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.PRECD;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.PRECD = value;
			}
		}
		public string SLIPNO
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.SLIPNO;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.SLIPNO = value;
			}
		}
		public string FEDICD
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.FEDICD;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.FEDICD = value;
			}
		}
		public string FROMSVR
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.FROMSVR;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.FROMSVR = value;
			}
		}
		public string BHNO
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.BHNO;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.BHNO = value;
			}
		}
		public string BHSEQ
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.BHSEQ;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.BHSEQ = value;
			}
		}
		public string BHCD
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.BHCD;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.BHCD = value;
			}
		}
		public string EMPNOT
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.EMPNOT;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.EMPNOT = value;
			}
		}
		public string AUTOTRF
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.AUTOTRF;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.AUTOTRF = value;
			}
		}
		public string ENTDAY
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.ENTDAY;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.ENTDAY = value;
			}
		}
		public string RELEASE
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.RELEASE;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.RELEASE = value;
			}
		}
		public string SUPNO1
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.SUPNO1;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.SUPNO1 = value;
			}
		}
		public string RLSDAY
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.RLSDAY;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.RLSDAY = value;
			}
		}
		public string R12TIM
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.R12TIM;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.R12TIM = value;
			}
		}



	}
}
