namespace VolleyManagement.Data.MsSql.Infrastructure
{
    using System;
    using Contracts;
    using Crosscutting.Ninject;
    using Domain.ContributorsAggregate;
    using Domain.FeedbackAggregate;
    using Domain.GamesAggregate;
    using Domain.PlayersAggregate;
    using Domain.RequestsAggregate;
    using Domain.RolesAggregate;
    using Domain.TeamsAggregate;
    using Domain.TournamentRequestAggregate;
    using Domain.TournamentsAggregate;
    using Domain.UsersAggregate;
    using Ninject.Activation;
    using Ninject.Extensions.Conventions;
    using Ninject.Infrastructure;
    using Ninject.Modules;
    using Queries;
    using Repositories;

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
            _scopeCallback = scopeCallback;
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
