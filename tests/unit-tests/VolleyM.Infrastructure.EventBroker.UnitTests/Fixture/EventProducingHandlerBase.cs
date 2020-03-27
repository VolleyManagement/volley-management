using LanguageExt;
using System.Collections.Generic;
using System.Threading.Tasks;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Framework.EventBroker;

namespace VolleyM.Infrastructure.EventBroker.UnitTests.Fixture
{
    public class EventProducingHandlerBase
    {
        public abstract class Request : IRequest<Unit>, IEventProducingRequest
        {
            public int EventData { get; set; }
        }

        public abstract class Handler<T> : IRequestHandler<T, Unit>, ICanProduceEvent 
            where T:Request
        {
            public Task<Either<Error, Unit>> Handle(T request)
            {
                DomainEvents.Add(GetEvent(request));
                return Task.FromResult<Either<Error, Unit>>(Unit.Default);
            }

            protected abstract object GetEvent(IEventProducingRequest request);

            public List<object> DomainEvents { get; } = new List<object>();
        }
    }
}