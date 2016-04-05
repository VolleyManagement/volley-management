namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
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
        private const int GAME_RESULT_ID = 2;
        private const int HOME_TEAM_ID = 10;
        private const int AWAY_TEAM_ID = 11;
        private const string HOME_TEAM_NAME = "Home";
        private const string AWAY_TEAM_NAME = "Away";
        private const string DETAILS_ACTION_NAME = "Details";
        #endregion

        #region Fields

        private IKernel _kernel;

        private Mock<ITeamService> _teamServiceMock = new Mock<ITeamService>();

        private Mock<IGameService> _gameServiceMock = new Mock<IGameService>();
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
        /// Test for edit post method. Invalid game results Id - redirect to  the edit view.
        /// </summary>
        [TestMethod]
        public void EditPost_MissingEntityException_RedirectToEditView()
        {
            // Arrange
            var gameResultViewModel = new GameResultViewModelBuilder().Build();

            _gameServiceMock.Setup(grs => grs.Edit(It.IsAny<Game>()))
                                  .Throws(new MissingEntityException());

            _teamServiceMock.Setup(ts => ts.Get()).Returns(new List<Team>());
            var sut = this._kernel.Get<GameResultsController>();

            // Act
            var result = sut.Edit(gameResultViewModel);

            // Assert
            Assert.AreEqual(result.GetType(), typeof(ViewResult));
        }

        /// <summary>
        /// Test for edit post method. Valid game results - redirect to  the tournament results.
        /// </summary>
        [TestMethod]
        public void EditPost_ValidEntity_RedirectToResultsList()
        {
            // Arrange
            var gameResultViewModel = new GameResultViewModelBuilder().Build();

            _teamServiceMock.Setup(ts => ts.Get()).Returns(new List<Team>());
            var sut = this._kernel.Get<GameResultsController>();

            // Act
            var result = sut.Edit(gameResultViewModel);

            // Assert
            Assert.AreEqual(result.GetType(), typeof(RedirectToRouteResult));
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

        #endregion

        #region Additional Methods

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
