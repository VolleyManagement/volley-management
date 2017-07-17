namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Contracts;
    using Contracts.Exceptions;
    using Crosscutting.Contracts.Providers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    using VolleyManagement.Contracts.Authorization;
    using VolleyManagement.Domain.GamesAggregate;
    using VolleyManagement.Domain.RolesAggregate;
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
        private const int TEST_ID = 1;
        private const int TEST_TOURNAMENT_ID = 1;
        private const int TEST_TEAM_ID = 1;
        private const int TEST_USER_ID = 1;
        private const int ANONYM_ID = -1;
        private const string TEST_TOURNAMENT_NAME = "Name";
        private const int TEST_ROUND_NUMBER = 2;
        private const int EMPTY_TEAMLIST_COUNT = 0;
        private const byte FIRST_ROUND_NUMBER = 1;
        private const byte SECOND_ROUND_NUMBER = 2;
        private const string INVALID_PARAMETR = "Invalid parametr.Your request didn't create.";
        private const string ASSERT_FAIL_VIEW_MODEL_MESSAGE = "View model must be returned to user.";
        private const string ASSERT_FAIL_JSON_RESULT_MESSAGE = "Json result must be returned to user.";
        private const string JSON_NO_RIGHTS_MESSAGE = "Please, login to apply team for the tournament.";
        private const string JSON_OK_MSG = "Your request was succesfully created. Please, wait until administrator confirm your request.";
        private const string INDEX_ACTION_NAME = "Index";
        private const string SHOW_SCHEDULE_ACTION_NAME = "ShowSchedule";
        private const string ROUTE_VALUES_KEY = "action";
        private const string MANAGE_TOURNAMENT_TEAMS = "/Teams/ManageTournamentTeams?tournamentId=";
        private const int DAYS_TO_APPLYING_PERIOD_START = 14;
        private const int DAYS_FOR_APPLYING_PERIOD = 14;
        private const int DAYS_FROM_APPLYING_PERIOD_END_TO_GAMES_START = 7;
        private const int DAYS_FOR_GAMES_PERIOD = 120;
        private const int DAYS_FROM_GAMES_START_TO_TRANSFER_START = 1;
        private const int DAYS_FOR_TRANSFER_PERIOD = 21;
        private static readonly DateTime _testDate = new DateTime(1996, 07, 25);

        private readonly List<AuthOperation> _allowedOperationsShowSchedule = new List<AuthOperation>
        {
            AuthOperations.Games.Create,
            AuthOperations.Games.Edit,
            AuthOperations.Games.Delete,
            AuthOperations.Games.SwapRounds,
            AuthOperations.Games.EditResult
        };

        private Mock<ITournamentService> _tournamentServiceMock = new Mock<ITournamentService>();
        private Mock<IGameService> _gameServiceMock = new Mock<IGameService>();
        private Mock<ITeamService> _teamServiceMock = new Mock<ITeamService>();
        private Mock<IAuthorizationService> _authServiceMock = new Mock<IAuthorizationService>();
        private Mock<ICurrentUserService> _currentUserServiceMock = new Mock<ICurrentUserService>();

        private Mock<ITournamentRequestService> _tournamentRequestServiceMock =
            new Mock<ITournamentRequestService>();

        private Mock<HttpContextBase> _httpContextMock = new Mock<HttpContextBase>();
        private Mock<HttpRequestBase> _httpRequestMock = new Mock<HttpRequestBase>();
        private Mock<TimeProvider> _timeMock = new Mock<TimeProvider>();

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _tournamentServiceMock = new Mock<ITournamentService>();
            _gameServiceMock = new Mock<IGameService>();
            _teamServiceMock = new Mock<ITeamService>();
            _authServiceMock = new Mock<IAuthorizationService>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _httpContextMock = new Mock<HttpContextBase>();
            _httpRequestMock = new Mock<HttpRequestBase>();
            _tournamentRequestServiceMock = new Mock<ITournamentRequestService>();

            _httpContextMock.SetupGet(c => c.Request).Returns(_httpRequestMock.Object);

            _timeMock.Setup(tp => tp.UtcNow).Returns(_testDate);
            TimeProvider.Current = _timeMock.Object;
        }

        /// <summary>
        /// Cleanup test data
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            TimeProvider.ResetToDefault();
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

            var sut = BuildSUT();

            // Act
            var actualCurrentTournaments = TestExtensions.GetModel<TournamentsCollectionsViewModel>(sut.Index())
                .CurrentTournaments.ToList();
            var actualUpcomingTournaments = TestExtensions.GetModel<TournamentsCollectionsViewModel>(sut.Index())
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
            SetupRequestRawUrl(MANAGE_TOURNAMENT_TEAMS + TEST_TOURNAMENT_ID);

            var sut = BuildSUT();
            SetupControllerContext(sut);

            // Act
            var returnedTeamsList = TestExtensions.GetModel<TournamentTeamsListReferrerViewModel>(
                sut.ManageTournamentTeams(TEST_TOURNAMENT_ID));

            // Assert
            Assert.IsTrue(new TournamentTeamsListViewModelComparer()
                .AreEqual(expectedTeamsList, returnedTeamsList.Model));
            Assert.AreEqual(returnedTeamsList.Referer, sut.Request.RawUrl);
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
            SetupRequestRawUrl(MANAGE_TOURNAMENT_TEAMS + TEST_TOURNAMENT_ID);

            var sut = BuildSUT();
            SetupControllerContext(sut);

            // Act
            var returnedTeamsList = TestExtensions.GetModel<TournamentTeamsListReferrerViewModel>(
                sut.ManageTournamentTeams(TEST_TOURNAMENT_ID));

            // Assert
            Assert.AreEqual(returnedTeamsList.Model.List.Count, EMPTY_TEAMLIST_COUNT);
            Assert.AreEqual(returnedTeamsList.Referer, sut.Request.RawUrl);
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
            var sut = BuildSUT();
            var result = TestExtensions.GetModel<ScheduleViewModel>(sut.ShowSchedule(TEST_TOURNAMENT_ID));

            // Assert
            Assert.IsFalse(sut.ModelState.IsValid);
            Assert.IsTrue(sut.ModelState.ContainsKey("LoadError"));
            Assert.IsNull(result, "Result should be null");

            VerifyGetAllowedOperations(_allowedOperationsShowSchedule, Times.Never());
        }

        /// <summary>
        /// Test for ShowSchedule method.
        /// Valid rounds is passed, no exception occurred.
        /// </summary>
        [TestMethod]
        public void ShowSchedule_TournamentHasGamesScheduled_RoundsCreatedCorrectly()
        {
            // Arrange
            const int TEST_ROUND_COUNT = 3;

            var tournament = new TournamentScheduleDto
            {
                Id = TEST_TOURNAMENT_ID,
                Name = TEST_TOURNAMENT_NAME,
                Scheme = TournamentSchemeEnum.One
            };

            var expected = new ScheduleViewModelBuilder().Build();

            SetupGetTournamentNumberOfRounds(tournament, TEST_ROUND_COUNT);
            SetupGetScheduleInfo(TEST_TOURNAMENT_ID, tournament);
            SetupGetTournamentResults(
                TEST_TOURNAMENT_ID,
                new GameServiceTestFixture().TestGameResults().Build());

            var sut = BuildSUT();

            // Act
            var actual = TestExtensions.GetModel<ScheduleViewModel>(sut.ShowSchedule(TEST_TOURNAMENT_ID));

            // Assert
            Assert.IsTrue(new ScheduleViewModelComparer().AreRoundsEqual(actual.Rounds, expected.Rounds));
            VerifyGetAllowedOperations(_allowedOperationsShowSchedule, Times.Once());
        }

        /// <summary>
        /// Test for ShowSchedule method.
        /// Valid schedule is passed, no exception occurred.
        /// </summary>
        [TestMethod]
        public void ShowSchedule_ValidScheduleViewModel_ScheduleViewModelIsReturned()
        {
            // Arrange
            const int TEST_ROUND_COUNT = 3;
            var tournament = new TournamentScheduleDto
            {
                Id = TEST_TOURNAMENT_ID,
                Name = TEST_TOURNAMENT_NAME,
                Scheme = TournamentSchemeEnum.One
            };

            SetupGetTournamentNumberOfRounds(tournament, TEST_ROUND_COUNT);
            SetupGetScheduleInfo(
                TEST_TOURNAMENT_ID,
                tournament);
            SetupGetTournamentResults(
                TEST_TOURNAMENT_ID,
                new GameServiceTestFixture().TestGameResults().Build());

            var expected = new ScheduleViewModelBuilder().WithTournamentScheme(TournamentSchemeEnum.One).Build();
            var sut = BuildSUT();

            // Act
            var actual = TestExtensions.GetModel<ScheduleViewModel>(sut.ShowSchedule(TEST_TOURNAMENT_ID));

            // Assert
            Assert.IsTrue(new ScheduleViewModelComparer().AreEqual(actual, expected));
            VerifyGetAllowedOperations(_allowedOperationsShowSchedule, Times.Once());
        }

        /// <summary>
        /// Test for ShowSchedule method.
        /// Valid schedule is passed, no exception occurred.
        /// </summary>
        [TestMethod]
        public void ShowSchedule_PlayoffScheme_RoundNamesAreCreated()
        {
            // Arrange
            const int TEST_ROUND_COUNT = 5;
            var tournament = new TournamentScheduleDto
            {
                Id = TEST_TOURNAMENT_ID,
                Name = TEST_TOURNAMENT_NAME,
                Scheme = TournamentSchemeEnum.PlayOff
            };

            SetupGetScheduleInfo(
                TEST_TOURNAMENT_ID,
                tournament);
            SetupGetTournamentResults(
                TEST_TOURNAMENT_ID,
                new GameServiceTestFixture().TestGameResults().Build());

            SetupGetTournamentNumberOfRounds(tournament, TEST_ROUND_COUNT);
            var expectedRoundNames = new string[] { "Round of 32", "Round of 16", "Quarter final", "Semifinal", "Final" };
            var expected = new ScheduleViewModelBuilder().WithRoundNames(expectedRoundNames).Build();

            var sut = BuildSUT();

            // Act
            var actual = TestExtensions.GetModel<ScheduleViewModel>(sut.ShowSchedule(TEST_TOURNAMENT_ID));

            // Assert
            CollectionAssert.AreEqual(actual.RoundNames, expected.RoundNames);
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
            var sut = BuildSUT();

            // Act
            var jsonResult =
                sut.AddTeamsToTournament(new TournamentTeamsListViewModel(testData, TEST_TOURNAMENT_ID));
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
            _tournamentServiceMock
                .Setup(ts => ts.AddTeamsToTournament(It.IsAny<List<Team>>(), It.IsAny<int>()))
                .Throws(new ArgumentException(string.Empty));

            var sut = BuildSUT();

            // Act
            var jsonResult =
                sut.AddTeamsToTournament(new TournamentTeamsListViewModel(testData, TEST_TOURNAMENT_ID));
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
            var sut = BuildSUT();

            // Act
            var result = TestExtensions.GetModel<GameViewModel>(sut.ScheduleGame(TEST_TOURNAMENT_ID));

            // Assert
            Assert.IsFalse(sut.ModelState.IsValid);
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

            var sut = BuildSUT();

            // Act
            var result = TestExtensions.GetModel<GameViewModel>(sut.ScheduleGame(TEST_TOURNAMENT_ID));

            // Assert
            VerifyInvalidModelState("LoadError", result, sut);
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

            var testTournament = new TournamentScheduleDto { Id = TEST_TOURNAMENT_ID, StartDate = _testDate };
            var testTeams = MakeTestTeams();
            SetupGetScheduleInfo(TEST_TOURNAMENT_ID, testTournament);
            SetupGetTournamentTeams(testTeams, TEST_TOURNAMENT_ID);
            SetupGetTournamentNumberOfRounds(testTournament, TEST_ROUND_COUNT);

            var expected = new GameViewModel
            {
                TournamentId = TEST_TOURNAMENT_ID,
                GameDate = _testDate,
                Teams = new SelectList(testTeams, "Id", "Name"),
                Rounds = new SelectList(Enumerable.Range(MIN_ROUND_NUMBER, TEST_ROUND_COUNT))
            };

            var sut = BuildSUT();

            // Act
            var actual = TestExtensions.GetModel<GameViewModel>(sut.ScheduleGame(TEST_TOURNAMENT_ID));

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

            var sut = BuildSUT();

            // Act
            var result = sut.ScheduleGame(testData, redirect) as RedirectToRouteResult;

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

            var sut = BuildSUT();

            // Act
            var result = sut.ScheduleGame(testData, redirect) as ViewResult;

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
            _gameServiceMock.Setup(ts => ts.Create(It.IsAny<Game>()))
                .Throws(new ArgumentException(string.Empty));

            var sut = BuildSUT();

            // Act
            var result = TestExtensions.GetModel<GameViewModel>(sut.ScheduleGame(testData, redirect));

            // Assert
            VerifyInvalidModelState("ValidationError", result, sut);
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

            var sut = BuildSUT();
            sut.ModelState.AddModelError(string.Empty, string.Empty);

            // Act
            var result = sut.ScheduleGame(testData, redirect) as ViewResult;

            // Assert
            VerifyCreateGame(Times.Never());
            Assert.IsNotNull(result);
        }

        #endregion

        #region EditScheduledGameGetAction

        /// <summary>
        /// Test for EditScheduledGame method (GET action). Correct game id passed. View with GameViewModel is returned.
        /// </summary>
        [TestMethod]
        public void EditScheduledGameGetAction_GameExists_GameViewModelIsReturned()
        {
            // Arrange
            const int MIN_ROUND_NUMBER = 1;
            const int TEST_ROUND_COUNT = 3;

            var testGame = new GameResultDto
            {
                Id = TEST_ID,
                HomeTeamId = TEST_ID,
                AwayTeamId = TEST_ID,
                TournamentId = TEST_TOURNAMENT_ID,
                Round = TEST_ROUND_NUMBER,
                GameDate = _testDate
            };
            SetupGetGame(TEST_ID, testGame);

            var testTournament = new TournamentScheduleDto { Id = TEST_TOURNAMENT_ID, StartDate = _testDate };
            var testTeams = MakeTestTeams();
            SetupGetScheduleInfo(TEST_TOURNAMENT_ID, testTournament);
            SetupGetTournamentTeams(testTeams, TEST_TOURNAMENT_ID);
            SetupGetTournamentNumberOfRounds(testTournament, TEST_ROUND_COUNT);

            var expected = GameViewModel.Map(testGame);
            expected.Teams = new SelectList(testTeams, "Id", "Name");
            expected.Rounds = new SelectList(Enumerable.Range(MIN_ROUND_NUMBER, TEST_ROUND_COUNT));

            var sut = BuildSUT();

            // Act
            var actual = TestExtensions.GetModel<GameViewModel>(sut.EditScheduledGame(TEST_ID));

            // Assert
            AssertEqual(actual, expected);
        }

        /// <summary>
        /// Test for EditScheduledGame method (GET action). Wrong game Id passed. View with error message is returned.
        /// </summary>
        [TestMethod]
        public void EditScheduledGameGetAction_NonExistentGame_ErrorViewIsReturned()
        {
            // Arrange
            _gameServiceMock.Setup(gs => gs.Get(TEST_ID)).Returns(null as GameResultDto);
            var sut = BuildSUT();

            // Act
            var result = TestExtensions.GetModel<GameViewModel>(sut.EditScheduledGame(TEST_ID));

            // Assert
            VerifyInvalidModelState("LoadError", result, sut);
        }

        #endregion

        #region EditScheduledGamePostAction

        /// <summary>
        /// Test for EditScheduledGame method (POST action).
        /// Valid gameViewModel is passed, no exception occurs.
        /// Game is updated. Browser is redirected to ShowSchedule action.
        /// </summary>
        [TestMethod]
        public void EditScheduledGamePostAction_ValidGameViewModel_GameIsUpdated()
        {
            // Arrange
            var testData = MakeTestGameViewModel();
            var sut = BuildSUT();

            // Act
            var result = sut.EditScheduledGame(testData) as RedirectToRouteResult;

            // Assert
            VerifyEditGame(Times.Once());
            VerifyRedirect("ShowSchedule", result);
        }

        /// <summary>
        /// Test for EditScheduledGame method (POST action).
        /// Valid game is passed, but ArgumentException occurs.
        /// Game is not updated. View with GameViewModel is returned.
        /// </summary>
        [TestMethod]
        public void EditScheduledGamePostAction_ServiceValidationFails_GameViewModelIsReturned()
        {
            // Arrange
            var testData = MakeTestGameViewModel();
            _gameServiceMock.Setup(gs => gs.Edit(It.IsAny<Game>())).Throws<ArgumentException>();

            var sut = BuildSUT();

            // Act
            var result = TestExtensions.GetModel<GameViewModel>(sut.EditScheduledGame(testData));

            // Assert
            VerifyInvalidModelState("ValidationError", result, sut);
        }

        /// <summary>
        /// Test for EditScheduledGame method (POST action).
        /// Valid game is passed, but MissingEntityException occurs.
        /// Game is not updated. View with error message is returned.
        /// </summary>
        [TestMethod]
        public void EditScheduledGamePostAction_NonExistentGameIsPassed_ErrorViewIsReturned()
        {
            // Arrange
            var testData = MakeTestGameViewModel();
            _gameServiceMock.Setup(gs => gs.Edit(It.IsAny<Game>())).Throws<MissingEntityException>();

            var sut = BuildSUT();

            // Act
            var result = TestExtensions.GetModel<GameViewModel>(sut.EditScheduledGame(testData));

            // Assert
            VerifyInvalidModelState("LoadError", result, sut);
        }

        /// <summary>
        /// Test for EditScheduledGame method (POST action). Invalid game view model is passed, view with GameViewModel is returned.
        /// </summary>
        [TestMethod]
        public void EditScheduledGamePostAction_InvalidGameViewModel_GameViewModelIsReturned()
        {
            // Arrange
            var testData = MakeTestGameViewModel();

            var sut = BuildSUT();
            sut.ModelState.AddModelError(string.Empty, string.Empty);

            // Act
            var result = sut.EditScheduledGame(testData) as ViewResult;

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
            var sut = BuildSUT();

            // Act
            var result = sut.GetFinished();

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

            var sut = BuildSUT();

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
            SetupGet(TEST_TOURNAMENT_ID, testData);

            var sut = BuildSUT();

            // Act
            var actual = TestExtensions.GetModel<TournamentViewModel>(sut.Details(TEST_TOURNAMENT_ID));

            // Assert
            TestHelper.AreEqual<TournamentViewModel>(expected, actual, new TournamentViewModelComparer());
            VerifyGetAllowedOperations(Times.Once());
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
            var now = _testDate;

            var expected = new TournamentViewModel
            {
                Season = (short)now.Year,
                ApplyingPeriodStart = now.AddDays(DAYS_TO_APPLYING_PERIOD_START),
                ApplyingPeriodEnd = now.AddDays(DAYS_TO_APPLYING_PERIOD_START
                                                + DAYS_FOR_APPLYING_PERIOD),
                GamesStart = now.AddDays(DAYS_TO_APPLYING_PERIOD_START
                                         + DAYS_FOR_APPLYING_PERIOD
                                         + DAYS_FROM_APPLYING_PERIOD_END_TO_GAMES_START),
                GamesEnd = now.AddDays(DAYS_TO_APPLYING_PERIOD_START
                                       + DAYS_FOR_APPLYING_PERIOD
                                       + DAYS_FROM_APPLYING_PERIOD_END_TO_GAMES_START
                                       + DAYS_FOR_GAMES_PERIOD),
                TransferStart = now.AddDays(DAYS_TO_APPLYING_PERIOD_START
                                            + DAYS_FOR_APPLYING_PERIOD
                                            + DAYS_FROM_APPLYING_PERIOD_END_TO_GAMES_START
                                            + DAYS_FROM_GAMES_START_TO_TRANSFER_START),
                TransferEnd = now.AddDays(DAYS_TO_APPLYING_PERIOD_START
                                          + DAYS_FOR_APPLYING_PERIOD
                                          + DAYS_FROM_APPLYING_PERIOD_END_TO_GAMES_START
                                          + DAYS_FROM_GAMES_START_TO_TRANSFER_START
                                          + DAYS_FOR_TRANSFER_PERIOD),
                IsSaved = false
            };

            var sut = BuildSUT();

            // Act
            var actual = TestExtensions.GetModel<TournamentViewModel>(sut.Create());

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
            var sut = BuildSUT();

            // Act
            var result = sut.Create(testData) as RedirectToRouteResult;

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
            var sut = BuildSUT();

            // Act
            var result = TestExtensions.GetModel<TournamentViewModel>(sut.Create(testData));

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
            var sut = BuildSUT();
            sut.ModelState.AddModelError(string.Empty, string.Empty);

            // Act
            var result = TestExtensions.GetModel<TournamentViewModel>(sut.Create(testData));

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
            var sut = BuildSUT();

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
            SetupGet(TEST_TOURNAMENT_ID, testData);

            var sut = BuildSUT();

            // Act
            var actual = TestExtensions.GetModel<TournamentViewModel>(sut.Edit(TEST_TOURNAMENT_ID));

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
            var sut = BuildSUT();

            // Act
            var result = sut.Edit(testData) as RedirectToRouteResult;

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
            var sut = BuildSUT();

            // Act
            var result = TestExtensions.GetModel<TournamentViewModel>(sut.Edit(testData));

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
            var sut = BuildSUT();
            sut.ModelState.AddModelError(string.Empty, string.Empty);

            // Act
            var result = TestExtensions.GetModel<TournamentViewModel>(sut.Edit(testData));

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
            _tournamentServiceMock
                .Setup(ts => ts.DeleteTeamFromTournament(It.IsAny<int>(), It.IsAny<int>()));
            var sut = BuildSUT();

            // Act
            var jsonResult = sut.DeleteTeamFromTournament(TEST_TOURNAMENT_ID, TEST_ID);
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
            _tournamentServiceMock
                .Setup(ts => ts.DeleteTeamFromTournament(It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new MissingEntityException());
            var sut = BuildSUT();

            // Act
            var jsonResult = sut.DeleteTeamFromTournament(TEST_TOURNAMENT_ID, TEST_ID);
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
            var sut = BuildSUT();

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
            SetupGet(TEST_TOURNAMENT_ID, testData);
            var sut = BuildSUT();

            // Act
            var actual = TestExtensions.GetModel<TournamentViewModel>(sut.Delete(TEST_TOURNAMENT_ID));

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
            var sut = BuildSUT();

            // Act
            var result = sut.DeleteConfirmed(TEST_TOURNAMENT_ID);

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
            var sut = BuildSUT();

            // Act
            var result = sut.DeleteConfirmed(TEST_TOURNAMENT_ID) as RedirectToRouteResult;

            // Assert
            VerifyDelete(TEST_TOURNAMENT_ID, Times.Once());
            VerifyRedirect(INDEX_ACTION_NAME, result);
        }

        #endregion

        #region SwapRounds

        /// <summary>
        /// Test for SwapRounds method.
        /// Wrong tournament Id passed. View with error message is returned.
        /// </summary>
        [TestMethod]
        public void SwapRounds_NonExistentTournament_ErrorViewIsReturned()
        {
            // Arrange
            SetupGet(TEST_TOURNAMENT_ID, null as Tournament);
            var sut = BuildSUT();

            // Act
            var result = TestExtensions
                .GetModel<ScheduleViewModel>(sut.SwapRounds(
                    TEST_TOURNAMENT_ID,
                    FIRST_ROUND_NUMBER,
                    SECOND_ROUND_NUMBER));

            // Assert
            Assert.IsFalse(sut.ModelState.IsValid);
            Assert.IsTrue(sut.ModelState.ContainsKey("LoadError"));
            Assert.IsNull(result);
        }

        /// <summary>
        /// Test for SwapRounds method. Games are exist.
        /// All games are swapped.
        /// </summary>
        [TestMethod]
        public void SwapRounds_ExistentGames_GamesIsSwapped()
        {
            // Arrange
            var tournament = new TournamentScheduleDtoBuilder().Build();

            SetupGetScheduleInfo(TEST_TOURNAMENT_ID, tournament);

            var sut = BuildSUT();

            // Act
            var result = sut
                .SwapRounds(TEST_TOURNAMENT_ID, FIRST_ROUND_NUMBER, SECOND_ROUND_NUMBER) as RedirectToRouteResult;

            // Assert
            VerifyRedirect("ShowSchedule", result);
        }

        /// <summary>
        /// Test for SwapRounds method. Some game are not exist.
        /// All games are swapped.
        /// </summary>
        [TestMethod]
        public void SwapRounds_NonExistentGames_ErrorViewIsReturned()
        {
            // Arrange
            var tournament = new TournamentScheduleDtoBuilder().Build();
            SetupGetScheduleInfo(TEST_TOURNAMENT_ID, tournament);
            _gameServiceMock.Setup(tr => tr.SwapRounds(
                TEST_TOURNAMENT_ID,
                FIRST_ROUND_NUMBER,
                SECOND_ROUND_NUMBER))
                .Throws(new MissingEntityException());

            var sut = BuildSUT();

            // Act
            var result = TestExtensions.GetModel<ScheduleViewModel>(sut.SwapRounds(
                TEST_TOURNAMENT_ID,
                FIRST_ROUND_NUMBER,
                SECOND_ROUND_NUMBER));

            // Assert
            Assert.IsFalse(sut.ModelState.IsValid);
            Assert.IsTrue(sut.ModelState.ContainsKey("LoadError"));
            Assert.IsNull(result);
        }

        #endregion

        #region ApplyForTournament

        [TestMethod]
        public void ApplyForTournament_TournamentExists_TournamentApplyViewModelReturned()
        {
            // Arrange
            var testData = MakeTestTournament(TEST_TOURNAMENT_ID);
            SetupGet(TEST_TOURNAMENT_ID, testData);
            SetupGetNonTournamentTeams(MakeTestTeams(), TEST_TOURNAMENT_ID);

            var expected = MakeTestTournamentApplyViewModel();
            var sut = BuildSUT();

            // Act
            var actual = TestExtensions.GetModel<TournamentApplyViewModel>(sut.ApplyForTournament(TEST_TOURNAMENT_ID));

            // Assert
            TestHelper.AreEqual<TournamentApplyViewModel>(expected, actual, new TournamentApplyViewModelComparer());
        }

        [TestMethod]
        public void ApplyForTournament_NonExistingTournament_HttpNotFoundResultIsReturned()
        {
            // Arrange
            SetupGet(TEST_TOURNAMENT_ID, null as Tournament);
            var sut = BuildSUT();

            // Act
            var actionResult = sut.ApplyForTournament(TEST_TOURNAMENT_ID);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void ApplyForTournamentPostAction_NonExistentUser_JsonResultIsReturned()
        {
            // Arrange
            SetupCurrentUserServiceReturnsUserId(ANONYM_ID);
            var sut = BuildSUT();

            // Act
            var result = sut.ApplyForTournament(TEST_TOURNAMENT_ID, TEST_TEAM_ID);

            // Assert
            Assert.IsNotNull(result, JSON_NO_RIGHTS_MESSAGE);
        }

        [TestMethod]
        public void ApplyForTournamentPostAction_UserExists_JsonResultIsReturned()
        {
            // Arrange
            SetupCurrentUserServiceReturnsUserId(TEST_USER_ID);
            var sut = BuildSUT();

            // Act
            var result = sut.ApplyForTournament(TEST_TOURNAMENT_ID, TEST_TEAM_ID);

            // Assert
            VerifyCreateTournamentRequest(TEST_USER_ID, TEST_TOURNAMENT_ID, TEST_TEAM_ID, Times.Once());
            Assert.IsNotNull(result, JSON_OK_MSG);
        }

        [TestMethod]
        public void ApplyForTournamentPostAction_ArgumentExceptionThrown_JsonResultIsReturned()
        {
            // Arrange
            SetupCurrentUserServiceReturnsUserId(TEST_USER_ID);
            SetupTournamentRequestServiceThrowsArgumentException(TEST_USER_ID, TEST_TOURNAMENT_ID, TEST_TEAM_ID);
            var sut = BuildSUT();

            // Act
            var result = sut.ApplyForTournament(TEST_TOURNAMENT_ID, TEST_TEAM_ID);

            // Assert
            Assert.IsNotNull(result, INVALID_PARAMETR);
        }

        #endregion

        #region Private

        private TournamentsController BuildSUT()
        {
            return new TournamentsController(
                _tournamentServiceMock.Object,
                _gameServiceMock.Object,
                _authServiceMock.Object,
                _tournamentRequestServiceMock.Object,
                _currentUserServiceMock.Object);
        }

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

        private TournamentApplyViewModel MakeTestTournamentApplyViewModel()
        {
            return new TournamentApplyViewModelBuilder().Build();
        }

        private GameViewModel MakeTestGameViewModel()
        {
            return new GameViewModelBuilder().Build();
        }

        private Game MakeTestGame()
        {
            return new GameBuilder().Build();
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
            _tournamentServiceMock.Setup(tr => tr.GetActual()).Returns(tournaments);
        }

        private void SetupGetFinished(List<Tournament> tournaments)
        {
            _tournamentServiceMock.Setup(tr => tr.GetFinished()).Returns(tournaments);
        }

        private void SetupGetTournamentTeams(List<Team> teams, int tournamentId)
        {
            _tournamentServiceMock
                .Setup(tr => tr.GetAllTournamentTeams(tournamentId))
                .Returns(teams);
        }

        private void SetupGetNonTournamentTeams(List<Team> teams, int tournamentId)
        {
            _tournamentServiceMock
                .Setup(tr => tr.GetAllNoTournamentTeams(tournamentId))
                .Returns(teams);
        }

        private void SetupGetTournamentNumberOfRounds(TournamentScheduleDto tournament, byte numberOfRounds)
        {
            _tournamentServiceMock
                .Setup(tr => tr.GetNumberOfRounds(tournament))
                .Returns(numberOfRounds);
        }

        private void SetupGet(int tournamentId, Tournament tournament)
        {
            _tournamentServiceMock.Setup(tr => tr.Get(tournamentId)).Returns(tournament);
        }

        private void SetupGetScheduleInfo(int tournamentId, TournamentScheduleDto tournament)
        {
            _tournamentServiceMock.Setup(tr => tr.GetTournamentScheduleInfo(tournamentId)).Returns(tournament);
        }

        private void SetupGetGame(int gameId, GameResultDto game)
        {
            _gameServiceMock.Setup(gs => gs.Get(gameId)).Returns(game);
        }

        private void SetupGetTournamentResults(int tournamentId, List<GameResultDto> expectedGames)
        {
            _gameServiceMock.Setup(t => t.GetTournamentResults(It.IsAny<int>())).Returns(expectedGames);
        }

        private void SetupCurrentUserServiceReturnsUserId(int id)
        {
            _currentUserServiceMock.Setup(m => m.GetCurrentUserId()).Returns(id);
        }

        private void SetupCreateThrowsTournamentValidationException()
        {
            _tournamentServiceMock.Setup(ts => ts.Create(It.IsAny<Tournament>()))
                .Throws(new TournamentValidationException(string.Empty, string.Empty, string.Empty));
        }

        private void SetupEditThrowsTournamentValidationException()
        {
            _tournamentServiceMock.Setup(ts => ts.Edit(It.IsAny<Tournament>()))
                .Throws(new TournamentValidationException(string.Empty, string.Empty, string.Empty));
        }

        private void SetupControllerContext(TournamentsController sut)
        {
            sut.ControllerContext = new ControllerContext(_httpContextMock.Object, new RouteData(), sut);
        }

        private void SetupRequestRawUrl(string rawUrl)
        {
            _httpRequestMock.Setup(x => x.RawUrl).Returns(rawUrl);
        }

        private void SetupUserServiceReturnsValidUserId(int userId)
        {
            _currentUserServiceMock.Setup(m => m.GetCurrentUserId()).Returns(userId);
        }

        private void SetupTournamentRequestServiceThrowsArgumentException(int userId, int tournamentId, int teamId)
        {
            _tournamentRequestServiceMock.Setup(ts => ts.Create(userId, tournamentId, teamId))
                .Throws(new ArgumentException(INVALID_PARAMETR));
        }

        private void VerifyCreate(Times times)
        {
            _tournamentServiceMock.Verify(ts => ts.Create(It.IsAny<Tournament>()), times);
        }

        private void VerifyEdit(Times times)
        {
            _tournamentServiceMock.Verify(ts => ts.Edit(It.IsAny<Tournament>()), times);
        }

        private void VerifyDelete(int tournamentId, Times times)
        {
            _tournamentServiceMock.Verify(ts => ts.Delete(tournamentId), times);
        }

        private void VerifyRedirect(string actionName, RedirectToRouteResult result)
        {
            Assert.AreEqual(actionName, result.RouteValues[ROUTE_VALUES_KEY]);
        }

        private void VerifyCreateGame(Times times)
        {
            _gameServiceMock.Verify(gs => gs.Create(It.IsAny<Game>()), times);
        }

        private void VerifyCreateTournamentRequest(int userId, int tournamentId, int teamId, Times times)
        {
            _tournamentRequestServiceMock.Verify(ts => ts.Create(userId, tournamentId, teamId), times);
        }

        private void VerifySwapRounds(int tournamentId, byte firstRoundNumber, byte secondRoundNumber)
        {
            _gameServiceMock.Verify(gs => gs.SwapRounds(tournamentId, firstRoundNumber, secondRoundNumber));
        }

        private void VerifyEditGame(Times times)
        {
            _gameServiceMock.Verify(gs => gs.Edit(It.IsAny<Game>()), times);
        }

        private void VerifyInvalidModelState(
            string expectedKey,
            GameViewModel gameViewModel,
            TournamentsController sut)
        {
            Assert.IsFalse(sut.ModelState.IsValid);
            Assert.IsTrue(sut.ModelState.ContainsKey(expectedKey));
            Assert.IsNull(gameViewModel);
        }

        private void VerifyGetAllowedOperations(Times times)
        {
            _authServiceMock.Verify(tr => tr.GetAllowedOperations(It.IsAny<List<AuthOperation>>()), times);
        }

        private void VerifyGetAllowedOperations(List<AuthOperation> allowedOperations, Times times)
        {
            _authServiceMock.Verify(tr => tr.GetAllowedOperations(allowedOperations), times);
        }

        private void AssertEqual(GameViewModel x, GameViewModel y)
        {
            Assert.AreEqual(x.Id, y.Id, "Actual game Id doesn't match expected");
            Assert.AreEqual(x.TournamentId, y.TournamentId, "Actual TournamentId doesn't match expected");
            Assert.AreEqual(x.HomeTeamId, y.HomeTeamId, "Actual HomeTeamId doesn't match expected");
            Assert.AreEqual(x.AwayTeamId, y.AwayTeamId, "Actual AwayTeamId doesn't match expected");
            Assert.AreEqual(x.Round, y.Round, "Actual Round number doesn't match expected");
            Assert.AreEqual(x.GameDate, y.GameDate, "Actual GameDate doesn't match expected");
            Assert.AreEqual(x.GameNumber, y.GameNumber, "Actual GameNumber doesn't match expected");

            Assert.IsTrue(
                x.Teams != null &&
                y.Teams != null &&
                x.Teams.Select(
                    team => new { Text = team.Text, Value = team.Value }).SequenceEqual(
                    y.Teams.Select(team => new { Text = team.Text, Value = team.Value })),
                "Actual Teams list doesn't match expected");

            Assert.IsTrue(
                          x.Rounds != null &&
                          y.Rounds != null &&
                         (x.Rounds.Items as IEnumerable<int>).SequenceEqual(
                          y.Rounds.Items as IEnumerable<int>),
                          "Actual Rounds list doesn't match expected");
        }
        #endregion
    }
}