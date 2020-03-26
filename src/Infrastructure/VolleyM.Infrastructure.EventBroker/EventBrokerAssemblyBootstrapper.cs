using AutoMapper.Configuration;
using SimpleInjector;
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

            container.Register<IEventHandlerWrapperCache, EventHandlerWrapperCache>(Lifestyle.Singleton);
        }

        public void RegisterMappingProfiles(MapperConfigurationExpression mce)
        {
            // do nothing
        }
    }
}