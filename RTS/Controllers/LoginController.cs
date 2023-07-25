using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RTS.Models;

namespace RTS.Controllers
{
    public class LoginController : Controller
    {
        private RTSDBEntities db = new RTSDBEntities();
        // GET: Login
        [AllowAnonymous]  
        public ActionResult Login()
        {
            return View(1);
        }

    
      
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(string username, string password)
        {
            webuser user = db.webusers.FirstOrDefault(x=>x.ticket.Equals(username)&&x.password.Equals(password));
            
            try
            {
                if (user != null)
                {
                    Session["ticket"] = user.ticket;
                    Session["role"] = user.role;
                    Session["officeId"] = user.officeId;

                    if (user.role == "admin")
                        return RedirectToAction("Index", "Home");
                    else if (user.role == "ddr")
                        return RedirectToAction("TownDashboard", "Home", new { id = user.officeId });
                    else if (user.role == "sdo")
                        return RedirectToAction("SubDivisionDashboard", "Home", new { id = user.officeId });
                    else
                        return View(0);
                }
                   
                 else
                    return View(0);
            }
            catch
            {
                return View(1);
            }
        }
        public ActionResult logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
        [HttpPost]
        public ActionResult ChangePassword(string oldpassword,string newpassword)
        {
            var user = db.webusers.FirstOrDefault(x => x.ticket.Equals((string)Session["ticket"]) && x.password.Equals(oldpassword));
            user.password = newpassword;
            db.SaveChanges();
            Session.Clear();
            return RedirectToAction("logout");
        }
        public ActionResult ChangePassword( )
        {
            return View();
             
        }

    }
}
