namespace VolleyManagement.Data.MsSql.Infrastructure
{
    using System;
    using System.Linq;
    using VolleyManagement.Crosscutting.Contracts.Infrastructure.IOC;
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

    public class IocDataAccessModule : IIocRegistrationModule
    {
        public void RegisterDependencies(IIocContainer container)
        {
            RegisterRepositories(container);
            RegisterQueries(container);
        }

        private void RegisterRepositories(IIocContainer container)
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

        private bool InterfaceIsQuery(Type type)
        {
            var typeDefinition = type.GetGenericTypeDefinition();
            return typeDefinition == typeof(IQuery<,>) || typeDefinition == typeof(IQueryAsync<,>);
        }
    }
}
