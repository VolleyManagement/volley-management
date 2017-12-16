namespace VolleyManagement.UnitTests.WebApi.Standings
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using UI.Areas.WebAPI.ViewModels.GameReports;

    /// <summary>
    /// Represents a comparer for <see cref="DivisionStandingsViewModel"/> objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class DivisionStandingsViewModelComparer : IComparer<DivisionStandingsViewModel>, IComparer
    {
        /// <summary>
        /// Compares two <see cref="DivisionStandingsViewModel"/> objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <see cref="DivisionStandingsViewModel"/> x and y.</returns>
        public int Compare(DivisionStandingsViewModel x, DivisionStandingsViewModel y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two <see cref="DivisionStandingsViewModel"/> objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <see cref="DivisionStandingsViewModel"/> x and y.</returns>
        public int Compare(object x, object y)
        {
            DivisionStandingsViewModel firstStandingsViewModel = x as DivisionStandingsViewModel;
            DivisionStandingsViewModel secondStandingsViewModel = y as DivisionStandingsViewModel;

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
        /// Finds out whether two <see cref="DivisionStandingsViewModel"/> objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given <see cref="DivisionStandingsViewModel"/> objects are equal.</returns>
        internal bool AreEqual(DivisionStandingsViewModel expected, DivisionStandingsViewModel actual)
        {
            Assert.AreEqual(expected.LastUpdateTime, actual.LastUpdateTime, "LastTimeUpdated should match");

            if (expected.StandingsTable != null || actual.StandingsTable != null)
            {
                if (expected.StandingsTable == null || actual.StandingsTable == null)
                {
                    Assert.Fail("One of the Standings colection is null");
                }

                Assert.AreEqual(expected.StandingsTable.Count, actual.StandingsTable.Count, "Number of Standings divisions should match");

                for (var i = 0; i < expected.StandingsTable.Count; i++)
                {
                    Assert.IsTrue(StandingsEntryViewModelEqualityComparer.AssertAreEqual(
                        expected.StandingsTable[i],
                        actual.StandingsTable[i],
                        $"[Standings#{i}] "));
                }
            }

            return true;
        }
    }
}
