using VolleyManagement.Crosscutting.Contracts.Infrastructure.IOC;
using VolleyManagement.Data.Contracts;
using VolleyManagement.Data.MsSql.Repositories;
using VolleyManagement.Domain.FeedbackAggregate;
using VolleyManagement.Domain.GamesAggregate;
using VolleyManagement.Domain.RequestsAggregate;
using VolleyManagement.Domain.TournamentRequestAggregate;
using VolleyManagement.Domain.TournamentsAggregate;

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
                .Register<IRequestRepository, RequestRepository>(IocLifetimeEnum.Scoped);
        }
    }
}
