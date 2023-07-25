using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MobileApi.HelperClasses;
using MobileApi.Models;
using System.Globalization;
using System.Web;
using System.Web.Hosting;
 
using System.Data;
using System.Data.Entity;
 
 
 
using System.IO;
using System.Text;
using System.Drawing;

namespace MobileApi.Controllers
{
    [TokenAuthenticationFilter]
    public class AFIController : ApiController
    {
        private RTSDBEntities db = new RTSDBEntities();
        
        public IEnumerable<wardViewModel> getWards(int userId)
        {
            var user = db.appUsers.FirstOrDefault(x => x.id == userId);

            if (user.role == "afi")
            {
                 
              List<wardViewModel> wards = db.Wards.Where(x => x.AFI_Id == user.afi_id).Select(x => new wardViewModel
                {
                    title = x.ward_desc,
                    id = x.Id,


                }).ToList();
                

                    return wards; 
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<numbers> getWardDetail(int wardId)
        {
            List<numbers> list = new List<numbers>();


            for (int i = 0; i < 7; i++)
            {
                list.Add(new numbers());
            }
            Ward w = db.Wards.FirstOrDefault(x => x.Id== wardId);
            if (w != null && w.Accounts.Count > 0)
            {
                foreach (var account in w.Accounts)
                {
                    list[0].label = "Total Accounts";
                    list[0].value = list[0].value + 1;
                    list[1].label = "Total Amount";
                    list[1].value = list[1].value + account.amount_due;
                    list[2].label = "Pending Accounts";
                    list[3].label = "Pending Amount";
                    list[4].label = "Recovered Accounts";
                    list[5].label = "Recovered Amount";
                    list[6].label = "Visited Properties";
                    if (account.statusId == 1)
                    {
                        
                        list[2].value = list[2].value + 1;
                        
                        list[3].value = list[3].value + account.amount_due;

                    }
                    else if (account.statusId == 8)
                    {
                        
                        list[4].value = list[4].value + 1;
                        
                        list[5].value = list[5].value + account.amount_due;
                    }
                   if (account.statusId != 1)
                    {
                       
                        list[6].value = list[6].value + 1;
                        
                    }
                }
                return list;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<accountViewModel> getWardAccounts(int wardId)
        {
            if(DateTime.Today.DayOfWeek==DayOfWeek.Sunday)
            {
                return new List<accountViewModel>();
            }

           
            if (!db.Accounts.Any(x => x.assignment_date == DateTime.Today && x.Ward_Id == wardId))
            {
                List<Account> newlList = db.Accounts.Where(x => x.Ward_Id == wardId && x.statusId == 1&& x.is_assigned == "n")
                                 .OrderBy(x => x.walk_sort)
                                  .Take(5).ToList();
                foreach (Account account in newlList)
                {
                    account.assignment_date = DateTime.Today;
                    account.is_assigned = "y";
                    db.SaveChanges();
                }
            }
            ///assigning installment accounts
            if (!db.Accounts.Any(x => x.nextVisit_date == DateTime.Today && x.Ward_Id == wardId&&x.is_assigned=="y"))
            {
                List<Account> newlList = db.Accounts.Where(x => x.Ward_Id == wardId && x.nextVisit_date == DateTime.Today&&x.is_assigned=="n")
                                 .OrderBy(x => x.walk_sort)
                                  .Take(2).ToList();
                foreach (Account account in newlList)
                {
                    account.assignment_date = DateTime.Today;
                    account.is_assigned = "y";
                    db.SaveChanges();
                }
            }
            ///assigning re-visit accounts 
            if (!db.Accounts.Any(x => x.assignment_date == DateTime.Today && x.Ward_Id == wardId && x.is_assigned == "y"&&x.visits>0))
            {
                DateTime check = DateTime.Today.AddDays(-7);
                List<Account> newlList = db.Accounts.Where(x => x.Ward_Id == wardId   && x.is_assigned == "n" &&
                ( x.statusId==3||x.statusId==5||x.statusId==6)&&x.assignment_date<check)
                                 .OrderBy(x => x.visits)
                                  .Take(3).ToList();
                foreach (Account account in newlList)
                {
                    account.assignment_date = DateTime.Today;
                    account.is_assigned = "y";
                    db.SaveChanges();
                }
            }
            List<Account> list = db.Accounts.Where(x => x.is_assigned == "y"&&x.Ward_Id==wardId).ToList();

            List<accountViewModel> viewList=new List<accountViewModel>();
            foreach (Account account in list)
            {
                accountViewModel obj = new accountViewModel();
                obj.id = account.Id;
                obj.Address = account.address;
                obj.Account_No = account.Account_No;
                obj.propertyNo = account.propertyNo;
                obj.Arears = account.amount_due;
                obj.accountType = account.Account_Type;
                obj.conectionDate = account.Connection_Date;
                obj.Consumer_Name = account.Consumer_Name;
                obj.Ward = account.Ward.ward_desc;
                obj.lastPaymentAmount = account.last_payment;
                obj.LastPaymentDate = account.last_payment_date;
                obj.accountType = account.Account_Type;
                obj.walkSort = account.walk_sort;
                obj.currentDemand = account.Current_Demand;
                obj.assignmentDate = account.assignment_date;

                viewList.Add(obj);


            }


            return viewList.OrderByDescending(x => x.assignmentDate);
        }
        public IEnumerable<accountStatu> getAllStatus()
        {

            var list = db.accountStatus.Where(x=>x.id!=1).ToList();


            return list;
        }
        public IEnumerable<accountViewModel> getUserHistory(int  userId)
        {
            var afi = db.appUsers.FirstOrDefault(x => x.id == userId).afi_id;
            List<Account> list = db.Accounts.Where(x => x.Ward.AFI_Id == afi && x.statusId != 1).ToList();
            List<accountViewModel> viewList = new List<accountViewModel>();
            foreach (Account account in list)
            {
                accountViewModel obj = new accountViewModel();
                obj.id = account.Id;
                obj.Address = account.address;
                obj.Account_No = account.Account_No;
                obj.propertyNo = account.propertyNo;
                obj.Arears = account.amount_due;
                obj.accountType = account.Account_Type;
                obj.conectionDate = account.Connection_Date;
                obj.Consumer_Name = account.Consumer_Name;
                obj.Ward = account.Ward.ward_desc;
                obj.lastPaymentAmount = account.last_payment;
                obj.LastPaymentDate = account.last_payment_date;
                obj.accountType = account.Account_Type;
                obj.walkSort = account.walk_sort;
                obj.currentDemand = account.Current_Demand;
                obj.statusId=account.statusId;
                obj.status = account.accountStatu.value;
                obj.visitDate = account.visit_date;

                viewList.Add(obj);


            }
             
            return viewList;
        }

        [HttpPost]

     
        public string postAccount([FromBody] accountViewModel account)
        {
            
            //if (!ModelState.IsValid)
            //    return "muje kiu nikala";

            var actObj=db.Accounts.FirstOrDefault(x=>x.Id==account.id);
            accountDetail ad=new accountDetail();
            if (actObj != null)
            {
                actObj.statusId = account.statusId;
                actObj.is_assigned = "n";

                actObj.visit_date = DateTime.Now;
                actObj.visit_lat = account.latitude;
                actObj.visit_long = account.longitude;
                actObj.remarks = account.remarks;
                 
                actObj.visits++;
                if (account.statusId == 4)
                {
                    actObj.nextVisit_date = account.nextVisitDate;
                }
                 
                if (account.statusId == 11)
                {
                    actObj.nextVisit_date = DateTime.Today.AddDays(30);
                    ad.is_installment = "y";
                    ad.installment_date = DateTime.Today;
                    ad.paymentAmount= account.paymentAmount;
                    if (actObj.partialPayment > 0)
                    {
                        actObj.partialPayment = actObj.partialPayment + account.paymentAmount;
                    }
                    else
                    {
                        actObj.partialPayment = account.paymentAmount;
                    }
                }
                else
                {
                    ad.is_installment = "n";
                }
                ad.id = (int)Convert.ToInt64(account.id.ToString() +db.accountDetails.Count().ToString());
                ad.accountId = account.id;
                ad.visit_date = DateTime.Today;
                ad.visit_lat = account.latitude;
                ad.visit_long= account.longitude;
                ad.remarks = account.remarks;
                db.accountDetails.Add(ad);


                if (account.Image != null)
                {
                    actObj.Image = account.id.ToString() + "-" + DateTime.Today.Day + "-" + DateTime.Today.Month + ".jpeg";
                    byte[] bytes = Convert.FromBase64String(account.Image);

                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        Image image = Image.FromStream(ms);
                        image.Save(HostingEnvironment.MapPath("~/Content/AppImages/" + actObj.Image), System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                }
                else
                {
                    if(account.statusId!=9)
                    return "Image File not recieved";
                }



                db.SaveChanges();
                return "Successfully Updated";

            }
            else
            {
                return "account not found";
            }
            
        }
      
    }
    public class accountViewModel
    {
        public int id { get; set; }
        public int statusId { get; set; }
        public string status { get; set; }  
        public string Account_No { get; set; }
        public string Consumer_Name { get; set; }
        public string Address { get; set; }
        public string propertyNo { get; set; }
        public int Arears { get; set; }
        public string Ward { get; set; }
        public DateTime? LastPaymentDate { get; set; }
        public int? lastPaymentAmount { get; set; }
        public DateTime? conectionDate { get; set; }
        public string accountType { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
        public string remarks { get; set; }
        public int walkSort { get; set; }
        public int paymentAmount { get; set; }
        // public HttpPostedFileBase Image { get; set; }
        public string Image { get; set; }
        public int? currentDemand { get; set; }  
        public DateTime? assignmentDate { get; set; }
        public DateTime? visitDate { get; set; }
        public DateTime? nextVisitDate { get; set; }
    }
    public class wardViewModel
    {
        public int id { get; set; }
        public string title { get; set; }
    }
    public class numbers
    {
        public string label { get; set; }
        public int value { get; set; }

    }
}
