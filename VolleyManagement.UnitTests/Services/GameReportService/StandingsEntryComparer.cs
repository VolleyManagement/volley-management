namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.GameReportsAggregate;

    /// <summary>
    /// Represents a comparer for <see cref="StandingsEntry"/> objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class StandingsEntryComparer : IComparer<StandingsEntry>, IComparer
    {
        /// <summary>
        /// Compares two <see cref="StandingsEntry"/> objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <see cref="StandingsEntry"/> x and y.</returns>
        public int Compare(StandingsEntry x, StandingsEntry y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two <see cref="StandingsEntry"/> objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <see cref="StandingsEntry"/> x and y.</returns>
        public int Compare(object x, object y)
        {
            StandingsEntry firstStandingsEntry = x as StandingsEntry;
            StandingsEntry secondStandingsEntry = y as StandingsEntry;

            if (firstStandingsEntry == null)
            {
                return -1;
            }
            else if (secondStandingsEntry == null)
            {
                return 1;
            }

            return Compare(firstStandingsEntry, secondStandingsEntry);
        }

        /// <summary>
        /// Finds out whether two <see cref="StandingsEntry"/> objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given <see cref="StandingsEntry"/> objects are equal.</returns>
        internal bool AreEqual(StandingsEntry x, StandingsEntry y)
        {
            return x.TeamName == y.TeamName
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
