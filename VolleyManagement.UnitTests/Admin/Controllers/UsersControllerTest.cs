using System.Collections.Generic;
using System.Linq;
using VolleyManagement.Domain.RolesAggregate;
using VolleyManagement.UI.Areas.Mvc.ViewModels.Teams;
using VolleyManagement.UnitTests.Services.PlayerService;
using VolleyManagement.UnitTests.Services.UserManager;

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


    class UsersControllerTest
    {
        private const int EXISTING_ID = 1;

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
            this._httpContextMock.SetupGet(c => c.Request).Returns(this._httpRequestMock.Object);
            this._sut = this._kernel.Get<UsersController>();
        }

        [TestMethod]
        public void UserDetails_NonExistentUser_HttpNotFoundResultIsReturned()
        {
            // Arrange
            SetupGetUser(EXISTING_ID, null);

            // Act
            var result = this._sut.UserDetails(EXISTING_ID);

            // Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        private User CreateUser()
        {
            return new UserBuilder()
                         .WithId(EXISTING_ID)
                         .Build();
        }

        private void MockUserServiceGetUser(User user)
        {
            _userServiceMock.Setup(ts => ts.GetUser(It.IsAny<int>())).Returns(user);
        }

        private void SetupGetUser(int userId, User user)
        {
            this._userServiceMock.Setup(tr => tr.GetUser(userId)).Returns(user);
        }

        private void SetupGetUserRoles(int userId, List<Role> roles)
        {
            this._userServiceMock.Setup(tr => tr.GetUserRoles(userId)).Returns(roles);
        }

        private void SetupGetAuthProviders(int userId, List<LoginProviderInfo> authProviders)
        {
            this._userServiceMock.Setup(tr => tr.GetAuthProviders(userId)).Returns(authProviders);
        }
    }
}
