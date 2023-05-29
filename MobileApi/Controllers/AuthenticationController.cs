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
       
    }
    public class userViewModel
    {
        public appUser user { get; set; }
        public string token { get; set; }

    }
}
