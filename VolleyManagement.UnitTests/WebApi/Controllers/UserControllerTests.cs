namespace VolleyManagement.UnitTests.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Web.Http.OData.Results;

    using Contracts;
    using Domain.Users;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using Services.UserService;

    using VolleyManagement.UI.Areas.WebApi.ApiControllers;
    using VolleyManagement.UI.Areas.WebApi.Controllers;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Users;
    using VolleyManagement.UnitTests.WebApi.ViewModels;

    /// <summary>
    /// Tests for UsersController class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class UserControllerTests
    {
        /// <summary>
        /// Test Fixture
        /// </summary>
        private readonly UserServiceTestFixture _testFixture = new UserServiceTestFixture();

        /// <summary>
        /// User Service Mock
        /// </summary>
        private readonly Mock<IUserService> _userServiceMock = new Mock<IUserService>();

        /// <summary>
        /// IoC for tests
        /// </summary>
        private IKernel _kernel;

        /// <summary>
        /// Initializes test data
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            this._kernel = new StandardKernel();
            this._kernel.Bind<IUserService>()
                   .ToConstant(this._userServiceMock.Object);
        }

        /// <summary>
        /// Test Post method. Basic story.
        /// </summary>
        [TestMethod]
        [Ignore] // BUG: FIX ASAP
        public void Post_ValidViewModel_UserCreated()
        {
            // Arrange
            var controller = _kernel.Get<UsersController>();
            var expected = new UserViewModelBuilder().Build();

            // Act
            var response = controller.Post(expected);
            var actual = (response as CreatedODataResult<UserViewModel>).Entity;

            // Assert
            _userServiceMock.Verify(us => us.Create(It.IsAny<User>()), Times.Once());
            AssertExtensions.AreEqual(expected, actual, new UserViewModelComparer());
        }

        /// <summary>
        /// Test for Get() method. Method should return existing users.
        /// </summary>
        [TestMethod]
        [Ignore] // BUG: FIX ASAP
        public void Get_UsersExist_UsersReturned()
        {
            // Arrange
            var testData = _testFixture.TestUsers().Build();
            MockUsers(testData);
            var sut = _kernel.Get<UsersController>();

            // Act
            // var actual = sut.Get().ToList();

            // Assert
            // CollectionAssert.AreEqual(expected, actual, new UserViewModelComparer());
        }

        /// <summary>
        /// Mocks users test data.
        /// </summary>
        /// <param name="testData">Users to mock.</param>
        private void MockUsers(IEnumerable<User> testData)
        {
            _userServiceMock.Setup(u => u.Get()).Returns(testData.AsQueryable());
        }
    }
}
