using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using VolleyM.Domain.Contracts;

namespace VolleyM.Infrastructure.AzureStorage
{
	public abstract class AzureTableConfiguration : AzureTableConnection
	{
		protected AzureTableConfiguration(AzureTableStorageOptions options) : base(options)
		{
		}

		/// <summary>
		/// When overriden should return all the tables particular configuration should be responsible for.
		/// </summary>
		/// <returns></returns>
		protected abstract IEnumerable<string> GetTablesForContext();

		public EitherAsync<Error, Unit> ConfigureTables()
		{
			var conn = base.OpenConnection();

			return conn.MapAsync(async client =>
				{
					var tables = GetTablesForContext();

					var createTasks = tables.Select(table =>
					{
						var tableRef = client.GetTableReference(table);
						return tableRef.CreateIfNotExistsAsync();
					}).ToList();

					await Task.WhenAll(createTasks);

					return Unit.Default;
				})
				.ToAsync();
		}

		public EitherAsync<Error, Unit> CleanTables()
		{
			var conn = base.OpenConnection();

			return conn.MapAsync(async client =>
				{
					var tables = GetTablesForContext();

					var deleteTasks = tables.Select(table =>
					{
						var tableRef = client.GetTableReference(table);
						return tableRef.DeleteIfExistsAsync();
					});

					await Task.WhenAll(deleteTasks);

					return Unit.Default;
				})
				.ToAsync();
		}
	}
}