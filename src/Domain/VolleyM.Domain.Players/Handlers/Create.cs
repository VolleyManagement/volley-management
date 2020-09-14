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
	public class Create
	{
		public class Request : IRequest<Player>
		{
			public string FirstName { get; set; }

			public string LastName { get; set; }

			public override string ToString()
			{
				return $"Name={FirstName} {LastName}";
			}
		}

		public class Validator : AbstractValidator<Request>
		{
			public Validator()
			{
				RuleFor(r => r.FirstName)
					.NotEmpty()
					.MaximumLength(60);

				RuleFor(r => r.LastName)
					.NotEmpty()
					.MaximumLength(60);
			}
		}

		public class Handler : IRequestHandler<Request, Player>, ICanProduceEvent
		{
			private readonly IPlayersRepository _repository;
			private readonly IRandomIdGenerator _idGenerator;
			private readonly ICurrentUserProvider _currentUser;

			public Handler(IPlayersRepository repository, IRandomIdGenerator idGenerator, ICurrentUserProvider currentUser)
			{
				_repository = repository;
				_idGenerator = idGenerator;
				_currentUser = currentUser;
			}

			public EitherAsync<Error, Player> Handle(Request request)
			{
				var id = new PlayerId(_idGenerator.GetRandomId());
				var player = new Player(_currentUser.Tenant, Version.Initial, id, request.FirstName, request.LastName);

				var addResult = _repository.Add(player);

				return addResult
					.Do(createdPlayer =>
					{
						DomainEvents.Add(new PlayerCreated
						{
							TenantId = createdPlayer.Tenant,
							Version = createdPlayer.Version,
							PlayerId = createdPlayer.Id,
							FirstName = createdPlayer.FirstName,
							LastName = createdPlayer.LastName
						});
					});
			}

			public List<IEvent> DomainEvents { get; } = new List<IEvent>();
		}
	}
}