using System.Threading.Tasks;
using VolleyM.Domain.Contracts.Crosscutting;

namespace VolleyM.Domain.Framework.UnitTests.EventBus.Fixture
{
    public class EventAListener : IEventListener<EventA>
    {
        private readonly EventListenerTestSpy _listenerTestSpy;

        public EventAListener(EventListenerTestSpy listenerTestSpy)
        {
            _listenerTestSpy = listenerTestSpy;
        }

        public Task Handle(EventA @event)
        {
            _listenerTestSpy.RegisterInvocation<EventA>();
            return Task.CompletedTask;
        }
    }
}