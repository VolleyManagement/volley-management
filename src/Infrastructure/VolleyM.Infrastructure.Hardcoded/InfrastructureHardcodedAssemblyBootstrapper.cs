using AutoMapper.Configuration;
using SimpleInjector;
using System.Composition;
using System.Reflection;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Framework.Authorization;
using VolleyM.Infrastructure.Bootstrap;

namespace VolleyM.Infrastructure.Hardcoded
{
    [Export(typeof(IAssemblyBootstrapper))]
    public class InfrastructureHardcodedAssemblyBootstrapper : IAssemblyBootstrapper
    {
        public void RegisterDependencies(Container container, Microsoft.Extensions.Configuration.IConfiguration config)
        {
            container.Register<IRolesStore, HardcodedRolesStore>(Lifestyle.Scoped);

            container.Register(typeof(IQuery<,>), Assembly.GetAssembly(GetType()), Lifestyle.Scoped);
        }

        public bool HasDomainComponents { get; } = false;

        public IDomainComponentDependencyRegistrar DomainComponentDependencyRegistrar { get; } = null;

        public void RegisterMappingProfiles(MapperConfigurationExpression mce)
        {
            // no mapping
        }
    }
}
