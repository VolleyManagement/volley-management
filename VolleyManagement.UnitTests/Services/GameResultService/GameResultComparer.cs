namespace VolleyManagement.UnitTests.Services.GameResultService
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
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
            ScoreComparer scoreComparer = new ScoreComparer();
            bool areEqual = x.Id == y.Id
                && x.TournamentId == y.TournamentId
                && x.HomeTeamId == y.HomeTeamId
                && x.AwayTeamId == y.AwayTeamId
                && scoreComparer.AreEqual(x.SetsScore, y.SetsScore)
                && x.IsTechnicalDefeat == y.IsTechnicalDefeat;

            if (areEqual)
            {
                if ((x.SetScores == null && y.SetScores != null)
                    || (x.SetScores != null && y.SetScores == null)
                    || (x.SetScores.Count != y.SetScores.Count))
                {
                    areEqual = false;
                }
            }

            if (areEqual && x.SetScores != null)
            {
                foreach (var xScore in x.SetScores)
                {
                    bool scoreEquals = false;

                    foreach (var yScore in y.SetScores)
                    {
                        if (scoreComparer.AreEqual(xScore, yScore))
                        {
                            scoreEquals = true;
                        }
                    }

                    if (!scoreEquals)
                    {
                        areEqual = false;
                        break;
                    }
                }
            }

            return areEqual;
        }
    }
}
