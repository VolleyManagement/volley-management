using AutoMapper.Configuration;
using SimpleInjector;
using System.Composition;
using VolleyM.Infrastructure.Bootstrap;

namespace VolleyM.API.Contributors
{
    [Export(typeof(IAssemblyBootstrapper))]
    public class ContributorsApiAssemblyBootstrapper : IAssemblyBootstrapper
    {
        public void RegisterDependencies(Container container, Microsoft.Extensions.Configuration.IConfiguration config)
        {
            // no dependencies
        }

        public void RegisterMappingProfiles(MapperConfigurationExpression mce) =>
            mce.AddProfile<ContributorsApiMappingProfile>();
    }
}