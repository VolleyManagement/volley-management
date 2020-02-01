using System.Collections.Generic;
using System.Threading.Tasks;
using VolleyM.Domain.Framework.EventBus;

namespace VolleyM.Domain.IDomainFrameworkTestFixture
{
    public class SpyEventListener : IEventHandler<SampleEvent>
    {
        public Task Handle(SampleEvent @event)
        {
            Events.Add(@event);
            return Task.CompletedTask;
        }

        public List<object> Events { get; set; } = new List<object>();
    }
}