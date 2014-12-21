namespace VolleyManagement.Dal.MsSql.Infrastructure
{
    using Ninject.Modules;
    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.Dal.MsSql.Services;

    /// <summary>
    /// Defines bindings for Service layer
    /// </summary>
    public class NinjectDataAccessModule : NinjectModule
    {
        /// <summary>
        /// Loads bindings
        /// </summary>
        public override void Load()
        {
            Bind<IUnitOfWork>().To<VolleyUnitOfWork>().InSingletonScope();
            Bind<ITournamentRepository>().To<TournamentRepository>().InSingletonScope();
            Bind<IUserRepository>().To<UserRepository>().InSingletonScope();
        }
    }
}
