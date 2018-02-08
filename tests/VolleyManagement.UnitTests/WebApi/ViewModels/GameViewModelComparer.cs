namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using UI.Areas.WebApi.ViewModels.Games;

    /// <summary>
    /// Builder for test game view models
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal static class GameViewModelComparer
    {
        public static void AssertAreEqual(GameViewModel expected, GameViewModel actual, string messagePrefix = "")
        {
            Assert.AreEqual(expected.AwayTeamName, actual.AwayTeamName, $"{messagePrefix}Away team name do not match");
            Assert.AreEqual(expected.Id, actual.Id, $"{messagePrefix}Id of the game do not match");
            Assert.AreEqual(expected.Date, actual.Date, $"{messagePrefix}Date of game do not match");
            Assert.AreEqual(expected.DivisionId, actual.DivisionId, $"{messagePrefix}Division id do not match");
            Assert.AreEqual(expected.DivisionName, actual.DivisionName, $"{messagePrefix}Division name do not match");
            Assert.AreEqual(expected.GameDate, actual.GameDate, $"{messagePrefix}GameDate do not match");
            Assert.AreEqual(expected.GroupId, actual.GroupId, $"{messagePrefix}Group id do not match");
            Assert.AreEqual(expected.HomeTeamName, actual.HomeTeamName, $"{messagePrefix}Home team id do not match");
            Assert.AreEqual(expected.Round, actual.Round, $"{messagePrefix}Round number do not match");
            Assert.AreEqual(expected.UrlToGameVideo, actual.UrlToGameVideo, $"{messagePrefix}UrlToGameVideo do not match");

            AreResultsEqual(expected.Result, actual.Result, messagePrefix);
        }

        private static void AreResultsEqual(GameViewModel.GameResult expected, GameViewModel.GameResult actual, string messagePrefix = "")
        {
            if (expected == null && actual == null)
            {
            }
            else
            {
                for (var i = 0; i < expected.SetScores.Count; i++)
                {
                    Assert.AreEqual(expected.SetScores[i].Home, actual.SetScores[i].Home, $"{messagePrefix}Score of {i} set for home team do not match");
                    Assert.AreEqual(expected.SetScores[i].Away, actual.SetScores[i].Away, $"{messagePrefix}Score of {i} set for away team do not match");
                    Assert.AreEqual(expected.SetScores[i].IsTechnicalDefeat, actual.SetScores[i].IsTechnicalDefeat, $"{messagePrefix}Technical defeat in {i} set do not match");
                }

                Assert.AreEqual(expected.TotalScore.Home, actual.TotalScore.Home, $"{messagePrefix}Game score for home team do not match");
                Assert.AreEqual(expected.TotalScore.Away, actual.TotalScore.Away, $"{messagePrefix}Game score for away team do not match");
                Assert.AreEqual(expected.IsTechnicalDefeat, actual.IsTechnicalDefeat, $"{messagePrefix}Technical defeat in game do not match");
            }
        }
    }
}
