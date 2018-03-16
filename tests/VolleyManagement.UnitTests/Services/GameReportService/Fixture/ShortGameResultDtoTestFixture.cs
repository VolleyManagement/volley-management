namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.GameReportsAggregate;

    [ExcludeFromCodeCoverage]
    internal class ShortGameResultDtoTestFixture
    {
        private List<ShortGameResultDto> _shortResults;

        public ShortGameResultDtoTestFixture GetShortResults()
        {
            _shortResults = new List<ShortGameResultDto>();

            _shortResults.Add(
                new ShortGameResultDto
                {
                    HomeTeamId = 1,
                    AwayTeamId = 3,
                    HomeGameScore = 3,
                    AwayGameScore = 2,
                    IsTechnicalDefeat = false
                });

            _shortResults.Add(
                new ShortGameResultDto
                {
                    HomeTeamId = 3,
                    AwayTeamId = 1,
                    HomeGameScore = 3,
                    AwayGameScore = 0,
                    IsTechnicalDefeat = true
                });

            _shortResults.Add(
                new ShortGameResultDto
                {
                    HomeTeamId = 2,
                    AwayTeamId = 1,
                    HomeGameScore = 1,
                    AwayGameScore = 3,
                    IsTechnicalDefeat = false
                });

            return this;
        }

        public ShortGameResultDtoTestFixture GetShortResultsForTwoTeamsScoresCompletelyEqual()
        {
            _shortResults = new List<ShortGameResultDto>();

            _shortResults.Add(
                new ShortGameResultDto
                {
                    HomeTeamId = 1,
                    AwayTeamId = 2,
                    HomeGameScore = 1,
                    AwayGameScore = 3,
                    IsTechnicalDefeat = false
                });

            _shortResults.Add(
                new ShortGameResultDto
                {
                    HomeTeamId = 1,
                    AwayTeamId = 3,
                    HomeGameScore = 1,
                    AwayGameScore = 3,
                    IsTechnicalDefeat = false
                });

            return this;
        }

        public List<ShortGameResultDto> Build()
        {
            return _shortResults;
        }
    }
}