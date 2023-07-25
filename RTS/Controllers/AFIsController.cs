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
    public class AFIsController : Controller
    {
        private RTSDBEntities db = new RTSDBEntities();

        // GET: AFIs
        public ActionResult Index()
        {
            var aFIs = db.AFIs ;
            return View(aFIs.ToList());
        }

        // GET: AFIs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AFI aFI = db.AFIs.Find(id);
            if (aFI == null)
            {
                return HttpNotFound();
            }
            return View(aFI);
        }

        // GET: AFIs/Create
        public ActionResult Create()
        {
            ViewBag.Ward_Id = new SelectList(db.Wards, "Id", "ward_desc");
            return View();
        }

        // POST: AFIs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,name,ticket,phone,longitude,latitude ")] AFI aFI)
        {
            if (ModelState.IsValid)
            {
                db.AFIs.Add(aFI);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

        //    ViewBag.Ward_Id = new SelectList(db.Wards, "Id", "ward_desc", aFI.Ward_Id);
            return View(aFI);
        }

        // GET: AFIs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AFI aFI = db.AFIs.Find(id);
            if (aFI == null)
            {
                return HttpNotFound();
            }
           // ViewBag.Ward_Id = new SelectList(db.Wards, "Id", "ward_desc", aFI.Ward_Id);
            return View(aFI);
        }

        // POST: AFIs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,name,ticket,phone,longitude,latitude,Ward_Id")] AFI aFI)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aFI).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           // ViewBag.Ward_Id = new SelectList(db.Wards, "Id", "ward_desc", aFI.Ward_Id);
            return View(aFI);
        }

        // GET: AFIs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AFI aFI = db.AFIs.Find(id);
            if (aFI == null)
            {
                return HttpNotFound();
            }
            return View(aFI);
        }

        // POST: AFIs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AFI aFI = db.AFIs.Find(id);
            db.AFIs.Remove(aFI);
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
