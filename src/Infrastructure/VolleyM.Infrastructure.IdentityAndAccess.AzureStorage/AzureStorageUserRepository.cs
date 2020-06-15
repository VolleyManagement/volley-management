using System;
using System.Threading.Tasks;
using AutoMapper;
using LanguageExt;
using Microsoft.Azure.Cosmos.Table;
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

		public EitherAsync<Error, User> Add(User user)
		{
			return PerformStorageOperation(_options.UsersTable,
				tableRef =>
				{
					var userEntity = new UserEntity(user);
					var createOperation = TableOperation.Insert(userEntity);

					var tryA=Prelude.TryAsync(tableRef.ExecuteAsync(createOperation));
					
					tryA.ToEither(err=>err.)

					var createResult = (EitherAsync<Error, TableResult>)tableRef.ExecuteAsync(createOperation);

					return createResult.Match(
						tableResult => tableResult.Result switch
						{
							UserEntity created => (Either<Error, User>)_userFactory.CreateUser(
								_mapper.Map<UserEntity, UserFactoryDto>(created)),
							_ => Error.InternalError(
								$"Azure Storage: Failed to create user with {tableResult.HttpStatusCode} error.")
						},
						MapError
					).ToAsync();
				}, "Add User");
		}

		private static Either<Error, User> MapError(Error e)
		{
			return  e;
		}

		public EitherAsync<Error, User> Get(TenantId tenant, UserId id)
		{
			return PerformStorageOperation(_options.UsersTable, 
				tableRef =>
				{
					var getOperation = TableOperation.Retrieve<UserEntity>(tenant.ToString(), id.ToString());

					var getResult = (EitherAsync<Error, TableResult>)tableRef.ExecuteAsync(getOperation);

					return getResult.Match(
						tableResult => tableResult.Result switch
						{
							UserEntity userEntity => (Either<Error, User>)_userFactory.CreateUser(
								_mapper.Map<UserEntity, UserFactoryDto>(userEntity)),
							_ => Error.NotFound()
						},
						e => e
					).ToAsync();
				}, "Get User");
		}

		public EitherAsync<Error, Unit> Delete(TenantId tenant, UserId id)
		{
			return PerformStorageOperation<Unit>(_options.UsersTable,
				tableRef =>
				{
					var userEntity = new UserEntity(tenant, id);

					userEntity.ETag = "*"; //Delete disregarding concurrency checks
					var deleteOperation = TableOperation.Delete(userEntity);

					EitherAsync<Error, TableResult> result = tableRef.ExecuteAsync(deleteOperation);

					return result.Map(tr => Unit.Default);
				}, "Delete User");
		}
	}
}