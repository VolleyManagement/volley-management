using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeatureToggle.Core.Fluent;
using VolleyManagement.Contracts;
using VolleyManagement.Contracts.Authorization;
using VolleyManagement.Contracts.ExternalResources;
using VolleyManagement.Crosscutting.Contracts.FeatureToggles;
using VolleyManagement.Crosscutting.Contracts.Infrastructure;
using VolleyManagement.Crosscutting.Contracts.Infrastructure.IOC;
using VolleyManagement.Services;
using VolleyManagement.Services.Mail;
using VolleyManagement.UI.Infrastructure;

namespace VolleyManagement.API.Infrastructure
{
    public class IocCoreApiModule
    {
        public void RegisterDependencies(IServiceCollection services)
        {
            services
                .AddScoped<ICurrentUserService, CurrentUserService>()
                .AddScoped<ICaptchaManager, CaptchaManager>()
                //.AddScoped<IFileService, FileService>()
                .AddSingleton<ILog, SimpleTraceLog>();

            //if (Is<IISDeployment>.Disabled)
            //{
            //    services.AddScoped<IMailService, SendGridMailService>();
            //}
            //else
            //{
            //    services.AddScoped<IMailService, DebugMailService>();
            //}
        }
    }
}
