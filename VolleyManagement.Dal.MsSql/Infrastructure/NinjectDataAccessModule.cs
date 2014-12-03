namespace VolleyManagement.Dal.MsSql.Infrastructure
{
    using Ninject.Modules;

    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.Dal.MsSql.Services;

    public class NinjectDataAccessModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUnitOfWork>().To<EFVolleyUnitOfWork>();
            Bind<ITournamentRepository>().To<TournamentRepository>();
        }
    }
}
