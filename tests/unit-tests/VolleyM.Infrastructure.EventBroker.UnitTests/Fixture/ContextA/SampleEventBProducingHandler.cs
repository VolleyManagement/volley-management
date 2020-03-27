using VolleyM.Infrastructure.EventBroker.UnitTests.Fixture;
using VolleyM.Infrastructure.EventBroker.UnitTests.Fixture.ContextA;

namespace VolleyM.Domain.ContextA
{
    public class SampleEventBProducingHandler
    {
        public class Request : EventProducingHandlerBase.Request
        {
        }

        public class Handler : EventProducingHandlerBase.Handler<Request>
        {
            protected override object GetEvent(IEventProducingRequest request)
            {
                return new EventB
                {
                    SomeData = $"{nameof(SampleEventBProducingHandler)} invoked", RequestData = request.EventData
                };
            }
        }
    }
}