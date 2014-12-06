namespace VolleyManagement.Services.Infrastructure
{
    using Ninject.Modules;
    using VolleyManagement.Contracts;
    using VolleyManagement.Services;

    /// <summary>
    /// Defines bindings for Service layer
    /// </summary>
    public class NinjectServiceBindModule : NinjectModule
    {
        /// <summary>
        /// Loads bindings
        /// </summary>
        public override void Load()
        {
            Bind<ITournamentService>().To<TournamentService>().InSingletonScope();
        }
    }
}
