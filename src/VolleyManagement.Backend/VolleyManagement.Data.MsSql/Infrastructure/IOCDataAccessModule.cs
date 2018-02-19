namespace VolleyManagement.Data.MsSql.Infrastructure
{
    using VolleyManagement.Crosscutting.Contracts.Infrastructure.IOC;

    public class IocDataAccessModule : IIocRegistrationModule
    {
        public void RegisterDependencies(IIocContainer container)
        {
            UserRelatedRepositoriesRegistrator.Register(container);
            RepositoriesRegistrator.Register(container);
            QueriesRegistrator.Register(container);
            EntityConfigurationRegistrator.Register(container);
        }
    }
}
