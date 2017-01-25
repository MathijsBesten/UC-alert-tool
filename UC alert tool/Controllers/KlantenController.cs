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
    public class KlantenController : Controller
    {
        private alertDatabaseEntities db = new alertDatabaseEntities();

        // GET: Klanten
        public ActionResult Index()
        {
            return View(db.Klanten.ToList());
        }

        // GET: Klanten/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Klanten klanten = db.Klanten.Find(id);
            if (klanten == null)
            {
                return HttpNotFound();
            }
            return View(klanten);
        }

        // GET: Klanten/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Klanten/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Naam,PrimaireEmail,SecundaireEmail,Telefoonnummer")] Klanten klanten)
        {
            if (ModelState.IsValid)
            {
                db.Klanten.Add(klanten);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(klanten);
        }

        // GET: Klanten/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Klanten klanten = db.Klanten.Find(id);
            if (klanten == null)
            {
                return HttpNotFound();
            }
            return View(klanten);
        }

        // POST: Klanten/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Naam,PrimaireEmail,SecundaireEmail,Telefoonnummer")] Klanten klanten)
        {
            if (ModelState.IsValid)
            {
                db.Entry(klanten).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(klanten);
        }

        // GET: Klanten/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Klanten klanten = db.Klanten.Find(id);
            if (klanten == null)
            {
                return HttpNotFound();
            }
            return View(klanten);
        }

        // POST: Klanten/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Klanten klanten = db.Klanten.Find(id);
            db.Klanten.Remove(klanten);
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
