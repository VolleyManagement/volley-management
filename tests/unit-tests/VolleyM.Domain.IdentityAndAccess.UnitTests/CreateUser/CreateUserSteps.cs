using System;
using FluentAssertions;
using NSubstitute;
using SimpleInjector;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.IdentityAndAccess.Handlers;
using Xunit.Gherkin.Quick;

namespace VolleyM.Domain.IdentityAndAccess.UnitTests
{
    [FeatureFile(@"./CreateUser/CreateUser.feature")]
    public class CreateUserSteps : IdentityAndAccessStepsBase
    {
        private readonly UserId aUserId = new UserId("google|123321");
        private readonly TenantId aTenantIdId = new TenantId("unit-tests");

        private CreateUser.Request _request = new CreateUser.Request();

        private IRequestHandler<CreateUser.Request, Unit> _handler;

        private IUserRepository _repositoryMock;

        private Result<Unit> _actualResult = Unit.Value;

        public CreateUserSteps(IdentityAndAccessFixture fixture)
            : base(fixture)
        {
            _repositoryMock = Substitute.For<IUserRepository>();

            Register(() => _repositoryMock, Lifestyle.Scoped);
        }

        [Given("UserId provided")]
        public void GivenUserIdProvided()
        {
            _request.Id = aUserId;
        }

        [And("Tenant provided")]
        public void AndTenantProvided()
        {
            _request.Tenant = aTenantIdId;
        }

        [And("such user already exists")]
        public void AndUserExists()
        {
            _repositoryMock.Get(aUserId).Returns(new User(aUserId, aTenantIdId));
        }

        [And("user does not exist")]
        public void AndDoesNotUserExist()
        {
            _repositoryMock.Get(aUserId).Returns(Error.NotFound());
        }

        [When("I execute CreateUser")]
        public async void WhenExecuteCommand()
        {
            _handler = Resolve<IRequestHandler<CreateUser.Request, Unit>>();

            _actualResult = await _handler.Handle(_request);
        }

        [Then("user is created")]
        public void ThenUserIsCreated()
        {
            _repositoryMock.Received()
                .Add(Arg.Is<User>(u => u.Id == aUserId || u.Tenant == aTenantIdId));
        }

        [Then("conflict error is returned")]
        public void ThenConflictErrorReturned()
        {
            Result<Unit> expected = Error.Conflict();

            _actualResult.Should().BeEquivalentTo(expected, "such user already exists");
        }
    }
}
