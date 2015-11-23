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

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Domain.PlayersAggregate;
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
        private const int MAX_PLAYERS_ON_PAGE = 5;
        private const int TESTING_PAGE = 1;
        private const int SAVED_PLAYER_ID = 10;
        private const int PLAYER_UNEXISTING_ID_TO_DELETE = 4;
        private const string SUBSTRING_TO_SEARCH = "CLastName";
        private const string HTTP_NOT_FOUND_DESCRIPTION
            = "При удалении игрока произошла непредвиденная ситуация. Пожалуйста, обратитесь к администратору";

        private const string TEST_CONTROLLER_NAME = "TestController";
        private const string INDEX_ACTION_NAME = "Index";
        private const string ROUTE_VALUES_KEY = "action";
        private const string ASSERT_FAIL_VIEW_MODEL_MESSAGE = "View model must be returned to user.";
        private const int TEST_PLAYER_ID = 1;

        private readonly Mock<IPlayerService> _playerServiceMock = new Mock<IPlayerService>();
        private IKernel _kernel;
        private PlayersController _sut;

        /// <summary>
        /// Initializes test data
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            this._kernel = new StandardKernel();
            this._kernel.Bind<IPlayerService>().ToConstant(this._playerServiceMock.Object);
            this._sut = this._kernel.Get<PlayersController>();
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
            var listOfPlayers = new List<Player>()
            {
                new Player() { Id = 1, FirstName = "FirstNameA", LastName = "LastNameA" },
                new Player() { Id = 2, FirstName = "FirstNameB", LastName = "LastNameB" },
                new Player() { Id = 3, FirstName = "FirstNameC", LastName = "LastNameC" },
                new Player() { Id = 4, FirstName = "FirstNameD", LastName = "LastNameD" }
            };

            _playerServiceMock.Setup(p => p.Get()).Returns(listOfPlayers.AsQueryable());
            var sup = this._kernel.Get<PlayersController>();

            var expected = listOfPlayers.OrderBy(p => p.LastName)
                .Where(p => (p.FirstName + " " + p.LastName).Contains(SUBSTRING_TO_SEARCH))
                .Skip((TESTING_PAGE - 1) * MAX_PLAYERS_ON_PAGE)
                .Take(MAX_PLAYERS_ON_PAGE)
                .Select(p => new PlayerNameViewModel { Id = p.Id, FullName = p.LastName + " " + p.FirstName })
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
        /// Test for Details method. Player with specified identifier does not exist. HttpNotFoundResult is returned.
        /// </summary>
        [TestMethod]
        public void Details_NonExistentPlayer_HttpNotFoundResultIsReturned()
        {
            // Arrange
            MockSetupGet(null);

            // Act
            var result = this._sut.Details(TEST_PLAYER_ID);

            // Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        /// <summary>
        /// Test for Details method. Player with specified identifier exists. View model of Player is returned.
        /// </summary>
        [TestMethod]
        public void Details_ExistingPlayer_PlayerViewModelIsReturned()
        {
            // Arrange
            var testData = MakeTestPlayer(TEST_PLAYER_ID);
            var expected = MakeTestPlayerViewModel(TEST_PLAYER_ID);
            MockSetupGet(testData);

            // Act
            var actual = TestExtensions.GetModel<PlayerRefererViewModel>(this._sut.Details(TEST_PLAYER_ID));

            // Assert
            TestHelper.AreEqual<PlayerViewModel>(expected, actual.Model, new PlayerViewModelComparer());
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
            TestHelper.AreEqual<PlayerViewModel>(expected, actual, new PlayerViewModelComparer());
        }

        /// <summary>
        /// Test for Create method (POST action). Player view model is not valid.
        /// Player is not created and player view model is returned.
        /// </summary>
        [TestMethod]
        public void CreatePostAction_InvalidPlayerViewModel_PlayerViewModelIsReturned()
        {
            // Arrange
            var testData = MakeTestPlayerViewModel();
            this._sut.ModelState.AddModelError(string.Empty, string.Empty);

            // Act
            var result = TestExtensions.GetModel<PlayerViewModel>(this._sut.Create(testData));

            // Assert
            VerifyCreate(Times.Never());
            Assert.IsNotNull(result, ASSERT_FAIL_VIEW_MODEL_MESSAGE);
        }

        /// <summary>
        /// Test for Create method (POST action). Player view model is valid and no exception is thrown during creation.
        /// Player is created successfully and user is redirected to the Index page.
        /// </summary>
        [TestMethod]
        public void CreatePostAction_ValidPlayerViewModelNoException_PlayerIsCreated()
        {
            // Arrange
            var testData = MakeTestPlayerViewModel();

            // Act
            var result = this._sut.Create(testData) as RedirectToRouteResult;

            // Assert
            VerifyCreate(Times.Once());
            VerifyRedirect(INDEX_ACTION_NAME, result);
        }

        /// <summary>
        /// Test for Create method (POST action). Player view model is valid, but exception is thrown during creation.
        /// Player view model is returned.
        /// </summary>
        [TestMethod]
        public void CreatePostAction_ValidPlayerViewModelWithArgumentException_PlayerViewModelIsReturned()
        {
            // Arrange
            var testData = MakeTestPlayerViewModel();
            MockSetupCreateArgumentException();

            // Act
            var result = TestExtensions.GetModel<PlayerViewModel>(this._sut.Create(testData));

            // Assert
            VerifyCreate(Times.Once());
            Assert.IsNotNull(result, ASSERT_FAIL_VIEW_MODEL_MESSAGE);
        }

        /// <summary>
        /// Test for Create method (POST action). Player view model is valid, but exception is thrown during creation.
        /// Player view model is returned.
        /// </summary>
        [TestMethod]
        public void CreatePostAction_ValidPlayerViewModelWithValidationException_PlayerViewModelIsReturned()
        {
            // Arrange
            var testData = MakeTestPlayerViewModel();
            MockSetupCreateValidationException();

            // Act
            var result = TestExtensions.GetModel<PlayerViewModel>(this._sut.Create(testData));

            // Assert
            VerifyCreate(Times.Once());
            Assert.IsNotNull(result, ASSERT_FAIL_VIEW_MODEL_MESSAGE);
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
            TestHelper.AreEqual<PlayerViewModel>(expected, actual, new PlayerViewModelComparer());
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
            controller.Edit(playerViewModel);

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

        /// <summary>
        /// Makes player with specified identifier filled with test data.
        /// </summary>
        /// <param name="playertId">Identifier of the player.</param>
        /// <returns>Player filled with test data.</returns>
        private Player MakeTestPlayer(int playertId)
        {
            return new PlayerBuilder().WithId(playertId).Build();
        }

        /// <summary>
        /// Makes player view model filled with test data.
        /// </summary>
        /// <returns>Player view model filled with test data.</returns>
        private PlayerViewModel MakeTestPlayerViewModel()
        {
            return new PlayerMvcViewModelBuilder().Build();
        }

        /// <summary>
        /// Makes player view model with specified player identifier filled with test data.
        /// </summary>
        /// <param name="playerId">Identifier of the player.</param>
        /// <returns>Player view model filled with test data.</returns>
        private PlayerViewModel MakeTestPlayerViewModel(int playerId)
        {
            return new PlayerMvcViewModelBuilder().WithId(playerId).Build();
        }

        /// <summary>
        /// Gets system being tested by a unit test.
        /// </summary>
        /// <returns>System being tested by a unit test.</returns>
        private PlayersController GetSystemUnderTest()
        {
            return this._kernel.Get<PlayersController>();
        }

        /// <summary>
        /// Sets up a mock for Get method of Player service with any parameter to return specified player.
        /// </summary>
        /// <param name="player">Player that will be returned by Get method of Player service.</param>
        private void MockSetupGet(Player player)
        {
            this._playerServiceMock.Setup(tr => tr.Get(It.IsAny<int>())).Returns(player);
        }

        /// <summary>
        /// Sets up a mock for Create method of Player service to throw ArgumentException.
        /// </summary>
        private void MockSetupCreateArgumentException()
        {
            this._playerServiceMock.Setup(ts => ts.Create(It.IsAny<Player>()))
                .Throws(new ArgumentException(string.Empty, string.Empty));
        }

        /// <summary>
        /// Sets up a mock for Create method of Player service to throw ValidationException.
        /// </summary>
        private void MockSetupCreateValidationException()
        {
            this._playerServiceMock.Setup(ts => ts.Create(It.IsAny<Player>()))
                .Throws(new ValidationException(string.Empty));
        }

        ///// <summary>
        ///// Sets up a mock for Edit method of Tournament service to throw TournamentValidationException.
        ///// </summary>
        //private void MockSetupEditTournamentValidationException()
        //{
        //    this._tournamentServiceMock.Setup(ts => ts.Edit(It.IsAny<Tournament>()))
        //        .Throws(new TournamentValidationException(string.Empty, string.Empty, string.Empty));
        //}

        /// <summary>
        /// Verifies that tournament is created required number of times.
        /// </summary>
        /// <param name="times">Number of times tournament must be created.</param>
        private void VerifyCreate(Times times)
        {
            this._playerServiceMock.Verify(ts => ts.Create(It.IsAny<Player>()), times);
        }

        /// <summary>
        /// Verifies that tournament is updated required number of times.
        /// </summary>
        /// <param name="times">Number of times tournament must be updated.</param>
        private void VerifyEdit(Times times)
        {
            this._playerServiceMock.Verify(ts => ts.Edit(It.IsAny<Player>()), times);
        }

        /// <summary>
        /// Verifies that tournament is deleted required number of times.
        /// </summary>
        /// <param name="times">Number of times tournament must be deleted.</param>
        private void VerifyDelete(Times times)
        {
            this._playerServiceMock.Verify(ts => ts.Delete(It.IsAny<int>()), times);
        }

        /// <summary>
        /// Verifies that redirect to specified action takes place.
        /// </summary>
        /// <param name="actionName">Name of the action where we are supposed to be redirected.</param>
        /// <param name="result">Actual redirection result.</param>
        private void VerifyRedirect(string actionName, RedirectToRouteResult result)
        {
            Assert.AreEqual(actionName, result.RouteValues[ROUTE_VALUES_KEY]);
        }
    }
}
