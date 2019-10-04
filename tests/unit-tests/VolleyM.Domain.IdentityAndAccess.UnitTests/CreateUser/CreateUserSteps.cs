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
        private readonly TenantId _aTenantIdId = new TenantId("auto-tests-tenant");

        private readonly IdentityAndAccessFixture _fixture;

        private readonly CreateUser.Request _request = new CreateUser.Request();
        private UserBuilder _expectedUser;

        private IRequestHandler<CreateUser.Request, Unit> _handler;

        private Result<Unit> _actualResult = Unit.Value;

        public CreateUserSteps(IdentityAndAccessFixture fixture)
            : base(fixture)
        {
            _fixture = fixture;

            _fixture.Initialize();

            _expectedUser = UserBuilder.New();
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
            _request.Tenant = _aTenantIdId;
            _expectedUser.WithTenant(_aTenantIdId);
        }

        [And("such user already exists")]
        public void AndUserExists()
        {
            _fixture.ConfigureUserExists(_aTenantIdId, _aUserId, _expectedUser.Build());
        }

        [And("user does not exist")]
        public void AndDoesNotUserExist()
        {
            _fixture.ConfigureUserDoesNotExist(_aTenantIdId, _aUserId);
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
            _fixture.VerifyUserCreated(_expectedUser.Build());
        }

        [Then("conflict error is returned")]
        public void ThenConflictErrorReturned()
        {
            Result<Unit> expected = Error.Conflict();

            _actualResult.Should().BeEquivalentTo(expected, "such user already exists");
        }
    }
}
