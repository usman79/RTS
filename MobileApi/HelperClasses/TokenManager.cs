using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MobileApi.Models;

namespace MobileApi.HelperClasses
{
    public class TokenManager  
    {
        private RTSDBEntities db = new RTSDBEntities();

        public bool Authenticate(string username, string pass)
        {
            var user = db.appUsers.FirstOrDefault(x => x.ticket == username);

            if (!string.IsNullOrWhiteSpace(username) &&
                !string.IsNullOrWhiteSpace(pass)
                && username == user.ticket
                && pass == user.password)
                return true;

            return false;

        }
        public Token NewToken(string ticket)
        {
            int t = Convert.ToInt32(ticket);
            var token = new Token
            {
                 Value = Guid.NewGuid().ToString(),
                //ExpiryDate = DateTime.Now.AddMinutes(50),
               // Value = "abcd",
                ExpiryDate = DateTime.Now.AddDays(1),
                id=t,

            };
           
            Token oldToken = db.Tokens.FirstOrDefault(x => x.id == t);
            if(oldToken == null)
            {
                db.Tokens.Add(token);
                db.SaveChanges();
                return token;
            }
            
            return oldToken;
          
        }
        public bool VerifyToken(string token)
        {
            return true;
            if (db.Tokens.Any(x => x.Value == token && x.ExpiryDate > DateTime.Now))
            {
                return true;
            }
            else
            {
                if(db.Tokens.Any(x => x.Value == token && x.ExpiryDate< DateTime.Now))
                {
                    var tok=db.Tokens.FirstOrDefault(x => x.Value == token );
                    db.Tokens.Remove(tok);
                    db.SaveChanges();
                }
                return false;
            }
        }
    }


}