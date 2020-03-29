using VolleyM.Domain.Contracts.EventBroker;

namespace VolleyM.Infrastructure.EventBroker.UnitTests.Fixture.ContextA
{
    public class EventI : EventBase, IPublicEvent
    {
        public string IgnoredProperty { get; set; }
    }
}