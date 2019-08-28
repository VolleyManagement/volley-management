using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using VolleyM.Infrastructure.Bootstrap;

namespace VolleyM.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var bootstrapper = new AssemblyBootstrapper();
            var pluginPath = GetPluginPath();
            bootstrapper.Compose(pluginPath);

            CreateHostBuilder(args, pluginPath)
                .Build()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args, string pluginPath) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseSetting(Constants.VM_PLUGIN_PATH, pluginPath);
                    webBuilder.UseStartup<Startup>();
                });

        private static string GetPluginPath()
        {
            var execAssembly = Assembly.GetExecutingAssembly();
            var assemblyUri = new Uri(execAssembly.CodeBase);
            var path = Path.GetDirectoryName(assemblyUri.LocalPath);

            return path;
        }
    }
}
