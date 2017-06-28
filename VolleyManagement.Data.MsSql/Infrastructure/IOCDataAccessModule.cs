using System;
using System.Linq;
using VolleyManagement.Crosscutting.IOC;
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

namespace VolleyManagement.Data.MsSql.Infrastructure
{
    public class IOCDataAccessModule : IIocRegistrationModule
    {
        public void RegisterDependencies(IocContainer container)
        {
            RegisterRepositories(container);
            RegisterQueries(container);
        }

        private void RegisterRepositories(IocContainer container)
        {
            container
                .Register<IUnitOfWork, VolleyUnitOfWork>(Lifetimes.Scoped)
                .Register<ITournamentRepository, TournamentRepository>(Lifetimes.Scoped)
                .Register<IUserRepository, UserRepository>(Lifetimes.Scoped)
                .Register<IPlayerRepository, PlayerRepository>(Lifetimes.Scoped)
                .Register<IContributorTeamRepository, ContributorTeamRepository>(Lifetimes.Scoped)
                .Register<ITeamRepository, TeamRepository>(Lifetimes.Scoped)
                .Register<IRoleRepository, RoleRepostitory>(Lifetimes.Scoped)
                .Register<IGameRepository, GameRepository>(Lifetimes.Scoped)
                .Register<IFeedbackRepository, FeedbackRepository>(Lifetimes.Scoped)
                .Register<ITournamentRequestRepository, TournamentRequestRepository>(Lifetimes.Scoped)
                .Register<IRequestRepository, RequestRepository>(Lifetimes.Scoped);
        }

        private void RegisterQueries(IocContainer container)
        {
            var queriesAssembly = typeof(TournamentQueries).Assembly;

            var registrations =
                from type in queriesAssembly.GetExportedTypes()
                where type.Namespace == "VolleyManagement.Data.MsSql.Queries"
                where type.GetInterfaces().Any(i => InterfaceIsQuery(i))
                select new
                {
                    Contracts = type.GetInterfaces().Where(InterfaceIsQuery),
                    Implementation = type
                };

            foreach (var item in registrations)
            {
                foreach (var contract in item.Contracts)
                {
                    container.Register(contract, item.Implementation, Lifetimes.Scoped);
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
