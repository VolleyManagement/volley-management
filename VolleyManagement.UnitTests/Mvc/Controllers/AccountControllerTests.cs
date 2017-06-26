namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Comparers;
    using Contracts;
    using Contracts.Authentication;
    using Contracts.Authentication.Models;
    using Contracts.Authorization;
    using Domain.RolesAggregate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Services.UserManager;
    using UI.Areas.Mvc.Controllers;
    using UI.Areas.Mvc.ViewModels.Users;
    using UI.Infrastructure;
    using ViewModels;

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class AccountControllerTests
    {
        #region Consts

        private const int USER_ID = 2;
        private const string USER_NAME = "Jack";

        private readonly Role _adminRole = new Role() { Id = 1, Name = "Admin" };

        #endregion

        #region Fields

        private Mock<IVolleyUserManager<UserModel>> _userManagerMock;
        private Mock<IRolesService> _rolesServiceMock;
        private Mock<ICurrentUserService> _currentUserServiceMock;
        private Mock<VolleyExceptionFilterAttribute> _exceptionFilter;
        private Mock<ICacheProvider> _cacheProviderMock;
        private Mock<IUserService> _userServiceMock;

        #endregion

        #region Init

        [TestInitialize]
        public void TestInit()
        {
            _userManagerMock = new Mock<IVolleyUserManager<UserModel>>();

            _rolesServiceMock = new Mock<IRolesService>();

            _currentUserServiceMock = new Mock<ICurrentUserService>();

            _exceptionFilter = new Mock<VolleyExceptionFilterAttribute>();

            _cacheProviderMock = new Mock<ICacheProvider>();

            _userServiceMock = new Mock<IUserService>();
        }

        #endregion

        #region Tests

        /// <summary>
        /// Test for Details()
        /// </summary>
        [TestMethod]
        public void Details_UserExists_UserIsReturned()
        {
            // Arrange
            MockCurrentUser();
            MockFindById();
            MockGetRole();

            var expected = new UserMvcViewModelBuilder()
                    .WithId(USER_ID)
                    .WithUserName(USER_NAME)
                    .Build();

            var sut = CreateController();

            // Act
            var actual = TestExtensions.GetModelAsync<UserViewModel>(sut.Details());

            // Assert
            TestHelper.AreEqual<UserViewModel>(expected, actual, new UserViewModelComparer());
        }

        /// <summary>
        /// Test for Edit()
        /// </summary>
        /// <returns>Asynchronous operation</returns>
        [TestMethod]
        public async Task EditPostAction_UserExists_UserUpdated()
        {
            // Arrange
            MockCurrentUser();
            MockFindById();
            MockGetRole();

            var userEditViewModel = new UserEditMvcViewModelBuilder()
                .WithId(USER_ID)
                .WithCellPhone("068-33-44-555")
                .WithEmail("example@ex.ua")
                .WithName("Vasya Petichkin")
                .Build();

            var sut = CreateController();

            // Act
            await sut.Edit(userEditViewModel);

            // Assert
            _userManagerMock.Verify(um => um.UpdateAsync(It.IsAny<UserModel>()), Times.Once());
        }

        /// <summary>
        /// Test() edit method.
        /// </summary>
        /// <returns>Asynchronous operation</returns>
        [TestMethod]
        [Ignore] // TODO: SUT should be refactored to be able mock User
        public async Task EditPostAction_UserIdPassed_ExceptionThrown()
        {
            // Arrange
            MockCurrentUser();
            MockGetRole();

            _userManagerMock.Setup(um => um.FindByIdAsync(USER_ID))
                            .Returns(Task.FromResult<UserModel>(null));

            var userEditViewModel = new UserEditMvcViewModelBuilder()
                .Build();

            var sut = CreateController();

            // Act
            await sut.Edit(userEditViewModel);

            // Assert
            _userManagerMock.Verify(um => um.UpdateAsync(It.IsAny<UserModel>()), Times.Never());
        }

        #endregion

        #region Additional methods

        private ControllerContext GetControllerContext()
        {
            var claim = new Claim("id", USER_ID.ToString());
            var identityMock = Mock.Of<ClaimsIdentity>(ci => ci.FindFirst(It.IsAny<string>()) == claim);
            var mockContext = Mock.Of<ControllerContext>(cc => cc.HttpContext.User.Identity == identityMock);
            return mockContext;
        }

        private void MockFindById()
        {
            _userManagerMock.Setup(um => um.FindByIdAsync(USER_ID))
                            .Returns(Task.FromResult(
                                new UserModelBuilder()
                                .WithId(USER_ID)
                                .WithUserName(USER_NAME)
                                .Build()));
        }

        private void MockGetRole()
        {
            _rolesServiceMock.Setup(rs => rs.GetRole(It.IsAny<int>()))
                .Returns(_adminRole);
        }

        private void MockCurrentUser()
        {
            _currentUserServiceMock.Setup(s => s.GetCurrentUserId())
                .Returns(USER_ID);
        }

        private AccountController CreateController()
        {
            return new AccountController(
                _userManagerMock.Object,
                _rolesServiceMock.Object,
                _userServiceMock.Object,
                _cacheProviderMock.Object,
                _currentUserServiceMock.Object);
        }

        #endregion
    }
}