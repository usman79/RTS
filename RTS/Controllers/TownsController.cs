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
    public class TownsController : Controller
    {
        private RTSDBEntities db = new RTSDBEntities();

        // GET: Towns
        public ActionResult Index()
        {
            return View(db.Towns.ToList());
        }

        // GET: Towns/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Town town = db.Towns.Find(id);
            if (town == null)
            {
                return HttpNotFound();
            }
            return View(town);
        }

        // GET: Towns/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Towns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,town_desc")] Town town)
        {
            if (ModelState.IsValid)
            {
                db.Towns.Add(town);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(town);
        }

        // GET: Towns/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Town town = db.Towns.Find(id);
            if (town == null)
            {
                return HttpNotFound();
            }
            return View(town);
        }

        // POST: Towns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,town_desc")] Town town)
        {
            if (ModelState.IsValid)
            {
                db.Entry(town).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(town);
        }

        // GET: Towns/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Town town = db.Towns.Find(id);
            if (town == null)
            {
                return HttpNotFound();
            }
            return View(town);
        }
        public ActionResult GetAfi(int? id)
        {
            var list = db.Wards.Where(x => x.SubDivision.Town_Id == id);


            var afis = list.Where(x=>x.AFI!=null).Select(x => x.AFI).ToList();
         
             
            return View("AFI", afis);
        }
        // POST: Towns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Town town = db.Towns.Find(id);
            db.Towns.Remove(town);
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
