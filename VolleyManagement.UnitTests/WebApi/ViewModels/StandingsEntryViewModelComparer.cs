namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using UI.Areas.WebApi.ViewModels.GameReports;

    /// <summary>
    /// Represents an equality comparer for <see cref="StandingsEntryViewModel"/> objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class StandingsEntryViewModelComparer : IComparer<StandingsEntryViewModel>, IComparer
    {
        /// <summary>
        /// Compares two standing entries objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of entries.</returns>
        public int Compare(StandingsEntryViewModel x, StandingsEntryViewModel y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two standing entries objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of entries.</returns>
        public int Compare(object x, object y)
        {
            StandingsEntryViewModel firstEntry = x as StandingsEntryViewModel;
            StandingsEntryViewModel secondEntry = y as StandingsEntryViewModel;

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

        /// <summary>
        /// Finds out whether two standings entries objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given entries have the same properties.</returns>
        private bool AreEqual(StandingsEntryViewModel x, StandingsEntryViewModel y)
        {
            return x.TeamName == y.TeamName
                && x.Position == y.Position
                && x.Points == y.Points
                && x.GamesTotal == y.GamesTotal
                && x.GamesWon == y.GamesWon
                && x.GamesLost == y.GamesLost
                && x.GamesWithScoreThreeNil == y.GamesWithScoreThreeNil
                && x.GamesWithScoreThreeOne == y.GamesWithScoreThreeOne
                && x.GamesWithScoreThreeTwo == y.GamesWithScoreThreeTwo
                && x.GamesWithScoreTwoThree == y.GamesWithScoreTwoThree
                && x.GamesWithScoreOneThree == y.GamesWithScoreOneThree
                && x.GamesWithScoreNilThree == y.GamesWithScoreNilThree
                && x.SetsWon == y.SetsWon
                && x.SetsLost == y.SetsLost
                && x.SetsRatio == y.SetsRatio
                && x.BallsWon == y.BallsWon
                && x.BallsLost == y.BallsLost
                && x.BallsRatio == y.BallsRatio;
        }
    }
}
