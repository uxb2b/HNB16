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
	/// Txn_L2300 ���K�n�y�z�C
	/// </summary>
	public class Txn_L2300 : EAITransaction<L2300_Rq.IFX,L2300_Rs.IFX>
	{


        public Txn_L2300()
            : base("L2300")
        {
            //
            // TODO: �b���[�J�غc�禡���{���X
            //
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "L2300_Rq.xml"));
            _rq = doc.ConvertTo<L2300_Rq.IFX>();
            
        }
       


    }
}
