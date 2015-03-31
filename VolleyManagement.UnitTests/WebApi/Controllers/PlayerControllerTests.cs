namespace VolleyManagement.UnitTests.WebApi.Controllers
{
    using Contracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using VolleyManagement.Domain.Players;
    using VolleyManagement.UI.Areas.WebApi.ApiControllers;
    using VolleyManagement.UnitTests.Services.PlayerService;

    /// <summary>
    /// Tests for PlayerController class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    class PlayerControllerTests
    {
        /// <summary>
        /// Test Fixture
        /// </summary>
        private readonly PlayerServiceTestFixture _testFixture = new PlayerServiceTestFixture();

        /// <summary>
        /// Players Service Mock
        /// </summary>
        private readonly Mock<IPlayerService> _playerServiceMock = new Mock<IPlayerService>();

        /// <summary>
        /// IoC for tests
        /// </summary>
        private IKernel _kernel;

        /// <summary>
        /// Test for Get() method. The method should return existing players
        /// </summary>
        [TestMethod]
        public void Get_PlayersExist_PlayersReturned()
        {
            // Arrange
            var testData = _testFixture.TestPlayers()
                                            .Build();
            MockTournaments(testData);
            var sut = _kernel.Get<PlayersController>();

            //// Expected result
            var expected = new PlayerViewModelServiceTestFixture()
                                            .TestPlayers()
                                            .Build()
                                            .ToList();

            //// Actual result
            var actual = sut.GetTournaments().ToList();

            //// Assert
            _playerServiceMock.Verify(ts => ts.Get(), Times.Once());
            CollectionAssert.AreEqual(expected, actual, new PlayerViewModelComparer());
        }

        /// <summary>
        /// Mock the players
        /// </summary>
        /// <param name="testData">Data what will be returned</param>
        private void MockTournaments(IList<Player> testData)
        {
            _playerServiceMock.Setup(tr => tr.Get())
                                            .Returns(testData.AsQueryable());
        }
    }
}
