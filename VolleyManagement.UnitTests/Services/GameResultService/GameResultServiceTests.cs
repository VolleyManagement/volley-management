namespace VolleyManagement.UnitTests.Services.GameResultService
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Domain.GameResultsAggregate;
    using VolleyManagement.Services;

    /// <summary>
    /// Tests for <see cref="GameResultService"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class GameResultServiceTests
    {
        private readonly Mock<IGameResultRepository> _gameResultRepositoryMock = new Mock<IGameResultRepository>();
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
        /// Test for Create method. The score of the first set is invalid. Exception is thrown during creation.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_GameResultInvalidSet1Score_ExceptionThrown()
        {
            // Arrange
            var newGameResult = new GameResultBuilder().WithInvalidSet1Score().Build();

            // Act
            _sut.Create(newGameResult);

            // Assert
            VerifyCreate(newGameResult, Times.Never());
        }

        /// <summary>
        /// Test for Create method. The score of the second set is invalid. Exception is thrown during creation.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_GameResultInvalidSet2Score_ExceptionThrown()
        {
            // Arrange
            var newGameResult = new GameResultBuilder().WithInvalidSet2Score().Build();

            // Act
            _sut.Create(newGameResult);

            // Assert
            VerifyCreate(newGameResult, Times.Never());
        }

        /// <summary>
        /// Test for Create method. The score of the third set is invalid. Exception is thrown during creation.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_GameResultInvalidSet3Score_ExceptionThrown()
        {
            // Arrange
            var newGameResult = new GameResultBuilder().WithInvalidSet3Score().Build();

            // Act
            _sut.Create(newGameResult);

            // Assert
            VerifyCreate(newGameResult, Times.Never());
        }

        /// <summary>
        /// Test for Create method. The score of the fourth set is invalid. Exception is thrown during creation.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_GameResultInvalidSet4Score_ExceptionThrown()
        {
            // Arrange
            var newGameResult = new GameResultBuilder().WithInvalidSet4Score().Build();

            // Act
            _sut.Create(newGameResult);

            // Assert
            VerifyCreate(newGameResult, Times.Never());
        }

        /// <summary>
        /// Test for Create method. The score of the fifth set is invalid. Exception is thrown during creation.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_GameResultInvalidSet5Score_ExceptionThrown()
        {
            // Arrange
            var newGameResult = new GameResultBuilder().WithInvalidSet5Score().Build();

            // Act
            _sut.Create(newGameResult);

            // Assert
            VerifyCreate(newGameResult, Times.Never());
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
    }
}
