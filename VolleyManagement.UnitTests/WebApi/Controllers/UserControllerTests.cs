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
        /// Test for Map() method.
        /// The method should map user domain model to view model.
        /// </summary>
        [TestMethod]
        public void Map_UserDomainModelAsParam_MappedToViewModelWebApi()
        {
            // Arrange
            var testUser = new UserBuilder()
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

            // Act
            var actual = UserViewModel.Map(testUser);

            // Assert
            AssertExtensions.AreEqual<UserViewModel>(expected, actual, new UserViewModelComparer());
        }

        /// <summary>
        /// Test for ToDomain() method.
        /// The method should map user view model to domain model.
        /// </summary>
        [TestMethod]
        public void Map_UserViewModelWebApi_MappedToDomainModel()
        {
            // Arrange
            var testUserViewModel = new UserViewModelBuilder()
                                        .WithId(2)
                                        .WithUserName("UserVolley")
                                        .WithFullName("Second User")
                                        .WithEmail("seconduser@gmail.com")
                                        .WithPassword("abc222")
                                        .WithCellPhone("0500000002")
                                        .Build();
            var expected = new UserBuilder()
                                        .WithId(2)
                                        .WithUserName("UserVolley")
                                        .WithFullName("Second User")
                                        .WithEmail("seconduser@gmail.com")
                                        .WithPassword("abc222")
                                        .WithCellPhone("0500000002")
                                        .Build();

            // Act
            var actual = testUserViewModel.ToDomain();

            // Assert
            AssertExtensions.AreEqual<User>(expected, actual, new UserComparer());
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
