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
	/// Txn_L4200 ���K�n�y�z�C
	/// </summary>
	public class Txn_L4200 : EAITransaction<L4200_Rq.IFX,L4200_Rs.IFX>
	{


        public Txn_L4200()
            : base("L4200")
        {
            //
            // TODO: �b���[�J�غc�禡���{���X
            //
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "L4200_Rq.xml"));
            _rq = doc.ConvertTo<L4200_Rq.IFX>();
            
        }
       


    }
}
