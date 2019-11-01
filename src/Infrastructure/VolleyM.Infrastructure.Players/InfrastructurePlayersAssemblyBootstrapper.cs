using AutoMapper.Configuration;
using SimpleInjector;
using System.Composition;
using System.Reflection;
using VolleyM.Domain.Contracts;

using VolleyM.Infrastructure.Bootstrap;

namespace VolleyM.Infrastructure.Players
{
    [Export(typeof(IAssemblyBootstrapper))]
    class InfrastructurePlayersAssemblyBootstrapper : IAssemblyBootstrapper
    {
        public void RegisterDependencies(Container container, Microsoft.Extensions.Configuration.IConfiguration config)
        {
            container.Register(typeof(IQuery<,>), Assembly.GetAssembly(GetType()), Lifestyle.Scoped);
        }
        public void RegisterMappingProfiles(MapperConfigurationExpression mce)
        {
            // no mapping
        }
    }
}
