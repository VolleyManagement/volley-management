using System;
using System.Collections.Generic;
using FluentAssertions;
using TechTalk.SpecFlow;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.IdentityAndAccess.Handlers;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.IdentityAndAccess.UnitTests
{
    [Binding]
    [Scope(Feature = "Get User by ID")]
    public class GetUserSteps : IdentityAndAccessStepsBase
    {
        private readonly UserId _aUserId = new UserId("google|123321");
        private readonly TenantId _aTenantId = new TenantId("auto-tests-tenant");

        private GetUser.Request _request;
        private User _expectedUser;

        private List<Tuple<TenantId, UserId>> _usersToTeardown;

        private IRequestHandler<GetUser.Request, User> _handler;

        private Result<User> _actualResult;

        protected override void ScenarioSetup()
        {
            base.ScenarioSetup();

            MockTestUserPermission(new Permission(Permissions.Context, Permissions.User.GetUser));

            _request = new GetUser.Request();
            _usersToTeardown = new List<Tuple<TenantId, UserId>>();
        }

        protected override void ScenarioTearDown()
        {
            Fixture.CleanUpUsers(_usersToTeardown);
        }

        [Given("user exists")]
        public void GivenUserExists()
        {
            _expectedUser = new User(_aUserId, _aTenantId);
            _request.UserId = _aUserId;
            _request.Tenant = _aTenantId;
            Fixture.ConfigureUserExists(_aTenantId, _aUserId, _expectedUser);
            _usersToTeardown.Add(Tuple.Create(_aTenantId, _aUserId));
        }

        [Given("user does not exist")]
        public void GivenDoesNotUserExist()
        {
            _request.UserId = _aUserId;
            _request.Tenant = _aTenantId;
            Fixture.ConfigureUserDoesNotExist(_aTenantId, _aUserId);
        }

        [When("I get user")]
        public void WhenExecuteCommand()
        {
            _handler = Container.GetInstance<IRequestHandler<GetUser.Request, User>>();

            _actualResult = _handler.Handle(_request).Result;
        }

        [Then("user is returned")]
        public void ThenUserIsReturned()
        {
            _actualResult.Should().BeSuccessful("user exists");
            _actualResult.Value.Should().BeEquivalentTo(_expectedUser, "all user attributes should be returned");
        }

        [Then("NotFound error is returned")]
        public void ThenNotFoundErrorReturned()
        {
            AssertErrorReturned(_actualResult, Error.NotFound(), "such user should not exist");
        }
    }
}
