namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;

    using Contracts;
    using Domain.GameResultsAggregate;
    using Domain.TeamsAggregate;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;

    using Services.TeamService;

    using UI.Areas.Mvc.Controllers;
    using UI.Areas.Mvc.ViewModels.GameResults;

    using ViewModels;

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

        #endregion

        #region Fields

        private IKernel _kernel;

        private Mock<ITeamService> _teamServiceMock = new Mock<ITeamService>();

        private Mock<IGameResultService> _gameResultServiceMock = new Mock<IGameResultService>();
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
            _kernel.Bind<IGameResultService>()
                .ToConstant(_gameResultServiceMock.Object);
        }

        #endregion

        #region Tests

        /// <summary>
        /// Test for Create POST methos. Valid model passed. Games result created.
        /// </summary>
        [TestMethod]
        public void CreatePostAction_ValidModel_Created()
        {
            // Arrange
            var gameResult = new GameResultViewModelBuilder()
                                .WithTournamentId(TOURNAMENT_ID)
                                .Build();

            var sut = this._kernel.Get<GameResultsController>();

            // Act
            var result = sut.Create(gameResult);

            // Assert
            Assert.AreEqual(result.GetType(), typeof(RedirectToRouteResult));
        }

        /// <summary>
        /// Test for Create POST methos. Invalid model passed. Redirect to page of create view.
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
            _gameResultServiceMock.Setup(gr => gr.Create(It.IsAny<GameResult>())).Throws(new ArgumentException());
            _teamServiceMock.Setup(ts => ts.Get()).Returns(new List<Team>());

            // Act
            var result = sut.Create(gameResult);

            // Assert
            Assert.AreEqual(result.GetType(), typeof(ViewResult));
        }

        /// <summary>
        /// Test for Details methos. Id passed - details view returned.
        /// </summary>
        [TestMethod]
        public void DetailsAction_IdPassed_ViewReturned()
        {
            // Arrange
            var expectedResultViewModel = new GameResultViewModelBuilder()
                                        .WithHomeTeamId(HOME_TEAM_ID)
                                        .WithAwayTeamId(HOME_TEAM_ID)
                                        .WithHomeTeamName(HOME_TEAM_NAME)
                                        .WithAwayTeamName(AWAY_TEAM_NAME)
                                        .Build();

            GameResult gameResultDomainModel = expectedResultViewModel.ToDomain();
            _gameResultServiceMock.Setup(grs => grs.Get(It.IsAny<int>())).Returns(gameResultDomainModel);

            MockTeams();

            var sut = this._kernel.Get<GameResultsController>();

            // Act
            var actual = TestExtensions.GetModel<GameResultViewModel>(sut.Details(GAME_RESULT_ID));

            // Assert
            TestHelper.AreEqual(actual, expectedResultViewModel, new GameResultViewModelComparer());
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
        }

        #endregion
    }
}