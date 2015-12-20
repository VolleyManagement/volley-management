namespace VolleyManagement.UnitTests.Services.GameResultService
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.GameResultsAggregate;

    /// <summary>
    /// Represents a comparer for <see cref="GameResultRetrievable"/> objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class GameResultRetrievableComparer : IComparer<GameResultRetrievable>, IComparer
    {
        /// <summary>
        /// Compares two <see cref="GameResultRetrievable"/> objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <see cref="GameResultRetrievable"/> x and y.</returns>
        public int Compare(GameResultRetrievable x, GameResultRetrievable y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two <see cref="GameResultRetrievable"/> objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <see cref="GameResultRetrievable"/> x and y.</returns>
        public int Compare(object x, object y)
        {
            GameResultRetrievable firstGameResult = x as GameResultRetrievable;
            GameResultRetrievable secondGameResult = y as GameResultRetrievable;

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
        /// Finds out whether two <see cref="GameResultRetrievable"/> objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given <see cref="GameResultRetrievable"/> objects are equal.</returns>
        internal bool AreEqual(GameResultRetrievable x, GameResultRetrievable y)
        {
            return x.Id == y.Id
                && x.TournamentId == y.TournamentId
                && x.HomeTeamId == y.HomeTeamId
                && x.AwayTeamId == y.AwayTeamId
                && x.HomeTeamName == y.HomeTeamName
                && x.AwayTeamName == y.AwayTeamName
                && x.HomeSetsScore == y.HomeSetsScore
                && x.AwaySetsScore == y.AwaySetsScore
                && x.IsTechnicalDefeat == y.IsTechnicalDefeat
                && x.HomeSet1Score == y.HomeSet1Score
                && x.AwaySet1Score == y.AwaySet1Score
                && x.HomeSet2Score == y.HomeSet2Score
                && x.AwaySet2Score == y.AwaySet2Score
                && x.HomeSet3Score == y.HomeSet3Score
                && x.AwaySet3Score == y.AwaySet3Score
                && x.HomeSet4Score == y.HomeSet4Score
                && x.AwaySet4Score == y.AwaySet4Score
                && x.HomeSet5Score == y.HomeSet5Score
                && x.AwaySet5Score == y.AwaySet5Score;
        }
    }
}
