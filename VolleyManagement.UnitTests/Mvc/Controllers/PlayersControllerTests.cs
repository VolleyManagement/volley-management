namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
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
        private const string TEST_CONTROLLER_NAME = "TestController";
        private const string INDEX_ACTION_NAME = "Index";
        private const string ROUTE_VALUES_KEY = "action";
        private const string ASSERT_FAIL_VIEW_MODEL_MESSAGE = "View model must be returned to user.";
        private const string ASSERT_FAIL_JSON_RESULT_MESSAGE = "Json result must be returned to user.";
        private const int TEST_PLAYER_ID = 1;
        private const int NON_EXISTENT_PAGE_NUMBER = -1;
        private const string PLAYER_NAME_TO_SEARCH = "Player Name";

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
        /// Test for Delete method (POST action). Player with specified identifier exists.
        /// Player is deleted and JsonResult is returned.
        /// </summary>
        [TestMethod]
        public void DeletePostAction_ExistingPlayer_PlayerIsDeleted()
        {
            // Act
            var result = this._sut.Delete(TEST_PLAYER_ID);

            // Assert
            VerifyDelete(Times.Once());
            Assert.IsNotNull(result, ASSERT_FAIL_JSON_RESULT_MESSAGE);
        }

        /// <summary>
        /// Test for Delete method (POST action). Player with specified identifier does not exist.
        /// Exception is thrown during player removal and JsonResult is returned.
        /// </summary>
        [TestMethod]
        public void DeletePostAction_NonExistentPlayer_JsonResultIsReturned()
        {
            // Arrange
            MockSetupDeleteMissingEntityException();

            // Act
            var result = this._sut.Delete(TEST_PLAYER_ID);

            // Assert
            VerifyDelete(Times.Once());
            Assert.IsNotNull(result, ASSERT_FAIL_JSON_RESULT_MESSAGE);
        }

        /// <summary>
        /// Test for Index method. Players from specified existing page are requested and no search text is specified.
        /// Players from specified page are returned.
        /// </summary>
        [TestMethod]
        public void Index_GetPlayersFromExistingPageNoSearchText_PlayersAreReturned()
        {
            // Arrange
            var testData = MakeTestPlayers();
            var expected = MakePlayerNameViewModels(testData);
            MockSetupGetAll(testData);

            // Act
            var actual = TestExtensions.GetModel<PlayersListViewModel>(this._sut.Index(null, string.Empty)).List;

            // Assert
            CollectionAssert.AreEqual(expected, actual, new PlayerNameViewModelComparer());
        }

        /// <summary>
        /// Test for Index method. Players from specified existing page are requested and search text is specified.
        /// Players from specified page are returned.
        /// </summary>
        [TestMethod]
        public void Index_GetPlayersFromExistingPageWithSearchText_PlayersAreReturned()
        {
            // Arrange
            var testData = MakeTestPlayers();
            var expected = GetPlayerNameViewModelsWithPlayerName(MakePlayerNameViewModels(testData), PLAYER_NAME_TO_SEARCH);
            MockSetupGetAll(testData);

            // Act
            var actual = TestExtensions.GetModel<PlayersListViewModel>(this._sut.Index(null, PLAYER_NAME_TO_SEARCH)).List;

            // Assert
            CollectionAssert.AreEqual(expected, actual, new PlayerNameViewModelComparer());
        }

        /// <summary>
        /// Test for Index method. Players from specified non-existent page are requested.
        /// Exception is thrown during players retrieval and user is redirected to the Index page.
        /// </summary>
        [TestMethod]
        public void Index_GetPlayersFromNonExistentPage_ExceptionIsThrown()
        {
            // Arrange
            var testData = MakeTestPlayers();
            MockSetupGetAll(testData);

            // Act
            var result = this._sut.Index(NON_EXISTENT_PAGE_NUMBER) as RedirectToRouteResult;

            // Assert
            VerifyRedirect(INDEX_ACTION_NAME, result);
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
        /// Test for Edit method (GET action). Player with specified identifier does not exist. HttpNotFoundResult is returned.
        /// </summary>
        [TestMethod]
        public void EditGetAction_NonExistentPlayer_HttpNotFoundResultIsReturned()
        {
            // Arrange
            MockSetupGet(null);

            // Act
            var result = this._sut.Edit(TEST_PLAYER_ID);

            // Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        /// <summary>
        /// Test for Edit method (GET action). Player with specified identifier exists. View model of Player is returned.
        /// </summary>
        [TestMethod]
        public void EditGetAction_ExistingPlayer_PlayerViewModelIsReturned()
        {
            // Arrange
            var testData = MakeTestPlayer(TEST_PLAYER_ID);
            var expected = MakeTestPlayerViewModel(TEST_PLAYER_ID);
            MockSetupGet(testData);

            // Act
            var actual = TestExtensions.GetModel<PlayerViewModel>(this._sut.Edit(TEST_PLAYER_ID));

            // Assert
            TestHelper.AreEqual<PlayerViewModel>(expected, actual, new PlayerViewModelComparer());
        }

        /// <summary>
        /// Test for Edit method (POST action). Player view model is valid and no exception is thrown during editing.
        /// Player is updated successfully and user is redirected to the Index page.
        /// </summary>
        [TestMethod]
        public void EditPostAction_ValidPlayerViewModelNoException_PlayerIsUpdated()
        {
            // Arrange
            var testData = MakeTestPlayerViewModel();

            // Act
            var result = this._sut.Edit(testData) as RedirectToRouteResult;

            // Assert
            VerifyEdit(Times.Once());
            VerifyRedirect(INDEX_ACTION_NAME, result);
        }

        /// <summary>
        /// Test for Edit method (POST action). Player view model is valid, but exception is thrown during editing.
        /// Player view model is returned.
        /// </summary>
        [TestMethod]
        public void EditPostAction_ValidPlayerViewModelWithMissingEntityException_PlayerViewModelIsReturned()
        {
            // Arrange
            var testData = MakeTestPlayerViewModel();
            MockSetupEditMissingEntityException();

            // Act
            var result = TestExtensions.GetModel<PlayerViewModel>(this._sut.Edit(testData));

            // Assert
            VerifyEdit(Times.Once());
            Assert.IsNotNull(result, ASSERT_FAIL_VIEW_MODEL_MESSAGE);
        }

        /// <summary>
        /// Test for Edit method (POST action). Player view model is valid, but exception is thrown during editing.
        /// Player view model is returned.
        /// </summary>
        [TestMethod]
        public void EditPostAction_ValidPlayerViewModelWithValidationException_PlayerViewModelIsReturned()
        {
            // Arrange
            var testData = MakeTestPlayerViewModel();
            MockSetupEditValidationException();

            // Act
            var result = TestExtensions.GetModel<PlayerViewModel>(this._sut.Edit(testData));

            // Assert
            VerifyEdit(Times.Once());
            Assert.IsNotNull(result, ASSERT_FAIL_VIEW_MODEL_MESSAGE);
        }

        /// <summary>
        /// Test for Edit method (POST action). Player view model is not valid.
        /// Player is not updated and player view model is returned.
        /// </summary>
        [TestMethod]
        public void EditPostAction_InvalidPlayerViewModel_PlayerViewModelIsReturned()
        {
            // Arrange
            var testData = MakeTestPlayerViewModel();
            this._sut.ModelState.AddModelError(string.Empty, string.Empty);

            // Act
            var result = TestExtensions.GetModel<PlayerViewModel>(this._sut.Edit(testData));

            // Assert
            VerifyEdit(Times.Never());
            Assert.IsNotNull(result, ASSERT_FAIL_VIEW_MODEL_MESSAGE);
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
        /// Makes players filled with test data.
        /// </summary>
        /// <returns>List of players with test data.</returns>
        private List<Player> MakeTestPlayers()
        {
            return new PlayerServiceTestFixture().TestPlayers().Build();
        }

        /// <summary>
        /// Makes PlayerNameViewModels from Player view models.
        /// </summary>
        /// <param name="players">List of Player view models.</param>
        /// <returns>List of PlayerNameViewModels.</returns>
        private List<PlayerNameViewModel> MakePlayerNameViewModels(List<Player> players)
        {
            return players.Select(p => new PlayerNameViewModel { Id = p.Id, FullName = p.LastName + " " + p.FirstName }).ToList();
        }

        private List<PlayerNameViewModel> GetPlayerNameViewModelsWithPlayerName(List<PlayerNameViewModel> players, string name)
        {
            return players.Where(p => p.FullName.Contains(name)).ToList();
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
        /// Sets up a mock for Get method of Player service to return specified players.
        /// </summary>
        /// <param name="teams">Players that will be returned by Get method of Player service.</param>
        private void MockSetupGetAll(List<Player> teams)
        {
            this._playerServiceMock.Setup(ps => ps.Get()).Returns(teams.AsQueryable());
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

        /// <summary>
        /// Sets up a mock for Edit method of Player service to throw MissingEntityException.
        /// </summary>
        private void MockSetupEditMissingEntityException()
        {
            this._playerServiceMock.Setup(ts => ts.Edit(It.IsAny<Player>()))
                .Throws(new MissingEntityException(string.Empty));
        }

        /// <summary>
        /// Sets up a mock for Edit method of Player service to throw ValidationException.
        /// </summary>
        private void MockSetupEditValidationException()
        {
            this._playerServiceMock.Setup(ts => ts.Edit(It.IsAny<Player>()))
                .Throws(new ValidationException(string.Empty));
        }

        /// <summary>
        /// Sets up a mock for Delete method of Player service to throw MissingEntityException.
        /// </summary>
        private void MockSetupDeleteMissingEntityException()
        {
            this._playerServiceMock.Setup(ts => ts.Delete(It.IsAny<int>()))
                .Throws(new MissingEntityException(string.Empty));
        }

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
