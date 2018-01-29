namespace VolleyManagement.UnitTests.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using VolleyManagement.Contracts;
    using VolleyManagement.Domain.GameReportsAggregate;
    using VolleyManagement.Domain.GamesAggregate;
    using VolleyManagement.Domain.TournamentsAggregate;
    using VolleyManagement.UI.Areas.WebApi.Controllers;
    using VolleyManagement.UI.Areas.WebAPI.ViewModels.Schedule;
    using VolleyManagement.UnitTests.Services.GameService;
    using VolleyManagement.UnitTests.Services.TournamentService;
    using VolleyManagement.UnitTests.WebApi.ViewModels.Schedule;

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ScheduleTests
    {
        /// <summary>
        /// ID for tests
        /// </summary>
        private const int TOURNAMENT_ID = 1;

        private Mock<ITournamentService> _tournamentServiceMock = new Mock<ITournamentService>();
        private Mock<IGameReportService> _gameReportServiceMock = new Mock<IGameReportService>();
        private Mock<IGameService> _gameServiceMock = new Mock<IGameService>();

        /// <summary>
        /// Initializes test data
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _tournamentServiceMock = new Mock<ITournamentService>();
            _gameReportServiceMock = new Mock<IGameReportService>();
            _gameServiceMock = new Mock<IGameService>();
        }

        #region GetSchedule

        /// <summary>
        /// Test for GetSchedule method.
        /// Tournament with game; game list returned
        /// </summary>
        [TestMethod]
        public void GetSchedule_TournamentWithOneWeekOneDivisionOneGame_ScheduleReturned()
        {
            // Arrange
            var testTournament = new TournamentBuilder().Build();
            MockGetTournament(testTournament, TOURNAMENT_ID);
            var testGames = new GameServiceTestFixture().WithOneWeekOneDivisionOneGame().Build();
            SetupGetTournamentResults(TOURNAMENT_ID, testGames);
            var expected = new ScheduleViewModelTestFixture().WithOneWeekOneDivisionOneGame().Build();

            var sut = BuildSUT();

            // Act
            var actual = sut.GetSchedule(TOURNAMENT_ID);

            // Assert
            _gameServiceMock.Verify(ts => ts.GetTournamentGames(TOURNAMENT_ID), Times.Once());

            ScheduleViewModelComparer.AssertAreEqual(expected, actual);
        }

        /// <summary>
        /// Test for GetSchedule method.
        /// Tournament without games; empty games list returned
        /// </summary>
        [TestMethod]
        public void GetSchedule_TournamentWithoutGames_EmptyScheduleReturned()
        {
            // Arrange
            var testTournament = new TournamentBuilder().Build();
            MockGetTournament(testTournament, TOURNAMENT_ID);
            SetupGetTournamentResults(TOURNAMENT_ID, new List<GameResultDto>());
            var expected = new ScheduleViewModelTestFixture().WithEmptyResult().Build();

            var sut = BuildSUT();

            // Act
            var actual = sut.GetSchedule(TOURNAMENT_ID);

            // Assert
            _gameServiceMock.Verify(ts => ts.GetTournamentGames(TOURNAMENT_ID), Times.Once());

            ScheduleViewModelComparer.AssertAreEqual(expected, actual);
        }

        /// <summary>
        /// Test for GetSchedule method.
        /// Tournament with games in different weeks. Schedule returned
        /// </summary>
        [TestMethod]
        public void GetSchedule_TournamentWithTwoWeeksTwoDivisionsTwoGames_ScheduleReturned()
        {
            // Arrange
            var testTournament = new TournamentBuilder().Build();
            MockGetTournament(testTournament, TOURNAMENT_ID);
            var testGames = new GameServiceTestFixture().
            TestGamesWithResultInTwoWeeksTwoDivisionsTwoGames().
            Build();

            SetupGetTournamentResults(TOURNAMENT_ID, testGames);
            var expected = new ScheduleViewModelTestFixture().WithTwoWeeksTwoDivisionsTwoGames().Build();

            var sut = BuildSUT();

            // Act
            var actual = sut.GetSchedule(TOURNAMENT_ID);

            // Assert
            _gameServiceMock.Verify(ts => ts.GetTournamentGames(TOURNAMENT_ID), Times.Once());

            ScheduleViewModelComparer.AssertAreEqual(expected, actual);
        }

        /// <summary>
        /// Test for GetSchedule method.
        /// Tournament with games in different weeks. Schedule returned
        /// </summary>
        [TestMethod]
        public void GetSchedule_TournamentWithOneWeekTwoGameDaysTwoDivisionsTwoGames_ScheduleReturned()
        {
            // Arrange
            var testTournament = new TournamentBuilder().Build();
            MockGetTournament(testTournament, TOURNAMENT_ID);
            var testGames = new GameServiceTestFixture().
                TestGamesWithResultInOneWeekTwoGameDaysTwoDivisionsTwoGames().
                Build();

            SetupGetTournamentResults(TOURNAMENT_ID, testGames);
            var expected = new ScheduleViewModelTestFixture().WithOneWeekTwoGameDaysTwoDivisionsTwoGames().Build();

            var sut = BuildSUT();

            // Act
            var actual = sut.GetSchedule(TOURNAMENT_ID);

            // Assert
            _gameServiceMock.Verify(ts => ts.GetTournamentGames(TOURNAMENT_ID), Times.Once());

            ScheduleViewModelComparer.AssertAreEqual(expected, actual);
        }

        /// <summary>
        /// Test for GetSchedule method.
        /// Tournament with games in different weeks. Schedule returned
        /// </summary>
        [TestMethod]
        public void GetSchedule_TournamentWithOneWeekOneGameDayTwoDivisionsTwoGames_ScheduleReturned()
        {
            // Arrange
            var testTournament = new TournamentBuilder().Build();
            MockGetTournament(testTournament, TOURNAMENT_ID);
            var testGames = new GameServiceTestFixture().
                TestGamesWithResultInOneWeekOneGameDayTwoDivisionsTwoGames().
                Build();

            SetupGetTournamentResults(TOURNAMENT_ID, testGames);
            var expected = new ScheduleViewModelTestFixture().WithOneWeekOneGameDayTwoDivisionsTwoGames().Build();

            var sut = BuildSUT();

            // Act
            var actual = sut.GetSchedule(TOURNAMENT_ID);

            // Assert
            _gameServiceMock.Verify(ts => ts.GetTournamentGames(TOURNAMENT_ID), Times.Once());

            ScheduleViewModelComparer.AssertAreEqual(expected, actual);
        }

        [TestMethod]
        public void GetSchedule_TournamentPlayedOverSeveralWeeks_ScheduleIsOrderedByWeekNumber()
        {
            // Arrange
            var testTournament = new TournamentBuilder().Build();
            MockGetTournament(testTournament, TOURNAMENT_ID);
            var testGames = new GameServiceTestFixture().
                TestGamesWithResultInThreeWeeksTwoDivisionsThreeGames().
                Build();

            SetupGetTournamentResults(TOURNAMENT_ID, testGames);
            var expected = new ScheduleViewModelTestFixture().WithThreeWeeksTwoDivisionsThreeGames().Build();

            var sut = BuildSUT();

            // Act
            var actual = sut.GetSchedule(TOURNAMENT_ID);

            // Assert
            _gameServiceMock.Verify(ts => ts.GetTournamentGames(TOURNAMENT_ID), Times.Once());

            ScheduleViewModelComparer.AssertAreEqual(expected, actual);
        }

        [TestMethod]
        public void GetSchedule_TournamentPlayedOverSeveralYears_ScheduleIsOrderedByYearThenByWeek()
        {
            // Arrange
            var testTournament = new TournamentBuilder().Build();
            MockGetTournament(testTournament, TOURNAMENT_ID);
            var testGames = new GameServiceTestFixture()
                                .TestGamesInSeveralYearsAndWeeks()
                                .Build();

            SetupGetTournamentResults(TOURNAMENT_ID, testGames);
            var expected = new ScheduleViewModelTestFixture().WithGamesInSeveralYearsAndWeeks().Build();

            var sut = BuildSUT();

            // Act
            var actual = sut.GetSchedule(TOURNAMENT_ID);

            // Assert
            ScheduleViewModelComparer.AssertAreEqual(expected, actual);
        }

        [TestMethod]
        public void GetSchedule_TournamentHasFreeDayGame_FreeDayGameIsLast()
        {
            // Arrange
            var testTournament = new TournamentBuilder().Build();
            MockGetTournament(testTournament, TOURNAMENT_ID);
            var testGames = new GameServiceTestFixture()
                .TestGamesForSeveralDivisionsAndFreeDayGameInOneDay()
                .Build();

            SetupGetTournamentResults(TOURNAMENT_ID, testGames);
            var expected = new ScheduleViewModelTestFixture().WithGamesInSeveralDivisionsAndFreeDayGameInOneDay().Build();

            var sut = BuildSUT();

            // Act
            var actual = sut.GetSchedule(TOURNAMENT_ID);

            // Assert
            ScheduleViewModelComparer.AssertAreEqual(expected, actual);
        }

        #endregion

        #region Private

        private TournamentsController BuildSUT()
        {
            return new TournamentsController(
                _tournamentServiceMock.Object,
                _gameServiceMock.Object,
                _gameReportServiceMock.Object);
        }

        private void MockGetTournament(Tournament tournament, int id)
        {
            _tournamentServiceMock.Setup(tr => tr.Get(id)).Returns(tournament);
        }

        private void SetupGetTournamentResults(int tournamentId, List<GameResultDto> expectedGames)
        {
            _gameServiceMock.Setup(t => t.GetTournamentGames(It.IsAny<int>())).Returns(expectedGames);
        }
        #endregion
    }
}
