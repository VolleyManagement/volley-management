using System.Collections.Generic;
using VolleyM.Infrastructure.AzureStorage;

namespace VolleyM.Infrastructure.IdentityAndAccess.AzureStorage.TableConfiguration
{
    public class TableConfiguration : AzureTableConfiguration
    {
        private readonly IdentityContextTableStorageOptions _options;

        public TableConfiguration(IdentityContextTableStorageOptions options)
            : base(options)
        {
            _options = options;
        }

        protected override IEnumerable<string> GetTablesForContext()
        {
			return new List<string>
			{
				_options.UsersTable
			};
		}
    }
}