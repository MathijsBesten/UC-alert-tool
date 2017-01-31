﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UC_alert_tool.Models;

namespace UC_alert_tool.Controllers
{
    [Authorize]
    public class RapporterenController : Controller
    {
        private alertDatabaseEntities db = new alertDatabaseEntities();

        // GET: Rapporteren
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Melding()
        {
            ViewBag.ProductID = new SelectList(db.Producten, "Id", "Naam");
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