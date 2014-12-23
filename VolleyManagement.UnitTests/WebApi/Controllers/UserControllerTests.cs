﻿namespace VolleyManagement.UnitTests.WebApi.Controllers
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
    using Services.UserService;
    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.UnitTests.WebApi.ViewModels;
    using VolleyManagement.WebApi.Controllers;
    using VolleyManagement.WebApi.Mappers;
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
            this._kernel = new StandardKernel();
            this._kernel.Bind<IUserService>()
                   .ToConstant(this._userServiceMock.Object);
        }

        /// <summary>
        /// Test Post method
        /// </summary>
        [TestMethod]
        public void Post_NewUser_CreateMethodInvoked()
        {
            // Arrange
            _userServiceMock.Setup(us => us.Create(It.IsAny<User>())).Verifiable();
            var user = new UserBuilder().WithId(1).Build();
            var userService = _userServiceMock.Object;

            // Act
            userService.Create(user);

            // Assert
            _userServiceMock.Verify();
        }

        /// <summary>
        /// Test Post method. Basic story.
        /// </summary>
        [TestMethod]
        public void Post_ValidViewModel_UserCreated()
        {
            // Arrange
            var controller = _kernel.Get<UsersController>();
            SetControllerRequest(controller);
            var expected = new UserViewModelBuilder().Build();

            // Act
            var response = controller.Post(expected);
            var actual = GetModelFromResponse<UserViewModel>(response);

            // Assert
            _userServiceMock.Verify(us => us.Create(It.IsAny<User>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            AssertExtensions.AreEqual<UserViewModel>(expected, actual, new UserViewModelComparer());
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
