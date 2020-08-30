using LanguageExt;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;
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

		public class Handler : IRequestHandler<Request, Unit>
		{
			private readonly ICurrentUserProvider _currentUser;
			private readonly IPlayersRepository _repo;

			public Handler(IPlayersRepository repo, ICurrentUserProvider currentUser)
			{
				_repo = repo;
				_currentUser = currentUser;
			}

			public EitherAsync<Error, Unit> Handle(Request request)
			{
				var player = _repo.Get(_currentUser.Tenant, request.PlayerId);

				var result = player.Map(p =>
				  {
					  p.ChangeName(request.FirstName, request.LastName);
					  return p;
				  })
					.Map(p => _repo.Update(p));

				return result.MatchAsync<Either<Error, Unit>>(
						RightAsync: async right => await right.ToEither(),
						Left: l => l)
					.ToAsync();
			}
		}
	}
}