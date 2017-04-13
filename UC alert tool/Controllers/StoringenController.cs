using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UC_alert_tool.Models;

namespace UC_alert_tool.Controllers
{
    [Authorize]
    public class StoringenController : Controller
    {
        private alertDatabaseEntities db = new alertDatabaseEntities();

        // GET: Storingen
        public ActionResult Index()
        {
            var storingen = db.Storingen.Include(s => s.Producten);
            return View(storingen.ToList());
        }

        // GET: Storingen/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Storingen storingen = db.Storingen.Find(id);
            if (storingen == null)
            {
                return HttpNotFound();
            }
            return View(storingen);
        }

        // GET: Storingen/Create
        public ActionResult Create()
        {
            ViewBag.ProductID = new SelectList(db.Producten, "Id", "Naam");
            return View();
        }

        // POST: Storingen/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProductID,Titel,Inhoud,EigenaarID,Begindatum,Einddatum,IsGesloten,Begintijd,Eindtijd")] Storingen storingen)
        {
            if (ModelState.IsValid)
            {
                db.Storingen.Add(storingen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductID = new SelectList(db.Producten, "Id", "Naam", storingen.ProductID);
            return View(storingen);
        }

        // GET: Storingen/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Storingen storingen = db.Storingen.Find(id);
            if (storingen == null)
            {
                return HttpNotFound();
            }
            ViewBag.previewSignaturetext = db.Settings.Single(s => s.Setting == "SignatureText").Value;
            ViewBag.previewImage = db.Settings.Single(s => s.Setting == "SignaturePath").Value;
            ViewBag.ProductCustomerCount = string.Join(",", Functions.Email.Information.GetCountOfEmailRecipients()); // this list has the same order as the 'default' product list
            ViewBag.ProductID = new SelectList(db.Producten, "Id", "Naam", storingen.ProductID);
            return View(storingen);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProductID,Titel,Inhoud,EigenaarID,Begindatum,Einddatum,IsGesloten,Begintijd,Eindtijd")] Storingen storingen, string emailtitle, string emailbody, string smsbericht)
        {
           if (ModelState.IsValid)
            {
                db.Entry(storingen).State = EntityState.Modified;
                db.SaveChanges();
                //receive all recipients form db
                var allProducts = db.Producten.ToList();
                var selectedProduct = allProducts[(storingen.ProductID - 1)]; // the selectlist is always in order as in the database - minus 1 because the list startes with a 1 instaid of a 0
                var selectedProductID = selectedProduct.Id;
                var allRecipients = selectedProduct.Klanten2Producten;

                if (smsbericht != "") // email
                {
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
                        text = smsbericht
                    };
                    Hangfire.BackgroundJob.Enqueue(() => Functions.SMS.Sending.sendSMSMessages(totalToSendMessages));
                }
                if (emailbody != "")
                {
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
                        EmailSubject = emailtitle,
                        EmailBody = emailbody,
                        FromEmailAddress = db.Settings.Single(s => s.Setting == "EmailSendingMailAddress").Value,
                        Recipients = allRecipientsOnlyEmailAddress,
                    };
                    Hangfire.BackgroundJob.Enqueue(() => Functions.Email.Sending.sendEmail(mail, true));
                }

                return RedirectToAction("Index", "home");
            }
            ViewBag.ProductID = new SelectList(db.Producten, "Id", "Naam", storingen.ProductID);
            return View(storingen);
        }

        // GET: Storingen/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Storingen storingen = db.Storingen.Find(id);
            if (storingen == null)
            {
                return HttpNotFound();
            }
            return View(storingen);
        }

        // POST: Storingen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Storingen storingen = db.Storingen.Find(id);
            db.Storingen.Remove(storingen);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
