using FluentAssertions;
using SimpleInjector;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.IdentityAndAccess.Handlers;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;
using VolleyM.Domain.IdentityAndAccess.UnitTests.Fixture;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.IdentityAndAccess.UnitTests
{
    [Binding]
    [Scope(Feature = "Get User by ID")]
    public class GetUserSteps
    {
        private readonly IIdentityAndAccessFixture _testFixture;
        private readonly IAuthFixture _authFixture;
        private readonly Container _container;

        private readonly TenantId _aTenantId = new TenantId("auto-tests-tenant");

        private UserBuilder _userBuilder;

        private GetUser.Request _request;
        private User _expectedUser;

        private IRequestHandler<GetUser.Request, User> _handler;

        private Result<User> _actualResult;

        public GetUserSteps(IIdentityAndAccessFixture testFixture, IAuthFixture authFixture, Container container)
        {
            _testFixture = testFixture;
            _authFixture = authFixture;
            _container = container;
        }

        [BeforeScenario(Order = Constants.BEFORE_SCENARIO_STEPS_ORDER)]
        public void ScenarioSetup()
        {
            _authFixture.SetTestUserPermission(new Permission(Permissions.Context, Permissions.User.GetUser));

            _request = new GetUser.Request();
            _userBuilder = UserBuilder.New();
        }

        [Given("user exists")]
        public async Task GivenUserExists()
        {
            _userBuilder
                .WithAnyId()
                .WithTenant(_aTenantId);

            _expectedUser = _userBuilder.Build();

            _request.UserId = _userBuilder.Id;
            _request.Tenant = _aTenantId;
            await _testFixture.ConfigureUserExists(_aTenantId, _userBuilder.Id, _expectedUser);
        }

        [Given("user does not exist")]
        public async Task GivenDoesNotUserExist()
        {
            _userBuilder
                .WithAnyId()
                .WithTenant(_aTenantId);

            _request.UserId = _userBuilder.Id;
            _request.Tenant = _aTenantId;
            await _testFixture.ConfigureUserDoesNotExist(_aTenantId, _userBuilder.Id);
        }

        [When("I get user")]
        public async Task WhenExecuteCommand()
        {
            _handler = _container.GetInstance<IRequestHandler<GetUser.Request, User>>();

            _actualResult = await _handler.Handle(_request);
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
            _actualResult.ShouldBeError(Error.NotFound(), "such user should not exist");
        }
    }
}
