using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ServiceStack.Logging;

namespace Alertsense.Azure.Lumberjack
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static ILog _log;
        public static List<KeyValuePair<string, string>> ConnectionList { get; set; }


        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            log4net.Config.XmlConfigurator.Configure();

            _log = LogManager.GetLogger("GlobalHttpApplication");

            ConnectionList = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Lumberjack", "LoggingConnection1"),
                new KeyValuePair<string, string>("JustBlogging","LoggingConnection2")
            };

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            if (_log != null) _log.Fatal(ex.Message, ex);
        }

    }
}