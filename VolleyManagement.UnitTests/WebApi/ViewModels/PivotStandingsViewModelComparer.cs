namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.GameReports;

    /// <summary>
    /// Represents an equality comparer for <see cref="PivotStandingsViewModel"/> objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class PivotStandingsViewModelComparer : IComparer<PivotStandingsViewModel>, IComparer
    {
        /// <summary>
        /// Compares two pivot standing entries objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of entries.</returns>
        public int Compare(PivotStandingsViewModel x, PivotStandingsViewModel y)
        {
            return this.AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two pivot standing entries objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of entries.</returns>
        public int Compare(object x, object y)
        {
            PivotStandingsViewModel firstEntry = x as PivotStandingsViewModel;
            PivotStandingsViewModel secondEntry = y as PivotStandingsViewModel;

            if (firstEntry == null)
            {
                return -1;
            }
            else if (secondEntry == null)
            {
                return 1;
            }

            return Compare(firstEntry, secondEntry);
        }

        private bool AreEqual(PivotStandingsViewModel x, PivotStandingsViewModel y)
        {
            return x.GamesStandings.SequenceEqual(y.GamesStandings, new PivotStandingsGameViewModelComparer())
                && x.TeamsStandings.SequenceEqual(y.TeamsStandings, new PivotStandingsEntryViewModelComparer());
        }
    }
}
