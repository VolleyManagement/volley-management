using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Framework.EventBroker;

namespace VolleyM.Domain.IDomainFrameworkTestFixture
{
    public class SeveralEventsHandler
    {
        public class Request : IRequest<Unit>
        {
            public int A { get; set; }

            public int B { get; set; }

        }

        public class SampleEventA
        {
            public string Data { get; set; }
        }

        public class SampleEventB
        {
            public string Data { get; set; }
        }

        public class Handler : IRequestHandler<Request, Unit>, ICanProduceEvent
        {
            public Task<Either<Error, Unit>> Handle(Request request)
            {
                DomainEvents.Add(new SampleEventA());
                DomainEvents.Add(new SampleEventB());
                return Task.FromResult<Either<Error, Unit>>(Unit.Default);
            }

            public List<object> DomainEvents { get; } = new List<object>();
        }
    }
}