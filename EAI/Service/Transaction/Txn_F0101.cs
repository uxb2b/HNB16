using System;
using System.Xml;
using System.Data;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>
	/// Txn_F0101 的摘要描述。
	/// </summary>
    public class Txn_F0101 : EAITransaction<F0101_Rq.IFX, F0101_Rs.IFX>
	{

        public Txn_F0101()
            : base("F0101")
        {
			//
			// TODO: 在此加入建構函式的程式碼
			//
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "F0101_Rq.xml"));
            _rq = doc.ConvertTo<F0101_Rq.IFX>();
            
            _rq.LogTxn.PrcDt = String.Format("{0:yyyy-MM-dd}", DateTime.Today);

            initializeData();

		}

        private void initializeData()
        {
            _rq.EAIBody.MsgRq.SvcRq.ACTNO = "";
            _rq.EAIBody.MsgRq.SvcRq.BRNO1 = "";
            _rq.EAIBody.MsgRq.SvcRq.BRNO2 = "";
            _rq.EAIBody.MsgRq.SvcRq.CIFERR = "";
            _rq.EAIBody.MsgRq.SvcRq.CIFKEY = "";
            _rq.EAIBody.MsgRq.SvcRq.CUCARD = "";
            _rq.EAIBody.MsgRq.SvcRq.CUNO = "";
            _rq.EAIBody.MsgRq.SvcRq.INQCD = "";
        }


	}
}
