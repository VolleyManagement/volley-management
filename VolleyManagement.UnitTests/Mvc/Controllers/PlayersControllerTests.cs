namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Contracts;
    using Domain.Players;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Dal.Exceptions;
    using VolleyManagement.UI.App_GlobalResources;
    using VolleyManagement.UI.Areas.Mvc.Controllers;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Players;
    using VolleyManagement.UnitTests.Mvc.ViewModels;
    using VolleyManagement.UnitTests.Services.PlayerService;

    /// <summary>
    /// Tests for MVC PlayersController class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class PlayersControllerTests
    {
        private const int NUMBER_OF_PLAYERS_FOR_MOCK = 12;
        private const int FIRST_ASCII_LETTER = 65;
        private const int LAST_ASCII_LETTER = 90;
        private const int MAX_PLAYERS_ON_PAGE = 10;
        private const int TESTING_PAGE = 1;
        private const int SAVED_PLAYER_ID = 10;
        private const int PLAYER_UNEXISTING_ID_TO_DELETE = 4;
        private const string SUBSTRING_TO_SEARCH = "CLastName";
        private const string HTTP_NOT_FOUND_DESCRIPTION
            = "При удалении игрока произошла непредвиденная ситуация. Пожалуйста, обратитесь к администратору";

        private const string TEST_CONTROLLER_NAME = "TestController";

        private readonly Mock<IPlayerService> _playerServiceMock = new Mock<IPlayerService>();
        private IKernel _kernel;

        /// <summary>
        /// Initializes test data
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            this._kernel = new StandardKernel();
            this._kernel.Bind<IPlayerService>()
                   .ToConstant(this._playerServiceMock.Object);
        }

        /// <summary>
        /// Delete method test. The method should invoke Delete() method of IPlayerService
        /// and return result as JavaScript Object Notation.
        /// </summary>
        [TestMethod]
        public void Delete_PlayerExists_PlayerIsDeleted()
        {
            // Act
            var sut = this._kernel.Get<PlayersController>();
            var actual = sut.Delete(PLAYER_UNEXISTING_ID_TO_DELETE) as JsonResult;

            // Assert
            _playerServiceMock.Verify(ps => ps.Delete(It.Is<int>(id => id == PLAYER_UNEXISTING_ID_TO_DELETE)), Times.Once());
            Assert.IsNotNull(actual);
        }

        /// <summary>
        /// Delete method test. Input parameter is player id, which doesn't exist in database.
        /// The method should return message as JavaScript Object Notation.
        /// </summary>
        [TestMethod]
        public void Delete_PlayerDoesntExist_JsonReturned()
        {
            // Arrange
            _playerServiceMock.Setup(ps => ps.Delete(PLAYER_UNEXISTING_ID_TO_DELETE)).Throws<MissingEntityException>();

            // Act
            var sut = this._kernel.Get<PlayersController>();
            var actual = sut.Delete(PLAYER_UNEXISTING_ID_TO_DELETE) as JsonResult;

            // Assert
            Assert.IsNotNull(actual);
        }

        /// <summary>
        /// Test for Index action. The action should return page of specified players
        /// </summary>
        [TestMethod]
        public void Index_PlayersExist_SpecifiedPlayersPageReturned()
        {
            // Arrange
            List<Player> listOfPlayers = new List<Player>()
            {
                new Player() { Id = 1, FirstName = "FirstNameA", LastName = "LastNameA" },
                new Player() { Id = 2, FirstName = "FirstNameB", LastName = "LastNameB" },
                new Player() { Id = 3, FirstName = "FirstNameC", LastName = "LastNameC" },
                new Player() { Id = 4, FirstName = "FirstNameD", LastName = "LastNameD" }
            };

            _playerServiceMock.Setup(p => p.Get()).Returns(listOfPlayers.AsQueryable());
            var sup = this._kernel.Get<PlayersController>();

            var expected = listOfPlayers.OrderBy(p => p.LastName)
                .Where(p => (p.FirstName + " "+ p.LastName).Contains(SUBSTRING_TO_SEARCH))
                .Skip((TESTING_PAGE - 1) * MAX_PLAYERS_ON_PAGE)
                .Take(MAX_PLAYERS_ON_PAGE)
                .Select(p =>
                    new PlayerNameViewModel
                    {
                        Id = p.Id,
                        FullName = p.LastName + " " + p.FirstName
                    })
                .ToList();

            // Act
            var actual = TestExtensions.GetModel<PlayersListViewModel>(sup.Index(null, SUBSTRING_TO_SEARCH)).List;

            // Assert
            CollectionAssert.AreEqual(expected, actual, new PlayerNameViewModelComparer());
            Assert.AreEqual(expected.Count, actual.Count);
        }

        /// <summary>
        /// Test for Index action. The action should return not empty ordering page with players
        /// </summary>
        [TestMethod]
        public void Index_PlayersExist_PlayersPageReturned()
        {
            // Arrange
            List<Player> currectList = new List<Player>();
            Random rand = new Random();

            for (int i = 0; i < NUMBER_OF_PLAYERS_FOR_MOCK; i++)
            {
                char lastName = (char)rand.Next(FIRST_ASCII_LETTER, LAST_ASCII_LETTER + 1);
                currectList.Add(new PlayerBuilder()
                    .WithId(i)
                    .WithLastName(lastName.ToString())
                    .Build());
            }

            _playerServiceMock.Setup(tr => tr.Get()).Returns(currectList.AsQueryable());

            var sut = this._kernel.Get<PlayersController>();
            var expected = currectList.OrderBy(p => p.LastName)
                .Skip((TESTING_PAGE - 1) * MAX_PLAYERS_ON_PAGE)
                .Take(MAX_PLAYERS_ON_PAGE)
                .Select(p =>
                    new PlayerNameViewModel
                    {
                        Id = p.Id,
                        FullName = p.LastName + " " + p.FirstName
                    })
                .ToList();

            // Act
            var actual = TestExtensions.GetModel<PlayersListViewModel>(sut.Index(TESTING_PAGE)).List;

            // Assert
            CollectionAssert.AreEqual(expected, actual, new PlayerNameViewModelComparer());
        }

        /// <summary>
        /// Test with negative scenario for Index action.
        /// The action should thrown Argument null exception
        /// </summary>
        [TestMethod]
        public void Index_PlayersDoNotExist_ExceptionThrown()
        {
            // Arrange
            this._playerServiceMock.Setup(tr => tr.Get())
                .Throws(new ArgumentNullException());

            var sut = this._kernel.Get<PlayersController>();
            var expected = (int)HttpStatusCode.NotFound;

            // Act
            var actual = (sut.Index(It.IsAny<int>()) as HttpNotFoundResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test for Details(). Requested id does not exist in the database.
        /// </summary>
        [TestMethod]
        public void Details_PlayerDoesNotExist_NotFoundResult()
        {
            // Arrange
            this._playerServiceMock.Setup(ps => ps.Get(It.IsAny<int>()))
                .Throws(new MissingEntityException());

            var sut = this._kernel.Get<PlayersController>();
            var expected = (int)HttpStatusCode.NotFound;

            // Act
            var actual = (sut.Details(It.IsAny<int>()) as HttpNotFoundResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test for Details(). Gets player with requested id.
        /// </summary>
        [TestMethod]
        public void Details_PlayerExists_PlayerIsReturned()
        {
            // Arrange
            _playerServiceMock.Setup(tr => tr.Get(It.Is<int>(id => id == SAVED_PLAYER_ID)))
                .Returns(new PlayerBuilder()
                .WithId(SAVED_PLAYER_ID)
                .Build());

            var controller = this._kernel.Get<PlayersController>();
            SetControllerRouteData(controller);

            var expectedPlayer = new PlayerMvcViewModelBuilder()
                .WithId(SAVED_PLAYER_ID)
                .Build();

            var expectedReferer = TEST_CONTROLLER_NAME;

            // Act
            var actual = TestExtensions.GetModel<PlayerRefererViewModel>(controller.Details(SAVED_PLAYER_ID));

            // Assert
            AssertExtensions.AreEqual<PlayerViewModel>(expectedPlayer, actual.Model, new PlayerViewModelComparer());
            Assert.AreEqual<string>(expectedReferer, actual.Referer);
        }

        /// <summary>
        /// Test for Create player action (GET)
        /// </summary>
        [TestMethod]
        public void CreateGetAction_GetView_ReturnsViewWithDefaultData()
        {
            // Arrange
            var controller = _kernel.Get<PlayersController>();
            var expected = new PlayerViewModel();

            // Act
            var actual = TestExtensions.GetModel<PlayerViewModel>(controller.Create());

            // Assert
            AssertExtensions.AreEqual<PlayerViewModel>(expected, actual, new PlayerViewModelComparer());
        }

        /// <summary>
        /// Create player action test (POST)
        /// </summary>
        [TestMethod]
        public void CreatePostAction_ValidPlayerViewModel_RedirectToIndex()
        {
            // Arrange
            var playerController = _kernel.Get<PlayersController>();
            var playerViewModel = new PlayerMvcViewModelBuilder()
                .WithId(0)
                .WithFirstName("FirstName")
                .WithLastName("LastName")
                .WithBirthYear(1983)
                .WithHeight(186)
                .WithWeight(95)
                .WithTeamId(1)
                .Build();

            var expected = new PlayerBuilder()
                .WithId(0)
                .WithFirstName("FirstName")
                .WithLastName("LastName")
                .WithBirthYear(1983)
                .WithHeight(186)
                .WithWeight(95)
                .WithTeamId(1)
                .Build();

            // Act
            var result = playerController.Create(playerViewModel) as RedirectToRouteResult;

            // Assert
            _playerServiceMock.Verify(ts => ts.Create(It.Is<Player>(pl => new PlayerComparer().AreEqual(pl, expected))), Times.Once());
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        /// <summary>
        /// Create player action test (POST)
        /// </summary>
        [TestMethod]
        public void CreatePostAction_ValidPlayerViewModel_GotNewIdFromDatabase()
        {
            // Arrange
            var playerController = _kernel.Get<PlayersController>();
            var playerViewModel = new PlayerMvcViewModelBuilder().Build();

            _playerServiceMock.Setup(ps => ps.Create(It.IsAny<Player>())).Callback<Player>(p => p.Id = SAVED_PLAYER_ID);

            // Act
            playerController.Create(playerViewModel);

            // Assert
            Assert.AreEqual(SAVED_PLAYER_ID, playerViewModel.Id, "The player wasn't saved");
        }

        /// <summary>
        /// Create player action test with an invalid view model (POST)
        /// </summary>
        [TestMethod]
        public void CreatePostAction_InvalidPlayerViewModel_ReturnsViewModelToView()
        {
            // Arrange
            var controller = _kernel.Get<PlayersController>();
            controller.ModelState.AddModelError("Key", "ModelIsInvalidNow");
            var playerViewModel = new PlayerMvcViewModelBuilder()
                .WithFirstName(string.Empty)
                .Build();

            // Act
            var actual = TestExtensions.GetModel<PlayerViewModel>(controller.Create(playerViewModel));

            // Assert
            _playerServiceMock.Verify(ps => ps.Create(It.IsAny<Player>()), Times.Never());
            Assert.IsNotNull(actual, "Model with incorrect data should be returned to the view.");
        }

        /// <summary>
        /// Create player action test (POST)
        /// </summary>
        [TestMethod]
        public void CreatePostAction_ArgumentException_ExceptionThrown()
        {
            // Arrange
            var playerViewModel = new PlayerMvcViewModelBuilder().Build();

            _playerServiceMock.Setup(ps => ps.Create(It.IsAny<Player>()))
                .Throws(new ArgumentException());
            var controller = _kernel.Get<PlayersController>();

            // Act
            var actual = TestExtensions.GetModel<PlayerViewModel>(controller.Create(playerViewModel));

            // Assert
            Assert.AreNotEqual(0, controller.ModelState.Count, "Model state should containe some error");
            Assert.IsNotNull(actual, "Model with incorrect data should be returned to the view.");
        }

        /// <summary>
        /// Create player action test (POST)
        /// </summary>
        [TestMethod]
        public void CreatePostAction_GeneralExceptionCatch_ReturnToEditPage()
        {
            // Arrange
            var playerViewModel = new PlayerMvcViewModelBuilder().Build();

            _playerServiceMock.Setup(ps => ps.Create(It.IsAny<Player>()))
                .Throws(new Exception());
            var controller = _kernel.Get<PlayersController>();

            // Act
            var actual = controller.Create(playerViewModel);

            // Assert
            Assert.IsInstanceOfType(actual, typeof(HttpNotFoundResult));
        }

        /// <summary>
        /// Test for Edit player action (GET)
        /// </summary>
        [TestMethod]
        public void EditGetAction_PlayerViewModel_ReturnsToTheView()
        {
            // Arrange
            var controller = _kernel.Get<PlayersController>();
            var player = new PlayerBuilder()
                            .WithId(1)
                            .WithFirstName("firstName")
                            .WithLastName("lastName")
                            .WithBirthYear(1993)
                            .WithHeight(201)
                            .WithWeight(80)
                            .Build();
            MockSinglePlayer(player);
            var expected = new PlayerMvcViewModelBuilder()
                            .WithId(1)
                            .WithFirstName("firstName")
                            .WithLastName("lastName")
                            .WithBirthYear(1993)
                            .WithHeight(201)
                            .WithWeight(80)
                            .Build();

            // Act
            var actual = TestExtensions.GetModel<PlayerViewModel>(controller.Edit(player.Id));

            // Assert
            AssertExtensions.AreEqual<PlayerViewModel>(expected, actual, new PlayerViewModelComparer());
        }

        /// <summary>
        /// Test for Edit player action (GET)
        /// </summary>
        [TestMethod]
        public void EditGetAction_MissingEntityExceptionCatch_NotFoundReturn()
        {
            // Arrange
            var playerId = 5;
            _playerServiceMock.Setup(ts => ts.Get(playerId))
               .Throws(new MissingEntityException());
            var controller = _kernel.Get<PlayersController>();

            // Act
            var actual = controller.Edit(playerId);

            // Assert
            Assert.IsInstanceOfType(actual, typeof(HttpNotFoundResult));
        }

        /// <summary>
        /// Test for Edit player action (POST)
        /// </summary>
        [TestMethod]
        public void EditPostAction_ValidPlayerViewModel_RedirectToIndex()
        {
            // Arrange
            var playersController = _kernel.Get<PlayersController>();
            var playerViewModel = new PlayerMvcViewModelBuilder()
                            .WithId(1)
                            .WithFirstName("firstName")
                            .WithLastName("lastName")
                            .WithBirthYear(1993)
                            .WithHeight(201)
                            .WithWeight(80)
                            .Build();

            // Act
            var result = playersController.Edit(playerViewModel) as RedirectToRouteResult;

            // Assert
            _playerServiceMock.Verify(ts => ts.Edit(It.IsAny<Player>()), Times.Once());
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        /// <summary>
        /// Test for Edit player action with invalid model (POST)
        /// </summary>
        [TestMethod]
        public void EditPostAction_InvalidPlayerViewModel_ReturnsViewModelToView()
        {
            // Arrange
            var controller = _kernel.Get<PlayersController>();
            controller.ModelState.AddModelError("Key", "ModelIsInvalidNow");
            var playerViewModel = new PlayerMvcViewModelBuilder()
                .WithFirstName(string.Empty)
                .Build();

            // Act
            var actual = TestExtensions.GetModel<PlayerViewModel>(controller.Edit(playerViewModel));

            // Assert
            _playerServiceMock.Verify(ts => ts.Edit(It.IsAny<Player>()), Times.Never());
            Assert.IsNotNull(actual, "Model with incorrect data should be returned to the view.");
        }

        /// <summary>
        /// Test for Edit player action (POST)
        /// </summary>
        [TestMethod]
        public void EditPostAction_ArgumentException_ExceptionThrown()
        {
            // Arrange
            var playerViewModel = new PlayerMvcViewModelBuilder()
                            .WithId(1)
                            .WithFirstName("firstName")
                            .WithLastName("lastName")
                            .WithBirthYear(1993)
                            .WithHeight(201)
                            .WithWeight(80)
                            .Build();
            _playerServiceMock.Setup(ts => ts.Edit(It.IsAny<Player>()))
                .Throws(new ValidationException());
            var controller = _kernel.Get<PlayersController>();

            // Act
            var actual = TestExtensions.GetModel<PlayerViewModel>(controller.Edit(playerViewModel));

            // Assert
            Assert.IsNotNull(actual, "Model with incorrect data should be returned to the view.");
        }

        /// <summary>
        /// Test for Edit player action (POST)
        /// </summary>
        [TestMethod]
        public void EditPostAction_MissingEntityExceptionCatch_ViewModelWithErrorReturn()
        {
            // Arrange
            var playerViewModel = new PlayerMvcViewModelBuilder()
                            .WithId(1)
                            .WithFirstName("firstName")
                            .WithLastName("lastName")
                            .WithBirthYear(1993)
                            .WithHeight(201)
                            .WithWeight(80)
                            .Build();
            _playerServiceMock.Setup(ts => ts.Edit(It.IsAny<Player>()))
                .Throws(new MissingEntityException());
            var controller = _kernel.Get<PlayersController>();

            // Act
            var actual = controller.Edit(playerViewModel);

            // Assert
            Assert.AreEqual(1, controller.ModelState.Count);
        }

        /// <summary>
        /// Mocks test data
        /// </summary>
        /// <param name="testData">Data to mock</param>
        private void MockSinglePlayer(Player testData)
        {
            _playerServiceMock.Setup(tr => tr.Get(testData.Id)).Returns(testData);
        }

        /// <summary>
        /// Set test route data to the ControllerContext.
        /// </summary>
        /// <param name="controller">Controller to set the route data of.</param>
        private void SetControllerRouteData(PlayersController controller)
        {
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.RouteData = new RouteData();
            controller.RouteData.Values["controller"] = TEST_CONTROLLER_NAME;
        }
    }
}
