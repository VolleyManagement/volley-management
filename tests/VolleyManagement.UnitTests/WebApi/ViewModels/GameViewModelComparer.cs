namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using UI.Areas.WebApi.ViewModels.Games;
    using FluentAssertions;

    /// <summary>
    /// Builder for test game view models
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal static class GameViewModelComparer
    {
        public static void AssertAreEqual(GameViewModel expected, GameViewModel actual, string messagePrefix = "")
        {
            actual.AwayTeamName.Should().Be(expected.AwayTeamName, $"{messagePrefix}Away team name do not match");
            actual.Id.Should().Be(expected.Id, $"{messagePrefix}Id of the game do not match");
            actual.Date.Should().Be(expected.Date, $"{messagePrefix}Date of game do not match");
            actual.DivisionId.Should().Be(expected.DivisionId, $"{messagePrefix}Division id do not match");
            actual.DivisionName.Should().Be(expected.DivisionName, $"{messagePrefix}Division name do not match");
            actual.GameDate.Should().Be(expected.GameDate, $"{messagePrefix}GameDate do not match");
            actual.GroupId.Should().Be(expected.GroupId, $"{messagePrefix}Group id do not match");
            actual.HomeTeamName.Should().Be(expected.HomeTeamName, $"{messagePrefix}Home team id do not match");
            actual.Round.Should().Be(expected.Round, $"{messagePrefix}Round number do not match");
            actual.UrlToGameVideo.Should().Be(expected.UrlToGameVideo, $"{messagePrefix}UrlToGameVideo do not match");

            AreResultsEqual(expected.Result, actual.Result, messagePrefix);
        }

        private static void AreResultsEqual(GameViewModel.GameResult expected, GameViewModel.GameResult actual, string messagePrefix = "")
        {
            if (expected != null || actual != null)
            {
                for (var i = 0; i < expected.SetScores.Count; i++)
                {
                    actual.SetScores[i].Home.Should().Be(expected.SetScores[i].Home,
                        $"{messagePrefix}Score of {i} set for home team do not match");
                    actual.SetScores[i].Away.Should().Be(expected.SetScores[i].Away,
                        $"{messagePrefix}Score of {i} set for away team do not match");
                    actual.SetScores[i].IsTechnicalDefeat.Should().Be(expected.SetScores[i].IsTechnicalDefeat,
                        $"{messagePrefix}Technical defeat in {i} set do not match");
                }

                actual.TotalScore.Home.Should().Be(expected.TotalScore.Home,
                    $"{messagePrefix}Game score for home team do not match");
                actual.TotalScore.Away.Should().Be(expected.TotalScore.Away,
                    $"{messagePrefix}Game score for away team do not match");
                actual.IsTechnicalDefeat.Should().Be(expected.IsTechnicalDefeat,
                    $"{messagePrefix}Technical defeat in game do not match");
            }
        }
    }
}
