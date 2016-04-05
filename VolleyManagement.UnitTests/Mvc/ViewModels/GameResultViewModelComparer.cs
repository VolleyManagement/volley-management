namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using UI.Areas.Mvc.ViewModels.GameResults;

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
            return x.Result.SetScores[0].Home == y.Result.SetScores[0].Home
                && x.Result.SetScores[1].Home == y.Result.SetScores[1].Home
                && x.Result.SetScores[2].Home == y.Result.SetScores[2].Home
                && x.Result.SetScores[3].Home == y.Result.SetScores[3].Home
                && x.Result.SetScores[4].Home == y.Result.SetScores[4].Home
                && x.Result.SetScores[0].Away == y.Result.SetScores[0].Away
                && x.Result.SetScores[1].Away == y.Result.SetScores[1].Away
                && x.Result.SetScores[2].Away == y.Result.SetScores[2].Away
                && x.Result.SetScores[3].Away == y.Result.SetScores[3].Away
                && x.Result.SetScores[4].Away == y.Result.SetScores[4].Away
                && x.AwayTeamId == y.AwayTeamId
                && x.Result.SetsScore.Home == y.Result.SetsScore.Home
                && x.Result.SetsScore.Away == y.Result.SetsScore.Away
                && x.HomeTeamId == y.HomeTeamId
                && x.Id == y.Id
                && x.Result.IsTechnicalDefeat == y.Result.IsTechnicalDefeat
                && x.TournamentId == y.TournamentId;
        }
    }
}