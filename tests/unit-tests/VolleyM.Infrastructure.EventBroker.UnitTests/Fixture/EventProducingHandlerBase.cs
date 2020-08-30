using System.Collections.Generic;
using LanguageExt;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.EventBroker;

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
            public EitherAsync<Error, Unit> Handle(T request)
            {
                DomainEvents.Add(GetEvent(request));
                return Unit.Default;
            }

            protected abstract IEvent GetEvent(IEventProducingRequest request);

            public List<IEvent> DomainEvents { get; } = new List<IEvent>();
        }
    }
}