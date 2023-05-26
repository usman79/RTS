using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RTS.Models;
using Newtonsoft.Json;
using RTS.HelperClasses;

namespace RTS.Controllers
{

    [Authorize]
    public class MobileAPIController : Controller
    {
        private RTSDBEntities db = new RTSDBEntities();
        // GET: MobileAPI
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult appLogin(int ticket, string password)
        {

            var afi = db.AFIs.FirstOrDefault(x => x.ticket == ticket && x.password == password);

            
          
            if (afi != null)
            {

                AFI a = new AFI();
                a.ticket =afi.ticket;
                a.name=afi.name;
                a.phone=afi.phone;
                a.Wards=afi.Wards.Select(x=>new Ward {Id= x.Id,ward_desc=x.ward_desc}).ToList();
                return Json(a, JsonRequestBehavior.AllowGet);
            }
          
            else
            {
                return Json("Wrong Ticket Or Password", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult getWardNumbers(int? wardId)
        {
            List< numbers> list= new List<numbers>();

             
            for(int i = 0; i < 6; i++)
            {
                list.Add(new numbers());
            }
            Ward w = db.Wards.FirstOrDefault(x => x.Id == wardId);
            if (w!=null &&w.Accounts.Count > 0)
            {
                foreach (var account in w.Accounts)
                {
                    list[0].label = "Total Accounts";
                    list[0].value = list[0].value + 1;
                    list[1].label = "Total Amount";
                    list[1].value = list[1].value + account.amount_due;

                    if (account.status == "n")
                    {
                        list[2].label = "Pending Accounts";
                        list[2].value = list[2].value + 1;
                        list[3].label = "Pending Amount";
                        list[3].value = list[1].value + account.amount_due;
 
                    }
                    else if (account.status == "r")
                    {
                        list[4].label = "Recovered Accounts";
                        list[4].value = list[2].value + 1;
                        list[5].label = "Recovered Amount";
                        list[5].value = list[1].value + account.amount_due;
                    }
                }
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("No Accounts found in this Ward", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult getWards(int userId)
        {
            
            var wards = db.Wards.Where(x => x.AFI_Id == userId ).Select(x => new
            {
                title = x.ward_desc,
                Id = x.Id,
                
                
            }).ToList();
            if (wards.Count > 0)
            {

                return Json(wards, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json("No Wards Found ", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult getAccounts(int wardId)
        {
            var list2 = db.Accounts.Where(x => x.Ward_Id == wardId && x.status == "n");
            var list = db.Accounts.Where(x => x.Ward_Id == wardId && x.status == "n").Select(x => new
            {
                title = x.title,
                Id = x.Id,
                amount_due = x.amount_due,
                latitude = x.latitude,
                longitude = x.longitude,
                address = x.address
            }).ToList();
            if(list.Count>0)
            {
               
                return Json(list, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json("No Pending Accounts Found", JsonRequestBehavior.AllowGet);
            }
        }
    }

    public class numbers
    {
        public string label { get; set; }
        public int value { get; set; }

    }
}