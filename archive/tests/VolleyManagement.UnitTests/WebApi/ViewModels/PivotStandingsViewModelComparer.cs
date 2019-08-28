namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Xunit;
    using UI.Areas.WebApi.ViewModels.GameReports;
    using FluentAssertions;

    /// <summary>
    /// Represents an equality comparer for <see cref="PivotStandingsViewModel"/> objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class PivotStandingsViewModelComparer : IComparer<PivotStandingsViewModel>, IEqualityComparer<PivotStandingsViewModel>
    {
        public int Compare(PivotStandingsViewModel expected, PivotStandingsViewModel actual)
        {
            if (expected != null || actual != null)
            {
                expected.Should().NotBeNull("One of the pivot standings object is null");
                actual.Should().NotBeNull("One of the pivot standings object is null");

                actual.TeamsStandings.Count.Should().Be(expected.TeamsStandings.Count, "Number of Team Standings divisions should match");
                actual.LastUpdateTime.Should().Be(expected.LastUpdateTime, "LastUpdateTime for division should match");
                actual.DivisionName.Should().Be(expected.DivisionName, "DivisionName for division should match");

                Assert.Equal(expected.TeamsStandings, actual.TeamsStandings, new PivotStandingsEntryViewModelComparer());

                actual.GamesStandings.Count.Should().Be(expected.GamesStandings.Count, "Number of Games Standings divisions should match");

                Assert.Equal(expected.GamesStandings, actual.GamesStandings, new PivotStandingsGameViewModelComparer());

            }

            return 0;
        }

        /// <summary>
        /// Compares two pivot standing entries objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of entries.</returns>
        public int Compare(object x, object y)
        {
            var expected = x as PivotStandingsViewModel;
            var actual = y as PivotStandingsViewModel;

            return Compare(expected, actual);
        }

        public bool Equals(PivotStandingsViewModel x, PivotStandingsViewModel y)
        {
            return Compare(x, y) == 0;
        }

        public int GetHashCode(PivotStandingsViewModel obj)
        {
            return obj.DivisionName.GetHashCode();
        }
    }
}
