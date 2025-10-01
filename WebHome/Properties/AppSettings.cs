using CommonLib.Core.Utility;
using CommonLib.Utility.Properties;
using CommonLib.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHome.Properties
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

        public String ApplicationPath { get; set; } = "";
        public double SessionTimeoutInMinutes { get; set; } = 20;
        public double LoginExpireMinutes { get; set; } = 1440 * 7;
        public String LoginUrl { get; set; } = "/Account/Login";
        public String LogoutUrl { get; set; } = "/Account/Logout";
        public string[] AllowCORS { get; set; } = { "https://localhost:7115", "http://localhost:5160" };

    }

}