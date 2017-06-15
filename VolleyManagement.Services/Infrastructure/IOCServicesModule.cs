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
    public class IOCServicesModule : IIOCRegistrationModule
    {
        public void RegisterDependencies(IOCContainer container)
        {
            container
                .RegisterScoped<ITournamentService, TournamentService>()
                .RegisterScoped<IPlayerService, PlayerService>()
                .RegisterScoped<IContributorTeamService, ContributorTeamService>()
                .RegisterScoped<ITeamService, TeamService>()
                .RegisterScoped<IVolleyUserManager<UserModel>, VolleyUserManager>()
                .RegisterScoped<IVolleyUserStore, VolleyUserStore>()
                .RegisterScoped<IRolesService, RolesService>()
                .RegisterScoped<IGameService, GameService>()
                .RegisterScoped<IGameReportService, GameReportService>()
                .RegisterScoped<IAuthorizationService, AuthorizationService>()
                .RegisterScoped<IUserService, UserService>()
                .RegisterScoped<IFeedbackService, FeedbackService>()
                .RegisterScoped<ITournamentRequestService, TournamentRequestService>()
                .RegisterScoped<IRequestService, RequestService>()
                .RegisterScoped<ICacheProvider, CacheProvider>()
                .RegisterScoped<IMailService, GmailAccountMailService>();
        }
    }
}
