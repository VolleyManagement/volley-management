using System.Threading.Tasks;
using VolleyM.Domain.Contracts.EventBroker;

namespace VolleyM.Infrastructure.EventBroker.UnitTests.Fixture.ContextB
{
    public class SamplePublicApplicationService : IEventHandler<EventD>, IEventHandler<EventF>
    {
        private readonly EventInvocationSpy _eventSpy;

        public SamplePublicApplicationService(EventInvocationSpy eventSpy)
        {
            _eventSpy = eventSpy;
        }

        public Task Handle(EventD @event)
        {
            return RegisterEvent(@event);
        }

        public Task Handle(EventF @event)
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