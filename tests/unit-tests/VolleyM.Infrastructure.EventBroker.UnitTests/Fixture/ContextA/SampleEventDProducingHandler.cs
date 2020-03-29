using VolleyM.Domain.Contracts.EventBroker;
using VolleyM.Infrastructure.EventBroker.UnitTests.Fixture;
using VolleyM.Infrastructure.EventBroker.UnitTests.Fixture.ContextA;

namespace VolleyM.Domain.ContextA
{
    public class SampleEventDProducingHandler
    {
        public class Request : EventProducingHandlerBase.Request
        {
        }

        public class Handler : EventProducingHandlerBase.Handler<Request>
        {
            protected override IEvent GetEvent(IEventProducingRequest request)
            {
                return new EventD
                {
                    SomeData = $"{nameof(SampleEventDProducingHandler)} invoked", RequestData = request.EventData
                };
            }
        }
    }
}