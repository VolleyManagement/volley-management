namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using Contracts;
    using Contracts.Exceptions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Domain.GamesAggregate;
    using VolleyManagement.Domain.TeamsAggregate;
    using VolleyManagement.Domain.TournamentsAggregate;
    using VolleyManagement.UI.Areas.Mvc.Controllers;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.GameResults;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Teams;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments;
    using VolleyManagement.UnitTests.Mvc.ViewModels;
    using VolleyManagement.UnitTests.Services.GameService;
    using VolleyManagement.UnitTests.Services.TeamService;
    using VolleyManagement.UnitTests.Services.TournamentService;

    /// <summary>
    /// Tests for MVC TournamentController class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TournamentsControllerTests
    {
        private const int TEST_TOURNAMENT_ID = 1;
        private const string TEST_TOURNAMENT_NAME = "Name";
        private const int TEST_TEAM_ID = 1;
        private const int EMPTY_TEAMLIST_COUNT = 0;
        private const string ASSERT_FAIL_VIEW_MODEL_MESSAGE = "View model must be returned to user.";
        private const string ASSERT_FAIL_JSON_RESULT_MESSAGE = "Json result must be returned to user.";
        private const string INDEX_ACTION_NAME = "Index";
        private const string SHOW_SCHEDULE_ACTION_NAME = "ShowSchedule";
        private const string ROUTE_VALUES_KEY = "action";
        private const int DAYS_TO_APPLYING_PERIOD_START = 14;
        private const int DAYS_FOR_APPLYING_PERIOD = 14;
        private const int DAYS_FROM_APPLYING_PERIOD_END_TO_GAMES_START = 7;
        private const int DAYS_FOR_GAMES_PERIOD = 120;
        private const int DAYS_FROM_GAMES_START_TO_TRANSFER_START = 1;
        private const int DAYS_FOR_TRANSFER_PERIOD = 21;
        private static readonly DateTime TEST_DATE = new DateTime(2016, 07, 25);

        private readonly Mock<ITournamentService> _tournamentServiceMock = new Mock<ITournamentService>();
        private readonly Mock<IGameService> _gameServiceMock = new Mock<IGameService>();

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
            this._kernel.Bind<IGameService>().ToConstant(this._gameServiceMock.Object);
            this._sut = this._kernel.Get<TournamentsController>();
        }

        #region Index
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
            SetupGetActual(testData);

            // Act
            var actualCurrentTournaments = TestExtensions.GetModel<TournamentsCollectionsViewModel>(this._sut.Index())
                .CurrentTournaments.ToList();
            var actualUpcomingTournaments = TestExtensions.GetModel<TournamentsCollectionsViewModel>(this._sut.Index())
                .UpcomingTournaments.ToList();

            // Assert
            CollectionAssert.AreEqual(expectedCurrentTournaments, actualCurrentTournaments, new TournamentComparer());
            CollectionAssert.AreEqual(expectedUpcomingTournaments, actualUpcomingTournaments, new TournamentComparer());
        }
        #endregion

        #region ManageTournamentTeams
        /// <summary>
        /// Test for ManageTournamentTeams.
        /// Actual tournament teams are requested. Actual tournament teams are returned.
        /// </summary>
        [TestMethod]
        public void ManageTournamentTeams_TournamentTeamsExist_TeamsInCurrentTournamentAreReturned()
        {
            // Arrange
            var testData = MakeTestTeams();
            SetupGetTournamentTeams(testData, TEST_TOURNAMENT_ID);
            var expectedTeamsList = new TournamentTeamsListViewModel(testData, TEST_TOURNAMENT_ID);

            // Act
            var returnedTeamsList = TestExtensions.GetModel<TournamentTeamsListViewModel>(
                this._sut.ManageTournamentTeams(TEST_TOURNAMENT_ID));

            // Assert
            Assert.IsTrue(new TournamentTeamsListViewModelComparer()
                .AreEqual(expectedTeamsList, returnedTeamsList));
        }

        /// <summary>
        /// Test for ManageTournamentTeams while there are no teams.
        /// Actual tournament teams are requested. Empty teams list is returned.
        /// </summary>
        [TestMethod]
        public void ManageTournamentTeams_NonExistTournamentTeams_EmptyTeamListIsReturned()
        {
            // Arrange
            var testData = new TeamServiceTestFixture().Build();
            SetupGetTournamentTeams(testData, TEST_TOURNAMENT_ID);

            // Act
            var returnedTeamsList = TestExtensions.GetModel<TournamentTeamsListViewModel>(
                this._sut.ManageTournamentTeams(TEST_TOURNAMENT_ID));

            // Assert
            Assert.AreEqual(returnedTeamsList.List.Count, EMPTY_TEAMLIST_COUNT);
        }
        #endregion

        #region ShowSchedule

        /// <summary>
        /// Test for ShowSchedule method.
        /// Wrong tournament Id passed. View with error message is returned.
        /// </summary>
        [TestMethod]
        public void ShowSchedule_NonExistentTournament_ErrorViewIsReturned()
        {
            // Arrange
            SetupGet(TEST_TOURNAMENT_ID, null as Tournament);

            // Act
            var result = TestExtensions.GetModel<ScheduleViewModel>(this._sut.ShowSchedule(TEST_TOURNAMENT_ID));

            // Assert
            Assert.IsFalse(_sut.ModelState.IsValid);
            Assert.IsNull(result);
        }

        /// <summary>
        /// Test for ShowSchedule method.
        /// Valid schedule is passed, no exception occurred.
        /// </summary>
        [TestMethod]
        public void ShowSchedule_TournamentExists_ScheduleViewModelIsReturned()
        {
            // Arrange
            const int TEST_ROUND_COUNT = 3;
            var tournament = new TournamentDto 
            {
                Id = TEST_TOURNAMENT_ID, 
                Name = TEST_TOURNAMENT_NAME, 
                Scheme = TournamentSchemeEnum.One 
            };
            var expectedGames = new GameServiceTestFixture().TestGameResults().Build();
            var expected = new ScheduleViewModelBuilder().Build();

            SetupGetTournamentNumberOfRounds(tournament, TEST_ROUND_COUNT);
            _tournamentServiceMock.Setup(t => t.GetTournamentScheduleInfo(TEST_TOURNAMENT_ID)).Returns(tournament);
            _gameServiceMock.Setup(t => t.GetTournamentResults(It.IsAny<int>())).Returns(expectedGames);

            // Act
            var actual = TestExtensions.GetModel<ScheduleViewModel>(this._sut.ShowSchedule(TEST_TOURNAMENT_ID));

            // Assert
            Assert.IsTrue(new ScheduleViewModelComparer().AreEqual(actual, expected));
        }
        #endregion

        #region AddTeamsToTournament
        /// <summary>
        /// Test for AddTeamsToTournament.
        /// Tournament teams list view model is valid and no exception is thrown during adding
        /// Teams are added successfully and json result is returned
        /// </summary>
        [TestMethod]
        public void AddTeamsToTournament_ValidTeamListViewModelNoException_JsonResultIsReturned()
        {
            // Arrange
            var testData = MakeTestTeams();
            var expectedDataResult = new TournamentTeamsListViewModel(testData, TEST_TOURNAMENT_ID);

            // Act
            var jsonResult = this._sut.AddTeamsToTournament(new TournamentTeamsListViewModel(testData, TEST_TOURNAMENT_ID));
            var returnedDataResult = jsonResult.Data as TournamentTeamsListViewModel;

            // Assert
            Assert.IsTrue(new TournamentTeamsListViewModelComparer()
                .AreEqual(returnedDataResult, expectedDataResult));
        }

        /// <summary>
        /// Test for AddTeamsToTournament.
        /// Tournament teams list view model is invalid and Argument exception is thrown during adding
        /// Teams are not added and json result  with model error is returned
        /// </summary>
        [TestMethod]
        public void AddTeamsToTournament_InValidTeamListViewModelWithException_JsonModelErrorReturned()
        {
            // Arrange
            var testData = MakeTestTeams();
            this._tournamentServiceMock
                .Setup(ts => ts.AddTeamsToTournament(It.IsAny<List<Team>>(), It.IsAny<int>()))
                .Throws(new ArgumentException(string.Empty));

            // Act
            var jsonResult = this._sut.AddTeamsToTournament(new TournamentTeamsListViewModel(testData, TEST_TOURNAMENT_ID));
            var modelResult = jsonResult.Data as TeamsAddToTournamentViewModel;

            // Assert
            Assert.IsNotNull(modelResult.Message);
        }
        #endregion

        #region ScheduleGameGetAction
        /// <summary>
        /// Test for ScheduleGame method (GET action). Wrong tournament Id passed. View with error message is returned.
        /// </summary>
        [TestMethod]
        public void ScheduleGameGetAction_NonExistentTournament_ErrorViewIsReturned()
        {
            // Arrange
            SetupGet(TEST_TOURNAMENT_ID, null as Tournament);

            // Act
            var result = TestExtensions.GetModel<GameViewModel>(this._sut.ScheduleGame(TEST_TOURNAMENT_ID));

            // Assert
            Assert.IsFalse(_sut.ModelState.IsValid);
            Assert.IsNull(result);
        }

        /// <summary>
        /// Test for ScheduleGame method (GET action).
        /// Tournament with scheme 1 and no teams passed. View with error message is returned.
        /// </summary>
        [TestMethod]
        public void ScheduleGameGetAction_NoTeamsAvailable_ErrorViewIsReturned()
        {
            // Arrange
            var testData = MakeTestTournament(TEST_TOURNAMENT_ID);
            SetupGet(TEST_TOURNAMENT_ID, testData);
            SetupGetTournamentTeams(new List<Team>(), TEST_TOURNAMENT_ID);

            // Act
            var result = TestExtensions.GetModel<GameViewModel>(this._sut.ScheduleGame(TEST_TOURNAMENT_ID));

            // Assert
            VerifyInvalidModelState("LoadError", result);
        }

        /// <summary>
        /// Test for ScheduleGame method (GET action). Tournament with scheme 1 and 3 teams passed. View with GameViewModel is returned.
        /// </summary>
        [TestMethod]
        public void ScheduleGameGetAction_TournamentExists_GameViewModelIsReturned()
        {
            // Arrange
            const int MIN_ROUND_NUMBER = 1;
            const int TEST_ROUND_COUNT = 3;

            var testTournament = new TournamentDto { Id = TEST_TOURNAMENT_ID, StartDate = TEST_DATE };
            var testTeams = MakeTestTeams();
            SetupGetScheduleInfo(TEST_TOURNAMENT_ID, testTournament);
            SetupGetTournamentTeams(testTeams, TEST_TOURNAMENT_ID);
            SetupGetTournamentNumberOfRounds(testTournament, TEST_ROUND_COUNT);

            var expected = new GameViewModel
            {
                TournamentId = TEST_TOURNAMENT_ID,
                GameDate = TEST_DATE,
                Teams = new SelectList(testTeams, "Id", "Name"),
                Rounds = new SelectList(Enumerable.Range(MIN_ROUND_NUMBER, TEST_ROUND_COUNT))
            };

            // Act
            var actual = TestExtensions.GetModel<GameViewModel>(this._sut.ScheduleGame(TEST_TOURNAMENT_ID));

            // Assert
            AssertEqual(actual, expected);
        }
        #endregion

        #region ScheduleGamePostAction
        /// <summary>
        /// Test for ScheduleGame method (POST action).
        /// Valid game is passed, no exception occurs.
        /// Game is created and browser is redirected to ShowSchedule action.
        /// </summary>
        [TestMethod]
        public void ScheduleGamePostAction_ValidGameViewModel_GameIsCreatedRedirectToSchedule()
        {
            // Arrange
            var testData = MakeTestGameViewModel();
            var redirect = true;

            // Act
            var result = this._sut.ScheduleGame(testData, redirect) as RedirectToRouteResult;

            // Assert
            VerifyCreateGame(Times.Once());
            VerifyRedirect(SHOW_SCHEDULE_ACTION_NAME, result);
        }

        /// <summary>
        /// Test for ScheduleGame method (POST action).
        /// Valid game is passed, no exception occurs.
        /// Game is created. Browser is not redirected.
        /// </summary>
        [TestMethod]
        public void ScheduleGamePostAction_ValidGameViewModel_GameIsCreated()
        {
            // Arrange
            var testData = MakeTestGameViewModel();
            var redirect = false;

            // Act
            var result = this._sut.ScheduleGame(testData, redirect) as ViewResult;

            // Assert
            VerifyCreateGame(Times.Once());
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Test for ScheduleGame method (POST action).
        /// Valid game is passed, but ArgumentException occurs.
        /// Game is not created. Browser is redirected to ScheduleGame action.
        /// </summary>
        [TestMethod]
        public void ScheduleGamePostAction_ServiceValidationFails_ScheduleGameViewIsReturned()
        {
            // Arrange
            var testData = MakeTestGameViewModel();
            var redirect = false;
            this._gameServiceMock.Setup(ts => ts.Create(It.IsAny<Game>()))
                            .Throws(new ArgumentException(string.Empty));

            // Act
            var result = TestExtensions.GetModel<GameViewModel>(this._sut.ScheduleGame(testData, redirect));

            // Assert
            VerifyInvalidModelState("ValidationError", result);
        }

        /// <summary>
        /// Test for ScheduleGame method (POST action). Invalid game view model is passed, HttpNotFound returned
        /// </summary>
        [TestMethod]
        public void ScheduleGamePostAction_InvalidGameViewModel_ScheduleGameViewIsReturned()
        {
            // Arrange
            var testData = MakeTestGameViewModel();
            var redirect = false;
            this._sut.ModelState.AddModelError(string.Empty, string.Empty);

            // Act
            var result = this._sut.ScheduleGame(testData, redirect) as ViewResult;

            // Assert
            VerifyCreateGame(Times.Never());
            Assert.IsNotNull(result);
        }
        #endregion

        #region GetFinished
        /// <summary>
        /// Test for GetFinished method. Finished tournaments are requested. JsonResult with finished tournaments is returned.
        /// </summary>
        [TestMethod]
        public void GetFinished_GetFinishedTournaments_JsonResultIsReturned()
        {
            // Arrange
            var testData = MakeTestTournaments();
            SetupGetFinished(testData);

            // Act
            var result = this._sut.GetFinished();

            // Assert
            Assert.IsNotNull(result, ASSERT_FAIL_JSON_RESULT_MESSAGE);
        }
        #endregion

        #region Details
        /// <summary>
        /// Test for Details method. Tournament with specified identifier does not exist. HttpNotFoundResult is returned.
        /// </summary>
        [TestMethod]
        public void Details_NonExistentTournament_HttpNotFoundResultIsReturned()
        {
            // Arrange
            SetupGet(TEST_TOURNAMENT_ID, null as Tournament);

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
            SetupGet(TEST_TOURNAMENT_ID, testData);

            // Act
            var actual = TestExtensions.GetModel<TournamentViewModel>(this._sut.Details(TEST_TOURNAMENT_ID));

            // Assert
            TestHelper.AreEqual<TournamentViewModel>(expected, actual, new TournamentViewModelComparer());
        }
        #endregion

        #region CreateGetAction
        /// <summary>
        /// Test for Create method (GET action). Tournament view model is requested. Tournament view model is returned.
        /// </summary>
        [TestMethod]
        public void CreateGetAction_GetTournamentViewModel_TournamentViewModelIsReturned()
        {
            // Arrange
            var expected = new TournamentViewModel()
            {
                ApplyingPeriodStart = DateTime.Now.AddDays(DAYS_TO_APPLYING_PERIOD_START),
                ApplyingPeriodEnd = DateTime.Now.AddDays(DAYS_TO_APPLYING_PERIOD_START + DAYS_FOR_APPLYING_PERIOD),
                GamesStart = DateTime.Now.AddDays(DAYS_TO_APPLYING_PERIOD_START + DAYS_FOR_APPLYING_PERIOD
                + DAYS_FROM_APPLYING_PERIOD_END_TO_GAMES_START),
                GamesEnd = DateTime.Now.AddDays(DAYS_TO_APPLYING_PERIOD_START + DAYS_FOR_APPLYING_PERIOD
                + DAYS_FROM_APPLYING_PERIOD_END_TO_GAMES_START + DAYS_FOR_GAMES_PERIOD),
                TransferStart = DateTime.Now.AddDays(DAYS_TO_APPLYING_PERIOD_START + DAYS_FOR_APPLYING_PERIOD
                + DAYS_FROM_APPLYING_PERIOD_END_TO_GAMES_START + DAYS_FROM_GAMES_START_TO_TRANSFER_START),
                TransferEnd = DateTime.Now.AddDays(DAYS_TO_APPLYING_PERIOD_START + DAYS_FOR_APPLYING_PERIOD
                + DAYS_FROM_APPLYING_PERIOD_END_TO_GAMES_START + DAYS_FROM_GAMES_START_TO_TRANSFER_START + DAYS_FOR_TRANSFER_PERIOD)
            };

            // Act
            var actual = TestExtensions.GetModel<TournamentViewModel>(this._sut.Create());

            // Assert
            TestHelper.AreEqual<TournamentViewModel>(expected, actual, new TournamentViewModelComparer());
        }
        #endregion

        #region CreatePostAction
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
            SetupCreateThrowsTournamentValidationException();

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
        #endregion

        #region EditGetAction
        /// <summary>
        /// Test for Edit method (GET action). Tournament with specified identifier does not exist. HttpNotFoundResult is returned.
        /// </summary>
        [TestMethod]
        public void EditGetAction_NonExistentTournament_HttpNotFoundResultIsReturned()
        {
            // Arrange
            SetupGet(TEST_TOURNAMENT_ID, null as Tournament);

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
            SetupGet(TEST_TOURNAMENT_ID, testData);

            // Act
            var actual = TestExtensions.GetModel<TournamentViewModel>(this._sut.Edit(TEST_TOURNAMENT_ID));

            // Assert
            TestHelper.AreEqual<TournamentViewModel>(expected, actual, new TournamentViewModelComparer());
        }
        #endregion

        #region EditPostAction
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
            SetupEditThrowsTournamentValidationException();

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
        #endregion

        #region DeleteTeamFromTournament
        /// <summary>
        /// Test for Delete team from tournament method (POST action)
        /// </summary>
        [TestMethod]
        public void DeleteTeamFromTournament_TeamExists_TeamDeleted()
        {
            // Arrange
            this._tournamentServiceMock
                .Setup(ts => ts.DeleteTeamFromTournament(It.IsAny<int>(), It.IsAny<int>()));

            // Act
            var jsonResult = this._sut.DeleteTeamFromTournament(TEST_TOURNAMENT_ID, TEST_TEAM_ID);
            var result = jsonResult.Data as TeamDeleteFromTournamentViewModel;

            // Assert
            Assert.IsTrue(result.HasDeleted);
        }

        /// <summary>
        /// Test for Delete team from tournament method (POST action)
        /// team is not exists
        /// </summary>
        [TestMethod]
        public void DeleteTeamFromTournament_NonExistTeam_TeamIsNotDeleted()
        {
            // Arrange
            this._tournamentServiceMock
                .Setup(ts => ts.DeleteTeamFromTournament(It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new MissingEntityException());

            // Act
            var jsonResult = this._sut.DeleteTeamFromTournament(TEST_TOURNAMENT_ID, TEST_TEAM_ID);
            var result = jsonResult.Data as TeamDeleteFromTournamentViewModel;

            // Assert
            Assert.IsFalse(result.HasDeleted);
        }
        #endregion

        #region DeleteGetAction
        /// <summary>
        /// Test for Delete method (GET action). Tournament with specified identifier does not exist. HttpNotFoundResult is returned.
        /// </summary>
        [TestMethod]
        public void DeleteGetAction_NonExistentTournament_HttpNotFoundResultIsReturned()
        {
            // Arrange
            SetupGet(TEST_TOURNAMENT_ID, null as Tournament);

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
            SetupGet(TEST_TOURNAMENT_ID, testData);

            // Act
            var actual = TestExtensions.GetModel<TournamentViewModel>(this._sut.Delete(TEST_TOURNAMENT_ID));

            // Assert
            TestHelper.AreEqual<TournamentViewModel>(expected, actual, new TournamentViewModelComparer());
        }
        #endregion

        #region DeletePostAction
        /// <summary>
        /// Test for DeleteConfirmed method (delete POST action). Tournament with specified identifier does not exist.
        /// HttpNotFoundResult is returned.
        /// </summary>
        [TestMethod]
        public void DeletePostAction_NonExistentTournament_HttpNotFoundResultIsReturned()
        {
            // Arrange
            SetupGet(TEST_TOURNAMENT_ID, null as Tournament);

            // Act
            var result = this._sut.DeleteConfirmed(TEST_TOURNAMENT_ID);

            // Assert
            VerifyDelete(TEST_TOURNAMENT_ID, Times.Never());
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
            SetupGet(TEST_TOURNAMENT_ID, testData);

            // Act
            var result = this._sut.DeleteConfirmed(TEST_TOURNAMENT_ID) as RedirectToRouteResult;

            // Assert
            VerifyDelete(TEST_TOURNAMENT_ID, Times.Once());
            VerifyRedirect(INDEX_ACTION_NAME, result);
        }
        #endregion

        #region Private
        private List<Tournament> MakeTestTournaments()
        {
            return new TournamentServiceTestFixture().TestTournaments().Build();
        }

        private List<Team> MakeTestTeams()
        {
            return new TeamServiceTestFixture().TestTeams().Build();
        }

        private Tournament MakeTestTournament(int tournamentId)
        {
            return new TournamentBuilder().WithId(tournamentId).Build();
        }

        private TournamentViewModel MakeTestTournamentViewModel()
        {
            return new TournamentMvcViewModelBuilder().Build();
        }

        private GameViewModel MakeTestGameViewModel()
        {
            return new GameViewModelBuilder().Build();
        }

        private TournamentViewModel MakeTestTournamentViewModel(int tournamentId)
        {
            return new TournamentMvcViewModelBuilder().WithId(tournamentId).Build();
        }

        private List<Tournament> GetTournamentsWithState(List<Tournament> tournaments, TournamentStateEnum state)
        {
            return tournaments.Where(tr => tr.State == state).ToList();
        }

        private void SetupGetActual(List<Tournament> tournaments)
        {
            this._tournamentServiceMock.Setup(tr => tr.GetActual()).Returns(tournaments);
        }

        private void SetupGetFinished(List<Tournament> tournaments)
        {
            this._tournamentServiceMock.Setup(tr => tr.GetFinished()).Returns(tournaments);
        }

        private void SetupGetTournamentTeams(List<Team> teams, int tournamentId)
        {
            this._tournamentServiceMock
                .Setup(tr => tr.GetAllTournamentTeams(tournamentId))
                .Returns(teams);
        }

        private void SetupGetTournamentNumberOfRounds(TournamentDto tournament, byte numberOfRounds)
        {
            this._tournamentServiceMock
                .Setup(tr => tr.GetNumberOfRounds(tournament))
                .Returns(numberOfRounds);
        }

        private void SetupGet(int tournamentId, Tournament tournament)
        {
            this._tournamentServiceMock.Setup(tr => tr.Get(tournamentId)).Returns(tournament);
        }

        private void SetupGetScheduleInfo(int tournamentId, TournamentDto tournament)
        {
            this._tournamentServiceMock.Setup(tr => tr.GetTournamentScheduleInfo(tournamentId)).Returns(tournament);
        }

        private void SetupCreateThrowsTournamentValidationException()
        {
            this._tournamentServiceMock.Setup(ts => ts.Create(It.IsAny<Tournament>()))
                .Throws(new TournamentValidationException(string.Empty, string.Empty, string.Empty));
        }

        private void SetupEditThrowsTournamentValidationException()
        {
            this._tournamentServiceMock.Setup(ts => ts.Edit(It.IsAny<Tournament>()))
                .Throws(new TournamentValidationException(string.Empty, string.Empty, string.Empty));
        }

        private void VerifyCreate(Times times)
        {
            this._tournamentServiceMock.Verify(ts => ts.Create(It.IsAny<Tournament>()), times);
        }

        private void VerifyEdit(Times times)
        {
            this._tournamentServiceMock.Verify(ts => ts.Edit(It.IsAny<Tournament>()), times);
        }

        private void VerifyDelete(int tournamentId, Times times)
        {
            this._tournamentServiceMock.Verify(ts => ts.Delete(tournamentId), times);
        }

        private void VerifyRedirect(string actionName, RedirectToRouteResult result)
        {
            Assert.AreEqual(actionName, result.RouteValues[ROUTE_VALUES_KEY]);
        }

        private void VerifyCreateGame(Times times)
        {
            this._gameServiceMock.Verify(gs => gs.Create(It.IsAny<Game>()), times);
        }

        private void VerifyInvalidModelState(string expectedKey, GameViewModel gameViewModel)
        {
            Assert.IsFalse(_sut.ModelState.IsValid);
            Assert.IsTrue(_sut.ModelState.ContainsKey(expectedKey));
            Assert.IsNull(gameViewModel);
        }

        private void AssertEqual(GameViewModel x, GameViewModel y) 
        {
            string WRONG_TOURNAMENT_ID = "Actual TournamentId doesn't match expected";
            string WRONG_HOME_TEAM_ID = "Actual HomeTeamId doesn't match expected";
            string WRONG_AWAY_TEAM_ID = "Actual AwayTeamId doesn't match expected";
            string WRONG_ROUND = "Actual Round number doesn't match expected";
            string WRONG_GAME_DATE = "Actual GameDate doesn't match expected";
            string WRONG_TEAMS = "Actual Teams list doesn't match expected";
            string WRONG_ROUNDS = "Actual Rounds list doesn't match expected";
            
            Assert.AreEqual(x.TournamentId, y.TournamentId, WRONG_TOURNAMENT_ID);
            Assert.AreEqual(x.HomeTeamId, y.HomeTeamId, WRONG_HOME_TEAM_ID);
            Assert.AreEqual(x.AwayTeamId, y.AwayTeamId, WRONG_AWAY_TEAM_ID);
            Assert.AreEqual(x.Round, y.Round, WRONG_ROUND);
            Assert.AreEqual(x.GameDate, y.GameDate, WRONG_GAME_DATE);

            Assert.IsTrue(x.Teams != null &&
                          y.Teams != null &&
                          x.Teams.Select(team => new { Text = team.Text, Value = team.Value }).SequenceEqual(
                          y.Teams.Select(team => new { Text = team.Text, Value = team.Value })), WRONG_TEAMS);
            
            Assert.IsTrue(x.Rounds != null &&
                          y.Rounds != null &&
                         (x.Rounds.Items as IEnumerable<int>).SequenceEqual(
                          y.Rounds.Items as IEnumerable<int>), WRONG_ROUNDS);            
        }
        #endregion
    }
}
