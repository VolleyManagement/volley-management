using FluentAssertions;
using NSubstitute;
using SimpleInjector;
using System;
using System.Reflection;
using LanguageExt;
using TechTalk.SpecFlow;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;
using VolleyM.Domain.DomainFrameworkTests;
using VolleyM.Domain.Framework.Authorization;
using VolleyM.Domain.Framework.UnitTests.Fixture;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.Framework.UnitTests.AuthorizationHandlerDecorator
{
    [Binding]
    [Scope(Feature = "Authorization Decorator")]
    public class AuthorizationDecoratorSteps
    {
        private enum HandlerType
        {
            SampleHandler,
            NoAttributeHandler
        }

        private HandlerType _handlerType;
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

        [Given(@"I have no permission attribute on a handler")]
        public void GivenIHaveNoPermissionAttributeOnAClass()
        {
            _handlerType = HandlerType.NoAttributeHandler;
        }

        [Given(@"I have handler with permission attribute")]
        public void GivenIHandlerWithPermissionAttributeOnAClass()
        {
            _handlerType = HandlerType.SampleHandler;
        }

        [Given(@"current user has permission to execute this handler")]
        public void GivenUserHasPermissionToCallHandler()
        {
            var permission = GetPermissionForHandler(_handlerType);

            _authorizationService.CheckAccess(permission).Returns(true);
        }

        [Given(@"current user has no permission to execute this handler")]
        public void GivenUserHasNoPermissionToCallHandler()
        {
            _authorizationService.CheckAccess(Arg.Any<Permission>()).Returns(false);
        }

        [When(@"I call decorated handler")]
        public void WhenICallDecoratedHandler()
        {
            _actualResult = ResolveAndCallHandler(_handlerType);
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

        private Either<Error, Unit> ResolveAndCallHandler(HandlerType handlerType)
        {
            return handlerType switch
            {
                HandlerType.NoAttributeHandler => ResolveAndCallSpecificHandler(new NoAttributeHandler.Request()),
                HandlerType.SampleHandler => ResolveAndCallSpecificHandler(new SampleHandler.Request()),
                _ => throw new NotSupportedException()
            };
        }

        private Permission GetPermissionForHandler(HandlerType handlerType)
        {
            return handlerType switch
            {
                HandlerType.NoAttributeHandler => new Permission("DomainFrameworkTests", "NoAttributeHandler"),
                HandlerType.SampleHandler => new Permission("DomainFrameworkTests", "SampleHandler"),
                _ => throw new NotSupportedException()
            };
        }

        private Either<Error, Unit> ResolveAndCallSpecificHandler<T>(T request) where T : IRequest<Unit>
        {
            var handler = _container.GetInstance<IRequestHandler<T, Unit>>();

            return handler.Handle(request).Result;
        }

        private void RegisterHandlers()
        {
            _container.RegisterCommonDomainServices(Assembly.GetAssembly(GetType()));
        }
    }
}
