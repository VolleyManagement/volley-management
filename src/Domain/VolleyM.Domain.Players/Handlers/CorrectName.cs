using System.Collections.Generic;
using FluentValidation;
using LanguageExt;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;
using VolleyM.Domain.Contracts.EventBroker;
using VolleyM.Domain.Players.Events;
using VolleyM.Domain.Players.PlayerAggregate;

namespace VolleyM.Domain.Players.Handlers
{
	public class CorrectName
	{
		public class Request : IRequest<Unit>, IPlayerNameRequest
		{
			public PlayerId PlayerId { get; set; }

			public Version EntityVersion { get; set; }

			public string FirstName { get; set; }

			public string LastName { get; set; }

			public override string ToString()
			{
				return $"Name={FirstName} {LastName}";
			}
		}

		public class Validator : AbstractValidator<CorrectName.Request>
		{
			public Validator()
			{
				Include(new PlayerNameValidator());
			}
		}

		public class Handler : IRequestHandler<Request, Unit>, ICanProduceEvent
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

				var result = player
					.Map(p =>
					{
						p.ChangeName(request.FirstName, request.LastName);
						return _repo.Update(p, request.EntityVersion);
					})
					.MatchAsync(
						RightAsync: async right => await right.ToEither(),
						Left: l => l)
					.ToAsync()
					.Do(p => DomainEvents.Add(new PlayerNameCorrected
					{
						TenantId = p.Tenant,
						PlayerId = p.Id,
						Version = p.Version,
						FirstName = p.FirstName,
						LastName = p.LastName
					}))
					.Map(p => Unit.Default);

				return result;
			}

			public List<IEvent> DomainEvents { get; } = new List<IEvent>();
		}
	}
}