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
    using Moq;
    using Contracts.Authorization;
    using Contracts.Exceptions;
    using Domain.PlayersAggregate;
    using Domain.RolesAggregate;
    using FluentAssertions;
    using UI.Areas.Mvc.Controllers;
    using UI.Areas.Mvc.ViewModels.Players;
    using ViewModels;
    using Services.PlayerService;
    using Xunit;

    /// <summary>
    /// Tests for MVC PlayersController class.
    /// </summary>
    [ExcludeFromCodeCoverage]
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
        private const string PLAYER_NAME_TO_SEARCH = "FirstNameA";
        private const string LINK_SUCCESSFULL_MESSAGE = "After admin approval you will be linked with";
        private const string LINK_ERROR_MESSAGE = "Can't find User Id";

        private readonly List<AuthOperation> _allowedOperationsIndex = new List<AuthOperation>
                {
                    AuthOperations.Players.Create,
                    AuthOperations.Players.Edit,
                    AuthOperations.Players.Delete
                };

        private readonly AuthOperation _allowedOperationDetails = AuthOperations.Players.Edit;

        private Mock<IPlayerService> _playerServiceMock;
        private Mock<IAuthorizationService> _authServiceMock;
        private Mock<HttpContextBase> _httpContextMock;
        private Mock<HttpRequestBase> _httpRequestMock;
        private Mock<ICurrentUserService> _currentUserServiceMock;
        private Mock<IRequestService> _requestServiceMock;

        /// <summary>
        /// Initializes test data
        /// </summary>
        public PlayersControllerTests()
        {
            _playerServiceMock = new Mock<IPlayerService>();
            _authServiceMock = new Mock<IAuthorizationService>();
            _httpContextMock = new Mock<HttpContextBase>();
            _httpRequestMock = new Mock<HttpRequestBase>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _requestServiceMock = new Mock<IRequestService>();
        }

        /// <summary>
        /// Test for Delete method (POST action). Player with specified identifier exists.
        /// Player is deleted and JsonResult is returned.
        /// </summary>
        [Fact]
        public void DeletePostAction_ExistingPlayer_PlayerIsDeleted()
        {
            // Act
            var sut = BuildSUT();
            var result = sut.Delete(TEST_PLAYER_ID);

            // Assert
            VerifyDelete(TEST_PLAYER_ID, Times.Once());
            result.Should().NotBeNull(ASSERT_FAIL_JSON_RESULT_MESSAGE);
        }

        /// <summary>
        /// Test for Delete method (POST action). Player with specified identifier does not exist.
        /// Exception is thrown during player removal and JsonResult is returned.
        /// </summary>
        [Fact]
        public void DeletePostAction_NonExistentPlayer_JsonResultIsReturned()
        {
            // Arrange
            SetupDeleteThrowsMissingEntityException();

            var sut = BuildSUT();

            // Act
            var result = sut.Delete(TEST_PLAYER_ID);

            // Assert
            VerifyDelete(TEST_PLAYER_ID, Times.Once());
            result.Should().NotBeNull(ASSERT_FAIL_JSON_RESULT_MESSAGE);
        }

        /// <summary>
        /// Test for Delete method (POST action). Player id is valid, but exception
        /// is thrown during deleting. JsonResult is returned.
        /// </summary>
        [Fact]
        public void DeletePostAction_ValidPlayerIdWithValidationException_JsonResultIsReturned()
        {
            // Arrange
            SetupDeleteThrowsValidationException();

            var sut = BuildSUT();

            // Act
            var result = sut.Delete(TEST_PLAYER_ID);

            // Assert
            VerifyDelete(TEST_PLAYER_ID, Times.Once());
            result.Should().NotBeNull(ASSERT_FAIL_JSON_RESULT_MESSAGE);
        }

        /// <summary>
        /// Test for Index method. Players from specified existing page are requested and no search text is specified.
        /// Players from specified page are returned.
        /// </summary>
        [Fact]
        public void Index_GetPlayersFromExistingPageNoSearchText_PlayersAreReturned()
        {
            // Arrange
            var testData = MakeTestPlayers();
            var expected = MakePlayerNameViewModels(testData);
            SetupGetAll(testData);

            var sut = BuildSUT();
            SetupControllerContext(sut);

            // Act
            var actual = TestExtensions.GetModel<PlayersListReferrerViewModel>(
                sut.Index(null, string.Empty)).Model.List;

            // Assert
            TestHelper.AreEqual(expected, actual, new PlayerNameViewModelComparer());
            VerifyGetAllowedOperations(_allowedOperationsIndex, Times.Once());
        }

        /// <summary>
        /// Test for Index method. Players from specified existing page are requested and search text is specified.
        /// Players from specified page are returned.
        /// </summary>
        [Fact]
        public void Index_GetPlayersFromExistingPageWithSearchText_PlayersAreReturned()
        {
            // Arrange
            var testData = MakeTestPlayers();
            var expected = GetPlayerNameViewModelsWithPlayerName(MakePlayerNameViewModels(testData), PLAYER_NAME_TO_SEARCH);
            SetupGetAll(testData);
            SetupRequestRawUrl("/Players");

            var sut = BuildSUT();
            SetupControllerContext(sut);

            // Act
            var actual = TestExtensions.GetModel<PlayersListReferrerViewModel>(
                sut.Index(null, PLAYER_NAME_TO_SEARCH));

            var playersList = actual.Model.List;

            // Assert
            TestHelper.AreEqual(expected, playersList, new PlayerNameViewModelComparer());
            VerifyGetAllowedOperations(_allowedOperationsIndex, Times.Once());
            Assert.Equal(actual.Referer, sut.Request.RawUrl);
        }

        /// <summary>
        /// Test for Index method. Players from specified non-existent page are requested.
        /// Exception is thrown during players retrieval and user is redirected to the Index page.
        /// </summary>
        [Fact]
        public void Index_GetPlayersFromNonExistentPage_ExceptionIsThrown()
        {
            // Arrange
            var testData = MakeTestPlayers();
            SetupGetAll(testData);

            var sut = BuildSUT();

            // Act
            var result = sut.Index(NON_EXISTENT_PAGE_NUMBER) as RedirectToRouteResult;

            // Assert
            VerifyRedirect(INDEX_ACTION_NAME, result);
            VerifyGetAllowedOperations(_allowedOperationsIndex, Times.Never());
        }

        /// <summary>
        /// Test for Details method. Player with specified identifier does not exist. HttpNotFoundResult is returned.
        /// </summary>
        [Fact]
        public void Details_NonExistentPlayer_HttpNotFoundResultIsReturned()
        {
            // Arrange
            SetupGet(TEST_PLAYER_ID, null);

            var sut = BuildSUT();

            // Act
            var result = sut.Details(TEST_PLAYER_ID);

            // Assert
            Assert.IsType<HttpNotFoundResult>(result);
            VerifyGetAllowedOperation(_allowedOperationDetails, Times.Never());
        }

        /// <summary>
        /// Test for Details method. Player with specified identifier exists. View model of Player is returned.
        /// </summary>
        [Fact]
        public void Details_ExistingPlayer_PlayerViewModelIsReturned()
        {
            // Arrange
            var testData = MakeTestPlayer(TEST_PLAYER_ID);
            var expected = MakeTestPlayerViewModel(TEST_PLAYER_ID);
            SetupGet(TEST_PLAYER_ID, testData);

            var sut = BuildSUT();

            // Act
            var actual = TestExtensions.GetModel<PlayerRefererViewModel>(sut.Details(TEST_PLAYER_ID));

            // Assert
            TestHelper.AreEqual<PlayerViewModel>(expected, actual.Model, new PlayerViewModelComparer());
            VerifyGetAllowedOperation(_allowedOperationDetails, Times.Once());
        }

        [Fact]
        public void LinkWithUser_UserExists_SuccessfullMessageIsReturned()
        {
            // Arrange
            var expected = LINK_SUCCESSFULL_MESSAGE;
            MockCurrenUserService(TEST_USER_ID);
            MockRequestService(TEST_USER_ID, TEST_PLAYER_ID);

            var sut = BuildSUT();

            // Act
            var actual = sut.LinkWithUser(TEST_PLAYER_ID);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LinkWithUser_UserNotFound_ErrorMessageIsReturned()
        {
            // Arrange
            var expected = LINK_ERROR_MESSAGE;
            MockCurrenUserService(USER_INVALID_ID);
            MockRequestService(USER_INVALID_ID, TEST_PLAYER_ID);

            var sut = BuildSUT();

            // Act
            var actual = sut.LinkWithUser(TEST_PLAYER_ID);

            // Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Test for Create player action (GET)
        /// </summary>
        [Fact]
        public void CreateGetAction_GetView_ReturnsViewWithDefaultData()
        {
            // Arrange
            var sut = BuildSUT();
            var expected = new PlayerViewModel();

            // Act
            var actual = TestExtensions.GetModel<PlayerViewModel>(sut.Create());

            // Assert
            TestHelper.AreEqual<PlayerViewModel>(expected, actual, new PlayerViewModelComparer());
        }

        /// <summary>
        /// Test for Create method (POST action). Player view model is not valid.
        /// Player is not created and player view model is returned.
        /// </summary>
        [Fact]
        public void CreatePostAction_InvalidPlayerViewModel_PlayerViewModelIsReturned()
        {
            // Arrange
            var testData = MakeTestPlayerViewModel();
            var sut = BuildSUT();

            sut.ModelState.AddModelError(string.Empty, string.Empty);

            // Act
            var result = TestExtensions.GetModel<PlayerViewModel>(sut.Create(testData));

            // Assert
            VerifyCreate(Times.Never());
            result.Should().NotBeNull(ASSERT_FAIL_VIEW_MODEL_MESSAGE);
        }

        /// <summary>
        /// Test for Create method (POST action). Player view model is valid and no exception is thrown during creation.
        /// Player is created successfully and user is redirected to the Index page.
        /// </summary>
        [Fact]
        public void CreatePostAction_ValidPlayerViewModelNoException_PlayerIsCreated()
        {
            // Arrange
            var testData = MakeTestPlayerViewModel();
            var player = MakeTestPlayer(1);
            var sut = BuildSUT();
            SetupCreate(player);

            // Act
            var result = sut.Create(testData) as RedirectToRouteResult;

            // Assert
            VerifyCreate(Times.Once());
            VerifyRedirect(INDEX_ACTION_NAME, result);
        }

        /// <summary>
        /// Test for Create method (POST action). Player view model is valid, but exception is thrown during creation.
        /// Player view model is returned.
        /// </summary>
        [Fact]
        public void CreatePostAction_ValidPlayerViewModelWithArgumentException_PlayerViewModelIsReturned()
        {
            // Arrange
            var testData = MakeTestPlayerViewModel();
            SetupCreateThrowsArgumentException();

            var sut = BuildSUT();

            // Act
            var result = TestExtensions.GetModel<PlayerViewModel>(sut.Create(testData));

            // Assert
            VerifyCreate(Times.Once());
            result.Should().NotBeNull(ASSERT_FAIL_VIEW_MODEL_MESSAGE);
        }

        /// <summary>
        /// Test for Create method (POST action). Player view model is valid, but exception is thrown during creation.
        /// Player view model is returned.
        /// </summary>
        [Fact]
        public void CreatePostAction_ValidPlayerViewModelWithValidationException_PlayerViewModelIsReturned()
        {
            // Arrange
            var testData = MakeTestPlayerViewModel();
            SetupCreateThrowsValidationException();

            var sut = BuildSUT();

            // Act
            var result = TestExtensions.GetModel<PlayerViewModel>(sut.Create(testData));

            // Assert
            VerifyCreate(Times.Once());
            result.Should().NotBeNull(ASSERT_FAIL_VIEW_MODEL_MESSAGE);
        }

        /// <summary>
        /// Test for Edit method (GET action). Player with specified identifier does not exist. HttpNotFoundResult is returned.
        /// </summary>
        [Fact]
        public void EditGetAction_NonExistentPlayer_HttpNotFoundResultIsReturned()
        {
            // Arrange
            SetupGet(TEST_PLAYER_ID, null);
            var sut = BuildSUT();

            // Act
            var result = sut.Edit(TEST_PLAYER_ID);

            // Assert
            Assert.IsType<HttpNotFoundResult>(result);
        }

        /// <summary>
        /// Test for Edit method (GET action). Player with specified identifier exists. View model of Player is returned.
        /// </summary>
        [Fact]
        public void EditGetAction_ExistingPlayer_PlayerViewModelIsReturned()
        {
            // Arrange
            var testData = MakeTestPlayer(TEST_PLAYER_ID);
            var expected = MakeTestPlayerViewModel(TEST_PLAYER_ID);
            SetupGet(TEST_PLAYER_ID, testData);

            var sut = BuildSUT();

            // Act
            var actual = TestExtensions.GetModel<PlayerViewModel>(sut.Edit(TEST_PLAYER_ID));

            // Assert
            TestHelper.AreEqual(expected, actual, new PlayerViewModelComparer());
        }

        /// <summary>
        /// Test for Edit method (POST action). Player view model is valid and no exception is thrown during editing.
        /// Player is updated successfully and user is redirected to the Index page.
        /// </summary>
        [Fact]
        public void EditPostAction_ValidPlayerViewModelNoException_PlayerIsUpdated()
        {
            // Arrange
            var testData = MakeTestPlayerViewModel();

            var sut = BuildSUT();

            // Act
            var result = sut.Edit(testData) as RedirectToRouteResult;

            // Assert
            VerifyEdit(Times.Once());
            VerifyRedirect(INDEX_ACTION_NAME, result);
        }

        /// <summary>
        /// Test for Edit method (POST action). Player view model is valid, but exception is thrown during editing.
        /// Player view model is returned.
        /// </summary>
        [Fact]
        public void EditPostAction_ValidPlayerViewModelWithMissingEntityException_PlayerViewModelIsReturned()
        {
            // Arrange
            var testData = MakeTestPlayerViewModel();
            SetupEditThrowsMissingEntityException();

            var sut = BuildSUT();

            // Act
            var result = TestExtensions.GetModel<PlayerViewModel>(sut.Edit(testData));

            // Assert
            VerifyEdit(Times.Once());
            result.Should().NotBeNull(ASSERT_FAIL_VIEW_MODEL_MESSAGE);
        }

        /// <summary>
        /// Test for Edit method (POST action). Player view model is valid, but exception is thrown during editing.
        /// Player view model is returned.
        /// </summary>
        [Fact]
        public void EditPostAction_ValidPlayerViewModelWithValidationException_PlayerViewModelIsReturned()
        {
            // Arrange
            var testData = MakeTestPlayerViewModel();
            SetupEditThrowsValidationException();

            var sut = BuildSUT();

            // Act
            var result = TestExtensions.GetModel<PlayerViewModel>(sut.Edit(testData));

            // Assert
            VerifyEdit(Times.Once());
            result.Should().NotBeNull(ASSERT_FAIL_VIEW_MODEL_MESSAGE);

        }

        /// <summary>
        /// Test for Edit method (POST action). Player view model is not valid.
        /// Player is not updated and player view model is returned.
        /// </summary>
        [Fact]
        public void EditPostAction_InvalidPlayerViewModel_PlayerViewModelIsReturned()
        {
            // Arrange
            var testData = MakeTestPlayerViewModel();
            var sut = BuildSUT();
            sut.ModelState.AddModelError(string.Empty, string.Empty);

            // Act
            var result = TestExtensions.GetModel<PlayerViewModel>(sut.Edit(testData));

            // Assert
            VerifyEdit(Times.Never());
            result.Should().NotBeNull(ASSERT_FAIL_VIEW_MODEL_MESSAGE);
        }

        private PlayersController BuildSUT()
        {
            return new PlayersController(
                _playerServiceMock.Object,
                _authServiceMock.Object,
                _currentUserServiceMock.Object,
                _requestServiceMock.Object);
        }

        private List<Player> MakeTestPlayers()
        {
            return new PlayerServiceTestFixture().TestPlayers().Build();
        }

        private ICollection<PlayerNameViewModel> MakePlayerNameViewModels(List<Player> players)
        {
            return players.Select(p => new PlayerNameViewModel { Id = p.Id, FirstName = p.FirstName, LastName = p.LastName }).ToList();
        }

        private List<PlayerNameViewModel> GetPlayerNameViewModelsWithPlayerName(ICollection<PlayerNameViewModel> players, string name)
        {
            return players.Where(p => p.FirstName.Contains(name) || p.LastName.Contains(name)).ToList();
        }

        private Player MakeTestPlayer(int playerId)
        {
            return new PlayerBuilder(playerId)
                .WithBirthYear(1983)
                .WithHeight(186)
                .WithWeight(95)
                .Build();
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

        private void SetupCreate(Player player)
        {
            _playerServiceMock.Setup(tr => tr.Create(It.IsAny<CreatePlayerDto>())).Returns(player);
        }

        private void SetupCreateThrowsArgumentException()
        {
            _playerServiceMock.Setup(ts => ts.Create(It.IsAny<CreatePlayerDto>()))
                .Throws(new ArgumentException(string.Empty, string.Empty));
        }

        private void SetupCreateThrowsValidationException()
        {
            _playerServiceMock.Setup(ts => ts.Create(It.IsAny<CreatePlayerDto>()))
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

        private void SetupControllerContext(PlayersController sut)
        {
            _httpContextMock.SetupGet(x => x.Request).Returns(_httpRequestMock.Object);
            sut.ControllerContext = new ControllerContext(_httpContextMock.Object, new RouteData(), sut);
        }

        private void SetupRequestRawUrl(string rawUrl)
        {
            _httpRequestMock.Setup(x => x.RawUrl).Returns(rawUrl);
        }

        private void VerifyCreate(Times times)
        {
            _playerServiceMock.Verify(ps => ps.Create(It.IsAny<CreatePlayerDto>()), times);
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
            Assert.Equal(actionName, result.RouteValues[ROUTE_VALUES_KEY]);
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