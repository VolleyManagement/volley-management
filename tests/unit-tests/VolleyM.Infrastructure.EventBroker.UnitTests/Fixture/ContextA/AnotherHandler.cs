using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Framework.EventBroker;
using VolleyM.Infrastructure.EventBroker.UnitTests.Fixture.ContextA;

namespace VolleyM.Domain.ContextA
{
    public class AnotherHandler
    {
        public class Request : IRequest<Unit>
        {
            public int EventData { get; set; }
        }

        public class Handler : IRequestHandler<Request, Unit>, ICanProduceEvent
        {
            public Task<Either<Error, Unit>> Handle(Request request)
            {
                DomainEvents.Add(new EventA { SomeData = "AnotherHandler invoked", RequestData = request.EventData });
                return Task.FromResult<Either<Error, Unit>>(Unit.Default);
            }

            public List<object> DomainEvents { get; } = new List<object>();
        }
    }
}