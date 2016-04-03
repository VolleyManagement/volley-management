namespace VolleyManagement.UnitTests.Services.GameService
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using VolleyManagement.Domain.GamesAggregate;

    /// <summary>
    /// Represents a comparer for <see cref="Game"/> objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class GameComparer : IComparer<Game>, IComparer
    {
        /// <summary>
        /// Compares two <see cref="Game"/> objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <see cref="Game"/> x and y.</returns>
        public int Compare(Game x, Game y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two <see cref="Game"/> objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <see cref="Game"/> x and y.</returns>
        public int Compare(object x, object y)
        {
            Game firstGameResult = x as Game;
            Game secondGameResult = y as Game;

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
        /// Finds out whether two <see cref="Game"/> objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given <see cref="Game"/> objects are equal.</returns>
        internal bool AreEqual(Game x, Game y)
        {
            return x.Id == y.Id
                && x.TournamentId == y.TournamentId
                && x.HomeTeamId == y.HomeTeamId
                && x.AwayTeamId == y.AwayTeamId
                && new ScoreComparer().Equals(x.Result.SetsScore, y.Result.SetsScore)
                && x.Result.IsTechnicalDefeat == y.Result.IsTechnicalDefeat
                && x.Result.SetScores.SequenceEqual(y.Result.SetScores, new ScoreComparer())
                && x.GameDate == y.GameDate
                && x.Round == y.Round;
        }
    }
}
