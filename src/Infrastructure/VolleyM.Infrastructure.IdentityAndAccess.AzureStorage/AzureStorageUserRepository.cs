using AutoMapper;
using LanguageExt;
using Microsoft.Azure.Cosmos.Table;
using System.Threading.Tasks;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.IdentityAndAccess;
using VolleyM.Infrastructure.AzureStorage;
using VolleyM.Infrastructure.IdentityAndAccess.AzureStorage.TableConfiguration;

namespace VolleyM.Infrastructure.IdentityAndAccess.AzureStorage
{
    public class AzureStorageUserRepository : AzureTableConnection, IUserRepository
    {
        private readonly IdentityContextTableStorageOptions _options;
        private readonly UserFactory _userFactory;
        private readonly IMapper _mapper;

        public AzureStorageUserRepository(IdentityContextTableStorageOptions options, UserFactory userFactory, IMapper mapper)
            : base(options)
        {
            _options = options;
            _userFactory = userFactory;
            _mapper = mapper;
        }

        public Task<Either<Error, User>> Add(User user)
        {
            return PerformStorageOperation(_options.UsersTable,
                async tableRef =>
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
            return PerformStorageOperation(_options.UsersTable, 
                async tableRef =>
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
            return PerformStorageOperation<Unit>(_options.UsersTable,
                async tableRef =>
            {
                var userEntity = new UserEntity(tenant, id);

                userEntity.ETag = "*"; //Delete disregarding concurrency checks
                var deleteOperation = TableOperation.Delete(userEntity);

                await tableRef.ExecuteAsync(deleteOperation);

                return Unit.Default;
            }, "Delete User");
        }
    }
}