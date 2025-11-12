using CommonLib.Core.Utility;
using CommonLib.Utility;
using CommonLib.Core.DataWork;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ModelCore.DataModel;
using ModelCore.Schema.ENID;

using System.Text;
using System.Xml;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
namespace TestConsole;

class Program
{
    static void Main(string[] args)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        if (args.Length > 0)
        {
            switch (args[0].ToLower())
            {
                case "lcr":
                    TestLCR();
                    return;
                case "lcd":
                    TestLCDetails();
                    return;
                case "lca":
                    TestLCA();
                    return;
                case "lcrn":
                    TestLCRN();
                    return;
                case "lcc":
                    TestLCC();
                    return;
                case "lccinfo":
                    TestLCCInfo();
                    return;
                case "csv":
                    if (!TestCSV(args))
                    {
                        return;
                    }
                    break;
                default:
                    Console.WriteLine("Invalid argument. Use 'lcr', 'lcd', 'lca', or 'csv'.");
                    return;
            }
        }
        else
        {
            Console.WriteLine("Invalid argument. Use 'lcr', 'lcd', 'lca', or 'csv'.");
        }

        //var json = ModelCore.Properties.Settings.Default.JsonStringify();
        //Console.WriteLine(json);
        //File.WriteAllText("G:\\temp\\settings.json", json, Encoding.UTF8);
        //bool flowControl = TestCSV(args);
        //if (!flowControl)
        //{
        //    return;
        //}

    }

    private static void TestLCCInfo()
    {
        using (ModelSource models = new ModelSource())
        {
            var sqlConnection = (SqlConnection)models.DataContext.Database.GetDbConnection();
            var reader = SqlHelper.ExecuteReader(sqlConnection, CommandType.Text,
                @"SELECT *,CONVERT(datetime, 
        REPLACE(REPLACE(STUFF(AUDIT_TIME, 11, 1, ' '),'上午','AM'),'下午','PM'), 
        0) as ConfirmTime FROM OPENROWSET('MSDASQL', 
   'Driver={Microsoft Access Text Driver (*.txt, *.csv)};DefaultDir=F:\Project\AppDev\HNB\Reference\hncb_enid_data_csv\;', 
   'SELECT * FROM EISSUED_LCD_AUDIT.csv') where ROLE_ID='manager'");
            while (reader.Read())
            {
                // Assuming the first column is the ID and the second column is the name
                var cancNo = reader.IsDBNull("LCD_NO") ? null : reader.GetString("LCD_NO");
                var cancelDate = reader.IsDBNull("ConfirmTime") ? DateTime.Now : reader.GetDateTime("ConfirmTime");
                if (string.IsNullOrEmpty(cancNo))
                {
                    Console.WriteLine("No data found.");
                    continue;
                }
                Console.WriteLine($"Application ID: {cancNo}");
                ModelStateDictionary modelState = new ModelStateDictionary();
                try
                {
                    var result = cancNo.CancelLc(cancelDate, modelState);
                    if (modelState.IsValid)
                    {
                        Console.WriteLine("Commit successful.");
                    }
                    else
                    {
                        foreach (var state in modelState)
                        {
                            Console.WriteLine($"Key: {state.Key}, Error: {state.Value.Errors.FirstOrDefault()?.ErrorMessage}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    FileLogger.Logger.Error(ex);
                }
            }
        }
    }

    private static void TestLCC()
    {
        using (ModelSource models = new ModelSource())
        {
            var sqlConnection = (SqlConnection)models.DataContext.Database.GetDbConnection();
            var reader = SqlHelper.ExecuteReader(sqlConnection, CommandType.Text,
                @"SELECT *,convert(varchar(max),dbo.TextToBinary(CIPHER)) as CipherData,convert(varchar(max),dbo.TextToBinary(VOID_XML)) as XmlData FROM OPENROWSET('MSDASQL', 
   'Driver={Microsoft Access Text Driver (*.txt, *.csv)};DefaultDir=F:\Project\AppDev\HNB\Reference\hncb_enid_data_csv\;', 
   'SELECT * FROM EISSUED_LCD.csv')");
            while (reader.Read())
            {
                // Assuming the first column is the ID and the second column is the name
                var cipherData = reader.IsDBNull("CipherData") ? null : reader.GetString("CipherData");
                var xmlData = reader.IsDBNull("XmlData") ? null : reader.GetString("XmlData");
                if (string.IsNullOrEmpty(xmlData))
                {
                    Console.WriteLine("No data found.");
                    continue;
                }
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlData);
                xmlDoc?.DocumentElement?.RemoveAttribute("xsi:type");
                CDSLcdCancellation cancelItem = xmlDoc.ConvertTo<CDSLcdCancellation>();
                Console.WriteLine($"Application ID: {cancelItem.lcCancInfo?.cancNo}");
                ModelStateDictionary modelState = new ModelStateDictionary();
                try
                {
                    var result = cancelItem.CommitLcCancellation(modelState);
                    if (modelState.IsValid)
                    {
                        Console.WriteLine("Commit successful.");
                    }
                    else
                    {
                        xmlDoc!.Save(Path.Combine(FileLogger.Logger.LogDailyPath, $"{cancelItem.lcCancInfo?.cancNo}.xml"));
                        foreach (var state in modelState)
                        {
                            Console.WriteLine($"Key: {state.Key}, Error: {state.Value.Errors.FirstOrDefault()?.ErrorMessage}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    FileLogger.Logger.Error(ex);
                }
            }
        }
    }

    private static void TestLCRN()
    {
        using (ModelSource models = new ModelSource())
        {
            var sqlConnection = (SqlConnection)models.DataContext.Database.GetDbConnection();
            var reader = SqlHelper.ExecuteReader(sqlConnection, CommandType.Text,
                @"SELECT *,convert(varchar(max),dbo.TextToBinary(CIPHER)) CipherData,convert(varchar(max),dbo.TextToBinary(NOTICE_XML)) XmlData FROM OPENROWSET('MSDASQL', 
   'Driver={Microsoft Access Text Driver (*.txt, *.csv)};DefaultDir=F:\Project\AppDev\HNB\Reference\hncb_enid_data_csv\;', 
   'SELECT * FROM EISSUED_LCRN.csv')");
            while (reader.Read())
            {
                // Assuming the first column is the ID and the second column is the name
                var cipherData = reader.IsDBNull("CipherData") ? null : reader.GetString("CipherData");
                var xmlData = reader.IsDBNull("XmlData") ? null : reader.GetString("XmlData");
                if (string.IsNullOrEmpty(xmlData))
                {
                    Console.WriteLine("No data found.");
                    continue;
                }
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlData);
                xmlDoc?.DocumentElement?.RemoveAttribute("xsi:type");
                CDSLcdAdvice notificationItem = xmlDoc.ConvertTo<CDSLcdAdvice>();
                Console.WriteLine($"Application ID: {notificationItem.lcAdviceInfo?.advNo}");
                ModelStateDictionary modelState = new ModelStateDictionary();
                try
                {
                    var result = notificationItem.CommitAmendmentNotification(modelState);
                    if (modelState.IsValid)
                    {
                        Console.WriteLine("Commit successful.");
                    }
                    else
                    {
                        xmlDoc!.Save(Path.Combine(FileLogger.Logger.LogDailyPath, $"{notificationItem.lcAdviceInfo?.advNo}.xml"));
                        foreach (var state in modelState)
                        {
                            Console.WriteLine($"Key: {state.Key}, Error: {state.Value.Errors.FirstOrDefault()?.ErrorMessage}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    FileLogger.Logger.Error(ex);
                }
            }
        }
    }

    private static void TestLCR()
    {
        using (ModelSource models = new ModelSource())
        {
                var sqlConnection = (SqlConnection)models.DataContext.Database.GetDbConnection();
                var reader = SqlHelper.ExecuteReader(sqlConnection, CommandType.Text,
                @"SELECT *,convert(varchar(max),dbo.TextToBinary(CIPHER)) CipherData,convert(varchar(max),dbo.TextToBinary(AMENDMENT_XML)) XmlData  FROM OPENROWSET('MSDASQL', 
   'Driver={Microsoft Access Text Driver (*.txt, *.csv)};DefaultDir=F:\Project\AppDev\HNB\Reference\hncb_enid_data_csv\;', 
   'SELECT * FROM EISSUED_LCR.csv')");
            while (reader.Read())
            {
                // Assuming the first column is the ID and the second column is the name
                var cipherData = reader.IsDBNull("CipherData") ? null : reader.GetString("CipherData");
                var xmlData = reader.IsDBNull("XmlData") ? null : reader.GetString("XmlData");
                if (string.IsNullOrEmpty(xmlData))
                {
                    Console.WriteLine("No data found.");
                    continue;
                }
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlData);
                xmlDoc?.DocumentElement?.RemoveAttribute("xsi:type");
                CDSLcdAmendment lcAmendment = xmlDoc.ConvertTo<CDSLcdAmendment>();
                Console.WriteLine($"Application ID: {lcAmendment.lcAmendInfo?.amendNo}");
                ModelStateDictionary modelState = new ModelStateDictionary();
                try
                {
                    var result = lcAmendment.CommitLcAmendment(modelState);
                    if (modelState.IsValid)
                    {
                        Console.WriteLine("Commit successful.");
                    }
                    else
                    {
                        xmlDoc!.Save(Path.Combine(FileLogger.Logger.LogDailyPath, $"{lcAmendment.lcAmendInfo?.amendNo}.xml"));
                        foreach (var state in modelState)
                        {
                            Console.WriteLine($"Key: {state.Key}, Error: {state.Value.Errors.FirstOrDefault()?.ErrorMessage}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    FileLogger.Logger.Error(ex);
                }
            }
        }
    }

    private static void TestLCDetails()
    {
        using (ModelSource models = new ModelSource())
        {
            var sqlConnection = (SqlConnection)models.DataContext.Database.GetDbConnection();
            var reader = SqlHelper.ExecuteReader(sqlConnection, CommandType.Text,
                @"SELECT *,convert(varchar(max),dbo.TextToBinary(CIPHER)) as CipherData,convert(varchar(max),dbo.TextToBinary(LC_XML)) as XmlData FROM OPENROWSET('MSDASQL', 
   'Driver={Microsoft Access Text Driver (*.txt, *.csv)};DefaultDir=F:\Project\AppDev\HNB\Reference\hncb_enid_data_csv\;', 
   'SELECT * FROM EISSUED_LCDETAIL.csv')");
            while (reader.Read())
            {
                // Assuming the first column is the ID and the second column is the name
                var cipherData = reader.IsDBNull("CipherData") ? null : reader.GetString("CipherData");
                var xmlData = reader.IsDBNull("XmlData") ? null : reader.GetString("XmlData");
                var version = reader.IsDBNull("VERSION") ? 0 : reader.GetInt32("VERSION");
                if (string.IsNullOrEmpty(xmlData))
                {
                    Console.WriteLine("No data found.");
                    continue;
                }
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlData);
                xmlDoc?.DocumentElement?.RemoveAttribute("xsi:type");
                CDSLcd lcDetails = xmlDoc.ConvertTo<CDSLcd>();
                Console.WriteLine($"Application ID: {lcDetails.lcInfo?.appNo}");
                //var items = lcDetails.lcDoc.conditions.Where(c => c.used == conditionsConditionUsed.@true).ToList();
                ModelStateDictionary modelState = new ModelStateDictionary();
                try
                {
                    var result = lcDetails.CommitLC(modelState, version);
                    if (modelState.IsValid)
                    {
                        Console.WriteLine("Commit successful.");
                    }
                    else
                    {
                        xmlDoc!.Save(Path.Combine(FileLogger.Logger.LogDailyPath, $"{lcDetails.lcDoc.lcNo}.xml"));
                        foreach (var state in modelState)
                        {
                            Console.WriteLine($"Key: {state.Key}, Error: {state.Value.Errors.FirstOrDefault()?.ErrorMessage}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    FileLogger.Logger.Error(ex);
                }
            }
        }
    }

    private static void TestLCA()
    {
        using (ModelSource models = new ModelSource())
        {
            var sqlConnection = (SqlConnection)models.DataContext.Database.GetDbConnection();
            var reader = SqlHelper.ExecuteReader(sqlConnection, CommandType.Text,
                @"SELECT *,convert(varchar(max),dbo.TextToBinary(CIPHER)) as CipherData,convert(varchar(max),dbo.TextToBinary(APPLICATION_XML)) as XmlData FROM OPENROWSET('MSDASQL', 
   'Driver={Microsoft Access Text Driver (*.txt, *.csv)};DefaultDir=F:\Project\AppDev\HNB\Reference\hncb_enid_data_csv\;', 
   'SELECT * FROM EISSUED_LCA.csv')");
            while (reader.Read())
            {
                // Assuming the first column is the ID and the second column is the name
                var cipherData = reader.IsDBNull("CipherData") ? null : reader.GetString("CipherData");
                var xmlData = reader.IsDBNull("XmlData") ? null : reader.GetString("XmlData");
                if (string.IsNullOrEmpty(xmlData))
                {
                    Console.WriteLine("No data found.");
                    continue;
                }
                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load("F:\\Project\\AppDev\\HNB\\master\\WebHome\\TestData\\LCA_CSC_1.xml");
                xmlDoc.LoadXml(xmlData);
                xmlDoc?.DocumentElement?.RemoveAttribute("xsi:type");
                CDSLcdApplication lcApp = xmlDoc.ConvertTo<CDSLcdApplication>();
                Console.WriteLine($"Application ID: {lcApp.lcAppInfo.appNo}");
                var items = lcApp.lcDoc.conditions.Where(c => c.used == conditionsConditionUsed.@true).ToList();
                ModelStateDictionary modelState = new ModelStateDictionary();
                try
                {
                    var result = lcApp.CommitCreditApplication(modelState);
                    if (modelState.IsValid)
                    {
                        Console.WriteLine("Commit successful.");
                    }
                    else
                    {
                        xmlDoc!.Save(Path.Combine(FileLogger.Logger.LogDailyPath, $"{lcApp.lcAppInfo.appNo}.xml"));
                        foreach (var state in modelState)
                        {
                            Console.WriteLine($"Key: {state.Key}, Error: {state.Value.Errors.FirstOrDefault()?.ErrorMessage}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    FileLogger.Logger.Error(ex);
                }
            }
        }
    }

    private static bool TestCSV(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine("Usage: <folderPath> <searchString> <replaceString> [extension]");
            return false;
        }

        string folderPath = args[0];
        string searchString = args[1];
        string replaceString = String.Compare("null", args[2], true) == 0 ? "" : args[2];
        string extension = args.Length > 3 ? args[3] : ".csv";
        if (!extension.StartsWith("."))
            extension = "." + extension;

        // Console.WriteLine($"{folderPath} {searchString} {replaceString} {extension}");
        // Console.ReadKey();

        if (!Directory.Exists(folderPath))
        {
            Console.WriteLine($"Folder not found: {folderPath}");
            return false;
        }

        var files = Directory.GetFiles(folderPath, "*" + extension, SearchOption.AllDirectories);
        Encoding encoding = Encoding.GetEncoding(950);
        foreach (var file in files)
        {
            string content = File.ReadAllText(file, encoding);
            if (content.Contains(searchString))
            {
                content = content.Replace(searchString, replaceString);
                Console.WriteLine($"Replaced in: {file}");
            }
            File.WriteAllText(file, content, encoding);
        }
        Console.WriteLine("Done.");
        return true;
    }
}
