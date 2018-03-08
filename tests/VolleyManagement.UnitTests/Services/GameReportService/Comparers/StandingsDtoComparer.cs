namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System.Collections.Generic;
    using Domain.GameReportsAggregate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class StandingsDtoComparer : IComparer<StandingsDto>
    {
        private StandingsEntryComparer standingsComparer;

        public StandingsDtoComparer() : this(new StandingsEntryComparer())
        {
        }

        internal StandingsDtoComparer(StandingsEntryComparer standingsComparer)
        {
            this.standingsComparer = standingsComparer;
        }

        public int Compare(StandingsDto x, StandingsDto y)
        {
            Assert.AreEqual(x.DivisionId, y.DivisionId, "Division Ids do not match");
            Assert.AreEqual(x.DivisionName, y.DivisionName, $"[DivisionId={x.DivisionId}] Division Names do not match");
            Assert.AreEqual(x.LastUpdateTime, y.LastUpdateTime, $"[DivisionId={x.DivisionId}] Last Update time do not match");

            if (x.Standings.Count == y.Standings.Count)
            {
                var xEnumerator = x.Standings.GetEnumerator();
                var yEnumerator = x.Standings.GetEnumerator();

                while (xEnumerator.MoveNext() && yEnumerator.MoveNext())
                {
                    if (standingsComparer.Compare(xEnumerator.Current, yEnumerator.Current) != 0)
                    {
                        return 1;
                    }
                }

                xEnumerator.Dispose();
                yEnumerator.Dispose();
            }
            else
            {
                Assert.Fail($"[DivisionId={x.DivisionId}] Number of standing entries does not match.");
            }

            return 0;
        }
    }
}