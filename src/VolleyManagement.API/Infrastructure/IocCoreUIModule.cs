using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VolleyManagement.API.Infrastructure;
using VolleyManagement.Contracts;
using VolleyManagement.Contracts.Authorization;
using VolleyManagement.Contracts.ExternalResources;
using VolleyManagement.Crosscutting.Contracts.Infrastructure;
using VolleyManagement.Crosscutting.Contracts.Infrastructure.IOC;
using VolleyManagement.Crosscutting.Contracts.Providers;
using VolleyManagement.Services;
using VolleyManagement.Services.Mail;

namespace VolleyManagement.UI.Infrastructure
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

            //if (Is<IisDeployment>.Disabled)
            //{
            container.Register<IMailService, SendGridMailService>(IocLifetimeEnum.Scoped);
            //}
            //else
            //{
            //    container.Register<IMailService, DebugMailService>(IocLifetimeEnum.Scoped);
            //}
        }
    }
}
