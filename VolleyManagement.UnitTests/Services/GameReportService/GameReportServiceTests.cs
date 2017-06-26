namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Data.Queries.Tournament;
    using Domain.TournamentsAggregate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Queries.GameResult;
    using VolleyManagement.Data.Queries.Team;
    using VolleyManagement.Domain.GamesAggregate;
    using VolleyManagement.Domain.TeamsAggregate;
    using VolleyManagement.Services;
    using VolleyManagement.UnitTests.Services.GameService;
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

        private Mock<IQuery<List<GameResultDto>, TournamentGameResultsCriteria>> _tournamentGameResultsQueryMock;
        private Mock<IQuery<List<Team>, FindByTournamentIdCriteria>> _tournamentTeamsQueryMock;
        private Mock<IQuery<TournamentScheduleDto, TournamentScheduleInfoCriteria>> _tournamentScheduleDtoByIdQueryMock;

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _tournamentScheduleDtoByIdQueryMock = new Mock<IQuery<TournamentScheduleDto, TournamentScheduleInfoCriteria>>();
            _tournamentTeamsQueryMock = new Mock<IQuery<List<Team>, FindByTournamentIdCriteria>>();
            _tournamentGameResultsQueryMock = new Mock<IQuery<List<GameResultDto>, TournamentGameResultsCriteria>>();
        }

        /// <summary>
        /// Test for GetStandings() method. No game results are available for the specified tournament.
        /// Tournament standings contains no entries.
        /// </summary>
        [TestMethod]
        public void GetStandings_NoGameResults_StandingsAllEntries()
        {
            // Arrange
            var gameResultsData = new GameServiceTestFixture().Build();
            var teamData = new TeamServiceTestFixture().TestTeams().Build();

            var expected = 3;

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsData);
            SetupTournamentTeamsQuery(TOURNAMENT_ID, teamData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID).Count;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test for GetStandings() method. Game results with all possible scores are available for the specified tournament.
        /// Points statistics in tournament standings is calculated correctly.
        /// </summary>
        [TestMethod]
        public void GetStandings_GameResultsAllPossibleScores_CorrectPointsStats()
        {
            // Arrange
            var gameResultsTestData = new GameServiceTestFixture().WithAllPossibleScores().Build();
            var teamData = new TeamServiceTestFixture().TestTeams().Build();

            var expected = new StandingsTestFixture().WithStandingsForAllPossibleScores()
                .OrderByPointsAndSetsAndBallsAndName()
                .Build()
                .Select(s => s.Points)
                .ToList();

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupTournamentTeamsQuery(TOURNAMENT_ID, teamData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID).Select(s => s.Points).ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test for GetStandings() method. Game results with all possible scores are available for the specified tournament.
        /// Games statistics in tournament standings is calculated correctly.
        /// </summary>
        [TestMethod]
        public void GetStandings_GameResultsAllPossibleScores_CorrectGamesStats()
        {
            // Arrange
            var gameResultsTestData = new GameServiceTestFixture().WithAllPossibleScores().Build();
            var teamData = new TeamServiceTestFixture().TestTeams().Build();

            var expected = new StandingsTestFixture().WithStandingsForAllPossibleScores()
                .OrderByPointsAndSetsAndBallsAndName()
                .Build()
                .Select(s => new
                {
                    GamesTotal = s.GamesTotal,
                    GamesWon = s.GamesWon,
                    GamesLost = s.GamesLost,
                    GamesWithScoreThreeNil = s.GamesWithScoreThreeNil,
                    GamesWithScoreThreeOne = s.GamesWithScoreThreeOne,
                    GamesWithScoreThreeTwo = s.GamesWithScoreThreeTwo,
                    GamesWithScoreTwoThree = s.GamesWithScoreTwoThree,
                    GamesWithScoreOneThree = s.GamesWithScoreOneThree,
                    GamesWithScoreNilThree = s.GamesWithScoreNilThree
                })
                .ToList();

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupTournamentTeamsQuery(TOURNAMENT_ID, teamData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID)
                .Select(s => new
                {
                    GamesTotal = s.GamesTotal,
                    GamesWon = s.GamesWon,
                    GamesLost = s.GamesLost,
                    GamesWithScoreThreeNil = s.GamesWithScoreThreeNil,
                    GamesWithScoreThreeOne = s.GamesWithScoreThreeOne,
                    GamesWithScoreThreeTwo = s.GamesWithScoreThreeTwo,
                    GamesWithScoreTwoThree = s.GamesWithScoreTwoThree,
                    GamesWithScoreOneThree = s.GamesWithScoreOneThree,
                    GamesWithScoreNilThree = s.GamesWithScoreNilThree
                })
                .ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test for GetStandings() method. Game results with all possible scores are available for the specified tournament.
        /// Sets statistics in tournament standings is calculated correctly.
        /// </summary>
        [TestMethod]
        public void GetStandings_GameResultsAllPossibleScores_CorrectSetsStats()
        {
            // Arrange
            var gameResultsTestData = new GameServiceTestFixture().WithAllPossibleScores().Build();
            var teamData = new TeamServiceTestFixture().TestTeams().Build();

            var expected = new StandingsTestFixture().WithStandingsForAllPossibleScores()
                .OrderByPointsAndSetsAndBallsAndName()
                .Build()
                .Select(s => new
                {
                    SetsWon = s.SetsWon,
                    SetsLost = s.SetsLost,
                    SetsRatio = s.SetsRatio
                })
                .ToList();

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupTournamentTeamsQuery(TOURNAMENT_ID, teamData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID)
                .Select(s => new
                {
                    SetsWon = s.SetsWon,
                    SetsLost = s.SetsLost,
                    SetsRatio = s.SetsRatio
                })
                .ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test for GetStandings() method. Game results with all possible scores are available for the specified tournament.
        /// Balls statistics in tournament standings is calculated correctly.
        /// </summary>
        [TestMethod]
        public void GetStandings_GameResultsAllPossibleScores_CorrectBallsStats()
        {
            // Arrange
            var gameResultsTestData = new GameServiceTestFixture().WithAllPossibleScores().Build();
            var teamData = new TeamServiceTestFixture().TestTeams().Build();

            var expected = new StandingsTestFixture().WithStandingsForAllPossibleScores()
                .OrderByPointsAndSetsAndBallsAndName()
                .Build()
                .Select(s => new
                {
                    BallsWon = s.BallsWon,
                    BallsLost = s.BallsLost,
                    BallsRatio = s.BallsRatio
                })
                .ToList();

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupTournamentTeamsQuery(TOURNAMENT_ID, teamData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID)
                .Select(s => new
                {
                    BallsWon = s.BallsWon,
                    BallsLost = s.BallsLost,
                    BallsRatio = s.BallsRatio
                })
                .ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test for GetStandings() method. Game result where one team has no lost sets is available for the specified tournament.
        /// Teams which has no lost sets is positioned at the top of the standings.
        /// </summary>
        [TestMethod]
        public void GetStandings_GameResultOneTeamNoLostSets_TopTeamNoLostSets()
        {
            // Arrange
            var gameResultsTestData = new GameServiceTestFixture().WithNoLostSetsForOneTeam().Build();
            var teamData = new TeamServiceTestFixture().TestTeams().Build();
            var expected = float.PositiveInfinity;

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupTournamentTeamsQuery(TOURNAMENT_ID, teamData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID)[TOP_TEAM_INDEX].SetsRatio;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test for GetStandings() method. Game result where one team has has no lost sets and no lost balls is available for
        /// the specified tournament. Teams which has no lost sets and no lost balls is positioned at the top of the standings.
        /// </summary>
        [TestMethod]
        public void GetStandings_GameResultOneTeamNoLostSetsNoLostBalls_TopTeamNoLostSetsNoLostBalls()
        {
            // Arrange
            var gameResultsTestData = new GameServiceTestFixture().WithNoLostSetsNoLostBallsForOneTeam().Build();
            var teamData = new TeamServiceTestFixture().TestTeams().Build();
            var expected = new { SetsRatio = (float?)float.PositiveInfinity, BallsRatio = (float?)float.PositiveInfinity };

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupTournamentTeamsQuery(TOURNAMENT_ID, teamData);

            var sut = BuildSUT();

            // Act
            var result = sut.GetStandings(TOURNAMENT_ID)[TOP_TEAM_INDEX];
            var actual = new { SetsRatio = result.SetsRatio, BallsRatio = result.BallsRatio };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test for GetStandings() method. Standings entries are not ordered.
        /// Team with maximum points is not positioned at the top of the standings.
        /// </summary>
        [TestMethod]
        public void GetStandings_EntriesNotOrdered_NotTopTeamMaxPoints()
        {
            // Arrange
            var gameResultsTestData = new GameServiceTestFixture().WithResultsForUniquePoints().Build();
            var teamData = new TeamServiceTestFixture().TestTeams().Build();
            var expected = new StandingsTestFixture().WithUniquePoints().Build()[TOP_TEAM_INDEX].Points;

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupTournamentTeamsQuery(TOURNAMENT_ID, teamData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID)[TOP_TEAM_INDEX].Points;

            // Assert
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        /// Test for GetStandings() method. Standings entries are ordered by points.
        /// Team with maximum sets ratio is not positioned at the top of the standings.
        /// </summary>
        [TestMethod]
        public void GetStandings_EntriesOrderedByPoints_NotTopTeamMaxSetsRatio()
        {
            // Arrange
            var gameResultsTestData = new GameServiceTestFixture().WithResultsForRepetitivePoints().Build();
            var teamData = new TeamServiceTestFixture().TestTeams().Build();
            var expected = new StandingsTestFixture().WithRepetitivePoints().OrderByPoints().Build()[TOP_TEAM_INDEX].SetsRatio;

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupTournamentTeamsQuery(TOURNAMENT_ID, teamData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID)[TOP_TEAM_INDEX].SetsRatio;

            // Assert
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        /// Test for GetStandings() method. Standings entries are ordered by points and then by sets ratio.
        /// Team with maximum balls ratio is not positioned at the top of the standings.
        /// </summary>
        [TestMethod]
        public void GetStandings_EntriesOrderedByPointsSetsRatio_NotTopTeamMaxBallsRatio()
        {
            // Arrange
            var gameResultsTestData = new GameServiceTestFixture().WithResultsForRepetitivePointsAndSetsRatio().Build();
            var teamData = new TeamServiceTestFixture().TestTeams().Build();

            var expected = new StandingsTestFixture().WithRepetitivePointsAndSetsRatio()
                .OrderByPointsAndSets()
                .Build()[TOP_TEAM_INDEX]
                .BallsRatio;

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupTournamentTeamsQuery(TOURNAMENT_ID, teamData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID)[TOP_TEAM_INDEX].BallsRatio;

            // Assert
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        /// Test for GetPivotStandings() method.
        /// Game results with all possible scores are available for the specified tournament.
        /// Teams points in tournament standings is calculated correctly.
        /// </summary>
        [TestMethod]
        public void GetPivotStandings_GameResultsAllPossibleScores_CorrectPoints()
        {
            // Arrange
            var gameResultsTestData = new GameServiceTestFixture().WithAllPossibleScores().Build();
            var teamsTestData = new TeamServiceTestFixture().TestTeams().Build();

            var expected = new TeamStandingsTestFixture().WithTeamStandingsForAllPossibleScores()
                .OrderByPoints()
                .Build()
                .Select(t => new { t.Points })
                .ToList();

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID).Teams.Select(t => new { t.Points }).ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test for GetPivotStandings() method.
        /// Game results with all possible scores are available for the specified tournament.
        /// Teams sets ratio in tournament standings is calculated correctly.
        /// </summary>
        [TestMethod]
        public void GetPivotStandings_GameResultsAllPossibleScores_CorrectSetsRatios()
        {
            // Arrange
            var gameResultsTestData = new GameServiceTestFixture().WithAllPossibleScores().Build();
            var teamsTestData = new TeamServiceTestFixture().TestTeams().Build();

            var expected = new TeamStandingsTestFixture().WithTeamStandingsForAllPossibleScores()
                .OrderByPointsAndSets()
                .Build()
                .Select(t => new { t.SetsRatio })
                .ToList();

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID).Teams.Select(t => new { t.SetsRatio }).ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test for GetPivotStandings() method.
        /// Game results with all possible scores are available for the specified tournament.
        /// Teams balls ratio in tournament standings is calculated correctly.
        /// </summary>
        [TestMethod]
        public void GetPivotStandings_GameResultsAllPossibleScores_CorrectBallsRatios()
        {
            // Arrange
            var gameResultsTestData = new GameServiceTestFixture().WithAllPossibleScores().Build();
            var teamsTestData = new TeamServiceTestFixture().TestTeams().Build();

            var expected = new TeamStandingsTestFixture().WithTeamStandingsForAllPossibleScores()
                .OrderByPointsAndSets()
                .Build()
                .Select(t => new { t.BallsRatio })
                .ToList();

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID).Teams.Select(t => new { t.BallsRatio }).ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test for GetPivotStandings() method.
        /// Game result where one team has no lost sets is available for the specified tournament.
        /// Teams which has no lost sets is positioned at the top of the standings and its value is infinity
        /// </summary>
        [TestMethod]
        public void GetPivotStandings_GameResultOneTeamNoLostSets_TopTeamNoLostSets()
        {
            // Arrange
            var gameResultsTestData = new GameServiceTestFixture().WithNoLostSetsForOneTeam().Build();
            var teamsTestData = new TeamServiceTestFixture().TestTeams().Build();

            var expected = float.PositiveInfinity;

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID).Teams.First().SetsRatio;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test for GetPivotStandings() method.
        /// Game result where one team has no lost sets is available for the specified tournament.
        /// Teams which has no lost sets is positioned at the top of the standings and its value is infinity
        /// </summary>
        [TestMethod]
        public void GetPivotStandings_OneTeamHasNoGameResults_LastTeamWithNoSetRatio()
        {
            // Arrange
            var gameResultsTestData = new GameServiceTestFixture().TestGameResults().Build();
            var teamWithNoResults = new Team { Id = 4 };
            var teamsTestData = new TeamServiceTestFixture().AddTeam(teamWithNoResults).TestTeams().Build();

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID).Teams.Last().SetsRatio;

            // Assert
            Assert.IsNull(actual);
        }

        private GameReportService BuildSUT()
        {
            return new GameReportService(
                _tournamentGameResultsQueryMock.Object,
                _tournamentTeamsQueryMock.Object,
               _tournamentScheduleDtoByIdQueryMock.Object);
        }

        private void SetupTournamentGameResultsQuery(int tournamentId, List<GameResultDto> testData)
        {
            _tournamentGameResultsQueryMock.Setup(m =>
                m.Execute(It.Is<TournamentGameResultsCriteria>(c => c.TournamentId == tournamentId)))
                .Returns(testData);
        }

        private void SetupTournamentTeamsQuery(int tournamentId, List<Team> testData)
        {
            _tournamentTeamsQueryMock.Setup(m =>
                m.Execute(It.Is<FindByTournamentIdCriteria>(c => c.TournamentId == tournamentId)))
                .Returns(testData);
        }

        private void SetupGetTournamentById(int id, TournamentScheduleDto tournament)
        {
            _tournamentScheduleDtoByIdQueryMock.Setup(m =>
                m.Execute(It.Is<TournamentScheduleInfoCriteria>(c => c.TournamentId == id)))
                .Returns(tournament);
        }
    }
}
