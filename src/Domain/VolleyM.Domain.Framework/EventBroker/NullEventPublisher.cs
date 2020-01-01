using System.Threading.Tasks;
using VolleyM.Domain.Framework.EventBus;

namespace VolleyM.Domain.Framework.EventBroker
{
    public class NullEventPublisher : IEventPublisher
    {
        public Task PublishEvent<TEvent>(TEvent @event) where TEvent : class
        {
            return Task.CompletedTask;
        }
    }
}