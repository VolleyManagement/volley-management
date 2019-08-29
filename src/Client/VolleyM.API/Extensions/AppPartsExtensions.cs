using System;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using VolleyM.Infrastructure.Bootstrap;

namespace VolleyM.API.Extensions
{
    internal static class AppPartsExtensions
    {
        internal static IMvcBuilder AddVolleyManagementApiParts(this IMvcBuilder mvcBuilder, AssemblyBootstrapper assemblyBootstrapper, string assemblyPrefix = "VolleyM.API.")
        {
            var pluginAssemblies = assemblyBootstrapper.DiscoveredAssemblies
                .Where(s => s.FullName.StartsWith(assemblyPrefix, StringComparison.OrdinalIgnoreCase))
                .Select(a => new AssemblyPart(a))
                .ToList();

            Console.WriteLine($"API: Parts discovered. Count={pluginAssemblies.Count}.");

            return mvcBuilder.ConfigureApplicationPartManager(apm => pluginAssemblies.ForEach(apm.ApplicationParts.Add));
        }
    }
}