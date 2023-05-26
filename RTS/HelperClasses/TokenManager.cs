using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RTS.Models;

namespace RTS.HelperClasses
{
    public class TokenManager : ITokenManager
    {
        private RTSDBEntities db = new RTSDBEntities();
        
        public bool Authenticate(string username, string pass)
        {

            if (!string.IsNullOrWhiteSpace(username) &&
                !string.IsNullOrWhiteSpace(pass))
                return true;

            return false;
        }
        public Token NewToken()
        {
            var token = new Token
            {
                Value = Guid.NewGuid().ToString(),
                ExpiryDate = DateTime.Now.AddMinutes(50),

            };
            db.Tokens.Add(token);
            db.SaveChanges();
            return token;
        }
        public bool VerifyToken(string token)
        {

            if (db.Tokens.Any(x => x.Value == token && x.ExpiryDate > DateTime.Now))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }


}