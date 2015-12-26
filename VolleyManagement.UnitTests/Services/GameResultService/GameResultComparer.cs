namespace VolleyManagement.UnitTests.Services.GameResultService
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using VolleyManagement.Domain.GameResultsAggregate;

    /// <summary>
    /// Represents a comparer for <see cref="GameResult"/> objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class GameResultComparer : IComparer<GameResult>, IComparer
    {
        /// <summary>
        /// Compares two <see cref="GameResult"/> objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <see cref="GameResult"/> x and y.</returns>
        public int Compare(GameResult x, GameResult y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two <see cref="GameResult"/> objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <see cref="GameResult"/> x and y.</returns>
        public int Compare(object x, object y)
        {
            GameResult firstGameResult = x as GameResult;
            GameResult secondGameResult = y as GameResult;

            if (firstGameResult == null)
            {
                return -1;
            }
            else if (secondGameResult == null)
            {
                return 1;
            }

            return Compare(firstGameResult, secondGameResult);
        }

        /// <summary>
        /// Finds out whether two <see cref="GameResult"/> objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given <see cref="GameResult"/> objects are equal.</returns>
        internal bool AreEqual(GameResult x, GameResult y)
        {
            return x.Id == y.Id
                && x.TournamentId == y.TournamentId
                && x.HomeTeamId == y.HomeTeamId
                && x.AwayTeamId == y.AwayTeamId
                && new ScoreComparer().Equals(x.SetsScore, y.SetsScore)
                && x.IsTechnicalDefeat == y.IsTechnicalDefeat
                && x.SetScores.SequenceEqual(y.SetScores, new ScoreComparer());
        }
    }
}
