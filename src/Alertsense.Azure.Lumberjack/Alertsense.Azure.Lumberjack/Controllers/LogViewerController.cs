using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Alertsense.Azure.Lumberjack.Models;
using AlertSense.Azure.Lumberjack.Contracts.Entities;
using AlertSense.Azure.Lumberjack.Contracts.Managers;
using AlertSense.Azure.Lumberjack.Services;
using log4net;

namespace Alertsense.Azure.Lumberjack.Controllers
{

    public class LogViewerController : Controller
    {
        readonly ILog _log = LogManager.GetLogger(typeof(LogViewerController));
 
        private ILogViewerManager _logViewerManager;
        public ILogViewerManager LogViewerManager
        {
            get
            {
                return _logViewerManager ?? (_logViewerManager = LogViewerManagerFactory.CreateManager());
            }
        }

        public ActionResult Index(LogViewerParamModel model)
        {
            var selectedConnections = new List<string>();
            if (model.Lumberjack != null && (model.Lumberjack.ToLowerInvariant() == "on" || model.Lumberjack.ToLowerInvariant() == "true")) selectedConnections.Add("Lumberjack");
            if (model.JustBlogging != null && (model.JustBlogging.ToLowerInvariant() == "on" || model.JustBlogging.ToLowerInvariant() == "true")) selectedConnections.Add("JustBlogging");

            var connectionList = new List<string>();
            connectionList.AddRange(MvcApplication.ConnectionList.Select(pair => pair.Key));

            IEnumerable<String> distinctLoggers = new List<String>();

            var allLogs = new List<SourcedAdoNetLog>();
            foreach (var conn in selectedConnections)
            {
                var connMap = MvcApplication.ConnectionList.FirstOrDefault(x => x.Key == conn).Value;
                var tableName = "Log";
                var logViewerManager = LogViewerManagerFactory.CreateManager(null, null, connMap);
                {
                    allLogs.AddRange(logViewerManager.GetAllLogs(tableName, conn));
                    //TODO: Make a new call to manager to get logs from the database using the search criteria.
                    allLogs.AddRange(logViewerManager.GetAllLogs(tableName, conn));
                    distinctLoggers = distinctLoggers.Concat(LogViewerManager.GetDistinctLoggersList(tableName));
                }
            }

            ViewBag.LogLevel = new SelectList(LogViewerManager.GetLogLevelList());
            ViewBag.LoggerType = new SelectList(distinctLoggers.Distinct());
            return View(new LogViewerViewModel
            {
                Logs = allLogs.OrderByDescending(x => x.Date),
                ConnectionList = connectionList
            });
        }

    }
}