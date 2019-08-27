using System;
using System.Collections.Generic;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;

namespace VolleyM.Infrastructure.Bootstrap
{
    public class AssemblyBootstrapper
    {
        /// <summary>
        /// Initializes Bootstrapper
        /// </summary>
        public AssemblyBootstrapper()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyPath">Location where bootstrapped assemblies located</param>
        public void Compose(string assemblyPath)
        {
            // Catalogs does not exists in Dotnet Core, so you need to manage your own.
            var assemblies = DiscoverAssemblies(assemblyPath, "VolleyM.");
            var configuration = new ContainerConfiguration()
                .WithAssemblies(assemblies);

            using var container = configuration.CreateContainer();
            var bootstrappers = container.GetExports<IAssemblyBootstrapper>().ToList();
            Console.WriteLine($"Discovered {bootstrappers.Count} bootstrappers.");

            static Task RegisterBootstrapper(IAssemblyBootstrapper assemblyBootstrapper)
            {
                try
                {
                    return assemblyBootstrapper.Register();
                }
                catch (Exception e)
                {
                    return Task.FromException(e);
                }
            }

            var registerTasks = bootstrappers.Select(b => (Name: b.GetType().FullName, Task: RegisterBootstrapper(b)))
                .ToList();

            Task.WhenAll(registerTasks.Select(t => t.Task));

            foreach (var (name, task) in registerTasks)
            {
                if (task.IsFaulted)
                {
                    Console.WriteLine($"Failed to register bootstrapper. Type={name}. Exception={task.Exception}");
                }
            }
        }

        private static IEnumerable<Assembly> DiscoverAssemblies(string assemblyPath, string assemblyPrefix)
        {
            var list = new List<Assembly>();
            var pluginAssemblies = Directory.GetFiles(assemblyPath, "*.dll", SearchOption.TopDirectoryOnly)
                .Select(AssemblyLoadContext.Default.LoadFromAssemblyPath)
                // Ensure that the assembly contains an implementation for the given type.
                .Where(s => s.FullName.StartsWith(assemblyPrefix, StringComparison.OrdinalIgnoreCase));
            list.AddRange(pluginAssemblies);
            return list;
        }
    }
}