using System.Threading.Tasks;
using FluentValidation;
using LanguageExt;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;
using VolleyM.Domain.Players.PlayerAggregate;

namespace VolleyM.Domain.Players.Handlers
{
	public class CreatePlayer
	{
		public class Request : IRequest<Player>
		{
			public TenantId Tenant { get; set; }

			public string FirstName { get; set; }

			public string LastName { get; set; }

			public override string ToString()
			{
				return $"Tenant:{Tenant};Name={FirstName} {LastName}";
			}
		}

		public class Validator : AbstractValidator<Request>
		{
			public Validator()
			{
				RuleFor(r => r.Tenant)
					.NotNull();

				RuleFor(r => r.FirstName)
					.NotEmpty()
					.MaximumLength(60);

				RuleFor(r => r.LastName)
					.NotEmpty()
					.MaximumLength(60);
			}
		}

		public class Handler : IRequestHandler<Request, Player>
		{
			private readonly IPlayersRepository _repository;
			private readonly IRandomIdGenerator _idGenerator;

			public Handler(IPlayersRepository repository, IRandomIdGenerator idGenerator)
			{
				_repository = repository;
				_idGenerator = idGenerator;
			}

			public async Task<Either<Error, Player>> Handle(Request request)
			{
				var id = new PlayerId(_idGenerator.GetRandomId());
				var player = new Player(request.Tenant, id, request.FirstName, request.LastName);

				var addResult = await _repository.Add(player);

				return addResult;
			}
		}
	}
}