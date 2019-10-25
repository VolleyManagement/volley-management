using AutoMapper;
using Microsoft.Azure.Cosmos.Table;
using Serilog;
using System;
using System.Threading.Tasks;
using LanguageExt;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.IdentityAndAccess;
using VolleyM.Infrastructure.IdentityAndAccess.AzureStorage.TableConfiguration;

namespace VolleyM.Infrastructure.IdentityAndAccess.AzureStorage
{
    public class AzureStorageUserRepository : IUserRepository
    {
        private readonly IdentityContextTableStorageOptions _options;
        private readonly UserFactory _userFactory;
        private readonly IMapper _mapper;

        public AzureStorageUserRepository(IdentityContextTableStorageOptions options, UserFactory userFactory, IMapper mapper)
        {
            _options = options;
            _userFactory = userFactory;
            _mapper = mapper;
        }

        public Task<Either<Error, User>> Add(User user)
        {
            return PerformStorageOperation(async tableRef =>
            {
                var userEntity = new UserEntity(user);
                var createOperation = TableOperation.Insert(userEntity);

                var createResult = (Either<Error, TableResult>)await tableRef.ExecuteAsync(createOperation);

                return createResult.Match(
                    tableResult => tableResult.Result switch
                    {
                        UserEntity created => (Either<Error, User>)_userFactory.CreateUser(
                            _mapper.Map<UserEntity, UserFactoryDto>(created)),
                        _ => Error.InternalError(
                            $"Azure Storage: Failed to create user with {tableResult.HttpStatusCode} error.")
                    },
                    e => e
                );
            }, "Add User");
        }

        public Task<Either<Error, User>> Get(TenantId tenant, UserId id)
        {
            return PerformStorageOperation(async tableRef =>
            {
                var getOperation = TableOperation.Retrieve<UserEntity>(tenant.ToString(), id.ToString());

                var getResult = (Either<Error, TableResult>)await tableRef.ExecuteAsync(getOperation);

                return getResult.Match(
                    tableResult => tableResult.Result switch
                    {
                        UserEntity userEntity => (Either<Error, User>)_userFactory.CreateUser(
                            _mapper.Map<UserEntity, UserFactoryDto>(userEntity)),
                        _ => Error.NotFound()
                    },
                    e => e
                );
            }, "Get User");
        }

        public Task<Either<Error, Unit>> Delete(TenantId tenant, UserId id)
        {
            return PerformStorageOperation<Unit>(async tableRef =>
            {
                var userEntity = new UserEntity(tenant, id);

                userEntity.ETag = "*"; //Delete disregarding concurrency checks
                var deleteOperation = TableOperation.Delete(userEntity);

                await tableRef.ExecuteAsync(deleteOperation);

                return Unit.Default;
            }, "Delete User");
        }

        private Either<Error, CloudTable> OpenConnection()
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

        private async Task<Either<Error, T>> PerformStorageOperation<T>(Func<CloudTable, Task<Either<Error, T>>> operation, string operationName)
        {
            var conn = OpenConnection();

            try
            {
                return await conn.Match(
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