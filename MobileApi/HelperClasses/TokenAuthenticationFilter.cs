
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.Mvc;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using System.Web.Http.Controllers;

namespace MobileApi.HelperClasses
{
    public class TokenAuthenticationFilter : AuthorizeAttribute
    {

        public override void OnAuthorization(HttpActionContext context)
        {
            var requestScope = context.Request.GetDependencyScope();
            //var tokenManager = requestScope.GetService(typeof(ITokenManager)) as ITokenManager;

            bool IsTokenValidated = false;
            IEnumerable<string> listVals = null;
            context.Request.Headers.TryGetValues("Authorization", out listVals);

            if (listVals != null && listVals.Count() > 0)
            {
                //if (!tokenManager.VerifyToken(listVals.FirstOrDefault()))
                //{
                //    IsTokenValidated = false;
                //}
            }

           
            if (!IsTokenValidated)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
        }
    }
}