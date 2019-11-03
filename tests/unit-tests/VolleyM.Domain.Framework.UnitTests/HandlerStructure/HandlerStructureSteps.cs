using LanguageExt;
using SimpleInjector;
using System;
using System.Reflection;
using TechTalk.SpecFlow;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;
using VolleyM.Domain.DomainFrameworkTests;
using VolleyM.Domain.Framework.HandlerMetadata;
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
            NotNestedHandler,
            SampleHandler,
            MockedHandler
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

        [Given(@"I have an example handler")]
        public void GivenSampleHandler()
        {
            _handlerType = HandlerType.SampleHandler;
        }

        [Given(@"I have mocked handler")]
        public void GivenIHaveMockedHandler()
        {
            _handlerType = HandlerType.MockedHandler;
        }

        [Given(@"I override Handler metadata for this request type")]
        public void GivenIOverrideHandlerMetadataForThisRequestType()
        {
            var metadataService = _container.GetInstance<HandlerMetadataService>();

            metadataService.OverrideHandlerMetadata<NotNestedHandler.Request>(
                new HandlerMetadata.HandlerMetadata {Context = "DomainFrameworkTests", Action = "NotNestedHandler"});
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

        [Then(@"handler result should be returned")]
        public void ThenHandlerResultShouldBeReturned()
        {
            _actualResult.ShouldBeEquivalent(Unit.Default);
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
                HandlerType.SampleHandler => ResolveAndCallSpecificHandler(new SampleHandler.Request()),
                HandlerType.MockedHandler => ResolveAndCallSpecificHandler(new NotNestedHandler.Request()), //usually it will look like not nested handler
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