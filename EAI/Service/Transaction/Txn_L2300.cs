using System;
using System.Xml;
using System.Data;

using EAI.Properties;
using CommonLib.Core.Utility;
using EAI.Helper;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>
	/// Txn_L2300 的摘要描述。
	/// </summary>
	public class Txn_L2300 : EAITransaction<L2300_Rq.IFX,L2300_Rs.IFX>
	{


        public Txn_L2300()
            : base("L2300")
        {
            //
            // TODO: 在此加入建構函式的程式碼
            //
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "L2300_Rq.xml"));
            _rq = doc.ConvertTo<L2300_Rq.IFX>();
            
        }
       


    }
}
