using SimpleInjector;
using SimpleInjector.Lifestyles;
using VolleyManagement.Contracts.Authorization;
using VolleyManagement.Contracts.ExternalResources;
using VolleyManagement.Crosscutting.Contracts.Infrastructure.IOC;
using VolleyManagement.Crosscutting.Contracts.Providers;
using VolleyManagement.Crosscutting.IOC;
using VolleyManagement.Data.MsSql.Infrastructure;
using VolleyManagement.Services.Infrastructure;
using VolleyManagement.Services.Mail;

namespace VolleyManagement.Specs.Infrastructure.IOC
{
    public class IocProvider
    {
        public static IocProvider Instance { get; } = new IocProvider();

        public IocProvider()
        {
            var ioc = new SimpleInjectorContainer();

            ioc.InternalContainer.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            ioc
                .Register(new IocDataAccessModule())
                .Register(new IocServicesModule());

            ioc.Register<IConfigurationProvider, IntegrationTestConfigurationProvider>(IocLifetimeEnum.Singleton);
            ioc.Register<ICurrentUserService, MockCurrentUserService>(IocLifetimeEnum.Scoped);
            ioc.Register<IMailService, DebugMailService>(IocLifetimeEnum.Singleton);

            _siContainer = ioc;
        }

        private readonly SimpleInjectorContainer _siContainer;

        public IIocContainer Container => _siContainer;

        public static T Get<T>() where T : class
        {
            return Instance.Container.Get<T>();
        }

        public Scope BeginScope()
        {
            return AsyncScopedLifestyle.BeginScope(_siContainer.InternalContainer);
        }

        public void Verify()
        {
            _siContainer.InternalContainer.Verify();
        }
    }
}