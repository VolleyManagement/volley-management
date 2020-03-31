using AutoMapper.Configuration;
using SimpleInjector;
using System.Composition;
using VolleyM.Infrastructure.Bootstrap;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace VolleyM.Infrastructure.EventBroker.MassTransit
{
    [Export(typeof(IAssemblyBootstrapper))]
    public class EventBrokerMassTransitAssemblyBootstrapper : IAssemblyBootstrapper
    {
        public void RegisterDependencies(Container container, IConfiguration config)
        {
            container.Register<Domain.Framework.EventBus.IEventPublisher, MassTransitEventPublisher>(Lifestyle.Singleton);
            container.RegisterInitializer<MassTransitEventPublisher>(mtep => mtep.StartBus().RunSynchronously());
        }

        public void RegisterMappingProfiles(MapperConfigurationExpression mce)
        {
            // do nothing
        }
    }
}