using System.Composition;
using AutoMapper.Configuration;
using SimpleInjector;
using VolleyM.Infrastructure.Bootstrap;

namespace VolleyM.API.Contributors
{
    [Export(typeof(IAssemblyBootstrapper))]
    public class ContributorsApiAssemblyBootstrapper : IAssemblyBootstrapper
    {
        public void RegisterDependencies(Container container)
        {
            // no dependencies
        }

        public void RegisterMappingProfiles(MapperConfigurationExpression mce) =>
            mce.AddProfile<ContributorsApiMappingProfile>();
    }
}