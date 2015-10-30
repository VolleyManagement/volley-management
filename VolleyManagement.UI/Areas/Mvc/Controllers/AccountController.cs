namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;

    using VolleyManagement.Contracts.Authentication;
    using VolleyManagement.Contracts.Authentication.Models;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Account;

    /// <summary>
    /// Manages Sign In/Out process
    /// </summary>
    public class AccountController : Controller
    {
        private readonly IVolleyUserManager<UserModel> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="userManager"> User Manager </param>
        public AccountController(IVolleyUserManager<UserModel> userManager)
        {
            this._userManager = userManager;
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
        /// Logout and return to another page.
        /// </summary>
        /// <param name="returnUrl"> Url to return after logout procedure. </param>
        /// <returns> Page from url. </returns>
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