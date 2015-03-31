namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;

    using VolleyManagement.Contracts;
    using VolleyManagement.Domain.Players;
    using VolleyManagement.UI.Areas.Mvc.Controllers;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Players;
    using VolleyManagement.UnitTests.Mvc.ViewModels;

    /// <summary>
    /// MVC PlayerController tests class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class PlayerControllerTests
    {
        private readonly Mock<IPlayerService> _playerServiceMock = new Mock<IPlayerService>();

        private IKernel _kernel;

        /// <summary>
        /// Initializes test data
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            this._kernel = new StandardKernel();
            this._kernel.Bind<IPlayerService>()
                   .ToConstant(this._playerServiceMock.Object);
        }

        /// <summary>
        /// Test for Create player action (GET)
        /// </summary>
        [TestMethod]
        public void Create_GetView_ReturnsViewWithDefaultData()
        {
            // Arrange
            var controller = _kernel.Get<PlayersController>();
            var expected = new PlayerViewModel();

            // Act
            var actual = TestExtensions.GetModel<PlayerViewModel>(controller.Create());

            // Assert
            AssertExtensions.AreEqual<PlayerViewModel>(expected, actual, new PlayerViewModelComparer());
        }

        /// <summary>
        /// Create player action test (POST)
        /// </summary>
        [TestMethod]
        public void CreatePostAction_ValidPlayerViewModel_RedirectToIndex()
        {
            // Arrange
            var playerController = _kernel.Get<PlayersController>();
            var playerViewModel = new PlayerMvcViewModelBuilder()
                .WithFirstName("FirstName")
                .WithLastName("LastName")
                .WithBirthYear(1983)
                .WithHeight(186)
                .WithWeight(95)
                .Build();

            // Act
            var result = playerController.Create(playerViewModel) as RedirectToRouteResult;

            // Assert
            _playerServiceMock.Verify(ts => ts.Create(It.IsAny<Player>()), Times.Once());
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        /// <summary>
        /// Create player action test with an invalid view model (POST)
        /// </summary>
        [TestMethod]
        public void CreatePostAction_InvalidPlayerViewModel_ReturnsViewModelToView()
        {
            // Arrange
            var controller = _kernel.Get<PlayersController>();
            controller.ModelState.AddModelError("Key", "ModelIsInvalidNow");
            var playerViewModel = new PlayerMvcViewModelBuilder()
                .WithFirstName("")
                .Build();

            // Act
            var actual = TestExtensions.GetModel<PlayerViewModel>(controller.Create(playerViewModel));

            // Assert
            _playerServiceMock.Verify(ps => ps.Create(It.IsAny<Player>()), Times.Never());
            Assert.IsNotNull(actual, "Model with incorrect data should be returned to the view.");
        }

        /// <summary>
        /// Create player action test (POST)
        /// </summary>
        [TestMethod]
        public void CreatePostAction_ArgumentException_ExceptionThrown()
        {
            // Arrange
            var playerViewModel = new PlayerMvcViewModelBuilder().Build();

            _playerServiceMock.Setup(ps => ps.Create(It.IsAny<Player>()))
                .Throws(new PlayerValidationException());
            var controller = _kernel.Get<PlayersController>();

            // Act
            var actual = TestExtensions.GetModel<PlayerViewModel>(controller.Create(playerViewModel));

            // Assert
            Assert.IsNotNull(actual, "Model with incorrect data should be returned to the view.");
        }

        /// <summary>
        /// Create player action test (POST)
        /// </summary>
        [TestMethod]
        public void CreatePostAction_ArgumentException_ExceptionThrown()
        {
            // Arrange
            var playerViewModel = new PlayerMvcViewModelBuilder().Build();

            _playerServiceMock.Setup(ps => ps.Create(It.IsAny<Player>()))
                .Throws(new ArgumentException());
            var controller = _kernel.Get<PlayersController>();

            // Act
            var actual = controller.Create(playerViewModel);

            // Assert
            Assert.IsInstanceOfType(actual, typeof(HttpNotFoundResult));
        }
    }
}
