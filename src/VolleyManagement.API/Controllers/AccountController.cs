using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Authentication.Internal;
using VolleyManagement.Contracts.Authentication.Models;

namespace VolleyManagement.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        [HttpPost]
        public bool TokenSignin(string idtoken)
        {
            var loginInfo = idtoken;
            return true;
        }
    }
}