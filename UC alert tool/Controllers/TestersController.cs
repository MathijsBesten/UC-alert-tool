using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UC_alert_tool.Controllers
{
    public class TestersController : Controller
    { 
        [Authorize]
        public ActionResult emailtester()
        {
            return View();
        }
        public ActionResult smstester()
        {
            return View();
        }
    }
}