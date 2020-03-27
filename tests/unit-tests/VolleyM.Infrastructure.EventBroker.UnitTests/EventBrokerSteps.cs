using FluentAssertions;
using LanguageExt;
using NSubstitute;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using VolleyM.Domain.ContextA;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;
using VolleyM.Domain.Framework.Authorization;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;
using VolleyM.Domain.UnitTests.Framework;
using VolleyM.Infrastructure.EventBroker.UnitTests.Fixture;
using VolleyM.Infrastructure.EventBroker.UnitTests.Fixture.ContextA;

namespace VolleyM.Infrastructure.EventBroker.UnitTests
{
    [Binding]
    [Scope(Feature = "EventBroker")]
    public class EventBrokerSteps
    {
        private enum HandlerType
        {
            None,
            SampleEventAProducingHandler,
            AnotherEventAProducingHandler,
            SampleEventBProducingHandler,
            SampleEventCProducingHandler
        }

        private readonly EventInvocationSpy _eventSpy = new EventInvocationSpy();

        private HandlerType _handlerType;
        private Either<Error, Unit> _actualResult;
        private List<object> _expectedEvents = new List<object>();

        private readonly Container _container;
        private IAuthorizationService _authorizationService;

        public EventBrokerSteps(Container container)
        {
            _container = container;
        }

        [BeforeScenario(Order = Constants.BEFORE_SCENARIO_REGISTER_DEPENDENCIES_ORDER)]
        public void RegisterDependenciesForScenario()
        {
            RegisterHandlers();

            _container.RegisterInstance(_eventSpy);

            _authorizationService = Substitute.For<IAuthorizationService>();
            _container.RegisterInstance(_authorizationService);
        }

        [Given(@"I have single event handler for (.*)")]
        public void GivenIHaveSingleEventHandlerForEvent(string eventType)
        {
            _handlerType = HandlerType.SampleEventAProducingHandler;
        }

        [Given("I have no event handlers for (.*)")]
        public void GivenIHaveNoEventHandlersForEvent(string eventType)
        {
            _handlerType = HandlerType.SampleEventBProducingHandler;
        }

        [Given("I have several event handlers for (.*)")]
        public void GivenIHaveSeveralEventHandlersForEvent(string eventType)
        {
            _handlerType = HandlerType.SampleEventCProducingHandler;
        }

        [Given(@"(.*) was published once")]
        public async Task GivenEventWasPublishedOnce(string eventType)
        {
            SetPermissionForHandler(handlerType: HandlerType.AnotherEventAProducingHandler);

            const int eventData = 20;
            _expectedEvents.Add(new EventA { RequestData = eventData, SomeData = "AnotherEventAProducingHandler invoked" });

            var result = await ResolveAndCallHandler(HandlerType.AnotherEventAProducingHandler,
                r => { r.EventData = eventData; });
        }

        [When(@"I publish (.*)")]
        public async Task WhenIPublishEvent(string eventType)
        {
            SetPermissionForHandler();

            const int eventData = 10;
            AddExpectedEvent(eventData, eventType);

            _actualResult = await ResolveAndCallHandler(_handlerType,
                r => { r.EventData = eventData; }); ;
        }

        [Then(@"handler result should be returned")]
        public void ThenHandlerResultShouldBeReturned()
        {
            _actualResult.ShouldBeEquivalent(Unit.Default);
        }

        [Then(@"handler should receive event")]
        [Then("handler should receive all events")]
        public void ThenHandlerShouldReceiveEvent()
        {
            _eventSpy.Invocations.Should().BeEquivalentTo(_expectedEvents);
        }

        private void RegisterHandlers()
        {
            _container.RegisterCommonDomainServices(Assembly.GetAssembly(GetType()));
        }

        private void SetPermissionForHandler(string context = "ContextA", HandlerType? handlerType = null)
        {
            _authorizationService
                .CheckAccess(
                    new Permission(context, (handlerType ?? _handlerType).ToString()))
                .Returns(true);
        }

        private Task<Either<Error, Unit>> ResolveAndCallHandler(HandlerType handlerType, Action<IEventProducingRequest> requestBuilder)
        {
            return handlerType switch
            {
                HandlerType.SampleEventAProducingHandler => ResolveAndCallSpecificHandler(new SampleEventAProducingHandler.Request(), requestBuilder),
                HandlerType.AnotherEventAProducingHandler => ResolveAndCallSpecificHandler(new AnotherEventAProducingHandler.Request(), requestBuilder),
                HandlerType.SampleEventBProducingHandler => ResolveAndCallSpecificHandler(new SampleEventBProducingHandler.Request(), requestBuilder),
                HandlerType.SampleEventCProducingHandler => ResolveAndCallSpecificHandler(new SampleEventCProducingHandler.Request(), requestBuilder),
                _ => throw new NotSupportedException()
            };
        }
        private Task<Either<Error, Unit>> ResolveAndCallSpecificHandler<T>(T request, Action<IEventProducingRequest> requestBuilder) where T : IRequest<Unit>, IEventProducingRequest
        {
            var handler = _container.GetInstance<IRequestHandler<T, Unit>>();

            requestBuilder(request);

            return handler.Handle(request);
        }

        private void AddExpectedEvent(int eventData, string eventType)
        {
            switch (eventType.ToLower())
            {
                case "singlesubscriberevent":
                    _expectedEvents.Add(new EventA
                    {
                        RequestData = eventData, SomeData = "SampleEventAProducingHandler invoked"
                    });
                    break;
                case "nosubscribersevent":
                    //do nothing
                    break;
                case "severalsubscribersevent":
                    var evt = new EventC {RequestData = eventData, SomeData = "SampleEventCProducingHandler invoked"};
                    _expectedEvents.Add(evt);
                    _expectedEvents.Add(evt);
                    break;
                default:
                    throw new InvalidOperationException("Unknown event type");
            }
        }
    }
}