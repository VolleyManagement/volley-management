using System.Collections.Generic;
using FluentValidation;
using LanguageExt;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.EventBroker;

namespace VolleyM.Domain.IDomainFrameworkTestFixture
{
	public class SampleHandler
	{
		public class Request : IRequest<Unit>
		{
			public int A { get; set; }

			public int B { get; set; }

		}

		public class Validator : AbstractValidator<Request>
		{
			public Validator()
			{
				RuleFor(r => r.A).Equal(0);
			}
		}

		public class Handler : IRequestHandler<Request, Unit>, ICanProduceEvent
		{
			public EitherAsync<Error, Unit> Handle(Request request)
			{
				DomainEvents.Add(new SampleEvent { Data = "SampleHandler invoked" });
				return Unit.Default;
			}

			public List<IEvent> DomainEvents { get; } = new List<IEvent>();
		}
	}
}