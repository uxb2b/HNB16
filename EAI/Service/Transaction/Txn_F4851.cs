using System;
using System.Xml;
using System.Data;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>
	/// Txn_F4851 的摘要描述。
	/// </summary>
    public class Txn_F4851 : EAITransaction<F4851_Rq.IFX, F4851_Rs.IFX>
	{

        public Txn_F4851()
            : base("F4851")
        {
			//
			// TODO: 在此加入建構函式的程式碼
			//
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "F4851_Rq.xml"));
            _rq = doc.ConvertTo<F4851_Rq.IFX>();
            
            _rq.LogTxn.PrcDt = String.Format("{0:yyyy-MM-dd}", DateTime.Today);

            initializeData();

		}

        private void initializeData()
        {

        }


    }
}
