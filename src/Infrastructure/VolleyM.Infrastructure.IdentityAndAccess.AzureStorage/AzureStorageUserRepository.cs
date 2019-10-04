using Microsoft.Azure.Cosmos.Table;
using System.Threading.Tasks;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.IdentityAndAccess;

namespace VolleyM.Infrastructure.IdentityAndAccess.AzureStorage
{
    public class AzureStorageUserRepository : IUserRepository
    {
        public async Task<Result<Unit>> Add(User user)
        {
            var conn = await OpenConnection();

            if (!conn.IsSuccessful)
            {
                return conn.Error;
            }

            var tableRef = conn.Value;

            try
            {
                var userEntity = new UserEntity(user);
                var createOperation = TableOperation.Insert(userEntity);

                await tableRef.ExecuteAsync(createOperation);

                return Unit.Value;
            }
            catch (StorageException e)
            {
                return Error.InternalError($"Azure Storage Failed to store record.");
            }
        }

        public async Task<Result<User>> Get(TenantId tenant, UserId id)
        {
            var conn = await OpenConnection();
            if (!conn.IsSuccessful)
            {
                return conn.Error;
            }
            var tableRef = conn.Value;

            try
            {
                var getOperation = TableOperation.Retrieve<UserEntity>(tenant.ToString(), id.ToString());

                var result = await tableRef.ExecuteAsync(getOperation);

                var userEntity = result.Result as UserEntity;

                if (userEntity != null)
                {
                    return new User(new UserId(userEntity.RowKey), new TenantId(userEntity.RowKey));
                }

                return Error.NotFound();
            }
            catch (StorageException e)
            {
                return Error.InternalError($"Azure Storage Failed to retrieve record.");
            }
        }

        public async Task<Result<Unit>> Delete(TenantId tenant, UserId id)
        {
            var conn = await OpenConnection();
            if (!conn.IsSuccessful)
            {
                return conn.Error;
            }
            var tableRef = conn.Value;

            try
            {
                var userEntity = new UserEntity(tenant, id);
                var deleteOperation = TableOperation.Delete(userEntity);

                await tableRef.ExecuteAsync(deleteOperation);

                return Unit.Value;
            }
            catch (StorageException e)
            {
                return Error.InternalError($"Azure Storage Failed to retrieve record.");
            }
        }

        private static async Task<Result<CloudTable>> OpenConnection()
        {
            if (!CloudStorageAccount.TryParse(
                "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;"
                , out CloudStorageAccount account))
            {
                {
                    return Error.InternalError("Azure Storage account connection is invalid.");
                }
            }

            var client = account.CreateCloudTableClient();

            var tableRef = client.GetTableReference("users");

            await tableRef.CreateIfNotExistsAsync();

            return tableRef;
        }
    }
}