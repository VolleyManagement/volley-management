using System.Threading.Tasks;
using AutoMapper;
using LanguageExt;
using Microsoft.Azure.Cosmos.Table;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Players.PlayerAggregate;
using VolleyM.Infrastructure.AzureStorage;
using VolleyM.Infrastructure.Players.AzureStorage.TableConfiguration;
using Error = VolleyM.Domain.Contracts.Error;

namespace VolleyM.Infrastructure.Players.AzureStorage
{
	public class PlayersRepository : AzureTableConnection, IPlayersRepository
	{
		private readonly PlayersContextTableStorageOptions _options;
		private readonly PlayerFactory _playerFactory;
		private readonly IMapper _mapper;

		public PlayersRepository(PlayersContextTableStorageOptions options, PlayerFactory playerFactory, IMapper mapper) : base(options)
		{
			_options = options;
			_playerFactory = playerFactory;
			_mapper = mapper;
		}

		public Task<Either<Error, Player>> Get(TenantId tenant, PlayerId id)
		{
			return PerformStorageOperation(_options.PlayersTable,
				async tableRef =>
				{
					var getOperation = TableOperation.Retrieve<PlayerEntity>(tenant.ToString(), id.ToString());

					var getResult = (Either<Error, TableResult>)await tableRef.ExecuteAsync(getOperation);

					return getResult.Match(
						tableResult => tableResult.Result switch
						{
							PlayerEntity playerEntity => (Either<Error, Player>)_playerFactory.Create(
								_mapper.Map<PlayerEntity, PlayerFactoryDto>(playerEntity)),
							_ => Error.NotFound()
						},
						e => e
					);
				}, "Get Player");
		}

		public Task<Either<Error, Player>> Add(Player player)
		{
			return PerformStorageOperation(_options.PlayersTable,
				async tableRef =>
				{
					var playerEntity = new PlayerEntity(player);
					var createOperation = TableOperation.Insert(playerEntity);

					var createResult = (Either<Error, TableResult>)await tableRef.ExecuteAsync(createOperation);

					return createResult.Match(
						tableResult => tableResult.Result switch
						{
							PlayerEntity created => (Either<Error, Player>)_playerFactory.Create(
								_mapper.Map<PlayerEntity, PlayerFactoryDto>(created)),
							_ => Error.InternalError(
								$"Azure Storage: Failed to create player with {tableResult.HttpStatusCode} error.")
						},
						e => e
					);
				}, "Create Player");
		}
	}
}