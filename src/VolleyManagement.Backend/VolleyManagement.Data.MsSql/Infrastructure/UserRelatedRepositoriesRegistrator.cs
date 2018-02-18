using VolleyManagement.Crosscutting.Contracts.Infrastructure.IOC;
using VolleyManagement.Data.MsSql.Repositories;
using VolleyManagement.Domain.ContributorsAggregate;
using VolleyManagement.Domain.PlayersAggregate;
using VolleyManagement.Domain.RolesAggregate;
using VolleyManagement.Domain.TeamsAggregate;
using VolleyManagement.Domain.UsersAggregate;

namespace VolleyManagement.Data.MsSql.Infrastructure
{
    static class UserRelatedRepositoriesRegistrator
    {
        public static void Register(IIocContainer container)
        {
            container
                .Register<IUserRepository, UserRepository>(IocLifetimeEnum.Scoped)
                .Register<IPlayerRepository, PlayerRepository>(IocLifetimeEnum.Scoped)
                .Register<IContributorTeamRepository, ContributorTeamRepository>(IocLifetimeEnum.Scoped)
                .Register<ITeamRepository, TeamRepository>(IocLifetimeEnum.Scoped)
                .Register<IRoleRepository, RoleRepostitory>(IocLifetimeEnum.Scoped);
        }
    }
}
