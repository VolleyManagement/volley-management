using System.Threading.Tasks;
using VolleyM.Domain.Framework.EventBus;

namespace VolleyM.Infrastructure.EventBroker.UnitTests.Fixture.ContextA
{
    public class SampleApplicationService : IEventHandler<EventA>
    {
        public Task Handle(EventA @event)
        {
            throw new System.NotImplementedException();
        }
    }
}