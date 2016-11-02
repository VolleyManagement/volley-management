using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Routing;
using VolleyManagement.Domain.RolesAggregate;
using VolleyManagement.UI.Areas.Admin.Models;
using VolleyManagement.UI.Areas.Mvc.ViewModels.Players;
using VolleyManagement.UI.Areas.Mvc.ViewModels.Teams;
using VolleyManagement.UnitTests.Services.PlayerService;
using VolleyManagement.UnitTests.Services.UserManager;
using VolleyManagement.UnitTests.Services.UsersService;

using VolleyManagement.UnitTests.Admin.ViewModels;
using VolleyManagement.UnitTests.Mvc.ViewModels;

namespace VolleyManagement.UnitTests.Admin.Controllers
{
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Authorization;
    using VolleyManagement.Domain.UsersAggregate;
    using VolleyManagement.UI.Areas.Admin.Controllers;
    using Comparers;

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class UsersControllerTest
    {
        private const int EXISTING_ID = 1;

        private readonly Mock<IAuthorizationService> _authServiceMock = new Mock<IAuthorizationService>();
        private readonly Mock<IUserService> _userServiceMock = new Mock<IUserService>();
        private readonly Mock<HttpContextBase> _httpContextMock = new Mock<HttpContextBase>();
        private readonly Mock<HttpRequestBase> _httpRequestMock = new Mock<HttpRequestBase>();

        private IKernel _kernel;
        private UsersController _sut;

        [TestInitialize]
        public void TestInit()
        {
            this._kernel = new StandardKernel();
            this._kernel.Bind<IUserService>().ToConstant(this._userServiceMock.Object);
            this._kernel.Bind<IAuthorizationService>().ToConstant(this._authServiceMock.Object);
            this._httpContextMock.SetupGet(c => c.Request).Returns(this._httpRequestMock.Object);
            this._sut = this._kernel.Get<UsersController>();
        }

        [TestMethod]
        public void UserDetails_NonExistentUser_HttpNotFoundResultIsReturned()
        {
            // Arrange
            SetupGetUserDetails(EXISTING_ID, null);

            // Act
            var result = this._sut.UserDetails(EXISTING_ID);

            // Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

      

        [TestMethod]
        public void UserDetails_ExistingUser_UserViewModelIsReturned()
        {
            // Arrange
            var user = CreateUser();
            SetupGetUserDetails(EXISTING_ID,user);

            var expected = CreateUserViewModel();

            // Act
            var actual = TestExtensions.GetModel<UserViewModel>(_sut.UserDetails(EXISTING_ID));

            // Assert
            TestHelper.AreEqual<UserViewModel>(expected, actual, new UserViewModelComparer());
        }



        private User CreateUser()
        {
            return new UserBuilder()
                         .WithId(EXISTING_ID)
                         .Build();
        }

        private UserViewModel CreateUserViewModel()
        {
            var player = CreatePlayerViewModel();

            return new UserAdminViewModelBuilder().Build();

        }

        private PlayerViewModel CreatePlayerViewModel()
        {
            return new PlayerMvcViewModelBuilder().Build();
        }

        private void MockUserServiceGetUserDetails(User user)
        {
            _userServiceMock.Setup(ts => ts.GetUser(It.IsAny<int>())).Returns(user);
        }

        private void SetupGetUserDetails(int userId, User user)
        {
            this._userServiceMock.Setup(tr => tr.GetUserDetails(userId)).Returns(user);
        }

        private void SetupGetAllUsers(List<User> users)
        {
            this._userServiceMock.Setup(tr => tr.GetAllUsers()).Returns(users);
        }

        private List<User> MakeTestUsers()
        {
            return new UserServiceTestFixture().TestUsers().Build();
        }

    }
}
