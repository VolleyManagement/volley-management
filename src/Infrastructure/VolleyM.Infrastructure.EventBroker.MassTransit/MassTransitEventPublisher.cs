using System.Threading.Tasks;
using MassTransit;
using Serilog;
using SimpleInjector;
using VolleyM.Domain.Framework.EventBus;

namespace VolleyM.Infrastructure.EventBroker.MassTransit
{
    public class MassTransitEventPublisher : IEventPublisher
    {
        private IBusControl _bus;
        private BusHandle _busHandle;

        public MassTransitEventPublisher(Container container)
        {
            _bus = Bus.Factory.CreateUsingInMemory(cfg =>
            {
                cfg.UseSerilog(Log.Logger);
                cfg.ReceiveEndpoint(ep =>
                {
                    ep.ConfigureConsumers(container);
                });
            });
        }

        public async Task StartBus()
        {
            _busHandle = await _bus.StartAsync();

            await _busHandle.Ready;
        }

        public Task PublishEvent<TEvent>(TEvent @event) where TEvent : class
        {
            return _bus.Publish(@event);
        }
    }
}
