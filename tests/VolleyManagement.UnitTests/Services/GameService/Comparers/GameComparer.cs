namespace VolleyManagement.UnitTests.Services.GameService
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Domain.GamesAggregate;

    /// <summary>
    /// Represents a comparer for <see cref="Game"/> objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class GameComparer : IComparer<Game>, IComparer, IEqualityComparer<Game>
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
            var firstGameResult = x as Game;
            var secondGameResult = y as Game;

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

        public bool Equals(Game x, Game y)
        {
            return AreEqual(x, y);
        }

        public int GetHashCode(Game obj)
        {
            return obj.Id.GetHashCode();
        }

        /// <summary>
        /// Finds out whether two <see cref="Game"/> objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given <see cref="Game"/> objects are equal.</returns>
        internal bool AreEqual(Game x, Game y)
        {
            var result = x.Id == y.Id
                && x.TournamentId == y.TournamentId
                && x.HomeTeamId == y.HomeTeamId
                && x.AwayTeamId == y.AwayTeamId
                && x.GameDate == y.GameDate
                && x.Round == y.Round
                && x.GameNumber == y.GameNumber;

            if (!result)
            {
                return false;
            }

            if (x.Result == null && y.Result == null)
            {
                return true;
            }

            if (x.Result != null && y.Result != null)
            {
                result = new ScoreComparer().Equals(x.Result.GameScore, y.Result.GameScore)
                         && x.Result.GameScore.IsTechnicalDefeat == y.Result.GameScore.IsTechnicalDefeat
                         && x.Result.SetScores.SequenceEqual(y.Result.SetScores, new ScoreComparer())
                         && PenaltiesAreEqual(x.Result.Penalty, y.Result.Penalty);
            }
            else
            {
                result = false;
            }

            return result;
        }

        private bool PenaltiesAreEqual(Penalty x, Penalty y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            if (ReferenceEquals(x, y))
            {
                return true;
            }

            return x.IsHomeTeam == y.IsHomeTeam
                   && x.Amount == y.Amount
                   && string.Compare(x.Description, y.Description, StringComparison.Ordinal) == 0;
        }
    }
}
