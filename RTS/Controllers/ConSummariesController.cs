using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RTS.Models;

namespace RTS.Controllers
{
    public class ConSummariesController : Controller
    {
        private RTSDBEntities db = new RTSDBEntities();

        // GET: ConSummaries
        public ActionResult Index()
        {
            var conSummaries = db.ConSummaries.Include(c => c.Town);
            return View(conSummaries.ToList());
        }

        // GET: ConSummaries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConSummary conSummary = db.ConSummaries.Find(id);
            if (conSummary == null)
            {
                return HttpNotFound();
            }
            return View(conSummary);
        }

        // GET: ConSummaries/Create
        public ActionResult Create()
        {
            ViewBag.townid = new SelectList(db.Towns, "Id", "town_desc");
            return View();
        }

        // POST: ConSummaries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,townid,domestic,commercial,cheritable,reading,average,water,sewer,water_and_sewer,aquifer_water,aquifer_sewer")] ConSummary conSummary)
        {
            if (ModelState.IsValid)
            {
                conSummary.entrdate = DateTime.Today;
                db.ConSummaries.Add(conSummary);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.townid = new SelectList(db.Towns, "Id", "town_desc", conSummary.townid);
            return View(conSummary);
        }

        // GET: ConSummaries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConSummary conSummary = db.ConSummaries.Find(id);
            if (conSummary == null)
            {
                return HttpNotFound();
            }
            ViewBag.townid = new SelectList(db.Towns, "Id", "town_desc", conSummary.townid);
            return View(conSummary);
        }

        // POST: ConSummaries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,entrdate,townid,domestic,commercial,cheritable,reading,average,water,sewer,water_and_sewer,aquifer_water,aquifer_sewer")] ConSummary conSummary)
        {
            if (ModelState.IsValid)
            {
                db.Entry(conSummary).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.townid = new SelectList(db.Towns, "Id", "town_desc", conSummary.townid);
            return View(conSummary);
        }

        // GET: ConSummaries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConSummary conSummary = db.ConSummaries.Find(id);
            if (conSummary == null)
            {
                return HttpNotFound();
            }
            return View(conSummary);
        }

        // POST: ConSummaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ConSummary conSummary = db.ConSummaries.Find(id);
            db.ConSummaries.Remove(conSummary);
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
