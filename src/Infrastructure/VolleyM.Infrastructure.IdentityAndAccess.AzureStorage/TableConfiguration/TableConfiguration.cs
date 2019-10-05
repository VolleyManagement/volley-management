using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VolleyM.Domain.Contracts;

namespace VolleyM.Infrastructure.IdentityAndAccess.AzureStorage.TableConfiguration
{
    public class TableConfiguration
    {
        private readonly IdentityContextTableStorageOptions _options;

        public TableConfiguration(IdentityContextTableStorageOptions options)
        {
            _options = options;
        }

        public string AccountName { get; private set; }

        public async Task<Result<Unit>> ConfigureTables()
        {
            var conn = OpenConnection();
            if (!conn.IsSuccessful)
            {
                return conn.Error;
            }
            var client = conn.Value;

            var tables = GetTables(_options);

            var createTasks = tables.Select(table =>
            {
                var tableRef = client.GetTableReference(table);
                return tableRef.CreateIfNotExistsAsync();
            }).ToList();

            await Task.WhenAll(createTasks);

            return Unit.Value;
        }

        public async Task<Result<Unit>> CleanTables()
        {
            var conn = OpenConnection();
            if (!conn.IsSuccessful)
            {
                return conn.Error;
            }
            var client = conn.Value;

            var tables = GetTables(_options);

            var deleteTasks = tables.Select(table =>
            {
                var tableRef = client.GetTableReference(table);
                return tableRef.DeleteIfExistsAsync();
            });

            await Task.WhenAll(deleteTasks);

            return Unit.Value;
        }

        private static IEnumerable<string> GetTables(IdentityContextTableStorageOptions options)
        {
            return new List<string>
            {
                options.UsersTable
            };
        }

        private Result<CloudTableClient> OpenConnection()
        {
            if (!CloudStorageAccount.TryParse(_options.ConnectionString, out CloudStorageAccount account))
            {
                return Error.InternalError("Azure Storage account connection is invalid.");
            }

            AccountName = account.Credentials.AccountName;

            return account.CreateCloudTableClient();
        }
    }
}