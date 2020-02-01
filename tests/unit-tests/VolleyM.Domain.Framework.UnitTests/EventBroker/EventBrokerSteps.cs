using FluentAssertions;
using SimpleInjector;
using System.Reflection;
using TechTalk.SpecFlow;
using VolleyM.Domain.Contracts.Crosscutting;
using VolleyM.Domain.Framework.EventBus;
using VolleyM.Domain.IDomainFrameworkTestFixture;
using VolleyM.Domain.UnitTests.Framework;
using VolleyM.Infrastructure.EventBroker.MassTransit;

namespace VolleyM.Domain.Framework.UnitTests.EventBroker
{
    [Binding]
    [Scope(Feature = "EventBroker")]
    public class EventBrokerSteps
    {
        private readonly Container _container;

        public EventBrokerSteps(Container container)
        {
            _container = container;
        }

        [BeforeScenario(Order = Constants.BEFORE_SCENARIO_REGISTER_DEPENDENCIES_ORDER)]
        public void RegisterDependenciesForScenario()
        {
            RegisterHandlers();

            _container.RegisterInstance(typeof(IEventPublisher), new MassTransitEventPublisher());
        }

        [Given(@"I have EventA listener")]
        public void GivenIHaveEventAListener()
        {
            //nothing to do: event is registered by bootstrapping logic 
        }

        [When(@"I publish this event")]
        public void WhenIPublishThisEvent()
        {
            var publisher = _container.GetInstance<IEventPublisher>();
            publisher.PublishEvent(CreateAnEvent());
        }

        [Then(@"event is delivered to the listener")]
        public void ThenEventIsDeliveredToTheListener()
        {
            var listener = _container.GetInstance<SpyEventListener>();
            var expectedEvents = new object[] { CreateAnEvent() };

            listener.Events.Should().BeEquivalentTo(expectedEvents);
        }

        private static SampleEvent CreateAnEvent()
        {
            return new SampleEvent { Data = "some arbitrary message" };
        }

        private void RegisterHandlers()
        {
            _container.RegisterCommonDomainServices(Assembly.GetAssembly(GetType()));
        }
    }
}
