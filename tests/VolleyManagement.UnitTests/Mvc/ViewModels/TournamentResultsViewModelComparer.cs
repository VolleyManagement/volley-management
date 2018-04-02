namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using UI.Areas.Mvc.ViewModels.GameReports;
    using UI.Areas.Mvc.ViewModels.GameResults;

    /// <summary>
    /// Represents a comparer for <see cref="TournamentResultsViewModel"/> objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class TournamentResultsViewModelComparer : IComparer<TournamentResultsViewModel>, IComparer
    {
        /// <summary>
        /// Compares two <see cref="StandingsViewModel"/> objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <see cref="StandingsViewModel"/> x and y.</returns>
        public int Compare(TournamentResultsViewModel x, TournamentResultsViewModel y)
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
            var firstTournamentResultsViewModel = x as TournamentResultsViewModel;
            var secondTournamentResultsViewModel = y as TournamentResultsViewModel;

            if (firstTournamentResultsViewModel == null)
            {
                return -1;
            }
            else if (secondTournamentResultsViewModel == null)
            {
                return 1;
            }

            return Compare(firstTournamentResultsViewModel, secondTournamentResultsViewModel);
        }

        /// <summary>
        /// Finds out whether two <see cref="StandingsViewModel"/> objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given <see cref="StandingsViewModel"/> objects are equal.</returns>
        internal bool AreEqual(TournamentResultsViewModel x, TournamentResultsViewModel y)
        {
            Assert.AreEqual(x.Id, y.Id, "Id should be equal.");
            Assert.AreEqual(x.Name, y.Name, "Name should be equal.");

            return x.GameResults.SequenceEqual(y.GameResults, new GameResultViewModelEqualityComparer());
        }
    }
}
