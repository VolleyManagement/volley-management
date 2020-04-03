using System.Threading.Tasks;
using LanguageExt;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Players.PlayerAggregate;

namespace VolleyM.Domain.Players.Handlers
{
	public class CreatePlayer
	{
		public class Request : IRequest<Player>
		{
			public TenantId Tenant { get; set; }

			public PlayerId Id { get; set; }

			public string FirstName { get; set; }

			public string LastName { get; set; }

			public override string ToString()
			{
				return $"PlayerId:{Id};Tenant:{Tenant}";
			}
		}

		public class Handler : IRequestHandler<Request, Player>
		{
			private IPlayersRepository _repository;

			public Handler(IPlayersRepository repository)
			{
				_repository = repository;
			}

			public async Task<Either<Error, Player>> Handle(Request request)
			{
				var player = new Player(request.Tenant, request.Id, request.FirstName, request.LastName);
				
				var addResult = await _repository.Add(player);

				return addResult;
			}
		}
	}
}