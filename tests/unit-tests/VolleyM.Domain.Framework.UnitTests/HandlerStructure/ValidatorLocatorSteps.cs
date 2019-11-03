using System;
using FluentAssertions;
using SimpleInjector;
using TechTalk.SpecFlow;
using VolleyM.Domain.Framework.HandlerMetadata;
using VolleyM.Domain.Framework.UnitTests.Fixture;
using VolleyM.Domain.IDomainFrameworkTestFixture;

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

        [Given(@"I have a handler that does not have corresponding validator")]
        public void GivenIHaveAHandlerThatDoesNotHaveCorrespondingValidator()
        {
            _requestType = typeof(NoValidationHandler.Handler);
        }

        [Given(@"I have a handler with validator")]
        public void GivenIHaveAHandlerWithValidator()
        {
            _requestType = typeof(SampleHandler.Handler);
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