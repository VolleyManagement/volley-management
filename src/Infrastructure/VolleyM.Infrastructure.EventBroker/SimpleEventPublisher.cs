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
        private readonly List<Type> _handlerEventTypes;

        public SimpleEventPublisher(Container container, IEventHandlerWrapperCache eventHandlerWrapperCache, IEnumerable<IEventHandler> allHandlers)
        {
            _container = container;
            _eventHandlerWrapperCache = eventHandlerWrapperCache;
            _allHandlers = allHandlers;
            _handlerEventTypes = new List<Type>();
        }

        /// <summary>
        /// Initializes internal caches
        /// </summary>
        public void Initialize()
        {
            _handlerEventTypes.AddRange(_allHandlers.SelectMany(h => GetEventTypes(h.GetType())).Distinct());
        }

        public Task PublishEvent<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var eventType = @event.GetType();

            var matchingEventTypes = FindMatchingTypes(@event);

            if (matchingEventTypes.Count == 0) return Task.CompletedTask;

            string serializedEvent = null;

            foreach (Type handlerEventType in matchingEventTypes)
            {
                var handlerEvent = GetHandlerEventInstance(@event, handlerEventType, eventType, ref serializedEvent);
                var wrapper = _eventHandlerWrapperCache.GetOrAdd(handlerEventType, () => new EventHandlerWrapper(handlerEventType));

                var handlers = _container.GetAllInstances(wrapper.HandlerType);

                foreach (var handler in handlers)
                {
                    CallHandler(handlerEvent, wrapper, handler);
                }
            }

            return Task.CompletedTask;
        }

        private static void CallHandler<TEvent>(TEvent @event, EventHandlerWrapper wrapper, object handler) where TEvent : IEvent
        {
            wrapper.Handle(handler, @event);
        }

        private List<Type> FindMatchingTypes<TEvent>(TEvent @event) where TEvent : IEvent
        {
            Func<Type, Type, bool> handlerMatcher =
                @event is IPublicEvent ? (Func<Type, Type, bool>)IsMatchingType : IsMatchingInternalType;

            List<Type> matchingTypes = _handlerEventTypes
                .Where(t => handlerMatcher(@event.GetType(), t))
                .ToList();
            return matchingTypes;
        }

        private static IEvent GetHandlerEventInstance<TEvent>(TEvent @event, Type handlerEventType, Type eventType, ref string serializedEvent) where TEvent : IEvent
        {
            IEvent handlerEvent;
            if (handlerEventType != eventType)
            {
                // Cache result of serialization
                serializedEvent ??= SerializeEvent(@event);

                handlerEvent = DeserializeEvent(serializedEvent, handlerEventType);
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
