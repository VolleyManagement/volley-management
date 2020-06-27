using LanguageExt;
using VolleyM.Domain.Contracts;

namespace RootNs
{
	public class RootNsHandler
	{
		public class Request : IRequest<Unit>
		{
			public int A { get; set; }

			public int B { get; set; }
		}

		public class Handler : IRequestHandler<RootNsHandler.Request, Unit>
		{
			public EitherAsync<Error, Unit> Handle(Request request)
			{
				return Unit.Default;
			}
		}
	}
}