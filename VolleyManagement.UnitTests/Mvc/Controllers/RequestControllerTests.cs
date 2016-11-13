using System.Web.Mvc;
using VolleyManagement.UnitTests.Services.UsersService;

namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Contracts;
    using VolleyManagement.Domain.PlayersAggregate;
    using VolleyManagement.Domain.RequestsAggregate;
    using VolleyManagement.Domain.UsersAggregate;
    using VolleyManagement.UI.Areas.Admin.Controllers;
    using VolleyManagement.UI.Areas.Admin.Models;
    using VolleyManagement.UnitTests.Admin.ViewModels;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Players;
    using VolleyManagement.UnitTests.Services.PlayerService;
    using VolleyManagement.UnitTests.Services.UserManager;
    using WebApi.ViewModels;
    using Admin.Comparers;
    using Contracts.Exceptions;

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class RequestControllerTests
    {
        #region Fields

        private const int EXISTING_ID = 1;

        private readonly Mock<IRequestService> _requestServiceMock = new Mock<IRequestService>();
        private readonly Mock<IUserService> _userServiceMock = new Mock<IUserService>();
        private readonly Mock<IPlayerService> _playerServiceMock = new Mock<IPlayerService>();
        private readonly Mock<HttpContextBase> _httpContextMock = new Mock<HttpContextBase>();
        private readonly Mock<HttpRequestBase> _httpRequestMock = new Mock<HttpRequestBase>();

        private IKernel _kernel;
        private RequestController _sut;
        #endregion

        #region Init

        [TestInitialize]
        public void TestInit()
        {
            this._kernel = new StandardKernel();
            this._kernel.Bind<IUserService>().ToConstant(this._userServiceMock.Object);
            this._kernel.Bind<IPlayerService>().ToConstant(this._playerServiceMock.Object);
            this._kernel.Bind<IRequestService>().ToConstant(this._requestServiceMock.Object);
            this._httpContextMock.SetupGet(c => c.Request).Returns(this._httpRequestMock.Object);
          //  this._sut = _kernel.Get<RequestController>();
        }

        #endregion

        #region Tests

       [TestMethod]
        public void UserDetails_ExistingUser_UserViewModelIsReturned()
        {
            // Arrange
            var expected = GetUser();
            int id = 1;
            MockUserService(id);
            var sut = _kernel.Get<RequestController>();

            // Act
            var actionResult = sut.UserDetails(id);
            var actual = TestExtensions.GetModel<User>(actionResult);

            // Assert
            TestHelper.AreEqual<User>(expected, actual, new UserComparer());
        }

        [TestMethod]
        public void UserDetails_NonExistentUser_HttpNotFoundResultIsReturned()
        {
            // Arrange
            const int USER_ID = 1;
            MockUserServiceReturnsNullUser(USER_ID);
            var sut = _kernel.Get<RequestController>();

            // Act
            var actionResult = sut.UserDetails(USER_ID);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void Confirm_AnyRequest_RequestRedirectToIndex()
        {
            // Arrange
            const int REQUEST_ID = 1;
            var controller = _kernel.Get<RequestController>();

            // Act
            var actionResult = controller.Confirm(REQUEST_ID);

            // Assert
            AssertValidRedirectResult(actionResult, "Index");
        }

        [TestMethod] // не нужен?
        public void Close_AnyRequest_RequestConfirmed()
        {
            // Arrange
            const int REQUEST_ID = 1;
            var controller = _kernel.Get<RequestController>();

            // Act
            var actionResult = controller.Confirm(REQUEST_ID);

            // Assert
            AssertConfirmVerify(_requestServiceMock, REQUEST_ID);
        }

        [TestMethod]
        public void Confirm_NonExistentRequest_ThrowsException()
        {
            // Arrange
            const int REQUEST_ID = 1;
            SetupConfirmThrowsMissingEntityException();

            // Act
            var result = _sut.Confirm(REQUEST_ID);

            // Assert
            Assert.IsNotNull(result, "");
        }

        #endregion

        #region Test data

        private User GetUser()
        {
            return new User();
        }

        private static List<Request> GetRequests()
        {
            return new List<Request>
                       {
                           new Request
                           {
                               Id = 1,
                               PlayerId = 1,
                               UserId = 1,
                           },
                           new Request
                           {
                               Id = 2,
                               PlayerId = 2,
                               UserId = 2
                           },
                       };
        }

        private static List<RequestViewModel> GetRequestsViewModels()
        {
            return new List<RequestViewModel>
                       {
                           new RequestViewModel
                           {
                               Id = 1,
                               PlayerLastName = "Player 1",
                               UserName = "User 1"
                           },
                           new RequestViewModel
                           {
                               Id = 2,
                               PlayerLastName = "Player 2",
                               UserName = "User 2"
                           },
                        };
        }

        #endregion

        private static void AssertValidRedirectResult(ActionResult actionResult, string view)
        {
            var result = (RedirectToRouteResult)actionResult;
            Assert.IsFalse(result.Permanent, "Redirect should not be permanent");
            Assert.AreEqual(1, result.RouteValues.Count, string.Format("Redirect should forward to Requests.{0} action", view));
            Assert.AreEqual(view, result.RouteValues["action"], string.Format("Redirect should forward to Requests.{0} action", view));
        }

        private static void AssertConfirmVerify(Mock<IRequestService> mock, int requestId)
        {
            mock.Verify(ps => ps.Approve(It.Is<int>(id => id == requestId)), Times.Once());
        }

        //Assert.AreEqual(expected.Id, actual.Id);
        private void MockUserService(int id)
        {
            this._userServiceMock.Setup(m => m.GetUser(id)).Returns(new User());
        }

        private void MockUserServiceReturnsNullUser(int id)
        {
            User user = null;
            this._userServiceMock.Setup(m => m.GetUser(id)).Returns(user);
        }

        private void SetupConfirmThrowsMissingEntityException()
        {
            _requestServiceMock.Setup(ts => ts.Get(It.IsAny<int>()))
                .Throws(new MissingEntityException(string.Empty));
        }

        private void SetupGetUserDetails(int userId, User user)
        {
            this._userServiceMock.Setup(tr => tr.GetUserDetails(userId)).Returns(user);
        }

        private void SetupGetPlayerDetails(int playerId, Player player)
        {
            _playerServiceMock.Setup(tr => tr.Get(playerId)).Returns(player);
        }

        private UserViewModel CreateUserViewModel()
        {
            return new UserAdminViewModelBuilder().Build();
        }
    }
}
