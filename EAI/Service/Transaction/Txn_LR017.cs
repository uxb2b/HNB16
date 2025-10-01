using System;
using System.Xml;
using System.Data;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>
	/// Txn_LR017 的摘要描述。
	/// </summary>
    public class Txn_LR017 : EAITransaction<LR017_Rq.IFX, LR017_Rs.IFX>
	{


		public Txn_LR017()
            : base("LR017")
        {
			//
			// TODO: 在此加入建構函式的程式碼
			//
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "LR017_Rq.xml"));
            _rq = doc.ConvertTo<LR017_Rq.IFX>();
            
            _rq.EAIBody.MsgRq.SvcRq.ACFLG = "2";

		}

		public string Account
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.ACTNO;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.ACTNO = value;
			}
		}

		public string BRNO
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.BRNO;
			}
		}
		public string TRMSEQ
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.TRMSEQ;
			}
		}
		public string TXTNO
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.TXTNO;
			}
		}
		public string APNO
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.APNO;
			}
		}
		public string SRNO
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.SRNO;
			}
		}
		public string CHKDG
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.CHKDG;
			}
		}
		public string SQNO
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.SQNO;
			}
		}
		public string CIFKEY
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.CIFKEY;
			}
		}
		public string CIFERR
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.CIFERR;
			}
		}
		public string ACNO
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.ACNO;
			}
		}
		public string SBNO
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.SBNO;
			}
		}
		public string CHARCD1
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.CHARCD1;
			}
		}
		public string CHARCD2
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.CHARCD2;
			}
		}
		public string SDAY
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.SDAY;
			}
		}
		public string EDAY
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.EDAY;
			}
		}
		public string LASTDT
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.LASTDT;
			}
		}
		public string RTDAY
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.RTDAY;
			}
		}
		public string CRGRRATE
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.CRGRRATE;
			}
		}
		public string CRGRTYPE
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.CRGRTYPE;
			}
		}
		public string SQAMT
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.SQAMT;
			}
		}
		public string SQBAL
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.SQBAL;
			}
		}
		public string GRAMT
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.GRAMT;
			}
		}
		public string GRBAL
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.GRBAL;
			}
		}
		public string ARBAL
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.ARBAL;
			}
		}
		public string ACPCD
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.ACPCD;
			}
		}
		public string NOTEBK
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.NOTEBK;
			}
		}
		public string BCIFKEY
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.BCIFKEY;
			}
		}
		public string BCIFERR
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.BCIFERR;
			}
		}
		public string ISDAYCNT
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.ISDAYCNT;
			}
		}
		public string RATE
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.RATE;
			}
		}
		public string FEE
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.FEE;
			}
		}
		public string CHRATE
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.CHRATE;
			}
		}
		public string CHFEE
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.CHFEE;
			}
		}
		public string HCRATE
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.HCRATE;
			}
		}
		public string HCFEE
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.HCFEE;
			}
		}
		public string FUNCD
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.FUNCD;
			}
		}
		public string DMPNO
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.DMPNO;
			}
		}
		public string DMUNO
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.DMUNO;
			}
		}
		public string DECODE
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.DECODE;
			}
		}
		public string DEBKNO
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.DEBKNO;
			}
		}
		public string ACTCD
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.ACTCD;
			}
		}
		public string MGBRNO
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.MGBRNO;
			}
		}
		public string MGAPNO
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.MGAPNO;
			}
		}
		public string MGSBNO
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.MGSBNO;
			}
		}
		public string MGSRNO
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.MGSRNO;
			}
		}
		public string IBKNO
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.IBKNO;
			}
		}
		public string IBRNO
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.IBRNO;
			}
		}
		public string RBRNO
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.RBRNO;
			}
		}
		public string SBRNO
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.SBRNO;
			}
		}
		public string SPCD
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.SPCD;
			}
		}
		public string MGENO
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.MGENO;
			}
		}
		public string SPENO
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.SPENO;
			}
		}
		public string DMCD
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.DMCD;
			}
		}
		public string OCODE
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.OCODE;
			}
		}
		public string MNCNT
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.MNCNT;
			}
		}
		public string DTCNT
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.DTCNT;
			}
		}
		public string PAYNO
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.PAYNO;
			}
		}
		public string PAYTYPE
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.PAYTYPE;
			}
		}
		public string PAYDAY
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.PAYDAY;
			}
		}
		public string CONT1
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.CONT1;
			}
		}
		public string CONT2
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.CONT2;
			}
		}
		public string CONT3
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.CONT3;
			}
		}
		public string SEAL
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.SEAL;
			}
		}
		public string DELIVER
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.DELIVER;
			}
		}
		public string DEADL
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.DEADL;
			}
		}
		public string VRATE
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.VRATE;
			}
		}
		public string VINT
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.VINT;
			}
		}
		public string NVINT
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.NVINT;
			}
		}
		public string VDFAMT
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.VDFAMT;
			}
		}
		public string NVDFAMT
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.NVDFAMT;
			}
		}
		public string EMPNOT
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.EMPNOT;
			}
		}
		public string EMPNOS
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.EMPNOS;
			}
		}
		public string VDATE
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.VDATE;
			}
		}
		public string CONVFG
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.CONVFG;
			}
		}
		public string CURCD
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.CURCD;
			}
		}
		public string LOANTYP
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.LOANTYP;
			}
		}
		public string GOVSTYP
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.GOVSTYP;
			}
		}
		public string SETGFG
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.SETGFG;
			}
		}
		public string LCBRNO
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.LCBRNO;
			}
		}
		public string LCAPNO
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.LCAPNO;
			}
		}
		public string LCSRNO
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.LCSRNO;
			}
		}
		public string LCCHKDG
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.LCCHKDG;
			}
		}
		public string LCSQNO
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.LCSQNO;
			}
		}
		public string TYPE
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.TYPE;
			}
		}
		public string FCURCD
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.FCURCD;
			}
		}
		public string FSQAMT
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.FSQAMT;
			}
		}
		public string EXRATE
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.EXRATE;
			}
		}
		public string FSQBAL
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.FSQBAL;
			}
		}
		public string ELCDTLFG
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.ELCDTLFG;
			}
		}
		public string GTXNO
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.GTXNO;
			}
		}

			
	}
}
