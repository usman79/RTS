using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MobileApi.HelperClasses;
using MobileApi.Models;

namespace MobileApi.Controllers
{
    public class AuthenticationController : ApiController
    {
        private RTSDBEntities db = new RTSDBEntities();

        [HttpGet]
        public userViewModel Login(string ticket, string password) {
            TokenManager tm=new TokenManager();
            if(tm.Authenticate(ticket, password))
            {
                userViewModel obj = new userViewModel();

                obj.token = tm.NewToken(ticket).Value;
                obj.user=db.appUsers.FirstOrDefault(x => x.ticket == ticket);   
                return obj;
            }

            return null;
        }
        public string getVersion()
        {
            return "2.1.0";
        }
        [HttpGet]
        public string resetPassword(string ticket, string password,string newpassword)
        {
            var user = db.appUsers.FirstOrDefault(x=>x.ticket==ticket && x.password==password);
            if (user != null)
            {
                user.password =newpassword;
                int t=Convert.ToInt32(ticket);
                Token tokenObj=db.Tokens.Where(x => x.id == t).First();
                db.Tokens.Remove(tokenObj);
                db.SaveChanges();
                return "Successfully Changed";
            }
            else { return "wrong ticket or password"; }

          
        }
    }
  
    public class userViewModel
    {
        public appUser user { get; set; }
        public string token { get; set; }

    }
}
