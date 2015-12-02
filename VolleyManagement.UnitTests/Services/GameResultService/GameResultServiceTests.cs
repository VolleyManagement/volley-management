namespace VolleyManagement.UnitTests.Services.GameResultService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Queries.Common;
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

        private readonly Mock<IGameResultRepository> _gameResultRepositoryMock = new Mock<IGameResultRepository>();

        private readonly Mock<IQuery<GameResult, FindByIdCriteria>> _getByIdQueryMock
            = new Mock<IQuery<GameResult, FindByIdCriteria>>();

        private readonly Mock<IQuery<List<GameResult>, GetAllCriteria>> _getAllQueryMock
            = new Mock<IQuery<List<GameResult>, GetAllCriteria>>();

        private GameResultService _sut;

        private IKernel _kernel;

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IGameResultRepository>().ToConstant(_gameResultRepositoryMock.Object);
            _kernel.Bind<IQuery<List<GameResult>, GetAllCriteria>>().ToConstant(_getAllQueryMock.Object);
            _kernel.Bind<IQuery<GameResult, FindByIdCriteria>>().ToConstant(_getByIdQueryMock.Object);
            _sut = _kernel.Get<GameResultService>();
        }

        /// <summary>
        /// Test for Create method. GameResult object contains valid data. Game result is created successfully.
        /// </summary>
        [TestMethod]
        public void Create_GameResultValid_GameResultCreated()
        {
            // Arrange
            var newGameResult = new GameResultBuilder().Build();

            // Act
            _sut.Create(newGameResult);

            // Assert
            VerifyCreate(newGameResult, Times.Once());
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

            // Act
            _sut.Create(newGameResult);

            // Assert
            VerifyCreate(newGameResult, Times.Never());
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

            // Act
            _sut.Create(newGameResult);

            // Assert
            VerifyCreate(newGameResult, Times.Never());
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

            // Act
            _sut.Create(newGameResult);

            // Assert
            VerifyCreate(newGameResult, Times.Never());
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

            // Act
            _sut.Create(newGameResult);

            // Assert
            VerifyCreate(newGameResult, Times.Never());
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

            // Act
            _sut.Create(newGameResult);

            // Assert
            VerifyCreate(newGameResult, Times.Never());
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

            // Act
            _sut.Create(newGameResult);

            // Assert
            VerifyCreate(newGameResult, Times.Never());
        }

        /// <summary>
        /// Test for Get method. All game results are requested. All game results are returned.
        /// </summary>
        [TestMethod]
        public void Get_AllGameResults_GameResultsReturned()
        {
            // Arrange
            var expected = new GameResultTestFixture().TestGameResults().Build();
            SetupGetAll(expected);

            // Act
            var actual = _sut.Get();

            // Assert
            CollectionAssert.AreEqual(expected, actual, new GameResultComparer());
        }

        /// <summary>
        /// Test for Get method. Existing game result is requested. Game result is returned.
        /// </summary>
        [TestMethod]
        public void Get_ExistingGameResult_GameResultReturned()
        {
            // Arrange
            var expected = new GameResultBuilder().WithId(GAME_RESULT_ID).Build();
            SetupGet(expected);

            // Act
            var actual = _sut.Get(GAME_RESULT_ID);

            // Assert
            TestHelper.AreEqual(expected, actual, new GameResultComparer());
        }

        private void VerifyCreate(GameResult gameResult, Times times)
        {
            _gameResultRepositoryMock.Verify(grr => grr.Add(It.Is<GameResult>(gr => AreEqualGameResults(gr, gameResult))), times);
        }

        private bool AreEqualGameResults(GameResult x, GameResult y)
        {
            var comparer = new GameResultComparer();
            return comparer.Compare(x, y) == 0;
        }

        private void SetupGetAll(IEnumerable<GameResult> gameResults)
        {
            _getAllQueryMock.Setup(gaq => gaq.Execute(It.IsAny<GetAllCriteria>())).Returns(gameResults.ToList());
        }

        private void SetupGet(GameResult gameResult)
        {
            _getByIdQueryMock.Setup(gbiq => gbiq.Execute(It.Is<FindByIdCriteria>(c => c.Id == gameResult.Id))).Returns(gameResult);
        }
    }
}
