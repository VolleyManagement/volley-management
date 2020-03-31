using System.Threading.Tasks;
using VolleyM.Domain.Contracts.EventBroker;

namespace VolleyM.Infrastructure.EventBroker.UnitTests.Fixture.ContextA
{
    public class ScopeAwareApplicationService1 : IEventHandler<EventJ>
    {
        private readonly EventInvocationSpy _eventSpy;
        private readonly ScopeLitmus _scopeLitmus;

        public ScopeAwareApplicationService1(EventInvocationSpy eventSpy, ScopeLitmus scopeLitmus)
        {
            _eventSpy = eventSpy;
            _scopeLitmus = scopeLitmus;
        }

        public Task Handle(EventJ @event)
        {
            @event.EventHandlerScope = _scopeLitmus.Scope;
            return RegisterEvent(@event);
        }

        private Task RegisterEvent(IEvent @event)
        {
            _eventSpy.RegisterInvocation(@event);
            return Task.CompletedTask;
        }
    }
}