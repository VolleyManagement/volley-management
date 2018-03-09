using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyManagement.Contracts;
using VolleyManagement.Contracts.Authentication;
using VolleyManagement.Contracts.Authentication.Models;
using VolleyManagement.Contracts.Authorization;
using VolleyManagement.Crosscutting.Contracts.Infrastructure.IOC;
using VolleyManagement.Services.Authentication;
using VolleyManagement.Services.Authorization;

namespace VolleyManagement.Services.Infrastructure
{
    public class IocCoreServicesModule
    {
        public void RegisterDependencies(IServiceCollection services)
        {
            services
                .AddScoped<ITournamentService, TournamentService>()
                .AddScoped<IPlayerService, PlayerService>()
                .AddScoped<IContributorTeamService, ContributorTeamService>()
                .AddScoped<ITeamService, TeamService>()
                .AddScoped<IVolleyUserManager<UserModel>, VolleyUserManager>()
                .AddScoped<IVolleyUserStore, VolleyUserStore>()
                .AddScoped<IRolesService, RolesService>()
                .AddScoped<IGameService, GameService>()
                .AddScoped<IGameReportService, GameReportService>()
                .AddScoped<IAuthorizationService, AuthorizationService>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<IFeedbackService, FeedbackService>()
                .AddScoped<ITournamentRequestService, TournamentRequestService>()
                .AddScoped<IRequestService, RequestService>()
                .AddScoped<ICacheProvider, CacheProvider>();
        }
    }
}
