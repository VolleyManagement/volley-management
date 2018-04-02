namespace VolleyManagement.UnitTests.Services.GameService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Collections;
    using Contracts;
    using Contracts.Authorization;
    using Contracts.Exceptions;
    using Crosscutting.Contracts.Providers;
    using Data.Contracts;
    using Data.Exceptions;
    using Data.Queries.Common;
    using Data.Queries.GameResult;
    using Data.Queries.Team;
    using Data.Queries.Tournament;
    using Domain.GamesAggregate;
    using Domain.RolesAggregate;
    using Domain.TeamsAggregate;
    using Domain.TournamentsAggregate;
    using GameReportService;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Mvc.ViewModels;
    using TournamentService;
    using VolleyManagement.Services;

    /// <summary>
    /// Tests for <see cref="GameService"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class GameServiceTests
    {
        #region Fields and constants

        private const int GAME_RESULT_ID = 1;
        private const int TOURNAMENT_ID = 1;
        private const string TOURNAMENT_DATE_START = "2016-04-02 10:00";
        private const string TOURNAMENT_DATE_END = "2016-04-04 10:00";
        private const string BEFORE_TOURNAMENT_DATE = "2016-04-02 07:00";
        private const string LATE_TOURNAMENT_DATE = "2016-04-06 10:00";

        private const double ONE_DAY = 1;

        private const int TEST_HOME_SET_SCORS = 3;
        private const int TEST_AWAY_SET_SCORS = 2;

        private const byte FIRST_ROUND_NUMBER = 1;
        private const byte SECOND_ROUND_NUMBER = 2;

        private const int SPECIFIC_GAME_ID = 2;
        private const int ANOTHER_GAME_ID = SPECIFIC_GAME_ID + 1;

        private const string FIRST_TEAM_PLACEHOLDER = "<Team 1>";
        private const string SECOND_TEAM_PLACEHOLDER = "<Team 2>";

        private readonly string _wrongRoundDate
            = "Start of the round should not be earlier than the start of the tournament or later than the end of the tournament";

        private readonly Mock<TimeProvider> _timeMock = new Mock<TimeProvider>();

        private Mock<IGameRepository> _gameRepositoryMock;
        private Mock<IAuthorizationService> _authServiceMock;
        private Mock<ITournamentService> _tournamentServiceMock;
        private Mock<ITournamentRepository> _tournamentRepositoryMock;
        private Mock<IQuery<GameResultDto, FindByIdCriteria>> _getByIdQueryMock;
        private Mock<IQuery<ICollection<GameResultDto>, TournamentGameResultsCriteria>> _tournamentGameResultsQueryMock;
        private Mock<IQuery<ICollection<Game>, TournamentRoundsGameResultsCriteria>> _gamesByTournamentIdRoundsNumberQueryMock;
        private Mock<IQuery<TournamentScheduleDto, TournamentScheduleInfoCriteria>> _tournamentScheduleDtoByIdQueryMock;
        private Mock<IQuery<Tournament, FindByIdCriteria>> _tournamentByIdQueryMock;
        private Mock<IQuery<ICollection<Game>, GamesByRoundCriteria>> _gamesByTournamentIdInRoundsByNumbersQueryMock;
        private Mock<IQuery<Game, GameByNumberCriteria>> _gameNumberByTournamentIdQueryMock;
        private Mock<IQuery<ICollection<TeamTournamentDto>, FindByTournamentIdCriteria>> _tournamentTeamsQueryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;

        #endregion

        #region Constuctor

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _gameRepositoryMock = new Mock<IGameRepository>();
            _authServiceMock = new Mock<IAuthorizationService>();
            _tournamentServiceMock = new Mock<ITournamentService>();
            _tournamentRepositoryMock = new Mock<ITournamentRepository>();
            _getByIdQueryMock = new Mock<IQuery<GameResultDto, FindByIdCriteria>>();
            _tournamentGameResultsQueryMock = new Mock<IQuery<ICollection<GameResultDto>, TournamentGameResultsCriteria>>();
            _gamesByTournamentIdRoundsNumberQueryMock = new Mock<IQuery<ICollection<Game>, TournamentRoundsGameResultsCriteria>>();
            _tournamentScheduleDtoByIdQueryMock = new Mock<IQuery<TournamentScheduleDto, TournamentScheduleInfoCriteria>>();
            _tournamentByIdQueryMock = new Mock<IQuery<Tournament, FindByIdCriteria>>();
            _gamesByTournamentIdInRoundsByNumbersQueryMock = new Mock<IQuery<ICollection<Game>, GamesByRoundCriteria>>();
            _gameNumberByTournamentIdQueryMock = new Mock<IQuery<Game, GameByNumberCriteria>>();
            _tournamentTeamsQueryMock = new Mock<IQuery<ICollection<TeamTournamentDto>, FindByTournamentIdCriteria>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _gameRepositoryMock.Setup(m => m.UnitOfWork).Returns(_unitOfWorkMock.Object);
            _tournamentRepositoryMock.Setup(tr => tr.UnitOfWork).Returns(_unitOfWorkMock.Object);
            _timeMock.SetupGet(tp => tp.UtcNow).Returns(new DateTime(2015, 06, 01));
            TimeProvider.Current = _timeMock.Object;
        }

        #endregion

        #region Create

        [TestMethod]
        public void Create_GameValid_GameCreated()
        {
            // Arrange
            MockDefaultTournament();
            var newGame = new GameBuilder().Build();
            var expectedGame = new GameBuilder().Build();
            var sut = BuildSUT();

            // Act
            sut.Create(newGame);

            // Assert
            VerifyCreateGame(expectedGame, Times.Once());
        }

        [TestMethod]
        public void Create_GameWithPenalty_GameCreated()
        {
            // Arrange
            MockDefaultTournament();
            var newGame = new GameBuilder().WithAPenalty().Build();
            var expectedGame = new GameBuilder().WithAPenalty().Build();
            var sut = BuildSUT();

            // Act
            sut.Create(newGame);

            // Assert
            VerifyCreateGame(expectedGame, Times.Once());
        }

        [TestMethod]
        public void Create_GameHasResult_LastTimeUpdated()
        {
            // Arrange
            MockDefaultTournament();
            var tour = new TournamentBuilder().Build();
            _tournamentByIdQueryMock.Setup(tr => tr.Execute(It.IsAny<FindByIdCriteria>())).Returns(tour);

            var newGame = new GameBuilder().Build();
            var sut = BuildSUT();

            // Act
            sut.Create(newGame);

            // Assert
            Assert.AreEqual(TimeProvider.Current.UtcNow, tour.LastTimeUpdated);
        }

        [TestMethod]
        public void Create_GameWithNoResult_LastTimeNotUpdated()
        {
            // Arrange
            MockDefaultTournament();
            var expectedTimeUpdated = new DateTime(2017, 9, 25, 17, 34, 12);
            var tour = new TournamentBuilder().Build();
            tour.LastTimeUpdated = expectedTimeUpdated;
            _tournamentByIdQueryMock.Setup(tr => tr.Execute(It.IsAny<FindByIdCriteria>())).Returns(tour);

            var newGame = new GameBuilder().Build();
            newGame.Result = null;
            var sut = BuildSUT();

            // Act
            sut.Create(newGame);

            // Assert
            Assert.AreEqual(expectedTimeUpdated, tour.LastTimeUpdated, "Last Update time should not be changed");
        }

        /// <summary>
        /// Test for Create method. The game result instance is null. Exception is thrown during creation.
        /// </summary>
        [TestMethod]
        public void Create_GameNull_ExceptionThrown()
        {
            Exception exception = null;

            // Arrange
            var newGame = null as Game;
            var sut = BuildSUT();

            // Act
            try
            {
                sut.Create(newGame);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, ExpectedExceptionMessages.GAME);
        }

        [TestMethod]
        public void Create_GameSameTeams_ExceptionThrown()
        {
            Exception exception = null;

            // Arrange
            var newGame = new GameBuilder().WithTheSameTeams().Build();

            _tournamentScheduleDtoByIdQueryMock.Setup(m => m.Execute(It.IsAny<TournamentScheduleInfoCriteria>()))
                .Returns(new TournamentScheduleDto());

            var sut = BuildSUT();

            // Act
            try
            {
                sut.Create(newGame);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, ExpectedExceptionMessages.GAME_SAME_TEAM);
        }

        [TestMethod]
        public void Create_GameInvalidSetsScore_ExceptionThrown()
        {
            Exception exception = null;

            // Arrange
            MockDefaultTournament();
            var newGame = new GameBuilder().WithInvalidSetsScore().Build();

            var sut = BuildSUT();

            // Act
            try
            {
                sut.Create(newGame);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, ExpectedExceptionMessages.GAME_SETS_SCORE_INVALID);
        }

        [TestMethod]
        public void Create_GameSetsScoreNoMatchSetScores_ExceptionThrown()
        {
            Exception exception = null;

            // Arrange
            MockDefaultTournament();
            var newGame = new GameBuilder().WithSetsScoreNoMatchSetScores().Build();
            var sut = BuildSUT();

            // Act
            try
            {
                sut.Create(newGame);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, ExpectedExceptionMessages.GAME_SETS_SCORE_NOMATCH_SET_SCORES);
        }

        [TestMethod]
        public void Create_GameResultInvalidRequiredSetScores_ExceptionThrown()
        {
            Exception exception = null;

            // Arrange
            MockDefaultTournament();
            var newGame = new GameBuilder().WithInvalidRequiredSetScores().Build();
            var sut = BuildSUT();

            // Act
            try
            {
                sut.Create(newGame);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, ExpectedExceptionMessages.GAME_REQUIRED_SET_SCORES_25_0);
        }

        [TestMethod]
        public void Create_GameInvalidOptionalSetScores_ExceptionThrown()
        {
            Exception exception = null;

            // Arrange
            MockDefaultTournament();
            var newGame = new GameBuilder().WithInvalidOptionalSetScores().Build();
            var sut = BuildSUT();

            // Act
            try
            {
                sut.Create(newGame);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, ExpectedExceptionMessages.GAME_REQUIRED_SET_SCORES_0_0);
        }

        [TestMethod]
        public void Create_GamePreviousOptionalSetUnplayed_ExceptionThrown()
        {
            Exception exception = null;

            // Arrange
            MockDefaultTournament();
            var newGame = new GameBuilder().WithPreviousOptionalSetUnplayed().Build();
            var sut = BuildSUT();

            // Act
            try
            {
                sut.Create(newGame);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, ExpectedExceptionMessages.GAME_PREVIOUS_OPTIONAL_SET_UNPLAYED);
        }

        [TestMethod]
        public void Create_GameSetScoresUnordered_ExceptionThrown()
        {
            Exception exception = null;

            // Arrange
            MockDefaultTournament();
            var newGame = new GameBuilder().WithSetScoresUnorderedForHomeTeam().Build();
            var sut = BuildSUT();

            // Act
            try
            {
                sut.Create(newGame);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, ExpectedExceptionMessages.GAME_SET_SCORES_NOT_ORDERED);
        }

        [TestMethod]
        public void Create_GameSetScoresUnorderedForAwayTeam_ExceptionThrown()
        {
            Exception exception = null;

            // Arrange
            MockDefaultTournament();
            var newGame = new GameBuilder().WithSetScoresUnorderedForAwayTeam().Build();
            var sut = BuildSUT();

            // Act
            try
            {
                sut.Create(newGame);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, ExpectedExceptionMessages.GAME_SET_SCORES_NOT_ORDERED);
        }

        [TestMethod]
        public void Create_GameHomeTeamTechnicalWinValidData_GameCreated()
        {
            // Arrange
            MockDefaultTournament();
            var newGame = new GameBuilder().WithTechnicalDefeatValidSetScoresHomeTeamWin().Build();
            var expectedGame = new GameBuilder().WithTechnicalDefeatValidSetScoresHomeTeamWin().Build();
            var sut = BuildSUT();

            // Act
            sut.Create(newGame);

            // Assert
            VerifyCreateGame(expectedGame, Times.Once());
        }

        [TestMethod]
        public void Create_GameAwayTeamTechnicalWinValidData_GameCreated()
        {
            // Arrange
            MockDefaultTournament();
            var newGame = new GameBuilder()
                .WithTechnicalDefeatValidSetScoresAwayTeamWin()
                .WithTournamentId(1)
                .Build();
            var expectedGame = new GameBuilder()
                .WithTechnicalDefeatValidSetScoresAwayTeamWin()
                .WithTournamentId(1)
                .Build();

            var sut = BuildSUT();

            // Act
            sut.Create(newGame);

            // Assert
            VerifyCreateGame(expectedGame, Times.Once());
        }

        [TestMethod]
        public void Create_TechnicalDefeatInvalidSetsScore_ExceptionThrown()
        {
            Exception exception = null;

            // Arrange
            MockDefaultTournament();
            var newGame = new GameBuilder().WithTechnicalDefeatInvalidSetsScore().Build();
            var sut = BuildSUT();

            // Act
            try
            {
                sut.Create(newGame);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, ExpectedExceptionMessages.GAME_SETS_SCORE_INVALID);
        }

        [TestMethod]
        public void Create_GameTechnicalDefeatInvalidSetScores_ExceptionThrown()
        {
            Exception exception = null;

            // Arrange
            MockDefaultTournament();
            var newGame = new GameBuilder().WithTechnicalDefeatInvalidSetScores().Build();
            var sut = BuildSUT();

            // Act
            try
            {
                sut.Create(newGame);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, ExpectedExceptionMessages.GAME_REQUIRED_SET_SCORES_25_0);
        }

        [TestMethod]
        public void Create_GameTechnicalDefeatOptionalSetScore_ExceptionThrown()
        {
            Exception exception = null;

            // Arrange
            MockDefaultTournament();
            var newGame = new GameBuilder().WithTechnicalDefeatValidOptional().Build();
            var sut = BuildSUT();

            // Act
            try
            {
                sut.Create(newGame);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, ExpectedExceptionMessages.GAME_SETS_SCORE_NOMATCH_SET_SCORES);
        }

        [TestMethod]
        public void Create_GameSetScoresNull_ExceptionThrown()
        {
            Exception exception = null;

            // Arrange
            MockDefaultTournament();
            var newGame = new GameBuilder().WithSetScoresNull().Build();
            var sut = BuildSUT();

            // Act
            try
            {
                sut.Create(newGame);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, ExpectedExceptionMessages.GAME_SETS_SCORE_NOMATCH_SET_SCORES);
        }

        [TestMethod]
        public void Create_FreeDayAsHomeTeam_GameCreated()
        {
            // Arrange
            MockDefaultTournament();
            var newGame = new GameBuilder()
                                .WithHomeTeamId(null)
                                .WithAwayTeamId(1)
                                .Build();
            var expectedGame = new GameBuilder()
                                .WithHomeTeamId(1)
                                .WithAwayTeamId(null)
                                .Build();

            var sut = BuildSUT();

            // Act
            sut.Create(newGame);

            // Assert
            VerifyCreateGame(expectedGame, Times.Once());
        }

        [TestMethod]
        public void Create_FreeDayAsHomeTeam_FreeDaySetToAwayTeam()
        {
            // Arrange
            MockDefaultTournament();
            var newGame = new GameBuilder()
                .WithHomeTeamId(null)
                .WithAwayTeamId(1)
                .Build();

            var sut = BuildSUT();

            // Act
            sut.Create(newGame);

            // Assert
            VerifyFreeDaySwapped(newGame);
        }

        [TestMethod]
        public void Create_GameSetsScoreInvalid_ExceptionThrown()
        {
            Exception exception = null;

            // Arrange
            MockDefaultTournament();
            var newGame = new GameBuilder().WithOrdinarySetsScoreInvalid().Build();
            var sut = BuildSUT();

            // Act
            try
            {
                sut.Create(newGame);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, ExpectedExceptionMessages.GAME_SETS_SCORE_INVALID);
        }

        [TestMethod]
        public void Create_GameWithNoResult_GameCreatedWithDefaultResult()
        {
            // Arrange
            MockDefaultTournament();

            var newGame = new GameBuilder()
                .WithNullResult()
                .Build();
            var expectedGameToCreate = new GameBuilder()
                .WithDefaultResult()
                .Build();

            var sut = BuildSUT();

            // Act
            sut.Create(newGame);

            // Assert
            VerifyCreateGame(expectedGameToCreate, Times.Once());
        }

        [TestMethod]
        public void Create_GameBeforeTournamentStarts_ExceptionThrown()
        {
            // Arrange
            Exception exception = null;

            MockDefaultTournament();
            MockTournamentServiceReturnTournament();

            var game = new GameBuilder()
                .WithStartDate(DateTime.Parse(BEFORE_TOURNAMENT_DATE))
                .Build();

            var sut = BuildSUT();

            // Act
            try
            {
                sut.Create(game);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert
            VerifyCreateGame(game, Times.Never());
            VerifyExceptionThrown(exception, _wrongRoundDate);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_GameSetLateDateTime_ExceptionThrown()
        {
            // Arrange
            var tournament = new TournamentScheduleDtoBuilder()
                .WithStartDate(DateTime.Parse(TOURNAMENT_DATE_START))
                .WithEndDate(DateTime.Parse(TOURNAMENT_DATE_END))
                .Build();

            var game = new GameBuilder()
                .WithTournamentId(tournament.Id)
                .WithStartDate(DateTime.Parse(LATE_TOURNAMENT_DATE))
                .Build();

            MockGetTournamentById(tournament.Id, new TournamentScheduleDto());
            MockGetTournamentResults(tournament.Id, new GameServiceTestFixture().TestGameResults().Build());
            var sut = BuildSUT();

            // Act
            sut.Create(game);

            // Assert
            VerifyCreateGame(game, Times.Never(), Times.Once());
        }

        [TestMethod]
        public void Create_GameDateNull_ExceptionThrown()
        {
            Exception exception = null;

            // Arrange
            MockDefaultTournament();

            var game = new GameBuilder()
                .WithNoStartDate()
                .Build();

            var sut = BuildSUT();

            // Act
            try
            {
                sut.Create(game);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, ExpectedExceptionMessages.GAME_DATE_NOT_SET);
        }

        [TestMethod]
        public void Create_SameGameInRound_ExceptionThrown()
        {
            // Arrange
            var excaptionWasThrown = false;

            var duplicate = new GameBuilder()
                .WithId(0)
                .WithHomeTeamId(1)
                .WithAwayTeamId(2)
                .WithRound(1)
                .Build();

            var gameResults = new GameServiceTestFixture().TestGameResults().Build();
            MockGetTournamentById(TOURNAMENT_ID, new TournamentScheduleDtoBuilder().WithScheme(TournamentSchemeEnum.One).Build());
            MockGetTournamentResults(1, gameResults);

            var sut = BuildSUT();

            // Act
            try
            {
                sut.Create(duplicate);
            }
            catch (ArgumentException)
            {
                excaptionWasThrown = true;
            }

            // Assert
            Assert.IsTrue(excaptionWasThrown);
        }

        [TestMethod]
        public void Create_SecondFreeDayInDifferentRoundInDifferentDivision_GameCreated()
        {
            // Arrange
            const int ANOTHER_DIVISION_TEAM_ID = 11;

            var games = new GameServiceTestFixture()
                .TestGamesWithFreeDay()
                .Build();

            var anotherRound = (byte)(games.Select(g => g.Round).Max() + 1);

            var duplicateFreeDayGame = new GameBuilder()
                .TestFreeDayGame()
                .New()
                .WithRound(anotherRound)
                .WithHomeTeamId(ANOTHER_DIVISION_TEAM_ID)
                .Build();

            MockGetTournamentDomain(TOURNAMENT_ID);
            MockGetTournamentById(TOURNAMENT_ID, new TournamentScheduleDtoBuilder().WithAnotherDivision().Build());
            MockGetTournamentResults(TOURNAMENT_ID, games);
            MockGetTeamsInTournament(TOURNAMENT_ID, new TeamInTournamentTestFixture().WithTeamsInTwoDivisionTwoGroups().Build());

            var sut = BuildSUT();

            // Act
            sut.Create(duplicateFreeDayGame);

            // Assert
            VerifyCreateGame(duplicateFreeDayGame, Times.Once());
        }

        [TestMethod]
        public void Create_SecondFreeDayInDifferentRoundInSameDivision_GameCreated()
        {
            // Arrange
            const int ANOTHER_TEAM_ID = 5;

            var games = new GameServiceTestFixture()
                .TestGamesWithFreeDay()
                .Build();

            var anotherRound = (byte)(games.Select(g => g.Round).Max() + 1);

            var duplicateFreeDayGame = new GameBuilder()
                .TestFreeDayGame()
                .New()
                .WithRound(anotherRound)
                .WithHomeTeamId(ANOTHER_TEAM_ID)
                .Build();

            MockGetTournamentDomain(TOURNAMENT_ID);
            MockGetTournamentById(TOURNAMENT_ID, new TournamentScheduleDtoBuilder().WithAnotherDivision().Build());
            MockGetTournamentResults(TOURNAMENT_ID, games);
            MockGetTeamsInTournament(TOURNAMENT_ID, new TeamInTournamentTestFixture().WithTeamsInTwoDivisionTwoGroups().Build());

            var sut = BuildSUT();

            // Act
            sut.Create(duplicateFreeDayGame);

            // Assert
            VerifyCreateGame(duplicateFreeDayGame, Times.Once());
        }

        [TestMethod]
        public void Create_SecondFreeDayInRoundInDifferentDivision_GameCreated()
        {
            // Arrange
            const int ANOTHER_DIVISION_TEAM_ID = 11;

            var games = new GameServiceTestFixture()
                .TestGamesWithFreeDay()
                .Build();

            var sameRound = games.Select(g => g.Round).Max();

            var duplicateFreeDayGame = new GameBuilder()
                .TestFreeDayGame()
                .New()
                .WithRound(sameRound)
                .WithHomeTeamId(ANOTHER_DIVISION_TEAM_ID)
                .Build();

            MockGetTournamentDomain(TOURNAMENT_ID);
            MockGetTournamentById(TOURNAMENT_ID, new TournamentScheduleDtoBuilder().WithAnotherDivision().Build());
            MockGetTournamentResults(TOURNAMENT_ID, games);
            MockGetTeamsInTournament(TOURNAMENT_ID, new TeamInTournamentTestFixture().WithTeamsInTwoDivisionTwoGroups().Build());

            var sut = BuildSUT();

            // Act
            sut.Create(duplicateFreeDayGame);

            // Assert
            VerifyCreateGame(duplicateFreeDayGame, Times.Once());
        }

        [TestMethod]
        public void Create_SecondFreeDayInSameRoundInSameDivision_ExceptionThrown()
        {
            // Arrange
            var exception = false;

            const int ANOTHER_TEAM_ID = 5;

            var games = new GameServiceTestFixture()
                .TestGamesWithFreeDay()
                .Build();

            var sameRound = games.Select(g => g.Round).Max();

            var duplicateFreeDayGame = new GameBuilder()
                .TestFreeDayGame()
                .New()
                .WithRound(sameRound)
                .WithHomeTeamId(ANOTHER_TEAM_ID)
                .Build();

            MockGetTournamentDomain(TOURNAMENT_ID);
            MockGetTournamentById(TOURNAMENT_ID, new TournamentScheduleDtoBuilder().WithAnotherDivision().Build());
            MockGetTournamentResults(TOURNAMENT_ID, games);
            MockGetTeamsInTournament(TOURNAMENT_ID, new TeamInTournamentTestFixture().WithTeamsInTwoDivisionTwoGroups().Build());

            var sut = BuildSUT();

            // Act
            try
            {
                sut.Create(duplicateFreeDayGame);
            }
            catch (ArgumentException)
            {
                exception = true;
            }

            // Assert
            Assert.IsTrue(exception);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_SameTeamInTwoGamesInOneRound_ExceptionThrown()
        {
            // Arrange
            var exceptionWasThrown = false;

            var gameInOneRound = new GameBuilder()
                .TestRoundGame()
                .Build();

            var gameInSameRound = new GameBuilder()
                .WithId(2)
                .TestRoundGame()
                .WithHomeTeamId(3)
                .Build();

            MockGetTournamentById(TOURNAMENT_ID, new TournamentScheduleDto());
            MockGetTournamentResults(1, new GameServiceTestFixture().TestGameResults().Build());

            var sut = BuildSUT();
            sut.Create(gameInOneRound);

            // Act
            try
            {
                sut.Create(gameInSameRound);
            }
            catch (ArgumentException)
            {
                exceptionWasThrown = true;
            }

            // Assert
            Assert.IsTrue(exceptionWasThrown);
        }

        [TestMethod]
        public void Create_SameGameTournamentSchemeOne_ExceptionThrown()
        {
            // Arrange
            var exceptionThrown = false;

            MockDefaultTournament();

            var gameInOtherRound = new GameBuilder()
                .TestRoundGame()
                .WithId(2)
                .WithRound(2)
                .Build();

            MockGetTournamentResults(
                gameInOtherRound.TournamentId,
                new GameServiceTestFixture().TestGamesForDuplicateSchemeOne().Build());

            var sut = BuildSUT();

            // Act
            try
            {
                sut.Create(gameInOtherRound);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void Create_SameGameSwitchedTeamsTournamentSchemeOne_ExceptionThrown()
        {
            // Arrange
            var exceptionThrown = false;

            MockDefaultTournament();

            var gameInOtherRound = new GameBuilder()
                .TestRoundGame()
                .WithId(2)
                .WithRound(2)
                .Build();

            MockGetTournamentResults(
               gameInOtherRound.TournamentId,
               new GameServiceTestFixture().TestGamesForDuplicateSchemeOne().Build());

            var sut = BuildSUT();

            // Act
            try
            {
                sut.Create(gameInOtherRound);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void Create_SameGameInOtherRoundTournamentSchemeTwo_ExceptionThrown()
        {
            // Arrange
            var exceptionThrown = false;

            MockDefaultTournament();

            var duplicate = new GameBuilder()
               .TestRoundGame()
               .WithRound(3)
               .WithId(3)
               .Build();

            MockGetTournamentResults(
              duplicate.TournamentId,
              new GameServiceTestFixture().TestGamesForDuplicateSchemeTwo().Build());

            var sut = BuildSUT();

            // Act
            try
            {
                sut.Create(duplicate);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void Create_TwoGamesWithSameTeamsOrderInTorunamentSchemeTwo_GameCreated()
        {
            // Arrange
            MockTournamentSchemeTwo();

            var duplicate = new GameBuilder()
              .TestRoundGame()
              .WithRound(3)
              .WithId(3)
              .Build();

            MockGetTournamentResults(
             duplicate.TournamentId,
             new GameServiceTestFixture().TestGamesForDuplicateSchemeTwo().Build());

            var sut = BuildSUT();

            // Act
            sut.Create(duplicate);

            // Assert
            VerifyCreateGame(duplicate, Times.Once());
        }

        [TestMethod]
        public void Create_ThirdDuplicateGameInTournamentSchemeTwo_ExceptionThrown()
        {
            // Arrange
            var exceptionThrown = false;

            MockTournamentSchemeTwo();

            var duplicate = new GameBuilder()
                .WithTournamentId(1)
                .TestRoundGame()
                .WithRound(4)
                .WithId(4)
                .Build();

            var gameResults = new GameServiceTestFixture()
                       .TestGamesSameTeamsSwitchedOrderTournamentSchemTwo()
                       .Build();
            MockGetTournamentResults(
                       duplicate.TournamentId,
                       gameResults);

            var sut = BuildSUT();

            // Act
            try
            {
                sut.Create(duplicate);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void Create_DuplicateFreeDayGameTournamentSchemeTwo_ExceptionThrown()
        {
            // Arrange
            var exceptionThrown = false;

            MockTournamentSchemeTwo();

            var freeDayGameDuplicate = new GameBuilder()
                .TestFreeDayGame()
                .WithId(3)
                .WithRound(3)
                .Build();

            var gameResults = new GameServiceTestFixture()
                .TestGamesWithTwoFreeDays()
                .Build();

            MockGetTournamentResults(
                freeDayGameDuplicate.TournamentId,
                gameResults);

            var sut = BuildSUT();

            // Act
            try
            {
                sut.Create(freeDayGameDuplicate);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void Create_DuplicateGamesInSameRound_ExceptionThrown()
        {
            // Arrange
            var exceptionThrown = false;

            MockDefaultTournament();

            var freeDayGmeInSameRound = new GameBuilder()
                .TestRoundGame()
                .WithId(2)
                .Build();

            var gameResults = new GameServiceTestFixture()
             .TestGamesForDuplicateSchemeOne()
             .Build();

            MockGetTournamentResults(
             freeDayGmeInSameRound.TournamentId,
             gameResults);

            var sut = BuildSUT();

            // Act
            try
            {
                sut.Create(freeDayGmeInSameRound);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void Create_DuplicateAwayTeamInGameInSameRound_ExceptionThrown()
        {
            // Arrange
            var exceptionThrown = false;

            MockDefaultTournament();

            var freeDayGameInSameRound = new GameBuilder()
                .TestRoundGame()
                .WithHomeTeamId(3)
                .WithId(2)
                .Build();

            var gameResults = new GameServiceTestFixture()
                .TestGamesForDuplicateSchemeOne()
                .Build();

            MockGetTournamentResults(
                freeDayGameInSameRound.TournamentId,
                gameResults);

            var sut = BuildSUT();

            // Act
            try
            {
                sut.Create(freeDayGameInSameRound);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void Create_DuplicateHomeTeamInGameInSameRound_ExceptionThrown()
        {
            // Arrange
            var exceptionThrown = false;

            MockDefaultTournament();

            var freeDayGmeInSameRound = new GameBuilder()
                .TestRoundGame()
                .WithAwayTeamId(3)
                .WithId(2)
                .Build();

            var gameResults = new GameServiceTestFixture()
                .TestGamesForDuplicateSchemeOne()
                .Build();

            MockGetTournamentResults(
                freeDayGmeInSameRound.TournamentId,
                gameResults);

            var sut = BuildSUT();

            // Act
            try
            {
                sut.Create(freeDayGmeInSameRound);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void Create_FifthSetScoreAsUsualSetScore_ExceptionThrown()
        {
            // Arrange
            Exception exception = null;
            MockDefaultTournament();
            var newGame = new GameBuilder().WithFifthSetScoreAsUsualSetScore().Build();
            var sut = BuildSUT();

            // Act
            try
            {
                sut.Create(newGame);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, ExpectedExceptionMessages.GAME_REQUIRED_SET_SCORES_15_0);
        }

        [TestMethod]
        public void Create_FifthSetScoreMoreThanMaxWithValidDifference_GameCreated()
        {
            // Arrange
            MockDefaultTournament();
            var newGame = new GameBuilder().WithFifthSetScoreMoreThanMaxWithValidDifference().Build();
            var sut = BuildSUT();

            // Act
            sut.Create(newGame);

            // Assert
            VerifyCreateGame(newGame, Times.Once());
        }

        [TestMethod]
        public void Create_FifthSetScoreMoreThanMaxWithInvalidDifference_ExceptionThrown()
        {
            // Arrange
            Exception exception = null;
            MockDefaultTournament();
            var newGame = new GameBuilder().WithFifthSetScoreMoreThanMaxWithInvalidDifference().Build();
            var sut = BuildSUT();

            // Act
            try
            {
                sut.Create(newGame);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, ExpectedExceptionMessages.GAME_REQUIRED_SET_SCORES_15_0);
        }

        [TestMethod]
        public void Create_FifthSetScoreLessThanMax_ExceptionThrown()
        {
            // Arrange
            Exception exception = null;
            MockDefaultTournament();
            var newGame = new GameBuilder().WithFifthSetScoreLessThanMax().Build();
            var sut = BuildSUT();

            // Act
            try
            {
                sut.Create(newGame);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, ExpectedExceptionMessages.GAME_REQUIRED_SET_SCORES_15_0);
        }

        [TestMethod]
        public void Create_FifthSetValidScore_GameCreated()
        {
            // Arrange
            MockDefaultTournament();
            var newGame = new GameBuilder().WithFifthSetScoreValid().Build();
            var sut = BuildSUT();

            // Act
            sut.Create(newGame);

            // Assert
            VerifyCreateGame(newGame, Times.Once());
        }

        [TestMethod]
        public void Create_SameTeamInTheRound_ExceptionThrown()
        {
            // Arrange
            MockDefaultTournament();
            var newGame = new GameBuilder()
                            .TestFreeDayGame()
                            .WithTournamentId(TOURNAMENT_ID)
                            .WithId(2)
                            .Build();

            var gameResults = new GameServiceTestFixture()
                .TestGamesForDuplicateSchemeOne()
                .Build();

            MockGetTournamentResults(
                newGame.TournamentId,
                gameResults);

            var sut = BuildSUT();

            ArgumentException argumentException = null;

            // Act
            try
            {
                sut.Create(newGame);
            }
            catch (ArgumentException ex)
            {
                argumentException = ex;
            }

            // Assert
            VerifyExceptionThrown(argumentException, ExpectedExceptionMessages.SAME_TEAM_IN_ROUND);
        }

        #endregion

        #region Get

        /// <summary>
        /// Test for Get method. Existing game in PlayOff is requested.
        /// Game is returned with WithAllowEditTotalScore = true.
        /// </summary>
        [TestMethod]
        public void Get_ExistingGameWithoutNextGamePlanned_GameReturnedWithAllowEditTotalScore()
        {
            // Arrange
            var testTournament = CreatePlayoffTournament();
            var games = new GameTestFixture()
                .TestGames()
                .Build();

            var expected = new GameResultDtoBuilder().WithId(GAME_RESULT_ID)
                                                     .WithAllowEditTotalScore(true)
                                                     .Build();
            var actual = new GameResultDtoBuilder().WithId(GAME_RESULT_ID)
                                                   .Build();

            MockGetById(actual);
            MockAllTournamentQueries(testTournament);
            MockGetTournamentResults(TOURNAMENT_ID, games);

            var sut = BuildSUT();

            // Act
            actual = sut.Get(GAME_RESULT_ID);

            // Assert
            TestHelper.AreEqual(expected, actual, new GameResultDtoComparer());
        }

        /// <summary>
        /// Test for Get method. Existing game in PlayOff is requested.
        /// Game is returned with WithAllowEditTotalScore = false, because next game is planned.
        /// </summary>
        [TestMethod]
        public void Get_ExistingGameWithNextGamePlanned_GameReturnedWithNotAllowEditTotalScore()
        {
            // Arrange
            var testTournament = CreatePlayoffTournament();
            var games = new GameTestFixture()
                .TestMinimumOddTeamsPlayOffSchedule()
                .TestGamesWithResults()
                .Build();

            var expected = new GameResultDtoBuilder().WithId(GAME_RESULT_ID)
                .Build();
            var actual = new GameResultDtoBuilder().WithId(GAME_RESULT_ID)
                .Build();
            
            MockGetById(actual);
            MockAllTournamentQueries(testTournament);
            MockGetTournamentResults(TOURNAMENT_ID, games);

            var sut = BuildSUT();

            // Act
            actual = sut.Get(GAME_RESULT_ID);

            // Assert
            TestHelper.AreEqual(expected, actual, new GameResultDtoComparer());
        }
        #endregion

        #region GetTournamentResults

        /// <summary>
        /// Test for GetTournamentResults method. Game results of specified tournament are requested. Game results are returned.
        /// </summary>
        [TestMethod]
        public void GetTournamentResults_GameResultsRequsted_GameResultsReturned()
        {
            // Arrange
            var existingGames = new GameServiceTestFixture().TestGameResults().Build();
            var expected = new GameServiceTestFixture().TestGameResults().Build();

            MockGetTournamentById(TOURNAMENT_ID, new TournamentScheduleDtoBuilder().Build());
            MockGetTournamentResults(TOURNAMENT_ID, existingGames);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetTournamentResults(TOURNAMENT_ID);

            // Assert
            TestHelper.AreEqual(expected, actual, new GameResultDtoComparer());
        }

        /// <summary>
        /// Test for GetTournamentResults method. Game results of specified tournament are requested. Game results are returned.
        /// </summary>
        [TestMethod]
        public void GetTournamentResults_HasGamesWithoutResults_OnlyGamesWithResultsReturned()
        {
            // Arrange
            var existingGames = new GameServiceTestFixture()
                .TestGamesWithFreeDay()
                .TestGameResults()
                .Build();
            var expected = new GameServiceTestFixture()
                .TestGameResults()
                .Build();

            MockGetTournamentById(TOURNAMENT_ID, new TournamentScheduleDtoBuilder().Build());
            MockGetTournamentResults(TOURNAMENT_ID, existingGames);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetTournamentResults(TOURNAMENT_ID);

            // Assert
            TestHelper.AreEqual(expected, actual, new GameResultDtoComparer());
        }

        [TestMethod]
        public void GetTournamentGames_GamesRequested_AllScheduledReturned()
        {
            // Arrange
            var existingGames = new GameServiceTestFixture().TestGameResults().Build();
            var expected = new GameServiceTestFixture().TestGameResults().Build();

            MockGetTournamentById(TOURNAMENT_ID, new TournamentScheduleDtoBuilder().Build());
            MockGetTournamentResults(TOURNAMENT_ID, existingGames);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetTournamentGames(TOURNAMENT_ID);

            // Assert
            TestHelper.AreEqual(expected, actual, new GameResultDtoComparer());
        }

        [TestMethod]
        public void GetPlayoffTournamentGames_FirstRoundGameHasNoTeams_PlaceholdersAreUsed()
        {
            // Arrange
            const int NUMBER_OF_GAMES_IN_FIRST_ROUND = 4;
            var existingGames = new GameServiceTestFixture().TestEmptyPlayoffFor6Teams().Build();

            MockGetTournamentById(TOURNAMENT_ID, new TournamentScheduleDtoBuilder().WithScheme(TournamentSchemeEnum.PlayOff).Build());
            MockGetTournamentResults(TOURNAMENT_ID, existingGames);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetTournamentGames(TOURNAMENT_ID);

            // Assert
            AssertPlaceholdersAreUsed(actual, NUMBER_OF_GAMES_IN_FIRST_ROUND);
        }

        [TestMethod]
        public void GetPlayoffTournamentGames_NonFirstRoundGameHasNoTeams_GameDependenciesAreSet()
        {
            // Arrange
            const int FIRST_SEMIFINAL_ID = 5;
            const int SECOND_SEMIFINAL_ID = 6;
            const int FINAL_ID = 8;
            var existingGames = new GameServiceTestFixture().TestEmptyPlayoffFor6Teams().Build();

            MockGetTournamentById(TOURNAMENT_ID, CreatePlayoffTournament());
            MockGetTournamentResults(TOURNAMENT_ID, existingGames);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetTournamentGames(TOURNAMENT_ID);

            // Assert
            AssertWinnerGameDependenciesAreSet(actual, FIRST_SEMIFINAL_ID, 1, 2);
            AssertWinnerGameDependenciesAreSet(actual, SECOND_SEMIFINAL_ID, 3, 4);
            AssertWinnerGameDependenciesAreSet(actual, FINAL_ID, FIRST_SEMIFINAL_ID, SECOND_SEMIFINAL_ID);
        }



        [TestMethod]
        public void GetPlayoffTournamentGames_NonFirstRoundGameHasOnlyOneTeams_GameDependenciesAreSetForMissingTeam()
        {
            // Arrange
            const string TEAM_NAME = "Team A";
            var existingGames = new GameServiceTestFixture().TestEmptyPlayoffFor6Teams().Build();
            var firstGameOfSecondRound = existingGames.Single(g => g.Id == 5);
            firstGameOfSecondRound.HomeTeamId = 1;
            firstGameOfSecondRound.HomeTeamName = TEAM_NAME;

            MockGetTournamentById(TOURNAMENT_ID, CreatePlayoffTournament());
            MockGetTournamentResults(TOURNAMENT_ID, existingGames);

            const int DEPENDENT_GAME_ID = 5;

            var sut = BuildSUT();

            // Act
            var actual = sut.GetTournamentGames(TOURNAMENT_ID);

            // Assert
            var game = actual.FirstOrDefault(g => g.Id == DEPENDENT_GAME_ID);
            Assert.IsNotNull(game, $"Game with id={DEPENDENT_GAME_ID} should exist");
            Assert.AreEqual(
                TEAM_NAME,
                game.HomeTeamName,
                $"[GameId:{game.Id}] Team name should be set");
            Assert.AreEqual(
                $"Winner2",
                game.AwayTeamName,
                $"[GameId:{game.Id}] Winner of upstream game should be set as Away team name");
        }

        [TestMethod]
        public void GetPlayoffTournamentGames_BronzeGameHasNoTeams_GameDependenciesAreSetFromLoosers()
        {
            // Arrange
            const int FIRST_SEMIFINAL_ID = 5;
            const int SECOND_SEMIFINAL_ID = 6;
            const int BRONZE_GAME_ID = 7;
            var existingGames = new GameServiceTestFixture().TestEmptyPlayoffFor6Teams().Build();

            MockGetTournamentById(TOURNAMENT_ID, CreatePlayoffTournament());
            MockGetTournamentResults(TOURNAMENT_ID, existingGames);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetTournamentGames(TOURNAMENT_ID);

            // Assert
            AssertLooserGameDependenciesAreSet(actual, BRONZE_GAME_ID, FIRST_SEMIFINAL_ID, SECOND_SEMIFINAL_ID);
        }

        #endregion

        #region Edit

        /// <summary>
        /// Test for Edit method. Tournament last date which was updated is today.
        /// Game is edited successfully.
        /// </summary>
        [TestMethod]
        public void Edit_GameEdited_LastTimeNotUpdated()
        {
            // Arrange
            MockDefaultTournament();
            var tour = new TournamentBuilder().Build();
            var expected = tour.LastTimeUpdated;

            _tournamentByIdQueryMock.Setup(tr => tr.Execute(It.IsAny<FindByIdCriteria>())).Returns(tour);

            var existingGame = new GameResultDtoBuilder()
                .WithId(GAME_RESULT_ID)
                .Build();

            MockGetTournamentResults(existingGame);

            var game = new GameBuilder()
                .WithId(GAME_RESULT_ID)
                .WithNullResult()
                .Build();

            var sut = BuildSUT();

            // Act
            sut.Edit(game);

            // Assert
            Assert.AreEqual(expected, tour.LastTimeUpdated);
        }

        /// <summary>
        /// Test for Edit method. Tournament last date which was updated is today.
        /// Game result is edited successfully.
        /// </summary>
        [TestMethod]
        public void Edit_GameResultEdited_LastTimeUpdated()
        {
            // Arrange
            MockDefaultTournament();
            var tour = new TournamentBuilder().Build();
            _tournamentByIdQueryMock.Setup(tr => tr.Execute(It.IsAny<FindByIdCriteria>())).Returns(tour);
            var existingGames = new List<GameResultDto> { new GameResultDtoBuilder().WithId(GAME_RESULT_ID).Build() };
            var game = new GameBuilder().WithId(GAME_RESULT_ID).Build();
            MockGetTournamentResults(TOURNAMENT_ID, existingGames);

            var sut = BuildSUT();

            // Act
            sut.EditGameResult(game);

            // Assert
            Assert.AreEqual(TimeProvider.Current.UtcNow, tour.LastTimeUpdated);
        }

        /// <summary>
        /// Test for Edit method. Game object contains valid data. Game is edited successfully.
        /// </summary>
        [TestMethod]
        public void Edit_GameValid_GameEdited()
        {
            // Arrange
            MockDefaultTournament();
            var existingGames = new List<GameResultDto> { new GameResultDtoBuilder().WithId(GAME_RESULT_ID).Build() };
            var game = new GameBuilder().WithId(GAME_RESULT_ID).Build();
            MockGetTournamentResults(TOURNAMENT_ID, existingGames);

            var sut = BuildSUT();

            // Act
            sut.Edit(game);

            // Assert
            VerifyEditGame(game, Times.Once());
        }

        /// <summary>
        /// Test for Edit method. Change time for game in second round with unknown teams. 
        /// No exceprtion returns and game is edited successfully.
        /// </summary>
        [TestMethod]
        public void Edit_ChangeTimeForPlayOffGameWithUnknownTeams_GameSaved()
        {
            //Arrange
            var testTournament = CreatePlayoffTournament();
            var games = new GameTestFixture()
                .TestEmptyGamePlayoffSchedule()
                .Build();

            var testGameForEdit = GetTestGameForEditFromSecondRoundInPlayOff(games);
            SetDafaultTimeForGame(testGameForEdit);

            MockAllTournamentQueries(testTournament);
            MockGetTournamentResults(TOURNAMENT_ID, games);

            //Act
            var sut = BuildSUT();
            sut.Edit(testGameForEdit);

            //Assert
            VerifyEditGame(testGameForEdit, Times.Once());
        }

        /// <summary>
        /// Test for Edit method. Change time for planned game in second round with defined teams. 
        /// No exceprtion returns and game is edited successfully.
        /// </summary>
        [TestMethod]
        public void Edit_ChangeTimeForPlayOffGameWithDefinedTeams_GameSaved()
        {
            //Arrange
            var testTournament = CreatePlayoffTournament();
            var games = new GameTestFixture()
                .TestEmptyGamePlayoffSchedule()
                .Build();

            var testGameForEdit = GetTestGameForEditFromSecondRoundInPlayOff(games);
            SetDafaultTimeForGame(testGameForEdit);
            SetTeamsInGame(testGameForEdit);

            MockAllTournamentQueries(testTournament);
            MockGetTournamentResults(TOURNAMENT_ID, games);

            //Act
            var sut = BuildSUT();
            sut.Edit(testGameForEdit);

            //Assert                    
            VerifyEditGame(testGameForEdit, Times.Once());
        }

        /// <summary>
        /// Test for Edit method. Game object contains valid data. Game is edited successfully.
        /// </summary>
        [TestMethod]
        public void Edit_DateChangedExistingGameHasResult_ResultsPersisted()
        {
            const string DATE = "2016-04-04 10:00";

            // Arrange
            MockDefaultTournament();

            var existingGame = new GameResultDtoBuilder().WithId(GAME_RESULT_ID).Build();

            var game = new GameBuilder()
                .WithId(GAME_RESULT_ID)
                .WithStartDate(DateTime.Parse(DATE))
                .Build();

            MockGetTournamentResults(existingGame);

            var sut = BuildSUT();

            // Act
            sut.Edit(game);

            // Assert
            VerifyEditGame(game, Times.Once());
        }

        /// <summary>
        /// Test for Edit method. Game is missing and cannot be edited. Exception is thrown during editing.
        /// </summary>
        [TestMethod]
        public void Edit_MissingGame_ExceptionThrown()
        {
            Exception exception = null;

            // Arrange
            MockDefaultTournament();
            var game = new GameBuilder().Build();
            var sut = BuildSUT();

            MockEditThrowsMissingEntityException(game);

            // Act
            try
            {
                sut.Edit(game);
            }
            catch (MissingEntityException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, ExpectedExceptionMessages.CONCURRENCY_EXCEPTION);
        }

        [TestMethod]
        public void EditPlayOff_AddResultsToGame_NextGameIsScheduled()
        {
            // Arrange
            MockDefaultTournament();

            var games = new GameTestFixture()
                .TestEmptyGamePlayoffSchedule()
                .Build();

            var gameInfo = new GameServiceTestFixture()
                .TestPlayoffGamesWithoutResults()
                .Build();

            MockTournamentSchemePlayoff(
                gameInfo,
                games);

            var finishedGame = BuildTestGameToEditInPlayoff();

            var sut = BuildSUT();

            var newScheduledGame = games
                    .Where(g => g.GameNumber == 5)
                    .SingleOrDefault();

            // Act
            sut.Edit(finishedGame);

            // Assert
            VerifyEditGames(
                new List<Game>
                {
                    finishedGame,
                    newScheduledGame
                },
                Times.AtLeastOnce());
        }

        [TestMethod]
        public void EditPlayoff_AddedDayOffGame_NextGameIsScheduled()
        {
            // Arrange
            MockDefaultTournament();

            var games = new GameTestFixture().TestEmptyGamePlayoffSchedule().Build();

            var gameInfo = new GameServiceTestFixture().TestPlayoffGamesWithoutResults().Build();

            MockTournamentSchemePlayoff(
                gameInfo,
                games);

            var dayOffGame = new GameBuilder()
                .TestFreeDayGame()
                .WithId(1)
                .WithGameNumber(1)
                .WithRound(1)
                .Build();

            var expectedNextGame = new Game
            {
                Id = 5,
                Round = 2,
                GameNumber = 5,
                TournamentId = 1,
                Result = new Result(),
                HomeTeamId = dayOffGame.HomeTeamId,
                AwayTeamId = null
            };

            var sut = BuildSUT();

            // Act
            sut.Edit(dayOffGame);

            // Assert
            VerifyEditGames(
                new List<Game>
                {
                    dayOffGame,
                    expectedNextGame
                },
                Times.AtLeastOnce());
        }

        /// <summary>
        /// Test method checks that 2 same teams(not null) can't be in one game in PlayOff scheme.
        /// Argument exception thrown. 
        /// </summary>
        [TestMethod]
        public void Edit_BothTeamsEqualInPlayOff_ArgumentExceptionThrown()
        {
            Exception exception = null;

            // Arrange
            var games = new GameTestFixture()
                .SameTeamsInOneGamePlayOffScheme()
                .Build();

            var gameInfo = new GameServiceTestFixture()
                .SameTeamsInOneGamePlayOffScheme()
                .Build();

            MockTournamentSchemePlayoff(
                gameInfo,
                games);

            var gameToEdit = new GameBuilder()
                .WithId(1)
                .WithGameNumber(1)
                .WithRound(1)
                .WithTheSameTeams()
                .Build();

            var sut = BuildSUT();

            // Act
            try
            {
                sut.Edit(gameToEdit);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, ExpectedExceptionMessages.GAME_SAME_TEAM);
        }

        [TestMethod]
        public void Edit_SeveralDayOffGamesInPlayoff_GameEdited()
        {
            // Arrange
            const int ANOTHER_PLAYOFF_GAME_ID = 4;

            var games = new GameTestFixture()
                .MinimalPlannedPlayOffWithPreliminaryStage()
                .ResetPlayoffGame(ANOTHER_PLAYOFF_GAME_ID)
                .Build();

            var gameInfo = new GameServiceTestFixture()
                .TestMinimalPlannedPlayOffWithPreliminaryStage()
                .ResetPlayoffGame(ANOTHER_PLAYOFF_GAME_ID)
                .Build();

            MockTournamentSchemePlayoff(
                gameInfo,
                games);

            MockGetTeamsInTournament(1, new TeamInTournamentTestFixture().With8TeamsPlayoff().Build());

            var gameToEdit = CreateGameToEdit(ANOTHER_PLAYOFF_GAME_ID);

            var sut = BuildSUT();

            // Act
            sut.Edit(gameToEdit);

            // Assert
            VerifyEditGame(gameToEdit, Times.Once());
        }

        [TestMethod]
        public void Edit_BothTeamsNotSetInPlayOff_GameEdited()
        {
            // Arrange
            MockDefaultTournament();

            var games = new GameTestFixture()
                .SameTeamsInOneGamePlayOffScheme()
                .Build();

            var gameInfo = new GameServiceTestFixture()
                .SameTeamsInOneGamePlayOffScheme()
                .Build();

            MockTournamentSchemePlayoff(
                gameInfo,
                games);

            var gameToEdit = new GameBuilder()
                .WithId(3)
                .WithGameNumber(3)
                .WithRound(2)
                .WithHomeTeamId(null)
                .WithAwayTeamId(null)
                .WithDefaultResult()
                .Build();

            var sut = BuildSUT();

            // Act
            sut.Edit(gameToEdit);

            // Assert
            VerifyEditGame(gameToEdit, Times.Once());
        }

        [TestMethod]
        public void Edit_AddResultsToPlayoffTournamentWithMinimalOddTeams_NewGameIsScheduled()
        {
            // Arrange
            MockDefaultTournament();

            var games = new GameTestFixture()
                .TestMinimumOddTeamsPlayOffSchedule()
                .Build();

            var gameInfo = new GameServiceTestFixture()
                .TestMinimumOddTeamsPlayOffSchedule()
                .Build();

            MockTournamentSchemePlayoff(
                gameInfo,
                games);

            MockGetTournamentById(TOURNAMENT_ID, new TournamentScheduleDtoBuilder().Build());

            var finishedGame = new GameBuilder()
                .WithId(2)
                .WithGameNumber(2)
                .WithRound(1)
                .WithSetsScore(new Score
                {
                    Home = 1,
                    Away = 3
                })
                .WithSetScores(
                new List<Score>
                {
                    new Score
                    {
                        Home = 10,
                        Away = 25
                    },
                    new Score
                    {
                        Home = 5,
                        Away = 25
                    },
                    new Score
                    {
                        Home = 25,
                        Away = 10
                    },
                    new Score
                    {
                        Home = 10,
                        Away = 25
                    }
                })
                .Build();

            var sut = BuildSUT();

            // Act
            sut.Edit(finishedGame);

            // Assert
            VerifyEditGames(
                new List<Game>
                {
                    finishedGame
                },
                Times.Once());
        }

        [TestMethod]
        public void Edit_AddResultsToPlayoffTournamentWithMinimalEvenTeams_NewGameIsScheduled()
        {
            // Arrange
            MockDefaultTournament();

            var games = new GameTestFixture()
                .TestMinimumEvenTeamsPlayOffSchedule()
                .Build();

            var gameInfo = new GameServiceTestFixture()
                .TestMinimumEvenEmptyGamesPlayoff()
                .Build();

            MockTournamentSchemePlayoff(
                gameInfo,
                games);

            var finishedGame = BuildTestGameToEditInPlayoff();

            var sut = BuildSUT();

            // Act
            sut.Edit(finishedGame);

            var newScheduledGame = games
                .Where(g => g.GameNumber == 3)
                .SingleOrDefault();

            // Assert
            VerifyEditGames(
                new List<Game>
                {
                    finishedGame,
                    newScheduledGame
                },
                Times.AtLeastOnce());
        }

        #endregion

        #region Delete

        /// <summary>
        /// Test for Delete method. Existing game has to be deleted. Game is deleted.
        /// </summary>
        [TestMethod]
        public void Delete_ExistingGame_GameDeleted()
        {
            // Arrange
            var now = TimeProvider.Current.UtcNow;

            var game = new GameResultDtoBuilder().WithDate(now.AddDays(ONE_DAY))
                    .WithAwaySetsScore(0).WithHomeSetsScore(0).Build();

            MockGetById(game);

            var sut = BuildSUT();

            // Act
            sut.Delete(GAME_RESULT_ID);

            // Assert
            VerifyDeleteGame(GAME_RESULT_ID, Times.Once());
        }

        /// <summary>
        /// Test for Delete method. Existing game has not to be deleted.
        /// Invalid game.
        /// </summary>
        [TestMethod]
        public void Delete_EndedGame_ThrowArgumentExeption()
        {
            // Arrange
            Exception exception = null;
            var now = TimeProvider.Current.UtcNow;
            var game = new GameResultDtoBuilder().WithId(GAME_RESULT_ID)
                                                 .WithDate(now.AddDays(-ONE_DAY)).Build();

            MockGetById(game);
            var sut = BuildSUT();

            // Act
            try
            {
                sut.Delete(GAME_RESULT_ID);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, ExpectedExceptionMessages.WRONG_DELETING_GAME);
        }

        /// <summary>
        /// Test for Delete method. Existing game has not to be deleted.
        /// Invalid game.
        /// </summary>
        [TestMethod]
        public void Delete_GameHasResult_ThrowArgumentExeption()
        {
            // Arrange
            Exception exception = null;
            var game = new GameResultDtoBuilder().WithId(GAME_RESULT_ID)
                                .WithAwaySetsScore(TEST_AWAY_SET_SCORS)
                                .WithHomeSetsScore(TEST_HOME_SET_SCORS).Build();

            MockGetById(game);
            var sut = BuildSUT();

            // Act
            try
            {
                sut.Delete(GAME_RESULT_ID);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, ExpectedExceptionMessages.WRONG_DELETING_GAME);
        }

        /// <summary>
        /// Test for Delete method. The game result instance is null.
        /// Exception is thrown during removing.
        /// </summary>
        [TestMethod]
        public void Delete_GameNull_ExceptionThrown()
        {
            Exception exception = null;

            // Arrange
            var gameNullId = 0;
            var sut = BuildSUT();

            // Act
            try
            {
                sut.Delete(gameNullId);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, ExpectedExceptionMessages.GAME_INVALID_ID);
        }
        #endregion

        #region SwapRounds

        /// <summary>
        /// Test for SwapRounds method. Swap rounds in existing games.
        /// </summary>
        [TestMethod]
        public void SwapRounds_AllGamesExist_GamesSwapped()
        {
            // Arrange
            MockDefaultTournament();
            var games = new List<Game> { new GameBuilder().WithRound(FIRST_ROUND_NUMBER).Build() };
            games.Add(new GameBuilder().WithRound(SECOND_ROUND_NUMBER).Build());
            var expectedGames = new List<Game> { new GameBuilder().WithRound(SECOND_ROUND_NUMBER).Build() };
            expectedGames.Add(new GameBuilder().WithRound(FIRST_ROUND_NUMBER).Build());

            _gamesByTournamentIdRoundsNumberQueryMock
                .Setup(m => m.Execute(It.IsAny<TournamentRoundsGameResultsCriteria>()))
                .Returns(expectedGames);

            var sut = BuildSUT();

            // Act
            sut.SwapRounds(TOURNAMENT_ID, FIRST_ROUND_NUMBER, SECOND_ROUND_NUMBER);

            // Assert
            Assert.IsTrue(Enumerable.SequenceEqual(
                                        games,
                                        expectedGames,
                                        new GameMvcEqualityComparer()));
        }

        /// <summary>
        /// Test for SwapRounds method. Games are missing and cannot be swapped.
        /// Exception is thrown during editing.
        /// </summary>
        [TestMethod]
        public void SwapRounds_MissingGames_ExceptionThrown()
        {
            Exception exception = null;
            const byte WRONG_ROUND_NUMBER = 0;

            // Arrange
            MockDefaultTournament();
            var games = new List<Game> { new GameBuilder().Build() };

            _gamesByTournamentIdRoundsNumberQueryMock
                .Setup(m => m.Execute(It.IsAny<TournamentRoundsGameResultsCriteria>()))
                .Returns(games);
            var sut = BuildSUT();

            // Act
            try
            {
                foreach (var game in games)
                {
                    MockEditThrowsMissingEntityException(game);
                }

                sut.SwapRounds(TOURNAMENT_ID, WRONG_ROUND_NUMBER, WRONG_ROUND_NUMBER);
            }
            catch (MissingEntityException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, ExpectedExceptionMessages.CONCURRENCY_EXCEPTION);
        }
        #endregion

        #region AddGamesInTournament

        /// <summary>
        /// Test for AddGamesInTournament method. Add games in tournament.
        /// </summary>
        [TestMethod]
        public void AddGamesInTournament_GamesCollectionExists_GamesAdded()
        {
            // Arrange
            var gamesToAdd = new GameTestFixture().TestGames().Build();
            var sut = BuildSUT();

            // Act
            sut.AddGames(gamesToAdd);

            // Assert
            VerifyGamesAdded(Times.Exactly(gamesToAdd.Count));
        }

        /// <summary>
        /// Test for AddGamesInTournament method. Don't add games in tournament.
        /// </summary>
        [TestMethod]
        public void AddGamesInTournament_EmptyGamesCollection_GamesNotAdded()
        {
            // Arrange
            var gamesToAdd = new GameTestFixture().Build();
            var sut = BuildSUT();

            // Act
            sut.AddGames(gamesToAdd);

            // Assert
            VerifyGamesAdded(Times.Never());
        }

        #endregion

        #region RemoveAllGamesInTournament

        /// <summary>
        /// Test for RemoveAllGamesInTournament method. Remove games from tournament.
        /// </summary>
        [TestMethod]
        public void RemoveAllGamesInTournament_GamesExists_GamesRemoved()
        {
            // Arrange
            var gamesInTournament = new GameServiceTestFixture().TestGamesWithoutResult().Build();

            MockGetTournamentResults(TOURNAMENT_ID, gamesInTournament);
            MockGetTournamentById(TOURNAMENT_ID, new TournamentScheduleDtoBuilder().Build());

            var sut = BuildSUT();

            // Act
            sut.RemoveAllGamesInTournament(TOURNAMENT_ID);

            // Assert
            VerifyGamesRemoved(Times.Exactly(gamesInTournament.Count));
        }

        /// <summary>
        /// Test for RemoveAllGamesInTournament method. Don't remove games from tournament.
        /// </summary>
        [TestMethod]
        public void RemoveAllGamesInTournament_NoGamesInTournament_GamesNotRemoved()
        {
            // Arrange
            var gamesInTournament = new GameServiceTestFixture().Build();

            MockGetTournamentResults(TOURNAMENT_ID, gamesInTournament);
            MockGetTournamentById(TOURNAMENT_ID, new TournamentScheduleDtoBuilder().Build());

            var sut = BuildSUT();

            // Act
            sut.RemoveAllGamesInTournament(TOURNAMENT_ID);

            // Assert
            VerifyGamesRemoved(Times.Never());
        }

        #endregion

        #region Authorization game tests

        /// <summary>
        /// Test for Create() method with no permission for such action. The method has to throw AuthorizationException,
        /// should invoke CheckAccess() and shouldn't invoke Commit() method of IUnitOfWork.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AuthorizationException))]
        public void Create_CreateNotPermitted_ExceptionThrown()
        {
            // Arrange
            var testData = new GameBuilder().WithId(SPECIFIC_GAME_ID).Build();
            MockAuthServiceThrowsExeption(AuthOperations.Games.Create);

            var sut = BuildSUT();

            // Act
            sut.Create(testData);

            // Assert
            VerifyCreateGame(testData, Times.Never(), Times.Once());
            VerifyCheckAccess(AuthOperations.Games.Create, Times.Once());
        }

        /// <summary>
        /// Test for Delete() method with no permission for such action. The method has to throw AuthorizationException,
        /// should invoke CheckAccess() and shouldn't invoke Commit() method of IUnitOfWork
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AuthorizationException))]
        public void Delete_DeleteNotPermitted_ExceptionThrown()
        {
            // Arrange
            var testData = new GameBuilder().WithId(SPECIFIC_GAME_ID).Build();
            MockAuthServiceThrowsExeption(AuthOperations.Games.Delete);

            var sut = BuildSUT();

            // Act
            sut.Delete(testData.Id);

            // Assert
            VerifyDeleteGame(testData.Id, Times.Never());
            VerifyCheckAccess(AuthOperations.Games.Delete, Times.Once());
        }

        /// <summary>
        /// Test for Edit() method with no permission for such action. The method has to throw AuthorizationException,
        /// should invoke CheckAccess() and shouldn't invoke Commit() method of IUnitOfWork
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AuthorizationException))]
        public void Edit_EditNotPermitted_ExceptionThrown()
        {
            // Arrange
            var testData = new GameBuilder().WithId(SPECIFIC_GAME_ID).Build();
            MockAuthServiceThrowsExeption(AuthOperations.Games.Edit);

            var sut = BuildSUT();

            // Act
            sut.Edit(testData);

            // Assert
            VerifyEditGame(testData, Times.Never());
            VerifyCheckAccess(AuthOperations.Games.Edit, Times.Once());
        }

        /// <summary>
        /// Test for EditGameResult() method with no permission for such action. The method has to throw AuthorizationException,
        /// should invoke CheckAccess() and shouldn't invoke Commit() method of IUnitOfWork
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AuthorizationException))]
        public void EditGameResult_EditNotPermitted_ExceptionThrown()
        {
            // Arrange
            var testData = new GameBuilder().WithId(SPECIFIC_GAME_ID).Build();
            MockAuthServiceThrowsExeption(AuthOperations.Games.EditResult);

            var sut = BuildSUT();

            // Act
            sut.EditGameResult(testData);

            // Assert
            VerifyEditGame(testData, Times.Never());
            VerifyCheckAccess(AuthOperations.Games.EditResult, Times.Once());
        }

        /// <summary>
        /// Test for SwapRounds() method with no permission for such action. The method has to throw AuthorizationException,
        /// should invoke CheckAccess() and shouldn't invoke Commit() method of IUnitOfWork
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AuthorizationException))]
        public void SwapRounds_SwapRoundsNotPermitted_ExceptionThrown()
        {
            // Arrange
            MockDefaultTournament();
            var games = new List<Game>
                            {
                                new GameBuilder().WithRound(FIRST_ROUND_NUMBER).Build(),
                                new GameBuilder().WithRound(SECOND_ROUND_NUMBER).Build()
                            };
            var expectedGames = new List<Game>
                            {
                                new GameBuilder().WithRound(SECOND_ROUND_NUMBER).Build(),
                                new GameBuilder().WithRound(FIRST_ROUND_NUMBER).Build()
                            };

            MockAuthServiceThrowsExeption(AuthOperations.Games.SwapRounds);
            _gamesByTournamentIdRoundsNumberQueryMock
                .Setup(m => m.Execute(It.IsAny<TournamentRoundsGameResultsCriteria>()))
                .Returns(expectedGames);

            var sut = BuildSUT();

            // Act
            sut.SwapRounds(TOURNAMENT_ID, FIRST_ROUND_NUMBER, SECOND_ROUND_NUMBER);

            // Assert
            VerifyCheckAccess(AuthOperations.Games.SwapRounds, Times.Once());
        }

        #endregion

        #region Creation Methods

        private GameService BuildSUT()
        {
            return new GameService(
                _gameRepositoryMock.Object,
                _getByIdQueryMock.Object,
                _tournamentGameResultsQueryMock.Object,
                _tournamentScheduleDtoByIdQueryMock.Object,
                _gamesByTournamentIdRoundsNumberQueryMock.Object,
                _authServiceMock.Object,
                _gamesByTournamentIdInRoundsByNumbersQueryMock.Object,
                _gameNumberByTournamentIdQueryMock.Object,
                _tournamentByIdQueryMock.Object,
                _tournamentRepositoryMock.Object,
                _tournamentTeamsQueryMock.Object);
        }

        private Game BuildTestGameToEditInPlayoff()
        {
            return new GameBuilder()
                .WithId(1)
                .WithGameNumber(1)
                .WithRound(1)
                .WithSetsScore(new Score
                {
                    Home = 3,
                    Away = 0
                })
                .Build();
        }

        private static Game CreateGameToEdit(int anotherPlayoffGameId)
        {
            return new GameBuilder()
                .WithId(anotherPlayoffGameId)
                .WithGameNumber(4)
                .WithRound(1)
                .WithHomeTeamId(8)
                .WithDayOff()
                .Build();
        }
        private static Game GetTestGameForEditFromSecondRoundInPlayOff(IEnumerable<Game> games)
        {
            var testTeam = games.FirstOrDefault(g => g.Round == 2);

            return testTeam;
        }

        private static void SetDafaultTimeForGame(Game game)
        {
            game.GameDate = DateTime.Parse(TOURNAMENT_DATE_START);
        }

        private static void SetTeamsInGame(Game game)
        {
            game.HomeTeamId = 1;
            game.AwayTeamId = 3;
        }

        private static TournamentScheduleDto CreatePlayoffTournament()
        {
            var result = new TournamentScheduleDtoBuilder().WithScheme(TournamentSchemeEnum.PlayOff).Build();

            result.Divisions.First().NumberOfRounds = 3;
            result.Divisions.First().TeamCount = 6;

            return result;
        }

        #endregion

        #region Mock Helpers

        private void MockGetById(GameResultDto gameResult)
        {
            _getByIdQueryMock
                .Setup(m => m.Execute(It.Is<FindByIdCriteria>(c => c.Id == gameResult.Id)))
                .Returns(gameResult);
        }

        private void MockGetTournamentResults(int tournamentId, List<GameResultDto> gameResults)
        {
            _tournamentGameResultsQueryMock.Setup(m =>
                    m.Execute(It.Is<TournamentGameResultsCriteria>(c => c.TournamentId == tournamentId)))
                .Returns(gameResults);
        }

        private void MockGetTournamentResults(int tournamentId, List<Game> gameResults)
        {
            _gamesByTournamentIdInRoundsByNumbersQueryMock.Setup(m =>
                    m.Execute(It.Is<GamesByRoundCriteria>(
                        c => c.TournamentId == tournamentId
                             && c.RoundNumbers.Any(n => gameResults.Any(gr => gr.Round == n)))))
                .Returns(gameResults);
        }

        private void MockGetTournamentById(int id, TournamentScheduleDto tournament)
        {
            _tournamentScheduleDtoByIdQueryMock.Setup(m =>
                    m.Execute(It.Is<TournamentScheduleInfoCriteria>(c => c.TournamentId == id)))
                .Returns(tournament);
        }

        private void MockEditThrowsMissingEntityException(Game game)
        {
            _gameRepositoryMock.Setup(m =>
                    m.Update(It.Is<Game>(grs => AreGamesEqual(grs, game))))
                .Throws(new ConcurrencyException());
        }

        private void MockDefaultTournament()
        {
            var tournament = new TournamentScheduleDtoBuilder()
                .TestTournamemtSchedultDto()
                .WithScheme(TournamentSchemeEnum.One)
                .Build();

            MockAllTournamentQueries(tournament);
        }

        private void MockTournamentSchemeTwo()
        {
            var tournament = new TournamentScheduleDtoBuilder()
                .TestTournamemtSchedultDto()
                .WithScheme(TournamentSchemeEnum.Two)
                .Build();

            MockAllTournamentQueries(tournament);
        }

        private void MockAllTournamentQueries(TournamentScheduleDto testData)
        {
            MockGetTournamentDomain(testData.Id);

            MockGetTournamentById(testData.Id, testData);
            MockGetTournamentResults(testData.Id, new List<GameResultDto>());

            MockGetTeamsInTournament(testData.Id, new TeamInTournamentTestFixture().WithTeamsInSingleDivisionSingleGroup().Build());
        }

        private void MockGetTournamentDomain(int id)
        {
            var tour = new TournamentBuilder().WithId(id).Build();
            _tournamentByIdQueryMock.Setup(tr => tr.Execute(It.IsAny<FindByIdCriteria>())).Returns(tour);
        }

        private void MockGetTeamsInTournament(int tournamentId, List<TeamTournamentDto> testData)
        {
            _tournamentTeamsQueryMock
                .Setup(q => q.Execute(It.Is<FindByTournamentIdCriteria>(c => c.TournamentId == tournamentId)))
                .Returns(testData);
        }

        private void MockAuthServiceThrowsExeption(AuthOperation operation)
        {
            _authServiceMock.Setup(tr => tr.CheckAccess(operation)).Throws<AuthorizationException>();
        }

        private void MockTournamentServiceReturnTournament()
        {
            var tour = new TournamentBuilder().Build();
            _tournamentServiceMock.Setup(ts => ts.Get(It.IsAny<int>())).Returns(tour);
        }

        private void MockTournamentSchemePlayoff(List<GameResultDto> allGames, List<Game> games)
        {
            var tournament = new TournamentScheduleDtoBuilder()
                .TestTournamemtSchedultDto()
                .WithScheme(TournamentSchemeEnum.PlayOff)
                .Build();

            MockGetTournamentById(tournament.Id, tournament);
            MockGetTournamentResults(tournament.Id, allGames);
            MockGetTournamentResults(tournament.Id, games);
        }

        private void MockGetTournamentResults(GameResultDto singleGame)
        {
            MockGetTournamentResults(TOURNAMENT_ID, new List<GameResultDto> { singleGame });
        }

        #endregion

        #region Custom Assertion

        private bool AreGamesEqual(Game x, Game y)
        {
            return new GameComparer().Compare(x, y) == 0;
        }

        private static void AssertPlaceholdersAreUsed(ICollection<GameResultDto> actual, int numberOfGamesInFirstRound)
        {
            var firstRoundGames = actual.Where(g => g.Round == 1).ToList();
            Assert.AreEqual(numberOfGamesInFirstRound, firstRoundGames.Count,
                $"This playoff tournament should have {numberOfGamesInFirstRound} games in first round.");
            foreach (var game in firstRoundGames)
            {
                Assert.AreEqual(FIRST_TEAM_PLACEHOLDER, game.HomeTeamName,
                    $"[GameId:{game.Id}] Placeholder should be used for Home team name");
                Assert.AreEqual(SECOND_TEAM_PLACEHOLDER, game.AwayTeamName,
                    $"[GameId:{game.Id}] Placeholder should be used for Away team name");
            }
        }

        private static void AssertWinnerGameDependenciesAreSet(
            IEnumerable<GameResultDto> games,
            int gameId,
            int upstreamHomeTeamGameNumber,
            int upstreamAwayTeamGameNumber)
        {
            AssertGameDependenciesAreSet(
                games,
                gameId,
                upstreamHomeTeamGameNumber,
                upstreamAwayTeamGameNumber,
                "Winner",
                "Winner");
        }

        private static void AssertLooserGameDependenciesAreSet(
            IEnumerable<GameResultDto> games,
            int gameId,
            int upstreamHomeTeamGameNumber,
            int upstreamAwayTeamGameNumber)
        {
            AssertGameDependenciesAreSet(
                games,
                gameId,
                upstreamHomeTeamGameNumber,
                upstreamAwayTeamGameNumber,
                "Looser",
                "Looser");
        }

        private static void AssertGameDependenciesAreSet(
            IEnumerable<GameResultDto> games,
            int gameId,
            int upstreamHomeTeamGameNumber,
            int upstreamAwayTeamGameNumber,
            string prefix,
            string messagePrefix)
        {
            var game = games.FirstOrDefault(g => g.Id == gameId);
            Assert.IsNotNull(game, $"Game with id={gameId} should exist");
            Assert.AreEqual(
                $"{prefix}{upstreamHomeTeamGameNumber}",
                game.HomeTeamName,
                $"[GameId:{game.Id}] {messagePrefix} of upstream game should be set as Home team name");
            Assert.AreEqual(
                $"{prefix}{upstreamAwayTeamGameNumber}",
                game.AwayTeamName,
                $"[GameId:{game.Id}] {messagePrefix} of upstream game should be set as Away team name");
        }

        private void VerifyCreateGame(Game game, Times times)
        {
            _gameRepositoryMock.Verify(
                m => m.Add(It.Is<Game>(grs => AreGamesEqual(grs, game))), times, "Game was not created");
            _unitOfWorkMock.Verify(m => m.Commit(), times);
        }

        private void VerifyCreateGame(Game game, Times times, Times unitOfWorkTimes)
        {
            _gameRepositoryMock.Verify(
                m => m.Add(It.Is<Game>(grs => AreGamesEqual(grs, game))), times);
            _unitOfWorkMock.Verify(m => m.Commit(), unitOfWorkTimes);
        }

        private void VerifyEditGame(Game game, Times times)
        {
            _gameRepositoryMock.Verify(
                m => m.Update(It.Is<Game>(grs => AreGamesEqual(grs, game))), times);
            _unitOfWorkMock.Verify(m => m.Commit(), times);
        }

        private void VerifyEditGames(List<Game> games, Times times)
        {
            foreach (var game in games)
            {
                _gameRepositoryMock.Verify(
                m => m.Update(It.Is<Game>(grs => AreGamesEqual(grs, game))), times);
                _unitOfWorkMock.Verify(m => m.Commit(), times);
            }
        }

        private void VerifyDeleteGame(int gameResultId, Times times)
        {
            _gameRepositoryMock.Verify(m => m.Remove(It.Is<int>(id => id == gameResultId)), times);
            _unitOfWorkMock.Verify(m => m.Commit(), times);
        }

        private void VerifyExceptionThrown(Exception exception, string expectedMessage)
        {
            Assert.IsNotNull(exception);

            Assert.IsTrue(exception.Message.Equals(expectedMessage));
        }

        private void VerifyFreeDaySwapped(Game game)
        {
            Assert.IsNotNull(game.HomeTeamId, "HomeTeamId should not be null");
            Assert.IsNull(game.AwayTeamId, "AwayTeamId should be null");
        }

        private void VerifyCheckAccess(AuthOperation operation, Times times)
        {
            _authServiceMock.Verify(tr => tr.CheckAccess(operation), times);
        }

        private void VerifyGamesAdded(Times times)
        {
            _gameRepositoryMock.Verify(gr => gr.Add(It.IsAny<Game>()), times);
        }

        private void VerifyGamesRemoved(Times times)
        {
            _gameRepositoryMock.Verify(gr => gr.Remove(It.IsAny<int>()), times);
        }

        #endregion
    }
}
