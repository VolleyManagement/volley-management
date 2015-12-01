namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using UI.Areas.Mvc.ViewModels.GameResults;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Teams;

    /// <summary>
    /// Comparer for team objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class GameResultViewModelComparer : IComparer<GameResultViewModel>, IComparer
    {
        /// <summary>
        /// Compares two player objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of players.</returns>
        public int Compare(GameResultViewModel x, GameResultViewModel y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two team objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of teams.</returns>
        public int Compare(object x, object y)
        {
            GameResultViewModel firstGameResult = x as GameResultViewModel;
            GameResultViewModel secondGameResult = y as GameResultViewModel;

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
        /// Finds out whether two team objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given team have the same properties.</returns>
        public bool AreEqual(GameResultViewModel x, GameResultViewModel y)
        {
            return x.AwaySet1Score == y.AwaySet1Score
                && x.AwaySet2Score == y.AwaySet2Score
                && x.AwaySet3Score == y.AwaySet3Score
                && x.AwaySet4Score == y.AwaySet4Score
                && x.AwaySet5Score == y.AwaySet5Score
                && x.AwaySetsScore == y.AwaySetsScore
                && x.AwayTeamId == y.AwayTeamId
                && x.HomeSet1Score == y.HomeSet1Score
                && x.HomeSet2Score == y.HomeSet2Score
                && x.HomeSet3Score == y.HomeSet3Score
                && x.HomeSet4Score == y.HomeSet4Score
                && x.HomeSet5Score == y.HomeSet5Score
                && x.HomeSetsScore == y.HomeSetsScore
                && x.HomeTeamId == y.HomeTeamId
                && x.Id == y.Id
                && x.IsTechnicalDefeat == y.IsTechnicalDefeat
                && x.TournamentId == y.TournamentId;
        }
    }
}