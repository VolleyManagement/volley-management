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
        private enum RequestHandlerType
        {
            None,
            SampleEventAProducingHandler,   // produces internal event
            AnotherEventAProducingHandler,  // produces internal event
            SampleEventBProducingHandler,   // produces internal event
            SampleEventCProducingHandler,   // produces internal event
            SampleEventDProducingHandler,   // produces public event
            SampleEventEProducingHandler,   // produces public event
        }

        /*
         * Each event type has hardcoded set of event handlers
         * | Event  | Internal  | Public |
         * | EventA | 1         | 0      |
         * | EventB | 1         | 0      |
         * | EventC | 1         | 0      |
         * | EventD | 0         | 1      |
         * | EventE | 0         | 0      |
         * | EventF | 1         |       |
         * | EventG | 1         |       |
         * | EventH | 1         |       |
         */

        private readonly EventInvocationSpy _eventSpy = new EventInvocationSpy();

        private RequestHandlerType _requestHandlerType;
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

        [Given(@"I have (\d+) (.*) event handlers for (.*)")]
        [Given(@"I have (\d+) (.*) event handler for (.*)")]
        public void GivenIHaveEventHandlerForEvent(int handlerCount, string handlerType, string eventType)
        {
            _requestHandlerType = SelectHandler(handlerCount, handlerType);
        }

        [Given(@"(.*) was published once")]
        public async Task GivenEventWasPublishedOnce(string eventType)
        {
            SetPermissionForHandler(handlerType: RequestHandlerType.AnotherEventAProducingHandler);

            const int eventData = 20;
            _expectedEvents.Add(new EventA { RequestData = eventData, SomeData = "AnotherEventAProducingHandler invoked" });

            var result = await ResolveAndCallHandler(RequestHandlerType.AnotherEventAProducingHandler,
                r => { r.EventData = eventData; });
        }

        [When(@"I publish (.*)")]
        public async Task WhenIPublishEvent(string eventType)
        {
            SetPermissionForHandler();

            const int eventData = 10;
            AddExpectedEvent(eventData, eventType);

            _actualResult = await ResolveAndCallHandler(_requestHandlerType,
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

        private void SetPermissionForHandler(string context = "ContextA", RequestHandlerType? handlerType = null)
        {
            _authorizationService
                .CheckAccess(
                    new Permission(context, (handlerType ?? _requestHandlerType).ToString()))
                .Returns(true);
        }

        private Task<Either<Error, Unit>> ResolveAndCallHandler(RequestHandlerType requestHandlerType, Action<IEventProducingRequest> requestBuilder)
        {
            return requestHandlerType switch
            {
                RequestHandlerType.SampleEventAProducingHandler => ResolveAndCallSpecificHandler(new SampleEventAProducingHandler.Request(), requestBuilder),
                RequestHandlerType.AnotherEventAProducingHandler => ResolveAndCallSpecificHandler(new AnotherEventAProducingHandler.Request(), requestBuilder),
                RequestHandlerType.SampleEventBProducingHandler => ResolveAndCallSpecificHandler(new SampleEventBProducingHandler.Request(), requestBuilder),
                RequestHandlerType.SampleEventCProducingHandler => ResolveAndCallSpecificHandler(new SampleEventCProducingHandler.Request(), requestBuilder),
                RequestHandlerType.SampleEventDProducingHandler => ResolveAndCallSpecificHandler(new SampleEventDProducingHandler.Request(), requestBuilder),
                RequestHandlerType.SampleEventEProducingHandler => ResolveAndCallSpecificHandler(new SampleEventEProducingHandler.Request(), requestBuilder),
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
            if (!_expectedEventsMap.TryGetValue(eventType.ToLower(), out var eventsFactory))
            {
                throw new InvalidOperationException("Unknown event type");
            }

            var events = eventsFactory();

            events.ForEach(e => e.RequestData = eventData);

            _expectedEvents.AddRange(events);
        }

        private RequestHandlerType SelectHandler(int handlerCount, string handlerType)
        {
            var handlerTypeStr = handlerType.ToLower();
            if (handlerTypeStr == "internal")
            {
                return handlerCount switch
                {
                    1 => RequestHandlerType.SampleEventAProducingHandler,
                    0 => RequestHandlerType.SampleEventBProducingHandler,
                    2 => RequestHandlerType.SampleEventCProducingHandler,
                    _ => throw new InvalidOperationException("Unsupported number of handlers")
                };
            }
            else if (handlerTypeStr == "public")
            {
                return handlerCount switch
                {
                    1 => RequestHandlerType.SampleEventDProducingHandler,
                    0 => RequestHandlerType.SampleEventEProducingHandler,
                    //2 => RequestHandlerType.SampleEventCProducingHandler,
                    _ => throw new InvalidOperationException("Unsupported number of handlers")
                };
            }

            throw new InvalidOperationException("Unsupported handler type");
        }

        #region Expected event setup

        private static Dictionary<string, Func<List<EventBase>>> _expectedEventsMap
            = new Dictionary<string, Func<List<EventBase>>>
            {
                ["singlesubscriberevent"] = GetSingleInternalCaseEvents,
                ["nosubscribersevent"] = GetNoInternalCaseEvents,
                ["severalsubscribersevent"] = GetSeveralInternalCaseEvents,
                ["publicsinglesubscriberevent"] = GetSinglePublicCaseEvents,
                ["publicnosubscribersevent"] = GetNoPublicCaseEvents
            };

        private static List<EventBase> GetSingleInternalCaseEvents()
        {
            return new List<EventBase> { new EventA { SomeData = "SampleEventAProducingHandler invoked" } };
        }
        private static List<EventBase> GetNoInternalCaseEvents() { return new List<EventBase>(); }
        private static List<EventBase> GetSeveralInternalCaseEvents()
        {
            return new List<EventBase>
            {
                new EventC { SomeData = "SampleEventCProducingHandler invoked" }, 
                new EventC { SomeData = "SampleEventCProducingHandler invoked" }
            };
        }
        private static List<EventBase> GetSinglePublicCaseEvents()
        {
            return new List<EventBase> { new Fixture.ContextB.EventD { SomeData = "SampleEventDProducingHandler invoked" } };
        }
        private static List<EventBase> GetNoPublicCaseEvents() { return new List<EventBase>(); }

        #endregion
    }
}