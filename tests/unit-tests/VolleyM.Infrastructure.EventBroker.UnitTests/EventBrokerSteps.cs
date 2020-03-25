using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using LanguageExt;
using NSubstitute;
using SimpleInjector;
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
        private readonly EventInvocationSpy _eventSpy = new EventInvocationSpy();

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
            // do nothing
        }

        [Given(@"(.*) was published once")]
        public void GivenEventWasPublishedOnce(string eventType)
        {
            SetPermissionForHandler();

            var request = new AnotherHandler.Request
            {
                EventData = 10
            };

            _expectedEvents.Add(new EventA { RequestData = request.EventData, SomeData = "AnotherHandler invoked" });

            var handler = _container.GetInstance<IRequestHandler<AnotherHandler.Request, Unit>>();

            var result = handler.Handle(request);
        }

        [When(@"I publish (.*)")]
        public async Task WhenIPublishEvent(string eventType)
        {
            SetPermissionForHandler();

            var request = new SampleHandler.Request
            {
                EventData = 10
            };

            _expectedEvents.Add(new EventA { RequestData = request.EventData, SomeData = "SampleHandler invoked" });

            var handler = _container.GetInstance<IRequestHandler<SampleHandler.Request, Unit>>();

            _actualResult = await handler.Handle(request);
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

        private void SetPermissionForHandler()
        {
            _authorizationService
                .CheckAccess(
                    new Permission("ContextA", "SampleHandler"))
                .Returns(true);

            _authorizationService
                .CheckAccess(
                    new Permission("ContextA", "AnotherHandler"))
                .Returns(true);
        }
    }
}