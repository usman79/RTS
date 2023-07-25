using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RTS.Models;

namespace MobileApi.Controllers
{
    public class ReportsController : Controller
    {
        private RTSDBEntities db = new RTSDBEntities();
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }

        // GET: Reports/Details/5
        public ActionResult DateReportFilter()
        {
            if ((string)Session["role"] == "admin")
            {
                ViewBag.id = new SelectList(db.SubDivisions, "id", "subdivision_desc");
                return View();
            }

                int townId = (int)Session["officeId"];
            if((string)Session["role"] == "ddr") {
                ViewBag.id = new SelectList(db.SubDivisions.Where(x => x.Town_Id == townId), "id", "subdivision_desc");
            }
            else if ((string)Session["role"] == "admin")
            {
                ViewBag.id = new SelectList(db.SubDivisions, "id", "subdivision_desc");
            }
           
            return View();
        }
        [HttpPost]
        public ActionResult DateReport(DateTime startDate, DateTime endDate )
        {
            List<Account> acts = new List<Account>();
            if ((string)Session["role"] == "ddr")
            {
                int townId = (int)Session["officeId"];
    
                acts = db.Accounts.Where(x => x.visit_date >= startDate && x.visit_date <= endDate&&x.statusId!=1 && x.Ward.SubDivision.Town_Id==townId  ).ToList();
            }
            else if ((string)Session["role"] == "admin")
            {
                acts = db.Accounts.Where(x => x.visit_date >= startDate && x.visit_date <= endDate && x.statusId != 1   ).ToList();
            }
            return View(acts);

        }

            // GET: Reports/Create
            public ActionResult Create()
        {
            return View();
        }

        // POST: Reports/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Reports/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Reports/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Reports/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Reports/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
