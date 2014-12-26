namespace VolleyManagement.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using VolleyManagement.Contracts;
    using VolleyManagement.Mvc.Mappers;
    using VolleyManagement.Mvc.ViewModels.Users;

    /// <summary>
    /// Defines UsersController
    /// </summary>
    public class UsersController : Controller
    {
        /// <summary>
        /// Holds UserService instance
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class
        /// </summary>
        /// <param name="userService">The user service</param>
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Gets all users with UserService
        /// </summary>
        /// <returns>View with collection of users</returns>
        public ActionResult Index()
        {
            try
            {
                var domainUsers = _userService.GetAll().ToList();
                var userViewModels = new List<UserViewModel>();
                foreach (var u in domainUsers)
                {
                    userViewModels.Add(DomainToViewModel.Map(u));
                }

                return View(userViewModels);
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
        }
    }
}