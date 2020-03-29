using Newtonsoft.Json;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VolleyM.Domain.Contracts.EventBroker;
using VolleyM.Domain.Framework.EventBroker;

namespace VolleyM.Infrastructure.EventBroker
{
    public class SimpleEventPublisher : IEventPublisher
    {
        private readonly Container _container;
        private readonly IEventHandlerWrapperCache _eventHandlerWrapperCache;
        private readonly IEnumerable<IEventHandler> _allHandlers;
        private readonly List<(IEventHandler Handler, Type EventType)> _mappedHandlers;

        public SimpleEventPublisher(Container container, IEventHandlerWrapperCache eventHandlerWrapperCache, IEnumerable<IEventHandler> allHandlers)
        {
            _container = container;
            _eventHandlerWrapperCache = eventHandlerWrapperCache;
            _allHandlers = allHandlers;
            _mappedHandlers = new List<(IEventHandler Handler, Type EventType)>();
        }

        /// <summary>
        /// Initializes internal caches
        /// </summary>
        public void Initialize()
        {
            _mappedHandlers.AddRange(_allHandlers
                .SelectMany(h =>
                        GetEventTypes(h.GetType())
                        .Select(t => (Handler: h, EventType: t)))
                .ToList());
        }

        public Task PublishEvent<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var handlerTasks = new List<Task>();
            var eventType = @event.GetType();

            List<(IEventHandler Handler, Type EventType)> matchedHandlers
                = FindMatchingHandlers(@event);

            if (matchedHandlers.Count == 0) return Task.CompletedTask;

            string serializedEvent = null;
            var targetEventCache = new Dictionary<Type, IEvent>();

            foreach ((IEventHandler handler, Type handlerEventType) in matchedHandlers)
            {
                var handlerEvent = GetHandlerEventInstance(@event, handlerEventType, eventType, targetEventCache, ref serializedEvent);
                var wrapper = _eventHandlerWrapperCache.GetOrAdd(handlerEventType, () => new EventHandlerWrapper(handlerEventType));

                handlerTasks.Add(CallHandler(handlerEvent, wrapper, handler));
            }

            return Task.WhenAll(handlerTasks);
        }

        private static Task CallHandler<TEvent>(TEvent @event, EventHandlerWrapper wrapper, object handler) where TEvent : IEvent
        {
            return wrapper.Handle(handler, @event);
        }

        private List<(IEventHandler Handler, Type EventType)> FindMatchingHandlers<TEvent>(TEvent @event) where TEvent : IEvent
        {
            Func<Type, Type, bool> handlerMatcher =
                @event is IPublicEvent ? (Func<Type, Type, bool>)IsMatchingType : IsMatchingInternalType;

            List<(IEventHandler Handler, Type EventType)> matchedHandlers = _mappedHandlers
                .Where(p => handlerMatcher(@event.GetType(), p.EventType))
                .ToList();
            return matchedHandlers;
        }

        private static IEvent GetHandlerEventInstance<TEvent>(TEvent @event, Type handlerEventType, Type eventType,
            IDictionary<Type, IEvent> targetEventCache, ref string serializedEvent) where TEvent : IEvent
        {
            IEvent handlerEvent;
            if (handlerEventType != @eventType)
            {
                // Cache result of serialization
                serializedEvent ??= SerializeEvent(@event);

                // Cache deserialized objects
                if (!targetEventCache.TryGetValue(handlerEventType, out handlerEvent))
                {
                    targetEventCache[handlerEventType] =
                        handlerEvent = DeserializeEvent(serializedEvent, handlerEventType);
                }
            }
            else
            {
                handlerEvent = @event;
            }

            return handlerEvent;
        }

        private static IEnumerable<Type> GetEventTypes(Type handlerType)
        {
            return handlerType.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>))
                .Select(i => i.GetGenericArguments()[0]);
        }

        private static bool IsMatchingType(Type producedType, Type eventType)
        {
            return producedType.Name == eventType.Name;
        }

        private static bool IsMatchingInternalType(Type producedType, Type eventType)
        {
            return producedType.FullName == eventType.FullName;
        }

        private static string SerializeEvent<TEvent>(TEvent @event) where TEvent : IEvent
        {
            return JsonConvert.SerializeObject(@event);
        }

        private static IEvent DeserializeEvent(string serializedEvent, Type handlerEventType)
        {
            return (IEvent)JsonConvert.DeserializeObject(serializedEvent, handlerEventType);
        }
    }
}
