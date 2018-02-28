namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System.Collections.Generic;
    using Domain.GameReportsAggregate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    internal class PivotStandingsComparer : IComparer<PivotStandingsDto>
    {
        private TeamStandingsDtoComparer teamsComparer;
        private bool hasComparerByResult;

        public PivotStandingsComparer()
        {
            this.teamsComparer = new TeamStandingsDtoComparer();
            hasComparerByResult = true;
        }
        public void WithPointsComparer()
        {
            teamsComparer.HasComparerBySets = false;
            teamsComparer.HasComparerByBalls = false;
            hasComparerByResult = false;
        }
        public void WithSetRatioComparer()
        {
            teamsComparer.HasComparerByPoints = false;
            teamsComparer.HasComparerByBalls = false;
            hasComparerByResult = false;
        }
        public void WithBallsRatioComparer()
        {
            teamsComparer.HasComparerByPoints = false;
            teamsComparer.HasComparerBySets = false;
            hasComparerByResult = false;
        }
        public void WithGameResultComparer()
        {
            teamsComparer.HasComparerByPoints = false;
            teamsComparer.HasComparerBySets = false;
            teamsComparer.HasComparerByBalls = false;
        }
       
        public int Compare(PivotStandingsDto x, PivotStandingsDto y)
        {
            Assert.AreEqual(x.DivisionId, y.DivisionId, "Division Ids do not match");
            Assert.AreEqual(x.DivisionName, y.DivisionName, $"[DivisionId={x.DivisionId}] Division Names do not match");
            Assert.AreEqual(x.LastUpdateTime, y.LastUpdateTime, $"[DivisionId={x.DivisionId}] Last Update time do not match");

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
            else
            {
                Assert.Fail($"[DivisionId={x.DivisionId}] Number of team entries does not match.");
            }

            if (hasComparerByResult)
            {
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
                else
                {
                    Assert.Fail($"[DivisionId={x.DivisionId}] Number of game result entries does not match.");
                }
            }
            return 0;
        }
    }
}