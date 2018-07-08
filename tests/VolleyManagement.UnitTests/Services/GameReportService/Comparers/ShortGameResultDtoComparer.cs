namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System.Collections.Generic;
    using Domain.GameReportsAggregate;
    using FluentAssertions;

    internal class ShortGameResultDtoComparer : IComparer<ShortGameResultDto>
    {
        public int Compare(ShortGameResultDto x, ShortGameResultDto y)
        {
            y.HomeTeamId.Should().Be(x.HomeTeamId, "HomeTeamId do not match");
            y.HomeGameScore.Should().Be(x.HomeGameScore, "HomeGameScore do not match");

            y.AwayTeamId.Should().Be(x.AwayTeamId, "AwayTeamId do not match");
            y.AwayGameScore.Should().Be(x.AwayGameScore, "AwayGameScore do not match");

            y.IsTechnicalDefeat.Should().Be(x.IsTechnicalDefeat, "IsTechnicalDefeat do not match");

            y.RoundNumber.Should().Be(x.RoundNumber, "RoundNumber do not match");

            y.WasPlayed.Should().Be(x.WasPlayed, "WasPlayed do not match");

            return 0;
        }
    }
}