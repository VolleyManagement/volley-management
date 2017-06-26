namespace VolleyManagement.Services.Infrastructure
{
    using System;
    using Authentication;
    using Authorization;
    using Contracts;
    using Contracts.Authentication;
    using Contracts.Authentication.Models;
    using Contracts.Authorization;
    using Crosscutting.Contracts.MailService;
    using Crosscutting.Ninject;
    using Ninject.Activation;
    using Ninject.Infrastructure;
    using Ninject.Modules;
    using Services;

    /// <summary>
    /// Defines bindings for Service layer
    /// </summary>
    public class NinjectServiceBindModule : NinjectModule
    {
        private readonly Func<IContext, object> _scopeCallback;

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectServiceBindModule"/> class.
        /// </summary>
        /// <param name="scopeCallback"> The scope callback. </param>
        public NinjectServiceBindModule(Func<IContext, object> scopeCallback)
        {
            _scopeCallback = scopeCallback;
        }

        /// <summary>
        /// Loads bindings
        /// </summary>
        public override void Load()
        {
            var configs = new IHaveBindingConfiguration[]
                              {
                                  Bind<ITournamentService>().To<TournamentService>(),
                                  Bind<IPlayerService>().To<PlayerService>(),
                                  Bind<IContributorTeamService>().To<ContributorTeamService>(),
                                  Bind<ITeamService>().To<TeamService>(),
                                  Bind<IVolleyUserManager<UserModel>>().To<VolleyUserManager>(),
                                  Bind<IVolleyUserStore>().To<VolleyUserStore>(),
                                  Bind<IRolesService>().To<RolesService>(),
                                  Bind<IGameService>().To<GameService>(),
                                  Bind<IGameReportService>().To<GameReportService>(),
                                  Bind<IAuthorizationService>().To<AuthorizationService>(),
                                  Bind<IUserService>().To<UserService>(),
                                  Bind<IFeedbackService>().To<FeedbackService>(),
                                  Bind<ITournamentRequestService>().To<TournamentRequestService>(),
                                  Bind<IRequestService>().To<RequestService>(),
                                  Bind<ICacheProvider>().To<CacheProvider>(),
                                  Bind<IMailService>().To<GmailAccountMailService>()
                              };
            configs.InScope(_scopeCallback);
        }
    }
}
