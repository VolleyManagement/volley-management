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

            TestHelper.AreEqual(x.Standings, y.Standings, new StandingsEntryComparer());

            return 0;
        }
    }
}