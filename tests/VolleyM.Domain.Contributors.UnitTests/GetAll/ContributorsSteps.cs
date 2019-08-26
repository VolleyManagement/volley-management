﻿using FluentAssertions;
using MediatR;
using NSubstitute;
using System.Collections.Generic;
using System.Threading;
using TestStack.BDDfy;
using VolleyM.Domain.Contracts;
using Xunit;

namespace VolleyM.Domain.Contributors.UnitTests.GetAll
{
    public class ContributorsSteps
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        private IRequestHandler<GetAllContributors.Request, List<ContributorDto>> _handler;
        private readonly GetAllContributors.IQueryObject _queryMock;

        private List<ContributorDto> _expectedResult;
        private List<ContributorDto> _actualResult;

        public ContributorsSteps()
        {
            _queryMock = Substitute.For<GetAllContributors.IQueryObject>();
        }

        [Fact]
        public void ExistingContributorsRetrieved()
        {
            this.Given(_ => GivenSeveralContributorsExist())
                .When(_ => WhenIQueryAllContributors())
                .Then(_ => ThenAllContributorsReceived())
                .BDDfy("Query all contributors");
        }

        public void GivenSeveralContributorsExist()
        {
            _expectedResult = GetMockData();

            MockQueryObject(GetMockData());
        }

        public async void WhenIQueryAllContributors()
        {
            _handler = CreateHandler();

            _actualResult = await _handler.Handle(new GetAllContributors.Request(), _cts.Token);
        }

        public void ThenAllContributorsReceived()
        {
            _actualResult.Should().BeEquivalentTo(_expectedResult, "handler should return all available contributors");
        }

        private GetAllContributors.Handler CreateHandler() =>
            new GetAllContributors.Handler(_queryMock);

        private void MockQueryObject(List<ContributorDto> testData) =>
            _queryMock.Execute(Null.Value).Returns<List<ContributorDto>>(testData);

        private static List<ContributorDto> GetMockData() =>
            new List<ContributorDto> {
                new ContributorDto{CourseDirection = "Test", FullName = "Name 1", Team = "Team 2" },
                new ContributorDto{CourseDirection = "Test", FullName = "Name 2", Team = "Team 1" }
            };
    }
}