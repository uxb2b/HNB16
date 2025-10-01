using System;
using System.Xml;
using System.Data;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>
	/// Txn_FR062 的摘要描述。
	/// </summary>
    public class Txn_FR062 : EAITransaction<FR062_Rq.IFX, FR062_Rs.IFX>
	{

        public Txn_FR062()
            : base("FR062")
        {
			//
			// TODO: 在此加入建構函式的程式碼
			//
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "FR062_Rq.xml"));
            _rq = doc.ConvertTo<FR062_Rq.IFX>();
            
            _rq.LogTxn.PrcDt = String.Format("{0:yyyy-MM-dd}", DateTime.Today);

            initializeData();

		}

        private void initializeData()
        {
            _rq.EAIBody.MsgRq.SvcRq.CIFERR = "";
            _rq.EAIBody.MsgRq.SvcRq.CIFKEY = "";
            _rq.EAIBody.MsgRq.SvcRq.NAL = "TW";
        }


    }
}
