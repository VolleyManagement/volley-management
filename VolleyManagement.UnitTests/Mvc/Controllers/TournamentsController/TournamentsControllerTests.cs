namespace VolleyManagement.UnitTests.Mvc.Controllers.TournamentsController
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using Contracts;
    using Domain.Tournaments;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Mvc.Controllers;
    using VolleyManagement.Mvc.ViewModels.Tournaments;
    using VolleyManagement.UnitTests.Mvc.Mappers;
    using VolleyManagement.UnitTests.Mvc.ViewModels;
    using VolleyManagement.UnitTests.Services.TournamentService;

    /// <summary>
    /// Tests for MVC TournamentController class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TournamentsControllerTests
    {
        /// <summary>
        /// Test Fixture
        /// </summary>
        private readonly TournamentServiceTestFixture _testFixture =
            new TournamentServiceTestFixture();

        /// <summary>
        /// Tournaments Service Mock
        /// </summary>
        private readonly Mock<ITournamentService> _tournamentServiceMock =
            new Mock<ITournamentService>();

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
            this._kernel.Bind<ITournamentService>()
                   .ToConstant(this._tournamentServiceMock.Object);
        }

        /// <summary>
        /// Test for Index action. The action should return not empty tournaments list
        /// </summary>
        [TestMethod]
        public void Index_TournamentsExist_TournamentsReturned()
        {
            // Arrange
            var testData = this._testFixture.TestTournaments()
                                       .Build();
            this.MockTournaments(testData);

            var sut = this._kernel.Get<TournamentsController>();

            var expected = new TournamentServiceTestFixture()
                                            .TestTournaments()
                                            .Build()
                                            .ToList();

            // Act
            var actual = GetModel<IEnumerable<Tournament>>(sut.Index());

            // Assert
            CollectionAssert.AreEqual(expected, actual.ToList(), new TournamentComparer());
        }

        /// <summary>
        /// Test with negative scenario for Index action.
        /// The action should thrown Argument null exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void Index_TournamentsDoNotExist_ExceptionThrown()
        {
            // Arrange
            this._tournamentServiceMock.Setup(tr => tr.GetAll())
                .Throws(new ArgumentNullException());

            var tournamentService = _tournamentServiceMock.Object;

            // Act
            tournamentService.GetAll();

            // Assert is handled by ExpectedException attribute
        }

        /// <summary>
        /// Test for Details()
        /// </summary>
        [TestMethod]
        public void Details_TournamentExists_TournamentIsReturned()
        {
            // Arrange
            int searchId = 11;

            _tournamentServiceMock.Setup(tr => tr.FindById(It.IsAny<int>()))
                .Returns(new TournamentBuilder()
                .WithId(11)
                .WithName("Tournament 11")
                .WithDescription("Tournament 11 description")
                .WithSeason("2014/2015")
                .WithScheme(TournamentSchemeEnum.Two)
                .WithRegulationsLink("www.Volleyball.dp.ua/Regulations/Tournaments('11')")
                .Build());

            var controller = this._kernel.Get<TournamentsController>();

            var expected = new TournamentBuilder()
                .WithId(searchId)
                .WithName("Tournament 11")
                .WithDescription("Tournament 11 description")
                .WithSeason("2014/2015")
                .WithScheme(TournamentSchemeEnum.Two)
                .WithRegulationsLink("www.Volleyball.dp.ua/Regulations/Tournaments('11')")
                .Build();

            // Act
            var actual = GetModel<Tournament>(controller.Details(searchId));

            // Assert
            AssertExtensions.AreEqual<Tournament>(expected, actual, new TournamentComparer());
        }

        /// <summary>
        /// Test for Delete tournament action
        /// </summary>
        [TestMethod]
        public void Delete_TournamentExists_TournamentIsDeleted()
        {
            var testData = this._testFixture.TestTournaments()
                                      .Build();
            var tournamentToDelete = testData.Last().Id;
            var tournamentService = _tournamentServiceMock.Object;

            tournamentService.Delete(tournamentToDelete);

            _tournamentServiceMock.Verify(m => m.Delete(tournamentToDelete));
        }

        /// <summary>
        /// Test for Create tournament action (GET)
        /// </summary>
        [TestMethod]
        public void Create_GetView_ReturnsViewWithDefaultData()
        {
            // Arrange
            var controller = _kernel.Get<TournamentsController>();
            var expected = new TournamentViewModel();

            // Act
            var actual = GetModel<TournamentViewModel>(controller.Create());

            // Assert
            AssertExtensions.AreEqual<TournamentViewModel>(expected, actual, new TournamentViewModelComparer());
        }

        /// <summary>
        /// Test for Create tournament action (POST)
        /// </summary>
        [TestMethod]
        public void CreatePostAction_ValidTournamentViewModel_RedirectToIndex()
        {
            // Arrange
            var tournamentsController = _kernel.Get<TournamentsController>();
            var tournamentViewModel = new TournamentMvcViewModelBuilder()
                .WithName("testName")
                .WithScheme(TournamentSchemeEnum.Two)
                .WithSeason("2015/2016")
                .Build();

            // Act
            var result = tournamentsController.Create(tournamentViewModel) as RedirectToRouteResult;

            // Assert
            _tournamentServiceMock.Verify(ts => ts.Create(It.IsAny<Tournament>()), Times.Once());
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        /// <summary>
        /// Test for Create tournament action with invalid view model (POST)
        /// </summary>
        [TestMethod]
        public void CreatePostAction_InvalidTournamentViewModel_ReturnsViewModelToView()
        {
            // Arrange
            var controller = _kernel.Get<TournamentsController>();
            var tournamentViewModel = new TournamentMvcViewModelBuilder()
                .WithName(null)
                .Build();

            // Act
            var actual = GetModel<TournamentViewModel>(controller.Create(tournamentViewModel));

            // Assert
            _tournamentServiceMock.Verify(ts => ts.Create(It.IsAny<Tournament>()), Times.Never());
            Assert.IsNotNull(actual, "Model with incorrect data should be returned to the view.");
        }

        /// <summary>
        /// Test for Create tournament action (POST)
        /// </summary>
        [TestMethod]
        public void CreatePostAction_ArgumentException_ExceptionThrown()
        {
            // Arrange
            var tournamentViewModel = new TournamentMvcViewModelBuilder()
                .WithId(1)
                .WithName("testName")
                .WithScheme(TournamentSchemeEnum.Two)
                .WithSeason("2015/2016")
                .Build();
            _tournamentServiceMock.Setup(ts => ts.Create(It.IsAny<Tournament>()))
                .Throws(new ArgumentException());
            var controller = _kernel.Get<TournamentsController>();

            // Act
            var actual = GetModel<TournamentViewModel>(controller.Create(tournamentViewModel));

            // Assert
            Assert.IsNotNull(actual, "Model with incorrect data should be returned to the view.");
        }

        /// <summary>
        /// Test for Create tournament action (POST)
        /// </summary>
        [TestMethod]
        public void CreatePostAction_GeneralException_ExceptionThrown()
        {
            // Arrange
            var tournamentViewModel = new TournamentMvcViewModelBuilder()
                .WithId(1)
                .WithName("testName")
                .WithScheme(TournamentSchemeEnum.Two)
                .WithSeason("2015/2016")
                .Build();
            _tournamentServiceMock.Setup(ts => ts.Create(It.IsAny<Tournament>()))
                .Throws(new Exception());
            var controller = _kernel.Get<TournamentsController>();

            // Act
            var actual = controller.Create(tournamentViewModel);

            // Assert
            Assert.IsInstanceOfType(actual, typeof(HttpNotFoundResult));
        }

        /// <summary>
        /// Test for Edit tournament action (GET)
        /// </summary>
        [TestMethod]
        public void EditGetAction_TournamentViewModel_ReturnsToTheView()
        {
            // Arrange
            var controller = _kernel.Get<TournamentsController>();
            var tournament = new TournamentBuilder()
                            .WithId(1)
                            .WithName("test tournament")
                            .WithDescription("Volley")
                            .WithScheme(TournamentSchemeEnum.Two)
                            .WithSeason("2016/2017")
                            .WithRegulationsLink("volley.dp.ua")
                            .Build();
            MockSingleTournament(tournament);
            var expected = new TournamentMvcViewModelBuilder()
                                        .WithId(1)
                                        .WithName("test tournament")
                                        .WithDescription("Volley")
                                        .WithScheme(TournamentSchemeEnum.Two)
                                        .WithSeason("2016/2017")
                                        .WithRegulationsLink("volley.dp.ua")
                                        .Build();

            // Act
            var actual = GetModel<TournamentViewModel>(controller.Edit(tournament.Id));

            // Assert
            AssertExtensions.AreEqual<TournamentViewModel>(expected, actual, new TournamentViewModelComparer());
        }

        /// <summary>
        /// Test for Edit tournament action (GET)
        /// </summary>
        [TestMethod]
        public void EditGetAction_GeneralException_ExceptionThrown()
        {
            // Arrange
            var tournamentId = 5;
            _tournamentServiceMock.Setup(ts => ts.FindById(tournamentId))
               .Throws(new ArgumentNullException());
            var controller = _kernel.Get<TournamentsController>();

            // Act
            var actual = controller.Edit(tournamentId);

            // Assert
            Assert.IsInstanceOfType(actual, typeof(HttpNotFoundResult));
        }

        /// <summary>
        /// Test for Edit tournament action (POST)
        /// </summary>
        [TestMethod]
        public void EditPostAction_ValidTournamentViewModel_RedirectToIndex()
        {
            // Arrange
            var tournamentsController = _kernel.Get<TournamentsController>();
            var tournamentViewModel = new TournamentMvcViewModelBuilder()
                .WithId(1)
                .WithName("testName")
                .WithScheme(TournamentSchemeEnum.Two)
                .WithSeason("2015/2016")
                .Build();

            // Act
            var result = tournamentsController.Edit(tournamentViewModel) as RedirectToRouteResult;

            // Assert
            _tournamentServiceMock.Verify(ts => ts.Edit(It.IsAny<Tournament>()), Times.Once());
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        /// <summary>
        /// Test for Edit tournament action with invalid model (POST)
        /// </summary>
        [TestMethod]
        public void EditPostAction_InvalidTournamentViewModel_ReturnsViewModelToView()
        {
            // Arrange
            var controller = _kernel.Get<TournamentsController>();
            var tournamentViewModel = new TournamentMvcViewModelBuilder()
                .WithName(string.Empty)
                .Build();

            // Act
            var actual = GetModel<TournamentViewModel>(controller.Edit(tournamentViewModel));

            // Assert
            _tournamentServiceMock.Verify(ts => ts.Edit(It.IsAny<Tournament>()), Times.Never());
            Assert.IsNotNull(actual, "Model with incorrect data should be returned to the view.");
        }

        /// <summary>
        /// Test for Edit tournament action (POST)
        /// </summary>
        [TestMethod]
        public void EditPostAction_ArgumentException_ExceptionThrown()
        {
            // Arrange
            var tournamentViewModel = new TournamentMvcViewModelBuilder()
                .WithId(1)
                .WithName("testName")
                .WithScheme(TournamentSchemeEnum.Two)
                .WithSeason("2015/2016")
                .Build();
            _tournamentServiceMock.Setup(ts => ts.Edit(It.IsAny<Tournament>()))
                .Throws(new ArgumentException());
            var controller = _kernel.Get<TournamentsController>();

            // Act
            var actual = GetModel<TournamentViewModel>(controller.Edit(tournamentViewModel));

            // Assert
            Assert.IsNotNull(actual, "Model with incorrect data should be returned to the view.");
        }

        /// <summary>
        /// Test for Edit tournament action (POST)
        /// </summary>
        [TestMethod]
        public void EditPostAction_GeneralException_ExceptionThrown()
        {
            // Arrange
            var tournamentViewModel = new TournamentMvcViewModelBuilder()
                .WithId(1)
                .WithName("testName")
                .WithScheme(TournamentSchemeEnum.Two)
                .WithSeason("2015/2016")
                .Build();
            _tournamentServiceMock.Setup(ts => ts.Edit(It.IsAny<Tournament>()))
                .Throws(new Exception());
            var controller = _kernel.Get<TournamentsController>();

            // Act
            var actual = controller.Edit(tournamentViewModel);

            // Assert
            Assert.IsInstanceOfType(actual, typeof(HttpNotFoundResult));
        }

        /// <summary>
        /// Mocks test data
        /// </summary>
        /// <param name="testData">Data to mock</param>
        private void MockTournaments(IEnumerable<Tournament> testData)
        {
            this._tournamentServiceMock.Setup(tr => tr.GetAll())
                .Returns(testData.AsQueryable());
        }

        /// <summary>
        /// Mocks test data
        /// </summary>
        /// <param name="testData">Data to mock</param>
        private void MockSingleTournament(Tournament testData)
        {
            _tournamentServiceMock.Setup(tr => tr.FindById(testData.Id)).Returns(testData);
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