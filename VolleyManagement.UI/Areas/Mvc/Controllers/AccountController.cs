namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;

    using Contracts.Authentication;
    using Contracts.Authentication.Models;
    using Contracts.Authorization;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;

    using Services.Authorization;
    using ViewModels.Account;
    using ViewModels.Users;

    /// <summary>
    /// Manages Sign In/Out process
    /// </summary>
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IVolleyUserManager<UserModel> _userManager;
        private readonly IRolesService _rolesService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="userManager"> User Manager </param>
        /// <param name="rolesService"> Roles service </param>
        public AccountController(
                    IVolleyUserManager<UserModel> userManager,
                    IRolesService rolesService)
        {
            this._userManager = userManager;
            this._rolesService = rolesService;
        }

        private int currentUserId
        {
            get
            {
                return System.Convert.ToInt32(User.Identity.GetUserId());
            }
        }

        private IAuthenticationManager AuthManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        /// <summary>
        /// Renders partial view to represent current Account information
        /// </summary>
        /// <returns> Partial view </returns>
        [AllowAnonymous]
        public PartialViewResult Info()
        {
            var vm = new AuthenticationStatusViewModel();
            vm.IsAuthenticated = HttpContext.User.Identity.IsAuthenticated;
            vm.ReturnUrl = GetReturnUrl();
            if (vm.IsAuthenticated)
            {
                vm.UserName = HttpContext.User.Identity.GetUserName();
            }

            return this.PartialView("Info", vm);
        }

        /// <summary>
        /// Logs current user
        /// </summary>
        /// <param name="returnUrl">URL to return</param>
        /// <returns>Action result</returns>
        [AllowAnonymous]
        public ActionResult Logout(string returnUrl)
        {
            AuthManager.SignOut();
            return this.Redirect(this.GetRedirectUrl(returnUrl));
        }

        /// <summary>
        /// Starts login interaction
        /// </summary>
        /// <param name="returnUrl"> The return Url. </param>
        /// <returns> View item </returns>
        [AllowAnonymous]
        [RequireHttps]
        public ActionResult Login(string returnUrl)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                // ToDo: return View("Error", new string[] { "Access Denied" });
            }

            ViewBag.returnUrl = returnUrl;
            return View();
        }

        /// <summary>
        /// Return view represents account information.
        /// </summary>
        /// <returns>View to represent</returns>
        [Authorize]
        public async Task<ActionResult> Details()
        {
            var user = await _userManager.FindByIdAsync(currentUserId);
            UserViewModel userViewModel = UserViewModel.Map(user);
            return View(userViewModel);
        }

        /// <summary>
        /// Return edit view.
        /// </summary>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        [Authorize]
        public async Task<ActionResult> Edit()
        {
            var user = await _userManager.FindByIdAsync(currentUserId);
            UserEditViewModel userEditViewModel = UserEditViewModel.Map(user);
            return View(userEditViewModel);
        }

        /// <summary>
        /// Post edit method
        /// </summary>
        /// <param name="editViewModel">Edit view model.</param>
        /// <returns>Details action result</returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Edit(UserEditViewModel editViewModel)
        {
            if (currentUserId != editViewModel.Id && !User.IsInRole(Resources.AuthorizationRoles.Admin))
            {
                return View("AccessDenied");
            }

            if (this.ModelState.IsValid)
            {
                var userModel = await _userManager.FindByIdAsync(editViewModel.Id);
                if (userModel == null)
                {
                    throw new ArgumentNullException(Resources.AccountController.InvalidEditEntityId);
                }

                userModel.PersonName = editViewModel.FullName;
                userModel.PhoneNumber = editViewModel.CellPhone;
                userModel.Email = editViewModel.Email;
                await _userManager.UpdateAsync(userModel);

                return RedirectToAction("Details");
            }

            return View(editViewModel);
        }

        /// <summary>
        /// The google login.
        /// </summary>
        /// <param name="returnUrl"> The return url. </param>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        [HttpPost]
        [AllowAnonymous]
        [RequireHttps]
        public ActionResult GoogleLogin(string returnUrl)
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("ExternalLoginCallback", new { returnUrl = returnUrl })
            };
            HttpContext.GetOwinContext().Authentication.Challenge(properties, "Google");
            return new HttpUnauthorizedResult();
        }

        /// <summary>
        /// The google login callback.
        /// </summary>
        /// <param name="returnUrl"> The return url. </param>
        /// <returns> The <see cref="Task"/>. </returns>
        [AllowAnonymous]
        [RequireHttps]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            ExternalLoginInfo loginInfo = await AuthManager.GetExternalLoginInfoAsync();
            UserModel user = await _userManager.FindAsync(loginInfo.Login);
            if (user == null)
            {
                user = new UserModel
                {
                    Email = loginInfo.Email,
                    UserName = loginInfo.DefaultUserName,
                    PersonName = loginInfo.ExternalIdentity.Name
                };
                IdentityResult result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return View("Error", result.Errors);
                }
                else
                {
                    result = await _userManager.AddLoginAsync(user.Id, loginInfo.Login);
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
            }

            ClaimsIdentity ident = await _userManager.CreateIdentityAsync(
                                                        user,
                                                        DefaultAuthenticationTypes.ApplicationCookie);
            ident.AddClaims(loginInfo.ExternalIdentity.Claims);
            AuthManager.SignOut();
            AuthManager.SignIn(new AuthenticationProperties { IsPersistent = false }, ident);

            return Redirect(GetRedirectUrl(returnUrl));
        }

        private string GetRedirectUrl(string returnUrl)
        {
            return returnUrl ?? this.GetDefaultUrl();
        }

        private string GetReturnUrl()
        {
            string result = null;
            var url = this.HttpContext.Request.Url;
            if (url != null)
            {
                result = url.ToString();
            }

            return result;
        }

        private string GetDefaultUrl()
        {
            return Url.Action("Index", "Tournaments");
        }
    }
}