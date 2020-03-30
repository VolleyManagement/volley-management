using AutoMapper.Configuration;
using SimpleInjector;
using VolleyM.Domain.Contracts.EventBroker;
using VolleyM.Domain.Framework.EventBroker;
using VolleyM.Infrastructure.Bootstrap;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace VolleyM.Infrastructure.EventBroker
{
    public class EventBrokerAssemblyBootstrapper : IAssemblyBootstrapper
    {
        public void RegisterDependencies(Container container, IConfiguration config)
        {
            container.Register<IEventPublisher, SimpleEventPublisher>();
            container.RegisterInitializer((SimpleEventPublisher p) => p.Initialize());

            container.Register<IEventHandlerWrapperCache, EventHandlerWrapperCache>(Lifestyle.Singleton);

            container.RegisterDecorator(
                typeof(IEventHandler<>),
                typeof(AsyncScopedEventHandlerProxy<>),
                Lifestyle.Scoped);
        }

        public void RegisterMappingProfiles(MapperConfigurationExpression mce)
        {
            // do nothing
        }
    }
}