namespace VolleyManagement.UnitTests.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Http.Results;
    using Contracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using Services.TournamentService;
    using UI.Areas.WebApi.ViewModels.Tournaments;
    using VolleyManagement.Domain.GamesAggregate;
    using VolleyManagement.Domain.Tournaments;
    using VolleyManagement.Domain.TournamentsAggregate;
    using VolleyManagement.UI.Areas.WebApi.Controllers;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Games;
    using VolleyManagement.UnitTests.Services.GameService;
    using VolleyManagement.UnitTests.WebApi.ViewModels;

    /// <summary>
    /// Tests for TournamentController class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TournamentControllerTests
    {
        /// <summary>
        /// ID for tests
        /// </summary>
        private const int SPECIFIC_TOURNAMENT_ID = 2;
        private const int TOURNAMENT_ID = 1;

        /// <summary>
        /// Test Fixture
        /// </summary>
        private readonly TournamentServiceTestFixture _testFixture = new TournamentServiceTestFixture();

        /// <summary>
        /// Tournaments Service Mock
        /// </summary>
        private readonly Mock<ITournamentService> _tournamentServiceMock = new Mock<ITournamentService>();

        /// <summary>
        /// Game Service Mock
        /// </summary>
        private readonly Mock<IGameService> _gameServiceMock = new Mock<IGameService>();

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
            _kernel = new StandardKernel();
            _kernel.Bind<ITournamentService>()
                   .ToConstant(_tournamentServiceMock.Object);
            _kernel.Bind<IGameService>()
                   .ToConstant(_gameServiceMock.Object);
        }

        #region GetAllTournaments
        [TestMethod]
        public void GetAllTournaments_TournamentsExist_TournamentsReturned()
        {
            // Arrange
            var sut = _kernel.Get<TournamentsController>();
            var testData = _testFixture.TestTournaments()
                                            .Build();
            MockGetTournaments(testData);
            var expected = new TournamentViewModelServiceTestFixture()
                                            .TestTournaments()
                                            .Build()
                                            .ToList();

            // Act
            var actual = sut.GetAllTournaments().ToList();

            // Assert
            _tournamentServiceMock.Verify(ts => ts.Get(), Times.Once());
            CollectionAssert.AreEqual(
                expected,
                actual,
                new TournamentViewModelComparer());
        }

        [TestMethod]
        public void GetAllTournaments_NoTournamentsAwailable_EmptyCollectionReturned()
        {
            // Arrange
            var sut = _kernel.Get<TournamentsController>();
            MockGetTournaments(new List<Tournament>());
            var expected = new List<TournamentViewModel>();

            // Act
            var actual = sut.GetAllTournaments().ToList();

            // Assert
            _tournamentServiceMock.Verify(ts => ts.Get(), Times.Once());
            CollectionAssert.AreEqual(
                expected,
                actual,
                new TournamentViewModelComparer());
        }
        #endregion

        #region GetTournament
        [TestMethod]
        public void GetTournament_SpecificTournamentExist_TournamentReturned()
        {
            // Arrange
            var sut = _kernel.Get<TournamentsController>();
            var testData = new TournamentBuilder()
                               .WithId(1)
                               .WithName("test")
                               .WithDescription("Volley")
                               .WithScheme(TournamentSchemeEnum.Two)
                               .WithSeason(2016)
                               .WithRegulationsLink("volley.dp.ua")
                               .Build();
            MockGetTournament(testData, SPECIFIC_TOURNAMENT_ID);
            var expected = new TournamentViewModel
            {
                Id = testData.Id,
                Name = testData.Name,
                Description = testData.Description,
                Scheme = testData.Scheme.ToDescription(),
                Season = testData.Season,
                RegulationsLink = testData.RegulationsLink
            };

            // Act
            var result = sut.GetTournament(SPECIFIC_TOURNAMENT_ID);
            var actual = result as OkNegotiatedContentResult<TournamentViewModel>;

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<TournamentViewModel>));
            TestHelper.AreEqual(expected, actual.Content, new TournamentViewModelComparer());
        }

        [TestMethod]
        public void GetTournament_NonExistentTournament_NotFoundIsReturned()
        {
            // Arrange
            var sut = _kernel.Get<TournamentsController>();
            var testData = null as Tournament;
            MockGetTournament(testData, SPECIFIC_TOURNAMENT_ID);

            // Act
            var result = sut.GetTournament(SPECIFIC_TOURNAMENT_ID);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
        #endregion

        #region GetSchedule
        /// <summary>
        /// Test for GetSchedule method.
        /// Tournament with games; games list returned
        /// </summary>
        [TestMethod]
        public void GetSchedule_TournamentWithGames_GamesListReturned()
        {
            // Arrange
            var sut = _kernel.Get<TournamentsController>();
            var testTournament = new TournamentBuilder().Build();
            MockGetTournament(testTournament, TOURNAMENT_ID);
            var testGames = new GameServiceTestFixture().TestGameResults().Build();
            SetupGetTournamentResults(TOURNAMENT_ID, testGames);
            var expected = new GameViewModelTestFixture().TestGames().Build().ToList();

            // Act
            var actual = sut.GetSchedule(TOURNAMENT_ID).ToList();

            // Assert
            _gameServiceMock.Verify(ts => ts.GetTournamentResults(TOURNAMENT_ID), Times.Once());
            CollectionAssert.AreEqual(expected, actual, new GameViewModelComparer());
        }

        /// <summary>
        /// Test for GetSchedule method.
        /// Tournament without games; empty games list returned
        /// </summary>
        [TestMethod]
        public void GetSchedule_TournamentWithoutGames_EmptyGamesListReturned()
        {
            // Arrange
            var sut = _kernel.Get<TournamentsController>();
            var testTournament = new TournamentBuilder().Build();
            MockGetTournament(testTournament, TOURNAMENT_ID);
            SetupGetTournamentResults(TOURNAMENT_ID, new List<GameResultDto>());
            var expected = new List<GameViewModel>();

            // Act
            var actual = sut.GetSchedule(TOURNAMENT_ID).ToList();

            // Assert
            _gameServiceMock.Verify(ts => ts.GetTournamentResults(TOURNAMENT_ID), Times.Once());
            CollectionAssert.AreEqual(expected, actual, new GameViewModelComparer());
        }

        /// <summary>
        /// Test for GetSchedule method.
        /// Tournament with games; Result does not exist, games list with result null returned
        /// </summary>
        [TestMethod]
        public void GetSchedule_TournamentWithGamesNoResult_GamesListReturned()
        {
            // Arrange
            var sut = _kernel.Get<TournamentsController>();
            var testTournament = new TournamentBuilder().Build();
            MockGetTournament(testTournament, TOURNAMENT_ID);
            var testGames = new GameServiceTestFixture().TestGamesWithoutResult().Build();
            SetupGetTournamentResults(TOURNAMENT_ID, testGames);
            var expected = new GameViewModelTestFixture().TestGamesWithoutResult().Build().ToList();

            // Act
            var actual = sut.GetSchedule(TOURNAMENT_ID).ToList();

            // Assert
            _gameServiceMock.Verify(ts => ts.GetTournamentResults(TOURNAMENT_ID), Times.Once());
            CollectionAssert.AreEqual(expected, actual, new GameViewModelComparer());
        }
        #endregion

        #region Private
        private void MockGetTournaments(List<Tournament> testData)
        {
            _tournamentServiceMock.Setup(tr => tr.Get()).Returns(testData);
        }

        private void MockGetTournament(Tournament tournament, int id)
        {
            _tournamentServiceMock.Setup(tr => tr.Get(id)).Returns(tournament);
        }

        private void SetupGetTournamentResults(int tournamentId, List<GameResultDto> expectedGames)
        {
            this._gameServiceMock.Setup(t => t.GetTournamentResults(It.IsAny<int>())).Returns(expectedGames);
        }
        #endregion
    }
}