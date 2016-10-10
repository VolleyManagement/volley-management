namespace VolleyManagement.UnitTests.Services.GameService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Authorization;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Crosscutting.Contracts.Providers;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Exceptions;
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Data.Queries.GameResult;
    using VolleyManagement.Data.Queries.Tournament;
    using VolleyManagement.Domain.GamesAggregate;
    using VolleyManagement.Domain.RolesAggregate;
    using VolleyManagement.Domain.TournamentsAggregate;
    using VolleyManagement.Services;
    using VolleyManagement.UnitTests.Mvc.ViewModels;
    using VolleyManagement.UnitTests.Services.TournamentService;

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

        private readonly Mock<IGameRepository> _gameRepositoryMock = new Mock<IGameRepository>();

        private readonly Mock<IGameService> _gameServiceMock = new Mock<IGameService>();

        private readonly Mock<IAuthorizationService> _authServiceMock = new Mock<IAuthorizationService>();

        private readonly Mock<ITournamentService> _tournamentServiceMock = new Mock<ITournamentService>();

        private readonly Mock<ITournamentRepository> _tournamentRepositoryMock = new Mock<ITournamentRepository>();

        private readonly Mock<IQuery<GameResultDto, FindByIdCriteria>> _getByIdQueryMock
            = new Mock<IQuery<GameResultDto, FindByIdCriteria>>();

        private readonly Mock<IQuery<List<GameResultDto>, TournamentGameResultsCriteria>> _tournamentGameResultsQueryMock
            = new Mock<IQuery<List<GameResultDto>, TournamentGameResultsCriteria>>();

        private readonly Mock<IQuery<List<Game>, TournamentRoundsGameResultsCriteria>> _gamesByTournamentIdRoundsNumberQueryMock
            = new Mock<IQuery<List<Game>, TournamentRoundsGameResultsCriteria>>();

        private readonly Mock<IQuery<TournamentScheduleDto, TournamentScheduleInfoCriteria>> _tournamentScheduleDtoByIdQueryMock
            = new Mock<IQuery<TournamentScheduleDto, TournamentScheduleInfoCriteria>>();

        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

        private readonly string _noTeamsInGame
            = "No teams are specified for current game in round {0}";

        private readonly string _wrongRoundDate
            = "Start of the round should not be earlier than the start of the tournament or later than the end of the tournament";

        private readonly Mock<TimeProvider> _timeMock = new Mock<TimeProvider>();

        private IKernel _kernel;

        #endregion

        #region Constuctor

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IGameRepository>().ToConstant(_gameRepositoryMock.Object);
            _kernel.Bind<ITournamentRepository>().ToConstant(_tournamentRepositoryMock.Object);
            _kernel.Bind<IQuery<GameResultDto, FindByIdCriteria>>()
                .ToConstant(_getByIdQueryMock.Object);
            _kernel.Bind<IQuery<List<GameResultDto>, TournamentGameResultsCriteria>>()
                .ToConstant(_tournamentGameResultsQueryMock.Object);
            _kernel.Bind<IQuery<TournamentScheduleDto, TournamentScheduleInfoCriteria>>()
                .ToConstant(_tournamentScheduleDtoByIdQueryMock.Object);
            _kernel.Bind<IQuery<List<Game>, TournamentRoundsGameResultsCriteria>>()
                .ToConstant(_gamesByTournamentIdRoundsNumberQueryMock.Object);
            _kernel.Bind<IGameService>().ToConstant(_gameServiceMock.Object);
            _kernel.Bind<ITournamentService>().ToConstant(_tournamentServiceMock.Object);
            _kernel.Bind<IAuthorizationService>().ToConstant(_authServiceMock.Object);
            _gameRepositoryMock.Setup(m => m.UnitOfWork).Returns(_unitOfWorkMock.Object);
            _tournamentRepositoryMock.Setup(tr => tr.UnitOfWork).Returns(_unitOfWorkMock.Object);
            _timeMock.SetupGet(tp => tp.UtcNow).Returns(new DateTime(2015, 06, 01));
            TimeProvider.Current = _timeMock.Object;
        }

        #endregion

        #region Create
        /// <summary>
        /// Test for Create method. GameResult object contains valid data. Game result is created successfully.
        /// </summary>
        [TestMethod]
        public void Create_GameValid_GameCreated()
        {
            // Arrange
            MockDefaultTournament();
            MockTournamentServiceReturnTournament();
            var newGame = new GameBuilder().Build();
            var sut = _kernel.Get<GameService>();

            // Act
            sut.Create(newGame);

            // Assert
            VerifyCreateGame(newGame, Times.Once());
        }

        /// <summary>
        /// Test for Create method. Tournament last date which was updated is today.
        /// Game result is created successfully.
        /// </summary>
        [TestMethod]
        public void Create_LastTimeUpdated_GameCreated()
        {
            // Arrange
            MockDefaultTournament();
            var tour = new TournamentBuilder().Build();
            _tournamentServiceMock.Setup(ts => ts.Get(It.IsAny<int>())).Returns(tour);
            var newGame = new GameBuilder().Build();
            var sut = _kernel.Get<GameService>();
           
            // Act
            sut.Create(newGame);

            // Assert
            Assert.AreEqual(TimeProvider.Current.UtcNow, tour.LastTimeUpdated);
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
            var sut = _kernel.Get<GameService>();

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

        /// <summary>
        /// Test for Create method. The home team and the away team are the same. Exception is thrown during creation.
        /// </summary>
        [TestMethod]
        public void Create_GameSameTeams_ExceptionThrown()
        {
            Exception exception = null;

            // Arrange
            var newGame = new GameBuilder().WithTheSameTeams().Build();
            var sut = _kernel.Get<GameService>();

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

        /// <summary>
        /// Test for Create method. The final score of the game (sets score) is invalid.
        /// Exception is thrown during creation.
        /// </summary>
        [TestMethod]
        public void Create_GameInvalidSetsScore_ExceptionThrown()
        {
            Exception exception = null;

            // Arrange
            MockDefaultTournament();
            var newGame = new GameBuilder().WithInvalidSetsScore().Build();
            var sut = _kernel.Get<GameService>();

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

        /// <summary>
        /// Test for Create method. The final score of the game (sets score) does not match set scores.
        /// Exception is thrown during creation.
        /// </summary>
        [TestMethod]
        public void Create_GameSetsScoreNoMatchSetScores_ExceptionThrown()
        {
            Exception exception = null;

            // Arrange
            MockDefaultTournament();
            var newGame = new GameBuilder().WithSetsScoreNoMatchSetScores().Build();
            var sut = _kernel.Get<GameService>();

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

        /// <summary>
        /// Test for Create method. The required set scores are invalid. Exception is thrown during creation.
        /// </summary>
        [TestMethod]
        public void Create_GameResultInvalidRequiredSetScores_ExceptionThrown()
        {
            Exception exception = null;

            // Arrange
            MockDefaultTournament();
            var newGame = new GameBuilder().WithInvalidRequiredSetScores().Build();
            var sut = _kernel.Get<GameService>();

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

        /// <summary>
        /// Test for Create method. The optional set scores are invalid. Exception is thrown during creation.
        /// </summary>
        [TestMethod]
        public void Create_GameInvalidOptionalSetScores_ExceptionThrown()
        {
            Exception exception = null;

            // Arrange
            MockDefaultTournament();
            var newGame = new GameBuilder().WithInvalidOptionalSetScores().Build();
            var sut = _kernel.Get<GameService>();

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

        /// <summary>
        /// Test for Create method. Previous optional set is not played (set score is 0:0). Exception is thrown during creation.
        /// </summary>
        [TestMethod]
        public void Create_GamePreviousOptionalSetUnplayed_ExceptionThrown()
        {
            Exception exception = null;

            // Arrange
            MockDefaultTournament();
            var newGame = new GameBuilder().WithPreviousOptionalSetUnplayed().Build();
            var sut = _kernel.Get<GameService>();

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

        /// <summary>
        /// Test for Create method. Set scores are not listed in the correct order. Exception is thrown during creation.
        /// </summary>
        [TestMethod]
        public void Create_GameSetScoresUnordered_ExceptionThrown()
        {
            Exception exception = null;

            // Arrange
            MockDefaultTournament();
            var newGame = new GameBuilder().WithSetScoresUnorderedForHomeTeam().Build();
            var sut = _kernel.Get<GameService>();

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

        /// <summary>
        /// Test for Create method. Set scores are not listed in the correct order. Exception is thrown during creation.
        /// </summary>
        [TestMethod]
        public void Create_GameSetScoresUnorderedForAwayTeam_ExceptionThrown()
        {
            Exception exception = null;

            // Arrange
            MockDefaultTournament();
            var newGame = new GameBuilder().WithSetScoresUnorderedForAwayTeam().Build();
            var sut = _kernel.Get<GameService>();

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

        /// <summary>
        /// Test for Create method. Game object contains valid data for technical win of away team. Game is created successfully.
        /// </summary>
        [TestMethod]
        public void Create_GameHomeTeamTechnicalWinValidData_GameCreated()
        {
            // Arrange
            MockDefaultTournament();
            MockTournamentServiceReturnTournament();
            var newGame = new GameBuilder().WithTechnicalDefeatValidSetScoresHomeTeamWin().Build();
            var sut = _kernel.Get<GameService>();

            // Act
            sut.Create(newGame);

            // Assert
            VerifyCreateGame(newGame, Times.Once());
        }

        /// <summary>
        /// Test for Create method. Game object contains valid data for technical win of away team. Game is created successfully.
        /// </summary>
        [TestMethod]
        public void Create_GameAwayTeamTechnicalWinValidData_GameCreated()
        {
            // Arrange
            MockDefaultTournament();
            MockTournamentServiceReturnTournament();
            var newGame = new GameBuilder()
                .WithTechnicalDefeatValidSetScoresAwayTeamWin()
                .WithTournamentId(1)
                .Build();
            var sut = _kernel.Get<GameService>();

            // Act
            sut.Create(newGame);

            // Assert
            VerifyCreateGame(newGame, Times.Once());
        }

        /// <summary>
        /// Test for Create method. Sets score are invalid. Exception is thrown during creation.
        /// </summary>
        [TestMethod]
        public void Create_TechnicalDefeatInvalidSetsScore_ExceptionThrown()
        {
            Exception exception = null;

            // Arrange
            MockDefaultTournament();
            var newGame = new GameBuilder().WithTechnicalDefeatInvalidSetsScore().Build();
            var sut = _kernel.Get<GameService>();

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

        /// <summary>
        /// Test for Create method. Set scores are invalid. Exception is thrown during creation.
        /// </summary>
        [TestMethod]
        public void Create_GameTechnicalDefeatInvalidSetScores_ExceptionThrown()
        {
            Exception exception = null;

            // Arrange
            MockDefaultTournament();
            var newGame = new GameBuilder().WithTechnicalDefeatInvalidSetScores().Build();
            var sut = _kernel.Get<GameService>();

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

        /// <summary>
        /// Test for Create method. Set scores are set to optional. Exception is thrown during creation.
        /// </summary>
        [TestMethod]
        public void Create_GameTechnicalDefeatOptionalSetScore_ExceptionThrown()
        {
            Exception exception = null;

            // Arrange
            MockDefaultTournament();
            var newGame = new GameBuilder().WithTechnicalDefeatValidOptional().Build();
            var sut = _kernel.Get<GameService>();

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

        /// <summary>
        /// Test for Create method. Set scores are null. Exception is thrown during creation.
        /// </summary>
        [TestMethod]
        public void Create_GameSetScoresNull_ExceptionThrown()
        {
            Exception exception = null;

            // Arrange
            MockDefaultTournament();
            var newGame = new GameBuilder().WithSetScoresNull().Build();
            var sut = _kernel.Get<GameService>();

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

        /// <summary>
        /// Test for Create method. Set scores are invalid. Exception is thrown during creation.
        /// </summary>
        [TestMethod]
        public void Create_GameSetsScoreInvalid_ExceptionThrown()
        {
            Exception exception = null;

            // Arrange
            MockDefaultTournament();
            var newGame = new GameBuilder().WithOrdinarySetsScoreInvalid().Build();
            var sut = _kernel.Get<GameService>();

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

        /// <summary>
        /// Test for Create method.
        /// Game object contains null result data.
        /// Game object is created successfully.
        /// Result of this game have to be initialized
        /// </summary>
        [TestMethod]
        public void Create_GameWithNoResult_GameCreatedWithDefaultResult()
        {
            // Arrange
            MockDefaultTournament();
            MockTournamentServiceReturnTournament();

            var newGame = new GameBuilder()
                .WithNullResult()
                .Build();
            var sut = _kernel.Get<GameService>();
            var expectedGameToCreate = new GameBuilder()
                .WithDefaultResult()
                .Build();

            // Act
            sut.Create(newGame);

            // Assert
            VerifyCreateGame(expectedGameToCreate, Times.Once());
        }

        /// <summary>
        /// Tests creation of the game with invalid date
        /// </summary>
        [TestMethod]
        public void Create_GameBeforeTournamentStarts_ExceptionThrown()
        {
            // Arrange
            Exception exception = null;

            MockDefaultTournament();
            MockTournamentServiceReturnTournament();

            Game game = new GameBuilder()
                .WithStartDate(DateTime.Parse(BEFORE_TOURNAMENT_DATE))
                .Build();

            var sut = _kernel.Get<GameService>();

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

        /// <summary>
        /// Tests creation of the game with invalid date
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_GameSetLateDateTime_ExceptionThrown()
        {
            // Arrange
            TournamentScheduleDto tournament = new TournamentScheduleDtoBuilder()
                .WithStartDate(DateTime.Parse(TOURNAMENT_DATE_START))
                .WithEndDate(DateTime.Parse(TOURNAMENT_DATE_END))
                .Build();

            Game game = new GameBuilder()
                .WithTournamentId(tournament.Id)
                .WithStartDate(DateTime.Parse(LATE_TOURNAMENT_DATE))
                .Build();

            var sut = _kernel.Get<GameService>();

            // Act
            sut.Create(game);

            // Assert
            VerifyCreateGame(game, Times.Never(), Times.Once());
        }

        /// <summary>
        /// Tests creation of same game in same round
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_SameGameInRound_ExceptionThrown()
        {
            // Arrange
            bool excaptionWasThrown = false;

            var game = new GameBuilder()
                .WithId(1)
                .TestRoundGame()
                .Build();

            var duplicate = new GameBuilder()
                .WithId(2)
                .TestRoundGame()
                .Build();

            var sut = _kernel.Get<GameService>();

            sut.Create(game);

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

        /// <summary>
        /// Tests creation of the duplicate free day game
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_SecondFreeDayInRound_ExceptionThrown()
        {
            // Arrange
            Exception exception = null;

            var freeDayGame = new GameBuilder()
                .TestFreeDayGame()
                .Build();

            var duplicateFreeDayGame = new GameBuilder()
                .TestFreeDayGame()
                .WithId(2)
                .Build();

            var sut = _kernel.Get<GameService>();
            sut.Create(freeDayGame);

            // Act
            try
            {
                sut.Create(duplicateFreeDayGame);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(
                exception,
                string.Format(_noTeamsInGame, duplicateFreeDayGame.Round));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_SameTeamInTwoGamesInOneRound_ExceptionThrown()
        {
            // Arrange
            bool exceptionWasThrown = false;

            var gameInOneRound = new GameBuilder()
                .TestRoundGame()
                .Build();

            var gameInSameRound = new GameBuilder()
                .WithId(2)
                .TestRoundGame()
                .WithHomeTeamId(3)
                .Build();

            var sut = _kernel.Get<GameService>();
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
            bool exceptionThrown = false;

            MockDefaultTournament();

            var gameInOtherRound = new GameBuilder()
                .TestRoundGame()
                .WithId(2)
                .WithRound(2)
                .Build();

            var sut = _kernel.Get<GameService>();

            SetupGetTournamentResults(
                gameInOtherRound.TournamentId,
                new GameServiceTestFixture().TestGamesForDuplicateSchemeOne().Build());

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
            bool exceptionThrown = false;

            MockDefaultTournament();

            var gameInOtherRound = new GameBuilder()
                .TestRoundGame()
                .WithId(2)
                .WithRound(2)
                .Build();

            var sut = _kernel.Get<GameService>();

            SetupGetTournamentResults(
               gameInOtherRound.TournamentId,
               new GameServiceTestFixture().TestGamesForDuplicateSchemeOne().Build());

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
            bool exceptionThrown = false;

            MockDefaultTournament();

            var duplicate = new GameBuilder()
               .TestRoundGame()
               .WithRound(3)
               .WithId(3)
               .Build();

            SetupGetTournamentResults(
              duplicate.TournamentId,
              new GameServiceTestFixture().TestGamesForDuplicateSchemeTwo().Build());
            var sut = _kernel.Get<GameService>();

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
            MockTournamentServiceReturnTournament();

            var duplicate = new GameBuilder()
              .TestRoundGame()
              .WithRound(3)
              .WithId(3)
              .Build();

            SetupGetTournamentResults(
             duplicate.TournamentId,
             new GameServiceTestFixture().TestGamesForDuplicateSchemeTwo().Build());
            var sut = _kernel.Get<GameService>();

            // Act
            sut.Create(duplicate);

            // Assert
            VerifyCreateGame(duplicate, Times.Once());
        }

        [TestMethod]
        public void Create_ThirdDuplicateGameInTournamentSchemeTwo_ExceptionThrown()
        {
            // Arrange
            bool exceptionThrown = false;

            MockTournamentSchemeTwo();

            var duplicate = new GameBuilder()
                .WithTournamentId(1)
                .TestRoundGame()
                .WithRound(4)
                .WithId(4)
                .Build();

            List<GameResultDto> gameResults = new GameServiceTestFixture()
                       .TestGamesSameTeamsSwitchedOrderTournamentSchemTwo()
                       .Build();
            SetupGetTournamentResults(
                       duplicate.TournamentId,
                       gameResults);

            var sut = _kernel.Get<GameService>();

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
            bool exceptionThrown = false;

            MockTournamentSchemeTwo();

            var freeDayGameDuplicate = new GameBuilder()
                .TestFreeDayGame()
                .WithId(3)
                .WithRound(3)
                .Build();

            List<GameResultDto> gameResults = new GameServiceTestFixture()
                .TestGamesWithTwoFreeDays()
                .Build();

            SetupGetTournamentResults(
                freeDayGameDuplicate.TournamentId,
                gameResults);

            var sut = _kernel.Get<GameService>();

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
        public void Create_FreeDayGameWithOtherTeamInSameRound_ExceptionThrown()
        {
            // Arrange
            Exception exception = null;

            MockDefaultTournament();

            var freeDayGmeInSameRound = new GameBuilder()
                .TestFreeDayGame()
                .WithId(2)
                .WithHomeTeamId(3)
                .Build();

            List<GameResultDto> gameResults = new GameServiceTestFixture()
                .TestGamesWithFreeDay()
                .Build();

            SetupGetTournamentResults(
                freeDayGmeInSameRound.TournamentId,
                gameResults);

            var sut = _kernel.Get<GameService>();

            // Act
            try
            {
                sut.Create(freeDayGmeInSameRound);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, "Free day game has already been scheduled in this round");
        }

        [TestMethod]
        public void Create_FreeDayGammeWithSameTeamInSameRound_ExceptionThrown()
        {
            // Arrange
            Exception exception = null;

            MockDefaultTournament();

            var freeDayGmeInSameRound = new GameBuilder()
                .TestFreeDayGame()
                .WithId(2)
                .Build();

            List<GameResultDto> gameResults = new GameServiceTestFixture()
                .TestGamesWithFreeDay()
                .Build();

            SetupGetTournamentResults(
                freeDayGmeInSameRound.TournamentId,
                gameResults);

            var sut = _kernel.Get<GameService>();

            // Act
            try
            {
                sut.Create(freeDayGmeInSameRound);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, "Free day game has already been scheduled in this round");
        }

        [TestMethod]
        public void Create_DuplicateGamesInSameRound_ExceptionThrown()
        {
            // Arrange
            bool exceptionThrown = false;

            MockDefaultTournament();

            var freeDayGmeInSameRound = new GameBuilder()
                .TestRoundGame()
                .WithId(2)
                .Build();

            List<GameResultDto> gameResults = new GameServiceTestFixture()
             .TestGamesForDuplicateSchemeOne()
             .Build();

            SetupGetTournamentResults(
             freeDayGmeInSameRound.TournamentId,
             gameResults);

            var sut = _kernel.Get<GameService>();

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
            bool exceptionThrown = false;

            MockDefaultTournament();

            var freeDayGmeInSameRound = new GameBuilder()
                .TestRoundGame()
                .WithHomeTeamId(3)
                .WithId(2)
                .Build();

            List<GameResultDto> gameResults = new GameServiceTestFixture()
                .TestGamesForDuplicateSchemeOne()
                .Build();

            SetupGetTournamentResults(
                freeDayGmeInSameRound.TournamentId,
                gameResults);

            var sut = _kernel.Get<GameService>();

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
        public void Create_DuplicateHomeTeamInGameInSameRound_ExceptionThrown()
        {
            // Arrange
            bool exceptionThrown = false;

            MockDefaultTournament();

            var freeDayGmeInSameRound = new GameBuilder()
                .TestRoundGame()
                .WithAwayTeamId(3)
                .WithId(2)
                .Build();

            List<GameResultDto> gameResults = new GameServiceTestFixture()
                .TestGamesForDuplicateSchemeOne()
                .Build();

            SetupGetTournamentResults(
                freeDayGmeInSameRound.TournamentId,
                gameResults);

            var sut = _kernel.Get<GameService>();

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

        /// <summary>
        /// Tests creation of the game with invalid fifth set score
        /// </summary>
        [TestMethod]
        public void Create_FifthSetScoreAsUsualSetScore_ExceptionThrown()
        {
            // Arrange
            Exception exception = null;
            MockDefaultTournament();
            var newGame = new GameBuilder().WithFifthSetScoreAsUsualSetScore().Build();
            var sut = _kernel.Get<GameService>();

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

        /// <summary>
        /// Tests creation of the game with invalid fifth set score
        /// </summary>
        [TestMethod]
        public void Create_FifthSetScoreMoreThanMaxWithValidDifference_GameCreated()
        {
            // Arrange
            MockDefaultTournament();
            MockTournamentServiceReturnTournament();
            var newGame = new GameBuilder().WithFifthSetScoreMoreThanMaxWithValidDifference().Build();
            var sut = _kernel.Get<GameService>();

            // Act
            sut.Create(newGame);

            // Assert
            VerifyCreateGame(newGame, Times.Once());
        }

        /// <summary>
        /// Tests creation of the game with valid fifth set score
        /// </summary>
        [TestMethod]
        public void Create_FifthSetScoreMoreThanMaxWithInvalidDifference_ExceptionThrown()
        {
            // Arrange
            Exception exception = null;
            MockDefaultTournament();
            var newGame = new GameBuilder().WithFifthSetScoreMoreThanMaxWithInvalidDifference().Build();
            var sut = _kernel.Get<GameService>();

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

        /// <summary>
        /// Tests creation of the game with invalid fifth set score
        /// </summary>
        [TestMethod]
        public void Create_FifthSetScoreLessThanMax_ExceptionThrown()
        {
            // Arrange
            Exception exception = null;
            MockDefaultTournament();
            var newGame = new GameBuilder().WithFifthSetScoreLessThanMax().Build();
            var sut = _kernel.Get<GameService>();

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

        /// <summary>
        /// Tests creation of the game with valid fifth set score
        /// </summary>
        [TestMethod]
        public void Create_FifthSetValidScore_ExceptionThrown()
        {
            // Arrange
            MockDefaultTournament();
            MockTournamentServiceReturnTournament();
            var newGame = new GameBuilder().WithFifthSetScoreValid().Build();
            var sut = _kernel.Get<GameService>();

            // Act
            sut.Create(newGame);

            // Assert
            VerifyCreateGame(newGame, Times.Once());
        }
        #endregion

        #region Get

        /// <summary>
        /// Test for Get method. Existing game is requested. Game is returned.
        /// </summary>
        [TestMethod]
        public void Get_ExistingGame_GameReturned()
        {
            // Arrange
            var expected = new GameResultDtoBuilder().WithId(GAME_RESULT_ID).Build();
            var sut = _kernel.Get<GameService>();

            SetupGet(expected);

            // Act
            var actual = sut.Get(GAME_RESULT_ID);

            // Assert
            TestHelper.AreEqual(expected, actual, new GameResultDtoComparer());
        }

        /// <summary>
        /// Test for GetTournamentResults method. Game results of specified tournament are requested. Game results are returned.
        /// </summary>
        [TestMethod]
        public void GetTournamentResults_GameResultsRequsted_GameResultsReturned()
        {
            // Arrange
            var expected = new GameServiceTestFixture().TestGameResults().Build();
            var sut = _kernel.Get<GameService>();

            SetupGetTournamentResults(TOURNAMENT_ID, expected);

            // Act
            var actual = sut.GetTournamentResults(TOURNAMENT_ID);

            // Assert
            CollectionAssert.AreEqual(expected, actual, new GameResultDtoComparer());
        }

        #endregion

        #region Edit
        /// <summary>
        /// Test for Edit method. Game object contains valid data. Game is edited successfully.
        /// </summary>
        [TestMethod]
        public void Edit_GameValid_GameEdited()
        {
            // Arrange
            MockDefaultTournament();
            MockTournamentServiceReturnTournament();
            var existingGames = new List<GameResultDto> { new GameResultDtoBuilder().WithId(GAME_RESULT_ID).Build() };
            var game = new GameBuilder().WithId(GAME_RESULT_ID).Build();
            _tournamentGameResultsQueryMock.Setup(m => m.Execute(It.IsAny<TournamentGameResultsCriteria>())).Returns(existingGames);
            var sut = _kernel.Get<GameService>();

            // Act
            sut.Edit(game);

            // Assert
            VerifyEditGame(game, Times.Once());
        }

        /// <summary>
        /// Test for Edit method. Tournament last date which was updated is today.
        /// Game is edited successfully.
        /// </summary>
        [TestMethod]
        public void Edit_LastTimeUpdated_GameEdited()
        {
            // Arrange
            MockDefaultTournament();
            var tour = new TournamentBuilder().Build();
            _tournamentServiceMock.Setup(ts => ts.Get(It.IsAny<int>())).Returns(tour);
            var existingGames = new List<GameResultDto> { new GameResultDtoBuilder().WithId(GAME_RESULT_ID).Build() };
            var game = new GameBuilder().WithId(GAME_RESULT_ID).Build();
            _tournamentGameResultsQueryMock.Setup(m => m.Execute(It.IsAny<TournamentGameResultsCriteria>())).Returns(existingGames);
            var sut = _kernel.Get<GameService>();

            // Act
            sut.Edit(game);

            // Assert
            Assert.AreEqual(TimeProvider.Current.UtcNow, tour.LastTimeUpdated);
        }

        /// <summary>
        /// Test for Edit method. Tournament last date which was updated is today.
        /// Game result is edited successfully.
        /// </summary>
        [TestMethod]
        public void Edit_LastTimeUpdated_GameResultEdited()
        {
            // Arrange
            MockDefaultTournament();
            var tour = new TournamentBuilder().Build();
            _tournamentServiceMock.Setup(ts => ts.Get(It.IsAny<int>())).Returns(tour);
            var existingGames = new List<GameResultDto> { new GameResultDtoBuilder().WithId(GAME_RESULT_ID).Build() };
            var game = new GameBuilder().WithId(GAME_RESULT_ID).Build();
            _tournamentGameResultsQueryMock.Setup(m => m.Execute(It.IsAny<TournamentGameResultsCriteria>())).Returns(existingGames);
            var sut = _kernel.Get<GameService>();

            // Act
            sut.EditGameResult(game);

            // Assert
            Assert.AreEqual(TimeProvider.Current.UtcNow, tour.LastTimeUpdated);
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
            var sut = _kernel.Get<GameService>();

            SetupEditMissingEntityException(game);

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

        #endregion

        #region Delete

        /// <summary>
        /// Test for Delete method. Existing game has to be deleted. Game is deleted.
        /// </summary>
        [TestMethod]
        public void Delete_ExistingGame_GameDeleted()
        {
            // Arrange
            var sut = _kernel.Get<GameService>();
            var now = TimeProvider.Current.UtcNow;

            var game = new GameResultDtoBuilder().WithDate(now.AddDays(ONE_DAY))
                    .WithAwaySetsScore(0).WithHomeSetsScore(0).Build();

            SetupGet(game);

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
            var sut = _kernel.Get<GameService>();
            var now = TimeProvider.Current.UtcNow;
            var game = new GameResultDtoBuilder().WithId(GAME_RESULT_ID)
                                                 .WithDate(now.AddDays(-ONE_DAY)).Build();

            SetupGet(game);

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
            var sut = _kernel.Get<GameService>();
            var game = new GameResultDtoBuilder().WithId(GAME_RESULT_ID)
                                .WithAwaySetsScore(TEST_AWAY_SET_SCORS)
                                .WithHomeSetsScore(TEST_HOME_SET_SCORS).Build();

            SetupGet(game);

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
            int gameNullId = 0;
            var sut = _kernel.Get<GameService>();

            // Act
            try
            {
                sut.Delete(gameNullId);
            }
            catch (ArgumentNullException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, ExpectedExceptionMessages.GAME);
        }

        #endregion

        #region SwapRonds

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
            var sut = _kernel.Get<GameService>();

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
            var sut = _kernel.Get<GameService>();

            // Act
            try
            {
                foreach (var game in games)
                {
                    SetupEditMissingEntityException(game);
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

            var sut = _kernel.Get<GameService>();

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

            var sut = _kernel.Get<GameService>();

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

            var sut = _kernel.Get<GameService>();

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

            var sut = _kernel.Get<GameService>();

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
            var sut = _kernel.Get<GameService>();

            // Act
            sut.SwapRounds(TOURNAMENT_ID, FIRST_ROUND_NUMBER, SECOND_ROUND_NUMBER);

            // Assert
            VerifyCheckAccess(AuthOperations.Games.SwapRounds, Times.Once());
        }

        #endregion

        #region Private
        private bool AreGamesEqual(Game x, Game y)
        {
            return new GameComparer().Compare(x, y) == 0;
        }

        private bool AreDatesEqual(DateTime? x, DateTime? y)
        {
            return new GameComparer().Compare(x, y) == 0;
        }

        private void SetupGet(GameResultDto gameResult)
        {
            _getByIdQueryMock
                .Setup(m => m.Execute(It.Is<FindByIdCriteria>(c => c.Id == gameResult.Id)))
                .Returns(gameResult);
        }

        private void SetupGetTournamentResults(int tournamentId, List<GameResultDto> gameResults)
        {
            _tournamentGameResultsQueryMock.Setup(m =>
                m.Execute(It.Is<TournamentGameResultsCriteria>(c => c.TournamentId == tournamentId)))
                .Returns(gameResults);
        }

        private void SetupGetTournamentById(int id, TournamentScheduleDto tournament)
        {
            _tournamentScheduleDtoByIdQueryMock.Setup(m =>
                m.Execute(It.Is<TournamentScheduleInfoCriteria>(c => c.TournamentId == id)))
                .Returns(tournament);
        }

        private void SetupEditMissingEntityException(Game game)
        {
            _gameRepositoryMock.Setup(m =>
                m.Update(It.Is<Game>(grs => AreGamesEqual(grs, game))))
                .Throws(new ConcurrencyException());
        }

        private void VerifyCreateGame(Game game, Times times)
        {
            _gameRepositoryMock.Verify(
                m => m.Add(It.Is<Game>(grs => AreGamesEqual(grs, game))), times);
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

        private void VerifyEditGame(Game game, Times repositoryTimes, Times unitOfWorkTimes)
        {
            _gameRepositoryMock.Verify(
                m => m.Update(It.Is<Game>(grs => AreGamesEqual(grs, game))), repositoryTimes);
            _unitOfWorkMock.Verify(m => m.Commit(), unitOfWorkTimes);
        }

        private void VerifyDeleteGame(int gameResultId, Times times)
        {
            _gameRepositoryMock.Verify(m => m.Remove(It.Is<int>(id => id == gameResultId)), times);
            _unitOfWorkMock.Verify(m => m.Commit(), times);
        }

        /// <summary>
        /// Checks if exception was thrown and has appropriate message
        /// </summary>
        /// <param name="exception">Exception that has been thrown</param>
        /// <param name="expectedMessage">Message to compare with</param>
        private void VerifyExceptionThrown(Exception exception, string expectedMessage)
        {
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception.Message.Equals(expectedMessage));
        }

        private void MockDefaultTournament()
        {
            var tournament = new TournamentScheduleDtoBuilder()
                .TestTournamemtSchedultDto()
                .WithScheme(TournamentSchemeEnum.One)
                .Build();

            SetupGetTournamentById(tournament.Id, tournament);
            SetupGetTournamentResults(tournament.Id, new List<GameResultDto>());
        }

        private void MockTournamentSchemeTwo()
        {
            var tournament = new TournamentScheduleDtoBuilder()
               .TestTournamemtSchedultDto()
               .WithScheme(TournamentSchemeEnum.Two)
               .Build();

            SetupGetTournamentById(tournament.Id, tournament);
            SetupGetTournamentResults(tournament.Id, new List<GameResultDto>());
        }

        /// <summary>
        /// Checks if exception was thrown and has appropriate message
        /// </summary>
        /// <param name="exception">Exception that has been thrown</param>
        /// <param name="expected">Expected exception</param>
        private void VerifyExceptionThrown(Exception exception, Exception expected)
        {
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception.Message.Equals(expected.Message));
        }

        private void VerifyCheckAccess(AuthOperation operation, Times times)
        {
            _authServiceMock.Verify(tr => tr.CheckAccess(operation), times);
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

        #endregion
    }
}
