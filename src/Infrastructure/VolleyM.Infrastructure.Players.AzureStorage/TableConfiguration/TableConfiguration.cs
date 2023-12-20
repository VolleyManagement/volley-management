using System.Collections.Generic;
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

		protected override IEnumerable<string> GetTablesForContext()
		{
			return new List<string>
			{
				_options.PlayersTable
			};
		}
	}
}