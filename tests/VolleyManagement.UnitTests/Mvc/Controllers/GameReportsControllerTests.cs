namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Contracts;
    using Domain.GameReportsAggregate;
    using UI.Areas.Mvc.Controllers;
    using UI.Areas.Mvc.ViewModels.GameReports;
    using ViewModels;
    using Services.GameReportService;

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

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _gameReportServiceMock = new Mock<IGameReportService>();
        }

        /// <summary>
        /// Test for Standings() method. Tournament standings are requested. Tournament standings are returned.
        /// </summary>
        [TestMethod]
        public void Standings_StandingsRequested_StandingsReturned()
        {
            // Arrange
            var testPivotStandings = new PivotStandingsTestFixture().WithMultipleDivisionsAllPossibleScores().Build();
            var testStandings = new StandingsTestFixture().WithMultipleDivisionsAllPossibleScores().Build();

            var expected = new StandingsViewModelBuilder().WithMultipleDivisionsAllPossibleScores().Build();

            SetupIsStandingsAvailableTrue(TOURNAMENT_ID);
            SetupGameReportGetStandings(TOURNAMENT_ID, testStandings);
            SetupGameReportGetPivotStandings(TOURNAMENT_ID, testPivotStandings);

            var sut = BuildSUT();

            // Act
            var actual = TestExtensions.GetModel<StandingsViewModel>(sut.Standings(TOURNAMENT_ID, TOURNAMENT_NAME));

            // Assert
            TestHelper.AreEqual(expected, actual, new StandingsViewModelComparer());
        }

        [TestMethod]
        public void Standings_LastUpdateTimeExists_StandingsReturnLastUpdateTime()
        {
            // Arrange
            var LAST_UPDATE_TIME = new DateTime(2017, 4, 5, 12, 4, 23);

            var testPivotStandings = new PivotStandingsTestFixture().WithMultipleDivisionsAllPossibleScores()
                                            .WithLastUpdateTime(LAST_UPDATE_TIME).Build();
            var testStandings = new StandingsTestFixture().WithMultipleDivisionsAllPossibleScores()
                                            .WithLastUpdateTime(LAST_UPDATE_TIME).Build();

            var expected = new StandingsViewModelBuilder().WithMultipleDivisionsAllPossibleScores()
                                            .WithLastUpdateTime(LAST_UPDATE_TIME).Build();

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

        private GameReportsController BuildSUT()
        {
            return new GameReportsController(_gameReportServiceMock.Object);
        }

        private void SetupGameReportGetStandings(int tournamentId, TournamentStandings<StandingsDto> testData)
        {
            _gameReportServiceMock
                .Setup(m => m.GetStandings(It.Is<int>(id => id == tournamentId))).Returns(testData);
        }

        private void SetupGameReportGetPivotStandings(int tournamentId, TournamentStandings<PivotStandingsDto> testData)
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
    }
}