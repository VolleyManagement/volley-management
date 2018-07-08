namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System;
    using System.Collections.Generic;
    using Domain.GameReportsAggregate;
    using Xunit;
    using FluentAssertions;

    internal class TeamStandingsDtoComparer : IComparer<TeamStandingsDto>
    {
        public bool HasComparerByPoints { get; set; }
        public bool HasComparerBySets { get; set; }
        public bool HasComparerByBalls { get; set; }

        public TeamStandingsDtoComparer()
        {
            HasComparerByPoints = true;
            HasComparerBySets = true;
            HasComparerByBalls = true;
        }
        public int Compare(TeamStandingsDto x, TeamStandingsDto y)
        {
            y.TeamName.Should().Be(x.TeamName, "TeamName do not match");
            y.TeamId.Should().Be(x.TeamId, $"[Team:{x.TeamName}] TeamId do not match");

            if (HasComparerByPoints)
            {
                y.Points.Should().Be(x.Points, $"[Team:{x.TeamName}] Points do not match");
            }
            if (HasComparerBySets)
            {
                Assert.True(AreNullableFloatsEqual(x.SetsRatio, y.SetsRatio), $"[Team:{x.TeamName}] SetsRatio do not match. Actual:<{x.SetsRatio}>, Expected:<{y.SetsRatio}>");
            }
            if (HasComparerByBalls)
            {
                Assert.True(AreNullableFloatsEqual(x.BallsRatio, y.BallsRatio), $"[Team:{x.TeamName}] SetsRatio do not match. Actual:<{x.BallsRatio}>, Expected:<{y.BallsRatio}>");
            }

            return 0;
        }

        private bool AreNullableFloatsEqual(float? x, float? y)
        {
            if (!x.HasValue && !y.HasValue)
            {
                return true;
            }

            if (!x.HasValue || !y.HasValue)
            {
                return false;
            }

            var xVal = x.GetValueOrDefault();
            var yVal = y.GetValueOrDefault();

            if (float.IsInfinity(xVal) && float.IsInfinity(yVal))
            {
                return true;
            }

            return Math.Abs(xVal - yVal) < 0.001;
        }
    }
}