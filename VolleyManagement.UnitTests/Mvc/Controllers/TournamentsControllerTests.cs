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
        private const int TEST_TOURNAMENT_ID = 1;
        private readonly Mock<ITournamentService> _tournamentServiceMock = new Mock<ITournamentService>();
        private readonly Mock<TimeProvider> _timeMock = new Mock<TimeProvider>();
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
        /// Test for Index method. Actual tournaments (current and upcoming) are requested. Actual tournaments are returned.
        /// </summary>
        [TestMethod]
        public void Index_GetActualTournaments_ActualTournamentsAreReturned()
        {
            // Arrange
            var testData = MakeTestTournaments();
            var expectedCurrentTournaments = GetTournamentsWithState(testData, TournamentStateEnum.Current);
            var expectedUpcomingTournaments = GetTournamentsWithState(testData, TournamentStateEnum.Upcoming);
            var sut = GetSystemUnderTest();
            MockSetupGetActual(testData);

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
        /// Test for Details method. Tournament with specified identifier does not exist. HttpNotFoundResult is returned.
        /// </summary>
        [TestMethod]
        public void Details_NonExistentTournament_HttpNotFoundResultIsReturned()
        {
            // Arrange
            var sut = GetSystemUnderTest();
            MockSetupGet(null as Tournament);

            // Act
            var result = sut.Details(TEST_TOURNAMENT_ID);

            // Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        /// <summary>
        /// Test for Details method. Tournament with specified identifier exists. View model of Tournament is returned.
        /// </summary>
        [TestMethod]
        public void Details_ExistingTournament_TournamentViewModelIsReturned()
        {
            // Arrange
            var testData = MakeTestTournament(TEST_TOURNAMENT_ID);
            var expected = MakeTestTournamentViewModel(TEST_TOURNAMENT_ID);
            var sut = GetSystemUnderTest();
            MockSetupGet(testData);

            // Act
            var actual = TestExtensions.GetModel<TournamentViewModel>(sut.Details(TEST_TOURNAMENT_ID));

            // Assert
            TestHelper.AreEqual<TournamentViewModel>(expected, actual, new TournamentViewModelComparer());
        }

        /// <summary>
        /// Test for Create method (GET action). Tournament view model is requested. Tournament view model is returned.
        /// </summary>
        [TestMethod]
        public void CreateGetAction_GetTournamentViewModel_TournamentViewModelIsReturned()
        {
            // Arrange
            var expected = new TournamentViewModel();
            var sut = GetSystemUnderTest();

            // Act
            var actual = TestExtensions.GetModel<TournamentViewModel>(sut.Create());

            // Assert
            TestHelper.AreEqual<TournamentViewModel>(expected, actual, new TournamentViewModelComparer());
        }

        /// <summary>
        /// Test for Create method (POST action). Tournament view model is valid and no exception is thrown during creation.
        /// Tournament is created successfully and user is redirected to the Index page.
        /// </summary>
        [TestMethod]
        public void CreatePostAction_ValidTournamentViewModelNoException_TournamentIsCreated()
        {
            // Arrange
            var testData = MakeTestTournamentViewModel();
            var sut = GetSystemUnderTest();

            // Act
            var result = sut.Create(testData) as RedirectToRouteResult;

            // Assert
            VerifyCreate(Times.Once());
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        /// <summary>
        /// Test for Create method (POST action). Tournament view model is valid, but exception is thrown during creation.
        /// Tournament view model is returned.
        /// </summary>
        [TestMethod]
        public void CreatePostAction_ValidTournamentViewModelWithException_TournamentViewModelIsReturned()
        {
            // Arrange
            var testData = MakeTestTournamentViewModel();
            var sut = GetSystemUnderTest();
            MockSetupCreateTournamentValidationException();

            // Act
            var result = TestExtensions.GetModel<TournamentViewModel>(sut.Create(testData));

            // Assert
            VerifyCreate(Times.Once());
            Assert.IsNotNull(result, "View model must be returned to user.");
        }

        /// <summary>
        /// Test for Create method (POST action). Tournament view model is not valid.
        /// Tournament is not created and tournament view model is returned.
        /// </summary>
        [TestMethod]
        public void CreatePostAction_InvalidTournamentViewModel_TournamentViewModelIsReturned()
        {
            // Arrange
            var testData = MakeTestTournamentViewModel();
            var sut = GetSystemUnderTest();
            sut.ModelState.AddModelError("Error", "An error occurred");

            // Act
            var result = TestExtensions.GetModel<TournamentViewModel>(sut.Create(testData));

            // Assert
            VerifyCreate(Times.Never());
            Assert.IsNotNull(result, "Invalid view model must be returned to user.");
        }

        /// <summary>
        /// Test for Edit method (GET action). Tournament with specified identifier does not exist. HttpNotFoundResult is returned.
        /// </summary>
        [TestMethod]
        public void EditGetAction_NonExistentTournament_HttpNotFoundResultIsReturned()
        {
            // Arrange
            var sut = GetSystemUnderTest();
            MockSetupGet(null as Tournament);

            // Act
            var result = sut.Edit(TEST_TOURNAMENT_ID);

            // Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        /// <summary>
        /// Test for Edit method (GET action). Tournament with specified identifier exists. View model of Tournament is returned.
        /// </summary>
        [TestMethod]
        public void EditGetAction_ExistingTournament_TournamentViewModelIsReturned()
        {
            // Arrange
            var testData = MakeTestTournament(TEST_TOURNAMENT_ID);
            var expected = MakeTestTournamentViewModel(TEST_TOURNAMENT_ID);
            var sut = GetSystemUnderTest();
            MockSetupGet(testData);

            // Act
            var actual = TestExtensions.GetModel<TournamentViewModel>(sut.Edit(TEST_TOURNAMENT_ID));

            // Assert
            TestHelper.AreEqual<TournamentViewModel>(expected, actual, new TournamentViewModelComparer());
        }

        /// <summary>
        /// Test for Edit method (POST action). Tournament view model is valid and no exception is thrown during editing.
        /// Tournament is updated successfully and user is redirected to the Index page.
        /// </summary>
        [TestMethod]
        public void EditPostAction_ValidTournamentViewModelNoException_TournamentIsUpdated()
        {
            // Arrange
            var testData = MakeTestTournamentViewModel();
            var sut = GetSystemUnderTest();

            // Act
            var result = sut.Edit(testData) as RedirectToRouteResult;

            // Assert
            VerifyEdit(Times.Once());
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        /// <summary>
        /// Test for Edit method (POST action). Tournament view model is valid, but exception is thrown during editing.
        /// Tournament view model is returned.
        /// </summary>
        [TestMethod]
        public void EditPostAction_ValidTournamentViewModelWithException_TournamentViewModelIsReturned()
        {
            // Arrange
            var testData = MakeTestTournamentViewModel();
            var sut = GetSystemUnderTest();
            MockSetupEditTournamentValidationException();

            // Act
            var result = TestExtensions.GetModel<TournamentViewModel>(sut.Edit(testData));

            // Assert
            VerifyEdit(Times.Once());
            Assert.IsNotNull(result, "View model must be returned to user.");
        }

        /// <summary>
        /// Test for Edit method (POST action). Tournament view model is not valid.
        /// Tournament is not updated and tournament view model is returned.
        /// </summary>
        [TestMethod]
        public void EditPostAction_InvalidTournamentViewModel_TournamentViewModelIsReturned()
        {
            // Arrange
            var testData = MakeTestTournamentViewModel();
            var sut = GetSystemUnderTest();
            sut.ModelState.AddModelError("Error", "An error occurred");

            // Act
            var result = TestExtensions.GetModel<TournamentViewModel>(sut.Edit(testData));

            // Assert
            VerifyEdit(Times.Never());
            Assert.IsNotNull(result, "Invalid view model must be returned to user.");
        }

        /// <summary>
        /// Test for Delete method (GET action). Tournament with specified identifier does not exist. HttpNotFoundResult is returned.
        /// </summary>
        [TestMethod]
        public void DeleteGetAction_NonExistentTournament_HttpNotFoundResultIsReturned()
        {
            // Arrange
            var sut = GetSystemUnderTest();
            MockSetupGet(null as Tournament);

            // Act
            var result = sut.Delete(TEST_TOURNAMENT_ID);

            // Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        /// <summary>
        /// Test for Delete method (GET action). Tournament with specified identifier exists. View model of Tournament is returned.
        /// </summary>
        [TestMethod]
        public void DeleteGetAction_ExistingTournament_TournamentViewModelIsReturned()
        {
            // Arrange
            var testData = MakeTestTournament(TEST_TOURNAMENT_ID);
            var expected = MakeTestTournamentViewModel(TEST_TOURNAMENT_ID);
            var sut = GetSystemUnderTest();
            MockSetupGet(testData);

            // Act
            var actual = TestExtensions.GetModel<TournamentViewModel>(sut.Delete(TEST_TOURNAMENT_ID));

            // Assert
            TestHelper.AreEqual<TournamentViewModel>(expected, actual, new TournamentViewModelComparer());
        }

        /// <summary>
        /// Test for DeleteConfirmed method (delete POST action). Tournament with specified identifier does not exist.
        /// HttpNotFoundResult is returned.
        /// </summary>
        [TestMethod]
        public void DeletePostAction_NonExistentTournament_HttpNotFoundResultIsReturned()
        {
            // Arrange
            var sut = GetSystemUnderTest();
            MockSetupGet(null as Tournament);

            // Act
            var result = sut.DeleteConfirmed(TEST_TOURNAMENT_ID);

            // Assert
            VerifyDelete(Times.Never());
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        /// <summary>
        /// Test for DeleteConfirmed method (delete POST action). Tournament with specified identifier exists.
        /// Tournament is deleted successfully and user is redirected to the Index page.
        /// </summary>
        [TestMethod]
        public void DeletePostAction_ExistingTournament_TournamentIsDeleted()
        {
            // Arrange
            var testData = MakeTestTournament(TEST_TOURNAMENT_ID);
            var sut = GetSystemUnderTest();
            MockSetupGet(testData);

            // Act
            var result = sut.DeleteConfirmed(TEST_TOURNAMENT_ID) as RedirectToRouteResult;

            // Assert
            VerifyDelete(Times.Once());
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        /// <summary>
        /// Makes tournaments filled with test data.
        /// </summary>
        /// <returns>List of tournaments with test data.</returns>
        private List<Tournament> MakeTestTournaments()
        {
            return new TournamentServiceTestFixture().TestTournaments().Build();
        }

        /// <summary>
        /// Makes tournament with specified identifier filled with test data.
        /// </summary>
        /// <param name="tournamentId">Identifier of the tournament.</param>
        /// <returns>Tournament filled with test data.</returns>
        private Tournament MakeTestTournament(int tournamentId)
        {
            return new TournamentBuilder().WithId(tournamentId).Build();
        }

        /// <summary>
        /// Makes tournament view model filled with test data.
        /// </summary>
        /// <returns>Tournament view model filled with test data.</returns>
        private TournamentViewModel MakeTestTournamentViewModel()
        {
            return new TournamentMvcViewModelBuilder().Build();
        }

        /// <summary>
        /// Makes tournament view model with specified tournament identifier filled with test data.
        /// </summary>
        /// <param name="tournamentId">Identifier of the tournament.</param>
        /// <returns>Tournament view model filled with test data.</returns>
        private TournamentViewModel MakeTestTournamentViewModel(int tournamentId)
        {
            return new TournamentMvcViewModelBuilder().WithId(tournamentId).Build();
        }

        /// <summary>
        /// Gets tournaments with specified state.
        /// </summary>
        /// <param name="tournaments">List of tournaments to filter.</param>
        /// <param name="state">Tournament state.</param>
        /// <returns>List of tournaments with specified state.</returns>
        private List<Tournament> GetTournamentsWithState(List<Tournament> tournaments, TournamentStateEnum state)
        {
            return tournaments.Where(tr => tr.State == state).ToList();
        }

        /// <summary>
        /// Gets system being tested by a unit test.
        /// </summary>
        /// <returns>System being tested by a unit test.</returns>
        private TournamentsController GetSystemUnderTest()
        {
            return this._kernel.Get<TournamentsController>();
        }

        /// <summary>
        /// Sets up a mock for GetActual method of Tournament service to return specified tournaments.
        /// </summary>
        /// <param name="tournaments">Tournament that will be returned by GetActual method of Tournament service.</param>
        private void MockSetupGetActual(List<Tournament> tournaments)
        {
            this._tournamentServiceMock.Setup(tr => tr.GetActual()).Returns(tournaments);
        }

        /// <summary>
        /// Sets up a mock for Get method of Tournament service with any parameter to return specified tournament.
        /// </summary>
        /// <param name="tournament">Tournament that will be returned by Get method of Tournament service.</param>
        private void MockSetupGet(Tournament tournament)
        {
            this._tournamentServiceMock.Setup(tr => tr.Get(It.IsAny<int>())).Returns(tournament);
        }

        /// <summary>
        /// Sets up a mock for Create method of Tournament service to throw TournamentValidationException.
        /// </summary>
        private void MockSetupCreateTournamentValidationException()
        {
            this._tournamentServiceMock.Setup(ts => ts.Create(It.IsAny<Tournament>()))
                .Throws(new TournamentValidationException("Message", "ValidationKey", "ParamName"));
        }

        /// <summary>
        /// Sets up a mock for Edit method of Tournament service to throw TournamentValidationException.
        /// </summary>
        private void MockSetupEditTournamentValidationException()
        {
            this._tournamentServiceMock.Setup(ts => ts.Edit(It.IsAny<Tournament>()))
                .Throws(new TournamentValidationException("Message", "ValidationKey", "ParamName"));
        }

        /// <summary>
        /// Verifies that tournament is created required number of times.
        /// </summary>
        /// <param name="times">Number of times tournament must be created.</param>
        private void VerifyCreate(Times times)
        {
            this._tournamentServiceMock.Verify(ts => ts.Create(It.IsAny<Tournament>()), times);
        }

        /// <summary>
        /// Verifies that tournament is updated required number of times.
        /// </summary>
        /// <param name="times">Number of times tournament must be updated.</param>
        private void VerifyEdit(Times times)
        {
            this._tournamentServiceMock.Verify(ts => ts.Edit(It.IsAny<Tournament>()), times);
        }

        /// <summary>
        /// Verifies that tournament is deleted required number of times.
        /// </summary>
        /// <param name="times">Number of times tournament must be deleted.</param>
        private void VerifyDelete(Times times)
        {
            this._tournamentServiceMock.Verify(ts => ts.Delete(It.IsAny<int>()), times);
        }
    }
}
