using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceStack.Logging;

namespace Alertsense.Azure.Lumberjack.Controllers
{
    public class HomeController : Controller
    {
        readonly ILog _log = LogManager.GetLogger(typeof(HomeController));
        public ActionResult Index()
        {
            _log.Debug("Home redirection to LogViewer Index.");
            try
            {
                throw new Exception("This is a warm fuzzy exception thrown to show a stack trace.");
            }
            catch (Exception e)
            {
                _log.Error("Catching a warm fuzzy exception..", e);
                return RedirectToAction("Index", "LogViewer");
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "This ain't about you...";
            _log.Debug("This ain't about you...");
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "657-5309.";

            return View();
        }
    }
}