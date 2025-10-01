using System;
using System.Xml;
using System.Data;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>
	/// Txn_F4852 的摘要描述。
	/// </summary>
    public class Txn_F4852 : EAITransaction<F4852_Rq.IFX, F4852_Rs.IFX>
	{

        public Txn_F4852()
            : base("F4852")
        {
			//
			// TODO: 在此加入建構函式的程式碼
			//
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "F4852_Rq.xml"));
            _rq = doc.ConvertTo<F4852_Rq.IFX>();
            
            _rq.LogTxn.PrcDt = String.Format("{0:yyyy-MM-dd}", DateTime.Today);

            initializeData();

		}

        private void initializeData()
        {

        }


    }
}
