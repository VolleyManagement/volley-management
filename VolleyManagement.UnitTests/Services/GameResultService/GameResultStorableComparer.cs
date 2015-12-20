namespace VolleyManagement.UnitTests.Services.GameResultService
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using VolleyManagement.Domain.GameResultsAggregate;

    /// <summary>
    /// Represents a comparer for <see cref="GameResultStorable"/> objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class GameResultStorableComparer : IComparer<GameResultStorable>, IComparer
    {
        /// <summary>
        /// Compares two <see cref="GameResultStorable"/> objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <see cref="GameResultStorable"/> x and y.</returns>
        public int Compare(GameResultStorable x, GameResultStorable y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two <see cref="GameResultStorable"/> objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <see cref="GameResultStorable"/> x and y.</returns>
        public int Compare(object x, object y)
        {
            GameResultStorable firstGameResult = x as GameResultStorable;
            GameResultStorable secondGameResult = y as GameResultStorable;

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
        /// Finds out whether two <see cref="GameResultStorable"/> objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given <see cref="GameResultStorable"/> objects are equal.</returns>
        internal bool AreEqual(GameResultStorable x, GameResultStorable y)
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
