namespace VolleyManagement.UnitTests.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using Contracts;
    using Domain.Users;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.UnitTests.Services.UserService;
    using VolleyManagement.UnitTests.WebApi.ViewModels;
    using VolleyManagement.WebApi.Controllers;
    using VolleyManagement.WebApi.ViewModels.Users;

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
            _kernel = new StandardKernel();
            _kernel.Bind<IUserService>().ToConstant(_userServiceMock.Object);
        }

        /// <summary>
        /// Test for Get() by key method. The method should return specific user
        /// </summary>
        [TestMethod]
        public void Get_SpecificUserExist_UserReturned()
        {
            // Arrange
            var user = new UserBuilder()
                            .WithId(2)
                            .WithUserName("UserLogin")
                            .WithFullName("Second User")
                            .WithEmail("seconduser@gmail.com")
                            .WithPassword("abc222")
                            .WithCellPhone("0503222233")
                            .Build();
            var expected = new UserViewModelBuilder()
                            .WithId(2)
                            .WithUserName("UserLogin")
                            .WithFullName("Second User")
                            .WithEmail("seconduser@gmail.com")
                            .WithPassword(string.Empty)
                            .WithCellPhone("0503222233")
                            .Build();
            MockSingleUser(user);
            var usersController = _kernel.Get<UsersController>();
            TestExtensions.SetControllerRequest(usersController);

            // Act
            var response = usersController.Get(user.Id);
            var actual = TestExtensions.GetModelFromResponse<UserViewModel>(response);

            // Assert
            AssertExtensions.AreEqual<UserViewModel>(expected, actual, new UserViewModelComparer());
        }

        /// <summary>
        /// Test for Get() by key method. The method should return "Not Found" status
        /// </summary>
        [TestMethod]
        public void Get_GeneralException_NotFoundStatusReturned()
        {
            // Arrange
            var userId = 5;
            _userServiceMock.Setup(us => us.FindById(userId))
               .Throws(new Exception());
            var usersController = _kernel.Get<UsersController>();
            TestExtensions.SetControllerRequest(usersController);
            var expected = HttpStatusCode.NotFound;

            // Act
            var actual = usersController.Get(userId);

            // Assert
            Assert.AreEqual(expected, actual.StatusCode);
        }

        /// <summary>
        /// Test Put method. Basic story.
        /// </summary>
        [TestMethod]
        public void Put_ValidViewModel_UserUpdated()
        {
            // Arrange
            var controller = _kernel.Get<UsersController>();
            TestExtensions.SetControllerRequest(controller);
            var expected = new UserViewModelBuilder().Build();

            // Act
            var actual = controller.Put(expected.Id, expected);

            // Assert
            _userServiceMock.Verify(us => us.Edit(It.IsAny<User>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.OK, actual.StatusCode);
        }

        /// <summary>
        /// Test Put method. Invalid data
        /// </summary>
        [TestMethod]
        public void Put_InvalidData_BadRequestReturned()
        {
            // Arrange
            var controller = _kernel.Get<UsersController>();
            TestExtensions.SetControllerRequest(controller);
            var expected = new UserViewModelBuilder().Build();
            var invalidKey = expected.Id + 1;

            // Act
            var actual = controller.Put(invalidKey, expected);

            // Assert
            _userServiceMock.Verify(us => us.Edit(It.IsAny<User>()), Times.Never());
            Assert.AreEqual(HttpStatusCode.BadRequest, actual.StatusCode);
        }

        /// <summary>
        /// Test for Put method. The method should return "Bad request" status
        /// </summary>
        [TestMethod]
        public void Put_ArgumentException_BadRequestReturned()
        {
            // Arrange
            var expected = new UserViewModelBuilder().Build();
            _userServiceMock.Setup(us => us.Edit(It.IsAny<User>()))
               .Throws(new ArgumentException());
            var controller = _kernel.Get<UsersController>();
            TestExtensions.SetControllerRequest(controller);

            // Act
            var actual = controller.Put(expected.Id, expected);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, actual.StatusCode);
        }

        /// <summary>
        /// Test for Put method. The method should return "Internal server error" status
        /// </summary>
        [TestMethod]
        public void Put_GeneralException_InternalServerErrorReturned()
        {
            // Arrange
            var expected = new UserViewModelBuilder().Build();
            _userServiceMock.Setup(us => us.Edit(It.IsAny<User>()))
               .Throws(new Exception());
            var controller = _kernel.Get<UsersController>();
            TestExtensions.SetControllerRequest(controller);

            // Act
            var actual = controller.Put(expected.Id, expected);

            // Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, actual.StatusCode);
        }

        /// <summary>
        /// Mocks test data
        /// </summary>
        /// <param name="testData">Data to mock</param>
        private void MockUsers(IEnumerable<User> testData)
        {
            _userServiceMock.Setup(ur => ur.GetAll()).Returns(testData.AsQueryable());
        }

        /// <summary>
        /// Mocks test data
        /// </summary>
        /// <param name="testData">Data to mock</param>
        private void MockSingleUser(User testData)
        {
            _userServiceMock.Setup(ur => ur.FindById(testData.Id)).Returns(testData);
        }
    }
}
