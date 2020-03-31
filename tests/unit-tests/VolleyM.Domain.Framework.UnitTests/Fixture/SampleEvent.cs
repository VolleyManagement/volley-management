using VolleyM.Domain.Contracts.EventBroker;

namespace VolleyM.Domain.IDomainFrameworkTestFixture
{
    public class SampleEvent : IEvent
    {
        public string Data { get; set; }
    }
}