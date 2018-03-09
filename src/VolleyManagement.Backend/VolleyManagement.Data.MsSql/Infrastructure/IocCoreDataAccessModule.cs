using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
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
    public class IocCoreDataAccessModule
    {
        public void RegisterDependencies(IServiceCollection services)
        {
            RegisterRepositories(services);
            RegisterQueries(services);
        }

        private void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, VolleyUnitOfWork>()
                .AddScoped<ITournamentRepository, TournamentRepository>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IPlayerRepository, PlayerRepository>()
                .AddScoped<IContributorTeamRepository, ContributorTeamRepository>()
                .AddScoped<ITeamRepository, TeamRepository>()
                .AddScoped<IRoleRepository, RoleRepostitory>()
                .AddScoped<IGameRepository, GameRepository>()
                .AddScoped<IFeedbackRepository, FeedbackRepository>()
                .AddScoped<ITournamentRequestRepository, TournamentRequestRepository>()
                .AddScoped<IRequestRepository, RequestRepository>();
        }

        private void RegisterQueries(IServiceCollection services)
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
                    services.AddScoped(contract, item.Implementation);
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
