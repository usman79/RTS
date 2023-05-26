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
    public class SubDivisionsController : Controller
    {
        private RTSDBEntities db = new RTSDBEntities();

        // GET: SubDivisions
        public ActionResult Index()
        {
            var subDivisions = db.SubDivisions.Include(s => s.Town);
            return View(subDivisions.ToList());
        }

        // GET: SubDivisions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubDivision subDivision = db.SubDivisions.Find(id);
            if (subDivision == null)
            {
                return HttpNotFound();
            }
            return View(subDivision);
        }

        // GET: SubDivisions/Create
        public ActionResult Create()
        {
            ViewBag.Town_Id = new SelectList(db.Towns, "Id", "town_desc");
            return View();
        }

        // POST: SubDivisions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,subdivision_desc,Town_Id")] SubDivision subDivision)
        {
            if (ModelState.IsValid)
            {
                db.SubDivisions.Add(subDivision);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Town_Id = new SelectList(db.Towns, "Id", "town_desc", subDivision.Town_Id);
            return View(subDivision);
        }

        // GET: SubDivisions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubDivision subDivision = db.SubDivisions.Find(id);
            if (subDivision == null)
            {
                return HttpNotFound();
            }
            ViewBag.Town_Id = new SelectList(db.Towns, "Id", "town_desc", subDivision.Town_Id);
            return View(subDivision);
        }

        // POST: SubDivisions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,subdivision_desc,Town_Id")] SubDivision subDivision)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subDivision).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Town_Id = new SelectList(db.Towns, "Id", "town_desc", subDivision.Town_Id);
            return View(subDivision);
        }

        // GET: SubDivisions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubDivision subDivision = db.SubDivisions.Find(id);
            if (subDivision == null)
            {
                return HttpNotFound();
            }
            return View(subDivision);
        }

        // POST: SubDivisions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubDivision subDivision = db.SubDivisions.Find(id);
            db.SubDivisions.Remove(subDivision);
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
