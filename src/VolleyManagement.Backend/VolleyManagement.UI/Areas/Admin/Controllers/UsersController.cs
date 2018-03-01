namespace VolleyManagement.UI.Areas.Admin.Controllers
{
    using System;
    using System.Web.Mvc;
    using Contracts;
    using Contracts.Exceptions;
    using Models;
    using System.Linq;

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
            //var users = _userService.GetAllUsers().ToList().ConvertAll(UserViewModel.Initialize);
            var users = _userService.GetAllUsers().Select(UserViewModel.Initialize).ToList();

            return View(users);
        }

        /// <summary>
        /// Get all active user list view.
        /// </summary>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        public ActionResult ActiveUsers()
        {
            var activeUsers = _userService.GetAllActiveUsers().Select(UserViewModel.Initialize).ToList();
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
        /// Change user's state GET action.
        /// </summary>
        /// <param name="id"> User Id. </param>
        /// <param name="toBlock">block or unblock user</param>
        /// <returns>Redirect to Users page</returns>
        [HttpGet]
        public ActionResult ChangeUserBlocked(int id, bool toBlock)
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
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (MissingEntityException ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}