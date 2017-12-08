namespace VolleyManagement.UnitTests.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Http.Results;
    using Contracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Services.TournamentService;
    using UI.Areas.WebApi.ViewModels.Tournaments;
    using VolleyManagement.Crosscutting.Contracts.Extensions;
    using VolleyManagement.Domain.GameReportsAggregate;
    using VolleyManagement.Domain.GamesAggregate;
    using VolleyManagement.Domain.TournamentsAggregate;
    using VolleyManagement.UI.Areas.WebApi.Controllers;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.GameReports;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Games;
    using VolleyManagement.UI.Areas.WebAPI.ViewModels.Schedule;
    using VolleyManagement.UnitTests.Services.GameReportService;
    using VolleyManagement.UnitTests.Services.GameService;
    using VolleyManagement.UnitTests.WebApi.ViewModels;
    using VolleyManagement.UnitTests.WebApi.ViewModels.Schedule;

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
        /// Count of empty list
        /// </summary>
        private const int EMPTY_LIST_COUNT = 0;

        /// <summary>
        /// Index of first element in collection
        /// </summary>
        private const int FIRST_ELEMENT_INDEX = 0;

        /// <summary>
        /// Index of second element in collection
        /// </summary>
        private const int SECOND_ELEMENT_INDEX = 1;

        private readonly TournamentServiceTestFixture _testFixture = new TournamentServiceTestFixture();

        private Mock<ITournamentService> _tournamentServiceMock = new Mock<ITournamentService>();
        private Mock<IGameReportService> _gameReportServiceMock = new Mock<IGameReportService>();
        private Mock<IGameService> _gameServiceMock = new Mock<IGameService>();

        /// <summary>
        /// Initializes test data
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _tournamentServiceMock = new Mock<ITournamentService>();
            _gameReportServiceMock = new Mock<IGameReportService>();
            _gameServiceMock = new Mock<IGameService>();
        }

        #region GetAllTournaments
        [TestMethod]
        public void GetAllTournaments_TournamentsExist_TournamentsReturned()
        {
            // Arrange
            var testData = _testFixture.TestTournaments()
                                            .Build();
            MockGetTournaments(testData);
            var expected = new TournamentViewModelServiceTestFixture()
                                            .TestTournaments()
                                            .Build()
                                            .ToList();

            var sut = BuildSUT();

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
            MockGetTournaments(new List<Tournament>());
            var expected = new List<TournamentViewModel>();

            var sut = BuildSUT();

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

            var sut = BuildSUT();

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
            var testData = null as Tournament;
            MockGetTournament(testData, SPECIFIC_TOURNAMENT_ID);

            var sut = BuildSUT();

            // Act
            var result = sut.GetTournament(SPECIFIC_TOURNAMENT_ID);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetStandings_StandingsExist_StandingsReturned()
        {
            // Arrange
            var testData = new List<List<StandingsEntry>> { new StandingsTestFixture().TestStandings().Build() };
            var expected = new StandingsEntryViewModelServiceTestFixture().TestEntries().Build().ToList();
            MockGetStandings(testData, SPECIFIC_TOURNAMENT_ID);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetTournamentStandings(SPECIFIC_TOURNAMENT_ID).First().ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual, new StandingsEntryViewModelComparer());
        }

        [TestMethod]
        public void GetStandings_NoStandingsExist_EmptyListReturned()
        {
            // Arrange
            var testData = new List<List<StandingsEntry>> { new StandingsTestFixture().Build() };
            var expected = new StandingsEntryViewModelServiceTestFixture().Build().ToList();
            MockGetStandings(testData, SPECIFIC_TOURNAMENT_ID);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetTournamentStandings(SPECIFIC_TOURNAMENT_ID).First().ToList();

            // Assert
            Assert.AreEqual(actual.Count, EMPTY_LIST_COUNT);
        }

        [TestMethod]
        [Ignore]
        public void GetPivotStandings_PivotStandingsExist_PivotStandingsReturned()
        {
            // Arrange
            var testTeams = new TeamStandingsTestFixture().TestTeamStandings().Build().ToList();
            var testGames = new ShortGameResultDtoTetsFixture().GetShortResults().Build().ToList();
            var testData = new PivotStandingsDto(testTeams, testGames);

            MockGetPivotStandings(testData, SPECIFIC_TOURNAMENT_ID);

            var expectedTeams = new PivotStandingsEntryViewModelServiceTestFixture().TestEntries().Build();
            var expectedGames = new PivotStandingsGameViewModelServiceTestFixture().TestEntries().Build();
            var expected = new PivotStandingsViewModel();
            expected.GamesStandings = expectedGames.ToList();
            expected.TeamsStandings = expectedTeams.ToList();

            var sut = BuildSUT();

            // Act
            var actual = sut.GetTournamentPivotStandings(SPECIFIC_TOURNAMENT_ID);

            // Assert
            // TestHelper.AreEqual(expected, actual, new PivotStandingsViewModelComparer());
        }

        [TestMethod]
        public void GetPivotStandings_NoStandingsExist_EmptyListReturned()
        {
            // Arrange
            var testData = new PivotStandingsDto(new List<TeamStandingsDto>(), new List<ShortGameResultDto>());
            MockGetPivotStandings(testData, SPECIFIC_TOURNAMENT_ID);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetTournamentPivotStandings(SPECIFIC_TOURNAMENT_ID);

            // Assert
            Assert.AreEqual(actual.First().TeamsStandings.Count, EMPTY_LIST_COUNT);
        }
        #endregion

        #region Private

        private TournamentsController BuildSUT()
        {
            return new TournamentsController(
                _tournamentServiceMock.Object,
                _gameServiceMock.Object,
                _gameReportServiceMock.Object);
        }

        private void MockGetStandings(List<List<StandingsEntry>> testData, int id)
        {
            _gameReportServiceMock.Setup(gr => gr.GetStandings(id)).Returns(testData);
        }

        private void MockGetPivotStandings(PivotStandingsDto testData, int id)
        {
            _gameReportServiceMock.Setup(gr => gr.GetPivotStandings(id)).Returns(new List<PivotStandingsDto> { testData });
        }

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
            _gameServiceMock.Setup(t => t.GetTournamentResults(It.IsAny<int>())).Returns(expectedGames);
        }
        #endregion
    }
}