using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VolleyManagement.API.Infrastructure;
using VolleyManagement.Contracts;
using VolleyManagement.Crosscutting.Contracts.Infrastructure.IOC;
using VolleyManagement.Data.Contracts;
using VolleyManagement.Data.MsSql;
using VolleyManagement.Data.MsSql.Context;
using VolleyManagement.Data.MsSql.Infrastructure;
using VolleyManagement.Data.MsSql.Repositories;
using VolleyManagement.Domain.TournamentsAggregate;
using VolleyManagement.Services;
using VolleyManagement.Services.Infrastructure;

namespace VolleyManagement.API
{
    public class Startup
    {
        const string ID_KEY = "GoogleClientId";
        const string SECRET_KEY = "GoogleClientSecret";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            ConfigureIoc(services);
            //services.AddDbContext<VolleyManagementEntities>(options =>
               // options.UseSqlServer(Configuration.GetConnectionString("VolleyManagementEntities")));
            //var googleId = Configuration[ID_KEY]; ;
            //var googleSecret = Configuration[SECRET_KEY];

            //if (googleId != null && googleSecret != null)
            //{
            //    services.AddAuthentication().AddGoogle(googleOptions =>
            //    {
            //        googleOptions.ClientId = googleId;
            //        googleOptions.ClientSecret = googleSecret;
            //    });
            //}
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //var options = new RewriteOptions().AddRedirectToHttps();
            //app.UseRewriter(options);

            app.UseMvc();
        }

        private static void ConfigureIoc(IServiceCollection services)
        {
            new IocCoreDataAccessModule().RegisterDependencies(services);
            new IocCoreServicesModule().RegisterDependencies(services);
            new IocCoreApiModule().RegisterDependencies(services);
        }
    }
}
