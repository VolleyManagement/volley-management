namespace VolleyManagement.UI.Infrastructure
{
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Authorization;
    using VolleyManagement.Crosscutting.IOC;
    using VolleyManagement.Services;

    public class IocUIModule : IIocRegistrationModule
    {
        public void RegisterDependencies(IocContainer container)
        {
            container
                .RegisterScoped<ICurrentUserService, CurrentUserService>()
                .RegisterScoped<ICaptchaManager, CaptchaManager>()
                .RegisterScoped<IFileService, FileService>();
        }
    }
}