using System.Composition;
using AutoMapper.Configuration;
using SimpleInjector;
using VolleyM.Domain.Framework.EventBus;
using VolleyM.Infrastructure.Bootstrap;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace VolleyM.Infrastructure.EventBroker.MassTransit
{
    [Export(typeof(IAssemblyBootstrapper))]
    public class EventBrokerMassTransitAssemblyBootstrapper : IAssemblyBootstrapper
    {
        public void RegisterDependencies(Container container, IConfiguration config)
        {
            var massTransitPublisher = new MassTransitEventPublisher();

            container.RegisterInstance(massTransitPublisher);
            container.RegisterInitializer();

        }

        public void RegisterMappingProfiles(MapperConfigurationExpression mce)
        {
            // do nothing
        }
    }
}