namespace VolleyManagement.UnitTesus.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Hosting;
    using Contracts;
    using Domain.Users;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.UnitTests.Services.UserService;
    using VolleyManagement.UnitTests.WebApi.ViewModels;
    using VolleyManagement.WebApi.Controllers;
    using VolleyManagement.WebApi.Mappers;
    using VolleyManagement.WebApi.ViewModels.Users;

    /// <summary>
    /// Tests for UsersController class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class UserControllerTesus
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
        /// User Repository Mock
        /// </summary>
        private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();

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
            var user = new UserBuilder().WithId(5).Build();
            MockSingleUser(user);
            var usersController = _kernel.Get<UsersController>();
            SetControllerRequest(usersController);

            // Act
            var response = usersController.Get(user.Id);
            var result = GetModelFromResponse<UserViewModel>(response);

            // Assert
            Assert.AreEqual(user.Id, result.Id);
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
            SetControllerRequest(usersController);
            var expected = HttpStatusCode.NotFound;

            // Act
            var actual = usersController.Get(userId);

            // Assert
            Assert.AreEqual(expected, actual.StatusCode);
        }

        /// <summary>
        /// Sets request message for controller
        /// </summary>
        /// <param name="controller">Current controller</param>
        public void SetControllerRequest(UsersController controller)
        {
            controller.Request = new HttpRequestMessage();
            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
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

        /// <summary>
        /// Gets generic T model from response content
        /// </summary>
        /// <typeparam name="T">Model type</typeparam>
        /// <param name="response">Http response message</param>
        /// <returns>T model</returns>
        private T GetModelFromResponse<T>(HttpResponseMessage response) where T : class
        {
            ObjectContent content = response.Content as ObjectContent;
            return (T)content.Value;
        }
    }
}
