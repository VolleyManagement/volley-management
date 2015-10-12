namespace VolleyManagement.Data.MsSql.Infrastructure
{
    using System;
    using System.Collections.Generic;

    using Ninject.Activation;
    using Ninject.Modules;
    using Ninject.Planning.Bindings;
    using Ninject.Extensions.Conventions;

    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.MsSql.Queries;
    using VolleyManagement.Data.MsSql.Repositories;
    using VolleyManagement.Domain.ContributorsAggregate;
    using VolleyManagement.Domain.PlayersAggregate;
    using VolleyManagement.Domain.TeamsAggregate;
    using VolleyManagement.Domain.TournamentsAggregate;
    using VolleyManagement.Domain.UsersAggregate;

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
            // Bind all queries
            Kernel.Bind(x =>
                    x.FromThisAssembly()
                     .SelectAllClasses()
                     .InNamespaceOf<TournamentQueries>()
                     .BindAllInterfaces()
                     .Configure(c => c.InTransientScope()));

            var configs = new List<IBindingConfiguration>
                              {
                                  Bind<IUnitOfWork>().To<VolleyUnitOfWork>().BindingConfiguration,
                                  Bind<ITournamentRepository>().To<TournamentRepository>().BindingConfiguration,
                                  Bind<IUserRepository>().To<UserRepository>().BindingConfiguration,
                                  Bind<IPlayerRepository>().To<PlayerRepository>().BindingConfiguration,
                                  Bind<IContributorTeamRepository>().To<ContributorTeamRepository>().BindingConfiguration,
                                  Bind<ITeamRepository>().To<TeamRepository>().BindingConfiguration
                              };
            if (this._scopeCallback != null)
            {
                configs.ForEach(bc => bc.ScopeCallback = this._scopeCallback);
            }
        }
    }
}
