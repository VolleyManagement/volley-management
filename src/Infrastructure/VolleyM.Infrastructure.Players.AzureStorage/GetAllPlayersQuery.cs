using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using LanguageExt;
using Microsoft.Azure.Cosmos.Table;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Players;
using VolleyM.Infrastructure.AzureStorage;
using VolleyM.Infrastructure.Players.AzureStorage.TableConfiguration;

namespace VolleyM.Infrastructure.Players.AzureStorage
{
	public class GetAllPlayersQuery : AzureTableConnection, IQuery<TenantId, List<PlayerDto>>
	{
		private readonly PlayersContextTableStorageOptions _options;
		private readonly IMapper _mapper;

		public GetAllPlayersQuery(PlayersContextTableStorageOptions options, IMapper mapper) : base(options)
		{
			_options = options;
			_mapper = mapper;
		}

		public EitherAsync<Error, List<PlayerDto>> Execute(TenantId tenant)
		{
			return PerformStorageOperation(_options.PlayersTable,
				tableRef =>
				{
					var filter = TableQuery.GenerateFilterCondition(
						nameof(PlayerEntity.PartitionKey),
						QueryComparisons.Equal,
						tenant.ToString());

					var query = new TableQuery<PlayerEntity> { FilterString = filter };

					// We are taking first segment for now as we don't have other cases yet
					// once it is not working feel free to refactor heavily
					var continuationToken = new TableContinuationToken();

					var getResult=Prelude
						.TryAsync(tableRef.ExecuteQuerySegmentedAsync(query, continuationToken))
						.ToEither(MapStorageError);

					return getResult.Match(
						tableResult => tableResult.Results
							.Select(p => _mapper.Map<PlayerEntity, PlayerDto>(p))
							.ToList(),
						MapError
					).ToAsync();
				}, "Get All Player");
		}
		
		private static Either<Error, List<PlayerDto>> MapError(Error e)
		{
			return e;
		}
	}
}