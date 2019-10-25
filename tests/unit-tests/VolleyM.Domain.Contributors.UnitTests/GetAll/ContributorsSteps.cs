using SimpleInjector;
using System.Collections.Generic;
using LanguageExt;
using TechTalk.SpecFlow;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.Contributors.UnitTests.GetAll
{
    [Binding]
    [Scope(Feature = "Contributors")]
    public class ContributorsSteps
    {
        private readonly IContributorsTestFixture _testFixture;
        private readonly IAuthFixture _authFixture;
        private readonly Container _container;

        private IRequestHandler<GetAllContributors.Request, List<ContributorDto>> _handler;

        private List<ContributorDto> _expectedResult;
        private Either<Error, List<ContributorDto>> _actualResult;

        public ContributorsSteps(IContributorsTestFixture testFixture, IAuthFixture authFixture, Container container)
        {
            _testFixture = testFixture;
            _authFixture = authFixture;
            _container = container;
        }

        [BeforeScenario(Order = Constants.BEFORE_SCENARIO_STEPS_ORDER)]
        public void ScenarioSetup()
        {
            _authFixture.SetTestUserPermission(new Permission(Permissions.Context, Permissions.GetAll));
        }

        [Given(@"several contributors exist")]
        public void GivenSeveralContributorsExist()
        {
            _expectedResult = GetMockData();
            _testFixture.MockSeveralContributorsExist(GetMockData());
        }

        [When(@"I query all contributors")]
        public async void WhenIQueryAllContributors()
        {
            _handler = _container.GetInstance<IRequestHandler<GetAllContributors.Request, List<ContributorDto>>>();

            _actualResult = await _handler.Handle(new GetAllContributors.Request());
        }

        [Then(@"all contributors received")]
        public void ThenAllContributorsReceived()
        {
            _actualResult.ShouldBeEquivalent(_expectedResult, "handler should return all available contributors");
        }

        private static List<ContributorDto> GetMockData() =>
            new List<ContributorDto>
            {
                new ContributorDto {CourseDirection = "Test", FullName = "Name 1", Team = "Team 2"},
                new ContributorDto {CourseDirection = "Test", FullName = "Name 2", Team = "Team 1"}
            };
    }
}
