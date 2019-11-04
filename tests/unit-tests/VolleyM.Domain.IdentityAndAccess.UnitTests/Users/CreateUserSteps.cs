using FluentAssertions;
using SimpleInjector;
using System.Threading.Tasks;
using LanguageExt;
using TechTalk.SpecFlow;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.IdentityAndAccess.Handlers;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;
using VolleyM.Domain.IdentityAndAccess.UnitTests.Fixture;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.IdentityAndAccess.UnitTests.Users
{
    [Binding]
    [Scope(Feature = "Create User")]
    public class CreateUserSteps
    {
        private readonly IIdentityAndAccessFixture _testFixture;
        private readonly IAuthFixture _authFixture;
        private readonly Container _container;

        private readonly TenantId _aTenantId = new TenantId("auto-tests-tenant");
        private readonly RoleId _aRoleId = new RoleId("roleA");

        private CreateUser.Request _request;
        private UserBuilder _expectedUser;

        private IRequestHandler<CreateUser.Request, User> _handler;

        private Either<Error, User> _actualResult;

        public CreateUserSteps(IIdentityAndAccessFixture testFixture, IAuthFixture authFixture, Container container)
        {
            _testFixture = testFixture;
            _authFixture = authFixture;
            _container = container;
        }

        [BeforeScenario(Order = Constants.BEFORE_SCENARIO_STEPS_ORDER)]
        public void ScenarioSetup()
        {
            _authFixture.SetTestUserPermission(IdentityAndAccessConstants.Context, nameof(CreateUser));

            _expectedUser = UserBuilder.New();
            _request = new CreateUser.Request();
        }

        [Given("UserId provided")]
        public void GivenUserIdProvided()
        {
            _expectedUser
                .WithAnyId();

            _request.UserId = _expectedUser.Id;
        }

        [Given("Tenant provided")]
        public void GivenTenantProvided()
        {
            _expectedUser.WithTenant(_aTenantId);
            _request.Tenant = _aTenantId;
        }

        [Given("Role provided")]
        public void GivenRoleProvided()
        {
            _expectedUser.WithRole(_aRoleId);
            _request.Role = _aRoleId;
        }

        [Given("such user already exists")]
        public async Task GivenUserExists()
        {
            await _testFixture.ConfigureUserExists(_aTenantId, _expectedUser.Id, _expectedUser.Build());
        }

        [Given("user does not exist")]
        public async Task GivenDoesNotUserExist()
        {
            await _testFixture.ConfigureUserDoesNotExist(_aTenantId, _expectedUser.Id);
        }

        [When("I execute CreateUser")]
        public async Task WhenExecuteCommand()
        {
            _handler = _container.GetInstance<IRequestHandler<CreateUser.Request, User>>();

            _actualResult = await _handler.Handle(_request);
        }

        [Then("user is created")]
        public async Task ThenUserIsCreated()
        {
            var user = _expectedUser.Build();
            await _testFixture.VerifyUserCreated(user);
        }

        [Then("Conflict error is returned")]
        public void ThenConflictErrorReturned()
        {
            _actualResult.ShouldBeError(Error.Conflict(), "such user already exists");
        }

        [Then("user is returned")]
        public void ThenUserIsReturned()
        {
            _actualResult.IsRight.Should().BeTrue("created user should be returned");
            _actualResult.IfRight(u => u.Should()
                .BeEquivalentTo(_expectedUser.Build(), "created user should be returned"));
        }
    }
}
