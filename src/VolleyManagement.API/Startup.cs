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
            services.AddMvc();
            IntegrateSimpleInjector(services);
        }

        private void IntegrateSimpleInjector(IServiceCollection services)
        {
            var ioc = new SimpleInjectorContainer();

            ioc.InternalContainer.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            ioc
                .Register(new IocDataAccessModule())
                .Register(new IocServicesModule())
                .Register(new IocUiModule());

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

            app.UseMvc();
        }
    }
}
