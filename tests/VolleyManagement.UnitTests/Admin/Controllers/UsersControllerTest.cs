namespace VolleyManagement.UnitTests.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Comparers;
    using Contracts;
    using Domain.UsersAggregate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Contracts.Authorization;
    using UI.Areas.Admin.Controllers;
    using UI.Areas.Admin.Models;
    using UI.Areas.Mvc.ViewModels.Players;
    using ViewModels;
    using Mvc.ViewModels;
    using Services.UsersService;

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class UsersControllerTest
    {
        private const int EXISTING_ID = 1;

        private Mock<IUserService> _userServiceMock;
        private Mock<ICurrentUserService> _currentUserService;

        [TestInitialize]
        public void TestInit()
        {
            _userServiceMock = new Mock<IUserService>();
            _currentUserService = new Mock<ICurrentUserService>();
        }

        [TestMethod]
        public void UserDetails_NonExistentUser_HttpNotFoundResultIsReturned()
        {
            // Arrange
            SetupGetUserDetails(EXISTING_ID, null);
            var sut = BuildSUT();

            // Act
            var result = sut.UserDetails(EXISTING_ID);

            // Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void UserDetails_ExistingUser_UserViewModelIsReturned()
        {
            // Arrange
            var user = CreateUser();
            SetupGetUserDetails(EXISTING_ID, user);

            var sut = BuildSUT();
            var expected = CreateUserViewModel();

            // Act
            var actual = TestExtensions.GetModel<UserViewModel>(sut.UserDetails(EXISTING_ID));

            // Assert
            TestHelper.AreEqual<UserViewModel>(expected, actual, new UserViewModelComparer());
        }

        private UsersController BuildSUT()
        {
            return new UsersController(_userServiceMock.Object);
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
            _userServiceMock.Setup(tr => tr.GetUserDetails(userId)).Returns(user);
        }

        private void SetupGetAllUsers(List<User> users)
        {
            _userServiceMock.Setup(tr => tr.GetAllUsers()).Returns(users);
        }

        private List<User> MakeTestUsers()
        {
            return new UserServiceTestFixture().TestUsers().Build();
        }
    }
}
