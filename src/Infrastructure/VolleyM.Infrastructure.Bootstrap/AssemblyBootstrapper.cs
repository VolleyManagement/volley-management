using System;
using System.Collections.Generic;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using SimpleInjector;

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
        public void Compose(Container iocContainer, string assemblyPath)
        {
            // Catalogs does not exists in Dotnet Core, so you need to manage your own.
            var assemblies = DiscoverAssemblies(assemblyPath, "VolleyM.");
            var configuration = new ContainerConfiguration()
                .WithAssemblies(assemblies);

            using var container = configuration.CreateContainer();
            var bootstrappers = container.GetExports<IAssemblyBootstrapper>().ToList();
            Console.WriteLine($"Discovered {bootstrappers.Count} bootstrappers.");

            void RegisterBootstrapper(IAssemblyBootstrapper assemblyBootstrapper)
            {
                try
                {
                    assemblyBootstrapper.Register(iocContainer);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to register bootstrapper. Type={assemblyBootstrapper.GetType().FullName}. Exception={e}");
                }
            }

            bootstrappers.ForEach(RegisterBootstrapper);
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