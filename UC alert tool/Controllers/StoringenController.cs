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
            ViewBag.ProductID = new SelectList(db.Producten, "Id", "Naam", storingen.ProductID);
            return View(storingen);
        }

        // POST: Storingen/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProductID,Titel,Inhoud,EigenaarID,Begindatum,Einddatum,IsGesloten,Begintijd,Eindtijd")] Storingen storingen)
        {
            if (ModelState.IsValid)
            {
                db.Entry(storingen).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
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
