using FluentAssertions;
using NSubstitute;
using System.Collections.Generic;
using System.Threading;
using VolleyM.Domain.Contracts;
using Xunit.Gherkin.Quick;

namespace VolleyM.Domain.Contributors.UnitTests.GetAll
{
    [FeatureFile(@"./GetAll/Contributors.feature")]
    public class ContributorsSteps : Feature
    {
        private IRequestHandler<GetAllContributors.Request, List<ContributorDto>> _handler;
        private readonly GetAllContributors.IQueryObject _queryMock;

        private List<ContributorDto> _expectedResult;
        private List<ContributorDto> _actualResult;

        public ContributorsSteps()
        {
            _queryMock = Substitute.For<GetAllContributors.IQueryObject>();
        }

        [Given("several contributors exist")]
        public void GivenSeveralContributorsExist()
        {
            _expectedResult = GetMockData();

            MockQueryObject(GetMockData());
        }

        [When("I query all contributors")]
        public async void WhenIQueryAllContributors()
        {
            _handler = CreateHandler();

            _actualResult = await _handler.Handle(new GetAllContributors.Request());
        }

        [Then("all contributors received")]
        public void ThenAllContributorsReceived()
        {
            _actualResult.Should().BeEquivalentTo(_expectedResult, "handler should return all available contributors");
        }

        private GetAllContributors.Handler CreateHandler() =>
            new GetAllContributors.Handler(_queryMock);

        private void MockQueryObject(List<ContributorDto> testData) =>
            _queryMock.Execute(Unit.Value).Returns<List<ContributorDto>>(testData);

        private static List<ContributorDto> GetMockData() =>
            new List<ContributorDto> {
                new ContributorDto{CourseDirection = "Test", FullName = "Name 1", Team = "Team 2" },
                new ContributorDto{CourseDirection = "Test", FullName = "Name 2", Team = "Team 1" }
            };
    }
}