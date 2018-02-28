namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System.Collections.Generic;
    using Domain.GameReportsAggregate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using VolleyManagement.UnitTests.Services.GameReportService.Comparers;

    internal class PivotStandingsComparer : IComparer<PivotStandingsDto>
    {
        private TeamStandingsDtoComparer teamsComparer;
        private ComparerBy comparer;

        public PivotStandingsComparer() : this(ComparerBy.All) { }

        public PivotStandingsComparer(ComparerBy comparer)
        {
            this.teamsComparer = new TeamStandingsDtoComparer(comparer);
            this.comparer = comparer;
        }

        public int Compare(PivotStandingsDto x, PivotStandingsDto y)
        {
            Assert.AreEqual(x.DivisionId, y.DivisionId, "Division Ids do not match");
            Assert.AreEqual(x.DivisionName, y.DivisionName, $"[DivisionId={x.DivisionId}] Division Names do not match");
            Assert.AreEqual(x.LastUpdateTime, y.LastUpdateTime, $"[DivisionId={x.DivisionId}] Last Update time do not match");

            if (comparer == ComparerBy.All || comparer != ComparerBy.Status)
            {
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
            }

            if (comparer == ComparerBy.All || comparer == ComparerBy.Status)
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