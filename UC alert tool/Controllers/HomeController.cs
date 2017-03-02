using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UC_alert_tool.Models;

namespace UC_alert_tool.Controllers
{
    public class HomeController : Controller
    {
        private static ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private alertDatabaseEntities db = new alertDatabaseEntities();
        public bool showSuccess = false;
        public bool showError = false;
        public string SuccessMessage = null;
        public string ErrorMessage = null;

        public ActionResult Index()
        {
            ViewBag.showSuccess = showSuccess;
            ViewBag.showError = showError;
            ViewBag.SuccessMessage = SuccessMessage;
            ViewBag.ErrorMessage = ErrorMessage;

            return View(Functions.Storingen.Get.AllStoringenFromLastWeek());
        }

        public ActionResult Contact()
        {     
            return View();
        }
    }
}