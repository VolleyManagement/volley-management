using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleInjector;
using VolleyM.API.Authentication;
using VolleyM.API.CORS;
using VolleyM.API.Extensions;
using VolleyM.Infrastructure.Bootstrap;

namespace VolleyM.API
{
    public class Startup
    {
        private readonly IConfiguration _config;

        private readonly Container _container = DiExtensions.CreateContainer();
        private readonly AssemblyBootstrapper _assemblyBootstrapper = new AssemblyBootstrapper();

        public Startup(IConfiguration config)
        {
            _config = config;

            _assemblyBootstrapper.Compose(_config[Constants.VM_PLUGIN_PATH]);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.UseJwtAuth()
                .AddAuth0JwtBearer(_config.GetSection("Auth0").Get<Auth0Options>());

            services.AddCorsFromSettings(_config.GetSection("CORS").Get<CorsOptions>());

            services.AddControllers()
                .AddVolleyManagementApiParts(_assemblyBootstrapper);

            services.AddSimpleInjector(_container, options =>
            {
                // AddAspNetCore() wraps web requests in a Simple Injector scope.
                options.AddAspNetCore()
                    .AddControllerActivation();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            _container.RegisterApplicationServices(_assemblyBootstrapper);

            _container.Verify();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

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
