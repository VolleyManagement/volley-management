using LanguageExt.ClassInstances;
using VolleyM.Domain.Contracts.EventBroker;

namespace VolleyM.Infrastructure.EventBroker.UnitTests.Fixture.ContextA
{
    public class EventJ : EventBase
    {
        public string RequestScope { get; set; }

        public string EventHandlerScope { get; set; }
    }
}