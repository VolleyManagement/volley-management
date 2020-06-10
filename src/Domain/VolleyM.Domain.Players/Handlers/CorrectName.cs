using System.Threading.Tasks;
using LanguageExt;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Players.PlayerAggregate;

namespace VolleyM.Domain.Players.Handlers
{
	public class CorrectName
	{
		public class Request : IRequest<Unit>
		{
			public PlayerId PlayerId { get; set; }

			public string FirstName { get; set; }

			public string LastName { get; set; }

			public override string ToString()
			{
				return $"Name={FirstName} {LastName}";
			}
		}

		public class Handler : IRequestHandlerOld<Request, Unit>
		{
			public Task<Either<Error, Unit>> Handle(Request request)
			{
				return Task.FromResult<Either<Error, Unit>>(Unit.Default);
			}
		}
	}
}