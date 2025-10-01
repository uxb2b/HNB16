using System;
using System.Xml;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>Txn_LR001 ���K�n�y�z�C</summary>
    public class Txn_LR001 : EAITransaction<LR001_Rq.IFX, LR001_Rs.IFX>
	{

		public Txn_LR001()	//�غc�禡
            : base("LR001")
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "LR001_Rq.xml"));
            _rq = doc.ConvertTo<LR001_Rq.IFX>();
            
            _rq.EAIBody.MsgRq.SvcRq.ACFLG = "0";
        }

		#region public property
		/// <summary>
		///		<BRNO>����O, 9(04)</BRNO> �}����N��
		/// </summary>
		public string BranchID
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.BRNO;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.BRNO = value;
			}
		}

		/// <summary>
		///		<CIFKEY>�Τ@�s��, X(10)</CIFKEY>
		/// </summary>
		public string CustomerReceiptNo
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.CIFKEY;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.CIFKEY = value;
			}
		}

		/// <summary>
		///		<CUNO>�U�ȸ��X, 9(05)</CUNO>
		/// </summary>
		public string CustomerID
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.CUNO;
			}
		}
		#endregion


	}
}
