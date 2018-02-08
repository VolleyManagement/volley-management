namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.GameReportsAggregate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            Assert.AreEqual(x.TeamName, y.TeamName, "TeamNames do not match");

            Assert.AreEqual(x.Points, y.Points, "Points do not match");

            Assert.AreEqual(x.GamesTotal, y.GamesTotal, "GamesTotal do not match");
            Assert.AreEqual(x.GamesWon, y.GamesWon, "GamesWon do not match");
            Assert.AreEqual(x.GamesLost, y.GamesLost, "GamesLost do not match");

            Assert.AreEqual(x.GamesWithScoreThreeNil, y.GamesWithScoreThreeNil, "GamesWithScoreThreeNil do not match");
            Assert.AreEqual(x.GamesWithScoreThreeOne, y.GamesWithScoreThreeOne, "GamesWithScoreThreeOne do not match");
            Assert.AreEqual(x.GamesWithScoreThreeTwo, y.GamesWithScoreThreeTwo, "GamesWithScoreThreeTwo do not match");
            Assert.AreEqual(x.GamesWithScoreTwoThree, y.GamesWithScoreTwoThree, "GamesWithScoreTwoThree do not match");
            Assert.AreEqual(x.GamesWithScoreOneThree, y.GamesWithScoreOneThree, "GamesWithScoreOneThree do not match");
            Assert.AreEqual(x.GamesWithScoreNilThree, y.GamesWithScoreNilThree, "GamesWithScoreNilThree do not match");

            Assert.AreEqual(x.SetsWon, y.SetsWon, "SetsWon do not match");
            Assert.AreEqual(x.SetsLost, y.SetsLost, "SetsLost do not match");

            Assert.AreEqual(x.BallsWon, y.BallsWon, "BallsWon do not match");
            Assert.AreEqual(x.BallsLost, y.BallsLost, "BallsLost do not match");

            return 0;
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
    }
}
