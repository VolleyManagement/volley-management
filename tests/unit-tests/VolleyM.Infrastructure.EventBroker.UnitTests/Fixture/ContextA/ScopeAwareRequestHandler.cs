using VolleyM.Domain.Contracts.EventBroker;
using VolleyM.Infrastructure.EventBroker.UnitTests.Fixture;
using VolleyM.Infrastructure.EventBroker.UnitTests.Fixture.ContextA;

namespace VolleyM.Domain.ContextA
{
    public class ScopeAwareRequestHandler
    {
        public class Request : EventProducingHandlerBase.Request
        {
        }

        public class Handler : EventProducingHandlerBase.Handler<Request>
        {
            private readonly ScopeLitmus _scopeLitmus;

            public Handler(ScopeLitmus scopeLitmus)
            {
                _scopeLitmus = scopeLitmus;
            }

            protected override IEvent GetEvent(IEventProducingRequest request)
            {
                return new EventJ
                {
                    SomeData = $"{nameof(ScopeAwareRequestHandler)} invoked",
                    RequestData = request.EventData,
                    RequestScope = _scopeLitmus.Scope
                };
            }
        }
    }
}