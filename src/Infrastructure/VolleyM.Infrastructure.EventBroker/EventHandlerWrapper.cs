using System.Reflection;
using System.Threading.Tasks;
using VolleyM.Domain.Contracts.Crosscutting;

namespace VolleyM.Infrastructure.EventBroker
{
    public class EventHandlerWrapper : IEventHandler<object>
    {
        //ToDo: Remove this field
        private readonly object _handler;
        private readonly MethodInfo _handleMethod;

        public EventHandlerWrapper(object handler)
        {
            _handler = handler;

            _handleMethod = _handler.GetType().GetMethod(nameof(IEventHandler<object>.Handle));
        }

        public Task Handle(object @event)
        {
            return (Task)_handleMethod.Invoke(_handler, new[] { @event });
        }
    }
}