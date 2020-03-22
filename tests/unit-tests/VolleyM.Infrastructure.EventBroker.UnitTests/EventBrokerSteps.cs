using TechTalk.SpecFlow;

namespace VolleyM.Infrastructure.EventBroker.UnitTests
{
    [Binding]
    [Scope(Feature = "EventBroker")]
    public class EventBrokerSteps
    {
        [Given(@"I have single event handler for (.*)")]
        public void GivenIHaveSingleEventHandlerForEvent(string eventType)
        {
        }

        [When(@"I publish (.*)")]
        public void WhenIPublishEvent(string eventType)
        {
        }

        [Then(@"handler should receive event")]
        public void ThenHandlerShouldReceiveEvent()
        {
        }

    }
}