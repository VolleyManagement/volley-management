using FluentAssertions;
using NSubstitute;
using SimpleInjector;
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
        private IRequestHandler<CreateUser.Request, Unit> _handler;

        private UserId _expectedId;
        private ClaimsIdentity _userClaims;

        private CreateUser.Request _actualRequest;
        private Result<Unit> _actualResult;

        public AuthorizeUserSteps(DomainFrameworkFixture fixture)
            : base(fixture)
        {
            _handler = Substitute.For<IRequestHandler<CreateUser.Request, Unit>>();
            Register(() => _handler, Lifestyle.Scoped);
        }

        [Given("new user is being authorized")]
        public void GivenNewUserIsBeingAuthorized()
        {
            _userClaims = new ClaimsIdentity();
            MockCreateUserSuccess();
        }

        [Given("existing user is being authorized")]
        public void GivenExistingUserIsBeingAuthorized()
        {
            _userClaims = new ClaimsIdentity();
            _userClaims.AddClaim(new Claim("sub", "user123"));
            MockCreateUserConflict();
        }

        [And(@"user has '(\S+)' claim with '(\S+)' value")]
        public void AndUserHasClaim(string claim, string value)
        {
            //Note: this might not work if you have several 'user has claim' statements in spec
            _expectedId = new UserId(value);

            _userClaims.AddClaim(new Claim(claim, value));
        }

        [When("I authorize user")]
        public void WhenIAuthorizeUser()
        {
            var userToAuthorize = new ClaimsPrincipal(_userClaims);

            var handler = Resolve<IAuthorizationHandler>();
            _actualResult = handler.AuthorizeUser(userToAuthorize).Result;
        }

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

        [And("new user should be created in the system")]
        public void AndNewUserIsCreated()
        {
            var expectedUserRequest = new CreateUser.Request { Tenant = TenantId.Default, UserId = _expectedId };
            _actualRequest.Should().BeEquivalentTo(expectedUserRequest, "all user attributes should be extracted");
        }

        [And("new user should not be created in the system")]
        public void AndNewUserIsNotCreated()
        {
            _handler.DidNotReceive().Handle(Arg.Any<CreateUser.Request>());
        }

        private void MockCreateUserSuccess()
        {
            _handler.Handle(Arg.Any<CreateUser.Request>())
                .Returns(Unit.Value)
                .AndDoes(ci => { _actualRequest = ci.Arg<CreateUser.Request>(); });
        }

        private void MockCreateUserConflict()
        {
            _handler.Handle(Arg.Any<CreateUser.Request>())
                .Returns(Error.Conflict());
        }
    }
}