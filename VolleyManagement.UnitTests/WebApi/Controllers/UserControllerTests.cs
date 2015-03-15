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
