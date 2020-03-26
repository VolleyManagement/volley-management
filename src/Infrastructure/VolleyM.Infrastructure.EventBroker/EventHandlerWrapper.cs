using System;
using System.Reflection;
using System.Threading.Tasks;
using VolleyM.Domain.Contracts.Crosscutting;

namespace VolleyM.Infrastructure.EventBroker
{
    public class EventHandlerWrapper
    {
        private readonly MethodInfo _handleMethod;

        public EventHandlerWrapper(Type handlerType)
        {
            _handleMethod = handlerType.GetMethod(nameof(IEventHandler<object>.Handle));
        }

        public Task Handle(object handler, object @event)
        {
            return (Task)_handleMethod.Invoke(handler, new[] { @event });
        }
    }
}