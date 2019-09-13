using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Serilog;
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

            Log.Information("API: {APIParts} Parts discovered.", pluginAssemblies.Count);

            return mvcBuilder.ConfigureApplicationPartManager(apm => pluginAssemblies.ForEach(apm.ApplicationParts.Add));
        }
    }
}