namespace VolleyManagement.Dal.MsSql.Infrastructure
{
    using System;
    using System.Collections.Generic;

    using Ninject.Activation;
    using Ninject.Modules;
    using Ninject.Planning.Bindings;

    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.Dal.MsSql.Services;

    /// <summary>
    /// Defines bindings for Service layer
    /// </summary>
    public class NinjectDataAccessModule : NinjectModule
    {
        private readonly Func<IContext, object> _scopeCallback;

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectDataAccessModule"/> class.
        /// </summary>
        /// <param name="scopeCallback"> The scope callback. </param>
        public NinjectDataAccessModule(Func<IContext, object> scopeCallback)
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
                                  Bind<IUnitOfWork>().To<VolleyUnitOfWork>().BindingConfiguration,
                                  Bind<ITournamentRepository>().To<TournamentRepository>().BindingConfiguration,
                                  Bind<IUserRepository>().To<UserRepository>().BindingConfiguration,
                                  Bind<IPlayerRepository>().To<PlayerRepository>().BindingConfiguration,
                                  Bind<IContributorRepository>().To<ContributorRepository>().BindingConfiguration
                              };
            if (_scopeCallback != null)
            {
                configs.ForEach(bc => bc.ScopeCallback = _scopeCallback);
            }
        }
    }
}
