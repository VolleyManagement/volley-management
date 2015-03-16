namespace VolleyManagement.UnitTests.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Results;
    using System.Web.Http.OData.Results;
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
        /// An unassigned user identifier.
        /// </summary>
        private const int UNASSIGNED_USER_ID = 0;

        /// <summary>
        /// An expected test user identifier.
        /// </summary>
        private const int EXPECTED_USER_ID = 10;

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
        /// Test Post method. Basic story.
        /// </summary>
        [TestMethod]
        public void Post_ValidViewModel_UserCreated()
        {
            // Arrange
            var controller = _kernel.Get<UsersController>();
            var expected = new UserViewModelBuilder().Build();
            expected.Id = EXPECTED_USER_ID;
            _userServiceMock.Setup(us => us.Create(It.IsAny<User>()))
                .Callback((User u) => { u.Id = EXPECTED_USER_ID; });
            var input = new UserViewModelBuilder()
                .WithId(UNASSIGNED_USER_ID)
                .Build();


            // Act
            var createdResult = controller.Post(input) as
                CreatedODataResult<UserViewModel>;

            // Assert
            Assert.IsNotNull(createdResult);
            var actual = createdResult.Entity;
            _userServiceMock.Verify(us => us.Create(It.IsAny<User>()), Times.Once);
            AssertExtensions.AreEqual<UserViewModel>(expected, actual, new UserViewModelComparer());
        }

        /// <summary>
        /// Test for Get() method. Method should return existing users.
        /// </summary>
        [TestMethod]
        public void Get_UsersExist_UsersReturned()
        {
            // Arrange
            var testData = _testFixture.TestUsers().Build();
            MockUsers(testData);
            var controller = _kernel.Get<UsersController>();
            var expected = GetExpectedUsersList();

            // Act
            var actual = controller.GetUsers().ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual, new UserViewModelComparer());
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

        /// <summary>
        /// Creates a test UserViewModel collection with the same data as
        /// in the UserServiceTestFixture.
        /// </summary>
        /// <returns>A list of UserViewModel with test data.</returns>
        private List<UserViewModel> GetExpectedUsersList()
        {
            var userModels = new List<UserViewModel>();
            userModels.Add(new UserViewModelBuilder()
                .WithId(1)
                .WithUserName("testA")
                .WithFullName("Test Name A")
                .WithEmail("test1@gmail.com")
                .WithPassword(string.Empty)
                .WithCellPhone("0500000001")
                .Build());
            userModels.Add(new UserViewModelBuilder()
                .WithId(2)
                .WithUserName("testB")
                .WithFullName("Test Name B")
                .WithEmail("test2@gmail.com")
                .WithPassword(string.Empty)
                .WithCellPhone("0500000002")
                .Build());
            userModels.Add(new UserViewModelBuilder()
                .WithId(3)
                .WithUserName("testC")
                .WithFullName("Test Name C")
                .WithEmail("test3@gmail.com")
                .WithPassword(string.Empty)
                .WithCellPhone("0500000003")
                .Build());
            return userModels;
        }
    }
}
