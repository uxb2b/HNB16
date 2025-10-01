using CommonLib.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelCore.Locale
{
    public static class ExtensionMethods
    {
        public static DateTime? FromChineseDateString(this string dateTimeStr)
        {
            dateTimeStr = dateTimeStr.GetEfficientString();
            if (dateTimeStr == null)
                return null;

            string[] ts = dateTimeStr.Split('.', '/');
            int year, month, day;
            if (ts.Length == 3 && int.TryParse(ts[0], out year) && int.TryParse(ts[1], out month) && int.TryParse(ts[2], out day))
            {
                return new DateTime(year + 1911, month, day);
            }
            return null;
        }
    }
}
