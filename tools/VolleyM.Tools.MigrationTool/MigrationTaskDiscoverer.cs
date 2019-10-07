using System;
using McMaster.NETCore.Plugins;
using Serilog;
using System.Collections.Generic;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
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
            var assemblies = DiscoverAssemblies(assemblyPath);

            MigrationTasks = assemblies.Select(CreateTaskInstance).Where(t => t != null).ToList();
            Log.Information("Discovered {MigrationTaskCount} migration tasks.", MigrationTasks.Count);
        }

        private static IEnumerable<Assembly> DiscoverAssemblies(string assemblyPath)
        {
            var list = new List<Assembly>();

            try
            {
                foreach (var dir in Directory.GetDirectories(assemblyPath))
                {
                    var dirName = Path.GetFileName(dir);
                    var pluginDll = Path.Combine(dir, dirName + ".dll");
                    if (File.Exists(pluginDll))
                    {
                        var loader = PluginLoader.CreateFromAssemblyFile(pluginDll, sharedTypes: new[] { typeof(IMigrationTask) });
                        list.Add(loader.LoadDefaultAssembly());
                    }
                }
            }
            catch (DirectoryNotFoundException e)
            {
                Log.Warning(e, "Plugin directory missing: {PluginDirectory}", assemblyPath);
            }
            return list;
        }

        private static IMigrationTask CreateTaskInstance(Assembly assembly)
        {
            var task = assembly.GetExportedTypes()
                .FirstOrDefault(t => t.GetInterfaces().Contains(typeof(IMigrationTask)));

            if (task == null)
            {
                Log.Warning("Assembly {AssemblyName} does not have any types implementing {InterfaceName}.",
                    assembly.GetName().Name,
                    nameof(IMigrationTask));
                return null;
            }

            try
            {
                return (IMigrationTask)Activator.CreateInstance(task);
            }
            catch (Exception e)
            {
                Log.Warning(e, "Failed to instantiate {Type} as migration task.", task);
                return null;
            }
        }
    }
}