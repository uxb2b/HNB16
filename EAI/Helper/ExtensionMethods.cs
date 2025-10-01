using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace EAI.Helper
{
    public static class ExtensionMethods
    {
        public static String GetResponseDescription(this XmlDocument doc)
        {
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("ifx","http://www.ifxforum.org");
            var node = doc.SelectSingleNode("/ifx:IFX/ifx:EAIBody/ifx:MsgRs/ifx:Header/ifx:Desc/text()", nsmgr);
            if (node != null)
                return node.Value.Trim();
            return null;
        }
    }
}
