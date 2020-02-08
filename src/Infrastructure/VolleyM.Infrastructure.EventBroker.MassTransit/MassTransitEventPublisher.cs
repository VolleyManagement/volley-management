using System.Threading.Tasks;
using MassTransit;
using VolleyM.Domain.Framework.EventBus;

namespace VolleyM.Infrastructure.EventBroker.MassTransit
{
    public class MassTransitEventPublisher : IEventPublisher
    {
        private IBusControl _bus;
        private BusHandle _busHandle;

        private readonly object _busInitLock = new object();

        public MassTransitEventPublisher()
        {
            _bus = Bus.Factory.CreateUsingInMemory(cfg =>
            {

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
