using System;

namespace VolleyM.Infrastructure.EventBroker
{
    public interface IEventHandlerWrapperCache
    {
        EventHandlerWrapper GetOrAdd(Type eventType, Func<EventHandlerWrapper> factory);
    }
}