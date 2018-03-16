using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Http.Results;
using VolleyManagement.Contracts;
using VolleyManagement.Crosscutting.Contracts.Extensions;
using VolleyManagement.Domain.GameReportsAggregate;
using VolleyManagement.Domain.GamesAggregate;
using VolleyManagement.Domain.TournamentsAggregate;
using VolleyManagement.UI.Areas.WebApi.Controllers;
using VolleyManagement.UI.Areas.WebApi.ViewModels.Tournaments;
using VolleyManagement.UnitTests.Services.GameReportService;
using VolleyManagement.UnitTests.Services.GameService;
using VolleyManagement.UnitTests.Services.TournamentService;
using VolleyManagement.UnitTests.WebApi.Standings;
using VolleyManagement.UnitTests.WebApi.ViewModels;
using TournamentViewModelComparer = VolleyManagement.UnitTests.WebApi.ViewModels.TournamentViewModelComparer;

namespace VolleyManagement.UnitTests.WebApi.Controllers
{

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
                                            .Build();

            var sut = BuildSUT();

            // Act
            var actual = sut.GetAllTournaments().ToList();

            // Assert
            _tournamentServiceMock.Verify(ts => ts.Get(), Times.Once());
            TestHelper.AreEqual(
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

        #endregion

        #region Standings

        [TestMethod]
        public void Standings_StandingsRequested_StandingsReturned()
        {
            // Arrange
            var testStandings = new StandingsTestFixture().WithMultipleDivisionsAllPossibleScores().Build();

            var expected = new DivisionStandingsViewModelTestFixture().WithMultipleDivisionsAllPossibleScores().Build();

            MockGetStandings(TOURNAMENT_ID, testStandings);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetTournamentStandings(TOURNAMENT_ID);

            // Assert
            TestHelper.AreEqual(expected, actual, new DivisionStandingsViewModelComparer());
        }

        [TestMethod]
        public void Standings_LastUpdateTimeExists_StandingsReturnLastUpdateTime()
        {
            // Arrange
            var LAST_UPDATE_TIME = new DateTime(2017, 4, 5, 12, 4, 23);

            var testStandings = new StandingsTestFixture().WithMultipleDivisionsAllPossibleScores()
                                            .WithLastUpdateTime(LAST_UPDATE_TIME).Build();

            var expected = new DivisionStandingsViewModelTestFixture().WithMultipleDivisionsAllPossibleScores()
                                            .WithLastUpdateTime(LAST_UPDATE_TIME).Build();

            MockGetStandings(TOURNAMENT_ID, testStandings);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetTournamentStandings(TOURNAMENT_ID);

            // Assert
            TestHelper.AreEqual(expected, actual, new DivisionStandingsViewModelComparer());
        }

        [TestMethod]
        public void PivotStandings_StandingsRequested_StandingsReturned()
        {
            // Arrange
            var testStandings = new PivotStandingsTestFixture()
                .WithMultipleDivisionsAllPossibleScores()
                .Build();

            var expected = new PivotStandingsViewModelTestFixture()
                .WithMultipleDivisionsAllPossibleScores()
                .Build();

            MockGetPivotStandings(TOURNAMENT_ID, testStandings);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetTournamentPivotStandings(TOURNAMENT_ID);

            // Assert
            TestHelper.AreEqual(expected, actual, new PivotStandingsViewModelComparer());
        }

        [TestMethod]
        public void PivotStandings_LastUpdateTimeExists_StandingsReturnLastUpdateTime()
        {
            // Arrange
            var LAST_UPDATE_TIME = new DateTime(2017, 4, 5, 12, 4, 23);

            var testStandings = new PivotStandingsTestFixture().WithMultipleDivisionsAllPossibleScores()
                .WithLastUpdateTime(LAST_UPDATE_TIME).Build();

            var expected = new PivotStandingsViewModelTestFixture().WithMultipleDivisionsAllPossibleScores()
                .WithLastUpdateTime(LAST_UPDATE_TIME).Build();

            MockGetPivotStandings(TOURNAMENT_ID, testStandings);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetTournamentPivotStandings(TOURNAMENT_ID);

            // Assert
            TestHelper.AreEqual(expected, actual, new PivotStandingsViewModelComparer());
        }

        [TestMethod]
        public void PivotStandings_PlannedGamesWithoutResults_RoundNumbersReturned()
        {
            // Arrange
            var testStandings = new PivotStandingsTestFixture()
                .WithNotAllGamesPlayed()
                .Build();

            var expected = new PivotStandingsViewModelTestFixture()
                .WithNotAllGamesPlayed()
                .Build();

            MockGetPivotStandings(TOURNAMENT_ID, testStandings);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetTournamentPivotStandings(TOURNAMENT_ID);

            // Assert
            TestHelper.AreEqual(expected, actual, new PivotStandingsViewModelComparer());
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

        private void MockGetStandings(int tournamentId, TournamentStandings<StandingsDto> testData)
        {
            _gameReportServiceMock.Setup(gr => gr.GetStandings(tournamentId)).Returns(testData);
        }

        private void MockGetPivotStandings(int tournamentId, TournamentStandings<PivotStandingsDto> testData)
        {
            _gameReportServiceMock.Setup(gr => gr.GetPivotStandings(tournamentId)).Returns(testData);
        }

        private void MockGetTournaments(List<Tournament> testData)
        {
            _tournamentServiceMock.Setup(tr => tr.Get()).Returns(testData);
        }

        private void MockGetTournament(Tournament tournament, int id)
        {
            _tournamentServiceMock.Setup(tr => tr.Get(id)).Returns(tournament);
        }
        #endregion
    }
}