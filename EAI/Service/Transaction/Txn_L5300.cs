using System;
using System.Xml;
using System.Data;

using EAI.Properties;
using CommonLib.Core.Utility;
using CommonLib.Utility;

namespace EAI.Service.Transaction
{
	/// <summary>
	/// Txn_L5300 的摘要描述。
	/// </summary>
    public class Txn_L5300 : EAITransaction<L5300_Rq.IFX, L5300_Rs.IFX>
	{

		public Txn_L5300()
            : base("L5300")
        {
			//
			// TODO: 在此加入建構函式的程式碼
			//
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Settings.Default.PhysicalTxnFilePath, "L5300_Rq.xml"));
            _rq = doc.ConvertTo<L5300_Rq.IFX>();
            
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
		public string ITEM
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.ITEM;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.ITEM = value;
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
		public string BCIFKEY
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.BCIFKEY;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.BCIFKEY = value;
			}
		}
		public string BCIFERR
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.BCIFERR;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.BCIFERR = value;
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
		public string DEADL
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.DEADL;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.DEADL = value;
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
		public string CHFEE
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.CHFEE;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.CHFEE = value;
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

		public string RVSYEAR
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.RVSYEAR;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.RVSYEAR = value;
			}
		}

		public string RVSSRNO
		{
			get
			{
				return _rq.EAIBody.MsgRq.SvcRq.RVSSRNO;
			}
			set
			{
				_rq.EAIBody.MsgRq.SvcRq.RVSSRNO = value;
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
		public string CNAME
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.CNAME;
			}
		}
		public string Rs_CIFKEY
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.CIFKEY;
			}
		}
		public string Rs_CIFERR
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.CIFERR;
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
		public string TXAMT
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.TXAMT;
			}
		}
		public string EDAY
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.EDAY;
			}
		}
		public string Rs_NOTEBK
		{
			get
			{
				return _rs.EAIBody.MsgRs.SvcRs.NOTEBK;
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
