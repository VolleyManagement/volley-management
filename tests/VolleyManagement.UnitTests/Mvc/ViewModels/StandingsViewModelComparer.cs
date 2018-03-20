namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using UI.Areas.Mvc.ViewModels.GameReports;

    /// <summary>
    /// Represents a comparer for <see cref="StandingsViewModel"/> objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class StandingsViewModelComparer : IComparer<StandingsViewModel>, IComparer
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

        /// <summary>
        /// Finds out whether two <see cref="StandingsViewModel"/> objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given <see cref="StandingsViewModel"/> objects are equal.</returns>
        internal bool AreEqual(StandingsViewModel expected, StandingsViewModel actual)
        {
            Assert.AreEqual(expected.TournamentId, actual.TournamentId, "TournamentId should match");
            Assert.AreEqual(expected.TournamentName, actual.TournamentName, "TournamentName should match");

            Assert.AreEqual(expected.Message, actual.Message, "Message should match");

            if (expected.StandingsTable != null || actual.StandingsTable != null)
            {
                if (expected.StandingsTable == null || actual.StandingsTable == null)
                {
                    Assert.Fail("One of the Standings colection is null");
                }

                Assert.AreEqual(expected.StandingsTable.Count, actual.StandingsTable.Count, "Number of Standings divisions should match");

                for (var i = 0; i < expected.StandingsTable.Count; i++)
                {
                    var expectedStandings = expected.StandingsTable[i];
                    var actualStandings = actual.StandingsTable[i];

                    Assert.AreEqual(expectedStandings.LastUpdateTime, actualStandings.LastUpdateTime, $"[Div#{i}] LastTimeUpdated should match");

                    Assert.AreEqual(actualStandings.StandingsEntries.Count, actualStandings.StandingsEntries.Count, $"[Div#{i}] Number of Standings should match");
                    for (var j = 0; j < actualStandings.StandingsEntries.Count; j++)
                    {
                        Assert.IsTrue(StandingsEntryViewModelEqualityComparer.AssertAreEqual(
                            expectedStandings.StandingsEntries[j],
                            actualStandings.StandingsEntries[j],
                            $"[Div#{i}][Standings[{j}]] "));
                    }
                }
            }

            if (expected.PivotTable != null || actual.PivotTable != null)
            {
                if (expected.PivotTable == null || actual.PivotTable == null)
                {
                    Assert.Fail("One of the PivotTable colection is null");
                }

                Assert.AreEqual(expected.PivotTable.Count, actual.PivotTable.Count, "Number of PivotTable divisions should match");

                for (var i = 0; i < expected.PivotTable.Count; i++)
                {
                    var expectedPivot = expected.PivotTable[i];
                    var actualPivot = actual.PivotTable[i];

                    Assert.AreEqual(expectedPivot.LastUpdateTime, actualPivot.LastUpdateTime, $"[Div#{i}] LastTimeUpdated should match");

                    Assert.AreEqual(expectedPivot.TeamsStandings.Count, actualPivot.TeamsStandings.Count, $"[Div#{i}] Number of teams in pivot table should match");

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
