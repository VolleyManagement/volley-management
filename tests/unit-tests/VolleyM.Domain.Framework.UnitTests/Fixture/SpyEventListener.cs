using System.Collections.Generic;
using System.Threading.Tasks;
using VolleyM.Domain.Framework.EventBus;

namespace VolleyM.Domain.IDomainFrameworkTestFixture
{
    public class SpyEventListener : IEventHandler<SampleEvent>
    {
        public Task Handle(SampleEvent @event)
        {
            throw new System.NotImplementedException();
        }

        public List<object> Events { get; set; }
    }
}