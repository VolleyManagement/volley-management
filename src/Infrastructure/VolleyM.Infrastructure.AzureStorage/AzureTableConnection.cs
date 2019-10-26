using LanguageExt;
using Microsoft.Azure.Cosmos.Table;
using Serilog;
using System;
using System.Threading.Tasks;
using VolleyM.Domain.Contracts;

namespace VolleyM.Infrastructure.AzureStorage
{
    /// <summary>
    /// Base class for all classes that need access to Azure Storage
    /// </summary>
    public abstract class AzureTableConnection
    {
        private readonly AzureTableStorageOptions _options;

        protected AzureTableConnection(AzureTableStorageOptions options)
        {
            _options = options;
        }

        protected Either<Error, CloudTableClient> OpenConnection()
        {
            if (!CloudStorageAccount.TryParse(_options.ConnectionString, out CloudStorageAccount account))
            {
                Log.Error("Azure Storage connection failed. Connection string is invalid");
                return Error.InternalError("Azure Storage account connection is invalid.");
            }

            return account.CreateCloudTableClient();
        }

        protected async Task<Either<Error, T>> PerformStorageOperation<T>(string tableName, Func<CloudTable, Task<Either<Error, T>>> operation, string operationName)
        {
            var conn = OpenConnection();

            try
            {
                var table = conn.Map(c => c.GetTableReference(tableName));
                return await table.Match(
                    operation,
                    e => Task.FromResult((Either<Error, T>)e));
            }
            catch (StorageException e) when (IsConflictError(e))
            {
                return Error.Conflict();
            }
            catch (StorageException e)
            {
                Log.Error(e, "{AzureStorageOperation} Azure Storage operation failed.", operationName);
                return Error.InternalError($"{operationName} Azure Storage operation failed.");
            }
        }

        private static bool IsConflictError(StorageException e) =>
            string.Compare("Conflict", e.Message, StringComparison.OrdinalIgnoreCase) == 0;
    }
}