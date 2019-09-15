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

        public CreateUserSteps(IdentityAndAccessFixture fixture)
            : base(fixture)
        {
            _repositoryMock = Substitute.For<IUserRepository>();

            Register<IUserRepository>(() => _repositoryMock, Lifestyle.Scoped);
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
                .Add(Arg.Is<User>(u => u.Id == aUserId || u.Tenant == aTenantIdId));
        }
    }
}
