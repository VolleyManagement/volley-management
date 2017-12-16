namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            Assert.AreEqual(x.AwayTeamName, y.AwayTeamName, "Away team name do not match");
            Assert.AreEqual(x.Id, y.Id, "Id of the game do not match");
            Assert.AreEqual(x.Date, y.Date, "Date of game do not match");
            Assert.AreEqual(x.DivisionId, y.DivisionId, "Division id do not match");
            Assert.AreEqual(x.DivisionName, y.DivisionName, "Division name do not match");
            Assert.AreEqual(x.GameDate, y.GameDate, "GameDate do not match");
            Assert.AreEqual(x.GroupId, y.GroupId, "Group id do not match");
            Assert.AreEqual(x.HomeTeamName, y.HomeTeamName, "Home team id do not match");
            Assert.AreEqual(x.Round, y.Round, "Round number do not match");

            return AreResultsEqual(x.Result, y.Result);
        }

        private bool AreResultsEqual(GameViewModel.GameResult x, GameViewModel.GameResult y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            else
            {
                Assert.AreEqual(x.SetScores[0].Home, y.SetScores[0].Home, "Score of 1 set for home team do not match");
                Assert.AreEqual(x.SetScores[1].Home, y.SetScores[1].Home, "Score of 2 set for home team do not match");
                Assert.AreEqual(x.SetScores[2].Home, y.SetScores[2].Home, "Score of 3 set for home team do not match");
                Assert.AreEqual(x.SetScores[3].Home, y.SetScores[3].Home, "Score of 4 set for home team do not match");
                Assert.AreEqual(x.SetScores[4].Home, y.SetScores[4].Home, "Score of 5 set for home team do not match");
                Assert.AreEqual(x.SetScores[0].Away, y.SetScores[0].Away, "Score of 1 set for away team do not match");
                Assert.AreEqual(x.SetScores[1].Away, y.SetScores[1].Away, "Score of 2 set for away team do not match");
                Assert.AreEqual(x.SetScores[2].Away, y.SetScores[2].Away, "Score of 3 set for away team do not match");
                Assert.AreEqual(x.SetScores[3].Away, y.SetScores[3].Away, "Score of 4 set for away team do not match");
                Assert.AreEqual(x.SetScores[4].Away, y.SetScores[4].Away, "Score of 5 set for away team do not match");
                Assert.AreEqual(x.SetScores[0].IsTechnicalDefeat, y.SetScores[0].IsTechnicalDefeat, "Technical defeat in 1 set do not match");
                Assert.AreEqual(x.SetScores[1].IsTechnicalDefeat, y.SetScores[1].IsTechnicalDefeat, "Technical defeat in 2 set do not match");
                Assert.AreEqual(x.SetScores[2].IsTechnicalDefeat, y.SetScores[2].IsTechnicalDefeat, "Technical defeat in 3 set do not match");
                Assert.AreEqual(x.SetScores[3].IsTechnicalDefeat, y.SetScores[3].IsTechnicalDefeat, "Technical defeat in 4 set do not match");
                Assert.AreEqual(x.SetScores[4].IsTechnicalDefeat, y.SetScores[4].IsTechnicalDefeat, "Technical defeat in 5 set do not match");
                Assert.AreEqual(x.TotalScore.Home, y.TotalScore.Home, "Game score for home team do not match");
                Assert.AreEqual(x.TotalScore.Away, y.TotalScore.Away, "Game score for away team do not match");
                Assert.AreEqual(x.IsTechnicalDefeat, y.IsTechnicalDefeat, "Technical defeat in game do not match");
                return true;
            }
        }
    }
}
