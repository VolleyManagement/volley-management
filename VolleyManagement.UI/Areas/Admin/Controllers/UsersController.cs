namespace VolleyManagement.UI.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Authorization;
    using VolleyManagement.Domain.RolesAggregate;
    using VolleyManagement.UI.Areas.Admin.Models;

    public class UsersController : Controller
    {

        private readonly IAuthorizationService _authService;
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="authService">Authorization service</param>
        /// <param name="userService">User service</param>
        public UsersController(IAuthorizationService authService,
                               IUserService userService )
        {
            this._authService = authService;
            this._userService = userService;
        }

        // GET: Admin/AllUsers
        public ActionResult Index()
        {
            var users = _userService.GetAllUsers().ConvertAll(UserViewModel.Initialize);
            return View(users);
        }

        // GET: Admin/GetDetails
        public ActionResult UserDetails(int id)
        {
            var user = _userService.GetUserDetails(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var result = UserViewModel.Map(user);
         
            return View(result);
        }
    }
}