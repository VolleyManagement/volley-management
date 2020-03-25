using SimpleInjector;
using System;
using System.Threading.Tasks;
using VolleyM.Domain.Framework.EventBroker;
using VolleyM.Domain.Framework.EventBus;

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

            var wrapper = new EventHandlerWrapper(handler, eventType);

            return wrapper.Handle(@event);
        }
    }

    public class EventHandlerWrapper : IEventHandler<object>
    {
        private readonly object _handler;
        private readonly Type _eventType;

        static EventHandlerWrapper()
        {
        }

        public EventHandlerWrapper(object handler, Type eventType)
        {
            _handler = handler;
            _eventType = eventType;
        }

        public Task Handle(object @event)
        {
            var handleMethod = _handler.GetType().GetMethod(nameof(IEventHandler<object>.Handle));

            //var method = handleMethod.MakeGenericMethod(_eventType);

            return (Task)handleMethod.Invoke(_handler, new[] { @event });
        }
    }

}
