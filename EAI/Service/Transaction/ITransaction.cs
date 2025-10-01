using System;
using System.IO;
using System.Threading;
using System.Xml;
using EAI.Helper;
using CommonLib.Core.Utility;
using CommonLib.Utility;


namespace EAI.Service.Transaction
{
	/// <summary>
	/// Summary description for ITransaction.
	/// </summary>
	public interface ITransaction
	{
		bool Commit();
	}

    public interface IEAITransaction : ITransaction
    {
        String GetResponseDescription();
        String RspCode { get; }
    }

    public abstract partial class EAITransaction<TRq,TRs> : IEAITransaction
    {

        protected TRq _rq;
        protected TRs _rs;
        protected XmlDocument _docRq;
        protected XmlDocument _docRs;
        protected String _txnID;

        private static int __SEQ_SEED = (int)((DateTime.Now.Ticks / 100000) % 1000000);

        public const String EAI_Retry_Queue = "EAIRetry";

        public EAITransaction(String txnID)
        {
            _txnID = txnID;
        }


        public virtual bool Commit()
        {
            DoTransaction();

            bool result = ErrCode == "0"
                    && _docRs.DocumentElement?["EAIBody"]?["MsgRs"]?["Header"]?["TxnId"]?.InnerText == RspCode;

            if (!result && AutoRetry)
            {
                PutInRetrial();
            }

            return result;
        }

        public void DoTransaction()
        {
            _docRq = _rq.ConvertToXml();
            _docRq.DocumentElement["LogTxn"]["PmtID"].InnerText = String.Format("{0:yyyyMMdd}20{1:000000}", DateTime.Now, currentSequence);
            DateTime start = DateTime.Now;
            _docRs = TransactionSvc.InvokeOutboundEAI(_txnID, _docRq);
            Logger.Debug($"{_txnID}:{_docRq.DocumentElement["LogTxn"]["PmtID"].InnerText} => {(DateTime.Now-start).TotalMilliseconds} ms");
            _rs = _docRs.ConvertTo<TRs>();
        }

        public String PMTID => _docRq?.DocumentElement?["LogTxn"]?["PmtID"]?.InnerText;

        public void PutInRetrial()
        {
            String retrialPath = Path.Combine(Logger.LogPath, EAI_Retry_Queue);
            retrialPath.CheckStoredPath();
            Rq.ConvertToXml().Save(Path.Combine(retrialPath, $"{_txnID}_{DateTime.Now:yyyyMMddHHmmss}_{currentSequence}.xml"));
        }

        public TRq Rq
        {
            get
            {
                return _rq;
            }
        }

        public TRs Rs
        {
            get
            {
                return _rs;
            }
        }

        /// <summary>ErrCode</summary>
        /// <remarks>
        /// ErrCode 单 0 琌硈诀Θぃ玂靡ユ琌Θ
        /// ゲ斗 ErrCode 单 0 τ RspCode 单 TxnId ユΘ
        /// ErrCode ぃ单 0 ボ硈诀ア毖ErrCode 单 3001 硈絬筄
        /// </remarks>
        public string ErrCode
        {
            get
            {
                return _docRs.DocumentElement?["EAIHeader"]?["ErrCode"]?.InnerText;
            }
        }

        /// <summary>RspCode</summary>
        /// <remarks>ErrCode = 0 穦Τ RspCode</remarks>
        public string RspCode
        {
            get
            {
                if (ErrCode == "0")
                {
                    return _docRs.DocumentElement["EAIBody"]["MsgRs"]["Header"]["RspCode"].InnerText;
                }
                else
                {
                    return ErrCode;
                }
            }
        }

        public string Desc
        {
            get
            {
                return ErrCode == "0" 
                    ? _docRs.DocumentElement["EAIBody"]["MsgRs"]["Header"]["Desc"].InnerText : null;
            }
        }

        public virtual string GetResponseDescription()
        {
            return _docRs != null ? $"{this._txnID}:{this.RspCode}:{this.Desc}" : null;
        }

        public bool AutoRetry
        { get; set; }

        protected int currentSequence
        {
            get
            {
                Interlocked.Increment(ref __SEQ_SEED);
                __SEQ_SEED %= 1000000;
                return __SEQ_SEED;
            }
        }

        public static TRs LoadRsFile(String rsFile)
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.Load(rsFile);
            return doc.ConvertTo<TRs>();
        }
    }
}
