namespace VolleyManagement.UI
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Configuration;
    using System.Web.Mvc;
    using Contracts.Authentication.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.Google;
    using Owin;
    using Services.Authentication;

    /// <summary>
    /// The OWIN config.
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public class OwinConfig
    {
        /// <summary>
        /// Configures OWIN
        /// </summary>
        /// <param name="app">App builder</param>
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuthentication(app);
        }

        private void ConfigureAuthentication(IAppBuilder app)
        {
            var cookieOptions = new CookieAuthenticationOptions { LoginPath = new PathString("/Account/Login") };
            cookieOptions.AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie;
            cookieOptions.Provider = new CookieAuthenticationProvider { OnValidateIdentity = OnValidateIdentityAsync };

            app.UseCookieAuthentication(cookieOptions);

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            SetupGoogleProvider(app);
        }

        private void SetupGoogleProvider(IAppBuilder app)
        {
            const string ID_KEY = "GoogleClientId";
            const string SECRET_KEY = "GoogleClientSecret";

            var googleId = WebConfigurationManager.AppSettings[ID_KEY];
            var googleSecret = WebConfigurationManager.AppSettings[SECRET_KEY];

            if (googleId != null && googleSecret != null)
            {
                app.UseGoogleAuthentication(
                    new GoogleOAuth2AuthenticationOptions {
                        ClientId = googleId,
                        ClientSecret = googleSecret
                    });
            }
        }

        private static Task OnValidateIdentityAsync(Microsoft.Owin.Security.Provider.BaseContext<CookieAuthenticationOptions> ctx)
        {
            RegisterUserManagerInOwinContext(ctx.OwinContext);
            SecurityStampValidator.OnValidateIdentity<VolleyUserManager, UserModel, int>(
                validateInterval: TimeSpan.FromMinutes(30),
                regenerateIdentityCallback: async (manager, user) =>
                    await manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie),
                getUserIdCallback: ci => ci.GetUserId<int>());
            return Task.FromResult(false);
        }

        private static void RegisterUserManagerInOwinContext(IOwinContext context)
        {
            var userManager = DependencyResolver.Current.GetService<VolleyUserManager>();
            context.Set(userManager);
        }
    }
}