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
    public class Startup
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
        private void IntegrateSimpleInjector(IServiceCollection services)
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
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.Name = COOKIE_NAME;
                options.Cookie.HttpOnly = false;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.Path = "/";
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

        private string GetVolleyManagementEntitiesConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory).AddJsonFile("appsettings.json", true).Build();
            return builder.GetConnectionString("VolleyManagementEntities");
        }
    }
}
