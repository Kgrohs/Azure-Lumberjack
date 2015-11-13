using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Alertsense.Azure.Lumberjack.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index","LogViewer");
        }

        public ActionResult About()
        {
            ViewBag.Message = "This ain't about you...";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "657-5309.";

            return View();
        }
    }
}