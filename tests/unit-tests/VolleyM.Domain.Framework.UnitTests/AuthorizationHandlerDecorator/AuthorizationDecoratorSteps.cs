using FluentAssertions;
using LanguageExt;
using NSubstitute;
using SimpleInjector;
using System.Reflection;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;
using VolleyM.Domain.Framework.Authorization;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;
using VolleyM.Domain.IDomainFrameworkTestFixture;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.Framework.UnitTests.AuthorizationHandlerDecorator
{
    [Binding]
    [Scope(Feature = "Authorization Decorator")]
    public class AuthorizationDecoratorSteps
    {
        private Either<Error, Unit> _actualResult;
        private IAuthorizationService _authorizationService;

        private readonly Container _container;

        public AuthorizationDecoratorSteps(Container container)
        {
            _container = container;
        }

        [BeforeScenario(Order = Constants.BEFORE_SCENARIO_REGISTER_DEPENDENCIES_ORDER)]
        public void RegisterDependenciesForScenario()
        {
            RegisterHandlers();

            _authorizationService = Substitute.For<IAuthorizationService>();
            _container.RegisterInstance(_authorizationService);
        }

        [Given(@"current user has permission to execute this handler")]
        public void GivenUserHasPermissionToCallHandler()
        {
            _authorizationService
                .CheckAccess(new Permission(nameof(IDomainFrameworkTestFixture), nameof(SampleHandler)))
                .Returns(true);
        }

        [Given(@"current user has no permission to execute this handler")]
        public void GivenUserHasNoPermissionToCallHandler()
        {
            _authorizationService.CheckAccess(Arg.Any<Permission>()).Returns(false);
        }

        [When(@"I call decorated handler")]
        public async Task WhenICallDecoratedHandler()
        {
            var handler = _container.GetInstance<IRequestHandler<SampleHandler.Request, Unit>>();

            _actualResult = await handler.Handle(new SampleHandler.Request());
        }

        [Then(@"NotAuthorized error should be returned with message ([A-Za-z ]+)")]
        public void ThenNotAuthorizedErrorShouldBeReturned(string errorMessage)
        {
            _actualResult.ShouldBeError(Error.NotAuthorized(errorMessage));
        }

        [Then(@"success result is returned")]
        public void ThenReturnsSuccess()
        {
            _actualResult.IsRight.Should().BeTrue("user has required permission");
        }

        private void RegisterHandlers()
        {
            _container.RegisterCommonDomainServices(Assembly.GetAssembly(GetType()));
        }
    }
}
