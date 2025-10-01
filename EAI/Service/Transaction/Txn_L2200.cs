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
	/// Txn_L2200 ���K�n�y�z�C
	/// </summary>
	public class Txn_L2200 : EAITransaction<L2200_Rq.IFX,L2200_Rs.IFX>
	{


        public Txn_L2200()
            : base("L2200")
        {
            //
            // TODO: �b���[�J�غc�禡���{���X
            //
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "L2200_Rq.xml"));
            _rq = doc.ConvertTo<L2200_Rq.IFX>();
            
        }
       


    }
}
