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
    public class Txn_R3801 : EAITransaction<R3801_Rq.IFX, R3801_Rs.IFX>
	{

		public Txn_R3801()
            : this(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "R3801_Rq.xml"))
        {
			//
			// TODO: 在此加入建構函式的程式碼
			//
		}

        public Txn_R3801(String rqPath)
            : base("R3801")
        {
            //
            // TODO: 在此加入建構函式的程式碼
            //
            XmlDocument doc = new XmlDocument();
            doc.Load(rqPath);
            _rq = doc.ConvertTo<R3801_Rq.IFX>();
            
            AutoRetry = true;
        }

        public override bool Commit()
        {
            if (DateTime.Now.TimeOfDay > Settings.Default.R3801TimeUp || DateTime.Now.TimeOfDay < Settings.Default.EAIRetryStartAt)
            {
                PutInRetrial();
                return false;
            }
            return base.Commit();
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

	}
}
