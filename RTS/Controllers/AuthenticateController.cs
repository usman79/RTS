using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
 
using Microsoft.AspNetCore.Mvc;
using RTS.HelperClasses;

namespace RTS.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly ITokenManager tokenManager;

        public AuthenticateController(ITokenManager tokenManager)
        {
            this.tokenManager = tokenManager;
        }

        public IActionResult Authenticate(string username, string pass)
        {

            if (tokenManager.Authenticate(username, pass))
            {
                return Ok(new { token = tokenManager.NewToken() });
            }
            else
            {
                ModelState.AddModelError("Not Authorized", "");
                return Unauthorized(ModelState);
            }
        }
    }
}