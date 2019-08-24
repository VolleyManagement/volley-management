using System.Collections.Generic;
using System.Threading;
using FluentAssertions;
using MediatR;
using NSubstitute;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contributors.GetAllContributors;
using Xunit;
using Xunit.Gherkin.Quick;

namespace VolleyM.Domain.Contributors.UnitTests.GetAll
{
    [FeatureFile(@".\GetAll\Contributors.feature")]
    public class ContributorsSteps : Feature
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        private IRequestHandler<GetAllContributorsRequest, List<ContributorDto>> _handler;
        private IQuery<Null, List<ContributorDto>> _queryMock;

        private List<ContributorDto> _expectedResult;
        private List<ContributorDto> _actualResult;

        public ContributorsSteps()
        {
            _queryMock = Substitute.For<IQuery<Null, List<ContributorDto>>>();
        }

        [Given("several contributor exist")]
        public void SeveralContributorsExist()
        {
            _expectedResult = GetMockData();

            MockQueryObject(GetMockData());
        }

        [When("I query all contributors")]
        public async void IQueryAll()
        {
            _handler = CreateHandler();

            _actualResult = await _handler.Handle(new GetAllContributorsRequest(), _cts.Token);
        }

        [Then("I receive all existing contributors")]
        public void AllContributorsReceived()
        {
            _actualResult.Should().BeEquivalentTo(_expectedResult, "handler should return all available contributors");
        }

        private GetAllContributorsQueryHandler CreateHandler() =>
            new GetAllContributorsQueryHandler(_queryMock);

        private void MockQueryObject(List<ContributorDto> testData) =>
            _queryMock.Execute(Null.Value).Returns<List<ContributorDto>>(testData);

        private static List<ContributorDto> GetMockData() =>
            new List<ContributorDto> {
                new ContributorDto{CourseDirection = "Test", FullName = "Name 1", Team = "Team 2" },
                new ContributorDto{CourseDirection = "Test", FullName = "Name 2", Team = "Team 1" }
            };
    }
}