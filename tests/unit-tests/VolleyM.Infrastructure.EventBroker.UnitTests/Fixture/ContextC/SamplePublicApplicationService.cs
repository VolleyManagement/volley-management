using System.Threading.Tasks;
using VolleyM.Domain.Contracts.EventBroker;

namespace VolleyM.Infrastructure.EventBroker.UnitTests.Fixture.ContextC
{
    public class SamplePublicApplicationService : IEventHandler<EventG>, IEventHandler<EventI>
    {
        private readonly EventInvocationSpy _eventSpy;

        public SamplePublicApplicationService(EventInvocationSpy eventSpy)
        {
            _eventSpy = eventSpy;
        }

        public Task Handle(EventG @event)
        {
            return RegisterEvent(@event);
        }

        public Task Handle(EventI @event)
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