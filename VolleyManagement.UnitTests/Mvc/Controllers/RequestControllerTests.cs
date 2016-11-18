namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Domain.PlayersAggregate;
    using VolleyManagement.Domain.UsersAggregate;
    using VolleyManagement.UI.Areas.Admin.Controllers;
    using VolleyManagement.UnitTests.Services.PlayerService;
    using VolleyManagement.UnitTests.Services.UsersService;

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class RequestControllerTests
    {
        #region Fields

        private readonly Mock<IRequestService> _requestServiceMock = new Mock<IRequestService>();
        private readonly Mock<IUserService> _userServiceMock = new Mock<IUserService>();
        private readonly Mock<IPlayerService> _playerServiceMock = new Mock<IPlayerService>();

        private IKernel _kernel;

        #endregion

        #region Init

        [TestInitialize]
        public void TestInit()
        {
            this._kernel = new StandardKernel();
            this._kernel.Bind<IUserService>().ToConstant(this._userServiceMock.Object);
            this._kernel.Bind<IPlayerService>().ToConstant(this._playerServiceMock.Object);
            this._kernel.Bind<IRequestService>().ToConstant(this._requestServiceMock.Object);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void UserDetails_ExistingUser_UserReturned()
        {
            // Arrange
            var expected = GetUser();
            const int USER_ID = 1;
            SetupUserService(USER_ID);
            var sut = _kernel.Get<RequestController>();

            // Act
            var actionResult = sut.UserDetails(USER_ID);
            var actual = TestExtensions.GetModel<User>(actionResult);

            // Assert
            TestHelper.AreEqual<User>(expected, actual, new UserComparer());
        }

        [TestMethod]
        public void UserDetails_NonExistentUser_HttpNotFoundResultIsReturned()
        {
            // Arrange
            const int USER_ID = 1;
            SetupUserServiceReturnsNullUser(USER_ID);
            var sut = _kernel.Get<RequestController>();

            // Act
            var actionResult = sut.UserDetails(USER_ID);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void PlayerDetails_ExistingPlayer_PlayerReturned()
        {
            // Arrange
            var expected = GetPlayer();
            const int PLAYER_ID = 1;
            SetupPlayerService(PLAYER_ID);
            var sut = _kernel.Get<RequestController>();

            // Act
            var actionResult = sut.PlayerDetails(PLAYER_ID);
            var actual = TestExtensions.GetModel<Player>(actionResult);

            // Assert
            TestHelper.AreEqual<Player>(expected, actual, new PlayerComparer());
        }

        [TestMethod]
        public void PlayerDetails_NonExistentUser_HttpNotFoundResultIsReturned()
        {
            // Arrange
            const int PLAYER_ID = 1;
            SetupPlayerServiceReturnsNullPlayer(PLAYER_ID);
            var sut = _kernel.Get<RequestController>();

            // Act
            var actionResult = sut.PlayerDetails(PLAYER_ID);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void Confirm_AnyRequest_RequestRedirectToIndex()
        {
            // Arrange
            const int REQUEST_ID = 1;
            var sut = _kernel.Get<RequestController>();

            // Act
            var actionResult = sut.Confirm(REQUEST_ID);

            // Assert
            AssertValidRedirectResult(actionResult, "Index");
        }

        [TestMethod]
        public void Confirm_AnyRequest_RequestConfirmed()
        {
            // Arrange
            const int REQUEST_ID = 1;
            var sut = _kernel.Get<RequestController>();

            // Act
            var actionResult = sut.Confirm(REQUEST_ID);

            // Assert
            AssertVerifyConfirm(_requestServiceMock, REQUEST_ID);
        }

        [TestMethod]
        public void Confirm_NonExistentRequest_ThrowsMissingEntityException()
        {
            // Arrange
            const int REQUEST_ID = 1;
            SetupConfirmThrowsMissingEntityException();
            var sut = _kernel.Get<RequestController>();

            // Act
            var actionResult = sut.Confirm(REQUEST_ID);

            // Assert
            Assert.IsNotNull(actionResult, "InvalidOperation");
        }

        [TestMethod]
        public void Decline_AnyRequest_RequestRedirectToIndex()
        {
            // Arrange
            const int REQUEST_ID = 1;
            var sut = _kernel.Get<RequestController>();

            // Act
            var actionResult = sut.Decline(REQUEST_ID);

            // Assert
            AssertValidRedirectResult(actionResult, "Index");
        }

        [TestMethod]
        public void Decline_AnyRequest_RequestDeclined()
        {
            // Arrange
            const int REQUEST_ID = 1;
            var sut = _kernel.Get<RequestController>();

            // Act
            var actionResult = sut.Decline(REQUEST_ID);

            // Assert
            AssertVerifyDecline(_requestServiceMock, REQUEST_ID);
        }

        [TestMethod]
        public void Decline_NonExistentRequest_ThrowsMissingEntityException()
        {
            // Arrange
            const int REQUEST_ID = 1;
            SetupDeclineThrowsMissingEntityException();
            var sut = _kernel.Get<RequestController>();

            // Act
            var actionResult = sut.Decline(REQUEST_ID);

            // Assert
            Assert.IsNotNull(actionResult, "InvalidOperation");
        }

        #endregion

        #region Custom assertions

        private static void AssertValidRedirectResult(ActionResult actionResult, string view)
        {
            var result = (RedirectToRouteResult)actionResult;
            Assert.IsFalse(result.Permanent, "Redirect should not be permanent");
            Assert.AreEqual(1, result.RouteValues.Count, string.Format("Redirect should forward to Requests.{0} action", view));
            Assert.AreEqual(view, result.RouteValues["action"], string.Format("Redirect should forward to Requests.{0} action", view));
        }

        #endregion

        #region Test data

        private User GetUser()
        {
            return new User();
        }

        private Player GetPlayer()
        {
            return new Player();
        }

        #endregion

        #region Mock

        private void SetupUserService(int id)
        {
            this._userServiceMock.Setup(m => m.GetUser(id)).Returns(new User());
        }

        private void SetupUserServiceReturnsNullUser(int id)
        {
            User user = null;
            this._userServiceMock.Setup(m => m.GetUser(id)).Returns(user);
        }

        private void SetupPlayerService(int id)
        {
            this._playerServiceMock.Setup(m => m.Get(id)).Returns(new Player());
        }

        private void SetupPlayerServiceReturnsNullPlayer(int id)
        {
            Player player = null;
            this._playerServiceMock.Setup(m => m.Get(id)).Returns(player);
        }

        private void SetupConfirmThrowsMissingEntityException()
        {
            _requestServiceMock.Setup(m => m.Approve(It.IsAny<int>()))
                .Throws(new MissingEntityException(string.Empty));
        }

        private void SetupDeclineThrowsMissingEntityException()
        {
            _requestServiceMock.Setup(m => m.Decline(It.IsAny<int>()))
                .Throws(new MissingEntityException(string.Empty));
        }

        private void AssertVerifyConfirm(Mock<IRequestService> mock, int requestId)
        {
            mock.Verify(m => m.Approve(It.Is<int>(id => id == requestId)), Times.Once());
        }

        private void AssertVerifyDecline(Mock<IRequestService> mock, int requestId)
        {
            mock.Verify(m => m.Decline(It.Is<int>(id => id == requestId)), Times.Once());
        }
        #endregion
    }
}