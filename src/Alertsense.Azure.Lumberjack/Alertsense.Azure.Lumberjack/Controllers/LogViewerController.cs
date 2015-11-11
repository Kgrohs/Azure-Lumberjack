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
            set { _logViewerManager = value; }
        }

        //TODO: add which select list items are chosen to model for GET/POST request  (GET would allow someone to send a link to coworker and get same data..)

        public ActionResult Index(bool lumberjack = false, bool justBlogging = false)
        {
            var selectedConnections = new List<string>();
            if (lumberjack) selectedConnections.Add("Lumberjack");
            if (justBlogging) selectedConnections.Add("JustBlogging");

            var connectionList = new List<string>();
            connectionList.AddRange(MvcApplication.ConnectionList.Select(pair => pair.Key));


            var allLogs = new List<SourcedAdoNetLog>();
            foreach (var conn in selectedConnections)
            {
                var connMap = MvcApplication.ConnectionList.FirstOrDefault(x => x.Key == conn).Value;
                var tableName = "Log";
                var logViewerManager = LogViewerManagerFactory.CreateManager(null, null, connMap);
                {
                    //TODO: add information about which connMap the data belongs to to the list of allLogs..
                    allLogs.AddRange(logViewerManager.GetAllLogs(tableName, connMap));
                }
            }
            return View(new LogViewerViewModel
            {
                Logs = allLogs.OrderByDescending(x => x.Date),
                ConnectionList = connectionList
            });
        }

    }
}