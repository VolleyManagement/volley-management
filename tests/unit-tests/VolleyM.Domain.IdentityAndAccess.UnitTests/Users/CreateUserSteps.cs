using System;
using System.Collections.Generic;
using FluentAssertions;
using TechTalk.SpecFlow;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.IdentityAndAccess.Handlers;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;
using VolleyM.Domain.IdentityAndAccess.UnitTests.Fixture;

namespace VolleyM.Domain.IdentityAndAccess.UnitTests.Users
{
    [Binding]
    [Scope(Feature = "Create User")]
    public class CreateUserSteps : IdentityAndAccessStepsBase
    {
        private readonly UserId _aUserId = new UserId("google|123321");
        private readonly TenantId _aTenantId = new TenantId("auto-tests-tenant");
        private readonly RoleId _aRoleId = new RoleId("roleA");

        private CreateUser.Request _request;
        private UserBuilder _expectedUser;

        //ToDo: move to Azure fixture
        private List<Tuple<TenantId, UserId>> _usersToTeardown;

        private IRequestHandler<CreateUser.Request, User> _handler;

        private Result<User> _actualResult;

        protected override void ScenarioSetup()
        {
            base.ScenarioSetup();

            MockTestUserPermission(new Permission(Permissions.Context, Permissions.User.CreateUser));

            _expectedUser = UserBuilder.New();
            _request = new CreateUser.Request();

            _usersToTeardown = new List<Tuple<TenantId, UserId>>();
        }

        protected override void ScenarioTearDown()
        {
            Fixture.CleanUpUsers(_usersToTeardown);

            base.ScenarioTearDown();
        }

        [Given("UserId provided")]
        public void GivenUserIdProvided()
        {
            _request.UserId = _aUserId;
            _expectedUser.WithId(_aUserId);
        }

        [Given("Tenant provided")]
        public void GivenTenantProvided()
        {
            _request.Tenant = _aTenantId;
            _expectedUser.WithTenant(_aTenantId);
        }

        [Given("Role provided")]
        public void GivenRoleProvided()
        {
            _request.Role = _aRoleId;
            _expectedUser.WithRole(_aRoleId);
        }

        [Given("such user already exists")]
        public void GivenUserExists()
        {
            Fixture.ConfigureUserExists(_aTenantId, _aUserId, _expectedUser.Build());
            _usersToTeardown.Add(Tuple.Create(_aTenantId, _aUserId));
        }

        [Given("user does not exist")]
        public void GivenDoesNotUserExist()
        {
            Fixture.ConfigureUserDoesNotExist(_aTenantId, _aUserId);
        }

        [When("I execute CreateUser")]
        public void WhenExecuteCommand()
        {
            _handler = Container.GetInstance<IRequestHandler<CreateUser.Request, User>>();

            _actualResult = _handler.Handle(_request).Result;
        }

        [Then("user is created")]
        public void ThenUserIsCreated()
        {
            var user = _expectedUser.Build();
            Fixture.VerifyUserCreated(user);
            _usersToTeardown.Add(Tuple.Create(user.Tenant, user.Id));
        }

        [Then("Conflict error is returned")]
        public void ThenConflictErrorReturned()
        {
            AssertErrorReturned(_actualResult, Error.Conflict(), "such user already exists");
        }

        [Then("user is returned")]
        public void ThenUserIsReturned()
        {
            _actualResult.Value.Should()
                .BeEquivalentTo(_expectedUser.Build(), "created user should be returned");
        }
    }
}
