using System;
using System.Xml;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>
	/// Txn_L1203 的摘要描述。
	/// </summary>
    public class Txn_L1203 : EAITransaction<L1203_Rq.IFX, L1203_Rs.IFX>
	{

		public Txn_L1203()
            : base("L1203")
        {
			//
			// TODO: 在此加入建構函式的程式碼
			//
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "L1203_Rq.xml"));
            _rq = doc.ConvertTo<L1203_Rq.IFX>();
            
            _rq.EAIBody.MsgRq.SvcRq.ACFLG = "0";

		}

		public string KINBR
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.KINBR;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.KINBR = value;
			}
		}
		public string HCODE
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.HCODE;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.HCODE = value;
			}
		}
		public string ECKIN
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.ECKIN;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.ECKIN = value;
			}
		}
		public string ECTRM
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.ECTRM;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.ECTRM = value;
			}
		}
		public string ECTNO
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.ECTNO;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.ECTNO = value;
			}
		}
		public string RBRNO
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.RBRNO;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.RBRNO = value;
			}
		}
		public string SBRNO
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.SBRNO;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.SBRNO = value;
			}
		}
		public string CIFKEY
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
		public string CIFERR
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.CIFERR;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.CIFERR = value;
			}
		}
		public string MGDATA
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.MGDATA;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.MGDATA = value;
			}
		}
		public string ACTNO
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
		public string CHARCD1
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.CHARCD1;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.CHARCD1 = value;
			}
		}
		public string CHARCD2
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.CHARCD2;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.CHARCD2 = value;
			}
		}
		public string ACNO
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.ACNO;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.ACNO = value;
			}
		}
		public string SBNO
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.SBNO;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.SBNO = value;
			}
		}
		public string SDAY
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.SDAY;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.SDAY = value;
			}
		}
		public string GRAMT
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.GRAMT;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.GRAMT = value;
			}
		}
		public string EDAY
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.EDAY;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.EDAY = value;
			}
		}
		public string RTDAY
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.RTDAY;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.RTDAY = value;
			}
		}
		public string NOTEBK
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.NOTEBK;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.NOTEBK = value;
			}
		}
		public string BUNINO
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.BUNINO;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.BUNINO = value;
			}
		}
		public string ISDAYCNT
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.ISDAYCNT;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.ISDAYCNT = value;
			}
		}
		public string RATE
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.RATE;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.RATE = value;
			}
		}
		public string FEE
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.FEE;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.FEE = value;
			}
		}
		public string HCRATE
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.HCRATE;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.HCRATE = value;
			}
		}
		public string HCFEE
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.HCFEE;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.HCFEE = value;
			}
		}
		public string TXTYPE
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.TXTYPE;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.TXTYPE = value;
			}
		}
		public string PACTNO
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.PACTNO;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.PACTNO = value;
			}
		}
		public string CKAMT
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.CKAMT;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.CKAMT = value;
			}
		}
		public string CKNO
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.CKNO;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.CKNO = value;
			}
		}
		public string CKBKNO7
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.CKBKNO7;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.CKBKNO7 = value;
			}
		}
		public string CKACTNO
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.CKACTNO;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.CKACTNO = value;
			}
		}
		public string FUNCD
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.FUNCD;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.FUNCD = value;
			}
		}
		public string MGENO
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.MGENO;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.MGENO = value;
			}
		}
		public string SPENO
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.SPENO;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.SPENO = value;
			}
		}
		public string PAYNO
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.PAYNO;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.PAYNO = value;
			}
		}
		public string PAYTYPE
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.PAYTYPE;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.PAYTYPE = value;
			}
		}
		public string PAYDAY
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.PAYDAY;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.PAYDAY = value;
			}
		}
		public string SEAL
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.SEAL;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.SEAL = value;
			}
		}
		public string DELIVER
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.DELIVER;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.DELIVER = value;
			}
		}
		public string CONT1
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.CONT1;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.CONT1 = value;
			}
		}
		public string CONT2
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.CONT2;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.CONT2 = value;
			}
		}
		public string CONT3
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.CONT3;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.CONT3 = value;
			}
		}
		public string DEADLE
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.DEADLE;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.DEADLE = value;
			}
		}
		public string GTXNO
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.GTXNO;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.GTXNO = value;
			}
		}
		public string RVSNO
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.RVSNO;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.RVSNO = value;
			}
		}
		public string LOANTYP
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.LOANTYP;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.LOANTYP = value;
			}
		}
		public string GOVSTYP
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.GOVSTYP;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.GOVSTYP = value;
			}
		}
		public string SETGFG
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.SETGFG;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.SETGFG = value;
			}
		}
		public string TYPE
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.TYPE;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.TYPE = value;
			}
		}
		public string FSQAMT
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.FSQAMT;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.FSQAMT = value;
			}
		}
		public string FCURCD
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.FCURCD;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.FCURCD = value;
			}
		}
		public string EXRATE
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.EXRATE;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.EXRATE = value;
			}
		}
		public string TXAMT
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.TXAMT;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.TXAMT = value;
			}
		}
		public string ELCDTLFG
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.ELCDTLFG;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.ELCDTLFG = value;
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
		public string Rs_ACTNO
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.ACTNO;
			}
		}
		public string NAME
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.NAME;
			}
		}
		public string COMADR
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.COMADR;
			}
		}
		public string BNAME
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.BNAME;
			}
		}
		public string BADDR
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.BADDR;
			}
		}
		public string BRCNAME
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.BRCNAME;
			}
		}
		public string BRCADDR
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.BRCADDR;
			}
		}
		public string BRNAME
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.BRNAME;
			}
		}
		public string ACBRNAME
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.ACBRNAME;
			}
		}
		public string CADDR
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.CADDR;
			}
		}
		public string SUPCNM
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.SUPCNM;
			}
		}



		public String CRGRTYPE
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.CRGRTYPE;;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.CRGRTYPE = value;
			}
		}

		public String CRGRRATE
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.CRGRRATE;;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.CRGRRATE = value;
			}

        }
        public String CRGRAMT
        {
            get
            {
                return _rq.EAIBody.MsgRq.SvcRq.CRGRAMT; ;
            }
            set
            {
                _rq.EAIBody.MsgRq.SvcRq.CRGRAMT = value;
            }

        }
        public String PAYDATE
        {
            get
            {
                return _rq.EAIBody.MsgRq.SvcRq.PAYDATE; ;
            }
            set
            {
                _rq.EAIBody.MsgRq.SvcRq.PAYDATE = value;
            }

        }

	}
}
