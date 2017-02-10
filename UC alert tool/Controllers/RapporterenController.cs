﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Melding(Storingen model)
        {
            if (ModelState.IsValid)
            {
                Storingen storing = new Storingen { Titel = model.Titel, Inhoud = model.Inhoud, Begindatum = model.Begindatum, Begintijd = model.Begintijd, Einddatum = model.Einddatum, Eindtijd = model.Eindtijd, IsGesloten = model.IsGesloten,ProductID = model.ProductID };
                db.Storingen.Add(storing);
                db.SaveChanges();
                return RedirectToAction("Index", "home");
            }
            else
            {
                return View(model);
            }
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