namespace VolleyManagement.UnitTests.WebApi.Controllers
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
        /// Users Service Mock
        /// </summary>
        private readonly Mock<IUserService> _userServiceMock = new Mock<IUserService>();

        /// <summary>
        /// Tournaments Repository Mock
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
            throw new NotImplementedException();
        }
    }
}
