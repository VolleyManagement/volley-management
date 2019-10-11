using FluentAssertions;
using NSubstitute;
using SimpleInjector;
using System.Linq;
using System.Security.Claims;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;
using VolleyM.Domain.IdentityAndAccess;
using VolleyM.Domain.IdentityAndAccess.Handlers;
using VolleyM.Domain.UnitTests.Framework;
using Xunit.Gherkin.Quick;

namespace VolleyM.Domain.Framework.UnitTests.AuthorizationHandler
{
    [FeatureFile(@"./AuthorizationHandler/AuthorizeUser.feature")]
    public class AuthorizeUserSteps : DomainFrameworkStepsBase
    {
        private IRequestHandler<CreateUser.Request, Unit> _createHandler;
        private IRequestHandler<GetUser.Request, User> _getHandler;

        private UserId _expectedId;
        private TenantId _expectedTenant = TenantId.Default;
        private ClaimsIdentity _userClaims;

        private CreateUser.Request _actualRequest;
        private Result<Unit> _actualResult;

        public AuthorizeUserSteps(DomainFrameworkFixture fixture)
            : base(fixture)
        {
            _createHandler = Substitute.For<IRequestHandler<CreateUser.Request, Unit>>();
            Register(() => _createHandler, Lifestyle.Scoped);

            _getHandler = Substitute.For<IRequestHandler<GetUser.Request, User>>();
            Register(() => _getHandler, Lifestyle.Scoped);
        }

        #region Given

        [Given("new user is being authorized")]
        public void GivenNewUserIsBeingAuthorized()
        {
            _userClaims = new ClaimsIdentity();
            MockUserNotFound();
            MockCreateUserSuccess();
        }

        [Given("existing user is being authorized")]
        public void GivenExistingUserIsBeingAuthorized()
        {
            _userClaims = new ClaimsIdentity();
            AndUserHasIdClaim();
            MockExistingUser();
        }

        [And(@"user has '(\S+)' claim with '(\S+)' value")]
        public void AndUserHasClaim(string claim, string value)
        {
            //Note: this might not work if you have several 'user has claim' statements in spec
            _expectedId = new UserId(value);

            _userClaims.AddClaim(new Claim(claim, value));
        }

        [And(@"user has no claims")]
        public void AndUserHasNoClaims()
        {
            var claims = _userClaims.Claims.ToList();

            claims.ForEach(_userClaims.RemoveClaim);
        }

        [And(@"user has correct ID claim")]
        public void AndUserHasIdClaim()
        {
            AndUserHasClaim("sub", "user123");
        }

        [And(@"get user operation has error")]
        public void AndGetUserReturnsError()
        {
            MockGetUserError();
        }

        [And(@"create user operation has error")]
        public void AndCreateUserReturnsError()
        {
            MockCreateUserError();
        }

        #endregion

        #region When

        [When("I authorize user")]
        public void WhenIAuthorizeUser()
        {
            var userToAuthorize = new ClaimsPrincipal(_userClaims);

            var handler = Resolve<IAuthorizationHandler>();
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
            var currentUser = Resolve<ICurrentUserProvider>();

            currentUser.User.Should().Be(_expectedId, "user with this Id was logged in");
            currentUser.Tenant.Should().Be(_expectedTenant, "current tenant should be set");
        }

        [And("new user should be created in the system")]
        public void AndNewUserIsCreated()
        {
            var expectedUserRequest = new CreateUser.Request { Tenant = _expectedTenant, UserId = _expectedId };
            _actualRequest.Should().BeEquivalentTo(expectedUserRequest, "all user attributes should be extracted");
        }

        [And("new user should not be created in the system")]
        public void AndNewUserIsNotCreated()
        {
            _createHandler.DidNotReceive().Handle(Arg.Any<CreateUser.Request>());
        }

        #endregion

        #region Mock

        private void MockCreateUserSuccess()
        {
            _createHandler.Handle(Arg.Any<CreateUser.Request>())
                .Returns(Unit.Value)
                .AndDoes(ci => { _actualRequest = ci.Arg<CreateUser.Request>(); });
        }

        public void MockCreateUserError()
        {
            MockCreateUser(Error.InternalError("random test error"));
        }

        private void MockCreateUser(Result<Unit> result)
        {
            _createHandler.Handle(Arg.Any<CreateUser.Request>())
                .Returns(result);
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
        }

        #endregion
    }
}