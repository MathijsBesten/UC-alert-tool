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
        private AlertToolContext db = new AlertToolContext();

        public ActionResult Index()
        {
            return View(db.Storingen.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {     
            return View();
        }
    }
}