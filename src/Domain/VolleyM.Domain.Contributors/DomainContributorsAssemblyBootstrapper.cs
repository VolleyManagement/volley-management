using AutoMapper.Configuration;
using SimpleInjector;
using System.Composition;
using System.Reflection;
using VolleyM.Domain.Contracts.Crosscutting;
using VolleyM.Infrastructure.Bootstrap;

namespace VolleyM.Domain.Contributors
{
    [Export(typeof(IAssemblyBootstrapper))]
    public class DomainContributorsAssemblyBootstrapper : IAssemblyBootstrapper
    {
        public void RegisterDependencies(Container container, Microsoft.Extensions.Configuration.IConfiguration config)
        {
            container.RegisterCommonDomainServices(Assembly.GetAssembly(GetType()));
        }

        public void RegisterMappingProfiles(MapperConfigurationExpression mce)
        {
            // no mapping
        }
    }
}
