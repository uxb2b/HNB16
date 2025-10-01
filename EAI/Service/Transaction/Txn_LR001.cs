using System;
using System.Xml;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>Txn_LR001 的摘要描述。</summary>
    public class Txn_LR001 : EAITransaction<LR001_Rq.IFX, LR001_Rs.IFX>
	{

		public Txn_LR001()	//建構函式
            : base("LR001")
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "LR001_Rq.xml"));
            _rq = doc.ConvertTo<LR001_Rq.IFX>();
            
            _rq.EAIBody.MsgRq.SvcRq.ACFLG = "0";
        }

		#region public property
		/// <summary>
		///		<BRNO>分行別, 9(04)</BRNO> 開狀行代號
		/// </summary>
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

		/// <summary>
		///		<CIFKEY>統一編號, X(10)</CIFKEY>
		/// </summary>
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

		/// <summary>
		///		<CUNO>顧客號碼, 9(05)</CUNO>
		/// </summary>
		public string CustomerID
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.CUNO;
			}
		}
		#endregion


	}
}
