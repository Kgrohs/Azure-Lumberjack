using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Alertsense.Azure.Lumberjack.Helpers;
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
            connectionList.Add("LogsGoHereEmail");

            IEnumerable<String> distinctLoggers = new List<String>();

            var allLogs = new List<SourcedAdoNetLog>();
            foreach (var conn in selectedConnections)
            {
                var connMap = MvcApplication.ConnectionList.FirstOrDefault(x => x.Key == conn).Value;
                var tableName = "Log";
                var logViewerManager = LogViewerManagerFactory.CreateManager(null, null, connMap);
                {
                    allLogs.AddRange(logViewerManager.FilteredLogs(tableName, conn, model.LogLevel, model.StartDate, model.EndDate, model.LoggerType, model.Thread));
                    distinctLoggers = distinctLoggers.Concat(LogViewerManager.GetDistinctLoggersList(tableName));
                }
            }
            if (model.LogsGoHereEmail != null && (model.LogsGoHereEmail.ToLowerInvariant() == "on" || model.LogsGoHereEmail.ToLowerInvariant() == "true")) { 
                var emailLogs = EmailHelper.ImapGetLogEmails(50);
                allLogs.AddRange(emailLogs.FilterLogs(model));
                distinctLoggers = distinctLoggers.Concat(emailLogs.Select(x => x.Logger).Distinct());
            }

            ViewBag.LogLevel = new SelectList(LogViewerManager.GetLogLevelList());
            ViewBag.LoggerType = new SelectList(distinctLoggers.Where(x => x != null).Distinct());

            return View( new LogViewerViewModel
            {
                Logs = allLogs.OrderByDescending(x => x.Date),
                ConnectionList = connectionList
            });
        }

    }
}