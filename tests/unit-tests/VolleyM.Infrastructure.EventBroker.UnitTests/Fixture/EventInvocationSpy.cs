using System.Collections.Generic;
using VolleyM.Domain.Contracts.EventBroker;

namespace VolleyM.Infrastructure.EventBroker.UnitTests.Fixture
{
    public class EventInvocationSpy
    {
        public List<IEvent> Invocations { get; } = new List<IEvent>();

        public void RegisterInvocation(IEvent evt) => Invocations.Add(evt);
    }
}