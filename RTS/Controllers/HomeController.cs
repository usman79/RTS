using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using RTS.Models;

namespace RTS.Controllers
{
    public class HomeController : Controller
    {
        private RTSDBEntities db = new RTSDBEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TownDashboard(int id)
        {
            //ViewBag.Message = "Your application description page.";
            var list = db.SubDivisions.Where(x => x.Town_Id == id).ToList();

            return View(list);
        }
        public ActionResult qu()
        {
            var result = db.Accounts
    .GroupBy(x => x.Ward.SubDivision.Town_Id)
    .Select(group => new
    {
        Town_Id = group.Key,
        TotalAmountDue = group.Sum(x => x.amount_due)
    })
    .ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCardData()
        {
            var towns = db.Towns;
            List<DashBoardCard> cards = new List<DashBoardCard>();
            foreach (var item in towns)
            {
                DashBoardCard card = new DashBoardCard();
                card.townId = item.Id;
                card.visited = db.Accounts.Where(x => x.Ward.SubDivision.Town_Id == item.Id && x.statusId != 1).Count();
                card.total = db.Accounts.Where(x => x.Ward.SubDivision.Town_Id == item.Id).Count();
                cards.Add(card);

            }
            return Json(cards, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getPieData()
        {
            List<decimal> values = new List<decimal>();
            //  values.Add( db.Accounts.Count());
            var total = (decimal)db.Accounts.Where(x => x.statusId != 1).Count();


            // values.Add((decimal)db.Accounts.Where(x => x.statusId == 1).Count() / total * 100m);
            values.Add((decimal)db.Accounts.Where(x => x.statusId == 8).Count() / total * 100m);
            values.Add((decimal)db.Accounts.Where(x => x.statusId == 3).Count() / total * 100m);
            values.Add((decimal)db.Accounts.Where(x => x.statusId == 4).Count() / total * 100m);
            values.Add((decimal)db.Accounts.Where(x => x.statusId == 5).Count() / total * 100m);
            values.Add((decimal)db.Accounts.Where(x => x.statusId == 6).Count() / total * 100m);
            values.Add((decimal)db.Accounts.Where(x => x.statusId == 7).Count() / total * 100m);
            values.Add((decimal)db.Accounts.Where(x => x.statusId == 9).Count() / total * 100m);
            values.Add((decimal)db.Accounts.Where(x => x.statusId == 10).Count() / total * 100m);

            return Json(values, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getChartData()
        {

            int[] d = { 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] v = { 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] r = { 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int i = 1; i < 9; i++)
            {
                d[i - 1] = db.Accounts.Where(x => x.Ward.SubDivision.Town_Id == i).Count();
            }
            for (int i = 1; i < 9; i++)
            {
                v[i - 1] = db.Accounts.Where(x => x.statusId != 1 && x.Ward.SubDivision.Town_Id == i).Count();
            }
            for (int i = 1; i < 9; i++)
            {
                r[i - 1] = db.Accounts.Where(x => x.statusId == 8 && x.Ward.SubDivision.Town_Id == i).Count();
            }
            var result = new { d, v, r };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getRecentActivity()
        {

            var acts = db.Accounts.Where((x) => x.statusId != 1).OrderByDescending(x => x.visit_date).Take(4).ToList();
            List<Activity> result = new List<Activity>();

            foreach (var act in acts)
            {
                Activity obj = new Activity();
                if (act.statusId == 10)
                {
                    obj.text = act.Ward.AFI.name + " Lodged <span style='color:darkred'>F.I.R</span>  against " + act.Account_No +
                        " Of Ward No." + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc;
                }
                else if (act.statusId == 12)
                {
                    obj.text = act.Ward.AFI.name + " Updated Status Of " + act.Account_No +
                        " Of Ward No." + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc + " As <span style='color:blue'>Open-Plot</span>";
                }
                else if (act.statusId == 13)
                {
                    obj.text = act.Ward.AFI.name + " Updated Status Of " + act.Account_No +
                        " Of Ward No." + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc + " As <span style='color:blue'>Demolished</span>";
                }
                else if (act.statusId == 14)
                {
                    obj.text = act.Ward.AFI.name + " Updated Status Of " + act.Account_No +
                        " Of Ward No." + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc + " As <span style='color:blue'>Property-Locked</span>";
                }
                else if (act.statusId == 9)
                {
                    obj.text = act.Ward.AFI.name + " Updated Status Of " + act.Account_No +
                        " Of Ward No." + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc + " As <span style='color:blue'>Un-Traceable</span>";
                }
                else if (act.statusId == 8)
                {
                    obj.text = act.Ward.AFI.name + " <span style='color:green'>Recovered</span>  Rs. " + act.amount_due + " From A/C No."
                         + act.Account_No + " Of Ward No." + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc;

                }
                else if (act.statusId == 3)
                {
                    obj.text = act.Ward.AFI.name + "<span style='color:red'> Sealed TubeWell</span>  of Customer No." + act.Account_No + " Of Ward No."
                        + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc;
                }
                else if (act.statusId == 11)
                {
                    obj.text = act.Ward.AFI.name + "<span style='color:green'> Partialy Recovered</span> Rs. " + act.partialPayment + " From Customer No."
                        + act.Account_No + " Of Ward No."
                        + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc; ;
                }
                else if (act.statusId == 4)
                {
                    obj.text = act.Ward.AFI.name + "<span style='color:#cb8402'> made installment </span>against Customer No."
                        + act.Account_No + " Of Ward No."
                        + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc; ;
                }
                else if (act.statusId == 5)
                {
                    obj.text = act.Ward.AFI.name + " Dissconected Water Of Customer No."
                         + act.Account_No + " Of Ward No." + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc;
                }
                else if (act.statusId == 6)
                {
                    obj.text = act.Ward.AFI.name + " Discontinued Sanitation Services Of Customer No." + act.Account_No
                        + " Of Ward No." + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc;
                }
                else if (act.statusId == 7)
                {
                    obj.text = act.Ward.AFI.name + " Updated Status Of Customer No." + act.Account_No +
                        " Of Ward No." + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc + " As Court Case";
                }

                TimeSpan span = (DateTime.Now).Subtract((DateTime)act.visit_date);
                if (span.Hours > 0)
                {
                    obj.time = span.Hours.ToString() + " hrs";
                }
                else
                    obj.time = span.Minutes.ToString() + " Min";
                result.Add(obj);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        ////////////////////////////////Town ////////////////////////
        public ActionResult SubDivisionDashboard(int id)
        {
            //ViewBag.Message = "Your application description page.";
            var list = db.Wards.Where(x => x.SubDivision_Id == id).ToList();

            return View(list);
        }
        public ActionResult townRecentActivity(int id)
        {

            var acts = db.Accounts.Where((x) => x.statusId != 1 && x.Ward.SubDivision.Town_Id == id).OrderByDescending(x => x.visit_date).Take(4).ToList();
            List<Activity> result = new List<Activity>();

            foreach (var act in acts)
            {
                Activity obj = new Activity();
                if (act.statusId == 10)
                {
                    obj.text = act.Ward.AFI.name + " Lodged <span style='color:darkred'>F.I.R</span>  against " + act.Account_No +
                        " Of Ward No." + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc;
                }
                else if (act.statusId == 12)
                {
                    obj.text = act.Ward.AFI.name + " Updated Status Of " + act.Account_No +
                        " Of Ward No." + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc + " As <span style='color:blue'>Open-Plot</span>";
                }
                else if (act.statusId == 13)
                {
                    obj.text = act.Ward.AFI.name + " Updated Status Of " + act.Account_No +
                        " Of Ward No." + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc + " As <span style='color:blue'>Demolished</span>";
                }
                else if (act.statusId == 14)
                {
                    obj.text = act.Ward.AFI.name + " Updated Status Of " + act.Account_No +
                        " Of Ward No." + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc + " As <span style='color:blue'>Property-Locked</span>";
                }
                else if (act.statusId == 9)
                {
                    obj.text = act.Ward.AFI.name + " Updated Status Of " + act.Account_No +
                        " Of Ward No." + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc + " As <span style='color:blue'>Un-Traceable</span>";
                }
                else if (act.statusId == 8)
                {
                    obj.text = act.Ward.AFI.name + " <span style='color:green'>Recovered</span>  Rs. " + act.amount_due + " From A/C No."
                         + act.Account_No + " Of Ward No." + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc;

                }
                else if (act.statusId == 3)
                {
                    obj.text = act.Ward.AFI.name + "<span style='color:red'> Sealed Tubewell</span>  of Consumer No." + act.Account_No + " Of Ward No."
                        + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc;
                }
                else if (act.statusId == 11)
                {
                    obj.text = act.Ward.AFI.name + "<span style='color:green'> Partialy Recovered</span> Rs. " + act.partialPayment + " From Consumer No."
                        + act.Account_No + " Of Ward No."
                        + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc; ;
                }
                else if (act.statusId == 4)
                {
                    obj.text = act.Ward.AFI.name + "<span style='color:#cb8402'> made installment  against Consumer No."
                        + act.Account_No + " Of Ward No."
                        + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc; ;
                }
                else if (act.statusId == 5)
                {
                    obj.text = act.Ward.AFI.name + " Dissconected Water Of Consumer No."
                         + act.Account_No + " Of Ward No." + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc;
                }
                else if (act.statusId == 6)
                {
                    obj.text = act.Ward.AFI.name + " Blocked Sewer Of Consumer No." + act.Account_No
                        + " Of Ward No." + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc;
                }
                else if (act.statusId == 7)
                {
                    obj.text = act.Ward.AFI.name + " Updated Status Of " + act.Account_No +
                        " Of Ward No." + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc + " As Court Case";
                }

                TimeSpan span = (DateTime.Now).Subtract((DateTime)act.visit_date);
                if (span.Hours > 0)
                {
                    obj.time = span.Hours.ToString() + " hrs";
                }
                else
                    obj.time = span.Minutes.ToString() + " Min";
                result.Add(obj);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult townChartData(int id)
        {

            List<int> d = new List<int>();
            List<int> v = new List<int>();
            List<int> r = new List<int>();
            var sds = db.SubDivisions.Where(x => x.Town_Id == id).ToList();
            foreach (var sd in sds)
            {
                d.Add(db.Accounts.Where(x => x.Ward.SubDivision_Id == sd.Id).Count());
            }
            foreach (var sd in sds)
            {
                v.Add(db.Accounts.Where(x => x.statusId != 1 && x.Ward.SubDivision_Id == sd.Id).Count());
            }
            foreach (var sd in sds)
            {
                r.Add(db.Accounts.Where(x => x.statusId == 8 && x.Ward.SubDivision_Id == sd.Id).Count());
            }
            var result = new { d, v, r };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult townPieData(int id)
        {
            List<decimal> values = new List<decimal>();
            //  values.Add( db.Accounts.Count());
            var total = (decimal)db.Accounts.Where(x => x.statusId != 1 && x.Ward.SubDivision.Town_Id == id).Count();


            // values.Add((decimal)db.Accounts.Where(x => x.statusId == 1).Count() / total * 100m);
            values.Add((decimal)db.Accounts.Where(x => x.statusId == 8 && x.Ward.SubDivision.Town_Id == id).Count() / total * 100m);
            values.Add((decimal)db.Accounts.Where(x => x.statusId == 3 && x.Ward.SubDivision.Town_Id == id).Count() / total * 100m);
            values.Add((decimal)db.Accounts.Where(x => x.statusId == 4 && x.Ward.SubDivision.Town_Id == id).Count() / total * 100m);
            values.Add((decimal)db.Accounts.Where(x => x.statusId == 5 && x.Ward.SubDivision.Town_Id == id).Count() / total * 100m);
            values.Add((decimal)db.Accounts.Where(x => x.statusId == 6 && x.Ward.SubDivision.Town_Id == id).Count() / total * 100m);
            values.Add((decimal)db.Accounts.Where(x => x.statusId == 7 && x.Ward.SubDivision.Town_Id == id).Count() / total * 100m);
            values.Add((decimal)db.Accounts.Where(x => x.statusId == 9 && x.Ward.SubDivision.Town_Id == id).Count() / total * 100m);
            values.Add((decimal)db.Accounts.Where(x => x.statusId == 10 && x.Ward.SubDivision.Town_Id == id).Count() / total * 100m);

            return Json(values, JsonRequestBehavior.AllowGet);
        }
        public ActionResult townCardData(int id)
        {
            var sds = db.SubDivisions.Where(x => x.Town_Id == id);
            List<DashBoardCard> cards = new List<DashBoardCard>();
            foreach (var item in sds)
            {
                DashBoardCard card = new DashBoardCard();
                card.townId = item.Id;
                card.visited = db.Accounts.Where(x => x.Ward.SubDivision_Id == item.Id && x.statusId != 1).Count();
                card.total = db.Accounts.Where(x => x.Ward.SubDivision_Id == item.Id).Count();
                cards.Add(card);

            }
            return Json(cards, JsonRequestBehavior.AllowGet);
        }
        ////////////////////////////////SubDivision ////////////////////////

        public ActionResult sdRecentActivity(int id)
        {

            var acts = db.Accounts.Where((x) => x.statusId != 1 && x.Ward.SubDivision_Id == id).OrderByDescending(x => x.visit_date).Take(4).ToList();
            List<Activity> result = new List<Activity>();

            foreach (var act in acts)
            {
                Activity obj = new Activity();
                if (act.statusId == 10)
                {
                    obj.text = act.Ward.AFI.name + " Lodged <span style='color:darkred'>F.I.R</span>  against " + act.Account_No +
                        " Of Ward No." + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc;
                }
                else if (act.statusId == 12)
                {
                    obj.text = act.Ward.AFI.name + " Updated Status Of " + act.Account_No +
                        " Of Ward No." + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc + " As <span style='color:blue'>Open-Plot</span>";
                }
                else if (act.statusId == 13)
                {
                    obj.text = act.Ward.AFI.name + " Updated Status Of " + act.Account_No +
                        " Of Ward No." + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc + " As <span style='color:blue'>Demolished</span>";
                }
                else if (act.statusId == 14)
                {
                    obj.text = act.Ward.AFI.name + " Updated Status Of " + act.Account_No +
                        " Of Ward No." + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc + " As <span style='color:blue'>Property-Locked</span>";
                }
                else if (act.statusId == 9)
                {
                    obj.text = act.Ward.AFI.name + " Updated Status Of " + act.Account_No +
                        " Of Ward No." + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc + " As <span style='color:blue'>Un-Traceable</span>";
                }
                else if (act.statusId == 8)
                {
                    obj.text = act.Ward.AFI.name + " <span style='color:green'>Recovered</span>  Rs. " + act.amount_due + " From A/C No."
                         + act.Account_No + " Of Ward No." + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc;

                }
                else if (act.statusId == 3)
                {
                    obj.text = act.Ward.AFI.name + "<span style='color:red'> Sealed Tubewell</span>   of Consumer No." + act.Account_No + " Of Ward No."
                        + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc;
                }
                else if (act.statusId == 11)
                {
                    obj.text = act.Ward.AFI.name + "<span style='color:green'> Partialy Recovered</span> Rs. " + act.partialPayment + " From Consumer No."
                        + act.Account_No + " Of Ward No."
                        + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc; ;
                }
                else if (act.statusId == 4)
                {
                    obj.text = act.Ward.AFI.name + "<span style='color:#cb8402'> made installment   against Consumer No."
                        + act.Account_No + " Of Ward No."
                        + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc; ;
                }
                else if (act.statusId == 5)
                {
                    obj.text = act.Ward.AFI.name + " Dissconected Water Of Consumer No."
                         + act.Account_No + " Of Ward No." + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc;
                }
                else if (act.statusId == 6)
                {
                    obj.text = act.Ward.AFI.name + " Blocked Sewer Of Consumer No." + act.Account_No
                        + " Of Ward No." + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc;
                }
                else if (act.statusId == 7)
                {
                    obj.text = act.Ward.AFI.name + " Updated Status Of " + act.Account_No +
                        " Of Ward No." + act.Ward.ward_desc + " In " + act.Ward.SubDivision.Town.town_desc + " As Court Case";
                }

                TimeSpan span = (DateTime.Now).Subtract((DateTime)act.visit_date);
                if (span.Hours > 0)
                {
                    obj.time = span.Hours.ToString() + " hrs";
                }
                else
                    obj.time = span.Minutes.ToString() + " Min";
                result.Add(obj);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult sdChartData(int id)
        {

            List<int> d = new List<int>();
            List<int> v = new List<int>();
            List<int> r = new List<int>();
            var sds = db.Wards.Where(x => x.SubDivision_Id == id).ToList();
            foreach (var sd in sds)
            {
                d.Add(db.Accounts.Where(x => x.Ward_Id == sd.Id).Count());
            }
            foreach (var sd in sds)
            {
                v.Add(db.Accounts.Where(x => x.statusId != 1 && x.Ward_Id == sd.Id).Count());
            }
            foreach (var sd in sds)
            {
                r.Add(db.Accounts.Where(x => x.statusId == 8 && x.Ward_Id == sd.Id).Count());
            }
            var result = new { d, v, r };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult sdPieData(int id)
        {
            List<decimal> values = new List<decimal>();
            //  values.Add( db.Accounts.Count());
            var total = (decimal)db.Accounts.Where(x => x.statusId != 1 && x.Ward.SubDivision_Id == id).Count();


            // values.Add((decimal)db.Accounts.Where(x => x.statusId == 1).Count() / total * 100m);
            values.Add((decimal)db.Accounts.Where(x => x.statusId == 8 && x.Ward.SubDivision_Id == id).Count() / total * 100m);
            values.Add((decimal)db.Accounts.Where(x => x.statusId == 3 && x.Ward.SubDivision_Id == id).Count() / total * 100m);
            values.Add((decimal)db.Accounts.Where(x => x.statusId == 4 && x.Ward.SubDivision_Id == id).Count() / total * 100m);
            values.Add((decimal)db.Accounts.Where(x => x.statusId == 5 && x.Ward.SubDivision_Id == id).Count() / total * 100m);
            values.Add((decimal)db.Accounts.Where(x => x.statusId == 6 && x.Ward.SubDivision_Id == id).Count() / total * 100m);
            values.Add((decimal)db.Accounts.Where(x => x.statusId == 7 && x.Ward.SubDivision_Id == id).Count() / total * 100m);
            values.Add((decimal)db.Accounts.Where(x => x.statusId == 9 && x.Ward.SubDivision_Id == id).Count() / total * 100m);
            values.Add((decimal)db.Accounts.Where(x => x.statusId == 10 && x.Ward.SubDivision_Id == id).Count() / total * 100m);

            return Json(values, JsonRequestBehavior.AllowGet);
        }
        public ActionResult sdCardData(int id)
        {
            var wards = db.Wards.Where(x => x.SubDivision_Id == id);
            List<DashBoardCard> cards = new List<DashBoardCard>();
            foreach (var item in wards)
            {
                DashBoardCard card = new DashBoardCard();
                card.townId = item.Id;
                card.visited = db.Accounts.Where(x => x.Ward_Id == item.Id && x.statusId != 1).Count();
                card.total = db.Accounts.Where(x => x.Ward_Id == item.Id).Count();
                cards.Add(card);

            }
            return Json(cards, JsonRequestBehavior.AllowGet);
        }


        ////////////////////2nd dashboard////////////////////
        public ActionResult recoveryDashboard()
        {
            RecoveryDashboard rd = new RecoveryDashboard();
            var list = db.ConSummaries.OrderByDescending(x => x.entrdate).Take(8);
            ConSummary cs = new ConSummary();
            foreach (var item in list)
            {
                cs.domestic = item.domestic + cs.domestic;
                cs.commercial = cs.commercial + item.commercial;
                cs.cheritable = cs.cheritable + item.cheritable;
                cs.average = cs.average + item.average;
                cs.water = cs.water + item.water;
                cs.water_and_sewer = cs.water_and_sewer + item.water_and_sewer;
                cs.sewer = cs.sewer + item.sewer;
                cs.aquifer_water = cs.aquifer_water + item.aquifer_water;
                cs.aquifer_sewer = cs.aquifer_water + item.aquifer_water;
                cs.reading = cs.reading + item.reading;
                cs.entrdate = item.entrdate;
            }
           
            rd.ConSummary = cs;
            rd.RPList = new List<RecoveryProgress>();
            RecoveryProgress obj=db.RecoveryProgresses.Where(x=>x.isComplete==false).OrderByDescending(x => x.year).ThenByDescending(x => x.month).First();
            //List<RecoveryProgress> templist = db.RecoveryProgresses.OrderByDescending(x => x.year).ThenByDescending(x => x.month).Take(8).ToList();
            List<RecoveryProgress> templist = db.RecoveryProgresses.Where(x=>x.month==obj.month&&x.year==obj.year).Take(8).ToList();
            foreach (var item in templist)
            {
                 
                    rd.RPList.Add(item);
                
            }
            rd.dtList = new List<defaulterTable>();
            var dtTotalAccount = db.DefaulterPayments.GroupBy(x => x.townId);
            if (dtTotalAccount.Any())
            {
                foreach (var item in dtTotalAccount)
                {
                    defaulterTable dt = new defaulterTable();
                    dt.town = item.First().Town.town_desc;
                    dt.totalAccounts = item.Count();
                    dt.recoveredAccounts = item.Where(x => x.amountRecieved > 0).Count();
                    dt.totalAmount = item.Sum(x => x.arears);
                    dt.recoveredAmount = item.Sum(x => x.amountRecieved);
                    rd.dtList.Add(dt);

                }
            }
            rd.marquee = "";
            var mar = db.marquees;
            foreach (var item in mar)
            {
                rd.marquee = rd.marquee + item.value + "                   ";
            }
            return View(rd);
        }
    }
    public class DashBoardCard
    {
        public int townId { get; set; }
        public int total { get; set; }
        public int visited { get; set; }
    }
    public class Activity
    {
        public string time { get; set; }
        public string text { get; set; }
    }

}