﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
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
            Storingen model = new Storingen();
            model.Begindatum = DateTime.Now;
            model.Begintijd = DateTime.Now.TimeOfDay;
            ViewBag.ProductID = new SelectList(db.Producten, "Id", "Naam");
            ViewBag.ProductCustomerCount = Functions.Email.Information.GetCountOfEmailRecipients(); // this list has the same order as the 'default' product list
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Melding(Storingen model)
        {
            ViewBag.dateNow = DateTime.Now;
            if (model.Begindatum > model.Einddatum || (model.Begindatum == model.Einddatum && model.Begintijd > model.Eindtijd)) // if startdate is greater than startdate - including time
            {
                ViewBag.ProductID = new SelectList(db.Producten, "Id", "Naam");
                return View(model);
            }
            if (model.IsGesloten == true && model.Einddatum == null) // user cannot close storing if there is no end date
            {
                ViewBag.ProductID = new SelectList(db.Producten, "Id", "Naam");
                return View(model);
            }
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
            rapporterenMetSMS model = new rapporterenMetSMS();
            model.Begindatum = DateTime.Now;
            model.Begintijd = DateTime.Now.TimeOfDay;
            ViewBag.ProductID = new SelectList(db.Producten, "Id", "Naam");
            return View(model);
        }
        [HttpPost]
        public ActionResult MeldingMetSMS(rapporterenMetSMS model)
        {
            if (ModelState.IsValid)
            {
                Storingen storing = new Storingen { Titel = model.Titel, Inhoud = model.Inhoud, Begindatum = model.Begindatum, Begintijd = model.Begintijd, Einddatum = model.Einddatum, Eindtijd = model.Eindtijd, IsGesloten = model.IsGesloten, ProductID = model.ProductID };
                db.Storingen.Add(storing);
                db.SaveChanges();

                //receive all recipients form db
                var allProducts = db.Producten.ToList();
                var selectedProduct = allProducts[(model.ProductID - 1)]; // the selectlist is always in order as in the database - minus 1 because the list startes with a 1 instaid of a 0
                var selectedProductID = selectedProduct.Id;
                var allRecipients = selectedProduct.Klanten2Producten;
                var allRecipientsOnlySMSNumber = new List<string>();
                foreach (var item in allRecipients)
                {
                    string email = item.Klanten.Telefoonnummer;
                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        allRecipientsOnlySMSNumber.Add(item.Klanten.Telefoonnummer);
                    }
                }
                SMS totalToSendMessages = new SMS
                {
                    Recipients = allRecipientsOnlySMSNumber,
                    text = model.smsbericht
                };
                Hangfire.BackgroundJob.Enqueue(() => Functions.SMS.Sending.sendSMSMessages(totalToSendMessages));
                return RedirectToAction("Index", "home");
            }
            else
            {
                return View(model);
            }

        }

        public ActionResult MeldingMetEmail()
        {
            rapporterenMetEmail model = new rapporterenMetEmail();
            model.Begindatum = DateTime.Now;
            model.Begintijd = DateTime.Now.TimeOfDay;
            ViewBag.ProductID = new SelectList(db.Producten, "Id", "Naam");
            ViewBag.previewSignaturetext = db.Settings.Single(s => s.Setting == "SignatureText").Value;
            ViewBag.previewImage = db.Settings.Single(s => s.Setting == "SignaturePath").Value;
            return View(model);
        }
        [HttpPost]
        public ActionResult MeldingMetEmail(rapporterenMetEmail model)
        {
            if (ModelState.IsValid)
            {
                Storingen storing = new Storingen { Titel = model.Titel, Inhoud = model.Inhoud , Begindatum = model.Begindatum, Begintijd = model.Begintijd, Einddatum = model.Einddatum, Eindtijd = model.Eindtijd, IsGesloten = model.IsGesloten, ProductID = model.ProductID };
                db.Storingen.Add(storing);
                db.SaveChanges();

                //receive all recipients form db
                var allProducts = db.Producten.ToList();
                var selectedProduct = allProducts[(model.ProductID - 1)]; // the selectlist is always in order as in the database - minus 1 because the list startes with a 1 instaid of a 0
                var selectedProductID = selectedProduct.Id;
                var allRecipients = selectedProduct.Klanten2Producten;
                var allRecipientsOnlyEmailAddress = new List<string>();
                foreach (var item in allRecipients)
                {
                    string email = item.Klanten.PrimaireEmail;
                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        allRecipientsOnlyEmailAddress.Add(item.Klanten.PrimaireEmail);
                    }
                }
                //here is space for saving the current recipients to a database

                //make new email
                email mail = new email();
                mail.EmailSubject = model.emailtitle;
                mail.EmailBody = model.emailbody;
                mail.FromEmailAddress = db.Settings.Single(s => s.Setting == "EmailSendingMailAddress").Value;
                mail.Recipients = allRecipientsOnlyEmailAddress;
                mail.SMTPServerURL = db.Settings.Single(s => s.Setting == "EmailServerIP").Value + ":" + db.Settings.Single(s => s.Setting == "EmailServerPort").Value;
                Hangfire.BackgroundJob.Enqueue(() => Functions.Email.Sending.sendEmail(mail, true));

                return RedirectToAction("Index", "home");
            }
            else
            {
                return View(model);
            }
        }
        public ActionResult MeldingMetEmailEnSMS()
        {
            rapporterenMetEmailenSMS model = new rapporterenMetEmailenSMS();
            model.Begindatum = DateTime.Now;
            model.Begintijd = DateTime.Now.TimeOfDay;
            ViewBag.ProductID = new SelectList(db.Producten, "Id", "Naam");
            ViewBag.previewSignaturetext = db.Settings.Single(s => s.Setting == "SignatureText").Value;
            ViewBag.previewImage = db.Settings.Single(s => s.Setting == "SignaturePath").Value;
            return View(model);
        }
    }
}