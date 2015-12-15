namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Queries.GameResult;
    using VolleyManagement.Data.Queries.Team;
    using VolleyManagement.Domain.GameResultsAggregate;
    using VolleyManagement.Domain.TeamsAggregate;
    using VolleyManagement.Services;
    using VolleyManagement.UnitTests.Services.GameResultService;
    using VolleyManagement.UnitTests.Services.TeamService;

    /// <summary>
    /// Tests for <see cref="GameReportService"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class GameReportServiceTests
    {
        private const int TOURNAMENT_ID = 1;

        private const int TOP_TEAM_INDEX = 0;

        private static readonly IEnumerable<int> _allTeamsIds = new List<int> { 1, 2, 3 };

        private static readonly IEnumerable<int> _homeAndAwayTeamsIds = new List<int> { 1, 2 };

        private readonly Mock<IQuery<List<GameResult>, TournamentGameResultsCriteria>> _tournamentGameResultsQueryMock =
            new Mock<IQuery<List<GameResult>, TournamentGameResultsCriteria>>();

        private readonly Mock<IQuery<IEnumerable<Team>, GameResultsTeamsCriteria>> _gameResultsTeamsQueryMock =
            new Mock<IQuery<IEnumerable<Team>, GameResultsTeamsCriteria>>();

        private IKernel _kernel;

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IQuery<List<GameResult>, TournamentGameResultsCriteria>>().ToConstant(_tournamentGameResultsQueryMock.Object);
            _kernel.Bind<IQuery<IEnumerable<Team>, GameResultsTeamsCriteria>>().ToConstant(_gameResultsTeamsQueryMock.Object);
        }

        /// <summary>
        /// Test for GetStandings() method. No game results are available for the specified tournament.
        /// Tournament standings contains no entries.
        /// </summary>
        [TestMethod]
        public void GetStandings_NoGameResults_StandingsNoEntries()
        {
            // Arrange
            var gameResultsTestData = new GameResultTestFixture().Build();
            var sut = _kernel.Get<GameReportService>();
            var expected = 0;

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID).Count;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test for GetStandings() method. Game results with all possible scores are available for the specified tournament.
        /// Tournament standings are returned.
        /// </summary>
        [TestMethod]
        public void GetStandings_GameResultsAllPossibleScores_StandingsReturned()
        {
            // Arrange
            var gameResultsTestData = new GameResultTestFixture().WithAllPossibleScores().Build();
            var teamsTestData = new TeamServiceTestFixture().TestTeams().Build();
            var sut = _kernel.Get<GameReportService>();
            var expected = new StandingsTestFixture().WithStandingsForAllPossibleScores().Build();

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupResultsTeamsQuery(_allTeamsIds, teamsTestData);

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID);

            // Assert
            CollectionAssert.AreEqual(expected, actual, new StandingsEntryComparer());
        }

        /// <summary>
        /// Test for GetStandings() method. Game result with no lost sets for the home team is available.
        /// Tournament standings entry with sets ratio equal to positive infinity for the home team is returned.
        /// </summary>
        [TestMethod]
        public void GetStandings_HomeTeamNoLostSets_HomeTeamSetsRatioInfinity()
        {
            // Arrange
            var gameResultsTestData = new GameResultTestFixture().WithNoLostSetsForHomeTeam().Build();
            var teamsTestData = new TeamServiceTestFixture().WithHomeAndAwayTeams().Build();
            var sut = _kernel.Get<GameReportService>();
            var expected = float.PositiveInfinity;

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupResultsTeamsQuery(_homeAndAwayTeamsIds, teamsTestData);

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID)[TOP_TEAM_INDEX].SetsRatio;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test for GetStandings() method. Game result with no lost sets for the away team is available.
        /// Tournament standings entry with sets ratio equal to positive infinity for the away team is returned.
        /// </summary>
        [TestMethod]
        public void GetStandings_AwayTeamNoLostSets_AwayTeamSetsRatioInfinity()
        {
            // Arrange
            var gameResultsTestData = new GameResultTestFixture().WithNoLostSetsForAwayTeam().Build();
            var teamsTestData = new TeamServiceTestFixture().WithHomeAndAwayTeams().Build();
            var sut = _kernel.Get<GameReportService>();
            var expected = float.PositiveInfinity;

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupResultsTeamsQuery(_homeAndAwayTeamsIds, teamsTestData);

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID)[TOP_TEAM_INDEX].SetsRatio;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test for GetStandings() method. Game result with no lost balls for the home team is available.
        /// Tournament standings entry with balls ratio equal to positive infinity for the home team is returned.
        /// </summary>
        [TestMethod]
        public void GetStandings_HomeTeamNoLostBalls_HomeTeamBallsRatioInfinity()
        {
            // Arrange
            var gameResultsTestData = new GameResultTestFixture().WithNoLostBallsForHomeTeam().Build();
            var teamsTestData = new TeamServiceTestFixture().WithHomeAndAwayTeams().Build();
            var sut = _kernel.Get<GameReportService>();
            var expected = float.PositiveInfinity;

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupResultsTeamsQuery(_homeAndAwayTeamsIds, teamsTestData);

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID)[TOP_TEAM_INDEX].BallsRatio;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test for GetStandings() method. Game result with no lost balls for the away team is available.
        /// Tournament standings entry with balls ratio equal to positive infinity for the away team is returned.
        /// </summary>
        [TestMethod]
        public void GetStandings_AwayTeamNoLostBalls_AwayTeamBallsRatioInfinity()
        {
            // Arrange
            var gameResultsTestData = new GameResultTestFixture().WithNoLostBallsForAwayTeam().Build();
            var teamsTestData = new TeamServiceTestFixture().WithHomeAndAwayTeams().Build();
            var sut = _kernel.Get<GameReportService>();
            var expected = float.PositiveInfinity;

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupResultsTeamsQuery(_homeAndAwayTeamsIds, teamsTestData);

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID)[TOP_TEAM_INDEX].BallsRatio;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test for GetStandings() method. Test standings entries are ordered only by points and actual standings entries
        /// are ordered correctly (by points, by sets ratio and by balls ratio). Test tournament standings are not equal to actual ones.
        /// </summary>
        [TestMethod]
        public void GetStandings_EntriesOrderedByPoints_StandingsNoMatch()
        {
            // Arrange
            var gameResultsTestData = new GameResultTestFixture().WithResultsForRepetitivePoints().Build();
            var teamsTestData = new TeamServiceTestFixture().WithHomeAndAwayTeams().Build();
            var sut = _kernel.Get<GameReportService>();
            var notExpected = new StandingsTestFixture().WithRepetitivePoints().Build();

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupResultsTeamsQuery(_homeAndAwayTeamsIds, teamsTestData);

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID);

            // Assert
            CollectionAssert.AreNotEqual(notExpected, actual, new StandingsEntryComparer());
        }

        /// <summary>
        /// Test for GetStandings() method. Test standings entries are ordered by points and by sets ratio and actual standings entries
        /// are ordered correctly (by points, by sets ratio and by balls ratio). Test tournament standings are not equal to actual ones.
        /// </summary>
        [TestMethod]
        public void GetStandings_EntriesOrderedByPointsAndSetsRatio_StandingsNoMatch()
        {
            // Arrange
            var gameResultsTestData = new GameResultTestFixture().WithResultsForRepetitivePointsAndSetsRatio().Build();
            var teamsTestData = new TeamServiceTestFixture().WithHomeAndAwayTeams().Build();
            var sut = _kernel.Get<GameReportService>();
            var notExpected = new StandingsTestFixture().WithRepetitivePointsAndSetsRatio().Build();

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupResultsTeamsQuery(_homeAndAwayTeamsIds, teamsTestData);

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID);

            // Assert
            CollectionAssert.AreNotEqual(notExpected, actual, new StandingsEntryComparer());
        }

        private void SetupTournamentGameResultsQuery(int tournamentId, List<GameResult> testData)
        {
            _tournamentGameResultsQueryMock.Setup(q =>
                q.Execute(It.Is<TournamentGameResultsCriteria>(c => c.TournamentId == tournamentId)))
                .Returns(testData);
        }

        private void SetupResultsTeamsQuery(IEnumerable<int> teamIds, List<Team> testData)
        {
            _gameResultsTeamsQueryMock.Setup(q =>
                q.Execute(It.Is<GameResultsTeamsCriteria>(c => c.TeamIds.SequenceEqual(teamIds))))
                .Returns(testData);
        }
    }
}
