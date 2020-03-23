using System.Threading.Tasks;
using VolleyM.Domain.Framework.EventBus;

namespace VolleyM.Infrastructure.EventBroker.UnitTests.Fixture.ContextA
{
    public class SampleApplicationService : IEventHandler<EventA>
    {
        private readonly EventInvocationSpy _eventSpy;

        public SampleApplicationService(EventInvocationSpy eventSpy)
        {
            _eventSpy = eventSpy;
        }

        public Task Handle(EventA @event)
        {
            _eventSpy.RegisterInvocation(@event);
            return Task.CompletedTask;
        }
    }
}