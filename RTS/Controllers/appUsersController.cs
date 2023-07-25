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
    public class appUsersController : Controller
    {
        private RTSDBEntities db = new RTSDBEntities();

        // GET: appUsers
        public ActionResult Index()
        {
            var appUsers = db.appUsers.OrderBy(x=>x.ticket).Include(a => a.AFI);
            return View(appUsers.ToList());
        }

        // GET: appUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            appUser appUser = db.appUsers.Find(id);
            if (appUser == null)
            {
                return HttpNotFound();
            }
            return View(appUser);
        }

        // GET: appUsers/Create
        public ActionResult Create()
        {
            appUser appUser = new appUser();
              appUser.id = db.appUsers.OrderByDescending(u => u.id).FirstOrDefault().id +1;
            ViewBag.afi_id = new SelectList(db.AFIs.OrderByDescending(x => x.Id), "Id", "name");
            return View(appUser);
        }

        // POST: appUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,ticket,password,role,afi_id,rfi_id")] appUser appUser)
        {
            if (ModelState.IsValid)
            {
                db.appUsers.Add(appUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.afi_id = new SelectList(db.AFIs, "Id", "name", appUser.afi_id);
            return View(appUser);
        }

        // GET: appUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            appUser appUser = db.appUsers.Find(id);
            if (appUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.afi_id = new SelectList(db.AFIs, "Id", "name", appUser.afi_id);
            return View(appUser);
        }

        // POST: appUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,ticket,password,role,afi_id,rfi_id")] appUser appUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.afi_id = new SelectList(db.AFIs, "Id", "name", appUser.afi_id);
            return View(appUser);
        }

        // GET: appUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            appUser appUser = db.appUsers.Find(id);
            if (appUser == null)
            {
                return HttpNotFound();
            }
            return View(appUser);
        }

        // POST: appUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            appUser appUser = db.appUsers.Find(id);
            db.appUsers.Remove(appUser);
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
