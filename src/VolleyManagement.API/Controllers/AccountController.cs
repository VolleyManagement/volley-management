using Google.Apis.Auth;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using VolleyManagement.API.Model;
using VolleyManagement.Contracts;
using VolleyManagement.Contracts.Authentication;
using VolleyManagement.Contracts.Authentication.Models;
using VolleyManagement.Contracts.Authorization;
using VolleyManagement.Crosscutting.Contracts.Providers;

namespace VolleyManagement.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly IVolleyUserManager<UserModel> _userManager;
        private readonly IUserService _userService;
        private readonly ICacheProvider _cacheProvider;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISecretsProvider _configuration;

        public AccountController(
            IVolleyUserManager<UserModel> userManager,
            IUserService userService,
            ICacheProvider cacheProvider,
            ICurrentUserService currentUserService,
            ISecretsProvider configuration)
        {
            _userManager = userManager;
            _userService = userService;
            _cacheProvider = cacheProvider;
            _currentUserService = currentUserService;
            _configuration = configuration;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> TokenSignin(string user)
        {
            var userInfoFromWebApp = JsonConvert.DeserializeObject<GoogleUserInfo>(user);

            var validatedLoginInfoFromGoogle = await GoogleJsonWebSignature.ValidateAsync(userInfoFromWebApp.IdToken,
                new GoogleJsonWebSignature.ValidationSettings() {
                    Audience = new List<string> { _configuration.GetGoogleClientId().Value }
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
                    result = await _userManager.AddLoginAsync(userInSystem.Id, new UserLoginInfo("Google", userInSystem.Email));
                    if (!result.Succeeded)
                    {
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
                new List<Claim>()
                {
                    new Claim(nameof(validatedLoginInfoFromGoogle.Name), validatedLoginInfoFromGoogle.Email),
                    new Claim(nameof(validatedLoginInfoFromGoogle.FamilyName), validatedLoginInfoFromGoogle.FamilyName),
                    new Claim(nameof(validatedLoginInfoFromGoogle.GivenName), validatedLoginInfoFromGoogle.GivenName)
                });


            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(ident),
                new AuthenticationProperties {
                    IsPersistent = false,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                });

            // TODO Commented cause receive errors here. Need to debug.
            //AddToActive(userInSystem.Id);

            return Ok();
        }

        /// <summary>
        /// Logs current user
        /// </summary>
        /// <param name="returnUrl">URL to return</param>
        /// <returns>Action result</returns>
        [AllowAnonymous]
        public async Task<IActionResult> Logout(string returnUrl)
        {
            // TODO Commented cause receive errors here. Need to debug.
            //DeleteFromActive(_currentUserService.GetCurrentUserId());
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }

        private void AddToActive(int id)
        {
            var activeUsersList = _cacheProvider["ActiveUsers"] as List<int> ?? new List<int>();

            if (!activeUsersList.Contains(id))
            {
                activeUsersList.Add(id);
            }

            _cacheProvider["ActiveUsers"] = activeUsersList;
        }

        private void DeleteFromActive(int id)
        {
            var activeUsersList = _cacheProvider["ActiveUsers"] as List<int> ?? new List<int>();

            if (activeUsersList.Contains(id))
            {
                activeUsersList.Remove(activeUsersList.Find(x => x == id));
            }

            _cacheProvider["ActiveUsers"] = activeUsersList;
        }

        private bool IsBlocked(int userId)
        {
            var currentUser = _userService.GetUser(userId);
            return currentUser.IsBlocked;
        }
    }
}