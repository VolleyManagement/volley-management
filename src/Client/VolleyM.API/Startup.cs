using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace VolleyM.API
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var contributors = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("VolleyM.API.Contributors"));

            services.AddControllers()
                .AddApplicationPart(contributors);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
