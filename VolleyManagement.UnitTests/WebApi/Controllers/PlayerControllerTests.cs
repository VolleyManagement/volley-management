namespace VolleyManagement.UnitTests.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Http.OData.Results;
    using Contracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Domain.Players;
    using VolleyManagement.UI.Areas.WebApi.ApiControllers;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Players;
    using VolleyManagement.UnitTests.Services.PlayerService;
    using VolleyManagement.UnitTests.WebApi.ViewModels;

    /// <summary>
    /// Tests for PlayerController class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class PlayerControllerTests
    {
        /// <summary>
        /// ID for tests
        /// </summary>
        private const int SPECIFIC_PLAYER_ID = 2;

        /// <summary>
        /// A new but not saved player id
        /// </summary>
        private const int UNASSIGNED_ID = 0;

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
        /// Initializes test data
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IPlayerService>()
                   .ToConstant(_playerServiceMock.Object);
        }

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
            var actual = sut.GetPlayers().ToList();

            //// Assert
            _playerServiceMock.Verify(ts => ts.Get(), Times.Once());
            CollectionAssert.AreEqual(expected, actual, new PlayerViewModelComparer());
        }

        /// <summary>
        /// Test Post method. Basic story.
        /// </summary>
        //[TestMethod]
        //public void Post_IdCreated_IdReturnedWithEntity()
        //{
        //    // Arrange
        //    var controller = _kernel.Get<PlayersController>();
        //    var expectedId = 10;
        //    _playerServiceMock.Setup(ts => ts.Create(It.IsAny<Player>()))
        //        .Callback((Player p) => { p.Id = expectedId; });

        //    // Act
        //    var input = new PlayerViewModelBuilder().WithId(0).Build();
        //    var response = controller.Post(input);
        //    var actual = ((CreatedODataResult<PlayerViewModel>)response).Entity;

        //    // Assert
        //    _playerServiceMock.Verify(ts => ts.Create(It.IsAny<Player>()), Times.AtLeastOnce());
        //    Assert.AreEqual<int>(expectedId, actual.Id);
        //}

        /// <summary>
        /// Test Post method. Does a valid ViewModel return after Player has been created.
        /// </summary>
        //[TestMethod]
        //public void Post_ValidViewModelPlayer_ReturnedAfterCreatedWebApi()
        //{
        //    // Arrange
        //    var controller = _kernel.Get<PlayersController>();
        //    var input = new PlayerViewModelBuilder().WithId(UNASSIGNED_ID).Build();

        //    _playerServiceMock.Setup(ts => ts.Create(It.IsAny<Player>()))
        //        .Callback<Player>(t => { t.Id = SPECIFIC_PLAYER_ID; });

        //    var expected = new PlayerViewModelBuilder().WithId(SPECIFIC_PLAYER_ID).Build();

        //    // Act
        //    var response = controller.Post(input);
        //    var actual = ((CreatedODataResult<PlayerViewModel>)response).Entity;

        //    // Assert
        //    AssertExtensions.AreEqual<PlayerViewModel>(expected, actual, new PlayerViewModelComparer());
        //}

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
