using System;
using System.Reflection;
using System.Threading.Tasks;
using VolleyM.Domain.Contracts.EventBroker;

namespace VolleyM.Infrastructure.EventBroker
{
    public class EventHandlerWrapper
    {
        private readonly MethodInfo _handleMethod;

        public EventHandlerWrapper(Type eventType)
        {
            HandlerType = typeof(IEventHandler<>).MakeGenericType(eventType);
            _handleMethod = HandlerType.GetMethod(nameof(IEventHandler<IEvent>.Handle));
        }

        public Type HandlerType { get; }

        public Task Handle(object handler, object @event)
        {
            return (Task)_handleMethod.Invoke(handler, new[] { @event });
        }
    }
}