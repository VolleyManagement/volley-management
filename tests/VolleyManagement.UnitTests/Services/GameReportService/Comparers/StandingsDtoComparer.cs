namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System.Collections.Generic;
    using Domain.GameReportsAggregate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class StandingsDtoComparer : IComparer<StandingsDto>
    {
        public int Compare(StandingsDto x, StandingsDto y)
        {
            Assert.AreEqual(x.DivisionId, y.DivisionId, "Division Ids do not match");
            Assert.AreEqual(x.DivisionName, y.DivisionName, $"[DivisionId={x.DivisionId}] Division Names do not match");
            Assert.AreEqual(x.LastUpdateTime, y.LastUpdateTime, $"[DivisionId={x.DivisionId}] Last Update time do not match");

            if (x.Standings.Count == y.Standings.Count)
            {
                var standingsComparer = new StandingsEntryComparer();
                for (var i = 0; i < x.Standings.Count; i++)
                {
                    if (standingsComparer.Compare(x.Standings[i], y.Standings[i]) != 0)
                    {
                        return 1;
                    }
                }
            }
            else
            {
                Assert.Fail($"[DivisionId={x.DivisionId}] Number of standing entries does not match.");
            }

            return 0;
        }
    }
}