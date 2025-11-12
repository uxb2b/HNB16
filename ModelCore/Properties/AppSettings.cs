using CommonLib.Utility;
using CommonLib.Utility.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelCore.Properties
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

        public String SystemKeyName { get; set; } = "SystemKey.new.json";
        public int PageSize { get; set; } = 10;
        public String TempPath { get; set; } = Path.Combine(AppRoot, "temp").CheckStoredPath();
        public string SystemID { get; set; } = "eLocalLC";
        public string BankID { get; set; } = "009";
        public string urlToCDS { get; set; } = "http://10.100.7.36/CHBGW01/CHB2CDSXML.ashx";
        public bool CheckSession { get; set; } = true;
        public int BankTimeoutInMinutes { get; set; } = 4;
        public string InvoiceResourcePath { get; set; } = "~/resource/invoice";
        public string AAUrl { get; set; } = "http://10.100.7.36:8080/eAuthority/DocumentFlow.do";
        public string SyncSessionUrl { get; set; } = "http://10.100.7.36:8080/eAuthority/syncSession.do";
        public string CheckCertificateUrl { get; set; } = "http://10.100.7.36:8080/eAuthority/checkCertificate.do";
        public string FpgConfirm { get; set; } = "https://ecrm.fpg.com.tw:8443/j2sp_LC_009/TestCon.jsp";
        public string FpgPost { get; set; } = "https://ecrm.fpg.com.tw:8443/j2sp_LC_009/TestRecv.jsp";
        public string FpgRemittanceFtp { get; set; } = "ftp://210.65.204.216/chbcp/online/{0:yyyyMMdd}/in";
        public string FpgRemittanceFtpUserName { get; set; } = "chbcptop";
        public string FpgRemittanceFtpPWD { get; set; } = "chbcptop";
        public string FpgRemittanceUrl { get; set; } = "http://localhost:5160/_test/WebDump.ashx";
        public string FpgCheckRemittanceUrl { get; set; } = "http://localhost:5160/_test/WebDump.ashx";
        public string ConnectionString { get; set; } = "";
        public bool EnableFpgRemittanceDispatch { get; set; } = false;
        public int HandlingChargeBaseAmount { get; set; } = 2000000;
        public int HandlingChargeStepAmount { get; set; } = 1000000;
        public string ProxyUrlToUXCDS { get; set; } //= "http://172.16.71.71:8080/";
        public String[] BankUserRoleID = ["auditor", "manager", "supervisor", "viewer", "administrator", "officer", "operator"];
        public String MailMessageUrl { get; set; } = "http://localhost:5160/MessageBox/MailMessage";
        public string WebMaster { get; set; } = "eNID@hncb.com.tw";
        public string ApplicationUrl { get; set; } = "http://localhost:5160";
        public string SmtpServer { get; set; } = "smtp.hncb.com.tw";
    }

}
