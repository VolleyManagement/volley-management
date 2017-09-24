namespace VolleyManagement.UI.Infrastructure
{
    using Contracts;
    using Contracts.Authorization;
    using Contracts.ExternalResources;
    using Crosscutting.Contracts.FeatureToggles;
    using Crosscutting.Contracts.Infrastructure;
    using Crosscutting.Contracts.Infrastructure.IOC;
    using FeatureToggle.Core.Fluent;
    using Services;
    using Services.Mail;

    public class IocUiModule : IIocRegistrationModule
    {
        public void RegisterDependencies(IIocContainer container)
        {
            container
                .Register<ICurrentUserService, CurrentUserService>(IocLifetimeEnum.Scoped)
                .Register<ICaptchaManager, CaptchaManager>(IocLifetimeEnum.Scoped)
                .Register<IFileService, FileService>(IocLifetimeEnum.Scoped)
                .Register<ILog, TraceLogger>(IocLifetimeEnum.Singleton);

            if (Is<IISDeployment>.Enabled)
            {
                container.Register<IMailService, GmailAccountMailService>(IocLifetimeEnum.Scoped);
            }
            else
            {
                container.Register<IMailService, SendGridMailService>(IocLifetimeEnum.Scoped);
            }
        }
    }
}