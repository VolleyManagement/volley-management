using System.Reflection;
using FluentAssertions;
using SimpleInjector;
using TechTalk.SpecFlow;
using VolleyM.Domain.Contracts.Crosscutting;
using VolleyM.Domain.Framework.EventBus;
using VolleyM.Domain.Framework.UnitTests.EventBus.Fixture;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.Framework.UnitTests.EventBus
{
    [Binding]
    [Scope(Feature = "Receive event notification")]
    public class EventBusSteps
    {
        private readonly Container _container;

        public EventBusSteps(Container container)
        {
            _container = container;
        }

        [BeforeScenario(Order = Constants.BEFORE_SCENARIO_REGISTER_DEPENDENCIES_ORDER)]
        public void RegisterDependenciesForScenario()
        {
            RegisterEventSubscribers();

            _container.Register<EventListenerTestSpy>(Lifestyle.Singleton);
        }

        [Given(@"I have event subscriber for (.*)")]
        public void GivenIHaveEventSubscriberForEvent(string eventType)
        {
            // do nothing all test subscribers are hooked up
        }

        [When(@"(.*) is fired")]
        public void WhenEventIsFired(string eventType)
        {
            var eventBus = _container.GetInstance<IEventBus>();

            eventBus.PublishEvent(new EventA());
        }

        [Then(@"subscriber is invoked")]
        public void ThenSubscriberIsInvoked()
        {
            var eventTestSpy = _container.GetInstance<EventListenerTestSpy>();

            var eventAInvocations = eventTestSpy.GetInvocations<EventA>();

            eventAInvocations.Should().Be(1, "EventA should be invoked once");
        }

        private void RegisterEventSubscribers()
        {
            _container.Collection.Register(typeof(IEventListener<>), Assembly.GetAssembly(GetType()));
        }
    }
}