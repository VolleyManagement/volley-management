using VolleyM.Infrastructure.EventBroker.UnitTests.Fixture;
using VolleyM.Infrastructure.EventBroker.UnitTests.Fixture.ContextA;

namespace VolleyM.Domain.ContextA
{
    public class AnotherEventAProducingHandler
    {
        public class Request : EventProducingHandlerBase.Request
        {
        }

        public class Handler : EventProducingHandlerBase.Handler<Request>
        {
            protected override object GetEvent(IEventProducingRequest request)
            {
                return new EventA
                {
                    SomeData = $"{nameof(AnotherEventAProducingHandler)} invoked",
                    RequestData = request.EventData
                };
            }
        }
    }
}