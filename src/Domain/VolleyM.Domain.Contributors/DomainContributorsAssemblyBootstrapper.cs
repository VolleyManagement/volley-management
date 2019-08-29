using System.Composition;
using System.Reflection;
using AutoMapper.Configuration;
using SimpleInjector;
using VolleyM.Domain.Contracts;
using VolleyM.Infrastructure.Bootstrap;

namespace VolleyM.Domain.Contributors
{
    [Export(typeof(IAssemblyBootstrapper))]
    public class DomainContributorsAssemblyBootstrapper : IAssemblyBootstrapper
    {
        public void RegisterDependencies(Container container)
        {
            container.Register(typeof(IRequestHandler<,>), Assembly.GetAssembly(GetType()), Lifestyle.Scoped);
        }

        public void RegisterMappingProfiles(MapperConfigurationExpression mce)
        {
            // no mapping
        }
    }
}
