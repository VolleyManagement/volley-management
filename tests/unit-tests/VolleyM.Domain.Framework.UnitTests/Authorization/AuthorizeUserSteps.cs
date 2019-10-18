using FluentAssertions;
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
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.Framework.UnitTests.Authorization
{
    [Binding]
    [Scope(Feature = "Authorize User")]
    public class AuthorizeUserSteps : DomainFrameworkStepsBase
    {
        private IRequestHandler<CreateUser.Request, User> _createHandler;
        private IRequestHandler<GetUser.Request, User> _getHandler;

        private UserId _expectedId;
        private TenantId _expectedTenant;
        private ClaimsIdentity _userClaims;

        private readonly UserId _predefinedAnonymousUserId = new UserId("anonym@volleym.idp");
        private readonly RoleId _visitorRole = new RoleId("visitor");

        private CreateUser.Request _actualRequest;
        private Result<Unit> _actualResult;

        public override void BeforeEachScenario()
        {
            base.BeforeEachScenario();

            _expectedTenant = TenantId.Default;

            _createHandler = Substitute.For<IRequestHandler<CreateUser.Request, User>>();
            Container.Register(() => _createHandler, Lifestyle.Scoped);

            _getHandler = Substitute.For<IRequestHandler<GetUser.Request, User>>();
            Container.Register(() => _getHandler, Lifestyle.Scoped);
        }

        #region Given

        [Given("new user is being authorized")]
        public void GivenNewUserIsBeingAuthorized()
        {
            _userClaims = CreateAuthenticatedIdentity();
            MockUserNotFound();
            MockCreateUserSuccess();
        }

        [Given("unauthenticated user is being authorized")]
        public void GivenNotAuthenticatedUserIsBeingAuthorized()
        {
            _userClaims = CreateNotAuthenticatedIdentity();
            MockUserNotFound();
            MockCreateUserSuccess();
        }

        [Given("existing user is being authorized")]
        public void GivenExistingUserIsBeingAuthorized()
        {
            _userClaims = CreateAuthenticatedIdentity();
            GivenUserHasIdClaim();
            MockExistingUser();
        }

        [Given(@"user has '(\S+)' claim with '(\S+)' value")]
        public void GivenUserHasClaim(string claim, string value)
        {
            //Note: this might not work if you have several 'user has claim' statements in spec
            _expectedId = new UserId(value);

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
            MockGetUserError();
        }

        [Given(@"create user operation has error")]
        public void GivenCreateUserReturnsError()
        {
            MockCreateUserError();
        }

        #endregion

        #region When

        [When("I authorize user")]
        public void WhenIAuthorizeUser()
        {
            var userToAuthorize = new ClaimsPrincipal(_userClaims);

            var handler = Container.GetInstance<IAuthorizationHandler>();
            _actualResult = handler.AuthorizeUser(userToAuthorize).Result;
        }

        #endregion

        #region Then

        [Then("user should be authorized")]
        public void ThenUserShouldBeAuthorized()
        {
            _actualResult.Should().BeSuccessful("user should be authorized");
        }

        [Then("user should not be authorized")]
        public void ThenUserShouldNotBeAuthorized()
        {
            _actualResult.Should().NotBeSuccessful("user should not be authorized");
        }

        [Then("this user is set into current context")]
        public void UserIsSetAsCurrent()
        {
            var currentUser = Container.GetInstance<ICurrentUserProvider>();

            currentUser.UserId.Should().Be(_expectedId, "user with this Id was logged in");
            currentUser.Tenant.Should().Be(_expectedTenant, "current tenant should be set");
        }

        [Then("new user should be created in the system")]
        public void ThenNewUserIsCreated()
        {
            _actualRequest.UserId.Should().Be(_expectedId, "this value was in ID claim");
            _actualRequest.Tenant.Should().Be(_expectedTenant, "this is default tenant");
            _actualRequest.Role.Should().Be(_visitorRole, "all new users are assigned visitor role");
        }

        [Then("new user should not be created in the system")]
        public void ThenNewUserIsNotCreated()
        {
            _createHandler.DidNotReceive().Handle(Arg.Any<CreateUser.Request>());
        }

        [Then("anonymous visitor set as current user")]
        public void ThenAnonymousSetAsCurrentUser()
        {
            var currentUser = Container.GetInstance<ICurrentUserManager>();

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

        #region Mock

        private void MockCreateUserSuccess()
        {
            _createHandler.Handle(Arg.Any<CreateUser.Request>())
                .Returns(ci => Task.FromResult(
                    (Result<User>)new User(
                        ci.Arg<CreateUser.Request>().UserId,
                        ci.Arg<CreateUser.Request>().Tenant)))
                .AndDoes(ci => { _actualRequest = ci.Arg<CreateUser.Request>(); });
            SetupPermissionAttribute(typeof(CreateUser.Request),
                new PermissionAttribute("IdentityAndAccess", "CreateUser"));
        }

        public void MockCreateUserError()
        {
            MockCreateUser(Error.InternalError("random test error"));
        }

        private void MockCreateUser(Result<User> result)
        {
            _createHandler.Handle(Arg.Any<CreateUser.Request>())
                .Returns(result);
            SetupPermissionAttribute(typeof(CreateUser.Request),
                new PermissionAttribute("IdentityAndAccess", "CreateUser"));
        }
        private void MockExistingUser()
        {
            MockGetUser(new User(_expectedId, _expectedTenant));
        }

        private void MockUserNotFound()
        {
            MockGetUser(Error.NotFound());
        }

        private void MockGetUserError()
        {
            MockGetUser(Error.InternalError("any test error"));
        }

        private void MockGetUser(Result<User> result)
        {
            _getHandler.Handle(Arg.Any<GetUser.Request>())
                .Returns(result);
            SetupPermissionAttribute(typeof(GetUser.Request), 
                new PermissionAttribute("IdentityAndAccess", "GetUser"));
        }

        #endregion
    }
}
