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
    public class IOCDataAccessModule : IIOCRegistrationModule
    {
        public void RegisterDependencies(IOCContainer container)
        {
            RegisterRepositories(container);
            RegisterQueries(container);
        }

        private void RegisterRepositories(IOCContainer container)
        {
            container
                .RegisterScoped<IUnitOfWork, VolleyUnitOfWork>()
                .RegisterScoped<ITournamentRepository, TournamentRepository>()
                .RegisterScoped<IUserRepository, UserRepository>()
                .RegisterScoped<IPlayerRepository, PlayerRepository>()
                .RegisterScoped<IContributorTeamRepository, ContributorTeamRepository>()
                .RegisterScoped<ITeamRepository, TeamRepository>()
                .RegisterScoped<IRoleRepository, RoleRepostitory>()
                .RegisterScoped<IGameRepository, GameRepository>()
                .RegisterScoped<IFeedbackRepository, FeedbackRepository>()
                .RegisterScoped<ITournamentRequestRepository, TournamentRequestRepository>()
                .RegisterScoped<IRequestRepository, RequestRepository>();
        }

        private void RegisterQueries(IOCContainer container)
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
                    container.RegisterScoped(contract, item.Implementation);
                }
            }

            //container.Register(typeof(IQuery<,>), new Assembly[] { queriesAssembly });
        }

        private bool InterfaceIsQuery(Type type)
        {
            var typeDefinition = type.GetGenericTypeDefinition();
            return typeDefinition == typeof(IQuery<,>) || typeDefinition == typeof(IQueryAsync<,>);
        }
    }
}
