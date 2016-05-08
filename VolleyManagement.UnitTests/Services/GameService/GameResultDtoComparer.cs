namespace VolleyManagement.UnitTests.Services.GameService
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.GamesAggregate;

    /// <summary>
    /// Represents a comparer for <see cref="GameResultDto"/> objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class GameResultDtoComparer : IComparer<GameResultDto>, IComparer
    {
        /// <summary>
        /// Compares two <see cref="GameResultDto"/> objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <see cref="GameResultDto"/> x and y.</returns>
        public int Compare(GameResultDto x, GameResultDto y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two <see cref="GameResultDto"/> objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <see cref="GameResultDto"/> x and y.</returns>
        public int Compare(object x, object y)
        {
            GameResultDto firstGameResult = x as GameResultDto;
            GameResultDto secondGameResult = y as GameResultDto;

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
        /// Finds out whether two <see cref="GameResultDto"/> objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given <see cref="GameResultDto"/> objects are equal.</returns>
        internal bool AreEqual(GameResultDto x, GameResultDto y)
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
                && x.AwaySet5Score == y.AwaySet5Score
                && x.GameDate == y.GameDate
                && x.Round == y.Round
                && x.GameNumber == y.GameNumber;
        }
    }
}
