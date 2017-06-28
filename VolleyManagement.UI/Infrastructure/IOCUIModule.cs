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
                .Register<ICurrentUserService, CurrentUserService>(Lifetimes.Scoped)
                .Register<ICaptchaManager, CaptchaManager>(Lifetimes.Scoped)
                .Register<IFileService, FileService>(Lifetimes.Scoped);
        }
    }
}