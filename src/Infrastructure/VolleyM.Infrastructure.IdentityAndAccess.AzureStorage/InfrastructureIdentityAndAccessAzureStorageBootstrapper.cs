using AutoMapper.Configuration;
using Microsoft.Extensions.Configuration;
using SimpleInjector;
using System.Composition;
using VolleyM.Domain.IdentityAndAccess;
using VolleyM.Infrastructure.Bootstrap;
using VolleyM.Infrastructure.IdentityAndAccess.AzureStorage.TableConfiguration;

namespace VolleyM.Infrastructure.IdentityAndAccess.AzureStorage
{
    [Export(typeof(IAssemblyBootstrapper))]
    public class InfrastructureIdentityAndAccessAzureStorageBootstrapper : IAssemblyBootstrapper
    {
        public void RegisterDependencies(Container container, Microsoft.Extensions.Configuration.IConfiguration config)
        {
            container.Register<IUserRepository, AzureStorageUserRepository>(Lifestyle.Scoped);

            var options = config.GetSection("IdentityContextTableStorageOptions")
                .Get<IdentityContextTableStorageOptions>();

            container.RegisterInstance(options);
        }

        public void RegisterMappingProfiles(MapperConfigurationExpression mce)
        {
            //no mapping
        }
    }
}