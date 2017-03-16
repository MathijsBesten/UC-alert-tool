using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UC_alert_tool.Models;

namespace UC_alert_tool.Controllers
{

    /*
     NEVER EVER PUT LOG4NET LOGGING IN THIS CONTROLLER
     THIS WILL LOG ALL ACTIVITY FROM ALL USERS AND WILL RESULT IN A VERY BIG LOG FILE IN PRODUCTION
     */

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //these viewbags are for showing a success message or error message after a result
            ViewBag.showSuccess = TempData["showSuccess"];
            ViewBag.showError = TempData["showError"];
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            return View(Functions.Storingen.Get.AllStoringenFromLastWeek());
        }

        public ActionResult Contact()
        {     
            return View();
        }
    }
}