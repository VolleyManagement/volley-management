namespace VolleyManagement.Services.Infrastructure
{
    using Ninject.Modules;

    using VolleyManagement.Services;
    using VolleyManagement.Contracts;

    public class NinjectServiceBindModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ITournamentService>().To<TournamentService>();
        }
    }
}
