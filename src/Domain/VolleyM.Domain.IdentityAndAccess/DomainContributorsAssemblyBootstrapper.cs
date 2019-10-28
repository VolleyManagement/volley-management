using AutoMapper.Configuration;
using SimpleInjector;
using System.Composition;
using System.Reflection;
using VolleyM.Domain.Contracts.Crosscutting;
using VolleyM.Infrastructure.Bootstrap;

namespace VolleyM.Domain.IdentityAndAccess
{
    [Export(typeof(IAssemblyBootstrapper))]
    public class DomainIdentityAndAccessAssemblyBootstrapper : IAssemblyBootstrapper
    {
        public void RegisterDependencies(Container container, Microsoft.Extensions.Configuration.IConfiguration config)
        {
            container.RegisterCommonDomainServices(Assembly.GetAssembly(GetType()));

            container.Register<UserFactory>(Lifestyle.Singleton);
        }

        public void RegisterMappingProfiles(MapperConfigurationExpression mce)
        {
            // no mapping
        }
    }
}
