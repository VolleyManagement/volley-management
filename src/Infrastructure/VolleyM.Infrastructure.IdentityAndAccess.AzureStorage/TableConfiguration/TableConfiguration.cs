using System;
using LanguageExt;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VolleyM.Domain.Contracts;
using VolleyM.Infrastructure.AzureStorage;

namespace VolleyM.Infrastructure.IdentityAndAccess.AzureStorage.TableConfiguration
{
    public class TableConfiguration : AzureTableConnection
    {
        private readonly IdentityContextTableStorageOptions _options;

        public TableConfiguration(IdentityContextTableStorageOptions options)
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
				_options.UsersTable
			};
		}
    }
}