using VolleyManagement.Crosscutting.Contracts.Infrastructure.IOC;
using VolleyManagement.Data.Contracts;
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
    static class RepositoriesRegistrator
    {
        public static void Register(IIocContainer container)
        {
            container
                .Register<IUnitOfWork, VolleyUnitOfWork>(IocLifetimeEnum.Scoped)
                .Register<ITournamentRepository, TournamentRepository>(IocLifetimeEnum.Scoped)
                .Register<IGameRepository, GameRepository>(IocLifetimeEnum.Scoped)
                .Register<IFeedbackRepository, FeedbackRepository>(IocLifetimeEnum.Scoped)
                .Register<ITournamentRequestRepository, TournamentRequestRepository>(IocLifetimeEnum.Scoped)
                .Register<IRequestRepository, RequestRepository>(IocLifetimeEnum.Scoped)
                .Register<IUserRepository, UserRepository>(IocLifetimeEnum.Scoped)
                .Register<IPlayerRepository, PlayerRepository>(IocLifetimeEnum.Scoped)
                .Register<IContributorTeamRepository, ContributorTeamRepository>(IocLifetimeEnum.Scoped)
                .Register<ITeamRepository, TeamRepository>(IocLifetimeEnum.Scoped)
                .Register<IRoleRepository, RoleRepostitory>(IocLifetimeEnum.Scoped);
        }
    }
}
