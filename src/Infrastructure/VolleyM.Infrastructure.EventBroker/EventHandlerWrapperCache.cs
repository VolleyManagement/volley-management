using System;
using System.Collections.Concurrent;

namespace VolleyM.Infrastructure.EventBroker
{
    internal class EventHandlerWrapperCache : IEventHandlerWrapperCache
    {
        private readonly ConcurrentDictionary<Type, EventHandlerWrapper> _cache
            = new ConcurrentDictionary<Type, EventHandlerWrapper>();

        public EventHandlerWrapper GetOrAdd(Type eventType, Func<EventHandlerWrapper> factory)
        {
            return _cache.GetOrAdd(eventType, t => factory());
        }
    }
}