namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Authentication;
    using VolleyManagement.Contracts.Authentication.Models;
    using VolleyManagement.Contracts.Authorization;
    using VolleyManagement.Domain.RolesAggregate;
    using VolleyManagement.UI.Areas.Mvc.Controllers;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Users;
    using VolleyManagement.UI.Infrastructure;
    using VolleyManagement.UnitTests.Mvc.Comparers;
    using VolleyManagement.UnitTests.Mvc.ViewModels;
    using VolleyManagement.UnitTests.Services.UserManager;

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

        private IKernel _kernel;

        private Mock<IVolleyUserManager<UserModel>> _userManagerMock = new Mock<IVolleyUserManager<UserModel>>();

        private Mock<IRolesService> _rolesServiceMock = new Mock<IRolesService>();

        private Mock<ICurrentUserService> _currentUserServiceMock = new Mock<ICurrentUserService>();

        private Mock<VolleyExceptionFilterAttribute> _exceptionFilter = new Mock<VolleyExceptionFilterAttribute>();

        private Mock<ICacheProvider> _cacheProviderMock = new Mock<ICacheProvider>();

        private Mock<IUserService> _userServiceMock = new Mock<IUserService>();
        #endregion

        #region Init

        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();

            _kernel.Bind<IVolleyUserManager<UserModel>>()
                   .ToConstant(_userManagerMock.Object);
            _kernel.Bind<IRolesService>()
                   .ToConstant(_rolesServiceMock.Object);
            _kernel.Bind<ICacheProvider>()
                   .ToConstant(_cacheProviderMock.Object);
            _kernel.Bind<ICurrentUserService>()
                   .ToConstant(_currentUserServiceMock.Object);
            _kernel.Bind<IUserService>()
                   .ToConstant(_userServiceMock.Object);
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
        public async Task EditPostAction_UserIdPassed_ExceptionThrown()
        {
            // Arrange
            _userManagerMock.Setup(um => um.FindByIdAsync(USER_ID))
                            .Returns(Task.FromResult<UserModel>(null));
            MockGetRole();

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

        private AccountController CreateController()
        {
            var sut = _kernel.Get<AccountController>();
            sut.ControllerContext = GetControllerContext();
            return sut;
        }

        #endregion
    }
}