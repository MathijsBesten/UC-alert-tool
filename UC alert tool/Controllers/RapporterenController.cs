using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UC_alert_tool.Controllers
{
    [Authorize]
    public class RapporterenController : Controller
    {
        // GET: Rapporteren
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Melding()
        {
            return View();
        }
        public ActionResult MeldingMetSMS()
        {
            return View();
        }
        public ActionResult MeldingMetEmail()
        {
            return View();
        }
        public ActionResult MeldingMetEmailEnSMS()
        {
            return View();
        }
    }
}