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
    public class marqueesController : Controller
    {
        private RTSDBEntities db = new RTSDBEntities();

        // GET: marquees
        public ActionResult Index()
        {
            return View(db.marquees.ToList());
        }

        // GET: marquees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            marquee marquee = db.marquees.Find(id);
            if (marquee == null)
            {
                return HttpNotFound();
            }
            return View(marquee);
        }

        // GET: marquees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: marquees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,value")] marquee marquee)
        {
            if (ModelState.IsValid)
            {
                db.marquees.Add(marquee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(marquee);
        }

        // GET: marquees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            marquee marquee = db.marquees.Find(id);
            if (marquee == null)
            {
                return HttpNotFound();
            }
            return View(marquee);
        }

        // POST: marquees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,value")] marquee marquee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(marquee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(marquee);
        }

        // GET: marquees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            marquee marquee = db.marquees.Find(id);
            if (marquee == null)
            {
                return HttpNotFound();
            }
            return View(marquee);
        }

        // POST: marquees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            marquee marquee = db.marquees.Find(id);
            db.marquees.Remove(marquee);
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
