using System.Threading.Tasks;
using VolleyM.Domain.Contracts.EventBroker;

namespace VolleyM.Infrastructure.EventBroker.UnitTests.Fixture.ContextA
{
    public class SampleApplicationService : IEventHandler<EventA>, IEventHandler<EventC>, IEventHandler<EventH>
    {
        private readonly EventInvocationSpy _eventSpy;

        public SampleApplicationService(EventInvocationSpy eventSpy)
        {
            _eventSpy = eventSpy;
        }

        public Task Handle(EventA @event)
        {
            return RegisterEvent(@event);
        }

        public Task Handle(EventC @event)
        {
            return RegisterEvent(@event);
        }

        public Task Handle(EventH @event)
        {
            return RegisterEvent(@event);
        }

        private Task RegisterEvent(IEvent @event)
        {
            _eventSpy.RegisterInvocation(@event);
            return Task.CompletedTask;
        }
    }
}