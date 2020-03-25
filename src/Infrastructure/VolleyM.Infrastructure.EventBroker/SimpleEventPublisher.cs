using SimpleInjector;
using System.Threading.Tasks;
using VolleyM.Domain.Contracts.Crosscutting;
using VolleyM.Domain.Framework.EventBroker;

namespace VolleyM.Infrastructure.EventBroker
{
    public class SimpleEventPublisher : IEventPublisher
    {
        private readonly Container _container;

        public SimpleEventPublisher(Container container)
        {
            _container = container;
        }

        public Task PublishEvent<TEvent>(TEvent @event) where TEvent : class
        {
            var eventType = @event.GetType();
            var handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);

            var handler = _container.GetInstance(handlerType);

            var wrapper = new EventHandlerWrapper(handler);

            return wrapper.Handle(@event);
        }
    }
}
