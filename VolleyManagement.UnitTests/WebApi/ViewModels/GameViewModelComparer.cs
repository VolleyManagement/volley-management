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
                for (var i = 0; i < x.SetScores.Count; i++)
                {
                    Assert.AreEqual(x.SetScores[i].Home, y.SetScores[i].Home, $"Score of {i} set for home team do not match");
                    Assert.AreEqual(x.SetScores[i].Away, y.SetScores[i].Away, $"Score of {i} set for away team do not match");
                    Assert.AreEqual(x.SetScores[i].IsTechnicalDefeat, y.SetScores[i].IsTechnicalDefeat, $"Technical defeat in {i} set do not match");
                }

                Assert.AreEqual(x.TotalScore.Home, y.TotalScore.Home, "Game score for home team do not match");
                Assert.AreEqual(x.TotalScore.Away, y.TotalScore.Away, "Game score for away team do not match");
                Assert.AreEqual(x.IsTechnicalDefeat, y.IsTechnicalDefeat, "Technical defeat in game do not match");
                return true;
            }
        }
    }
}
