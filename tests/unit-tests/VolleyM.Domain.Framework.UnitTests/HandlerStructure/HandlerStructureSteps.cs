using LanguageExt;
using NSubstitute;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Reflection;
using RootNs;
using TechTalk.SpecFlow;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;
using VolleyM.Domain.Framework.Authorization;
using VolleyM.Domain.Framework.HandlerMetadata;
using VolleyM.Domain.Framework.UnitTests.Fixture;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;
using VolleyM.Domain.IDomainFrameworkTestFixture;
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
            MockedHandler,
            RootNsHandler
        }

        private HandlerType _handlerType;
        private Either<Error, Unit> _actualResult;

        private readonly Container _container;
        private IAuthorizationService _authorizationService;

        public HandlerStructureSteps(Container container)
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

        [Given(@"I have a handler that is in short name space")]
        public void GivenIHaveAHandlerThatIsInShortNameSpace()
        {
            _handlerType = HandlerType.RootNsHandler;
        }

        [Given(@"I override Handler metadata for this request type")]
        public void GivenIOverrideHandlerMetadataForThisRequestType()
        {
            var metadataService = _container.GetInstance<HandlerMetadataService>();

            metadataService.OverrideHandlerMetadata<MockedHandler.Request>(
                new HandlerInfo(nameof(IDomainFrameworkTestFixture), nameof(MockedHandler) ));
        }

        [When(@"I call decorated handler")]
        public void WhenICallDecoratedHandler()
        {
            SetPermissionForHandler();
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
            FrameworkDomainComponentDependencyRegistrar.RegisterCommonServices(_container,
                new List<Assembly> { Assembly.GetAssembly(GetType()) });
        }

        private Either<Error, Unit> ResolveAndCallHandler(HandlerType handlerType)
        {
            return handlerType switch
            {
                HandlerType.TwoInterfacesHandler => ResolveAndCallSpecificHandler(new TwoInterfacesHandler.Request()),
                HandlerType.NotNestedHandler => ResolveAndCallSpecificHandler(new NotNestedHandler.Request()),
                HandlerType.SampleHandler => ResolveAndCallSpecificHandler(new SampleHandler.Request()),
                HandlerType.MockedHandler => ResolveAndCallSpecificHandler(new MockedHandler.Request()),
                HandlerType.RootNsHandler => ResolveAndCallSpecificHandler(new RootNsHandler.Request()),
                _ => throw new NotSupportedException()
            };
        }
        private Either<Error, Unit> ResolveAndCallSpecificHandler<T>(T request) where T : IRequest<Unit>
        {
            var handler = _container.GetInstance<IRequestHandler<T, Unit>>();

            return handler.Handle(request).Result;
        }
        private void SetPermissionForHandler()
        {
            _authorizationService
                .CheckAccess(
                    new Permission(nameof(IDomainFrameworkTestFixture), _handlerType.ToString()))
                .Returns(true);
        }
    }
}