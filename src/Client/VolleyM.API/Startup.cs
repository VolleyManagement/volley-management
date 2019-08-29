using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleInjector;
using VolleyM.API.Extensions;

namespace VolleyM.API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private readonly Container _container = DiExtensions.CreateContainer();

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddVolleyManagementApiParts(_config[Constants.VM_PLUGIN_PATH]);

            services.AddSimpleInjector(_container, options =>
            {
                // AddAspNetCore() wraps web requests in a Simple Injector scope.
                options.AddAspNetCore()
                    .AddControllerActivation();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            _container.RegisterApplicationServices(_config[Constants.VM_PLUGIN_PATH]);

            _container.Verify();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World! CD is working and pipeline triggers for master only. Yeah!");
                });
                endpoints.MapControllers();
            });
        }
    }
}
