using System;
using System.Collections.Generic;
using LanguageExt;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using SimpleInjector;
using System.Reflection;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;
using VolleyM.Domain.Contracts.EventBroker;
using VolleyM.Domain.Framework.Authorization;
using VolleyM.Domain.Framework.EventBroker;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;
using VolleyM.Domain.IDomainFrameworkTestFixture;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.Framework.UnitTests.EventBroker
{
    [Binding]
    [Scope(Feature = "Domain Event dispatching")]
    public class EventProducerDecoratorSteps
    {
        private enum HandlerType
        {
            None,
            SampleHandler,
            HandlerWhichDoesNotProduceEvent,
            SeveralEventsHandler,
            NoEventSupportHandler,
            HandlerWithNullDomainEventsProperty
        }

        private HandlerType _handlerType;
        private Either<Error, Unit> _actualResult;
        private IEventPublisher _eventPublisher;

        private readonly Container _container;
        private IAuthorizationService _authorizationService;

        public EventProducerDecoratorSteps(Container container)
        {
            _container = container;
        }

        [BeforeScenario(Order = Constants.BEFORE_SCENARIO_REGISTER_DEPENDENCIES_ORDER)]
        public void RegisterDependenciesForScenario()
        {
            RegisterHandlers();

            _eventPublisher = Substitute.For<IEventPublisher>();
            _eventPublisher.PublishEvent(Arg.Any<IEvent>()).Returns(Task.CompletedTask);
            _container.RegisterInstance(_eventPublisher);

            _authorizationService = Substitute.For<IAuthorizationService>();
            _container.RegisterInstance(_authorizationService);
        }

        [Given(@"I have a handler which can produce events")]
        public void GivenIHaveAHandlerWhichCanProduceEvents()
        {
            _handlerType = HandlerType.SampleHandler;
        }

        [Given(@"I have a handler which does not produce events")]
        public void GivenIHaveAHandlerWhichDoesNotProduceEvents()
        {
            _handlerType = HandlerType.NoEventSupportHandler;
        }

        [Given(@"I have a handler which does not have domain events initialized")]
        public void GivenIHaveAHandlerWhichDoesNotHaveDomainEventsInitialized()
        {
            _handlerType = HandlerType.HandlerWithNullDomainEventsProperty;
        }

        [Given(@"handler produces event")]
        public void GivenHandlerProducesEvent()
        {
            _handlerType = HandlerType.SampleHandler;
        }

        [Given(@"handler does not produce event")]
        public void GivenHandlerDoesNotProduceEvent()
        {
            _handlerType = HandlerType.HandlerWhichDoesNotProduceEvent;
        }

        [Given(@"handler produced several events")]
        public void GivenHandlerProducedSeveralEvents()
        {
            _handlerType = HandlerType.SeveralEventsHandler;
        }

        [When(@"I call decorated handler")]
        public async void WhenICallDecoratedHandler()
        {
            SetPermissionForHandler();
            _actualResult = await ResolveAndCallHandler(_handlerType);
        }

        [Then(@"event is published to event broker")]
        public void ThenEventIsPublishedToEventBroker()
        {
            _eventPublisher
                .Received(Quantity.Exactly(1))
                .PublishEvent(Arg.Any<SampleEvent>());
        }

        [Then(@"nothing is published to event broker")]
        public void ThenNothingIsPublishedToEventBroker()
        {
            _eventPublisher
                .DidNotReceive()
                .PublishEvent(Arg.Any<IEvent>());
        }

        [Then(@"all events are published to event broker")]
        public void ThenAllEventsArePublishedToEventBroker()
        {
            _eventPublisher
                .Received(Quantity.Exactly(1))
                .PublishEvent(Arg.Any<SeveralEventsHandler.SampleEventA>());
            _eventPublisher
                .Received(Quantity.Exactly(1))
                .PublishEvent(Arg.Any<SeveralEventsHandler.SampleEventB>());
        }

        [Then(@"handler result should be returned")]
        public void ThenHandlerResultShouldBeReturned()
        {
            _actualResult.ShouldBeEquivalent(Unit.Default);
        }

        [Then(@"DesignViolation error should be returned with message '(.*)'")]
        public void ThenDesignViolationErrorShouldBeReturnedWithMessage(string message)
        {
            _actualResult.ShouldBeError(Error.DesignViolation(message));
        }

        private void RegisterHandlers()
        {
            FrameworkDomainComponentDependencyRegistrar.RegisterCommonServices(_container,
                new List<Assembly> { Assembly.GetAssembly(GetType()) });
        }

        private void SetPermissionForHandler()
        {
            _authorizationService
                .CheckAccess(
                    new Permission(nameof(IDomainFrameworkTestFixture), _handlerType.ToString()))
                .Returns(true);
        }

        private Task<Either<Error, Unit>> ResolveAndCallHandler(HandlerType handlerType)
        {
            return handlerType switch
            {
                HandlerType.SampleHandler => ResolveAndCallSpecificHandler(new SampleHandler.Request()),
                HandlerType.HandlerWhichDoesNotProduceEvent => ResolveAndCallSpecificHandler(new HandlerWhichDoesNotProduceEvent.Request()),
                HandlerType.SeveralEventsHandler => ResolveAndCallSpecificHandler(new SeveralEventsHandler.Request()),
                HandlerType.NoEventSupportHandler => ResolveAndCallSpecificHandler(new NoEventSupportHandler.Request()),
                HandlerType.HandlerWithNullDomainEventsProperty => ResolveAndCallSpecificHandler(new HandlerWithNullDomainEventsProperty.Request()),
                _ => throw new NotSupportedException()
            };
        }
        private Task<Either<Error, Unit>> ResolveAndCallSpecificHandler<T>(T request) where T : IRequest<Unit>
        {
            var handler = _container.GetInstance<IRequestHandler<T, Unit>>();

            return handler.Handle(request);
        }
    }
}