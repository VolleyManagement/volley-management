using System;
using FluentAssertions;
using SimpleInjector;
using TechTalk.SpecFlow;
using VolleyM.Domain.Framework.HandlerMetadata;
using VolleyM.Domain.Framework.UnitTests.Fixture;

namespace VolleyM.Domain.Framework.UnitTests.HandlerStructure
{
    [Binding]
    [Scope(Feature = "Validator locator")]
    public class ValidatorLocatorSteps
    {
        private readonly Container _container;

        private bool _actualResult;
        private Type _requestType;

        public ValidatorLocatorSteps(Container container)
        {
            _container = container;
        }

        [Given(@"I have a handler that is not nested in a class")]
        public void GivenIHaveAHandlerThatIsNotNestedInAClass()
        {
            _requestType = typeof(NotNestedHandler);
        }

        [When(@"has validator check is performed")]
        public void WhenHasValidatorCheckIsPerformed()
        {
            var service = _container.GetInstance<HandlerMetadataService>();
            _actualResult = service.HasValidator(_requestType);
        }

        [Then(@"(.*) is returned")]
        public void ThenFalseIsReturned(bool result)
        {
            _actualResult.Should().Be(result);
        }

    }
}