namespace VolleyManagement.Services.Infrastructure
{
    using System;
    using System.Collections.Generic;

    using Ninject.Activation;
    using Ninject.Modules;
    using Ninject.Planning.Bindings;

    using VolleyManagement.Contracts;
    using VolleyManagement.Services;

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
            var configs = new List<IBindingConfiguration>
                              {
                                  Bind<ITournamentService>().To<TournamentService>().BindingConfiguration,
                                  Bind<IUserService>().To<UserService>().BindingConfiguration,
                                  Bind<IPlayerService>().To<PlayerService>().BindingConfiguration,
                                  Bind<IContributorService>().To<ContributorService>().BindingConfiguration,
                                  Bind<IContributorTeamService>().To<ContributorTeamService>().BindingConfiguration
                              };
            if (_scopeCallback != null)
            {
                configs.ForEach(bc => bc.ScopeCallback = _scopeCallback);
            }
        }
    }
}
