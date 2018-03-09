namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using UI.Areas.WebApi.ViewModels.GameReports;

    /// <summary>
    /// Represents an equality comparer for <see cref="PivotStandingsViewModel"/> objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class PivotStandingsViewModelComparer : IComparer<PivotStandingsViewModel>
    {
        public int Compare(PivotStandingsViewModel expected, PivotStandingsViewModel actual)
        {
            if (expected != null || actual != null)
            {
                if (expected == null || actual == null)
                {
                    Assert.Fail("One of the pivot standings object is null");
                }

                Assert.AreEqual(expected.TeamsStandings.Count, actual.TeamsStandings.Count, "Number of Team Standings divisions should match");
                Assert.AreEqual(expected.LastUpdateTime, actual.LastUpdateTime, "LastUpdateTime for division should match");
                Assert.AreEqual(expected.DivisionName, actual.DivisionName, "DivisionName for division should match");


                var expectedTeamsStandingsEnumerator = expected.TeamsStandings.GetEnumerator();
                var actualTeamsStandingsEnumerator = actual.TeamsStandings.GetEnumerator();

                int i = 0;
                while (expectedTeamsStandingsEnumerator.MoveNext() && actualTeamsStandingsEnumerator.MoveNext())
                {
                    PivotStandingsEntryViewModelComparer.AssertAreEqual(expectedTeamsStandingsEnumerator.Current, actualTeamsStandingsEnumerator.Current, $"[Team#{i}] ");
                    i++;
                }

                expectedTeamsStandingsEnumerator.Dispose();
                actualTeamsStandingsEnumerator.Dispose();

                Assert.AreEqual(expected.GamesStandings.Count, actual.GamesStandings.Count, "Number of Games Standings divisions should match");

                var expectedGamesStandingsEnumerator = expected.GamesStandings.GetEnumerator();
                var actualGamesStandingsEnumerator = actual.GamesStandings.GetEnumerator();


                while (expectedGamesStandingsEnumerator.MoveNext() && actualGamesStandingsEnumerator.MoveNext())
                {
                    PivotStandingsGameViewModelComparer.AssertAreEqual(expectedGamesStandingsEnumerator.Current, actualGamesStandingsEnumerator.Current, $"[Game#{i}] ");
                    i++;
                }

                expectedGamesStandingsEnumerator.Dispose();
                actualGamesStandingsEnumerator.Dispose();
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
    }
}
