namespace VolleyManagement.UI.Areas.Admin.Controllers
{
    using System;
    using System.Web.Mvc;

    using VolleyManagement.Contracts.Authorization;
    using VolleyManagement.UI.Areas.Admin.Models;

    /// <summary>
    /// Provides Roles management
    /// </summary>
    [Authorize]
    public class RolesController : Controller
    {
        private readonly IRolesService _rolesService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RolesController"/> class.
        /// </summary>
        /// <param name="rolesService"> The roles service. </param>
        public RolesController(IRolesService rolesService)
        {
            this._rolesService = rolesService;
        }

        /// <summary>
        /// Index view
        /// </summary>
        /// <returns>Action result</returns>
        public ActionResult Index()
        {
            var roles = _rolesService.GetAllRoles().ConvertAll(r => new RoleViewModel(r));
            return View(roles);
        }

        /// <summary>
        /// Edit role view.
        /// </summary>
        /// <param name="id"> Role Id. </param>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        public ActionResult Edit(int id)
        {
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
                return this.View();
            }
        }

        /// <summary>
        /// Role details.
        /// </summary>
        /// <param name="id"> Role Id. </param>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        public ActionResult Details(int id)
        {
            var role = _rolesService.GetRole(id);
            var result = new RoleDetailsViewModel(role);

            result.Users = _rolesService.GetUsersInRole(id).ConvertAll(u => u.UserName);

            return View(result);
        }
    }
}