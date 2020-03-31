using System;
using System.Threading.Tasks;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using VolleyM.Domain.Contracts.EventBroker;

namespace VolleyM.Infrastructure.EventBroker
{
    public class AsyncScopedEventHandlerProxy<T> : IEventHandler<T> where T : IEvent
    {
        private readonly Container _container;
        private readonly Func<IEventHandler<T>> _decorateeFactory;

        public AsyncScopedEventHandlerProxy(Func<IEventHandler<T>> decorateeFactory, Container container)
        {
            _decorateeFactory = decorateeFactory;
            _container = container;
        }

        public Task Handle(T @event)
        {
            using (AsyncScopedLifestyle.BeginScope(_container))
            {
                var handler = _decorateeFactory.Invoke();
                return handler.Handle(@event);
            }
        }
    }
}