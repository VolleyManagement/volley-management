using System;
using System.Collections.Generic;
using FluentAssertions;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.IdentityAndAccess.Handlers;
using VolleyM.Domain.IdentityAndAccess.UnitTests.Fixture;
using Xunit.Gherkin.Quick;

namespace VolleyM.Domain.IdentityAndAccess.UnitTests
{
    [FeatureFile(@"./CreateUser/CreateUser.feature")]
    public class CreateUserSteps : IdentityAndAccessStepsBase
    {
        private readonly UserId _aUserId = new UserId("google|123321");
        private readonly TenantId _aTenantId = new TenantId("auto-tests-tenant");

        private readonly IdentityAndAccessFixture _fixture;

        private readonly CreateUser.Request _request = new CreateUser.Request();
        private UserBuilder _expectedUser;

        private List<Tuple<TenantId, UserId>> _usersToTeardown = new List<Tuple<TenantId, UserId>>();

        private IRequestHandler<CreateUser.Request, Unit> _handler;

        private Result<Unit> _actualResult = Unit.Value;

        public CreateUserSteps(IdentityAndAccessFixture fixture)
            : base(fixture)
        {
            _fixture = fixture;

            _fixture.Setup();

            _expectedUser = UserBuilder.New();
        }

        ~CreateUserSteps()
        {
            _fixture.CleanUpUsers(_usersToTeardown);
        }

        [Given("UserId provided")]
        public void GivenUserIdProvided()
        {
            _request.Id = _aUserId;
            _expectedUser.WithId(_aUserId);
        }

        [And("Tenant provided")]
        public void AndTenantProvided()
        {
            _request.Tenant = _aTenantId;
            _expectedUser.WithTenant(_aTenantId);
        }

        [And("such user already exists")]
        public void AndUserExists()
        {
            _fixture.ConfigureUserExists(_aTenantId, _aUserId, _expectedUser.Build());
            _usersToTeardown.Add(Tuple.Create(_aTenantId, _aUserId));
        }

        [And("user does not exist")]
        public void AndDoesNotUserExist()
        {
            _fixture.ConfigureUserDoesNotExist(_aTenantId, _aUserId);
        }

        [When("I execute CreateUser")]
        public void WhenExecuteCommand()
        {
            _handler = Resolve<IRequestHandler<CreateUser.Request, Unit>>();

            _actualResult = _handler.Handle(_request).Result;
        }

        [Then("user is created")]
        public void ThenUserIsCreated()
        {
            var user = _expectedUser.Build();
            _fixture.VerifyUserCreated(user);
            _usersToTeardown.Add(Tuple.Create(user.Tenant, user.Id));
        }

        [Then("conflict error is returned")]
        public void ThenConflictErrorReturned()
        {
            Result<Unit> expected = Error.Conflict();

            _actualResult.Should().BeEquivalentTo(expected, "such user already exists");
        }
    }
}
