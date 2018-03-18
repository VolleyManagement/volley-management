namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Comparers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Services.GameService;
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
            var firstGameResult = x as GameResultViewModel;
            var secondGameResult = y as GameResultViewModel;

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
            Assert.AreEqual(x.Id, y.Id, "Name should be equal.");
            Assert.AreEqual(x.TournamentId, y.TournamentId, "Name should be equal.");

            Assert.AreEqual(x.AwayTeamId, y.AwayTeamId, "AwayTeamId should be equal.");
            Assert.AreEqual(x.HomeTeamId, y.HomeTeamId, "HomeTeamId should be equal.");
            Assert.AreEqual(x.AwayTeamName, y.AwayTeamName, "AwayTeamName should be equal.");
            Assert.AreEqual(x.HomeTeamName, y.HomeTeamName, "HomeTeamName should be equal.");

            Assert.AreEqual(x.GameDate, y.GameDate, "GameDate should be equal.");
            Assert.AreEqual(x.Round, y.Round, "Round should be equal.");
            Assert.AreEqual(x.DisplayGameNumber, y.DisplayGameNumber, "DisplayGameNumber should be equal.");
            Assert.AreEqual(x.IsTechnicalDefeat, y.IsTechnicalDefeat, "IsTechnicalDefeat should be equal.");

            Assert.AreEqual(x.HasPenalty, y.HasPenalty, "HasPenalty should be equal.");
            Assert.AreEqual(x.IsHomeTeamPenalty, y.IsHomeTeamPenalty, "IsHomeTeamPenalty should be equal.");
            Assert.AreEqual(x.PenaltyAmount, y.PenaltyAmount, "PenaltyAmount should be equal.");
            Assert.AreEqual(x.PenaltyDescrition, y.PenaltyDescrition, "PenaltyDescrition should be equal.");

            Assert.AreEqual(x.UrlToGameVideo, y.UrlToGameVideo, "UrlToGameVideo should be equal.");

            ScoreViewModelComparer.AssertAreEqual(x.GameScore, y.GameScore);
            for (var i = 0; i < 5; i++)
            {
                ScoreViewModelComparer.AssertAreEqual(x.SetScores[i], y.SetScores[i], $"[Set:{i + 1}]");
            }

            return true;
        }
    }
}