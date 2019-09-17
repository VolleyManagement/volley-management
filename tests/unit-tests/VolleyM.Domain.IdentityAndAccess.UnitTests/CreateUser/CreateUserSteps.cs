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
        private readonly UserId _aUserId = new UserId("google|123321");
        private readonly TenantId _aTenantIdId = new TenantId("unit-tests");

        private readonly CreateUser.Request _request = new CreateUser.Request();

        private IRequestHandler<CreateUser.Request, Unit> _handler;

        private readonly IUserRepository _repositoryMock;

        public CreateUserSteps(IdentityAndAccessFixture fixture)
            : base(fixture)
        {
            _repositoryMock = Substitute.For<IUserRepository>();

            Register(() => _repositoryMock, Lifestyle.Scoped);
        }

        [Given("UserId provided")]
        public void GivenUserIdProvided()
        {
            _request.Id = _aUserId;
        }

        [And("Tenant provided")]
        public void AndTenantProvided()
        {
            _request.Tenant = _aTenantIdId;
        }

        [When("I execute CreateUser")]
        public async void WhenExecuteCommand()
        {
            _handler = Resolve<IRequestHandler<CreateUser.Request, Unit>>();

            await _handler.Handle(_request);
        }

        [Then("user is created")]
        public void ThenUserIsCreated()
        {
            _repositoryMock.Received()
                .Add(Arg.Is<User>(u => u.Id == _aUserId || u.Tenant == _aTenantIdId));
        }
    }
}
