namespace VolleyManagement.UnitTests.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Http.OData.Results;
    using System.Web.Http.Results;
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
        /// Not valid birth year
        /// </summary>
        private const int NOT_VALID_BIRTH_YEAR = 2101;

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
            MockPlayers(testData);
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
        [TestMethod]
        public void Post_IdCreated_IdReturnedWithEntity()
        {
            // Arrange
            var controller = _kernel.Get<PlayersController>();
            var expectedId = 10;
            _playerServiceMock.Setup(ps => ps.Create(It.IsAny<Player>()))
                .Callback((Player p) => { p.Id = expectedId; });

            // Act
            var input = new PlayerViewModelBuilder().WithId(0).Build();
            var response = controller.Post(input);
            var actual = ((CreatedODataResult<PlayerViewModel>)response).Entity;

            // Assert
            _playerServiceMock.Verify(ps => ps.Create(It.IsAny<Player>()), Times.AtLeastOnce());
            Assert.AreEqual<int>(expectedId, actual.Id);
        }

        /// <summary>
        /// Test Post method. Does a valid ViewModel return after Player has been created.
        /// </summary>
        [TestMethod]
        public void Post_ValidViewModelPlayer_ReturnedAfterCreatedWebApi()
        {
            // Arrange
            var controller = _kernel.Get<PlayersController>();
            var input = new PlayerViewModelBuilder().WithId(UNASSIGNED_ID).Build();

            _playerServiceMock.Setup(ts => ts.Create(It.IsAny<Player>()))
                .Callback<Player>(p => { p.Id = SPECIFIC_PLAYER_ID; });

            var expected = new PlayerViewModelBuilder().WithId(SPECIFIC_PLAYER_ID).Build();

            // Act
            var response = controller.Post(input);
            var actual = ((CreatedODataResult<PlayerViewModel>)response).Entity;

            // Assert
            AssertExtensions.AreEqual<PlayerViewModel>(expected, actual, new PlayerViewModelComparer());
        }

        /// <summary>
        /// Test Post method(). Returns InvalidModelStateResult
        /// if the ModelState has some errors
        /// </summary>
        [TestMethod]
        public void Post_NotValidPlayerViewModel_ReturnBadRequestWebApi()
        {
            // Arrange
            var controller = _kernel.Get<PlayersController>();
            controller.ModelState.Clear();
            var notValidViewModel = new PlayerViewModelBuilder().WithBirthYear(NOT_VALID_BIRTH_YEAR).Build();
            controller.ModelState.AddModelError("NotValidBirthYear", "Birth year field isn't valid");

            // Act
            var result = controller.Post(notValidViewModel) as InvalidModelStateResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ModelState.Count == 1);
            Assert.IsTrue(result.ModelState.Keys.Contains("NotValidBirthYear"));
        }

        /// <summary>
        /// Test Post method. Is valid player domain model
        /// pass to Create Service method
        /// </summary>
        [TestMethod]
        public void Post_ValidPlayerDomain_PassToCreateMethod()
        {
            // Arrange
            var controller = _kernel.Get<PlayersController>();
            var sent = new PlayerViewModelBuilder().Build();
            var expected = new PlayerViewModelBuilder().Build();

            var expectedDomain = new PlayerBuilder()
                .WithId(expected.Id)
                .WithFirstName(expected.FirstName)
                .WithLastName(expected.LastName)
                .WithBirthYear(expected.BirthYear)
                .WithWeight(expected.Weight)
                .WithHeight(expected.Height)
                .Build();

            // Act
            controller.Post(sent);

            // Assert
            _playerServiceMock.Verify(
                pServ => pServ.Create(It.Is<Player>(p => new PlayerComparer().AreEqual(p, expectedDomain))),
                Times.Once());
        }

        /// <summary>
        /// Test Put method(). Returns InvalidModelStateResult
        /// if the ModelState has some errors
        /// </summary>
        [TestMethod]
        public void Put_NotValidPlayerViewModel_ReturnBadRequestWebApi()
        {
            // Arrange
            var controller = _kernel.Get<PlayersController>();
            controller.ModelState.Clear();
            var notValidViewModel = new PlayerViewModelBuilder().WithBirthYear(1).Build();
            controller.ModelState.AddModelError("NotValidBirthYear", "BirthYear field isn't valid");

            // Act
            var result = controller.Post(notValidViewModel) as InvalidModelStateResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ModelState.Count == 1);
            Assert.IsTrue(result.ModelState.Keys.Contains("NotValidBirthYear"));
        }

        /// <summary>
        /// Test Put method. Is valid player domain model
        /// pass to Edit Service method
        /// </summary>
        [TestMethod]
        public void Put_ValidPlayerDomain_PassToEditMethod()
        {
            // Arrange
            var controller = _kernel.Get<PlayersController>();
            var sent = new PlayerViewModelBuilder().Build();
            var expected = new PlayerViewModelBuilder().Build();

            var expectedDomain = new PlayerBuilder()
                .WithId(expected.Id)
                .WithFirstName(expected.FirstName)
                .WithLastName(expected.LastName)
                .WithBirthYear(expected.BirthYear)
                .WithHeight(expected.Height)
                .WithWeight(expected.Weight)
                .Build();

            // Act
            controller.Put(sent);

            // Assert
            _playerServiceMock.Verify(
                plServ => plServ.Edit(It.Is<Player>(p => new PlayerComparer().AreEqual(p, expectedDomain))),
                Times.Once());
        }

        /// <summary>
        /// Test Put method. Is valid player view model
        /// returns after create
        /// </summary>
        [TestMethod]
        public void Put_ValidPlayerViewModelIncoming_ValidViewModelOutcoming()
        {
            // Arrange
            var incomingPlayerVM = new PlayerViewModelBuilder().Build();
            var expectedPlayerVM = new PlayerViewModelBuilder().Build();
            var controller = _kernel.Get<PlayersController>();

            // Act
            var response = controller.Put(incomingPlayerVM);
            var actual = ((UpdatedODataResult<PlayerViewModel>)response).Entity;

            // Assert
            AssertExtensions.AreEqual<PlayerViewModel>(expectedPlayerVM, actual, new PlayerViewModelComparer());
        }

        /// <summary>
        /// Test Put method. Thrown Agrument exception
        /// returns bad request
        /// </summary>
        [TestMethod]
        public void Put_ThrownArgumentException_BadRequestReturns()
        {
            // Arrange
            var controller = _kernel.Get<PlayersController>();
            var playerViewModel = new PlayerViewModelBuilder().Build();
            _playerServiceMock.Setup(
                ps => ps.Edit(It.IsAny<Player>())).Throws(new ArgumentException());

            // Act
            var result = controller.Put(playerViewModel) as InvalidModelStateResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ModelState.Count > 0);
        }

        /// <summary>
        /// Test Put method. Thrown ValidationException
        /// returns bad request
        /// </summary>
        [TestMethod]
        public void Put_ThrownValidationException_BadRequestReturns()
        {
            // Arrange
            var controller = _kernel.Get<PlayersController>();
            var playerViewModel = new PlayerViewModelBuilder().Build();
            _playerServiceMock.Setup(
                ps => ps.Edit(It.IsAny<Player>())).Throws(new ValidationException());

            // Act
            var result = controller.Put(playerViewModel) as InvalidModelStateResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ModelState.Count > 0);
        }

        /// <summary>
        /// Mock the players
        /// </summary>
        /// <param name="testData">Data what will be returned</param>
        private void MockPlayers(IList<Player> testData)
        {
            _playerServiceMock.Setup(tr => tr.Get())
                                            .Returns(testData.AsQueryable());
        }
    }
}
