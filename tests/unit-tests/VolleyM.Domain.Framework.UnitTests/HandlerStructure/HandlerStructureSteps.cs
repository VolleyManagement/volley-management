using LanguageExt;
using SimpleInjector;
using System;
using System.Reflection;
using TechTalk.SpecFlow;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;
using VolleyM.Domain.Framework.UnitTests.Fixture;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.Framework.UnitTests.HandlerStructure
{
    [Binding]
    [Scope(Feature = "Handler Structure validation")]
    public class HandlerStructureSteps
    {
        private enum HandlerType
        {
            TwoInterfacesHandler,
            NotNestedHandler
        }

        private HandlerType _handlerType;
        private Either<Error, Unit> _actualResult;
        
        private readonly Container _container;

        public HandlerStructureSteps(Container container)
        {
            _container = container;
        }

        [BeforeScenario(Order = Constants.BEFORE_SCENARIO_REGISTER_DEPENDENCIES_ORDER)]
        public void RegisterDependenciesForScenario()
        {
            RegisterHandlers();
        }

        [Given(@"I have a handler with several IRequestHandler<,> interfaces")]
        public void GivenHandlerHasSeveralIRequestHandlerInterfaces()
        {
            _handlerType = HandlerType.TwoInterfacesHandler;
        }

        [Given(@"I have a handler that is not nested in a class")]
        public void GivenHandlerIsNotNested()
        {
            _handlerType = HandlerType.NotNestedHandler;
        }

        [When(@"I call decorated handler")]
        public void WhenICallDecoratedHandler()
        {
            _actualResult = ResolveAndCallHandler(_handlerType);
        }

        [Then(@"DesignViolation error should be returned with message '(.*)'")]
        public void ThenDesignViolationErrorShouldBeReturnedWithMessage(string message)
        {
            _actualResult.ShouldBeError(Error.DesignViolation(message));
        }

        private void RegisterHandlers()
        {
            _container.RegisterCommonDomainServices(Assembly.GetAssembly(GetType()));
        }

        private Either<Error, Unit> ResolveAndCallHandler(HandlerType handlerType)
        {
            return handlerType switch
            {
                HandlerType.TwoInterfacesHandler => ResolveAndCallSpecificHandler(new TwoInterfacesHandler.Request()),
                HandlerType.NotNestedHandler => ResolveAndCallSpecificHandler(new NotNestedHandler.Request()),
                _ => throw new NotSupportedException()
            };
        }
        private Either<Error, Unit> ResolveAndCallSpecificHandler<T>(T request) where T : IRequest<Unit>
        {
            var handler = _container.GetInstance<IRequestHandler<T, Unit>>();

            return handler.Handle(request).Result;
        }
    }
}