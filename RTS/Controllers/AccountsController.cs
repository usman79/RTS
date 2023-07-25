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
    public class AccountsController : Controller
    {
        private RTSDBEntities db = new RTSDBEntities();

        // GET: Accounts
        public ActionResult Index()
        {
            var accounts = db.Accounts.Where(a=>a.Ward.SubDivision.Town_Id==7).Include(a => a.Ward).Include(a => a.accountStatu);
            return View(accounts.ToList());
        }

        // GET: Accounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: Accounts/Create
        public ActionResult Create()
        {
            ViewBag.Ward_Id = new SelectList(db.Wards, "Id", "ward_desc");
            ViewBag.statusId = new SelectList(db.accountStatus, "id", "value");
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,amount_due,longitude,latitude,Ward_Id,address,walk_sort,Consumer_Name,last_payment,last_payment_date,Account_No,statusId,Account_Type,Connection_Date,Current_Demand,propertyNo,visit_date,visit_lat,visit_long,remarks,is_assigned,assignment_date,Image,partialPayment")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Accounts.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Ward_Id = new SelectList(db.Wards, "Id", "ward_desc", account.Ward_Id);
            ViewBag.statusId = new SelectList(db.accountStatus, "id", "value", account.statusId);
            return View(account);
        }

        // GET: Accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            ViewBag.Ward_Id = new SelectList(db.Wards, "Id", "ward_desc", account.Ward_Id);
            ViewBag.statusId = new SelectList(db.accountStatus, "id", "value", account.statusId);
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,amount_due,longitude,latitude,Ward_Id,address,walk_sort,Consumer_Name,last_payment,last_payment_date,Account_No,statusId,Account_Type,Connection_Date,Current_Demand,propertyNo,visit_date,visit_lat,visit_long,remarks,is_assigned,assignment_date,Image,partialPayment")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Ward_Id = new SelectList(db.Wards, "Id", "ward_desc", account.Ward_Id);
            ViewBag.statusId = new SelectList(db.accountStatus, "id", "value", account.statusId);
            return View(account);
        }

        // GET: Accounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Account account = db.Accounts.Find(id);
            db.Accounts.Remove(account);
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
