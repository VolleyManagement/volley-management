using System.Threading.Tasks;
using VolleyM.Domain.Contracts.EventBroker;

namespace VolleyM.Domain.Framework.EventBroker
{
    public class NullEventPublisher : IEventPublisher
    {
        public Task PublishEvent<TEvent>(TEvent @event) where TEvent : IEvent
        {
            return Task.CompletedTask;
        }
    }
}