namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System.Collections.Generic;
    using Domain.GameReportsAggregate;
    using Xunit;
    using FluentAssertions;

    public class StandingsDtoComparer : IComparer<StandingsDto>, IEqualityComparer<StandingsDto>
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
            y.DivisionId.Should().Be(x.DivisionId, "Division Ids do not match");
            y.DivisionName.Should().Be(x.DivisionName, $"[DivisionId={x.DivisionId}] Division Names do not match");
            y.LastUpdateTime.Should().Be(x.LastUpdateTime, $"[DivisionId={x.DivisionId}] Last Update time do not match");

            Assert.Equal(x.Standings, y.Standings, new StandingsEntryComparer());

            return 0;
        }

        public bool Equals(StandingsDto x, StandingsDto y)
        {
            return Compare(x, y) == 0;
        }

        public int GetHashCode(StandingsDto obj)
        {
            return obj.DivisionId.GetHashCode();
        }
    }
}