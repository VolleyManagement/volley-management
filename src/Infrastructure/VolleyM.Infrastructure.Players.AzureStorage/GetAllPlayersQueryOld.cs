using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LanguageExt;
using Microsoft.Azure.Cosmos.Table;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Players;
using VolleyM.Domain.Players.PlayerAggregate;
using VolleyM.Infrastructure.AzureStorage;
using VolleyM.Infrastructure.Players.AzureStorage.TableConfiguration;

namespace VolleyM.Infrastructure.Players.AzureStorage
{
	[Obsolete]
	public class GetAllPlayersQueryOld : AzureTableConnection, IQueryOld<TenantId, List<PlayerDto>>
	{
		private readonly PlayersContextTableStorageOptions _options;
		private readonly PlayerFactory _playerFactory;
		private readonly IMapper _mapper;

		public GetAllPlayersQueryOld(PlayersContextTableStorageOptions options, PlayerFactory playerFactory, IMapper mapper) : base(options)
		{
			_options = options;
			_playerFactory = playerFactory;
			_mapper = mapper;
		}

		public Task<Either<Error, List<PlayerDto>>> Execute(TenantId tenant)
		{
			return PerformStorageOperationOld(_options.PlayersTable,
				async tableRef =>
				{
					var filter = TableQuery.GenerateFilterCondition(
						nameof(PlayerEntity.PartitionKey),
						QueryComparisons.Equal,
						tenant.ToString());

					var query = new TableQuery<PlayerEntity> { FilterString = filter };

					// We are taking first segment for now as we don't have other cases yet
					// once it is not working feel free to refactor heavily
					var continuationToken = new TableContinuationToken();
					var getResult = (Either<Error, TableQuerySegment<PlayerEntity>>)
						await tableRef.ExecuteQuerySegmentedAsync(query, continuationToken);

					return getResult.Match(
						tableResult => (Either<Error, List<PlayerDto>>)tableResult.Results
							.Select(p => _mapper.Map<PlayerEntity, PlayerDto>(p))
							.ToList(),
						e => e
					);
				}, "Get All Player");
		}
	}
}