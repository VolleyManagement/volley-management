using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using VolleyM.Domain.Contracts;
using VolleyM.Infrastructure.AzureStorage;

namespace VolleyM.Infrastructure.Players.AzureStorage.TableConfiguration
{
	public class TableConfiguration : AzureTableConfiguration
	{
		private readonly PlayersContextTableStorageOptions _options;

		public TableConfiguration(PlayersContextTableStorageOptions options)
			: base(options)
		{
			_options = options;
		}

		[Obsolete]
		public async Task<Either<Error, Unit>> ConfigureTablesOld()
		{
			var conn = OpenConnection();

			return await conn.MapAsync(async client =>
			{
				var tables = GetTablesForContext();

				var createTasks = tables.Select(table =>
				{
					var tableRef = client.GetTableReference(table);
					return tableRef.CreateIfNotExistsAsync();
				}).ToList();

				await Task.WhenAll(createTasks);

				return Unit.Default;
			});
		}

		[Obsolete]
		public async Task<Either<Error, Unit>> CleanTablesOld()
		{
			var conn = OpenConnection();

			return await conn.MapAsync(async client =>
			{
				var tables = GetTablesForContext();

				var deleteTasks = tables.Select(table =>
				{
					var tableRef = client.GetTableReference(table);
					return tableRef.DeleteIfExistsAsync();
				});

				await Task.WhenAll(deleteTasks);

				return Unit.Default;
			});
		}

		protected override IEnumerable<string> GetTablesForContext()
		{
			return new List<string>
			{
				_options.PlayersTable
			};
		}
	}
}