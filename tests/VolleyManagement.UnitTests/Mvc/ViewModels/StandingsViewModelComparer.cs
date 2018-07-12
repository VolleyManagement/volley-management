namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Xunit;
    using UI.Areas.Mvc.ViewModels.GameReports;
    using FluentAssertions;

    /// <summary>
    /// Represents a comparer for <see cref="StandingsViewModel"/> objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class StandingsViewModelComparer : IComparer<StandingsViewModel>, IComparer, IEqualityComparer<StandingsViewModel>
    {
        /// <summary>
        /// Compares two <see cref="StandingsViewModel"/> objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <see cref="StandingsViewModel"/> x and y.</returns>
        public int Compare(StandingsViewModel x, StandingsViewModel y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two <see cref="StandingsViewModel"/> objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <see cref="StandingsViewModel"/> x and y.</returns>
        public int Compare(object x, object y)
        {
            var firstStandingsViewModel = x as StandingsViewModel;
            var secondStandingsViewModel = y as StandingsViewModel;

            if (firstStandingsViewModel == null)
            {
                return -1;
            }
            else if (secondStandingsViewModel == null)
            {
                return 1;
            }

            return Compare(firstStandingsViewModel, secondStandingsViewModel);
        }

        public bool Equals(StandingsViewModel x, StandingsViewModel y)
        {
            return AreEqual(x, y);
        }

        public int GetHashCode(StandingsViewModel obj)
        {
            return obj.TournamentId.GetHashCode();
        }

        /// <summary>
        /// Finds out whether two <see cref="StandingsViewModel"/> objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given <see cref="StandingsViewModel"/> objects are equal.</returns>
        internal bool AreEqual(StandingsViewModel expected, StandingsViewModel actual)
        {
            actual.TournamentId.Should().Be(expected.TournamentId, "TournamentId should match");
            actual.TournamentName.Should().Be(expected.TournamentName, "TournamentName should match");

            actual.Message.Should().Be(expected.Message, "Message should match");

            if (expected.StandingsTable != null || actual.StandingsTable != null)
            {
                expected.StandingsTable.Should().NotBeNull("One of the Standings colection is null");
                actual.StandingsTable.Should().NotBeNull("One of the Standings colection is null");

                actual.StandingsTable.Count.Should().Be(expected.StandingsTable.Count, "Number of Standings divisions should match");

                for (var i = 0; i < expected.StandingsTable.Count; i++)
                {
                    var expectedStandings = expected.StandingsTable[i];
                    var actualStandings = actual.StandingsTable[i];

                    actualStandings.LastUpdateTime.Should().Be(expectedStandings.LastUpdateTime, $"[Div#{i}] LastTimeUpdated should match");

                    actualStandings.StandingsEntries.Count.Should().Be(actualStandings.StandingsEntries.Count, $"[Div#{i}] Number of Standings should match");
                    for (var j = 0; j < actualStandings.StandingsEntries.Count; j++)
                    {
                        Assert.True(StandingsEntryViewModelEqualityComparer.AssertAreEqual(
                            expectedStandings.StandingsEntries[j],
                            actualStandings.StandingsEntries[j],
                            $"[Div#{i}][Standings[{j}]] "));
                    }
                }
            }

            if (expected.PivotTable != null || actual.PivotTable != null)
            {
                expected.PivotTable.Should().NotBeNull("One of the PivotTable colection is null");
                actual.PivotTable.Should().NotBeNull("One of the PivotTable colection is null");

                actual.PivotTable.Count.Should().Be(expected.PivotTable.Count, "Number of PivotTable divisions should match");

                for (var i = 0; i < expected.PivotTable.Count; i++)
                {
                    var expectedPivot = expected.PivotTable[i];
                    var actualPivot = actual.PivotTable[i];

                    actualPivot.LastUpdateTime.Should().Be(expectedPivot.LastUpdateTime, $"[Div#{i}] LastTimeUpdated should match");

                    actualPivot.TeamsStandings.Count.Should().Be(expectedPivot.TeamsStandings.Count, $"[Div#{i}] Number of teams in pivot table should match");

                    for (var j = 0; j < expectedPivot.TeamsStandings.Count; j++)
                    {
                        PivotTeamStandingsViewModelEqualityComparer.AssertAreEqual(
                            expectedPivot.TeamsStandings[j],
                            actualPivot.TeamsStandings[j],
                            $"[Div#{i}]Standings:{j}: ");
                    }

                    PivotTableEqualityComparer.AreResultTablesEquals(
                        expectedPivot.AllGameResults,
                        actualPivot,
                        $"[Div#{i}]");
                }
            }

            return true;
        }
    }
}
