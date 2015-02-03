namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Contracts;
    using VolleyManagement.Domain.Users;
    using VolleyManagement.Mvc.Controllers;
    using VolleyManagement.Mvc.Mappers;
    using VolleyManagement.Mvc.ViewModels.Users;
    using VolleyManagement.UnitTests.Mvc.ViewModels;
    using VolleyManagement.UnitTests.Services.UserService;

    /// <summary>
    /// Tests for MVC UsersController class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class UsersControllerTests
    {
        /// <summary>
        /// Test Fixture
        /// </summary>
        private readonly UserServiceTestFixture _testFixture =
            new UserServiceTestFixture();

        /// <summary>
        /// Users Service Mock
        /// </summary>
        private readonly Mock<IUserService> _userServiceMock =
            new Mock<IUserService>();

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
        /// Test to Index action. The action should return not empty users list
        /// </summary>
        [TestMethod]
        public void Index_UsersExist_UsersReturned()
        {
            // Arrange
            var testData = this._testFixture
                                       .AddUser(new UserBuilder()
                                                        .WithId(1)
                                                        .WithUserName("testUser")
                                                        .WithFullName("User")
                                                        .WithEmail("testuser@test.com")
                                                        .WithCellPhone("1234567890")
                                                        .WithPassword("testpass")
                                                        .Build())
                                       .Build();
            this.MockUsers(testData);

            var sut = this._kernel.Get<UsersController>();

            var expected = new List<UserViewModel>
            {
                new UserViewModelBuilder()
                            .WithId(1)
                            .WithUserName("testUser")
                            .WithFullName("User")
                            .WithEmail("testuser@test.com")
                            .WithCellPhone("1234567890")
                            .WithPassword(string.Empty)
                            .Build()
            };

            // Act
            var actual = TestExtensions.GetModel<IEnumerable<UserViewModel>>(sut.Index()).ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual, new UserViewModelComparer());
        }

        /// <summary>
        /// Test with negative scenario for Index action.
        /// The action should thrown Argument null exception
        /// </summary>
        [TestMethod]
        public void Index_UsersDoNotExist_ExceptionThrown()
        {
            // Arrange
            this._userServiceMock.Setup(ur => ur.GetAll())
                .Throws(new ArgumentNullException());

            var sut = this._kernel.Get<UsersController>();
            var expected = (int)HttpStatusCode.NotFound;

            // Act
            var actual = (sut.Index() as HttpNotFoundResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Mocks test data
        /// </summary>
        /// <param name="testData">Data to mock</param>
        private void MockUsers(IEnumerable<User> testData)
        {
            this._userServiceMock.Setup(ur => ur.GetAll())
                .Returns(testData.AsQueryable());
        }
    }
}
