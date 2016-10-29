using VolleyManagement.Contracts.Authorization;
using VolleyManagement.Domain.RolesAggregate;

namespace VolleyManagement.UI.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class UsersController : Controller
    {

        private readonly IAuthorizationService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="authService">Authorization service</param>
        public UsersController(IAuthorizationService authService)
        {
            this._authService = authService;
        }

        // GET: Admin/AllUsers
        public ActionResult AllUsers()
        {
           // this._authService.CheckAccess(AuthOperations.AllUsers.ViewList);
            return View();
        }

        // GET: Admin/GetDetails
        public ActionResult GetDetails()
        {
            return View();
        }

    }
}