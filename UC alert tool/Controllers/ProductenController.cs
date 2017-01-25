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
    public class ProductenController : Controller
    {
        private alertDatabaseEntities db = new alertDatabaseEntities();

        // GET: Producten
        public ActionResult Index()
        {
            return View(db.Producten.ToList());
        }

        // GET: Producten/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producten producten = db.Producten.Find(id);
            if (producten == null)
            {
                return HttpNotFound();
            }
            return View(producten);
        }

        // GET: Producten/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Producten/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Naam,HelpdeskID")] Producten producten)
        {
            if (ModelState.IsValid)
            {
                db.Producten.Add(producten);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(producten);
        }

        // GET: Producten/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producten producten = db.Producten.Find(id);
            if (producten == null)
            {
                return HttpNotFound();
            }
            return View(producten);
        }

        // POST: Producten/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Naam,HelpdeskID")] Producten producten)
        {
            if (ModelState.IsValid)
            {
                db.Entry(producten).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(producten);
        }

        // GET: Producten/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producten producten = db.Producten.Find(id);
            if (producten == null)
            {
                return HttpNotFound();
            }
            return View(producten);
        }

        // POST: Producten/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Producten producten = db.Producten.Find(id);
            db.Producten.Remove(producten);
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
