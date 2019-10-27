using System;
using System.Reflection;
using LanguageExt;
using NSubstitute;
using SimpleInjector;
using TechTalk.SpecFlow;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Framework.Authorization;
using VolleyM.Domain.Framework.UnitTests.Fixture;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.Framework.UnitTests.ValidationDecorator
{
    [Binding]
    [Scope(Feature = "Validation Decorator")]
    public class ValidationDecoratorSteps
    {
        private enum HandlerType
        {
            SampleHandler,
            NoValidationHandler
        }

        private HandlerType _handlerType;
        private Either<Error, Unit> _actualResult;

        private readonly Container _container;
        private IAuthorizationService _authorizationService;

        public ValidationDecoratorSteps(Container container)
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

        [Given(@"I have a handler")]
        public void GivenIHaveAHandler()
        {
            // do nothing
        }

        [Given(@"validator is not defined")]
        public void GivenValidatorIsNotDefined()
        {
            _handlerType = HandlerType.NoValidationHandler;
            SetPermissionForHandler();
        }

        [Given(@"single validator defined")]
        public void GivenSingleValidatorDefined()
        {
            _handlerType = HandlerType.SampleHandler;
            SetPermissionForHandler();
        }

        [Given(@"validator passes")]
        public void GivenValidatorPasses()
        {
            // it's handled by request factory
        }

        [When(@"I call decorated handler")]
        public void WhenICallDecoratedHandler()
        {
            _actualResult = ResolveAndCallHandler(_handlerType);
        }

        [Then(@"handler result should be returned")]
        public void ThenHandlerResultShouldBeReturned()
        {
            _actualResult.ShouldBeEquivalent(Unit.Default);
        }

        private Either<Error, Unit> ResolveAndCallHandler(HandlerType handlerType)
        {
            return handlerType switch
            {
                HandlerType.NoValidationHandler => ResolveAndCallSpecificHandler(new NoValidationHandler.Request()),
                HandlerType.SampleHandler => ResolveAndCallSpecificHandler(new SampleHandler.Request()),
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
            _container.Register(typeof(IRequestHandler<,>), Assembly.GetAssembly(GetType()), Lifestyle.Scoped);
        }

        private void SetPermissionForHandler()
        {
            _authorizationService
                .CheckAccess(
                    new Permission("DomainFrameworkTests", _handlerType.ToString()))
                .Returns(true);
        }

    }
}