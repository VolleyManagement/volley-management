namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using Contracts;
    using Contracts.Exceptions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
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
        private const string ASSERT_FAIL_VIEW_MODEL_MESSAGE = "View model must be returned to user.";
        private const string ASSERT_FAIL_JSON_RESULT_MESSAGE = "Json result must be returned to user.";
        private const string INDEX_ACTION_NAME = "Index";
        private const string ROUTE_VALUES_KEY = "action";

        private readonly Mock<ITournamentService> _tournamentServiceMock = new Mock<ITournamentService>();

        private IKernel _kernel;
        private TournamentsController _sut;

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            this._kernel = new StandardKernel();
            this._kernel.Bind<ITournamentService>().ToConstant(this._tournamentServiceMock.Object);
            this._sut = this._kernel.Get<TournamentsController>();
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
            MockSetupGetActual(testData);

            // Act
            var actualCurrentTournaments = TestExtensions.GetModel<TournamentsCollectionsViewModel>(this._sut.Index())
                .CurrentTournaments.ToList();
            var actualUpcomingTournaments = TestExtensions.GetModel<TournamentsCollectionsViewModel>(this._sut.Index())
                .UpcomingTournaments.ToList();

            // Assert
            CollectionAssert.AreEqual(expectedCurrentTournaments, actualCurrentTournaments, new TournamentComparer());
            CollectionAssert.AreEqual(expectedUpcomingTournaments, actualUpcomingTournaments, new TournamentComparer());
        }

        /// <summary>
        /// Test for GetFinished method. Finished tournaments are requested. JsonResult with finished tournaments is returned.
        /// </summary>
        [TestMethod]
        public void GetFinished_GetFinishedTournaments_JsonResultIsReturned()
        {
            // Arrange
            var testData = MakeTestTournaments();
            MockSetupGetFinished(testData);

            // Act
            var result = this._sut.GetFinished();

            // Assert
            Assert.IsNotNull(result, ASSERT_FAIL_JSON_RESULT_MESSAGE);
        }

        /// <summary>
        /// Test for Details method. Tournament with specified identifier does not exist. HttpNotFoundResult is returned.
        /// </summary>
        [TestMethod]
        public void Details_NonExistentTournament_HttpNotFoundResultIsReturned()
        {
            // Arrange
            MockSetupGet(null as Tournament);

            // Act
            var result = this._sut.Details(TEST_TOURNAMENT_ID);

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
            MockSetupGet(testData);

            // Act
            var actual = TestExtensions.GetModel<TournamentViewModel>(this._sut.Details(TEST_TOURNAMENT_ID));

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

            // Act
            var actual = TestExtensions.GetModel<TournamentViewModel>(this._sut.Create());

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

            // Act
            var result = this._sut.Create(testData) as RedirectToRouteResult;

            // Assert
            VerifyCreate(Times.Once());
            VerifyRedirect(INDEX_ACTION_NAME, result);
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
            MockSetupCreateTournamentValidationException();

            // Act
            var result = TestExtensions.GetModel<TournamentViewModel>(this._sut.Create(testData));

            // Assert
            VerifyCreate(Times.Once());
            Assert.IsNotNull(result, ASSERT_FAIL_VIEW_MODEL_MESSAGE);
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
            this._sut.ModelState.AddModelError(string.Empty, string.Empty);

            // Act
            var result = TestExtensions.GetModel<TournamentViewModel>(this._sut.Create(testData));

            // Assert
            VerifyCreate(Times.Never());
            Assert.IsNotNull(result, ASSERT_FAIL_VIEW_MODEL_MESSAGE);
        }

        /// <summary>
        /// Test for Edit method (GET action). Tournament with specified identifier does not exist. HttpNotFoundResult is returned.
        /// </summary>
        [TestMethod]
        public void EditGetAction_NonExistentTournament_HttpNotFoundResultIsReturned()
        {
            // Arrange
            MockSetupGet(null as Tournament);

            // Act
            var result = this._sut.Edit(TEST_TOURNAMENT_ID);

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
            MockSetupGet(testData);

            // Act
            var actual = TestExtensions.GetModel<TournamentViewModel>(this._sut.Edit(TEST_TOURNAMENT_ID));

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

            // Act
            var result = this._sut.Edit(testData) as RedirectToRouteResult;

            // Assert
            VerifyEdit(Times.Once());
            VerifyRedirect(INDEX_ACTION_NAME, result);
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
            MockSetupEditTournamentValidationException();

            // Act
            var result = TestExtensions.GetModel<TournamentViewModel>(this._sut.Edit(testData));

            // Assert
            VerifyEdit(Times.Once());
            Assert.IsNotNull(result, ASSERT_FAIL_VIEW_MODEL_MESSAGE);
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
            this._sut.ModelState.AddModelError(string.Empty, string.Empty);

            // Act
            var result = TestExtensions.GetModel<TournamentViewModel>(this._sut.Edit(testData));

            // Assert
            VerifyEdit(Times.Never());
            Assert.IsNotNull(result, ASSERT_FAIL_VIEW_MODEL_MESSAGE);
        }

        /// <summary>
        /// Test for Delete method (GET action). Tournament with specified identifier does not exist. HttpNotFoundResult is returned.
        /// </summary>
        [TestMethod]
        public void DeleteGetAction_NonExistentTournament_HttpNotFoundResultIsReturned()
        {
            // Arrange
            MockSetupGet(null as Tournament);

            // Act
            var result = this._sut.Delete(TEST_TOURNAMENT_ID);

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
            MockSetupGet(testData);

            // Act
            var actual = TestExtensions.GetModel<TournamentViewModel>(this._sut.Delete(TEST_TOURNAMENT_ID));

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
            MockSetupGet(null as Tournament);

            // Act
            var result = this._sut.DeleteConfirmed(TEST_TOURNAMENT_ID);

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
            MockSetupGet(testData);

            // Act
            var result = this._sut.DeleteConfirmed(TEST_TOURNAMENT_ID) as RedirectToRouteResult;

            // Assert
            VerifyDelete(Times.Once());
            VerifyRedirect(INDEX_ACTION_NAME, result);
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
        /// Sets up a mock for GetActual method of Tournament service to return specified tournaments.
        /// </summary>
        /// <param name="tournaments">Tournament that will be returned by GetActual method of Tournament service.</param>
        private void MockSetupGetActual(List<Tournament> tournaments)
        {
            this._tournamentServiceMock.Setup(tr => tr.GetActual()).Returns(tournaments);
        }

        /// <summary>
        /// Sets up a mock for GetFinished method of Tournament service to return specified tournaments.
        /// </summary>
        /// <param name="tournaments">Tournament that will be returned by GetFinished method of Tournament service.</param>
        private void MockSetupGetFinished(List<Tournament> tournaments)
        {
            this._tournamentServiceMock.Setup(tr => tr.GetFinished()).Returns(tournaments);
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
                .Throws(new TournamentValidationException(string.Empty, string.Empty, string.Empty));
        }

        /// <summary>
        /// Sets up a mock for Edit method of Tournament service to throw TournamentValidationException.
        /// </summary>
        private void MockSetupEditTournamentValidationException()
        {
            this._tournamentServiceMock.Setup(ts => ts.Edit(It.IsAny<Tournament>()))
                .Throws(new TournamentValidationException(string.Empty, string.Empty, string.Empty));
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

        /// <summary>
        /// Verifies that redirect to specified action takes place.
        /// </summary>
        /// <param name="actionName">Name of the action where we are supposed to be redirected.</param>
        /// <param name="result">Actual redirection result.</param>
        private void VerifyRedirect(string actionName, RedirectToRouteResult result)
        {
            Assert.AreEqual(actionName, result.RouteValues[ROUTE_VALUES_KEY]);
        }
    }
}
