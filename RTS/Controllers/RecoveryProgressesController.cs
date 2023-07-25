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
    public class RecoveryProgressesController : Controller
    {
        private RTSDBEntities db = new RTSDBEntities();

        // GET: RecoveryProgresses
        public ActionResult Index()
        {
            var recoveryProgresses = db.RecoveryProgresses.OrderByDescending(x=>x.year).ThenByDescending(x=>x.month).Take(120).Include(r => r.Town);
            return View(recoveryProgresses.ToList());
        }

        // GET: RecoveryProgresses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecoveryProgress recoveryProgress = db.RecoveryProgresses.Find(id);
            if (recoveryProgress == null)
            {
                return HttpNotFound();
            }
            return View(recoveryProgress);
        }

        // GET: RecoveryProgresses/Create
        public ActionResult Create()
        {
            ViewBag.townId = new SelectList(db.Towns, "Id", "town_desc");
            return View();
        }

        // POST: RecoveryProgresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,month,year,isComplete,target,recovered,NCR,UCR,MisUse,townId")] RecoveryProgress recoveryProgress)
       {
            if (ModelState.IsValid)
            {
                recoveryProgress.month = DateTime.Today.Month;
                recoveryProgress.year = DateTime.Today.Year;
                db.RecoveryProgresses.Add(recoveryProgress);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.townId = new SelectList(db.Towns, "Id", "town_desc", recoveryProgress.townId);
            return View(recoveryProgress);
        }

        // GET: RecoveryProgresses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecoveryProgress recoveryProgress = db.RecoveryProgresses.Find(id);
            if (recoveryProgress == null)
            {
                return HttpNotFound();
            }
            ViewBag.townId = new SelectList(db.Towns, "Id", "town_desc", recoveryProgress.townId);
            return View(recoveryProgress);
        }

        // POST: RecoveryProgresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,month,year,isComplete,target,recovered,NCR,UCR,MisUse,townId,accountPaid")] RecoveryProgress recoveryProgress)
        {
            if (ModelState.IsValid)
            {
                db.Entry(recoveryProgress).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.townId = new SelectList(db.Towns, "Id", "town_desc", recoveryProgress.townId);
            return View(recoveryProgress);
        }

        // GET: RecoveryProgresses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecoveryProgress recoveryProgress = db.RecoveryProgresses.Find(id);
            if (recoveryProgress == null)
            {
                return HttpNotFound();
            }
            return View(recoveryProgress);
        }

        // POST: RecoveryProgresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RecoveryProgress recoveryProgress = db.RecoveryProgresses.Find(id);
            db.RecoveryProgresses.Remove(recoveryProgress);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult getMonthlyChart()
        {
            var a = db.RecoveryProgresses.Where(x => x.isComplete == true).OrderByDescending(x => x.year).ThenByDescending(x => x.month).Take(8).OrderBy(x => x.townId);
            var month = a.First().month;
            var year = a.First().year;
            var b =   db.RecoveryProgresses.Where(x => x.isComplete == true && x.month == month  && x.year == year-1).Take(8).OrderBy(x=>x.townId);
            List<chart1> c1 = new List<chart1>();
            List<chart1> c2 = new List<chart1>();
            foreach (var x in a)
            {
                chart1 c = new chart1();
                c.year = x.year;
                c.month = x.month;
                c.label = x.Town.town_desc;
                c.y = x.recovered/1000000;
                c1.Add(c);
            }
            foreach (var x in b)
            {
                chart1 c = new chart1();
                c.year = x.year;
                c.month = x.month;
                c.label = x.Town.town_desc;
                c.y = x.recovered / 1000000;
                c2.Add(c);
            }
            if (c2.Count == 0)
            {
                foreach (var x in a)
                {
                    chart1 c = new chart1();
                    c.year = x.year-1;
                    c.month = x.month;
                    c.label = x.Town.town_desc;
                    c.y = 0;
                    c2.Add(c);
                }
            }
            
            List<chart1>[] result = new List<chart1>[] { c1, c2 };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getTargetChart()
        {
            var result = db.RecoveryProgresses
    .Where(x => x.isComplete == true)
    .GroupBy(x => new { x.month, x.year }) // Group by month and year
    .Select(group => new
    {
        group.Key.month,
        group.Key.year,
        TotalRecovered = group.Sum(x => x.recovered) ,
        Target = group.Sum(x => x.target) // Calculate the sum of the "recovered" column for each group
    })
    .OrderByDescending(x => x.year)
    .ThenByDescending(x => x.month)
    .Take(10);
            List<chart1> r = new List<chart1>();
foreach (var item in result)
{
                chart1 c = new chart1();
    c.month = item.month;
    c.year = item.year;
    c.y = item.TotalRecovered/1000000;
                c.t = item.Target / 1000000;
                r.Add(c);
    
}
 
  return Json(r.OrderBy(x=>x.year).ThenBy(x=>x.month), JsonRequestBehavior.AllowGet);
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
    public class chart1
    {
        public string label;
        public int y;
        public int t;
        public int year;
        public int month;
    }
}
