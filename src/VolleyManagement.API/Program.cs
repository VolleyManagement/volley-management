using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using VolleyM.Infrastructure.Bootstrap;

namespace VolleyManagement.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var bootstrapper = new AssemblyBootstrapper();

            bootstrapper.Compose(Directory.GetCurrentDirectory());

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
