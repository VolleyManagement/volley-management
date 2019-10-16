using FluentAssertions;
using NSubstitute;
using SimpleInjector;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.Contributors.UnitTests.GetAll
{
    [Binding]
    public class ContributorsSteps : ContributorsStepsBase
    {
        private IRequestHandler<GetAllContributors.Request, List<ContributorDto>> _handler;
        private IQuery<Unit, List<ContributorDto>> _queryMock;

        private Result<List<ContributorDto>> _expectedResult;
        private Result<List<ContributorDto>> _actualResult;

        public override void BeforeEachScenario()
        {
            base.BeforeEachScenario();

            _queryMock = Substitute.For<IQuery<Unit, List<ContributorDto>>>();

            Container.Register(() => _queryMock, Lifestyle.Scoped);
        }

        [Given(@"several contributors exist")]
        public void GivenSeveralContributorsExist()
        {
            _expectedResult = GetMockData();

            MockQueryObject(GetMockData());
        }

        [When(@"I query all contributors")]
        public async void WhenIQueryAllContributors()
        {
            _handler = Container.GetInstance<IRequestHandler<GetAllContributors.Request, List<ContributorDto>>>();

            _actualResult = await _handler.Handle(new GetAllContributors.Request());
        }

        [Then(@"all contributors received")]
        public void ThenAllContributorsReceived()
        {
            _actualResult.Should().BeEquivalentTo(_expectedResult, "handler should return all available contributors");
        }
        private void MockQueryObject(List<ContributorDto> testData) =>
            _queryMock.Execute(Unit.Value).Returns(testData);

        private static List<ContributorDto> GetMockData() =>
            new List<ContributorDto>
            {
                new ContributorDto {CourseDirection = "Test", FullName = "Name 1", Team = "Team 2"},
                new ContributorDto {CourseDirection = "Test", FullName = "Name 2", Team = "Team 1"}
            };
    }
}
