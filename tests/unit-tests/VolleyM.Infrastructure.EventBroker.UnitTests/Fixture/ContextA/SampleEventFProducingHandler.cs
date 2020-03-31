using VolleyM.Domain.Contracts.EventBroker;
using VolleyM.Infrastructure.EventBroker.UnitTests.Fixture;
using VolleyM.Infrastructure.EventBroker.UnitTests.Fixture.ContextA;

namespace VolleyM.Domain.ContextA
{
    public class SampleEventFProducingHandler
    {
        public class Request : EventProducingHandlerBase.Request
        {
        }

        public class Handler : EventProducingHandlerBase.Handler<Request>
        {
            protected override IEvent GetEvent(IEventProducingRequest request)
            {
                return new EventF
                {
                    SomeData = $"{nameof(SampleEventFProducingHandler)} invoked", RequestData = request.EventData
                };
            }
        }
    }
}