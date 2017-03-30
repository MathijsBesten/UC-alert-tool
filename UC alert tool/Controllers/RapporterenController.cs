using log4net;
using System;
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
        private static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
            ViewBag.ProductCustomerCount = string.Join(",", Functions.Email.Information.GetCountOfEmailRecipients()); // this list has the same order as the 'default' product list
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
                log.Info("user created a report");
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
            ViewBag.ProductCustomerCount = string.Join(",", Functions.Email.Information.GetCountOfEmailRecipients()); // this list has the same order as the 'default' product list
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
                log.Info("user created a report");

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
            ViewBag.ProductCustomerCount = string.Join(",", Functions.Email.Information.GetCountOfEmailRecipients()); // this list has the same order as the 'default' product list
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
                log.Info("user created a report");

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
                email mail = new email
                {
                    EmailSubject = model.emailtitle,
                    EmailBody = model.emailbody,
                    FromEmailAddress = db.Settings.Single(s => s.Setting == "EmailSendingMailAddress").Value,
                    Recipients = allRecipientsOnlyEmailAddress,
                };
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
            ViewBag.ProductCustomerCount = string.Join(",", Functions.Email.Information.GetCountOfEmailRecipients()); // this list has the same order as the 'default' product list
            return View(model);
        }
        [HttpPost]
        public ActionResult MeldingMetEmailEnSMS(rapporterenMetEmailenSMS model)
        {
            if (ModelState.IsValid)
            {
                //save storing to database
                Storingen storing = new Storingen { Titel = model.Titel, Inhoud = model.Inhoud, Begindatum = model.Begindatum, Begintijd = model.Begintijd, Einddatum = model.Einddatum, Eindtijd = model.Eindtijd, IsGesloten = model.IsGesloten, ProductID = model.ProductID };
                db.Storingen.Add(storing);
                db.SaveChanges();

                //receive all recipients form db
                var allProducts = db.Producten.ToList();
                var selectedProduct = allProducts[(model.ProductID - 1)]; // the selectlist is always in order as in the database - minus 1 because the list startes with a 1 instaid of a 0
                var selectedProductID = selectedProduct.Id;
                var allRecipients = selectedProduct.Klanten2Producten;
                var allRecipientsOnlySMSNumber = new List<string>();
                var allRecipientsOnlyEmailAddress = new List<string>();

                //fill list with people who wants to receive sms message
                foreach (var item in allRecipients)
                {
                    string email = item.Klanten.Telefoonnummer;
                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        allRecipientsOnlySMSNumber.Add(item.Klanten.Telefoonnummer);
                    }
                }
                // fill the list with people who wants to receive a email
                foreach (var item in allRecipients)
                {
                    string email = item.Klanten.PrimaireEmail;
                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        allRecipientsOnlyEmailAddress.Add(item.Klanten.PrimaireEmail);
                    }
                }

                SMS totalToSendMessages = new SMS  // make new sms
                {
                    Recipients = allRecipientsOnlySMSNumber,
                    text = model.smsbericht
                };

                //make new email
                email mail = new email
                {
                    EmailSubject = model.emailtitle,
                    EmailBody = model.emailbody,
                    FromEmailAddress = db.Settings.Single(s => s.Setting == "EmailSendingMailAddress").Value,
                    Recipients = allRecipientsOnlyEmailAddress,
                };

                //send email and sms messages async
                Hangfire.BackgroundJob.Enqueue(() => Functions.SMS.Sending.sendSMSMessages(totalToSendMessages));
                Hangfire.BackgroundJob.Enqueue(() => Functions.Email.Sending.sendEmail(mail, true));
                return RedirectToAction("Index", "home");

            }
            else
            {
                return View(model);
            }
        }

        public string recipientSMSCount(string productname)
        {
            return Functions.Recipients.SMS.Get.getTotalCountRecipients(productname).ToString();
        }
        public string recipientEmailCount(string productname)
        {
            return Functions.Recipients.Email.Get.getTotalCountRecipients(productname).ToString();
        }
    }
}