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
    using VolleyManagement.Data.Queries.Division;
    using VolleyManagement.Data.Queries.GameResult;
    using VolleyManagement.Data.Queries.Team;
    using VolleyManagement.Domain.GamesAggregate;
    using VolleyManagement.Domain.TeamsAggregate;
    using VolleyManagement.Services;
    using VolleyManagement.UnitTests.Services.GameService;
    using VolleyManagement.UnitTests.Services.TeamService;
    using VolleyManagement.UnitTests.Services.TournamentService;

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
        private Mock<IQuery<List<List<Team>>, FindTeamsInDivisionsByTournamentIdCriteria>> _teamsByDivisionIdQueryMock;
        private Mock<IQuery<List<Division>, TournamentDivisionsCriteria>> _divisionsByTournamentIdQueryMock;

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _tournamentScheduleDtoByIdQueryMock = new Mock<IQuery<TournamentScheduleDto, TournamentScheduleInfoCriteria>>();
            _tournamentTeamsQueryMock = new Mock<IQuery<List<Team>, FindByTournamentIdCriteria>>();
            _tournamentGameResultsQueryMock = new Mock<IQuery<List<GameResultDto>, TournamentGameResultsCriteria>>();
            _teamsByDivisionIdQueryMock = new Mock<IQuery<List<List<Team>>, FindTeamsInDivisionsByTournamentIdCriteria>>();
            _divisionsByTournamentIdQueryMock = new Mock<IQuery<List<Division>, TournamentDivisionsCriteria>>();
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
            var teamsTestData = new TeamServiceTestFixture().TestTeamsByDivisions().BuildWithDivisions();

            var expected = 3;

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsData);
            SetupTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID).First().Count;

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
            var teamsTestData = new TeamServiceTestFixture().TestTeamsByDivisions().BuildWithDivisions();

            var expected = new StandingsTestFixture().WithStandingsForAllPossibleScores()
                .OrderByPointsAndSetsAndBallsAndName()
                .Build()
                .Select(s => s.Points)
                .ToList();

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID).First().Select(s => s.Points).ToList();

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
            var teamsTestData = new TeamServiceTestFixture().TestTeamsByDivisions().BuildWithDivisions();

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
            SetupTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID)
                .First()
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
            var teamsTestData = new TeamServiceTestFixture().TestTeamsByDivisions().BuildWithDivisions();

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
            SetupTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID)
                .First()
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
            var teamsTestData = new TeamServiceTestFixture().TestTeamsByDivisions().BuildWithDivisions();

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
            SetupTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID)
                .First()
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
            var teamsTestData = new TeamServiceTestFixture().TestTeamsByDivisions().BuildWithDivisions();
            var expected = float.PositiveInfinity;

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID).First()[TOP_TEAM_INDEX].SetsRatio;

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
            var teamsTestData = new TeamServiceTestFixture().TestTeamsByDivisions().BuildWithDivisions();
            var expected = new { SetsRatio = (float?)float.PositiveInfinity, BallsRatio = (float?)null };

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var result = sut.GetStandings(TOURNAMENT_ID).First()[TOP_TEAM_INDEX];
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
            var teamsTestData = new TeamServiceTestFixture().TestTeamsByDivisions().BuildWithDivisions();

            var expected = new StandingsTestFixture().WithUniquePoints().Build()[TOP_TEAM_INDEX].Points;

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID).First()[TOP_TEAM_INDEX].Points;

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
            var teamsTestData = new TeamServiceTestFixture().TestTeamsByDivisions().BuildWithDivisions();
            var expected = new StandingsTestFixture().WithRepetitivePoints().OrderByPoints().Build()[TOP_TEAM_INDEX].SetsRatio;

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID).First()[TOP_TEAM_INDEX].SetsRatio;

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
            var teamsTestData = new TeamServiceTestFixture().TestTeamsByDivisions().BuildWithDivisions();

            var expected = new StandingsTestFixture().WithRepetitivePointsAndSetsRatio()
                .OrderByPointsAndSets()
                .Build()[TOP_TEAM_INDEX]
                .BallsRatio;

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID).First()[TOP_TEAM_INDEX].BallsRatio;

            // Assert
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        /// Test for GetStandings() method. There is one set with technical defeat in results.
        /// Balls in technical defeat set dont count for statistics.
        /// </summary>
        [TestMethod]
        public void GetStandings_OneTeamHasTechnicalDefeatInSet_BallsDontCount()
        {
            // Arrange
            var gameResultsTestData = new GameServiceTestFixture().WithTechnicalDefeatInSet().Build();
            var teamsTestData = new TeamServiceTestFixture().TestTeamsByDivisions().BuildWithDivisions();

            var expected = new StandingsTestFixture().WithTeamStandingsForOneSetTechnicalDefeat()
                .OrderByPointsAndSets()
                .Build()
                .Select(item => item.BallsRatio)
                .ToList();

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID).First().Select(item => item.BallsRatio).ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetStandings_HomeTeamHasPenalty_PenaltyDeductedFromTotalPoints()
        {
            // Arrange
            var gameResultsTestData = new GameServiceTestFixture().WithHomeTeamPenalty().Build();
            var teamsTestData = new TeamServiceTestFixture().TestTeamsByDivisions().BuildWithDivisions();

            var expected = new StandingsTestFixture().WithTeamAPenalty()
                .OrderByPoints()
                .Build()
                .Select(item => item.Points)
                .ToList();

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID).First().Select(item => item.Points).ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetStandings_AwayTeamHasPenalty_PenaltyDeductedFromTotalPoints()
        {
            // Arrange
            var gameResultsTestData = new GameServiceTestFixture().WithAwayTeamPenalty().Build();
            var teamsTestData = new TeamServiceTestFixture().TestTeamsByDivisions().BuildWithDivisions();

            var expected = new StandingsTestFixture().WithTeamCPenalty()
                .OrderByPoints()
                .Build()
                .Select(item => item.Points)
                .ToList();

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID).First().Select(item => item.Points).ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual);
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
            var teamsTestData = new TeamServiceTestFixture().TestTeamsByDivisions().BuildWithDivisions();

            var expected = new TeamStandingsTestFixture().WithTeamStandingsForAllPossibleScores()
                .OrderByPoints()
                .Build()
                .Select(t => new { t.Points })
                .ToList();

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID).First().Teams.Select(t => new { t.Points }).ToList();

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
            var teamsTestData = new TeamServiceTestFixture().TestTeamsByDivisions().BuildWithDivisions();

            var expected = new TeamStandingsTestFixture().WithTeamStandingsForAllPossibleScores()
                .OrderByPointsAndSets()
                .Build()
                .Select(t => new { t.SetsRatio })
                .ToList();

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID).First().Teams.Select(t => new { t.SetsRatio }).ToList();

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
            var teamsTestData = new TeamServiceTestFixture().TestTeamsByDivisions().BuildWithDivisions();

            var expected = new TeamStandingsTestFixture().WithTeamStandingsForAllPossibleScores()
                .OrderByPointsAndSets()
                .Build()
                .Select(t => new { t.BallsRatio })
                .ToList();

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID).First().Teams.Select(t => new { t.BallsRatio }).ToList();

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
            var teamsTestData = new TeamServiceTestFixture().TestTeamsByDivisions().BuildWithDivisions();

            var expected = float.PositiveInfinity;

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID).First().Teams.First().SetsRatio;

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
            var teamWithNoResults = new List<Team> { new Team { Id = 4 } };
            var teamsTestData = new TeamServiceTestFixture().AddTeams(teamWithNoResults).TestTeamsByDivisions().BuildWithDivisions();

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID).First().Teams.Last().SetsRatio;

            // Assert
            Assert.IsNull(actual);
        }

        /// <summary>
        /// Test for GetPivotStandings() method.
        /// There is one game with technical defeat in results.
        /// Balls in technical defeat game dont count for statistics.
        /// </summary>
        [TestMethod]
        public void GetPivotStandings_OneTeamHasTechnicalDefeat_BallDoesntCount()
        {
            // Arrange
            var gameResultsTestData = new GameServiceTestFixture().TestGameResults().Build();
            var teamsTestData = new TeamServiceTestFixture().TestTeamsByDivisions().BuildWithDivisions();

            var expected = new TeamStandingsTestFixture().WithTeamStandingsForOneGameTechnicalDefeat()
                .OrderByPointsAndSets()
                .Build()
                .Select(t => new { t.BallsRatio })
                .ToList();

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID).First().Teams.Select(t => new { t.BallsRatio }).ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        private GameReportService BuildSUT()
        {
            return new GameReportService(
                _tournamentGameResultsQueryMock.Object,
                _tournamentTeamsQueryMock.Object,
               _tournamentScheduleDtoByIdQueryMock.Object,
               _teamsByDivisionIdQueryMock.Object,
               _divisionsByTournamentIdQueryMock.Object);
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

        private void SetupTournamentTeamsGroupedByDivisionsQuery(int tournamentId, List<List<Team>> testData)
        {
            _teamsByDivisionIdQueryMock.SetupSequence(m =>
                m.Execute(It.Is<FindTeamsInDivisionsByTournamentIdCriteria>(c => c.TournamentId == tournamentId)))
                .Returns(testData);
        }
    }
}
