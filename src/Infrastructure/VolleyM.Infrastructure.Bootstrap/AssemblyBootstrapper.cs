using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using AutoMapper.Configuration;
using SimpleInjector;

namespace VolleyM.Infrastructure.Bootstrap
{
    public class AssemblyBootstrapper
    {
        /// <summary>
        /// Initializes Bootstrapper
        /// </summary>
        public AssemblyBootstrapper() =>
            DiscoveredAssemblies = Enumerable.Empty<Assembly>().ToImmutableList();

        /// <summary>
        /// Returns all assemblies that were discovered by bootstrapper during compose
        /// </summary>
        public ImmutableList<Assembly> DiscoveredAssemblies { get; private set; }

        private List<IAssemblyBootstrapper> Bootstrappers { get; set; }

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
            Bootstrappers = container.GetExports<IAssemblyBootstrapper>().ToList();
            var bootstrappedAssemblies = new List<Assembly>();
            Console.WriteLine($"Discovered {Bootstrappers.Count} bootstrappers.");

            foreach (var bootstrapper in Bootstrappers)
            {
                bootstrappedAssemblies.Add(Assembly.GetAssembly(bootstrapper.GetType()));
            }

            DiscoveredAssemblies = bootstrappedAssemblies.ToImmutableList();
        }

        public void RegisterDependencies(Container iocContainer)
        {
            foreach (var bootstrapper in Bootstrappers)
            {
                RunAction(p => p.RegisterDependencies(iocContainer), bootstrapper, "Register Dependencies");
            }
        }

        public void RegisterMappingProfiles(MapperConfigurationExpression mce)
        {
            foreach (var bootstrapper in Bootstrappers)
            {
                RunAction(p => p.RegisterMappingProfiles(mce), bootstrapper, "Mapper Profiles");
            }
        }

        private void RunAction(Action<IAssemblyBootstrapper> action, IAssemblyBootstrapper instance, string name)
        {
            try
            {
                action(instance);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to {name} in bootstrapper. Type={instance.GetType().FullName}. Exception={e}");
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