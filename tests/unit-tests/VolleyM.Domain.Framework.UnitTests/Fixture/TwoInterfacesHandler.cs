using LanguageExt;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.Framework.UnitTests.Fixture
{
	public class TwoInterfacesHandler
	{
		public class Request : IRequest<Unit>
		{
			public int A { get; set; }

			public int B { get; set; }
		}

		public class Request2 : IRequest<Unit>
		{
			public int A { get; set; }

			public int B { get; set; }

		}

		public class Handler : IRequestHandler<Request, Unit>, IRequestHandler<Request2, Unit>
		{
			public EitherAsync<Error, Unit> Handle(Request request)
			{
				return Unit.Default;
			}

			public EitherAsync<Error, Unit> Handle(Request2 request)
			{
				return Unit.Default;
			}
		}
	}
}