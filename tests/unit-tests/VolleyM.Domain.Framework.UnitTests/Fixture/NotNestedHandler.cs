using LanguageExt;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.Framework.UnitTests.Fixture
{
	public class NotNestedHandler : IRequestHandler<NotNestedHandler.Request, Unit>
	{
		public class Request : IRequest<Unit>
		{
			public int A { get; set; }

			public int B { get; set; }
		}

		public EitherAsync<Error, Unit> Handle(Request request)
		{
			return Unit.Default;
		}
	}
}