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
            string ActiveStoringenName = Functions.Appsettings.Get.setting("activeIssuesTitle");
            string PlannedStoringenName = Functions.Appsettings.Get.setting("plannedIssuesTitle");
            string OldStoringenName = Functions.Appsettings.Get.setting("solvedIssuesTitle");
            List<Storingen> allStoringenFromLastWeek = Functions.Storingen.Get.AllStoringenFromLastWeek();
            List<Storingen> RunningStoringen = allStoringenFromLastWeek.Where(e => e.IsGesloten == false && e.Einddatum <= DateTime.Now).ToList();
            List<Storingen> PlannedStoringen = allStoringenFromLastWeek.Where(e => e.Begindatum.DayOfYear > DateTime.Now.DayOfYear && e.IsGesloten == false).ToList();
            List<Storingen> OldStoringen = allStoringenFromLastWeek.Where(e => e.IsGesloten == true).ToList();
            ViewBag.RunningStoringen = RunningStoringen;
            ViewBag.PlannedStoringen = PlannedStoringen;
            ViewBag.OldStoringen = OldStoringen;
            ViewBag.RunningStoringenTitle = ActiveStoringenName;
            ViewBag.PlannedStoringenTitle = PlannedStoringenName;
            ViewBag.OldStoringenTitle = OldStoringenName;
            return View();
        }

        public ActionResult Contact()
        {     
            return View();
        }
    }
}