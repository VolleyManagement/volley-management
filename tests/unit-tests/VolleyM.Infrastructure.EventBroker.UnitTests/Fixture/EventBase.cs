using VolleyM.Domain.Contracts.EventBroker;

namespace VolleyM.Infrastructure.EventBroker.UnitTests.Fixture
{
    public abstract class EventBase : IEvent
    {
        public string SomeData { get; set; }
        public int RequestData { get; set; }
    }
}