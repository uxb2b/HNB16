using System;
using System.Xml;
using System.Data;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>
	/// Txn_L0602 的摘要描述。
	/// </summary>
    public class Txn_L0602 : EAITransaction<L0602_Rq.IFX, L0602_Rs.IFX>
	{

        public Txn_L0602()
            : base("L0602")
        {
			//
			// TODO: 在此加入建構函式的程式碼
			//
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "L0602_Rq.xml"));
            _rq = doc.ConvertTo<L0602_Rq.IFX>();
            
            _rq.LogTxn.PrcDt = String.Format("{0:yyyy-MM-dd}", DateTime.Today);

            initializeData();

		}

        private void initializeData()
        {
            _rq.EAIBody.MsgRq.SvcRq.CIFERR = "";
            _rq.EAIBody.MsgRq.SvcRq.CIFKEY = "";
            _rq.EAIBody.MsgRq.SvcRq.KINBR = "";
            _rq.EAIBody.MsgRq.SvcRq.BRNO = "";
        }


    }
}
