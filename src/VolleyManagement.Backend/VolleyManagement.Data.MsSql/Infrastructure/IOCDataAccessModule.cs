namespace VolleyManagement.Data.MsSql.Infrastructure
{
    using System;
    using System.Linq;
    using Crosscutting.Contracts.Infrastructure.IOC;
    using Contracts;
    using Queries;
    using Repositories;
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

#pragma warning disable S1200 // IoC registration
    public class IocDataAccessModule : IIocRegistrationModule
#pragma warning restore S1200
    {
        public void RegisterDependencies(IIocContainer container)
        {
            RegisterRepositories(container);
            RegisterQueries(container);
        }

        private static void RegisterRepositories(IIocContainer container)
        {
            container
                .Register<IUnitOfWork, VolleyUnitOfWork>(IocLifetimeEnum.Scoped)
                .Register<ITournamentRepository, TournamentRepository>(IocLifetimeEnum.Scoped)
                .Register<IUserRepository, UserRepository>(IocLifetimeEnum.Scoped)
                .Register<IPlayerRepository, PlayerRepository>(IocLifetimeEnum.Scoped)
                .Register<IContributorTeamRepository, ContributorTeamRepository>(IocLifetimeEnum.Scoped)
                .Register<ITeamRepository, TeamRepository>(IocLifetimeEnum.Scoped)
                .Register<IRoleRepository, RoleRepostitory>(IocLifetimeEnum.Scoped)
                .Register<IGameRepository, GameRepository>(IocLifetimeEnum.Scoped)
                .Register<IFeedbackRepository, FeedbackRepository>(IocLifetimeEnum.Scoped)
                .Register<ITournamentRequestRepository, TournamentRequestRepository>(IocLifetimeEnum.Scoped)
                .Register<IRequestRepository, RequestRepository>(IocLifetimeEnum.Scoped);
        }

        private void RegisterQueries(IIocContainer container)
        {
            var queriesAssembly = typeof(TournamentQueries).Assembly;

            var registrations =
                from type in queriesAssembly.GetExportedTypes()
                where type.Namespace == "VolleyManagement.Data.MsSql.Queries"
                where type.GetInterfaces().Any(InterfaceIsQuery)
                select new
                {
                    Contracts = type.GetInterfaces().Where(InterfaceIsQuery),
                    Implementation = type
                };

            foreach (var item in registrations)
            {
                foreach (var contract in item.Contracts)
                {
                    container.Register(contract, item.Implementation, IocLifetimeEnum.Scoped);
                }
            }
        }

        private static bool InterfaceIsQuery(Type type)
        {
            var typeDefinition = type.GetGenericTypeDefinition();
            return typeDefinition == typeof(IQuery<,>) || typeDefinition == typeof(IQueryAsync<,>);
        }
    }
}
