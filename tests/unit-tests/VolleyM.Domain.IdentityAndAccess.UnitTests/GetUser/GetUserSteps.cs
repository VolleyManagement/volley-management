using FluentAssertions;
using System;
using System.Collections.Generic;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.IdentityAndAccess.Handlers;
using VolleyM.Domain.UnitTests.Framework;
using Xunit.Gherkin.Quick;

namespace VolleyM.Domain.IdentityAndAccess.UnitTests
{
    [FeatureFile(@"./GetUser/GetUser.feature")]
    public class GetUserSteps : IdentityAndAccessStepsBase
    {
        private readonly IdentityAndAccessFixture _fixture;

        private readonly UserId _aUserId = new UserId("google|123321");
        private readonly TenantId _aTenantId = new TenantId("auto-tests-tenant");

        private readonly GetUser.Request _request = new GetUser.Request();
        private User _expectedUser;

        private readonly List<Tuple<TenantId, UserId>> _usersToTeardown = new List<Tuple<TenantId, UserId>>();

        private IRequestHandler<GetUser.Request, User> _handler;

        private Result<User> _actualResult;

        public GetUserSteps(IdentityAndAccessFixture fixture)
            : base(fixture)
        {
            _fixture = fixture;

            _fixture.Setup();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _fixture.CleanUpUsers(_usersToTeardown);
            }
            base.Dispose(disposing);
        }

        [Given("user exists")]
        public void AndUserExists()
        {
            _expectedUser = new User(_aUserId, _aTenantId);
            _request.UserId = _aUserId;
            _request.Tenant = _aTenantId;
            _fixture.ConfigureUserExists(_aTenantId, _aUserId, _expectedUser);
            _usersToTeardown.Add(Tuple.Create(_aTenantId, _aUserId));
        }

        [Given("user does not exist")]
        public void GivenDoesNotUserExist()
        {
            _request.UserId = _aUserId;
            _request.Tenant = _aTenantId;
            _fixture.ConfigureUserDoesNotExist(_aTenantId, _aUserId);
        }

        [When("I get user")]
        public void WhenExecuteCommand()
        {
            _handler = Resolve<IRequestHandler<GetUser.Request, User>>();

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