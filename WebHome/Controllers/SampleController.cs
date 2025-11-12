using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebHome.Properties;
using ModelCore.Models.ViewModel;
using ModelCore.Helper;
using CommonLib.Utility;
using CommonLib.Core.DataWork;
using CommonLib.Core.Utility;
using WebHome.Helper;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using ModelCore.DataModel;

namespace WebHome.Controllers
{
    public class SampleController : CommonLib.Core.Controllers.SampleController
    {
        protected internal ModelSource? _dataSource;

        protected internal bool _dbInstance;
        protected internal GenericManager<LcEntityDbContext>? models;

        public SampleController(IServiceProvider serviceProvider, ILoggerFactory loggerFactory) : base(serviceProvider)
        {
            Logger = loggerFactory.CreateLogger(this.GetType());
        }

        public ILogger Logger
        {
            get;
            private set;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (_dbInstance && models!=null)
            {
                models.Dispose();
            }
        }

        public ModelSource DataSource => _dataSource!;

        protected LcEntityDbContext db => models!.DataContext;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            models = HttpContext.Items["__DB_Instance"] as GenericManager<LcEntityDbContext>;
            if (models == null)
            {
                models = new GenericManager<LcEntityDbContext>();
                _dbInstance = true;
                HttpContext.Items["__DB_Instance"] = models;
            }

            _dataSource = new ModelSource(models);
            HttpContext.Items["Models"] = DataSource;
            HttpContext.Items["Controller"] = this;

            var lang = Request.Cookies["cLang"];
            if (lang != null)
            {
                var cultureInfo = new CultureInfo(lang);
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);
                ViewBag.Lang = lang;
            }
        }

        //protected ActionResult PageResult(QueryViewModel viewModel, IQueryable<dynamic> items)
        //{

        //    viewModel.RecordCount = items.Count();
        //    if (viewModel.InitQuery != true && viewModel.PageIndex.HasValue)
        //    {
        //        viewModel.PageIndex--;
        //        return View(viewModel.ResultView, items);
        //    }
        //    else
        //    {
        //        viewModel.PageIndex = 0;
        //        if (viewModel.QueryResult == null)
        //        {
        //            viewModel.QueryResult = "~/Views/Common/Module/QueryResult.cshtml";
        //        }

        //        return View(viewModel.QueryResult, items);
        //    }
        //}

        //public ActionResult CreateExcelDownloadResult(IQueryable<dynamic> items, String tableName, String downloadFileName)
        //{

        //    DataTable table = new DataTable(tableName);
        //    items.BuildDataColumns(table);

        //    ProcessRequest processItem = new ProcessRequest
        //    {
        //        Sender = HttpContext.GetUser()?.UID,
        //        SubmitDate = DateTime.Now,
        //        ProcessStart = DateTime.Now,
        //        ResponsePath = System.IO.Path.Combine(CommonLib.Core.Utility.FileLogger.Logger.LogDailyPath, Guid.NewGuid().ToString() + ".xlsx"),
        //    };
        //    models!.GetTable<ProcessRequest>().Add(processItem);
        //    models.SubmitChanges();

        //    SqlCommand sqlCmd = (SqlCommand)models.GetCommand(items);
        //    sqlCmd.SaveAsExcel(table, processItem.ResponsePath, processItem.TaskID);

        //    return View("~/Views/Shared/Module/PromptCheckDownload.cshtml",
        //            new AttachmentViewModel
        //            {
        //                TaskID = processItem.TaskID,
        //                FileName = processItem.ResponsePath,
        //                FileDownloadName = downloadFileName,
        //            });
        //}

    }
}