using System;
using System.Threading.Tasks;
using VolleyM.Domain.Framework.EventBus;

namespace VolleyM.Infrastructure.EventBroker.MassTransit
{
    public class MassTransitEventPublisher : IEventPublisher
    {
        public MassTransitEventPublisher()
        {

        }

        public Task PublishEvent<TEvent>(TEvent @event) where TEvent : class
        {
            throw new NotImplementedException();
        }
    }
}
