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
        private alertDatabaseEntities db = new alertDatabaseEntities();

        public ActionResult Index()
        {
            return View(db.Storingen.ToList());
        }

        public ActionResult Contact()
        {     
            return View();
        }
    }
}