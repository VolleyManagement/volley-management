using LanguageExt;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.IDomainFrameworkTestFixture
{
	public class NoEventSupportHandler
	{
		public class Request : IRequest<Unit>
		{
			public int A { get; set; }

			public int B { get; set; }
		}

		public class Handler : IRequestHandler<Request, Unit>
		{
			public EitherAsync<Error, Unit> Handle(Request request)
			{
				return Unit.Default;
			}
		}
	}
}