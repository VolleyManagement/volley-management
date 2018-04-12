using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using System.Threading.Tasks;
using System.Web.Http;
using VolleyManagement.API.Infrastructure;
using VolleyManagement.Crosscutting.IOC;
using VolleyManagement.Data.MsSql.Context;
using VolleyManagement.Data.MsSql.Infrastructure;
using VolleyManagement.Services.Infrastructure;

namespace VolleyManagement.API
{
#pragma warning disable S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)
    public class Startup
#pragma warning restore S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)
    {
        private const string COOKIE_NAME = "VMCookie";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            InitializeConnectionString();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc();

            IntegrateSimpleInjector(services);

            services.AddIdentity<IdentityUser, IdentityRole>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).
                AddCookie(COOKIE_NAME, ConfigureCookies());

            services.Configure<MvcOptions>(options => { options.Filters.Add(new RequireHttpsAttribute()); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());

            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvcWithDefaultRoute();
        }

        /// <summary>
        /// Integrates SimpleInjector
        /// </summary>
        /// <param name="services">Interface <see cref="IServiceCollection"/></param>
        private static void IntegrateSimpleInjector(IServiceCollection services)
        {
            var ioc = new SimpleInjectorContainer();

            ioc.InternalContainer.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            ioc
                .Register(new IocDataAccessModule())
                .Register(new IocServicesModule())
                .Register(new IocCoreUIModule());

            ioc.InternalContainer.Verify();

            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(ioc.InternalContainer);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IControllerActivator>(
                new SimpleInjectorControllerActivator(ioc.InternalContainer));

            services.EnableSimpleInjectorCrossWiring(ioc.InternalContainer);
            services.UseSimpleInjectorAspNetRequestScoping(ioc.InternalContainer);
        }

        /// <summary>
        /// Configure cookies
        /// </summary>
        /// <returns>Instance of <see cref="CookieAuthenticationOptions"/></returns>
        private static Action<CookieAuthenticationOptions> ConfigureCookies()
        {
            return options =>
            {
                // A flag indicating if the cookie created should be the same protocol as the request
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                // Sets the name of the cookie.
                options.Cookie.Name = COOKIE_NAME;
                // A flag indicating if the cookie should be accessible only to servers.
                options.Cookie.HttpOnly = false;
                // Indicates whether the browser should allow the cookie to be attached to
                // cross -site requests using safe HTTP methods and same-site requests (SameSiteMode.Lax)
                options.Cookie.SameSite = SameSiteMode.Lax;
                // Used to isolate apps running on the same host name.
                options.Cookie.Path = "/";
                // A delegate assigned to this property will be invoked when the related method
                // is called.
                options.Events.OnRedirectToLogin = (context) =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            };
        }

        private void InitializeConnectionString()
        {
            VolleyManagementDbContextFactory.ConnectionNameOrString = GetVolleyManagementEntitiesConnectionString();
        }

        private static string GetVolleyManagementEntitiesConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory).AddJsonFile("appsettings.json", true).Build();
            return builder.GetConnectionString("VolleyManagementEntities");
        }
    }
}
