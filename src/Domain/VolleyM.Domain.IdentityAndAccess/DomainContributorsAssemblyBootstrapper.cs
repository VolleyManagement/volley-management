using AutoMapper.Configuration;
using SimpleInjector;
using System.Composition;
using VolleyM.Infrastructure.Bootstrap;

namespace VolleyM.Domain.IdentityAndAccess
{
    [Export(typeof(IAssemblyBootstrapper))]
    public class DomainIdentityAndAccessAssemblyBootstrapper : IAssemblyBootstrapper
    {
        public void RegisterDependencies(Container container, Microsoft.Extensions.Configuration.IConfiguration config)
        {
            container.Register<UserFactory>(Lifestyle.Singleton);
        }

        public bool HasDomainComponents { get; } = true;
        public IDomainComponentDependencyRegistrar DomainComponentDependencyRegistrar { get; } = null;

        public void RegisterMappingProfiles(MapperConfigurationExpression mce)
        {
            // no mapping
        }
    }
}
