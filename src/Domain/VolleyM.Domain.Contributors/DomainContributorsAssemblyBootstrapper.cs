using AutoMapper.Configuration;
using SimpleInjector;
using System.Composition;
using VolleyM.Infrastructure.Bootstrap;

namespace VolleyM.Domain.Contributors
{
    [Export(typeof(IAssemblyBootstrapper))]
    public class DomainContributorsAssemblyBootstrapper : IAssemblyBootstrapper
    {
        public void RegisterDependencies(Container container, Microsoft.Extensions.Configuration.IConfiguration config)
        {
            // do nothing
        }

        public bool HasDomainComponents { get; } = true;
        public IDomainComponentDependencyRegistrar DomainComponentDependencyRegistrar { get; } = null;

        public void RegisterMappingProfiles(MapperConfigurationExpression mce)
        {
            // no mapping
        }
    }
}
