using System;
using System.Web.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using VolleyManagement.Data.MsSql.Infrastructure;
using VolleyManagement.Services.Infrastructure;
using VolleyManagement.UI.Infrastructure;
using VolleyManagement.UI.Infrastructure.IOC;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Google;
using VolleyManagement.Services.Authentication;

namespace VolleyManagement.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            SetupGoogleProvider(services);
            services.AddMvc();
            services.AddCors();
            IntegrateSimpleInjector(services);
        }

        private void IntegrateSimpleInjector(IServiceCollection services)
        {
            var ioc = new SimpleInjectorContainer();

            ioc.InternalContainer.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            ioc
                .Register(new IocDataAccessModule())
                .Register(new IocServicesModule())
                .Register(new IocCoreUIModule());

            ioc.InternalContainer.Verify();

            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(ioc.InternalContainer);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IControllerActivator>(
                new SimpleInjectorControllerActivator(ioc.InternalContainer));

            services.EnableSimpleInjectorCrossWiring(ioc.InternalContainer);
            services.UseSimpleInjectorAspNetRequestScoping(ioc.InternalContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseMvc();
        }

        private void SetupGoogleProvider(IServiceCollection services)
        {
            services.
                AddAuthentication(
                v =>
                {
                    v.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
                    v.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
                }).AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            });
        }
    }
}
