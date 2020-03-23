using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using LanguageExt;
using NSubstitute;
using SimpleInjector;
using TechTalk.SpecFlow;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;
using VolleyM.Domain.Framework.Authorization;
using VolleyM.Domain.Framework.EventBus;
using VolleyM.Domain.UnitTests.Framework;
using VolleyM.Infrastructure.EventBroker.UnitTests.Fixture;
using VolleyM.Infrastructure.EventBroker.UnitTests.Fixture.ContextA;

namespace VolleyM.Infrastructure.EventBroker.UnitTests
{
    [Binding]
    [Scope(Feature = "EventBroker")]
    public class EventBrokerSteps
    {
        private readonly EventInvocationSpy _eventSpy = new EventInvocationSpy();

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
            // do nothing
        }

        [When(@"I publish (.*)")]
        public async Task WhenIPublishEvent(string eventType)
        {
            var request = new SampleHandler.Request
            {
                EventData = 10
            };

            _expectedEvents.Add(new EventA { RequestData = 10, SomeData = "SampleHandler invoked" });

            var handler = _container.GetInstance<IRequestHandler<SampleHandler.Request, Unit>>();

            await handler.Handle(request);
        }

        [Then(@"handler should receive event")]
        public void ThenHandlerShouldReceiveEvent()
        {
            _eventSpy.Invocations.Should().BeEquivalentTo(_expectedEvents);
        }

        private void RegisterHandlers()
        {
            _container.RegisterCommonDomainServices(Assembly.GetAssembly(GetType()));
        }
    }
}