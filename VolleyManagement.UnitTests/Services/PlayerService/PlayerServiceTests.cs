namespace VolleyManagement.UnitTests.Services.PlayerService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Contracts;
    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.Domain.Players;
    using VolleyManagement.Services;

    /// <summary>
    /// Tests for TournamentService class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class PlayerServiceTests
    {
        /// <summary>
        /// Test Fixture.
        /// </summary>
        private readonly PlayerServiceTestFixture _testFixture = new PlayerServiceTestFixture();

        /// <summary>
        /// Players Repository Mock.
        /// </summary>
        private readonly Mock<IPlayerRepository> _playerRepositoryMock = new Mock<IPlayerRepository>();

        /// <summary>
        /// Unit of work mock.
        /// </summary>
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

        /// <summary>
        /// ITournament service mock
        /// </summary>
        private readonly Mock<IPlayerService> _playerServiceMock = new Mock<IPlayerService>();

        /// <summary>
        /// IoC for tests.
        /// </summary>
        private IKernel _kernel;

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IPlayerRepository>()
                   .ToConstant(_playerRepositoryMock.Object);
            _playerRepositoryMock.Setup(tr => tr.UnitOfWork).Returns(_unitOfWorkMock.Object);
        }

        /// <summary>
        /// Test for Get() method. The method should return existing players
        /// (order is important).
        /// </summary>
        [TestMethod]
        public void GetAll_PlayersExist_PlayersReturned()
        {
            // Arrange
            var testData = _testFixture.TestPlayers()
                                       .Build();
            MockRepositoryFindAll(testData);
            var sut = _kernel.Get<PlayerService>();
            var expected = new PlayerServiceTestFixture()
                                            .TestPlayers()
                                            .Build()
                                            .ToList();

            // Act
            var actual = sut.Get().ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual, new PlayerComparer());
        }

        /// <summary>
        /// Mocks Find method.
        /// </summary>
        /// <param name="testData">Test data to mock.</param>
        private void MockRepositoryFindAll(IEnumerable<Player> testData)
        {
            _playerRepositoryMock.Setup(tr => tr.Find()).Returns(testData.AsQueryable());
        }

        /// <summary>
        /// Test for Create() method. The method should create a new player.
        /// </summary>
        [TestMethod]
        public void Create_PlayerPassed_PlayerCreated()
        {
            // Arrange
            var newPlayer = new PlayerBuilder().Build();

            // Act
            var sut = _kernel.Get<PlayerService>();
            sut.Create(newPlayer);

            // Assert
            _playerRepositoryMock.Verify(
                ur => ur.Add(It.Is<Player>(u => PlayersAreEqual(u, newPlayer))));
            _unitOfWorkMock.Verify(u => u.Commit());
        }

        /// <summary>
        /// Find out whether two players objects have the same property values.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if the players have the same property values.</returns>
        private bool PlayersAreEqual(Player x, Player y)
        {
            return new PlayerComparer().Compare(x, y) == 0;
        }
    }
}
