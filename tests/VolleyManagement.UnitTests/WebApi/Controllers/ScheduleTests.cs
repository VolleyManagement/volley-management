namespace VolleyManagement.UnitTests.WebApi.Controllers
{
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Contracts;
    using VolleyManagement.Domain.GamesAggregate;
    using VolleyManagement.Domain.TournamentsAggregate;
    using VolleyManagement.UI.Areas.WebApi.Controllers;
    using VolleyManagement.UnitTests.Services.GameService;
    using VolleyManagement.UnitTests.WebApi.ViewModels.Schedule;
    using Xunit;

    [ExcludeFromCodeCoverage]
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
        public ScheduleTests()
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
        [Fact]
        public void GetSchedule_TournamentWithOneWeekOneDivisionOneGame_ScheduleReturned()
        {
            // Arrange
            const byte TEST_ROUND_COUNT = 5;
            var tournament = CreateTournamentData(TEST_ROUND_COUNT);
            var testGames = new GameServiceTestFixture().WithOneWeekOneDivisionOneGame().Build();

            MockGetScheduleInfo(TOURNAMENT_ID, tournament);
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
        [Fact]
        public void GetSchedule_TournamentWithoutGames_EmptyScheduleReturned()
        {
            // Arrange
            const byte TEST_ROUND_COUNT = 5;
            var tournament = CreateTournamentData(TEST_ROUND_COUNT);
            SetupGetTournamentResults(TOURNAMENT_ID, new List<GameResultDto>());
            MockGetScheduleInfo(TOURNAMENT_ID, tournament);
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
        [Fact]
        public void GetSchedule_TournamentWithTwoWeeksTwoDivisionsTwoGames_ScheduleReturned()
        {
            // Arrange
            const byte TEST_ROUND_COUNT = 5;
            var tournament = CreateTournamentData(TEST_ROUND_COUNT);
            var testGames = new GameServiceTestFixture().
            TestGamesWithResultInTwoWeeksTwoDivisionsTwoGames().
            Build();

            MockGetScheduleInfo(TOURNAMENT_ID, tournament);
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
        [Fact]
        public void GetSchedule_TournamentWithOneWeekTwoGameDaysTwoDivisionsTwoGames_ScheduleReturned()
        {
            // Arrange
            const byte TEST_ROUND_COUNT = 5;
            var tournament = CreateTournamentData(TEST_ROUND_COUNT);
            var testGames = new GameServiceTestFixture().
                TestGamesWithResultInOneWeekTwoGameDaysTwoDivisionsTwoGames().
                Build();

            MockGetScheduleInfo(TOURNAMENT_ID, tournament);

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
        [Fact]
        public void GetSchedule_TournamentWithOneWeekOneGameDayTwoDivisionsTwoGames_ScheduleReturned()
        {
            // Arrange
            const byte TEST_ROUND_COUNT = 5;
            var tournament = CreateTournamentData(TEST_ROUND_COUNT);
            var testGames = new GameServiceTestFixture().
                TestGamesWithResultInOneWeekOneGameDayTwoDivisionsTwoGames().
                Build();

            MockGetScheduleInfo(TOURNAMENT_ID, tournament);
            SetupGetTournamentResults(TOURNAMENT_ID, testGames);
            var expected = new ScheduleViewModelTestFixture().WithOneWeekOneGameDayTwoDivisionsTwoGames().Build();

            var sut = BuildSUT();

            // Act
            var actual = sut.GetSchedule(TOURNAMENT_ID);

            // Assert
            _gameServiceMock.Verify(ts => ts.GetTournamentGames(TOURNAMENT_ID), Times.Once());

            ScheduleViewModelComparer.AssertAreEqual(expected, actual);
        }

        [Fact]
        public void GetSchedule_TournamentPlayedOverSeveralWeeks_ScheduleIsOrderedByWeekNumber()
        {
            // Arrange
            const byte TEST_ROUND_COUNT = 5;
            var tournament = CreateTournamentData(TEST_ROUND_COUNT);
            var testGames = new GameServiceTestFixture().
                TestGamesWithResultInThreeWeeksTwoDivisionsThreeGames().
                Build();

            MockGetScheduleInfo(TOURNAMENT_ID, tournament);

            SetupGetTournamentResults(TOURNAMENT_ID, testGames);
            var expected = new ScheduleViewModelTestFixture().WithThreeWeeksTwoDivisionsThreeGames().Build();

            var sut = BuildSUT();

            // Act
            var actual = sut.GetSchedule(TOURNAMENT_ID);

            // Assert
            _gameServiceMock.Verify(ts => ts.GetTournamentGames(TOURNAMENT_ID), Times.Once());

            ScheduleViewModelComparer.AssertAreEqual(expected, actual);
        }

        [Fact]
        public void GetSchedule_TournamentPlayedOverSeveralYears_ScheduleIsOrderedByYearThenByWeek()
        {
            // Arrange
            const byte TEST_ROUND_COUNT = 5;
            var tournament = CreateTournamentData(TEST_ROUND_COUNT);
            var testGames = new GameServiceTestFixture()
                                .TestGamesInSeveralYearsAndWeeks()
                                .Build();

            MockGetScheduleInfo(TOURNAMENT_ID, tournament);

            SetupGetTournamentResults(TOURNAMENT_ID, testGames);
            var expected = new ScheduleViewModelTestFixture().WithGamesInSeveralYearsAndWeeks().Build();

            var sut = BuildSUT();

            // Act
            var actual = sut.GetSchedule(TOURNAMENT_ID);

            // Assert
            ScheduleViewModelComparer.AssertAreEqual(expected, actual);
        }

        [Fact]
        public void GetSchedule_TournamentHasFreeDayGame_FreeDayGameIsLast()
        {
            // Arrange
            const byte TEST_ROUND_COUNT = 5;
            var tournament = CreateTournamentData(TEST_ROUND_COUNT);

            var testGames = new GameServiceTestFixture()
                .TestGamesForSeveralDivisionsAndFreeDayGameInOneDay()
                .Build();

            MockGetScheduleInfo(TOURNAMENT_ID, tournament);
            SetupGetTournamentResults(TOURNAMENT_ID, testGames);
            var expected = new ScheduleViewModelTestFixture().WithGamesInSeveralDivisionsAndFreeDayGameInOneDay().Build();

            var sut = BuildSUT();

            // Act
            var actual = sut.GetSchedule(TOURNAMENT_ID);

            // Assert
            ScheduleViewModelComparer.AssertAreEqual(expected, actual);
        }

        [Fact]
        public void GetSchedule_PlayoffScheme_RoundNamesAreCreated()
        {
            // Arrange
            const byte TEST_ROUND_COUNT = 5;
            var tournament = CreateTournamentData(TEST_ROUND_COUNT);
            tournament.Scheme = TournamentSchemeEnum.PlayOff;

            MockGetScheduleInfo(TOURNAMENT_ID, tournament);
            SetupGetTournamentResults(TOURNAMENT_ID, new GameServiceTestFixture().TestPlayoffWith5Rounds().Build());

            var expected = new ScheduleViewModelTestFixture().With5RoundPlayoffGames().Build();

            var sut = BuildSUT();

            // Act
            var actual = sut.GetSchedule(TOURNAMENT_ID);

            // Assert
            ScheduleViewModelComparer.AssertAreEqual(expected, actual);
        }

        [Fact]
        public void GetSchedule_PlayoffScheme_NotScheduledGamesAreRemoved()
        {
            // Arrange
            const byte TEST_ROUND_COUNT = 3;
            var tournament = CreateTournamentData(TEST_ROUND_COUNT);
            tournament.Scheme = TournamentSchemeEnum.PlayOff;

            MockGetScheduleInfo(TOURNAMENT_ID, tournament);
            SetupGetTournamentResults(TOURNAMENT_ID, new GameServiceTestFixture().TestPlayoffWithFirstRoundScheduledOnly().Build());

            var expected = new ScheduleViewModelTestFixture().WithPlayoffWithFirstRoundScheduledOnly().Build();

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

        private void MockGetScheduleInfo(int tournamentId, TournamentScheduleDto tournament)
        {
            _tournamentServiceMock.Setup(tr => tr.GetTournamentScheduleInfo(tournamentId)).Returns(tournament);
        }

        private static TournamentScheduleDto CreateTournamentData(byte roundCount)
        {
            return new TournamentScheduleDto {
                Id = TOURNAMENT_ID,
                Name = "Some tournament",
                Scheme = TournamentSchemeEnum.One,
                StartDate = new DateTime(1996, 07, 25),
                Divisions = new List<DivisionScheduleDto>
                {
                    new DivisionScheduleDto
                    {
                        DivisionId     = 1,
                        DivisionName   = "Division 1",
                        NumberOfRounds = roundCount,
                    },
                },
            };
        }
        #endregion
    }
}
