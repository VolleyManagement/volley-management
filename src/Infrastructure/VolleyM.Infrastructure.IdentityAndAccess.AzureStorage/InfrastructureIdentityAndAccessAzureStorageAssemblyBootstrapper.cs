using AutoMapper.Configuration;
using SimpleInjector;
using System.Composition;
using VolleyM.Domain.IdentityAndAccess;
using VolleyM.Infrastructure.Bootstrap;

namespace VolleyM.Infrastructure.IdentityAndAccess.AzureStorage
{
    [Export(typeof(IAssemblyBootstrapper))]
    public class InfrastructureIdentityAndAccessAzureStorageAssemblyBootstrapper : IAssemblyBootstrapper
    {
        public void RegisterDependencies(Container container)
        {
            container.Register<IUserRepository, AzureStorageUserRepository>(Lifestyle.Scoped);
        }

        public void RegisterMappingProfiles(MapperConfigurationExpression mce)
        {
            //no mapping
        }
    }
}