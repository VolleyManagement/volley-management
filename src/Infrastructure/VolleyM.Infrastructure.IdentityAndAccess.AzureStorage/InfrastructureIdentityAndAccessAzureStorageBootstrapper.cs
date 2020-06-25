using AutoMapper.Configuration;
using Microsoft.Extensions.Configuration;
using SimpleInjector;
using System.Composition;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.IdentityAndAccess;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;
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

        public bool HasDomainComponents { get; } = false;

        public IDomainComponentDependencyRegistrar DomainComponentDependencyRegistrar { get; } = null;

        public void RegisterMappingProfiles(MapperConfigurationExpression mce)
        {
            mce.CreateMap<UserEntity, UserFactoryDto>()
                .ForMember(m => m.Id,
                    m => m.MapFrom(src => new UserId(src.RowKey)))
                .ForMember(m => m.Tenant,
                    m => m.MapFrom(src => new TenantId(src.PartitionKey)))
                .ForMember(m => m.Role,
                    m =>
                    {
                        m.Condition(src => !string.IsNullOrEmpty(src.RoleId));
                        m.MapFrom(src => new RoleId(src.RoleId));
                    });
        }
    }
}