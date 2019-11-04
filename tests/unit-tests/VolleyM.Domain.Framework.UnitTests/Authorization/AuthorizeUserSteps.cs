using FluentAssertions;
using LanguageExt;
using NSubstitute;
using SimpleInjector;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;
using VolleyM.Domain.Framework.Authorization;
using VolleyM.Domain.IdentityAndAccess;
using VolleyM.Domain.IdentityAndAccess.Handlers;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;

using Constants = VolleyM.Domain.UnitTests.Framework.Constants;

namespace VolleyM.Domain.Framework.UnitTests.Authorization
{
    [Binding]
    [Scope(Feature = "Authorize User")]
    public class AuthorizeUserSteps
    {
        private readonly IDomainFrameworkTestFixture _testFixture;
        private readonly Container _container;

        private ClaimsIdentity _userClaims;

        private readonly UserId _predefinedAnonymousUserId = new UserId("anonym@volleym.idp");
        private readonly RoleId _visitorRole = new RoleId("visitor");

        private Either<Error, Unit> _actualResult;
        private CreateUser.Request _expectedRequest;

        public AuthorizeUserSteps(IDomainFrameworkTestFixture testFixture, Container container)
        {
            _testFixture = testFixture;
            _container = container;
        }

        [BeforeScenario(Order = Constants.BEFORE_SCENARIO_STEPS_ORDER)]
        public void ScenarioSetup()
        {
            _expectedRequest = new CreateUser.Request
            {
                Tenant = TenantId.Default,
                Role = _visitorRole
            };
        }

        #region Given

        [Given("new user is being authorized")]
        public void GivenNewUserIsBeingAuthorized()
        {
            _userClaims = CreateAuthenticatedIdentity();
            _testFixture.MockUserNotFound();
            _testFixture.MockCreateUserSuccess();
        }

        [Given("unauthenticated user is being authorized")]
        public void GivenNotAuthenticatedUserIsBeingAuthorized()
        {
            _userClaims = CreateNotAuthenticatedIdentity();
            _testFixture.MockUserNotFound();
            _testFixture.MockCreateUserSuccess();
        }

        [Given("existing user is being authorized")]
        public void GivenExistingUserIsBeingAuthorized()
        {
            _userClaims = CreateAuthenticatedIdentity();
            GivenUserHasIdClaim();
            _testFixture.MockUserExists(new User(_expectedRequest.UserId, _expectedRequest.Tenant));
        }

        [Given(@"user has '(\S+)' claim with '(\S+)' value")]
        public void GivenUserHasClaim(string claim, string value)
        {
            //Note: this might not work if you have several 'user has claim' statements in spec
            _expectedRequest.UserId = new UserId(value);

            _userClaims.AddClaim(new Claim(claim, value));
        }

        [Given(@"user has no claims")]
        public void GivenUserHasNoClaims()
        {
            var claims = _userClaims.Claims.ToList();

            claims.ForEach(_userClaims.RemoveClaim);
        }

        [Given(@"user has correct ID claim")]
        public void GivenUserHasIdClaim()
        {
            GivenUserHasClaim("sub", "user123");
        }

        [Given(@"get user operation has error")]
        public void GivenGetUserReturnsError()
        {
            _testFixture.MockGetUserError();
        }

        [Given(@"create user operation has error")]
        public void GivenCreateUserReturnsError()
        {
            _testFixture.MockCreateUserError();
        }

        #endregion

        #region When

        [When("I authorize user")]
        public async Task WhenIAuthorizeUser()
        {
            var userToAuthorize = new ClaimsPrincipal(_userClaims);

            var handler = _container.GetInstance<IAuthorizationHandler>();
            _actualResult = await handler.AuthorizeUser(userToAuthorize);
        }

        #endregion

        #region Then

        [Then("user should be authorized")]
        public void ThenUserShouldBeAuthorized()
        {
            _actualResult.IsRight.Should().BeTrue("user should be authorized");
        }

        [Then("user should not be authorized")]
        public void ThenUserShouldNotBeAuthorized()
        {
            _actualResult.IsLeft.Should().BeTrue("user should not be authorized");
        }

        [Then("this user is set into current context")]
        public void UserIsSetAsCurrent()
        {
            var currentUser = _container.GetInstance<ICurrentUserProvider>();

            currentUser.UserId.Should().Be(_expectedRequest.UserId, "user with this Id was logged in");
            currentUser.Tenant.Should().Be(_expectedRequest.Tenant, "current tenant should be set");
        }

        [Then("new user should be created in the system")]
        public void ThenNewUserIsCreated()
        {
            _testFixture.VerifyUserCreated(_expectedRequest);
        }

        [Then("new user should not be created in the system")]
        public void ThenNewUserIsNotCreated()
        {
            _testFixture.VerifyUserNotCreated();
        }

        [Then("anonymous visitor set as current user")]
        public void ThenAnonymousSetAsCurrentUser()
        {
            var currentUser = _container.GetInstance<ICurrentUserManager>();

            currentUser.Context.User.Id.Should().Be(_predefinedAnonymousUserId, "user was not authenticated");
            currentUser.Context.User.Role.Should().Be(_visitorRole, "anonymous user should have Visitor role assigned");
        }

        #endregion

        #region Creation methods

        private static ClaimsIdentity CreateAuthenticatedIdentity()
        {
            var baseIdentity = Substitute.For<IIdentity>();
            baseIdentity.IsAuthenticated.Returns(true);
            baseIdentity.AuthenticationType.Returns("AuthenticationTypes.Federation");
            return new ClaimsIdentity(baseIdentity);
        }


        private static ClaimsIdentity CreateNotAuthenticatedIdentity()
        {
            var baseIdentity = Substitute.For<IIdentity>();
            baseIdentity.IsAuthenticated.Returns(false);
            return new ClaimsIdentity(baseIdentity);
        }

        #endregion
    }
}
