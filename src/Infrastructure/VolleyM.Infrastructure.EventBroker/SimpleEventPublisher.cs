using System.Collections.Generic;
using SimpleInjector;
using System.Threading.Tasks;
using VolleyM.Domain.Contracts.Crosscutting;
using VolleyM.Domain.Framework.EventBroker;

namespace VolleyM.Infrastructure.EventBroker
{
    public class SimpleEventPublisher : IEventPublisher
    {
        private readonly Container _container;
        private readonly IEventHandlerWrapperCache _eventHandlerWrapperCache;

        public SimpleEventPublisher(Container container, IEventHandlerWrapperCache eventHandlerWrapperCache)
        {
            _container = container;
            _eventHandlerWrapperCache = eventHandlerWrapperCache;
        }

        public Task PublishEvent<TEvent>(TEvent @event) where TEvent : class
        {
            var eventType = @event.GetType();
            var handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);

            var handlers = _container.GetAllInstances(handlerType);

            var wrapper = _eventHandlerWrapperCache.GetOrAdd(eventType, () => new EventHandlerWrapper(handlerType));

            List<Task> tasks = new List<Task>();
            foreach (var handler in handlers)
            {
                var task = wrapper.Handle(handler, @event);
                tasks.Add(task);
            }

            return Task.WhenAll(tasks);
        }
    }
}
