using AutoMapper;
using Microsoft.Azure.Cosmos.Table;
using Serilog;
using System;
using System.Threading.Tasks;
using LanguageExt;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.IdentityAndAccess;
using VolleyM.Infrastructure.IdentityAndAccess.AzureStorage.TableConfiguration;
using Unit = VolleyM.Domain.Contracts.Unit;

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

        public Task<Result<User>> Add(User user)
        {
            return PerformStorageOperation<User>(async tableRef =>
            {
                var userEntity = new UserEntity(user);
                var createOperation = TableOperation.Insert(userEntity);

                var createResult = await tableRef.ExecuteAsync(createOperation);

                if (createResult.Result is UserEntity created)
                {
                    var newUser = _userFactory.CreateUser(_mapper.Map<UserEntity, UserFactoryDto>(created));

                    return newUser;
                }

                return Error.InternalError($"Azure Storage: Failed to create user with {createResult.HttpStatusCode} error.");
            }, "Add User");
        }

        public Task<Either<Error, User>> Add1(User user)
        {
            return PerformStorageOperation1(async tableRef =>
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

        public Task<Result<User>> Get(TenantId tenant, UserId id)
        {
            return PerformStorageOperation<User>(async tableRef =>
            {
                var getOperation = TableOperation.Retrieve<UserEntity>(tenant.ToString(), id.ToString());

                var result = await tableRef.ExecuteAsync(getOperation);

                if (result.Result is UserEntity userEntity)
                {
                    var userDto = _mapper.Map<UserEntity, UserFactoryDto>(userEntity);

                    return _userFactory.CreateUser(userDto);
                }

                return Error.NotFound();
            }, "Get User");
        }

        public Task<Either<Error, User>> Get1(TenantId tenant, UserId id)
        {
            return PerformStorageOperation1(async tableRef =>
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

        public Task<Result<Unit>> Delete(TenantId tenant, UserId id)
        {
            return PerformStorageOperation<Unit>(async tableRef =>
            {
                var userEntity = new UserEntity(tenant, id);

                userEntity.ETag = "*"; //Delete disregarding concurrency checks
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

        private async Task<Either<Error, T>> PerformStorageOperation1<T>(Func<CloudTable, Task<Either<Error, T>>> operation, string operationName)
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