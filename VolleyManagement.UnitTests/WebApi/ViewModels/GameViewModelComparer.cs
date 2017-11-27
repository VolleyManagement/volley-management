namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using UI.Areas.WebApi.ViewModels.Games;

    /// <summary>
    /// Builder for test game view models
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class GameViewModelComparer : IComparer<GameViewModel>, IComparer, IEqualityComparer<GameViewModel>
    {
        /// <summary>
        /// Compares two game objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of games.</returns>
        public int Compare(GameViewModel x, GameViewModel y)
        {
            return AreEqual(x, y) ? 0 : 1;
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
        public bool Equals(GameViewModel x, GameViewModel y)
        {
            return AreEqual(x, y);
        }

        /// <summary>
        /// Get hashcode of given object
        /// </summary>
        /// <param name="obj">Instance of <see cref="GameViewModel"/> to get hashcode </param>
        /// <returns>hashcode</returns>
        public int GetHashCode(GameViewModel obj)
        {
            return obj.Id.GetHashCode();
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
                && AreEqualResult(x.Result, y.Result);
        }

        private bool AreEqualResult(GameViewModel.GameResult x, GameViewModel.GameResult y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            else
            {
                return
                    x.SetScores[0].Home == y.SetScores[0].Home
                    && x.SetScores[1].Home == y.SetScores[1].Home
                    && x.SetScores[2].Home == y.SetScores[2].Home
                    && x.SetScores[3].Home == y.SetScores[3].Home
                    && x.SetScores[4].Home == y.SetScores[4].Home
                    && x.SetScores[0].Away == y.SetScores[0].Away
                    && x.SetScores[1].Away == y.SetScores[1].Away
                    && x.SetScores[2].Away == y.SetScores[2].Away
                    && x.SetScores[3].Away == y.SetScores[3].Away
                    && x.SetScores[4].Away == y.SetScores[4].Away
                    && x.SetScores[0].IsTechnicalDefeat == y.SetScores[0].IsTechnicalDefeat
                    && x.SetScores[1].IsTechnicalDefeat == y.SetScores[1].IsTechnicalDefeat
                    && x.SetScores[2].IsTechnicalDefeat == y.SetScores[2].IsTechnicalDefeat
                    && x.SetScores[3].IsTechnicalDefeat == y.SetScores[3].IsTechnicalDefeat
                    && x.SetScores[4].IsTechnicalDefeat == y.SetScores[4].IsTechnicalDefeat
                    && x.TotalScore.Home == y.TotalScore.Home
                    && x.TotalScore.Away == y.TotalScore.Away
                    && x.IsTechnicalDefeat == y.IsTechnicalDefeat;
            }
        }
    }
}
