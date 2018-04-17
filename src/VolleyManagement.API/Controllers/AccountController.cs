namespace VolleyManagement.API.Controllers
{
    using Google.Apis.Auth;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Model;
    using Contracts;
    using Contracts.Authentication;
    using Contracts.Authentication.Models;
    using Crosscutting.Contracts.Providers;

#pragma warning disable S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
#pragma warning restore S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)
    {
        private const string GOOGLE = "Google";
        private const string COOKIE_NAME = "VMCookie";

        private readonly IVolleyUserManager<UserModel> _userManager;
        private readonly IUserService _userService;
        private readonly IConfigurationProvider _configuration;

        public AccountController(
            IVolleyUserManager<UserModel> userManager,
            IUserService userService,
            IConfigurationProvider configuration)
        {
            _userManager = userManager;
            _userService = userService;
            _configuration = configuration;
        }

        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="user">json with user info provided by UI</param>
        /// <returns>Action result</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> TokenSigninGoogle(string user)
        {
            var userInfoFromWebApp = JsonConvert.DeserializeObject<GoogleUserInfo>(user);

            var validatedLoginInfoFromGoogle = await GoogleJsonWebSignature.ValidateAsync(userInfoFromWebApp.IdToken,
                new GoogleJsonWebSignature.ValidationSettings {
                    Audience = new List<string> { _configuration.GetGoogleClientId() }
                });

            var userInSystem = await _userManager.FindByEmailAsync(validatedLoginInfoFromGoogle.Email);

            if (userInSystem == null)
            {
                userInSystem = new UserModel {
                    Email = validatedLoginInfoFromGoogle.Email,
                    UserName = validatedLoginInfoFromGoogle.Email,
                    PersonName = $"{validatedLoginInfoFromGoogle.FamilyName} {validatedLoginInfoFromGoogle.GivenName}"
                };
                var result = await _userManager.CreateAsync(userInSystem);
                if (!result.Succeeded)
                {
                    return Unauthorized();
                }
                else
                {
                    result = await _userManager.AddLoginAsync(userInSystem.Id,
                        new UserLoginInfo(GOOGLE, userInSystem.Email));
                    if (!result.Succeeded)
                    {
                        result = await _userManager.DeleteAsync(userInSystem);
                        return Unauthorized();
                    }
                }
            }

            var ident = await _userManager.CreateIdentityAsync(
                userInSystem,
                DefaultAuthenticationTypes.ApplicationCookie);

            if (IsBlocked(ident.GetUserId<int>()))
            {
                return Unauthorized();
            }

            ident.AddClaims(
                new List<Claim>
                {
                    new Claim(nameof(validatedLoginInfoFromGoogle.Name), validatedLoginInfoFromGoogle.Email),
                    new Claim(nameof(validatedLoginInfoFromGoogle.FamilyName), validatedLoginInfoFromGoogle.FamilyName),
                    new Claim(nameof(validatedLoginInfoFromGoogle.GivenName), validatedLoginInfoFromGoogle.GivenName)
                });


            await HttpContext.SignOutAsync(COOKIE_NAME);
            await HttpContext.SignInAsync(COOKIE_NAME,
                new ClaimsPrincipal(ident),
                new AuthenticationProperties { IsPersistent = false }
            );

            return Ok();
        }

        /// <summary>
        /// Logout current user
        /// </summary>
        /// <returns>Action result</returns>
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }

        private bool IsBlocked(int userId)
        {
            var currentUser = _userService.GetUser(userId);
            return currentUser.IsBlocked;
        }
    }
}