namespace VolleyManagement.UnitTests.Services.PlayerService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.Domain.Players;
    using VolleyManagement.Services;

    /// <summary>
    /// Tests for PlayerService class.
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PlayerServiceTests
    {
        /// <summary>
        /// Players Repository Mock.
        /// </summary>
        private readonly Mock<IPlayerRepository> _playerRepositoryMock = new Mock<IPlayerRepository>();

        /// <summary>
        /// Unit of work mock.
        /// </summary>
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

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
