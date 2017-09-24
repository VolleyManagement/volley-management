namespace VolleyManagement.UI.Infrastructure
{
    using Contracts;
    using Contracts.Authorization;
    using Contracts.ExternalResources;
    using Crosscutting.Contracts.Infrastructure.IOC;
    using FeatureToggles;
    using Services;
    using Services.Mail;

    public class IocUiModule : IIocRegistrationModule
    {
        public void RegisterDependencies(IIocContainer container)
        {
            container
                .Register<ICurrentUserService, CurrentUserService>(IocLifetimeEnum.Scoped)
                .Register<ICaptchaManager, CaptchaManager>(IocLifetimeEnum.Scoped)
                .Register<IFileService, FileService>(IocLifetimeEnum.Scoped);

            if (new IISDeployment().FeatureEnabled)
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