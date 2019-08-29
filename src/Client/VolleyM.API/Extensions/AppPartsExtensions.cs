using System;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

namespace VolleyM.API.Extensions
{
    internal static class AppPartsExtensions
    {
        internal static IMvcBuilder AddVolleyManagementApiParts(this IMvcBuilder mvcBuilder, string assemblyPath, string assemblyPrefix = "VolleyM.API.")
        {
            var pluginAssemblies = Directory.GetFiles(assemblyPath, "*.dll", SearchOption.TopDirectoryOnly)
                .Select(AssemblyLoadContext.Default.LoadFromAssemblyPath)
                // Ensure that the assembly contains an implementation for the given type.
                .Where(s => s.FullName.StartsWith(assemblyPrefix, StringComparison.OrdinalIgnoreCase))
                .Select(a => new AssemblyPart(a))
                .ToList();

            Console.WriteLine($"API: Parts discovered. Count={pluginAssemblies.Count}.");

            return mvcBuilder.ConfigureApplicationPartManager(apm => pluginAssemblies.ForEach(apm.ApplicationParts.Add));
        }
    }
}