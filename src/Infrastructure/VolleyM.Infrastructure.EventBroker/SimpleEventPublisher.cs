using LanguageExt;
using Newtonsoft.Json;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using VolleyM.Domain.Contracts.EventBroker;
using VolleyM.Domain.Framework.EventBroker;
using VolleyM.Domain.IdentityAndAccess.Handlers;

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

            List<(IEventHandler Handler, Type EventType)> matchedHandlers;

            if (@event is IPublicEvent)
            {
                matchedHandlers = _mappedHandlers
                    .Where(p => IsMatchingType(@event.GetType(), p.EventType))
                    .ToList();
            }
            else
            {
                matchedHandlers = _mappedHandlers
                    .Where(p => IsMatchingInternalType(@event.GetType(), p.EventType))
                    .ToList();
            }

            if (matchedHandlers.Count == 0) return Task.CompletedTask;

            foreach (var (handler, handlerEventType) in matchedHandlers)
            {
                IEvent handlerEvent;

                if (handlerEventType != @eventType)
                {
                    var serializedEvent = SerializeEvent(@event);

                    handlerEvent = (IEvent)JsonConvert.DeserializeObject(serializedEvent, handlerEventType);
                }
                else
                {
                    handlerEvent = @event;
                }

                var wrapper = _eventHandlerWrapperCache.GetOrAdd(handlerEventType, () => new EventHandlerWrapper(handlerEventType));

                handlerTasks.Add(CallHandler(handlerEvent, wrapper, handler));
            }

            return Task.WhenAll(handlerTasks);
        }

        private static Task CallHandler<TEvent>(TEvent @event, EventHandlerWrapper wrapper, object handler) where TEvent : IEvent
        {
            return wrapper.Handle(handler, @event);
        }

        private IEnumerable<Type> GetEventTypes(Type handlerType)
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

        private string SerializeEvent<TEvent>(TEvent @event) where TEvent : IEvent
        {
            return JsonConvert.SerializeObject(@event);
        }
    }
}
