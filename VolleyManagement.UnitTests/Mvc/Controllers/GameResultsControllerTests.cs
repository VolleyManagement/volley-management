namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Domain.GamesAggregate;
    using VolleyManagement.Domain.TeamsAggregate;
    using VolleyManagement.UI.Areas.Mvc.Controllers;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.GameResults;
    using VolleyManagement.UnitTests.Mvc.ViewModels;
    using VolleyManagement.UnitTests.Services.GameService;
    using VolleyManagement.UnitTests.Services.TeamService;

    /// <summary>
    /// Tests for MVC <see cref="GameResultsControllerTests"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class GameResultsControllerTests
    {
        #region Consts

        private const int TOURNAMENT_ID = 1;
        private const string TOURNAMENT_NAME = "Name";
        private const int GAME_RESULT_ID = 2;
        private const int GAME_RESULTS_ID = 1;
        private const int HOME_TEAM_ID = 10;
        private const int AWAY_TEAM_ID = 11;
        private const string HOME_TEAM_NAME = "Home";
        private const string AWAY_TEAM_NAME = "Away";
        private const string DETAILS_ACTION_NAME = "Details";
        private const string ASSERT_FAIL_VIEW_MODEL_MESSAGE = "View model must be returned to user.";
        #endregion

        #region Fields

        private IKernel _kernel;

        private Mock<ITeamService> _teamServiceMock = new Mock<ITeamService>();

        private Mock<IGameService> _gameServiceMock = new Mock<IGameService>();
        private Mock<ITournamentService> _tournamentServiceMock = new Mock<ITournamentService>();
        #endregion

        #region Init

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<ITeamService>()
                .ToConstant(_teamServiceMock.Object);
            _kernel.Bind<IGameService>()
                .ToConstant(_gameServiceMock.Object);
        }

        #endregion

        #region Tests

        /// <summary>
        /// Test for Create POST method. Valid model passed. Games result created.
        /// </summary>
        [TestMethod]
        public void CreatePostAction_ValidModel_Created()
        {
            // Arrange
            var gameResult = new GameResultViewModelBuilder()
                                .WithTournamentId(TOURNAMENT_ID)
                                .Build();
            Game expectedGameResult = gameResult.ToDomain();

            Game actualGameResult = null;
            _gameServiceMock.Setup(h => h.Create(It.IsAny<Game>()))
                .Callback<Game>(r => actualGameResult = r);

            var sut = this._kernel.Get<GameResultsController>();

            // Act
            var result = sut.Create(gameResult) as RedirectToRouteResult;

            // Assert
            TestHelper.AreEqual(expectedGameResult, actualGameResult, new GameComparer());
        }

        /// <summary>
        /// Test for Create POST method. Valid model passed. Redirected to Details view.
        /// </summary>
        [TestMethod]
        public void CreatePostAction_ValidModel_RedirectedToDetailsView()
        {
            // Arrange
            var game = new GameResultViewModelBuilder()
                                .WithTournamentId(TOURNAMENT_ID)
                                .Build();
            Game expectedGameResult = game.ToDomain();

            Game actualGameResult = null;
            _gameServiceMock.Setup(h => h.Create(It.IsAny<Game>()))
                .Callback<Game>(r => actualGameResult = r);

            var sut = this._kernel.Get<GameResultsController>();

            // Act
            var result = sut.Create(game) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual(DETAILS_ACTION_NAME, result.RouteValues["action"]);
        }

        /// <summary>
        /// Test for Create POST method. Invalid model passed. Redirect to page of create view.
        /// </summary>
        [TestMethod]
        public void CreatePostAction_InvalidModel_ExceptionThrown()
        {
            // Arrange
            var gameResult = new GameResultViewModelBuilder()
                                .WithTournamentId(TOURNAMENT_ID)
                                .WithHomeTeamId(HOME_TEAM_ID)
                                .WithAwayTeamId(HOME_TEAM_ID)
                                .Build();

            var sut = this._kernel.Get<GameResultsController>();
            _gameServiceMock.Setup(gr => gr.Create(It.IsAny<Game>())).Throws(new ArgumentException());
            _teamServiceMock.Setup(ts => ts.Get()).Returns(new List<Team>());

            // Act
            var actual = TestExtensions.GetModel<GameResultViewModel>(sut.Create(gameResult));

            // Assert
            TestHelper.AreEqual(actual, gameResult, new GameResultViewModelComparer());
        }

        /// <summary>
        /// Test for Create method (GET action). Game result view model is requested.  Game result view model is returned.
        /// </summary>
        [TestMethod]
        public void CreateGetAction_GameResultViewModelRequested_GameResultViewModelIsReturned()
        {
            // Arrange
            var controller = _kernel.Get<GameResultsController>();
            _teamServiceMock.Setup(ts => ts.Get()).Returns(new List<Team>());
            var expected = new GameResultViewModel
            {
                TournamentId = TOURNAMENT_ID
            };

            // Act
            var actual = TestExtensions.GetModel<GameResultViewModel>(controller.Create(TOURNAMENT_ID));

            // Assert
            TestHelper.AreEqual<GameResultViewModel>(expected, actual, new GameResultViewModelComparer());
        }

        /// <summary>
        /// Test for Edit method (GET action). Valid game result id.  Game result view model is returned.
        /// </summary>
        [TestMethod]
        public void EditGetAction_ValidGameResultId_GameResultViewModelIsReturned()
        {
            // Arrange
            var controller = _kernel.Get<GameResultsController>();
            _teamServiceMock.Setup(ts => ts.Get()).Returns(new List<Team>());
            var testData = new GameServiceTestFixture().TestGameResults().Build();
            var expected = new GameResultViewModelBuilder()
                .WithTournamentId(TOURNAMENT_ID)
                .WithAwayTeamName("TeamNameB")
                .WithHomeTeamName("TeamNameA")
                .WithId(GAME_RESULTS_ID)
                .WithSetsScore(3, 2)
                .WithSetScores(new List<Score>
                    {
                        new Score(25, 20),
                        new Score(24, 26),
                        new Score(28, 30),
                        new Score(25, 22),
                        new Score(27, 25)
                    })
                .Build();
            SetupGet(TOURNAMENT_ID, testData.ElementAt(0));

            // Act
            var actual = TestExtensions.GetModel<GameResultViewModel>(controller.Edit(TOURNAMENT_ID));

            // Assert
            TestHelper.AreEqual<GameResultViewModel>(expected, actual, new GameResultViewModelComparer());
        }

        /// <summary>
        /// Test for Edit method. GameResult with specified identifier does not exist. HttpNotFoundResult is returned.
        /// </summary>
        [TestMethod]
        public void Edit_NotExistedGameResult_HttpNotFoundResultIsReturned()
        {
            // Arrange
            SetupGet(GAME_RESULT_ID, null as GameResultDto);

            // Act
            var controller = _kernel.Get<GameResultsController>();
            var result = controller.Edit(GAME_RESULT_ID);

            // Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        /// <summary>
        /// Test for edit post method. Invalid game results Id - redirect to  the edit view.
        /// </summary>
        [TestMethod]
        public void EditPost_MissingEntityException_RedirectToGameResultViewModel()
        {
            // Arrange
            var gameResultViewModel = new GameResultViewModelBuilder().Build();

            _gameServiceMock.Setup(grs => grs.Edit(It.IsAny<Game>()))
                                  .Throws(new MissingEntityException());

            var sut = this._kernel.Get<GameResultsController>();

            // Act
            var result = TestExtensions.GetModel<GameResultViewModel>(sut.Edit(gameResultViewModel));

            // Assert
            VerifyEdit(Times.Once());
            Assert.IsNotNull(result, ASSERT_FAIL_VIEW_MODEL_MESSAGE);
        }

        /// <summary>
        /// Test for edit post method. Invalid game results Id - redirect to  the edit view.
        /// </summary>
        [TestMethod]
        public void EditPost_ValidEntity_GameResultViewModelIsReturned()
        {
            var controller = _kernel.Get<GameResultsController>();
            var testData = new GameResultViewModelBuilder().Build();
            _teamServiceMock.Setup(ts => ts.Get()).Returns(new List<Team>());

            // Act
            var result = controller.Edit(testData);

            // Assert
            VerifyEdit(Times.Once());
            Assert.IsNotNull(result, ASSERT_FAIL_VIEW_MODEL_MESSAGE);
        }

        /// <summary>
        /// Test for edit post method. Valid game results - redirect to  the tournament results.
        /// </summary>
        [TestMethod]
        public void EditPost_ValidEntity_RedirectToResultsList()
        {
            // Arrange
            var gameResultViewModel = new GameResultViewModelBuilder().Build();
            var sut = this._kernel.Get<GameResultsController>();

            // Act
            var result = sut.Edit(gameResultViewModel);

            // Assert
            VerifyEdit(Times.Once());
            Assert.AreEqual(result.GetType(), typeof(RedirectToRouteResult));
        }

        /// <summary>
        /// Test for Edit method (POST action). Game Result  view model is not valid.
        /// Game Result is not updated and Game Result view model is returned.
        /// </summary>
        [TestMethod]
        public void EditPostAction_InvalidGameResultViewModel_GameResultViewModelIsReturned()
        {
            // Arrange
            var testData = new GameResultViewModelBuilder().Build();
            var sut = this._kernel.Get<GameResultsController>();
            sut.ModelState.AddModelError(string.Empty, string.Empty);

            // Act
            var result = TestExtensions.GetModel<GameResultViewModel>(sut.Edit(testData));

            // Assert
            VerifyEdit(Times.Never());
            Assert.IsNotNull(result, ASSERT_FAIL_VIEW_MODEL_MESSAGE);
        }

        /// <summary>
        /// Test for delete post method. Valid game result id - redirect to  the tournament results.
        /// </summary>
        [TestMethod]
        public void DeletePost_ValidId_Deleted()
        {
            // Arrange
            var gameResult = new GameResultDtoBuilder().Build();

            _gameServiceMock.Setup(grs => grs.Get(It.IsAny<int>())).Returns(gameResult);

            var sut = this._kernel.Get<GameResultsController>();

            // Act
            sut.Delete(GAME_RESULT_ID);

            // Assert
            _gameServiceMock.Verify(grs => grs.Delete(GAME_RESULT_ID), Times.Once);
        }

        /// <summary>
        /// Test for delete post method. Invalid game result id - Missing entity exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(MissingEntityException))]
        public void DeletePost_InvalidId_MissingEntityExceptionThrows()
        {
            // Arrange
            _gameServiceMock.Setup(grs => grs.Get(It.IsAny<int>()))
                .Returns((GameResultDto)null);

            var sut = this._kernel.Get<GameResultsController>();

            // Act
            sut.Delete(GAME_RESULT_ID);

            // Assert
            _gameServiceMock.Verify(grs => grs.Delete(GAME_RESULT_ID), Times.Never);
        }

        /// <summary>
        /// Test for TournamentResults method. Tournament results are requested. Tournament results are returned.
        /// </summary>
        [TestMethod]
        public void TournamentResults_TournamentResultsRequested_TournamentResultsReturned()
        {
            // Arrange
            var testTournamentResults = new GameServiceTestFixture().TestGameResults().Build();
            var sut = this._kernel.Get<GameResultsController>();
            var expected = new TournamentResultsViewModelBuilder().Build();

            SetupGameResultsGetTournamentResults(TOURNAMENT_ID, testTournamentResults);

            // Act
            var actual = TestExtensions.GetModel<TournamentResultsViewModel>(sut.TournamentResults(TOURNAMENT_ID, TOURNAMENT_NAME));

            // Assert
            TestHelper.AreEqual(expected, actual, new TournamentResultsViewModelComparer());
        }

        #endregion

        #region Additional Methods

        private void VerifyEdit(Times times)
        {
            this._gameServiceMock.Verify(ts => ts.Edit(It.IsAny<Game>()), times);
        }

        private void SetupGet(int gameResultId, GameResultDto gameResult)
        {
            this._gameServiceMock.Setup(tr => tr.Get(gameResultId)).Returns(gameResult);
        }

        private void SetupGameResultsGetTournamentResults(int tournamentId, List<GameResultDto> testData)
        {
            _gameServiceMock.Setup(m => m.GetTournamentResults(It.Is<int>(id => id == tournamentId))).Returns(testData);
        }

        private void MockTeams()
        {
            var homeTeam = new TeamBuilder()
                            .WithId(HOME_TEAM_ID)
                            .WithName(HOME_TEAM_NAME)
                            .Build();

            var awayTeam = new TeamBuilder()
                .WithId(AWAY_TEAM_ID)
                .WithName(AWAY_TEAM_NAME)
                .Build();

            _teamServiceMock.Setup(ts => ts.Get(HOME_TEAM_ID)).Returns(homeTeam);
            _teamServiceMock.Setup(ts => ts.Get(AWAY_TEAM_ID)).Returns(awayTeam);
            _teamServiceMock.Setup(ts => ts.Get()).Returns(new List<Team>() { homeTeam, awayTeam });
        }

        #endregion
    }
}
