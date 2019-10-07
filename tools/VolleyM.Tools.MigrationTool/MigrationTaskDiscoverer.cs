using System;
using System.Collections.Generic;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Serilog;
using VolleyM.Tools.AzureStorageMigrator.Contracts;

namespace VolleyM.Tools.AzureStorageMigrator
{
    public class MigrationTaskDiscoverer
    {
        public List<IMigrationTask> MigrationTasks { get; private set; }

        public void Discover(string assemblyPath)
        {
            Log.Debug("MigrationTaskDiscoverer started");
            Log.Debug("Path to assemblies {MigrationTasksPath}.", assemblyPath);

            // Catalogs does not exists in Dotnet Core, so you need to manage your own.
            var assemblies = DiscoverAssemblies(assemblyPath, "VolleyM.");
            var configuration = new ContainerConfiguration()
                .WithAssemblies(assemblies);

            using var container = configuration.CreateContainer();
            MigrationTasks = container.GetExports<IMigrationTask>().ToList();
            Log.Information("Discovered {MigrationTaskCount} migration tasks.", MigrationTasks.Count);
        }

        private static IEnumerable<Assembly> DiscoverAssemblies(string assemblyPath, string assemblyPrefix)
        {
            var list = new List<Assembly>();

            try
            {
                var pluginAssemblies = Directory.GetFiles(assemblyPath, "*.dll", SearchOption.TopDirectoryOnly)
                    .Select(AssemblyLoadContext.Default.LoadFromAssemblyPath)
                    // Ensure that the assembly contains an implementation for the given type.
                    .Where(s => s.FullName.StartsWith(assemblyPrefix, StringComparison.OrdinalIgnoreCase));
                list.AddRange(pluginAssemblies);
            }
            catch (DirectoryNotFoundException e)
            {
                Log.Warning(e, "Plugin directory missing: {PluginDirectory}", assemblyPath);
            }
            return list;
        }
    }
}