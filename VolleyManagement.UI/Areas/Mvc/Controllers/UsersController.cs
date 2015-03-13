namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using VolleyManagement.Contracts;
    using VolleyManagement.Domain.Users;
    using VolleyManagement.UI.Areas.Mvc.Mappers;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Users;

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
            this._userService = userService;
        }

        /// <summary>
        /// Gets all users from UserService
        /// </summary>
        /// <returns>View with collection of users</returns>
        public ActionResult Index()
        {
            try
            {
                var users = this._userService.Get().ToList();
                return this.View(users);
            }
            catch (Exception)
            {
                return this.HttpNotFound();
            }
        }

        /// <summary>
        /// Gets details for specific user
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>View with specific user</returns>
        public ActionResult Details(int id)
        {
            try
            {
                User user = this._userService.Get(id);
                return this.View(user);
            }
            catch (Exception)
            {
                return this.HttpNotFound();
            }
        }

        /// <summary>
        /// Create user action (GET)
        /// </summary>
        /// <returns>View to create a user</returns>
        public ActionResult Create()
        {
            var userViewModel = new UserViewModel();
            return this.View(userViewModel);
        }

        /// <summary>
        /// Create user action (POST)
        /// </summary>
        /// <param name="userViewModel">Created user</param>
        /// <returns>Index view if user was valid, else - create view</returns>
        [HttpPost]
        public ActionResult Create(UserViewModel userViewModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                   var user = ViewModelToDomain.Map(userViewModel);
                   this._userService.Create(user);
                    return this.RedirectToAction("Index");
                }

                return this.View(userViewModel);
            }
            catch (ArgumentException ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return this.View(userViewModel);
            }
            catch (Exception)
            {
                return this.HttpNotFound();
            }
        }

        /// <summary>
        /// Edit user action (GET)
        /// </summary>
        /// <param name="id">User id to edit.</param>
        /// <returns>View to edit a user.</returns>
        public ActionResult Edit(int id)
        {
            return this.View();
        }

        /// <summary>
        /// Edit user action (POST)
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="collection">View form collection.</param>
        /// <returns>Redirect to Index.</returns>
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                return this.RedirectToAction("Index");
            }
            catch
            {
                return this.View();
            }
        }

        /// <summary>
        /// Delete user action (GET)
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>View to delete specific user</returns>
        public ActionResult Delete(int id)
        {
            return this.View();
        }

        /// <summary>
        /// Delete user action (POST)
        /// </summary>
        /// <param name="id">User id.</param>
        /// <param name="collection">View form collection.</param>
        /// <returns>Redirect to Index.</returns>
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                return this.RedirectToAction("Index");
            }
            catch
            {
                return this.View();
            }
        }
    }
}