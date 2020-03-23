using System.Threading.Tasks;
using VolleyM.Domain.Framework.EventBroker;

namespace VolleyM.Infrastructure.EventBroker
{
    public class SimpleEventPublisher : IEventPublisher
    {
        public Task PublishEvent<TEvent>(TEvent @event) where TEvent : class
        {
            throw new System.NotImplementedException();
        }
    }
}
