namespace VolleyManagement.UI.Areas.Admin.Controllers
{
    using System;
    using System.Web.Mvc;
    using Contracts;
    using Contracts.Exceptions;
    using Models;

    /// <summary>
    /// Provides User management
    /// </summary>
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userService">User service</param>
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get all user list view.
        /// </summary>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        public ActionResult Index()
        {
            var users = _userService.GetAllUsers().ConvertAll(UserViewModel.Initialize);
            return View(users);
        }

        /// <summary>
        /// Get all active user list view.
        /// </summary>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        public ActionResult ActiveUsers()
        {
            var activeUsers = _userService.GetAllActiveUsers().ConvertAll(UserViewModel.Initialize);
            return View(activeUsers);
        }

        /// <summary>
        /// Get user's details view.
        /// </summary>
        /// <param name="id"> User Id. </param>
        /// <returns> The <see cref="ActionResult"/>. </returns>
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

        /// <summary>
        /// Get user's details view.
        /// </summary>
        /// <param name="id"> User Id. </param>
        /// <param name="toBlock">block or unblock user</param>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        public ActionResult ChangeUserBlocked(int id, bool toBlock)
        {
            var user = _userService.GetUserDetails(id);
            var users = _userService.GetAllUsers().ConvertAll(UserViewModel.Initialize);

            if (user == null)
            {
                return HttpNotFound();
            }

            try
            {
                _userService.ChangeUserBlocked(id, toBlock);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("ValidationError", ex.Message);
            }
            catch (MissingEntityException ex)
            {
                ModelState.AddModelError("ValidationError", ex.Message);
            }

            return View("Index", users);
        }
    }
}