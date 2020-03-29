using System;
using System.Collections.Generic;
using SimpleInjector;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Newtonsoft.Json;
using VolleyM.Domain.Contracts.EventBroker;
using VolleyM.Domain.Framework.EventBroker;

namespace VolleyM.Infrastructure.EventBroker
{
    public class SimpleEventPublisher : IEventPublisher
    {
        private readonly Container _container;
        private readonly IEventHandlerWrapperCache _eventHandlerWrapperCache;
        private readonly List<IEventHandler> _allHandlers;

        public SimpleEventPublisher(Container container, IEventHandlerWrapperCache eventHandlerWrapperCache, List<IEventHandler> allHandlers)
        {
            _container = container;
            _eventHandlerWrapperCache = eventHandlerWrapperCache;
            _allHandlers = allHandlers;
        }

        public Task PublishEvent<TEvent>(TEvent @event) where TEvent : IEvent
        {
            if (@event is IPublicEvent)
            {
                var allEventHandlers = _allHandlers
                    .Select(h => (Handler: h, EventType: GetEventType(h.GetType())))
                    .Where(p => IsForeignContextEventHandler(@event.GetType(), p.EventType))
                    .ToList();

                if (allEventHandlers.Count > 0)
                {
                    var serializedEvent = SerializeEvent(@event);
                    var eventType1 = allEventHandlers[0].EventType;

                    var targetEvent = (IEvent)JsonConvert.DeserializeObject(serializedEvent, eventType1);

                    var handlerType1 = typeof(IEventHandler<>).MakeGenericType(eventType1);

                    var handlers1 = allEventHandlers.Select(p => p.Handler);

                    var wrapper1 = _eventHandlerWrapperCache.GetOrAdd(eventType1, () => new EventHandlerWrapper(handlerType1));

                    return Task.WhenAll(handlers1
                        .Select(handler => CallHandler(targetEvent, wrapper1, handler)));
                }
            }

            var eventType = @event.GetType();
            var handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);

            var handlers = _container.GetAllInstances(handlerType);

            var wrapper = _eventHandlerWrapperCache.GetOrAdd(eventType, () => new EventHandlerWrapper(handlerType));

            return Task.WhenAll(handlers
                .Select(handler => CallHandler(@event, wrapper, handler)));
        }

        private Type GetEventType(Type handlerType)
        {
            return handlerType.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>)).GetGenericArguments()[0];
        }

        private static Task CallHandler<TEvent>(TEvent @event, EventHandlerWrapper wrapper, object handler) where TEvent : IEvent
        {
            return wrapper.Handle(handler, @event);
        }

        private static bool IsForeignContextEventHandler(Type producedType, Type eventType)
        {
            return producedType.Name == eventType.Name;
        }

        private string SerializeEvent<TEvent>(TEvent @event) where TEvent : IEvent
        {
            return JsonConvert.SerializeObject(@event);
        }
    }
}
