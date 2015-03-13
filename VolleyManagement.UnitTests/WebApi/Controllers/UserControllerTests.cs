namespace VolleyManagement.UnitTests.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Contracts;
    using VolleyManagement.Domain.Users;
    using VolleyManagement.UI.Areas.WebApi.ApiControllers;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Users;
    using VolleyManagement.UnitTests.Services.UserService;
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
            _kernel = new StandardKernel();
            _kernel.Bind<IUserService>().ToConstant(_userServiceMock.Object);
        }

        /// <summary>
        /// Test for Get() by key method. The method should return specific user
        /// </summary>
        [TestMethod]
        [Ignore]// BUG: FIX ASAP
        public void Get_SpecificUserExist_UserReturned()
        {
            // Arrange
            //var user = new UserBuilder()
            //                .WithId(2)
            //                .WithUserName("UserLogin")
            //                .WithFullName("Second User")
            //                .WithEmail("seconduser@gmail.com")
            //                .WithPassword("abc222")
            //                .WithCellPhone("0503222233")
            //                .Build();
            //var expected = new UserViewModelBuilder()
            //                .WithId(2)
            //                .WithUserName("UserLogin")
            //                .WithFullName("Second User")
            //                .WithEmail("seconduser@gmail.com")
            //                .WithPassword(string.Empty)
            //                .WithCellPhone("0503222233")
            //                .Build();
            //MockSingleUser(user);
            //var usersController = _kernel.Get<UsersController>();
            //TestExtensions.SetControllerRequest(usersController);

            //// Act
            //var response = usersController.Get(user.Id);
            //var actual = TestExtensions.GetModelFromResponse<UserViewModel>(response);

            //// Assert
            //AssertExtensions.AreEqual<UserViewModel>(expected, actual, new UserViewModelComparer());
        }

        /// <summary>
        /// Test for Get() by key method. The method should return "Not Found" status
        /// </summary>
        [TestMethod]
        [Ignore]// BUG: FIX ASAP
        public void Get_GeneralException_NotFoundStatusReturned()
        {
            // Arrange
            //var userId = 5;
            //_userServiceMock.Setup(us => us.FindById(userId))
            //   .Throws(new Exception());
            //var usersController = _kernel.Get<UsersController>();
            //TestExtensions.SetControllerRequest(usersController);
            //var expected = HttpStatusCode.NotFound;

            //// Act
            //var actual = usersController.Get(userId);

            //// Assert
            //Assert.AreEqual(expected, actual.StatusCode);
        }

        /// <summary>
        /// Test Put method. Basic story.
        /// </summary>
        [TestMethod]
        [Ignore]// BUG: FIX ASAP
        public void Put_ValidViewModel_UserUpdated()
        {
            // Arrange
            //var controller = _kernel.Get<UsersController>();
            //TestExtensions.SetControllerRequest(controller);
            //var expected = new UserViewModelBuilder().Build();

            //// Act
            //var actual = controller.Put(expected.Id, expected);

            //// Assert
            //_userServiceMock.Verify(us => us.Edit(It.IsAny<User>()), Times.Once());
            //Assert.AreEqual(HttpStatusCode.OK, actual.StatusCode);
        }

        /// <summary>
        /// Test Put method. Invalid data
        /// </summary>
        [TestMethod]
        [Ignore]// BUG: FIX ASAP
        public void Put_InvalidData_BadRequestReturned()
        {
            // Arrange
            //var controller = _kernel.Get<UsersController>();
            //TestExtensions.SetControllerRequest(controller);
            //var expected = new UserViewModelBuilder().Build();
            //var invalidKey = expected.Id + 1;

            //// Act
            //var actual = controller.Put(invalidKey, expected);

            //// Assert
            //_userServiceMock.Verify(us => us.Edit(It.IsAny<User>()), Times.Never());
            //Assert.AreEqual(HttpStatusCode.BadRequest, actual.StatusCode);
        }

        /// <summary>
        /// Test for Put method. The method should return "Bad request" status
        /// </summary>
        [TestMethod]
        [Ignore]// BUG: FIX ASAP
        public void Put_ArgumentException_BadRequestReturned()
        {
            // Arrange
            //var expected = new UserViewModelBuilder().Build();
            //_userServiceMock.Setup(us => us.Edit(It.IsAny<User>()))
            //   .Throws(new ArgumentException());
            //var controller = _kernel.Get<UsersController>();
            //TestExtensions.SetControllerRequest(controller);

            //// Act
            //var actual = controller.Put(expected.Id, expected);

            //// Assert
            //Assert.AreEqual(HttpStatusCode.BadRequest, actual.StatusCode);
        }

        /// <summary>
        /// Test for Put method. The method should return "Internal server error" status
        /// </summary>
        [TestMethod]
        [Ignore]// BUG: FIX ASAP
        public void Put_GeneralException_InternalServerErrorReturned()
        {
            // Arrange
            //var expected = new UserViewModelBuilder().Build();
            //_userServiceMock.Setup(us => us.Edit(It.IsAny<User>()))
            //   .Throws(new Exception());
            //var controller = _kernel.Get<UsersController>();
            //TestExtensions.SetControllerRequest(controller);

            //// Act
            //var actual = controller.Put(expected.Id, expected);

            //// Assert
            //Assert.AreEqual(HttpStatusCode.InternalServerError, actual.StatusCode);
        }

        /// <summary>
        /// Test Post method. Basic story.
        /// </summary>
        [TestMethod]
        [Ignore]// BUG: FIX ASAP
        public void Post_ValidViewModel_UserCreated()
        {
            // Arrange
            //var controller = _kernel.Get<UsersController>();
            //TestExtensions.SetControllerRequest(controller);
            //var expected = new UserViewModelBuilder().Build();

            //// Act
            //var response = controller.Post(expected);
            //var actual = TestExtensions.GetModelFromResponse<UserViewModel>(response);

            //// Assert
            //_userServiceMock.Verify(us => us.Create(It.IsAny<User>()), Times.Once());
            //Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            //AssertExtensions.AreEqual<UserViewModel>(expected, actual, new UserViewModelComparer());
        }

        /// <summary>
        /// Test for Get() method. Method should return existing users.
        /// </summary>
        [TestMethod]
        [Ignore]// BUG: FIX ASAP
        public void Get_UsersExist_UsersReturned()
        {
            // Arrange
            //var testData = _testFixture.TestUsers().Build();
            //MockUsers(testData);
            //var sut = _kernel.Get<UsersController>();

            //var expected = new List<UserViewModel>();
            //foreach (var user in testData)
            //{
            //    expected.Add(DomainToViewModel.Map(user));
            //}

            //// Act
            //var actual = sut.Get().ToList();

            //// Assert
            //CollectionAssert.AreEqual(expected, actual, new UserViewModelComparer());
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

        /// <summary>
        /// Mocks single test user.
        /// </summary>
        /// <param name="testUser">User to mock.</param>
        private void MockSingleUser(User testUser)
        {
            _userServiceMock.Setup(ur => ur.Get(testUser.Id)).Returns(testUser);
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
