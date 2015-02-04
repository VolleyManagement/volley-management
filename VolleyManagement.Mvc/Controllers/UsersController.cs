namespace VolleyManagement.Mvc.Controllers
{
    using System;
    using System.Web.Mvc;
    using VolleyManagement.Contracts;
    using VolleyManagement.Domain.Users;
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
        /// Gets all users from UserService
        /// </summary>
        /// <returns>View with collection of users</returns>
        public ActionResult Index()
        {
            return View();
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
                User user = _userService.FindById(id);
                return View(user);
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
        }

        /// <summary>
        /// Create user action (GET)
        /// </summary>
        /// <returns>View to create a user</returns>
        public ActionResult Create()
        {
            UserViewModel userViewModel = new UserViewModel();
            return View(userViewModel);
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
                if (ModelState.IsValid)
                {
                   var user = ViewModelToDomain.Map(userViewModel);
                   _userService.Create(user);
                    return RedirectToAction("Index");
                }

                return View(userViewModel);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(userViewModel);
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Delete user action (GET)
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>View to delete specific user</returns>
        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}