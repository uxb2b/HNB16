using System;
using System.Xml;
using System.Data;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>
	/// Txn_F4855 的摘要描述。
	/// </summary>
    public class Txn_F4855 : EAITransaction<F4855_Rq.IFX, F4855_Rs.IFX>
	{

        public Txn_F4855()
            : base("F4855")
        {
			//
			// TODO: 在此加入建構函式的程式碼
			//
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "F4855_Rq.xml"));
            _rq = doc.ConvertTo<F4855_Rq.IFX>();
            
            _rq.LogTxn.PrcDt = String.Format("{0:yyyy-MM-dd}", DateTime.Today);

            initializeData();

		}

        private void initializeData()
        {

        }


    }
}
