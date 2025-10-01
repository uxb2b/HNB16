using System;
using System.Xml;
using System.Linq;
using System.Collections.Generic;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>
	/// Txn_LR001 的摘要描述。
	/// </summary>
    public class Txn_D0310 : EAITransaction<D0310_Rq.IFX, D0310_Rs.IFX>
	{

		public Txn_D0310()
            : base("D0310")
        {
			//
			// TODO: 在此加入建構函式的程式碼
			//
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "D0310_Rq.xml"));
            _rq = doc.ConvertTo<D0310_Rq.IFX>();
            
            _rq.EAIBody.MsgRq.SvcRq.ACFLG = "0";

		}
		public string MGNO(bool bolAtsight)
		{
            string itemNo = bolAtsight ? "8910" : "8920";
            return MGNO(itemNo);
		}

        public string MGNO(String itemNo)
        {
            if (_rs.EAIBody.MsgRs.SvcRs.Detail != null)
            {
                var item = _rs.EAIBody.MsgRs.SvcRs.Detail.Where(d => d.MGNO != null && d.MGNO.Contains(itemNo)).OrderByDescending(d => d.MGNO).FirstOrDefault();
                if (item != null)
                {
                    return item.MGNO;
                }
            }
            return "";
        }


        public EAI.Service.Transaction.D0310_Rs.IFXEAIBodyMsgRsSvcRsDetail[] AccountList
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.Detail;
			}
		}


		public string BranchID            //分行ID
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

		public string CustomerReceiptNo   //統一編號
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
        		
	}
}
