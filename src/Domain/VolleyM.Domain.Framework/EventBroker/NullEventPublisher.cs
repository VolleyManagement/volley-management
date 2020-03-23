using System.Threading.Tasks;

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