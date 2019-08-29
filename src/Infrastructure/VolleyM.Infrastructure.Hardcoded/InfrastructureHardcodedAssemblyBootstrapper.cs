using System;
using System.Composition;
using System.Linq;
using System.Reflection;
using AutoMapper.Configuration;
using SimpleInjector;
using VolleyM.Infrastructure.Bootstrap;

namespace VolleyM.Infrastructure.Hardcoded
{
    [Export(typeof(IAssemblyBootstrapper))]
    public class InfrastructureHardcodedAssemblyBootstrapper : IAssemblyBootstrapper
    {
        public void RegisterDependencies(Container container)
        {
            var repositoryAssembly = Assembly.GetAssembly(GetType());

            var registrations =
                from type in repositoryAssembly.GetExportedTypes()
                where type.Name.EndsWith("query", StringComparison.OrdinalIgnoreCase)
                from service in type.GetInterfaces()
                select new { service, implementation = type };

            foreach (var reg in registrations)
            {
                container.Register(reg.service, reg.implementation, Lifestyle.Singleton);
            }
        }
        public void RegisterMappingProfiles(MapperConfigurationExpression mce)
        {
            // no mapping
        }
    }
}
