﻿namespace VolleyManagement.Data.MsSql.Infrastructure
{
    using System;
    using Ninject.Activation;
    using Ninject.Extensions.Conventions;
    using Ninject.Infrastructure;
    using Ninject.Modules;
    using VolleyManagement.Crosscutting.Ninject;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.MsSql.Queries;
    using VolleyManagement.Data.MsSql.Repositories;
    using VolleyManagement.Domain.ContributorsAggregate;
    using VolleyManagement.Domain.FeedbackAggregate;
    using VolleyManagement.Domain.GamesAggregate;
    using VolleyManagement.Domain.PlayersAggregate;
    using VolleyManagement.Domain.RequestsAggregate;
    using VolleyManagement.Domain.RolesAggregate;
    using VolleyManagement.Domain.TeamsAggregate;
    using VolleyManagement.Domain.TournamentRequestAggregate;
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

            var configs = new IHaveBindingConfiguration[]
                              {
                                  Bind<IUnitOfWork>().To<VolleyUnitOfWork>(),
                                  Bind<ITournamentRepository>().To<TournamentRepository>(),
                                  Bind<IUserRepository>().To<UserRepository>(),
                                  Bind<IPlayerRepository>().To<PlayerRepository>(),
                                  Bind<IContributorTeamRepository>().To<ContributorTeamRepository>(),
                                  Bind<ITeamRepository>().To<TeamRepository>(),
                                  Bind<IRoleRepository>().To<RoleRepostitory>(),
                                  Bind<IGameRepository>().To<GameRepository>(),
                                  Bind<IFeedbackRepository>().To<FeedbackRepository>(),
                                  Bind<ITournamentRequestRepository>().To<TournamentRequestRepository>(),
                                  Bind<IRequestRepository>().To<RequestRepository>()
                              };

            configs.InScope(_scopeCallback);
        }
    }
}
