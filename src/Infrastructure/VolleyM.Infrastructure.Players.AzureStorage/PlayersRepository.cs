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

		public EitherAsync<Error, Player> Get(TenantId tenant, PlayerId id)
		{
			return PerformStorageOperation(_options.PlayersTable,
				 tableRef =>
				{
					var getOperation = TableOperation.Retrieve<PlayerEntity>(tenant.ToString(), id.ToString());

					EitherAsync<Error, TableResult> getResult = tableRef.ExecuteAsync(getOperation);

					return getResult.Match<Either<Error, Player>>(
						tableResult => tableResult.Result switch
						{
							PlayerEntity playerEntity => _playerFactory.Create(
								_mapper.Map<PlayerEntity, PlayerFactoryDto>(playerEntity)),
							_ => Error.NotFound()
						},
						e => e
					).ToAsync();
				}, "Get Player");
		}

		public EitherAsync<Error, Player> Add(Player player)
		{
			return PerformStorageOperation(_options.PlayersTable,
				tableRef =>
				{
					var playerEntity = new PlayerEntity(player);
					var createOperation = TableOperation.Insert(playerEntity);

					var createResult = (EitherAsync<Error, TableResult>)tableRef.ExecuteAsync(createOperation);

					return createResult.Match(
						tableResult => tableResult.Result switch
						{
							PlayerEntity created => (Either<Error, Player>)_playerFactory.Create(
								_mapper.Map<PlayerEntity, PlayerFactoryDto>(created)),
							_ => Error.InternalError(
								$"Azure Storage: Failed to create player with {tableResult.HttpStatusCode} error.")
						},
						e => e
					).ToAsync();
				}, "Create Player");
		}

		public EitherAsync<Error, Unit> Update(Player player)
		{
			return PerformStorageOperation(_options.PlayersTable,
				tableRef =>
				{
					var playerEntity = new PlayerEntity(player);

					playerEntity.ETag = "*";

					var mergeOperation = TableOperation.Merge(playerEntity);

					var mergeResult = (EitherAsync<Error, TableResult>)tableRef.ExecuteAsync(mergeOperation);

					return mergeResult.Match(
						tableResult => tableResult.Result switch
						{
							PlayerEntity updated => (Either<Error, Unit>)Unit.Default,
							_ => Error.InternalError(
								$"Azure Storage: Failed to create player with {tableResult.HttpStatusCode} error.")
						},
						e => e
					).ToAsync();
				}, "Update Player");
		}

		public EitherAsync<Error, Unit> Delete(TenantId tenant, PlayerId id)
		{
			return PerformStorageOperation<Unit>(_options.PlayersTable,
				tableRef =>
				{
					var playerEntity = new PlayerEntity(tenant, id);

					playerEntity.ETag = "*";

					var deleteOperation = TableOperation.Delete(playerEntity);

					EitherAsync<Error, TableResult> result = tableRef.ExecuteAsync(deleteOperation);

					return result.Map(tr => Unit.Default);
				}, "Delete Player");
		}
	}
}