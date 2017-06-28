using VolleyManagement.Contracts;
using VolleyManagement.Contracts.Authentication;
using VolleyManagement.Contracts.Authentication.Models;
using VolleyManagement.Contracts.Authorization;
using VolleyManagement.Crosscutting.Contracts.MailService;
using VolleyManagement.Crosscutting.IOC;
using VolleyManagement.Services.Authentication;
using VolleyManagement.Services.Authorization;

namespace VolleyManagement.Services.Infrastructure
{
    public class IOCServicesModule : IIocRegistrationModule
    {
        public void RegisterDependencies(IocContainer container)
        {
            container
                .Register<ITournamentService, TournamentService>(Lifetimes.Scoped)
                .Register<IPlayerService, PlayerService>(Lifetimes.Scoped)
                .Register<IContributorTeamService, ContributorTeamService>(Lifetimes.Scoped)
                .Register<ITeamService, TeamService>(Lifetimes.Scoped)
                .Register<IVolleyUserManager<UserModel>, VolleyUserManager>(Lifetimes.Scoped)
                .Register<IVolleyUserStore, VolleyUserStore>(Lifetimes.Scoped)
                .Register<IRolesService, RolesService>(Lifetimes.Scoped)
                .Register<IGameService, GameService>(Lifetimes.Scoped)
                .Register<IGameReportService, GameReportService>(Lifetimes.Scoped)
                .Register<IAuthorizationService, AuthorizationService>(Lifetimes.Scoped)
                .Register<IUserService, UserService>(Lifetimes.Scoped)
                .Register<IFeedbackService, FeedbackService>(Lifetimes.Scoped)
                .Register<ITournamentRequestService, TournamentRequestService>(Lifetimes.Scoped)
                .Register<IRequestService, RequestService>(Lifetimes.Scoped)
                .Register<ICacheProvider, CacheProvider>(Lifetimes.Scoped)
                .Register<IMailService, GmailAccountMailService>(Lifetimes.Scoped);
        }
    }
}
