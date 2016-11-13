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
            this._sut = _kernel.Get<RequestController>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void UserDetails_ExistingUser_UserViewModelIsReturned()
        {
            // Arrange
            var requests = GetRequests();
            _requestServiceMock.Setup(r => r.Get()).Returns(requests);

            var user = new UserBuilder().WithId(EXISTING_ID).Build();

            SetupGetUserDetails(EXISTING_ID, user);
            var expected = CreateUserViewModel();

            // Act
            var actual = TestExtensions.GetModel<UserViewModel>(_sut.UserDetails(EXISTING_ID));

            // Assert
            TestHelper.AreEqual<UserViewModel>(expected, actual, new UserViewModelComparer());
        }

        #endregion

        #region Test data

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
