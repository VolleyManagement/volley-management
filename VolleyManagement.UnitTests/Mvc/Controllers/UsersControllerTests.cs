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
            var testData = this._testFixture.TestUsers()
                                       .Build();
            this.MockUsers(testData);

            var usersController = this._kernel.Get<UsersController>();

            var expected = new UserServiceTestFixture()
                                            .TestUsers()
                                            .Build()
                                            .ToList();

            // Act
            var viewResult = usersController.Index() as ViewResult;
            var actual = (IEnumerable<User>)viewResult.ViewData.Model;

            // Assert
            CollectionAssert.AreEqual(expected, actual.ToList(), new UserComparer());
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
        /// Test for Create user action (GET)
        /// </summary>
        [TestMethod]
        public void Create_GetView_ReturnsViewWithDefaultData()
        {
            // Arrange
            var controller = _kernel.Get<UsersController>();
            var expected = new UserViewModel();

            // Act
            var actual = GetModel<UserViewModel>(controller.Create());

            // Assert
            AssertExtensions.AreEqual<UserViewModel>(expected, actual, new UserViewModelComparer());
        }

        /// <summary>
        /// Test for Create user action (POST)
        /// </summary>
        [TestMethod]
        public void CreatePostAction_ValidUserViewModel_RedirectToIndex()
        {
            // Arrange
            var usersController = _kernel.Get<UsersController>();
            var userViewModel = new UserMvcViewModelBuilder()
                .WithUserName("testLoginB")
                .WithFullName("Test Name B")
                .WithEmail("test2@gmail.com")
                .WithPassword("abc222")
                .WithCellPhone("0500000002")
                .Build();

            // Act
            var result = usersController.Create(userViewModel) as RedirectToRouteResult;

            // Assert
            _userServiceMock.Verify(us => us.Create(It.IsAny<User>()), Times.Once());
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        /// <summary>
        /// Test for Create user action with invalid view model (POST)
        /// </summary>
        [TestMethod]
        public void CreatePostAction_InvalidUserViewModel_ReturnsViewModelToView()
        {
            // Arrange
            var controller = _kernel.Get<UsersController>();
            controller.ModelState.AddModelError("Key", "ModelIsInvalidNow");
            var userViewModel = new UserMvcViewModelBuilder()
                .WithUserName(string.Empty)
                .Build();

            // Act
            var actual = GetModel<UserViewModel>(controller.Create(userViewModel));

            // Assert
            _userServiceMock.Verify(us => us.Create(It.IsAny<User>()), Times.Never());
            Assert.IsNotNull(actual, "Model with incorrect data should be returned to the view.");
        }

        /// <summary>
        /// Test for Create user action (POST)
        /// </summary>
        [TestMethod]
        public void CreatePostAction_ArgumentException_ExceptionThrown()
        {
            // Arrange
            var userViewModel = new UserMvcViewModelBuilder()
                .WithId(1)
                .WithUserName("testLogin2")
                .WithFullName("Test Name 1")
                .WithEmail("test3@gmail.com")
                .WithPassword("abc222")
                .WithCellPhone("+38(050)0000002")
                .Build();
            _userServiceMock.Setup(ts => ts.Create(It.IsAny<User>()))
                .Throws(new ArgumentException());
            var controller = _kernel.Get<UsersController>();

            // Act
            var actual = GetModel<UserViewModel>(controller.Create(userViewModel));

            // Assert
            Assert.IsNotNull(actual, "Model with incorrect data should be returned to the view.");
        }

        /// <summary>
        /// Test for Create user action (POST)
        /// </summary>
        [TestMethod]
        public void CreatePostAction_GeneralException_ExceptionThrown()
        {
            // Arrange
            var userViewModel = new UserMvcViewModelBuilder()
                .WithId(1)
                .WithUserName("testLoginC")
                .WithFullName("Test Name A")
                .WithEmail("test2@gmail.com")
                .WithPassword("abc222")
                .WithCellPhone("0500000002")
                .Build();
            _userServiceMock.Setup(ts => ts.Create(It.IsAny<User>()))
                .Throws(new Exception());
            var controller = _kernel.Get<UsersController>();

            // Act
            var actual = controller.Create(userViewModel);

            // Assert
            Assert.IsInstanceOfType(actual, typeof(HttpNotFoundResult));
        }
        
        /// <summary>
        /// Mocks test data
        /// </summary>
        /// <param name="testData">Data to mock</param>
        private void MockUsers(IEnumerable<User> testData)
        {
            this._userServiceMock.Setup(tr => tr.GetAll())
                .Returns(testData.AsQueryable());
        }

        /// <summary>
        /// Get generic T model by ViewResult from action view
        /// </summary>
        /// <typeparam name="T">model type</typeparam>
        /// <param name="result">object to convert and return</param>
        /// <returns>T result by ViewResult from action view</returns>
        private T GetModel<T>(object result) where T : class
        {
            return (T)(result as ViewResult).ViewData.Model;
        }
    }
}
