using System;
using System.Xml;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>
	/// Txn_LR018 的摘要描述。
	/// </summary>
    public class Txn_LR018 : EAITransaction<LR018_Rq.IFX, LR018_Rs.IFX>
	{

		public Txn_LR018()
            : base("LR018")
        {
			//
			// TODO: 在此加入建構函式的程式碼
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "LR018_Rq.xml"));
            _rq = doc.ConvertTo<LR018_Rq.IFX>();
            
            _rq.EAIBody.MsgRq.SvcRq.ACFLG = "0";

		}

		public string BranchID
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

		public string CustomerReceiptNo
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

		public DateTime StartDate
		{
			get
			{
				return System.DateTime.ParseExact(_rq.EAIBody.MsgRq.SvcRq.SDAY,"yyyyMMdd",System.Globalization.CultureInfo.CurrentCulture);
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.SDAY = String.Format("{0:yyyyMMdd}",value);
			}
		}

		public DateTime EndDate
		{
			get
			{
				return System.DateTime.ParseExact(_rq.EAIBody.MsgRq.SvcRq.EDAY,"yyyyMMdd",System.Globalization.CultureInfo.CurrentCulture);
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.EDAY = String.Format("{0:yyyyMMdd}",value);
			}
		}

		public int Kind
		{
			get
			{
				return int.Parse(_rq.EAIBody.MsgRq.SvcRq.KIND);
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.KIND = value.ToString();
			}
		}

		public double Amount
		{
			get
			{
				return double.Parse(_rq.EAIBody.MsgRq.SvcRq.AMT);
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.AMT = value.ToString();
			}
		}

		public bool IsDayCnt
		{
			get
			{
				return "1".Equals(_rq.EAIBody.MsgRq.SvcRq.ISDAYCNT);
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.ISDAYCNT = (value)?"1":"0";
			}
		}

		public string Rate1
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.RATE1;
			}
		}

		public string Rate2
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.RATE2;
			}
		}

		public string Rate3
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.RATE3;
			}
		}

		public string Rate4
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.RATE4;
			}
		}

		public string Hcamt1
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.HCAMT1;
			}
		}

		public string Hcamt2
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.HCAMT2;
			}
		}

		public string Hcamt3
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.HCAMT3;
			}
		}

		public string Expmt
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.EXPAMT;
			}
		}

		public string Expmt1
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.EXPAMT1;
			}
		}

		public string Cramt
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.CRAMT;
			}
		}

        
		
	}
}
