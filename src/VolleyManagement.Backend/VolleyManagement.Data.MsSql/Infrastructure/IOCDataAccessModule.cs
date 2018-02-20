namespace VolleyManagement.Data.MsSql.Infrastructure
{
    using VolleyManagement.Crosscutting.Contracts.Infrastructure.IOC;

    public class IocDataAccessModule : IIocRegistrationModule
    {
        public void RegisterDependencies(IIocContainer container)
        {
            RepositoriesRegistrator.Register(container);
            QueriesRegistrator.Register(container);
        }
    }
}
