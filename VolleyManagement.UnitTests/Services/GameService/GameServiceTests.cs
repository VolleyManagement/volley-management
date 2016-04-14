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
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Exceptions;
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Data.Queries.GameResult;
    using VolleyManagement.Domain.GamesAggregate;
    using VolleyManagement.Domain.TournamentsAggregate; 
    using VolleyManagement.Services;
    using VolleyManagement.UnitTests.Services.TournamentService; 

    /// <summary>
    /// Tests for <see cref="GameService"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class GameServiceTests
    {
        private const int GAME_RESULT_ID = 1;

        private const int TOURNAMENT_ID = 1;

        private const string TOURNAMENT_DATE_START = "2016-04-02 10:00";

        private const string TOURNAMENT_DATE_END = "2016-04-04 10:00";

        private const string BEFORE_TOURNAMENT_DATE = "2016-04-02 07:00";

        private const string LATE_TOURNAMENT_DATE = "2016-04-06 10:00"; 

        private readonly Mock<IGameRepository> _gameRepositoryMock = new Mock<IGameRepository>();

        private readonly Mock<IGameService> _gameServiceMock = new Mock<IGameService>();

        private readonly Mock<IQuery<GameResultDto, FindByIdCriteria>> _getByIdQueryMock
            = new Mock<IQuery<GameResultDto, FindByIdCriteria>>();

        private readonly Mock<IQuery<List<GameResultDto>, TournamentGameResultsCriteria>> _tournamentGameResultsQueryMock
            = new Mock<IQuery<List<GameResultDto>, TournamentGameResultsCriteria>>();

        private readonly Mock<IQuery<Tournament, FindByIdCriteria>> _tournamentByIdQueryMock
            = new Mock<IQuery<Tournament, FindByIdCriteria>>();

        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

        private readonly string NoTeamsInGame
            = "No teams are specified for current game in round {0}";

        private readonly string WrongRoundDate
            = "Start of the round should not be earlier than the start of the tournament or later than the end of the tournament";
        
        private IKernel _kernel;

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IGameRepository>().ToConstant(_gameRepositoryMock.Object);
            _kernel.Bind<IQuery<GameResultDto, FindByIdCriteria>>().ToConstant(_getByIdQueryMock.Object);
            _kernel.Bind<IQuery<List<GameResultDto>, TournamentGameResultsCriteria>>()
                .ToConstant(_tournamentGameResultsQueryMock.Object);
            _kernel.Bind<IQuery<Tournament, FindByIdCriteria>>().ToConstant(_tournamentByIdQueryMock.Object);
            _kernel.Bind<IGameService>().ToConstant(_gameServiceMock.Object);
            _gameRepositoryMock.Setup(m => m.UnitOfWork).Returns(_unitOfWorkMock.Object);
        }

        /// <summary>
        /// Test for Create method. GameResult object contains valid data. Game result is created successfully.
        /// </summary>
        [TestMethod]
        public void Create_GameValid_GameCreated()
        {
            // Arrange
            AddTestTournament(); 
            var newGame = new GameBuilder().Build();
            var sut = _kernel.Get<GameService>();

            // Act
            sut.Create(newGame);

            // Assert
            VerifyCreateGame(newGame, Times.Once());
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
            AddTestTournament();
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
            AddTestTournament();
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
            AddTestTournament();
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
            AddTestTournament();
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
            AddTestTournament();
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
            AddTestTournament();
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
            AddTestTournament();
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
            AddTestTournament();
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
            AddTestTournament(); 

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
            AddTestTournament();
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
            AddTestTournament();
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
            AddTestTournament();
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
            AddTestTournament();
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
            AddTestTournament();
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
            AddTestTournament(); 

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
            
            Tournament tournament = new TournamentBuilder()
                .WithApplyingPeriodStart(DateTime.Parse(TOURNAMENT_DATE_START))
                .WithApplyingPeriodEnd(DateTime.Parse(TOURNAMENT_DATE_END))
                .Build();

            SetupGetTournamentById(tournament.Id, tournament); 

            Game game = new GameBuilder()
                .WithTournamentId(tournament.Id)
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
            VerifyExceptionThrown(exception, WrongRoundDate); 
        }

        /// <summary>
        /// Tests creation of the game with invalid date 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_GameSetLateDateTime_ExceptionThrown()
        {
            // Arrange
            Tournament tournament = new TournamentBuilder()
                .WithApplyingPeriodStart(DateTime.Parse(TOURNAMENT_DATE_START))
                .WithApplyingPeriodEnd(DateTime.Parse(TOURNAMENT_DATE_END))
                .Build();

            Game game = new GameBuilder()
                .WithTournamentId(tournament.Id)
                .WithStartDate(DateTime.Parse(LATE_TOURNAMENT_DATE))
                .Build();

            var sut = _kernel.Get<GameService>();

            // Act 
            sut.Create(game);

            // Assert 
            VerifyCreateGame(game, Times.Never());
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
                string.Format(NoTeamsInGame, duplicateFreeDayGame.Round));
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
        [ExpectedException(typeof(ArgumentException))]
        public void Create_SameGameTournamentSchemeOne_ExceptionThrown()
        {
            // Arrange 
            bool exceptionThrown = false;

            var tournament = new TournamentBuilder()
                .WithScheme(TournamentSchemeEnum.One)
                .Build();

            SetupGetTournamentById(tournament.Id, tournament); 

            var gameInRound = new GameBuilder()
                .TestRoundGame()
                .WithRound(1)
                .Build();

            var gameInOtherRound = new GameBuilder()
                .TestRoundGame()
                .WithId(2)
                .WithRound(2)
                .Build();

            var sut = _kernel.Get<GameService>();
            sut.Create(gameInRound);

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
        [ExpectedException(typeof(ArgumentException))]
        public void Create_SameGameSwitchedTeamsTournamentSchemeOne_ExceptionThrown()
        {
            // Arrange 
            bool exceptionThrown = false;

            var tournament = new TournamentBuilder()
                .WithScheme(TournamentSchemeEnum.One)
                .Build();

            SetupGetTournamentById(tournament.Id, tournament);

            var gameInRound = new GameBuilder()
                .TestRoundGame()
                .WithRound(1)
                .WithHomeTeamId(2)
                .WithAwayTeamId(1)
                .Build();

            var gameInOtherRound = new GameBuilder()
                .TestRoundGame()
                .WithId(2)
                .WithRound(2)
                .Build();

            var sut = _kernel.Get<GameService>();
            sut.Create(gameInRound);

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
        [ExpectedException(typeof(ArgumentException))]
        public void Create_SameGameInOtherRoundTournamentSchemeTwo_ExceptionThrown()
        {
            // Arrange 
            bool exceptionThrown = false;

            var tournament = new TournamentBuilder()
                .WithScheme(TournamentSchemeEnum.One)
                .Build();

            SetupGetTournamentById(tournament.Id, tournament); 
           
            var gameInRound = new GameBuilder()
                .TestRoundGame()
                .WithRound(1)
                .Build();

            var sameGameInOtherRound = new GameBuilder()
                .TestRoundGameSwithedTeams()
                .WithId(2)
                .WithRound(2)
                .Build();
           
            var duplicate = new GameBuilder()
                .TestRoundGame()
                .WithRound(3)
                .WithId(3)
                .Build();

            var sut = _kernel.Get<GameService>();
            sut.Create(gameInRound);
            sut.Create(sameGameInOtherRound);

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

        /// <summary>
        /// Test for Edit method. Game object contains valid data. Game is edited successfully.
        /// </summary>
        [TestMethod]
        public void Edit_GameValid_GameEdited()
        {
            // Arrange
            AddTestTournament(); 
            var game = new GameBuilder().Build();
            var sut = _kernel.Get<GameService>();

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
            AddTestTournament(); 
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

        /// <summary>
        /// Test for Delete method. Existing game has to be deleted. Game is deleted.
        /// </summary>
        [TestMethod]
        public void Delete_ExistingGame_GameDeleted()
        {
            // Arrange
            var sut = _kernel.Get<GameService>();

            // Act
            sut.Delete(GAME_RESULT_ID);

            // Assert
            VerifyDeleteGame(GAME_RESULT_ID, Times.Once());
        }

        private bool AreGamesEqual(Game x, Game y)
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

        private void SetupGetTournamentById(int id, Tournament tournament)
        {
            _tournamentByIdQueryMock.Setup(m =>
                m.Execute(It.Is<FindByIdCriteria>(c => c.Id == id)))
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

        private void VerifyEditGame(Game game, Times times)
        {
            _gameRepositoryMock.Verify(
                m => m.Update(It.Is<Game>(grs => AreGamesEqual(grs, game))), times);
            _unitOfWorkMock.Verify(m => m.Commit(), times);
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

        private void AddTestTournament()
        {
            var tournament = new TournamentBuilder()
                .TestTournament()
                .WithScheme(TournamentSchemeEnum.One)
                .Build();

            SetupGetTournamentById(tournament.Id, tournament);
            SetupGetTournamentResults(tournament.Id, new List<GameResultDto>()); 
        }
    }
}
