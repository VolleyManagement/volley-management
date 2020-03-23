using System.Collections.Generic;

namespace VolleyM.Infrastructure.EventBroker.UnitTests.Fixture
{
    public class EventInvocationSpy
    {
        public List<object> Invocations { get; } = new List<object>();

        public void RegisterInvocation(object evt) => Invocations.Add(evt);
    }
}