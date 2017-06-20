namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.GameResults;

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
            return x.SetScores[0].Home == y.SetScores[0].Home
                && x.SetScores[1].Home == y.SetScores[1].Home
                && x.SetScores[2].Home == y.SetScores[2].Home
                && x.SetScores[3].Home == y.SetScores[3].Home
                && x.SetScores[4].Home == y.SetScores[4].Home
                && x.SetScores[0].Away == y.SetScores[0].Away
                && x.SetScores[1].Away == y.SetScores[1].Away
                && x.SetScores[2].Away == y.SetScores[2].Away
                && x.SetScores[3].Away == y.SetScores[3].Away
                && x.SetScores[4].Away == y.SetScores[4].Away
                && x.AwayTeamId == y.AwayTeamId
                && x.SetsScore.Home == y.SetsScore.Home
                && x.SetsScore.Away == y.SetsScore.Away
                && x.HomeTeamId == y.HomeTeamId
                && x.Id == y.Id
                && x.IsTechnicalDefeat == y.IsTechnicalDefeat
                && x.TournamentId == y.TournamentId
                && x.GameDate == y.GameDate
                && x.Round == y.Round
                && x.HomeTeamName == y.HomeTeamName
                && y.AwayTeamName == y.AwayTeamName;
        }
    }
}