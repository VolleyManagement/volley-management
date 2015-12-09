namespace VolleyManagement.Services.Infrastructure
{
    using System;

    using Ninject.Activation;
    using Ninject.Infrastructure;
    using Ninject.Modules;

    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Authentication;
    using VolleyManagement.Contracts.Authentication.Models;
    using VolleyManagement.Contracts.Authorization;
    using VolleyManagement.Crosscutting.Ninject;
    using VolleyManagement.Services;
    using VolleyManagement.Services.Authentication;
    using VolleyManagement.Services.Authorization;

    /// <summary>
    /// Defines bindings for Service layer
    /// </summary>
    public class NinjectServiceBindModule : NinjectModule
    {
        private readonly Func<IContext, object> _scopeCallback;

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectServiceBindModule"/> class.
        /// </summary>
        /// <param name="scopeCallback"> The scope callback. </param>
        public NinjectServiceBindModule(Func<IContext, object> scopeCallback)
        {
            this._scopeCallback = scopeCallback;
        }

        /// <summary>
        /// Loads bindings
        /// </summary>
        public override void Load()
        {
            var configs = new IHaveBindingConfiguration[]
                              {
                                  Bind<ITournamentService>().To<TournamentService>(),
                                  Bind<IPlayerService>().To<PlayerService>(),
                                  Bind<IContributorTeamService>().To<ContributorTeamService>(),
                                  Bind<ITeamService>().To<TeamService>(),
                                  Bind<IVolleyUserManager<UserModel>>().To<VolleyUserManager>(),
                                  Bind<IVolleyUserStore>().To<VolleyUserStore>(),
                                  Bind<IRolesService>().To<RolesService>(),
                                  Bind<IGameResultService>().To<GameResultService>()
                              };
            configs.InScope(_scopeCallback);
        }
    }
}
