using System.Threading.Tasks;
using FluentAssertions;
using LanguageExt;
using NSubstitute;
using NSubstitute.Core;
using SimpleInjector;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;
using VolleyM.Domain.Framework.Authorization;
using VolleyM.Domain.Framework.HandlerMetadata;
using VolleyM.Domain.IdentityAndAccess;
using VolleyM.Domain.IdentityAndAccess.Handlers;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;

namespace VolleyM.Domain.Framework.UnitTests
{
	public class UnitDomainFrameworkTestFixture : IDomainFrameworkTestFixture
	{
		private readonly Container _container;

		private IRequestHandlerOld<CreateUserOld.Request, User> _createHandler;
		private IRequestHandlerOld<GetUserOld.Request, User> _getHandler;
		private IRolesStore _rolesStore;
		private IApplicationInfo _applicationInfo;

		private CreateUserOld.Request _actualCreateRequest;

		public UnitDomainFrameworkTestFixture(Container container)
		{
			_container = container;
		}

		public void RegisterScenarioDependencies(Container container)
		{
			_createHandler = Substitute.For<IRequestHandlerOld<CreateUserOld.Request, User>>();
			container.Register(() => _createHandler, Lifestyle.Scoped);

			_getHandler = Substitute.For<IRequestHandlerOld<GetUserOld.Request, User>>();
			container.Register(() => _getHandler, Lifestyle.Scoped);

			_rolesStore = Substitute.For<IRolesStore>();
			container.Register(() => _rolesStore, Lifestyle.Scoped);

			_applicationInfo = Substitute.For<IApplicationInfo>();
			_applicationInfo.IsRunningInProduction.Returns(true);
			container.Register(() => _applicationInfo, Lifestyle.Singleton);
		}

		public Task ScenarioSetup()
		{
			return Task.CompletedTask;
		}

		public Task ScenarioTearDown()
		{
			return Task.CompletedTask;
		}

		public void SetCurrentUser(User currentUser)
		{
			var currentUserMgr = _container.GetInstance<ICurrentUserManager>();
			currentUserMgr.Context = new CurrentUserContext
			{
				User = currentUser
			};
		}

		public User CreateAUser()
		{
			return new User(new UserId("user|abc"), TenantId.Default);
		}


		public void VerifyUserNotCreated()
		{
			_createHandler.DidNotReceive().Handle(Arg.Any<CreateUserOld.Request>());
		}

		public void VerifyUserCreated(CreateUserOld.Request expectedRequest)
		{
			_actualCreateRequest.Should().BeEquivalentTo(expectedRequest,
				"user should be created with ID claim, with default tenant and assigned visitor role");
		}

		public void MockCreateUserSuccess()
		{
			User BuildUser(CallInfo ci)
			{
				var req = ci.Arg<CreateUserOld.Request>();
				var result = new User(
					req.UserId,
					req.Tenant);

				if (req.Role != null)
				{
					result.AssignRole(req.Role);
				}

				return result;
			}

			_createHandler.Handle(Arg.Any<CreateUserOld.Request>())
				.Returns(ci => Task.FromResult(
					(Either<Error, User>)BuildUser(ci)))
				.AndDoes(ci => { _actualCreateRequest = ci.Arg<CreateUserOld.Request>(); });

			OverrideHandlerMetadata<CreateUserOld.Request>(
				new HandlerInfo("IdentityAndAccess", "CreateUserOld"));
		}

		public void MockCreateUserError()
		{
			MockCreateUser(Error.InternalError("random test error"));
		}

		private void MockCreateUser(Either<Error, User> result)
		{
			_createHandler.Handle(Arg.Any<CreateUserOld.Request>())
				.Returns(result);
			OverrideHandlerMetadata<CreateUserOld.Request>(
				new HandlerInfo("IdentityAndAccess", "CreateUserOld"));
		}
		public void MockUserExists(User user)
		{
			MockGetUser(user);
		}

		public void MockUserNotFound()
		{
			MockGetUser(Error.NotFound());
		}

		public void MockGetUserError()
		{
			MockGetUser(Error.InternalError("any test error"));
		}

		private void MockGetUser(Either<Error, User> result)
		{
			_getHandler.Handle(Arg.Any<GetUserOld.Request>())
				.Returns(result);
			OverrideHandlerMetadata<GetUserOld.Request>(
				new HandlerInfo("IdentityAndAccess", "GetUserOld"));
		}

		public void MockRoleStoreError()
		{
			_rolesStore.Get(Arg.Any<RoleId>()).Returns(Error.InternalError("random test error"));
		}

		public void SetupRole(Role role)
		{
			_rolesStore.Get(role.Id).Returns(role);
		}

		public void MockHostingEnvironmentIsProduction(bool isProduction)
		{
			_applicationInfo.IsRunningInProduction.Returns(isProduction);
		}

		/// <summary>
		/// Mocked handlers does not work with Authorization decorator
		/// This methods mocks internal cache to avoid metadata resolution based on required structure
		/// </summary>
		private void OverrideHandlerMetadata<T>(HandlerInfo info)
		{
			var metadataService = _container.GetInstance<HandlerMetadataService>();
			metadataService.OverrideHandlerMetadata<T>(info);
		}
	}
}