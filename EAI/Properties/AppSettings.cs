using CommonLib.Utility.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAI.Properties
{
    public partial class AppSettings : AppSettingsBase
    {
        static AppSettings()
        {
            _default = Initialize<AppSettings>(typeof(AppSettings).Namespace);
        }

        public AppSettings() : base()
        {

        }

        static AppSettings _default;
        public static AppSettings Default => _default;

        public static void Reload()
        {
            Reload<AppSettings>(ref _default, typeof(AppSettings).Namespace);
        }

        public string PhysicalTxnFilePath
        {
            get;
            set;
        }

        TimeInterval[] _interval;

        public TimeInterval[] TimeToRetryR3801ByTheDay
        {
            get
            {
                return _interval;
            }
            set
            {
                _interval = value;
            }
        }

        public string TxnFilePath { get; set; }
        public string host { get; set; }
        public int port { get; set; }
        public string qManagerName { get; set; }
        public string channelName { get; set; }
        public string sendQName { get; set; }
        public string rcvQName { get; set; }
        public bool UseEAI { get; set; }
        public bool UseEAIProxy { get; set; }
        public string EAIProxy { get; set; }
        public string BS_Host { get; set; }
        public int BS_Port { get; set; }
        public string BS_QManagerName { get; set; }
        public string BS_QChannelName { get; set; }
        public string BS_SendQName { get; set; }
        public string BS_RCVQName { get; set; }
        public bool BS_UseEAIProxy { get; set; }
        public string BS_EAIProxy { get; set; }
        public string JS_R3801RetryTime { get; set; }
        public global::System.TimeSpan R3801TimeUp { get; set; }
        public global::System.TimeSpan EAIRetryStartAt { get; set; }
        public string BR_Host { get; set; }
        public int BR_Port { get; set; }
        public string BR_QManagerName { get; set; }
        public string BR_QChannelName { get; set; }
        public string BR_SendQName { get; set; }
        public string BR_RCVQName { get; set; }

        public class TimeInterval
        {
            public TimeSpan From { get; set; }
            public TimeSpan To { get; set; }
        }
    }

}
