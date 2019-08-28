namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System.Collections.Generic;
    using Domain.GameReportsAggregate;
    using FluentAssertions;

    internal class PivotStandingsComparer : IComparer<PivotStandingsDto>, IEqualityComparer<PivotStandingsDto>
    {
        private TeamStandingsDtoComparer teamsComparer;
        private bool hasComparerByGames = true;

        public PivotStandingsComparer()
        {
            this.teamsComparer = new TeamStandingsDtoComparer();
        }
        public void WithPointsComparer()
        {
            CleanComparerFlags();
            teamsComparer.HasComparerByPoints = true;
        }
        public void WithSetsComparer()
        {
            CleanComparerFlags();
            teamsComparer.HasComparerBySets = true;
        }
        public void WithBallsComparer()
        {
            CleanComparerFlags();
            teamsComparer.HasComparerByBalls = true;
        }
        public void WithGamesComparer()
        {
            CleanComparerFlags();
            hasComparerByGames = true;
        }
        private void CleanComparerFlags()
        {
            teamsComparer.HasComparerByPoints = false;
            teamsComparer.HasComparerByBalls = false;
            teamsComparer.HasComparerBySets = false;
            hasComparerByGames = false;
        }
        public int Compare(PivotStandingsDto x, PivotStandingsDto y)
        {
            y.DivisionId.Should().Be(x.DivisionId, "Division Ids do not match");
            y.DivisionName.Should().Be(x.DivisionName, $"[DivisionId={x.DivisionId}] Division Names do not match");
            y.LastUpdateTime.Should().Be(x.LastUpdateTime, $"[DivisionId={x.DivisionId}] Last Update time do not match");

            x.Teams.Count.Should().Be(y.Teams.Count,
                $"[DivisionId={x.DivisionId}] Number of team entries does not match.");

            if (x.Teams.Count == y.Teams.Count)
            {
                for (var i = 0; i < x.Teams.Count; i++)
                {
                    if (teamsComparer.Compare(x.Teams[i], y.Teams[i]) != 0)
                    {
                        return 1;
                    }
                }
            }

            if (hasComparerByGames)
            {
                x.GameResults.Count.Should().Be(y.GameResults.Count,
                    $"[DivisionId={x.DivisionId}] Number of game result entries does not match.");
                if (x.GameResults.Count == y.GameResults.Count)
                {
                    var gameResultComparer = new ShortGameResultDtoComparer();
                    for (var i = 0; i < x.GameResults.Count; i++)
                    {
                        if (gameResultComparer.Compare(x.GameResults[i], y.GameResults[i]) != 0)
                        {
                            return 1;
                        }
                    }
                }
            }
            return 0;
        }

        public bool Equals(PivotStandingsDto x, PivotStandingsDto y)
        {
            return Compare(x, y) == 0;
        }

        public int GetHashCode(PivotStandingsDto obj)
        {
            return obj.DivisionId.GetHashCode();
        }
    }
}