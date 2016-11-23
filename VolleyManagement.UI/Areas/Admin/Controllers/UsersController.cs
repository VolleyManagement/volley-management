namespace VolleyManagement.UI.Areas.Admin.Controllers
{
    using System;
    using System.Web.Mvc;
    using Contracts.Exceptions;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Authorization;
    using VolleyManagement.UI.Areas.Admin.Models;

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
            this._userService = userService;
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
        /// <param name="backTo"> return to url where button click </param>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        public ActionResult ChangeUserBlocked(int id, bool toBlock, string backTo)
        {
            var user = _userService.GetUserDetails(id);
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
                return View(
                    "ErrorPage",
                    new OperationResultViewModel
                    {
                        Message = ex.Message
                    });
            }
            catch (MissingEntityException ex)
            {
                return View(
                    "ErrorPage",
                    new OperationResultViewModel
                    {
                        Message = ex.Message
                    });
            }

            return Redirect(backTo);
        }
    }
}