namespace VolleyManagement.UnitTests.Services.GameResultService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Exceptions;
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Data.Queries.GameResult;
    using VolleyManagement.Domain.GameResultsAggregate;
    using VolleyManagement.Services;

    /// <summary>
    /// Tests for <see cref="GameResultService"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class GameResultServiceTests
    {
        private const int GAME_RESULT_ID = 1;

        private const int TOURNAMENT_ID = 1;

        private readonly Mock<IGameResultRepository> _gameResultRepositoryMock = new Mock<IGameResultRepository>();

        private readonly Mock<IQuery<GameResultDto, FindByIdCriteria>> _getByIdQueryMock
            = new Mock<IQuery<GameResultDto, FindByIdCriteria>>();

        private readonly Mock<IQuery<List<GameResultDto>, TournamentGameResultsCriteria>> _tournamentGameResultsQueryMock
            = new Mock<IQuery<List<GameResultDto>, TournamentGameResultsCriteria>>();

        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

        private IKernel _kernel;

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IGameResultRepository>().ToConstant(_gameResultRepositoryMock.Object);
            _kernel.Bind<IQuery<GameResultDto, FindByIdCriteria>>().ToConstant(_getByIdQueryMock.Object);
            _kernel.Bind<IQuery<List<GameResultDto>, TournamentGameResultsCriteria>>()
                .ToConstant(_tournamentGameResultsQueryMock.Object);
            _gameResultRepositoryMock.Setup(m => m.UnitOfWork).Returns(_unitOfWorkMock.Object);
        }

        /// <summary>
        /// Test for Create method. GameResult object contains valid data. Game result is created successfully.
        /// </summary>
        [TestMethod]
        public void Create_GameResultValid_GameResultCreated()
        {
            // Arrange
            var newGameResult = new GameResultBuilder().Build();
            var sut = _kernel.Get<GameResultService>();

            // Act
            sut.Create(newGameResult);

            // Assert
            VerifyCreateGameResult(newGameResult, Times.Once());
        }

        /// <summary>
        /// Test for Create method. The game result instance is null. Exception is thrown during creation.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_GameResultNull_ExceptionThrown()
        {
            // Arrange
            var newGameResult = null as GameResult;
            var sut = _kernel.Get<GameResultService>();

            // Act
            sut.Create(newGameResult);

            // Assert
            VerifyCreateGameResult(newGameResult, Times.Never());
        }

        /// <summary>
        /// Test for Create method. The home team and the away team are the same. Exception is thrown during creation.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_GameResultSameTeams_ExceptionThrown()
        {
            // Arrange
            var newGameResult = new GameResultBuilder().WithTheSameTeams().Build();
            var sut = _kernel.Get<GameResultService>();

            // Act
            sut.Create(newGameResult);

            // Assert
            VerifyCreateGameResult(newGameResult, Times.Never());
        }

        /// <summary>
        /// Test for Create method. The final score of the game (sets score) is invalid.
        /// Exception is thrown during creation.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_GameResultInvalidSetsScore_ExceptionThrown()
        {
            // Arrange
            var newGameResult = new GameResultBuilder().WithInvalidSetsScore().Build();
            var sut = _kernel.Get<GameResultService>();

            // Act
            sut.Create(newGameResult);

            // Assert
            VerifyCreateGameResult(newGameResult, Times.Never());
        }

        /// <summary>
        /// Test for Create method. The final score of the game (sets score) does not match set scores.
        /// Exception is thrown during creation.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_GameResultSetsScoreNoMatchSetScores_ExceptionThrown()
        {
            // Arrange
            var newGameResult = new GameResultBuilder().WithSetsScoreNoMatchSetScores().Build();
            var sut = _kernel.Get<GameResultService>();

            // Act
            sut.Create(newGameResult);

            // Assert
            VerifyCreateGameResult(newGameResult, Times.Never());
        }

        /// <summary>
        /// Test for Create method. The required set scores are invalid. Exception is thrown during creation.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_GameResultInvalidRequiredSetScores_ExceptionThrown()
        {
            // Arrange
            var newGameResult = new GameResultBuilder().WithInvalidRequiredSetScores().Build();
            var sut = _kernel.Get<GameResultService>();

            // Act
            sut.Create(newGameResult);

            // Assert
            VerifyCreateGameResult(newGameResult, Times.Never());
        }

        /// <summary>
        /// Test for Create method. The optional set scores are invalid. Exception is thrown during creation.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_GameResultInvalidOptionalSetScores_ExceptionThrown()
        {
            // Arrange
            var newGameResult = new GameResultBuilder().WithInvalidOptionalSetScores().Build();
            var sut = _kernel.Get<GameResultService>();

            // Act
            sut.Create(newGameResult);

            // Assert
            VerifyCreateGameResult(newGameResult, Times.Never());
        }

        /// <summary>
        /// Test for Create method. Previous optional set is not played (set score is 0:0). Exception is thrown during creation.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_GameResultPreviousOptionalSetUnplayed_ExceptionThrown()
        {
            // Arrange
            var newGameResult = new GameResultBuilder().WithPreviousOptionalSetUnplayed().Build();
            var sut = _kernel.Get<GameResultService>();

            // Act
            sut.Create(newGameResult);

            // Assert
            VerifyCreateGameResult(newGameResult, Times.Never());
        }

        /// <summary>
        /// Test for Create method. Set scores are not listed in the correct order. Exception is thrown during creation.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_GameResultSetScoresUnordered_ExceptionThrown()
        {
            // Arrange
            var newGameResult = new GameResultBuilder().WithSetScoresUnordered().Build();
            var sut = _kernel.Get<GameResultService>();

            // Act
            sut.Create(newGameResult);

            // Assert
            VerifyCreateGameResult(newGameResult, Times.Never());
        }

        /// <summary>
        /// Test for Get method. Existing game result is requested. Game result is returned.
        /// </summary>
        [TestMethod]
        public void Get_ExistingGameResult_GameResultReturned()
        {
            // Arrange
            var expected = new GameResultDtoBuilder().WithId(GAME_RESULT_ID).Build();
            var sut = _kernel.Get<GameResultService>();

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
            var expected = new GameResultTestFixture().TestGameResults().Build();
            var sut = _kernel.Get<GameResultService>();

            SetupGetTournamentResults(TOURNAMENT_ID, expected);

            // Act
            var actual = sut.GetTournamentResults(TOURNAMENT_ID);

            // Assert
            CollectionAssert.AreEqual(expected, actual, new GameResultDtoComparer());
        }

        /// <summary>
        /// Test for Edit method. GameResult object contains valid data. Game result is edited successfully.
        /// </summary>
        [TestMethod]
        public void Edit_GameResultValid_GameResultEdited()
        {
            // Arrange
            var gameResult = new GameResultBuilder().Build();
            var sut = _kernel.Get<GameResultService>();

            // Act
            sut.Edit(gameResult);

            // Assert
            VerifyEditGameResult(gameResult, Times.Once());
        }

        /// <summary>
        /// Test for Edit method. Game result is missing and cannot be edited. Exception is thrown during editing.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(MissingEntityException))]
        public void Edit_MissingGameResult_ExceptionThrown()
        {
            // Arrange
            var gameResult = new GameResultBuilder().Build();
            var sut = _kernel.Get<GameResultService>();

            SetupEditMissingEntityException(gameResult);

            // Act
            sut.Edit(gameResult);

            // Assert
            VerifyEditGameResult(gameResult, Times.Once(), Times.Never());
        }

        /// <summary>
        /// Test for Delete method. Existing game result has to be deleted. Game result is deleted.
        /// </summary>
        [TestMethod]
        public void Delete_ExistingGameResult_GameResultDeleted()
        {
            // Arrange
            var sut = _kernel.Get<GameResultService>();

            // Act
            sut.Delete(GAME_RESULT_ID);

            // Assert
            VerifyDeleteGameResult(GAME_RESULT_ID, Times.Once());
        }

        private bool AreGameResultsEqual(GameResult x, GameResult y)
        {
            return new GameResultComparer().Compare(x, y) == 0;
        }

        private void SetupGet(GameResultDto gameResult)
        {
            _getByIdQueryMock.Setup(m => m.Execute(It.Is<FindByIdCriteria>(c => c.Id == gameResult.Id))).Returns(gameResult);
        }

        private void SetupGetTournamentResults(int tournamentId, List<GameResultDto> gameResults)
        {
            _tournamentGameResultsQueryMock.Setup(m =>
                m.Execute(It.Is<TournamentGameResultsCriteria>(c => c.TournamentId == tournamentId)))
                .Returns(gameResults);
        }

        private void SetupEditMissingEntityException(GameResult gameResult)
        {
            _gameResultRepositoryMock.Setup(m =>
                m.Update(It.Is<GameResult>(grs => AreGameResultsEqual(grs, gameResult))))
                .Throws(new ConcurrencyException());
        }

        private void VerifyCreateGameResult(GameResult gameResult, Times times)
        {
            _gameResultRepositoryMock.Verify(
                m => m.Add(It.Is<GameResult>(grs => AreGameResultsEqual(grs, gameResult))), times);
            _unitOfWorkMock.Verify(m => m.Commit(), times);
        }

        private void VerifyEditGameResult(GameResult gameResult, Times times)
        {
            _gameResultRepositoryMock.Verify(
                m => m.Update(It.Is<GameResult>(grs => AreGameResultsEqual(grs, gameResult))), times);
            _unitOfWorkMock.Verify(m => m.Commit(), times);
        }

        private void VerifyEditGameResult(GameResult gameResult, Times repositoryTimes, Times unitOfWorkTimes)
        {
            _gameResultRepositoryMock.Verify(
                m => m.Update(It.Is<GameResult>(grs => AreGameResultsEqual(grs, gameResult))), repositoryTimes);
            _unitOfWorkMock.Verify(m => m.Commit(), unitOfWorkTimes);
        }

        private void VerifyDeleteGameResult(int gameResultId, Times times)
        {
            _gameResultRepositoryMock.Verify(m => m.Remove(It.Is<int>(id => id == gameResultId)), times);
            _unitOfWorkMock.Verify(m => m.Commit(), times);
        }
    }
}
