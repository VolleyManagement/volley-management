namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Games;

    /// <summary>
    /// Builder for test game view models
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class GameViewModelComparer : IComparer<GameViewModel>, IComparer
    {
        /// <summary>
        /// Compares two game objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of games.</returns>
        public int Compare(GameViewModel x, GameViewModel y)
        {
            return this.AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two game objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of games.</returns>
        public int Compare(object x, object y)
        {
            GameViewModel firstGame = x as GameViewModel;
            GameViewModel secondGame = y as GameViewModel;

            if (firstGame == null)
            {
                return -1;
            }
            else if (secondGame == null)
            {
                return 1;
            }

            return Compare(firstGame, secondGame);
        }

        /// <summary>
        /// Finds out whether two game objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given games have the same properties.</returns>
        private bool AreEqual(GameViewModel x, GameViewModel y)
        {
            return x.AwayTeamName == y.AwayTeamName
                && x.GameDate == y.GameDate
                && x.Id == y.Id
                && x.HomeTeamName == y.HomeTeamName
                && x.Result.SetScores[0].Home == y.Result.SetScores[0].Home
                && x.Result.SetScores[1].Home == y.Result.SetScores[1].Home
                && x.Result.SetScores[2].Home == y.Result.SetScores[2].Home
                && x.Result.SetScores[3].Home == y.Result.SetScores[3].Home
                && x.Result.SetScores[4].Home == y.Result.SetScores[4].Home
                && x.Result.SetScores[0].Away == y.Result.SetScores[0].Away
                && x.Result.SetScores[1].Away == y.Result.SetScores[1].Away
                && x.Result.SetScores[2].Away == y.Result.SetScores[2].Away
                && x.Result.SetScores[3].Away == y.Result.SetScores[3].Away
                && x.Result.SetScores[4].Away == y.Result.SetScores[4].Away
                && x.Result.TotalScore.Home == y.Result.TotalScore.Home
                && x.Result.TotalScore.Away == y.Result.TotalScore.Away
                && x.Result.IsTechnicalDefeat == y.Result.IsTechnicalDefeat;
        }
    }
}
