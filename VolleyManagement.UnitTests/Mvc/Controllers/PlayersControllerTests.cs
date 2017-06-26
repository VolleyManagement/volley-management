namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Contracts;
    using Contracts.Authorization;
    using Contracts.Exceptions;
    using Domain.PlayersAggregate;
    using Domain.RolesAggregate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using Services.PlayerService;
    using UI.Areas.Mvc.Controllers;
    using UI.Areas.Mvc.ViewModels.Players;
    using ViewModels;

    /// <summary>
    /// Tests for MVC PlayersController class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class PlayersControllerTests
    {
        private const int TEST_PLAYER_ID = 1;
        private const int TEST_USER_ID = 1;
        private const int USER_INVALID_ID = -1;
        private const int NON_EXISTENT_PAGE_NUMBER = -1;
        private const string INDEX_ACTION_NAME = "Index";
        private const string ROUTE_VALUES_KEY = "action";
        private const string ASSERT_FAIL_VIEW_MODEL_MESSAGE = "View model must be returned to user.";
        private const string ASSERT_FAIL_JSON_RESULT_MESSAGE = "Json result must be returned to user.";
        private const string PLAYER_NAME_TO_SEARCH = "Player Name";
        private const string LINK_SUCCESSFULL_MESSAGE = "After admin approval you will be linked with";
        private const string LINK_ERROR_MESSAGE = "Can't find User Id";

        private readonly Mock<IPlayerService> _playerServiceMock = new Mock<IPlayerService>();
        private readonly Mock<IAuthorizationService> _authServiceMock = new Mock<IAuthorizationService>();
        private readonly Mock<HttpContextBase> _httpContextMock = new Mock<HttpContextBase>();
        private readonly Mock<HttpRequestBase> _httpRequestMock = new Mock<HttpRequestBase>();
        private readonly Mock<ICurrentUserService> _currentUserServiceMock = new Mock<ICurrentUserService>();
        private readonly Mock<IRequestService> _requestServiceMock = new Mock<IRequestService>();

        private readonly List<AuthOperation> _allowedOperationsIndex = new List<AuthOperation>
                {
                    AuthOperations.Players.Create,
                    AuthOperations.Players.Edit,
                    AuthOperations.Players.Delete
                };

        private readonly AuthOperation _allowedOperationDetails = AuthOperations.Players.Edit;

        private IKernel _kernel;
        private PlayersController _sut;

        /// <summary>
        /// Initializes test data
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IPlayerService>().ToConstant(_playerServiceMock.Object);
            _kernel.Bind<IAuthorizationService>().ToConstant(_authServiceMock.Object);
            _kernel.Bind<ICurrentUserService>().ToConstant(_currentUserServiceMock.Object);
            _kernel.Bind<IRequestService>().ToConstant(_requestServiceMock.Object);
            _httpContextMock.SetupGet(c => c.Request).Returns(_httpRequestMock.Object);
            _sut = _kernel.Get<PlayersController>();
        }

        /// <summary>
        /// Test for Delete method (POST action). Player with specified identifier exists.
        /// Player is deleted and JsonResult is returned.
        /// </summary>
        [TestMethod]
        public void DeletePostAction_ExistingPlayer_PlayerIsDeleted()
        {
            // Act
            var result = _sut.Delete(TEST_PLAYER_ID);

            // Assert
            VerifyDelete(TEST_PLAYER_ID, Times.Once());
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
            SetupDeleteThrowsMissingEntityException();

            // Act
            var result = _sut.Delete(TEST_PLAYER_ID);

            // Assert
            VerifyDelete(TEST_PLAYER_ID, Times.Once());
            Assert.IsNotNull(result, ASSERT_FAIL_JSON_RESULT_MESSAGE);
        }

        /// <summary>
        /// Test for Delete method (POST action). Player id is valid, but exception
        /// is thrown during deleting. JsonResult is returned.
        /// </summary>
        [TestMethod]
        public void DeletePostAction_ValidPlayerIdWithValidationException_JsonResultIsReturned()
        {
            // Arrange
            SetupDeleteThrowsValidationException();

            // Act
            var result = _sut.Delete(TEST_PLAYER_ID);

            // Assert
            VerifyDelete(TEST_PLAYER_ID, Times.Once());
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
            SetupGetAll(testData);
            SetupControllerContext();

            // Act
            var actual = TestExtensions.GetModel<PlayersListReferrerViewModel>(_sut.Index(null, string.Empty)).Model.List;

            // Assert
            CollectionAssert.AreEqual(expected, actual, new PlayerNameViewModelComparer());
            VerifyGetAllowedOperations(_allowedOperationsIndex, Times.Once());
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
            SetupGetAll(testData);
            SetupRequestRawUrl("/Players");
            SetupControllerContext();

            // Act
            var actual = TestExtensions.GetModel<PlayersListReferrerViewModel>(_sut.Index(null, PLAYER_NAME_TO_SEARCH));
            var playersList = actual.Model.List;

            // Assert
            CollectionAssert.AreEqual(expected, playersList, new PlayerNameViewModelComparer());
            VerifyGetAllowedOperations(_allowedOperationsIndex, Times.Once());
            Assert.AreEqual(actual.Referer, _sut.Request.RawUrl);
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
            SetupGetAll(testData);

            // Act
            var result = _sut.Index(NON_EXISTENT_PAGE_NUMBER) as RedirectToRouteResult;

            // Assert
            VerifyRedirect(INDEX_ACTION_NAME, result);
            VerifyGetAllowedOperations(_allowedOperationsIndex, Times.Never());
        }

        /// <summary>
        /// Test for Details method. Player with specified identifier does not exist. HttpNotFoundResult is returned.
        /// </summary>
        [TestMethod]
        public void Details_NonExistentPlayer_HttpNotFoundResultIsReturned()
        {
            // Arrange
            SetupGet(TEST_PLAYER_ID, null);

            // Act
            var result = _sut.Details(TEST_PLAYER_ID);

            // Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
            VerifyGetAllowedOperation(_allowedOperationDetails, Times.Never());
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
            SetupGet(TEST_PLAYER_ID, testData);

            // Act
            var actual = TestExtensions.GetModel<PlayerRefererViewModel>(_sut.Details(TEST_PLAYER_ID));

            // Assert
            TestHelper.AreEqual<PlayerViewModel>(expected, actual.Model, new PlayerViewModelComparer());
            VerifyGetAllowedOperation(_allowedOperationDetails, Times.Once());
        }

        [TestMethod]
        public void LinkWithUser_UserExists_SuccessfullMessageIsReturned()
        {
            // Arrange
            string expected = LINK_SUCCESSFULL_MESSAGE;
            MockCurrenUserService(TEST_USER_ID);
            MockRequestService(TEST_USER_ID, TEST_PLAYER_ID);
            var sut = _kernel.Get<PlayersController>();

            // Act
            string actual = sut.LinkWithUser(TEST_PLAYER_ID);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void LinkWithUser_UserNotFound_ErrorMessageIsReturned()
        {
            // Arrange
            string expected = LINK_ERROR_MESSAGE;
            MockCurrenUserService(USER_INVALID_ID);
            MockRequestService(USER_INVALID_ID, TEST_PLAYER_ID);
            var sut = _kernel.Get<PlayersController>();

            // Act
            string actual = sut.LinkWithUser(TEST_PLAYER_ID);

            // Assert
            Assert.AreEqual(expected, actual);
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
            _sut.ModelState.AddModelError(string.Empty, string.Empty);

            // Act
            var result = TestExtensions.GetModel<PlayerViewModel>(_sut.Create(testData));

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
            var result = _sut.Create(testData) as RedirectToRouteResult;

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
            SetupCreateThrowsArgumentException();

            // Act
            var result = TestExtensions.GetModel<PlayerViewModel>(_sut.Create(testData));

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
            SetupCreateThrowsValidationException();

            // Act
            var result = TestExtensions.GetModel<PlayerViewModel>(_sut.Create(testData));

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
            SetupGet(TEST_PLAYER_ID, null);

            // Act
            var result = _sut.Edit(TEST_PLAYER_ID);

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
            SetupGet(TEST_PLAYER_ID, testData);

            // Act
            var actual = TestExtensions.GetModel<PlayerViewModel>(_sut.Edit(TEST_PLAYER_ID));

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
            var result = _sut.Edit(testData) as RedirectToRouteResult;

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
            SetupEditThrowsMissingEntityException();

            // Act
            var result = TestExtensions.GetModel<PlayerViewModel>(_sut.Edit(testData));

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
            SetupEditThrowsValidationException();

            // Act
            var result = TestExtensions.GetModel<PlayerViewModel>(_sut.Edit(testData));

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
            _sut.ModelState.AddModelError(string.Empty, string.Empty);

            // Act
            var result = TestExtensions.GetModel<PlayerViewModel>(_sut.Edit(testData));

            // Assert
            VerifyEdit(Times.Never());
            Assert.IsNotNull(result, ASSERT_FAIL_VIEW_MODEL_MESSAGE);
        }

        private List<Player> MakeTestPlayers()
        {
            return new PlayerServiceTestFixture().TestPlayers().Build();
        }

        private List<PlayerNameViewModel> MakePlayerNameViewModels(List<Player> players)
        {
            return players.Select(p => new PlayerNameViewModel { Id = p.Id, FullName = p.LastName + " " + p.FirstName }).ToList();
        }

        private List<PlayerNameViewModel> GetPlayerNameViewModelsWithPlayerName(List<PlayerNameViewModel> players, string name)
        {
            return players.Where(p => p.FullName.Contains(name)).ToList();
        }

        private Player MakeTestPlayer(int playerId)
        {
            return new PlayerBuilder().WithId(playerId).Build();
        }

        private PlayerViewModel MakeTestPlayerViewModel()
        {
            return new PlayerMvcViewModelBuilder().Build();
        }

        private PlayerViewModel MakeTestPlayerViewModel(int playerId)
        {
            return new PlayerMvcViewModelBuilder().WithId(playerId).Build();
        }

        private void SetupGetAll(List<Player> teams)
        {
            _playerServiceMock.Setup(ps => ps.Get()).Returns(teams.AsQueryable());
        }

        private void SetupGet(int playerId, Player player)
        {
            _playerServiceMock.Setup(tr => tr.Get(playerId)).Returns(player);
        }

        private void SetupCreateThrowsArgumentException()
        {
            _playerServiceMock.Setup(ts => ts.Create(It.IsAny<Player>()))
                .Throws(new ArgumentException(string.Empty, string.Empty));
        }

        private void SetupCreateThrowsValidationException()
        {
            _playerServiceMock.Setup(ts => ts.Create(It.IsAny<Player>()))
                .Throws(new ValidationException(string.Empty));
        }

        private void SetupEditThrowsMissingEntityException()
        {
            _playerServiceMock.Setup(ts => ts.Edit(It.IsAny<Player>()))
                .Throws(new MissingEntityException(string.Empty));
        }

        private void SetupEditThrowsValidationException()
        {
            _playerServiceMock.Setup(ts => ts.Edit(It.IsAny<Player>()))
                .Throws(new ValidationException(string.Empty));
        }

        private void SetupDeleteThrowsMissingEntityException()
        {
            _playerServiceMock.Setup(ts => ts.Delete(It.IsAny<int>()))
                .Throws(new MissingEntityException(string.Empty));
        }

        private void SetupDeleteThrowsValidationException()
        {
            _playerServiceMock.Setup(ts => ts.Delete(It.IsAny<int>()))
                .Throws(new ValidationException(string.Empty));
        }

        private void SetupControllerContext()
        {
            _sut.ControllerContext = new ControllerContext(_httpContextMock.Object, new RouteData(), _sut);
        }

        private void SetupRequestRawUrl(string rawUrl)
        {
            _httpRequestMock.Setup(x => x.RawUrl).Returns(rawUrl);
        }

        private void VerifyCreate(Times times)
        {
            _playerServiceMock.Verify(ps => ps.Create(It.IsAny<Player>()), times);
        }

        private void VerifyEdit(Times times)
        {
            _playerServiceMock.Verify(ts => ts.Edit(It.IsAny<Player>()), times);
        }

        private void VerifyDelete(int playerId, Times times)
        {
            _playerServiceMock.Verify(ts => ts.Delete(It.Is<int>(id => id == playerId)), times);
        }

        private void VerifyRedirect(string actionName, RedirectToRouteResult result)
        {
            Assert.AreEqual(actionName, result.RouteValues[ROUTE_VALUES_KEY]);
        }

        private void VerifyGetAllowedOperations(List<AuthOperation> allowedOperations, Times times)
        {
            _authServiceMock.Verify(tr => tr.GetAllowedOperations(allowedOperations), times);
        }

        private void VerifyGetAllowedOperation(AuthOperation allowedOperation, Times times)
        {
            _authServiceMock.Verify(tr => tr.GetAllowedOperations(allowedOperation), times);
        }

        private void MockCurrenUserService(int userId)
        {
            _currentUserServiceMock.Setup(tr => tr.GetCurrentUserId()).Returns(userId);
        }

        private void MockRequestService(int userId, int playerId)
        {
            _requestServiceMock.Setup(tr => tr.Create(userId, playerId));
        }
    }
}