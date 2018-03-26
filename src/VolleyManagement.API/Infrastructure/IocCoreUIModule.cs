using FeatureToggle;
using FeatureToggle.Core.Fluent;
using VolleyManagement.Contracts;
using VolleyManagement.Contracts.Authorization;
using VolleyManagement.Contracts.ExternalResources;
using VolleyManagement.Crosscutting.Contracts.Infrastructure;
using VolleyManagement.Crosscutting.Contracts.Infrastructure.IOC;
using VolleyManagement.Crosscutting.Contracts.Providers;
using VolleyManagement.Services;
using VolleyManagement.Services.Mail;
using VolleyManagement.UI.Infrastructure;

namespace VolleyManagement.API.Infrastructure
{
    public class IocCoreUIModule : IIocRegistrationModule
    {
        public void RegisterDependencies(IIocContainer container)
        {
            container
                .Register<ICurrentUserService, CurrentUserService>(IocLifetimeEnum.Scoped)
                .Register<ICaptchaManager, CaptchaManager>(IocLifetimeEnum.Scoped)
                .Register<IFileService, FileService>(IocLifetimeEnum.Scoped)
                .Register<ILog, SimpleTraceLog>(IocLifetimeEnum.Singleton)
                .Register<IConfigurationProvider, ApiUiConfigurationProvider>(IocLifetimeEnum.Singleton);

            if (Is<Crosscutting.Contracts.FeatureToggles.Core.IisDeployment>.Disabled)
            {
                container.Register<IMailService, SendGridMailService>(IocLifetimeEnum.Scoped);
            }
            else
            {
                container.Register<IMailService, DebugMailService>(IocLifetimeEnum.Scoped);
            }
        }
    }
}
