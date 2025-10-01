using System;
using System.Xml;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>Txn_LR028 的摘要描述。</summary>
    public class Txn_LR028 : EAITransaction<LR028_Rq.IFX, LR028_Rs.IFX>
	{

		public Txn_LR028()	//建構函式
            : base("LR028")
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "LR028_Rq.xml"));
            _rq = doc.ConvertTo<LR028_Rq.IFX>();
            
            _rq.EAIBody.MsgRq.SvcRq.ACFLG = "0";
        }

		#region request property
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
		public string INTTM
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.INTTM;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.INTTM = value;
			}
		}
		public string INTTW
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.INTTW;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.INTTW = value;
			}
		}
		public string PRTM
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.PRTM;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.PRTM = value;
			}
		}
		public string PRTW
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.PRTW;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.PRTW = value;
			}
		}
		public string PRCD
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.PRCD;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.PRCD = value;
			}
		}
		public string GRACEDAY
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.GRACEDAY;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.GRACEDAY = value;
			}
		}
		public string CNIRDT
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.CNIRDT;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.CNIRDT = value;
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
		public string MORADT
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.MORADT;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.MORADT = value;
			}
		}
		#endregion

		#region response property
		/// <summary>下次攤還日</summary>
		public string PRDATE {
			get { return _rs.EAIBody.MsgRs.SvcRs.PRDATE; }
		}
		#endregion



	}
}
