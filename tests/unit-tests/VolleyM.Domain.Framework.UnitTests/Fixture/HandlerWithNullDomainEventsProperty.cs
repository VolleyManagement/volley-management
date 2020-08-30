using System.Collections.Generic;
using LanguageExt;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.EventBroker;

namespace VolleyM.Domain.IDomainFrameworkTestFixture
{
	public class HandlerWithNullDomainEventsProperty
	{
		public class Request : IRequest<Unit>
		{
			public int A { get; set; }

			public int B { get; set; }
		}

		public class Handler : IRequestHandler<Request, Unit>, ICanProduceEvent
		{
			public EitherAsync<Error, Unit> Handle(Request request)
			{
				return Unit.Default;
			}

			public List<IEvent> DomainEvents { get; }
		}
	}
}