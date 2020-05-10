using Esquio.Abstractions;
using Esquio.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SimpleInjector;
using VolleyM.API.Authentication;
using VolleyM.API.Authorization;
using VolleyM.API.CORS;
using VolleyM.API.Extensions;
using VolleyM.Domain.Contracts.Crosscutting;
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

			services.AddControllers(opts =>
				{
					opts.Filters.Add(new AuthorizeFilter());
				})
				.AddVolleyManagementApiParts(_assemblyBootstrapper);

			services.AddDefaultVolleyMAuthorization(_container);

			services.AddSimpleInjector(_container, options =>
			{
				options.AutoCrossWireFrameworkComponents = false;

				options.CrossWire<IFeatureService>();

				// AddAspNetCore() wraps web requests in a Simple Injector scope.
				options.AddAspNetCore()
					.AddControllerActivation();
			});

			services.AddEsquio(opts => opts.ConfigureNotFoundBehavior(NotFoundBehavior.SetEnabled))
				.AddAspNetCoreDefaultServices()
				.AddConfigurationStore(_config, "Esquio");
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			_container.RegisterInstance<IApplicationInfo>(new VolleyMApiApplicationInfo(env.IsProduction()));

			app.RegisterApplicationServices(_container, _assemblyBootstrapper, _config);

			_container.Verify();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseSerilogRequestLogging();

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
				endpoints.MapEsquio(pattern: "esquio");
			});
		}
	}
}
