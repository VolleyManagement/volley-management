using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using LanguageExt;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;
using VolleyM.Domain.Contracts.EventBroker;
using VolleyM.Domain.Players.Events;
using VolleyM.Domain.Players.PlayerAggregate;

namespace VolleyM.Domain.Players.Handlers
{
	public class CreateOld
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

		public class Handler : IRequestHandlerOld<Request, Player>, ICanProduceEvent
		{
			private readonly IPlayersRepositoryOld _repository;
			private readonly IRandomIdGenerator _idGenerator;
			private readonly ICurrentUserProvider _currentUser;

			public Handler(IPlayersRepositoryOld repository, IRandomIdGenerator idGenerator, ICurrentUserProvider currentUser)
			{
				_repository = repository;
				_idGenerator = idGenerator;
				_currentUser = currentUser;
			}

			public async Task<Either<Error, Player>> Handle(Request request)
			{
				var id = new PlayerId(_idGenerator.GetRandomId());
				var player = new Player(_currentUser.Tenant, id, request.FirstName, request.LastName);

				var addResult = await _repository.Add(player);

				return addResult
					.Map(createdPlayer =>
					{
						DomainEvents.Add(new PlayerCreated
						{
							TenantId = player.Tenant,
							PlayerId = player.Id,
							FirstName = player.FirstName,
							LastName = player.LastName
						});
						return createdPlayer;
					});
			}

			public List<IEvent> DomainEvents { get; } = new List<IEvent>();
		}
	}
}