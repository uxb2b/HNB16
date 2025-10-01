using ModelCore.EventMessageApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelCore.Helper
{
    public static class Logger
    {
        public enum LogLevel
        {
            Normal = 1,
            Information,
            Debug,
            Trace,
            Error,
            Critial,
            Fatal,
        }

        public static void Error(String log, LogLevel? level = null)
        {
            Task.Run(() =>
            {
                CommonLib.Core.Utility.Logger.Error(log);

                if (level.HasValue)
                {
                    MessageNotification.ExceptionMessageAlert(log);
                }
            });
        }

        public static void Error(Exception ex, LogLevel? level = null)
        {
            Task.Run(() =>
            {
                CommonLib.Core.Utility.Logger.Error(ex);

                if (level.HasValue)
                {
                    MessageNotification.ExceptionAlert(ex);
                }
            });
        }
    }
}
