using CommonLib.Core.Utility;
using CommonLib.PlugInAdapter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace EAI.Helper
{
    public class EAILog : ILogObject2
    {
        public String TxnID;
        public XmlDocument EAIDoc;
        public bool IsRs;

        public string GetFileName(string currentLogPath, string qName, ulong key)
        {
            return Path.Combine(currentLogPath, String.Format("{0}_{1:000000000000}_{2}.xml", TxnID, key, IsRs ? "RS" : "RQ"));
        }

        public string Subject
        {
            get { return TxnID; }
        }

        public override string ToString()
        {
            return EAIDoc != null ? EAIDoc.OuterXml : null;
        }
    }
}
