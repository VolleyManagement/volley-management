namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using VolleyManagement.Contracts;
    using VolleyManagement.Domain.GameReportsAggregate;
    using VolleyManagement.UI.Areas.Mvc.Controllers;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.GameReports;
    using VolleyManagement.UnitTests.Mvc.ViewModels;
    using VolleyManagement.UnitTests.Services.GameReportService;
    using VolleyManagement.UnitTests.Services.TournamentService;

    /// <summary>
    /// Tests for <see cref="GameReportsController"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class GameReportsControllerTests
    {
        private const int TOURNAMENT_ID = 1;
        private const int TOURNAMENT_PLAYOFF_ID = 4;
        private const string TOURNAMENT_NAME = "Name";

        private Mock<IGameReportService> _gameReportServiceMock;
        private Mock<ITournamentService> _tournamentServiceMock;

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _gameReportServiceMock = new Mock<IGameReportService>();
            _tournamentServiceMock = new Mock<ITournamentService>();
        }

        /// <summary>
        /// Test for Standings() method. Tournament standings are requested. Tournament standings are returned.
        /// </summary>
        [TestMethod]
        public void Standings_StandingsRequested_StandingsReturned()
        {
            // Arrange
            var teams = new TeamStandingsTestFixture().TestTeamStandings().Build();
            var gameResults = new ShortGameResultDtoTetsFixture().GetShortResults().Build();
            var testPivotStandings = new List<PivotStandingsDto> { new PivotStandingsDto(teams, gameResults) };

            var testStandings = new List<List<StandingsEntry>> { new StandingsTestFixture().TestStandings().Build() };
            var expected = new StandingsViewModelBuilder().Build();

            MockTournamentServiceReturnTournament();
            SetupIsStandingsAvailableTrue(TOURNAMENT_ID);
            SetupGameReportGetStandings(TOURNAMENT_ID, testStandings);
            SetupGameReportGetPivotStandings(TOURNAMENT_ID, testPivotStandings);

            var sut = BuildSUT();

            // Act
            var actual = TestExtensions.GetModel<StandingsViewModel>(sut.Standings(TOURNAMENT_ID, TOURNAMENT_NAME));

            // Assert
            TestHelper.AreEqual(expected, actual, new StandingsViewModelComparer());
        }

        /// <summary>
        /// Test for Standings() method. Tournament standings view model are requested.
        /// Tournament standings not available for playoff scheme
        /// </summary>
        [TestMethod]
        public void Standings_StandingsForPlayoffScheme_StandingsNotAvailableReturned()
        {
            // Arrange
            var expected = new StandingsViewModelBuilder().WithStandingsNotAvailableMessage().Build();

            SetupIsStandingsAvailableFalse(TOURNAMENT_PLAYOFF_ID);

            var sut = BuildSUT();

            // Act
            var actual = TestExtensions.GetModel<StandingsViewModel>(sut.Standings(TOURNAMENT_PLAYOFF_ID, TOURNAMENT_NAME));

            // Assert
            TestHelper.AreEqual(expected, actual, new StandingsViewModelComparer());
        }

        /// <summary>
        /// Test for Standings() method. Tournament standings view model are requested.
        /// Tournament standings view model with 2 team scores completely equal returned.
        /// </summary>
        [TestMethod]
        public void Standings_StandingsWithTwoTeamsScoresCompletelyEqual_TeamsHaveSamePosition()
        {
            // Arrange
            var teams = new TeamStandingsTestFixture()
                .WithTeamStandingsTwoTeamsScoresCompletelyEqual()
                .Build();

            var gameResults = new ShortGameResultDtoTetsFixture()
                .GetShortResultsForTwoTeamsScoresCompletelyEqual()
                .Build();

            var testPivotStandings = new List<PivotStandingsDto> { new PivotStandingsDto(teams, gameResults) };

            var testStandings = new List<List<StandingsEntry>>
            {
                new StandingsTestFixture().WithRepetitivePointsSetsRatioAndBallsRatio().Build()
            };

            var expected = new StandingsViewModelBuilder().WithTwoTeamsScoresCompletelyEqual().Build();

            MockTournamentServiceReturnTournament();
            SetupIsStandingsAvailableTrue(TOURNAMENT_ID);
            SetupGameReportGetStandings(TOURNAMENT_ID, testStandings);
            SetupGameReportGetPivotStandings(TOURNAMENT_ID, testPivotStandings);

            var sut = BuildSUT();

            // Act
            var actual = TestExtensions.GetModel<StandingsViewModel>(sut.Standings(TOURNAMENT_ID, TOURNAMENT_NAME));

            // Assert
            TestHelper.AreEqual(expected, actual, new StandingsViewModelComparer());
        }

        private GameReportsController BuildSUT()
        {
            return new GameReportsController(_gameReportServiceMock.Object, _tournamentServiceMock.Object);
        }

        private void SetupGameReportGetStandings(int tournamentId, List<List<StandingsEntry>> testData)
        {
            _gameReportServiceMock
                .Setup(m => m.GetStandings(It.Is<int>(id => id == tournamentId))).Returns(testData);
        }

        private void SetupGameReportGetPivotStandings(int tournamentId, List<PivotStandingsDto> testData)
        {
            _gameReportServiceMock
                .Setup(m => m.GetPivotStandings(It.Is<int>(id => id == tournamentId))).Returns(testData);
        }

        private void SetupIsStandingsAvailableTrue(int tournamentId)
        {
            _gameReportServiceMock
                .Setup(m => m.IsStandingAvailable(It.Is<int>(id => id == tournamentId))).Returns(true);
        }

        private void SetupIsStandingsAvailableFalse(int tournamentId)
        {
            _gameReportServiceMock
                .Setup(m => m.IsStandingAvailable(It.Is<int>(id => id == tournamentId))).Returns(false);
        }

        private void MockTournamentServiceReturnTournament()
        {
            var tour = new TournamentBuilder().Build();
            _tournamentServiceMock.Setup(ts => ts.Get(It.IsAny<int>())).Returns(tour);
        }
    }
}