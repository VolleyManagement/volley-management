namespace VolleyManagement.UI.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Contracts.Authorization;
    using Domain.RolesAggregate;
    using Models;
    using System.Linq;

    /// <summary>
    /// Provides Roles management
    /// </summary>
    [Authorize]
    public class RolesController : Controller
    {
        private readonly IRolesService _rolesService;
        private readonly IAuthorizationService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RolesController"/> class.
        /// </summary>
        /// <param name="rolesService"> The roles service. </param>
        /// <param name="authService">Authorization service</param>
        public RolesController(
                IRolesService rolesService,
                IAuthorizationService authService)
        {
            _rolesService = rolesService;
            _authService = authService;
        }

        /// <summary>
        /// Index view
        /// </summary>
        /// <returns>Action result</returns>
        public ActionResult Index()
        {
            _authService.CheckAccess(AuthOperations.AdminDashboard.View);

            var roles = (_rolesService.GetAllRoles()).Select(r => new RoleViewModel(r)).ToList();
            return View(roles);
        }

        /// <summary>
        /// Edit role view.
        /// </summary>
        /// <param name="id"> Role Id. </param>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        public ActionResult Edit(int id)
        {
            _authService.CheckAccess(AuthOperations.AdminDashboard.View);

            var allUsers = _rolesService.GetAllUsersWithRoles();
            var role = _rolesService.GetRole(id);
            var result = new RoleEditViewModel(role);
            result.SetFromUsers(allUsers);

            return View(result);
        }

        /// <summary>
        /// The edit post action to apply changes
        /// </summary>
        /// <param name="modifiedRoles"> The modified roles data. </param>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        [HttpPost]
        public ActionResult Edit(ModifiedRoleViewModel modifiedRoles)
        {
            _authService.CheckAccess(AuthOperations.AdminDashboard.View);

            try
            {
                _rolesService.ChangeRoleMembership(
                            modifiedRoles.RoleId,
                            modifiedRoles.IdsToAdd,
                            modifiedRoles.IdsToDelete);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return View();
            }
        }

        /// <summary>
        /// Role details.
        /// </summary>
        /// <param name="id"> Role Id. </param>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        public ActionResult Details(int id)
        {
            _authService.CheckAccess(AuthOperations.AdminDashboard.View);

            var role = _rolesService.GetRole(id);
            var result = new RoleDetailsViewModel(role);

            result.Users = _rolesService.GetUsersInRole(id).ToList().ConvertAll(u => u.UserName);

            return View(result);
        }
    }
}