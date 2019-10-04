using Microsoft.Azure.Cosmos.Table;
using Serilog;
using System;
using System.Threading.Tasks;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.IdentityAndAccess;
using VolleyM.Infrastructure.IdentityAndAccess.AzureStorage.TableConfiguration;

namespace VolleyM.Infrastructure.IdentityAndAccess.AzureStorage
{
    public class AzureStorageUserRepository : IUserRepository
    {
        private readonly IdentityContextTableStorageOptions _options;

        public AzureStorageUserRepository(IdentityContextTableStorageOptions options)
        {
            _options = options;
        }

        public Task<Result<Unit>> Add(User user)
        {
            return PerformStorageOperation<Unit>(async tableRef =>
            {
                var userEntity = new UserEntity(user);
                var createOperation = TableOperation.Insert(userEntity);

                await tableRef.ExecuteAsync(createOperation);

                return Unit.Value;
            }, "Add User");
        }

        public Task<Result<User>> Get(TenantId tenant, UserId id)
        {
            return PerformStorageOperation<User>(async tableRef =>
            {
                var getOperation = TableOperation.Retrieve<UserEntity>(tenant.ToString(), id.ToString());

                var result = await tableRef.ExecuteAsync(getOperation);

                if (result.Result is UserEntity userEntity)
                {
                    return new User(new UserId(userEntity.RowKey), new TenantId(userEntity.RowKey));
                }

                return Error.NotFound();
            }, "Get User");
        }

        public Task<Result<Unit>> Delete(TenantId tenant, UserId id)
        {
            return PerformStorageOperation<Unit>(async tableRef =>
            {
                var userEntity = new UserEntity(tenant, id);
                var deleteOperation = TableOperation.Delete(userEntity);

                await tableRef.ExecuteAsync(deleteOperation);

                return Unit.Value;
            }, "Delete User");
        }

        private Result<CloudTable> OpenConnection()
        {
            if (!CloudStorageAccount.TryParse(_options.ConnectionString, out CloudStorageAccount account))
            {
                Log.Error("Failed to initialize Azure Storage connection. Check connection string.");
                return Error.InternalError("Failed to initialize Azure Storage connection. Check connection string.");
            }

            var client = account.CreateCloudTableClient();
            var tableRef = client.GetTableReference(_options.UsersTable);

            return tableRef;
        }

        private async Task<Result<T>> PerformStorageOperation<T>(Func<CloudTable, Task<Result<T>>> operation, string operationName)
            where T : class
        {
            var conn = OpenConnection();
            if (!conn.IsSuccessful)
            {
                return conn.Error;
            }
            var tableRef = conn.Value;

            try
            {
                return await operation(tableRef);
            }
            catch (StorageException e)
            {
                Log.Error(e, "{AzureStorageOperation} Azure Storage operation failed.", operationName);
                return Error.InternalError($"{operationName} Azure Storage operation failed.");
            }
        }
    }
}