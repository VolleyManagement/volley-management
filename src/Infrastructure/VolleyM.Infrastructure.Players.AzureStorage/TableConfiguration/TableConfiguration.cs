using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using VolleyM.Domain.Contracts;
using VolleyM.Infrastructure.AzureStorage;

namespace VolleyM.Infrastructure.Players.AzureStorage.TableConfiguration
{
    public class TableConfiguration : AzureTableConnection
    {
        private readonly PlayersContextTableStorageOptions _options;

        public TableConfiguration(PlayersContextTableStorageOptions options)
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

        private static IEnumerable<string> GetTables(PlayersContextTableStorageOptions options)
        {
            return new List<string>
            {
                options.PlayersTable
            };
        }
    }
}