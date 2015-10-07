namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using Contracts;
    using Contracts.Exceptions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;

    using VolleyManagement.Crosscutting.Contracts.Providers;
    using VolleyManagement.Domain;
    using VolleyManagement.Domain.TournamentsAggregate;
    using VolleyManagement.UI.Areas.Mvc.Controllers;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments;
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
        /// TimeProvider mock
        /// </summary>
        private readonly Mock<TimeProvider> _timeMock = new Mock<TimeProvider>();

        private readonly TournamentServiceTestFixture _testFixture = new TournamentServiceTestFixture();
        private readonly Mock<ITournamentService> _tournamentServiceMock = new Mock<ITournamentService>();
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
            this._timeMock.SetupGet(tp => tp.UtcNow).Returns(new DateTime(2015, 04, 01));
            TimeProvider.Current = this._timeMock.Object;
        }

        /// <summary>
        /// Cleanup test data
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            TimeProvider.ResetToDefault();
        }

        /// <summary>
        /// Index action test. The method should invoke GetActual() method of ITournamentService
        /// </summary>
        public void Index_ActualTournamentsRequest_GetActualCalled()
        {
            // Act
            var sut = this._kernel.Get<TournamentsController>();
            sut.Index();

            // Assert
            _tournamentServiceMock.Verify(m => m.GetActual(), Times.Once());
        }

        /// <summary>
        /// Index action test. The action should return TournamentsCollectionsViewModel
        /// with two correct collections of tournaments: Current and Upcoming
        /// </summary>
        [TestMethod]
        public void Index_ActualTournamentsRequest_CorrectCollectionsReturned()
        {
            // Arrange
            var testData = _testFixture.TestTournaments().Build();

            this._tournamentServiceMock.Setup(tr => tr.GetActual())
                .Returns(testData.AsQueryable());

            var sut = this._kernel.Get<TournamentsController>();

            DateTime now = TimeProvider.Current.UtcNow;

            var expectedCurrentTournaments = new TournamentServiceTestFixture()
                                            .TestTournaments()
                                            .Build().Where(tr => tr.GamesStart <= now
                                                && tr.GamesEnd >= now)
                                            .ToList();

            var expectedUpcomingTournaments = new TournamentServiceTestFixture()
                                            .TestTournaments()
                                            .Build().Where(tr => tr.GamesStart > now
                                                && tr.GamesStart <= now.AddMonths(
                                                Constants.Tournament.UPCOMING_TOURNAMENTS_MONTH_LIMIT))
                                            .ToList();

            // Act
            var actualCurrentTournaments = TestExtensions.GetModel<TournamentsCollectionsViewModel>(sut.Index())
                .CurrentTournaments.ToList();
            var actualUpcomingTournaments = TestExtensions.GetModel<TournamentsCollectionsViewModel>(sut.Index())
                .UpcomingTournaments.ToList();

            // Assert
            CollectionAssert.AreEqual(expectedCurrentTournaments, actualCurrentTournaments, new TournamentComparer());
            CollectionAssert.AreEqual(expectedUpcomingTournaments, actualUpcomingTournaments, new TournamentComparer());
        }

        /// <summary>
        /// Test for Details()
        /// </summary>
        [TestMethod]
        public void Details_TournamentDoesNotExist_NotFoundResult()
        {
            // Arrange
            this._tournamentServiceMock.Setup(tr => tr.Get(It.IsAny<int>()))
                .Throws(new InvalidOperationException());

            var sut = this._kernel.Get<TournamentsController>();
            var expected = (int)HttpStatusCode.NotFound;

            // Act
            var actual = (sut.Details(It.IsAny<int>()) as HttpNotFoundResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test for Details()
        /// </summary>
        [TestMethod]
        public void Details_TournamentExists_TournamentIsReturned()
        {
            // Arrange
            int searchId = 11;

            _tournamentServiceMock.Setup(tr => tr.Get(It.IsAny<int>()))
                .Returns(new TournamentBuilder()
                .WithId(11)
                .WithName("Tournament 11")
                .WithDescription("Tournament 11 description")
                .WithSeason(2014)
                .WithScheme(TournamentSchemeEnum.Two)
                .WithRegulationsLink("www.Volleyball.dp.ua/Regulations/Tournaments('11')")
                .Build());

            var controller = this._kernel.Get<TournamentsController>();

            var expected = new TournamentBuilder()
                .WithId(searchId)
                .WithName("Tournament 11")
                .WithDescription("Tournament 11 description")
                .WithSeason(2014)
                .WithScheme(TournamentSchemeEnum.Two)
                .WithRegulationsLink("www.Volleyball.dp.ua/Regulations/Tournaments('11')")
                .Build();

            // Act
            var actual = TestExtensions.GetModel<Tournament>(controller.Details(searchId));

            // Assert
            AssertExtensions.AreEqual<Tournament>(expected, actual, new TournamentComparer());
        }

        /// <summary>
        /// Test for Delete tournament action
        /// </summary>
        [TestMethod]
        public void DeleteGetAction_Tournament_ReturnsToTheView()
        {
            // Arrange
            var controller = _kernel.Get<TournamentsController>();
            var tournament = new TournamentBuilder()
                            .WithId(1)
                            .WithName("MyTournament")
                            .WithDescription("Hello!")
                            .WithScheme(TournamentSchemeEnum.Two)
                            .WithSeason(2016)
                            .WithRegulationsLink("google.com.ua")
                            .Build();
            MockSingleTournament(tournament);
            var expected = new TournamentBuilder()
                                        .WithId(1)
                                        .WithName("MyTournament")
                                        .WithDescription("Hello!")
                                        .WithScheme(TournamentSchemeEnum.Two)
                                        .WithSeason(2016)
                                        .WithRegulationsLink("google.com.ua")
                                        .Build();

            // Act
            var actual = TestExtensions.GetModel<Tournament>(controller.Delete(tournament.Id));

            // Assert
            AssertExtensions.AreEqual<Tournament>(expected, actual, new TournamentComparer());
        }

        /// <summary>
        /// Test for DeleteConfirmed method. The method should invoke Delete() method of ITournamentService
        /// and redirect to Index.
        /// </summary>
        public void DeleteConfirmed_TournamentExists_TournamentIsDeleted()
        {
            // Arrange
            int tournamentIdToDelete = 4;

            // Act
            var sut = this._kernel.Get<TournamentsController>();
            var actual = sut.DeleteConfirmed(tournamentIdToDelete) as RedirectToRouteResult;

            // Assert
            _tournamentServiceMock.Verify(m => m.Delete(It.Is<int>(id => id == tournamentIdToDelete)), Times.Once());
            Assert.AreEqual("Index", actual.RouteValues["action"]);
        }

        /// <summary>
        /// Test for DeleteConfirmed method where input parameter is tournament id, which doesn't exist in database.
        /// The method should return HttpNotFound.
        /// </summary>
        [TestMethod]
        public void DeleteConfirmed_TournamentDoesntExist_HttpNotFoundReturned()
        {
            // Arrange
            int tournamentIdToDelete = 4;
            _tournamentServiceMock.Setup(ts => ts.Delete(4)).Throws<InvalidOperationException>();

            // Act
            var sut = this._kernel.Get<TournamentsController>();
            var actual = sut.DeleteConfirmed(tournamentIdToDelete);

            // Assert
            Assert.IsInstanceOfType(actual, typeof(HttpNotFoundResult));
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
            var actual = TestExtensions.GetModel<TournamentViewModel>(controller.Create());

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
                .WithSeason(2015)
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
            controller.ModelState.AddModelError("Key", "ModelIsInvalidNow");
            var tournamentViewModel = new TournamentMvcViewModelBuilder()
                .WithName(string.Empty)
                .Build();

            // Act
            var actual = TestExtensions.GetModel<TournamentViewModel>(controller.Create(tournamentViewModel));

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
                .WithSeason(2015)
                .Build();

            _tournamentServiceMock.Setup(ts => ts.Create(It.IsAny<Tournament>()))
                .Throws(new TournamentValidationException("Message", "ValidationKey", "paramName"));
            var controller = _kernel.Get<TournamentsController>();

            // Act
            var actual = TestExtensions.GetModel<TournamentViewModel>(controller.Create(tournamentViewModel));

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
                .WithSeason(2015)
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
                            .WithSeason(2016)
                            .WithRegulationsLink("volley.dp.ua")
                            .Build();
            MockSingleTournament(tournament);
            var expected = new TournamentMvcViewModelBuilder()
                                        .WithId(1)
                                        .WithName("test tournament")
                                        .WithDescription("Volley")
                                        .WithScheme(TournamentSchemeEnum.Two)
                                        .WithSeason(2016)
                                        .WithRegulationsLink("volley.dp.ua")
                                        .Build();

            // Act
            var actual = TestExtensions.GetModel<TournamentViewModel>(controller.Edit(tournament.Id));

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
            _tournamentServiceMock.Setup(ts => ts.Get(tournamentId))
               .Throws(new Exception());
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
                .WithSeason(2015)
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
            controller.ModelState.AddModelError("Key", "ModelIsInvalidNow");
            var tournamentViewModel = new TournamentMvcViewModelBuilder()
                .WithName(string.Empty)
                .Build();

            // Act
            var actual = TestExtensions.GetModel<TournamentViewModel>(controller.Edit(tournamentViewModel));

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
                .WithSeason(2015)
                .Build();
            _tournamentServiceMock.Setup(ts => ts.Edit(It.IsAny<Tournament>()))
                .Throws(new TournamentValidationException());
            var controller = _kernel.Get<TournamentsController>();

            // Act
            var actual = TestExtensions.GetModel<TournamentViewModel>(controller.Edit(tournamentViewModel));

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
                .WithSeason(2015)
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
            this._tournamentServiceMock.Setup(tr => tr.Get())
                .Returns(testData.AsQueryable());
        }

        /// <summary>
        /// Mocks test data
        /// </summary>
        /// <param name="testData">Data to mock</param>
        private void MockSingleTournament(Tournament testData)
        {
            _tournamentServiceMock.Setup(tr => tr.Get(testData.Id)).Returns(testData);
        }
    }
}