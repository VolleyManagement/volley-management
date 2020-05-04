using System;
using System.Threading.Tasks;
using FluentAssertions;
using LanguageExt;
using NSubstitute;
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

		private IRequestHandler<CreateUser.Request, User> _createHandler;
		private IRequestHandler<GetUser.Request, User> _getHandler;
		private IRolesStore _rolesStore;
		private IApplicationInfo _applicationInfo;

		private CreateUser.Request _actualCreateRequest;

		public UnitDomainFrameworkTestFixture(Container container)
		{
			_container = container;
		}

		public void RegisterScenarioDependencies(Container container)
		{
			_createHandler = Substitute.For<IRequestHandler<CreateUser.Request, User>>();
			container.Register(() => _createHandler, Lifestyle.Scoped);

			_getHandler = Substitute.For<IRequestHandler<GetUser.Request, User>>();
			container.Register(() => _getHandler, Lifestyle.Scoped);

			_rolesStore = Substitute.For<IRolesStore>();
			container.Register(() => _rolesStore, Lifestyle.Scoped);

			_applicationInfo = Substitute.For<IApplicationInfo>();
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
			_createHandler.DidNotReceive().Handle(Arg.Any<CreateUser.Request>());
		}

		public void VerifyUserCreated(CreateUser.Request expectedRequest)
		{
			_actualCreateRequest.Should().BeEquivalentTo(expectedRequest,
				"user should be created with ID claim, with default tenant and assigned visitor role");
		}

		public void MockCreateUserSuccess()
		{
			_createHandler.Handle(Arg.Any<CreateUser.Request>())
				.Returns(ci => Task.FromResult(
					(Either<Error, User>)new User(
						ci.Arg<CreateUser.Request>().UserId,
						ci.Arg<CreateUser.Request>().Tenant)))
				.AndDoes(ci => { _actualCreateRequest = ci.Arg<CreateUser.Request>(); });

			OverrideHandlerMetadata<CreateUser.Request>(
				new HandlerInfo("IdentityAndAccess", "CreateUser"));
		}

		public void MockCreateUserError()
		{
			MockCreateUser(Error.InternalError("random test error"));
		}

		private void MockCreateUser(Either<Error, User> result)
		{
			_createHandler.Handle(Arg.Any<CreateUser.Request>())
				.Returns(result);
			OverrideHandlerMetadata<CreateUser.Request>(
				new HandlerInfo("IdentityAndAccess", "CreateUser"));
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
			_getHandler.Handle(Arg.Any<GetUser.Request>())
				.Returns(result);
			OverrideHandlerMetadata<GetUser.Request>(
				new HandlerInfo("IdentityAndAccess", "GetUser"));
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