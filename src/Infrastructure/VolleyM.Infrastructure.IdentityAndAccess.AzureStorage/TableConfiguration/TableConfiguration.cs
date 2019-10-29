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

        public async Task<Either<Error, Unit>> ConfigureTables()
        {
            var conn = OpenConnection();

            return await conn.MapAsync(async client =>
            {
                var tables = GetTables(_options);

                var createTasks = tables.Select(table =>
                {
                    var tableRef = client.GetTableReference(table);
                    return tableRef.CreateIfNotExistsAsync();
                }).ToList();

                await Task.WhenAll(createTasks);

                return Unit.Default;
            });
        }

        public async Task<Either<Error, Unit>> CleanTables()
        {
            var conn = OpenConnection();

            return await conn.MapAsync(async client =>
            {
                var tables = GetTables(_options);

                var deleteTasks = tables.Select(table =>
                {
                    var tableRef = client.GetTableReference(table);
                    return tableRef.DeleteIfExistsAsync();
                });

                await Task.WhenAll(deleteTasks);

                return Unit.Default;
            });
        }

        private static IEnumerable<string> GetTables(IdentityContextTableStorageOptions options)
        {
            return new List<string>
            {
                options.UsersTable
            };
        }
    }
}