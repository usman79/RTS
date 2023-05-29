using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MobileApi.HelperClasses;
using System;
using System.Collections.Generic;

using MobileApi.Models;

namespace MobileApi.Controllers
{
  //  [TokenAuthenticationFilter]
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


            for (int i = 0; i < 6; i++)
            {
                list.Add(new numbers());
            }
            Ward w = db.Wards.FirstOrDefault(x => x.Id == wardId);
            if (w != null && w.Accounts.Count > 0)
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
                return list;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<Account> getWardAccounts(int wardId)
        {
      
          var list = db.Accounts.Where(x => x.Ward_Id == wardId && x.status == "n"). ToList();
 

            

            return list;
        }
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
