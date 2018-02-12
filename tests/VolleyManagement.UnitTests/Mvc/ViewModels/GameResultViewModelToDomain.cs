namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System;
    using System.Collections.Generic;
    using Domain.GamesAggregate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Services.GameService;
    using UI.Areas.Mvc.ViewModels.GameResults;

    /// <summary>
    /// View model game result class test
    /// </summary>
    [TestClass]
    public class GameResultViewModelToDomain
    {
        /// <summary>
        /// Map() method test.
        /// Does correct game result mapped to a view model.
        /// </summary>
        [TestMethod]
        public void Map_GameResultDtoAsParam_MappedToViewModel()
        {
            // Arrange
            var testViewModel = new GameResultViewModelBuilder()
                .WithId(1)
                .WithTournamentId(1)
                .WithHomeTeamId(1)
                .WithAwayTeamId(2)
                .WithHomeTeamName("HomeTeam")
                .WithAwayTeamName("AwayTeam")
                .WithSetsScore(3, 0)
                .WithTechnicalDefeat(false)
                .WithDate(DateTime.Parse("2016-04-12 12:15"))
                .WithRound(1)
                .WithSetScores(new List<ScoreViewModel>
                    {
                        new ScoreViewModel(25, 20),
                        new ScoreViewModel(26, 24),
                        new ScoreViewModel(30, 28),
                        new ScoreViewModel(0, 0),
                        new ScoreViewModel(0, 0),
                    })
                .Build();

            var testDomainModel = new GameResultDtoBuilder()
                .WithId(1)
                .WithTournamentId(1)
                .WithHomeTeamId(1)
                .WithAwayTeamId(2)
                .WithHomeTeamName("HomeTeam")
                .WithAwayTeamName("AwayTeam")
                .WithHomeSetsScore(3)
                .WithAwaySetsScore(0)
                .WithNoTechnicalDefeat()
                .WithHomeSet1Score(25)
                .WithHomeSet2Score(26)
                .WithHomeSet3Score(30)
                .WithHomeSet4Score(0)
                .WithHomeSet5Score(0)
                .WithAwaySet1Score(20)
                .WithAwaySet2Score(24)
                .WithAwaySet3Score(28)
                .WithAwaySet4Score(0)
                .WithAwaySet5Score(0)
                .WithDate(DateTime.Parse("2016-04-12 12:15"))
                .WithRound(1)
                .Build();

            // Act
            var actual = GameResultViewModel.Map(testDomainModel);

            // Assert
            TestHelper.AreEqual<GameResultViewModel>(testViewModel, actual, new GameResultViewModelComparer());
        }
    }
}
