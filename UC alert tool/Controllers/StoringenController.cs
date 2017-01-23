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
        private AlertToolContext db = new AlertToolContext();

        // GET: Storingen
        public ActionResult Index()
        {
            return View(db.Storingen.ToList());
        }

        // GET: Storingen/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Storing storing = db.Storingen.Find(id);
            if (storing == null)
            {
                return HttpNotFound();
            }
            return View(storing);
        }

        // GET: Storingen/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Storingen/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,productID,Titel,inhoud,eigenaarID,begindatum,einddatum,isGesloten")] Storing storing)
        {
            if (ModelState.IsValid)
            {
                db.Storingen.Add(storing);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(storing);
        }

        // GET: Storingen/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Storing storing = db.Storingen.Find(id);
            if (storing == null)
            {
                return HttpNotFound();
            }
            return View(storing);
        }

        // POST: Storingen/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,productID,Titel,inhoud,eigenaarID,begindatum,einddatum,isGesloten")] Storing storing)
        {
            if (ModelState.IsValid)
            {
                db.Entry(storing).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(storing);
        }

        // GET: Storingen/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Storing storing = db.Storingen.Find(id);
            if (storing == null)
            {
                return HttpNotFound();
            }
            return View(storing);
        }

        // POST: Storingen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Storing storing = db.Storingen.Find(id);
            db.Storingen.Remove(storing);
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
