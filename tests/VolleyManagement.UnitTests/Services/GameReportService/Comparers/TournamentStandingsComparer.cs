namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System.Collections.Generic;
    using Domain.GameReportsAggregate;
    using Xunit;
    using FluentAssertions;
    using System.Linq;

    public class TournamentStandingsComparer<T> : IEqualityComparer<TournamentStandings<T>>
    {
        private readonly IEqualityComparer<T> _groupItemComparer;

        public TournamentStandingsComparer(IEqualityComparer<T> groupItemComparer)
        {
            _groupItemComparer = groupItemComparer;
        }

        public int Compare(TournamentStandings<T> x, TournamentStandings<T> y)
        {
            if (x == null && y == null)
            {
                return 0;
            }

            x.Should().NotBeNull("One instance is null");
            y.Should().NotBeNull("One instance is null");

            Assert.Equal(x.Divisions, y.Divisions, _groupItemComparer);

            return 0;
        }

        public bool Equals(TournamentStandings<T> x, TournamentStandings<T> y)
        {
            return Compare(x, y) == 0;
        }

        public int GetHashCode(TournamentStandings<T> obj)
        {
            return obj.Divisions.First().GetHashCode();
        }
    }
}