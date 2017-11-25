namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Data.Queries.Tournament;
    using Domain.GameReportsAggregate;
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

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsData);
            MockTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID).First().Count;

            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        [TestMethod]
        public void GetStandings_GameResultsAllPossibleScores_CorrectPointsStats()
        {
            // Arrange
            var gameResultsTestData = new GameServiceTestFixture().WithAllPossibleScores().Build();
            var teamsTestData = new TeamServiceTestFixture().TestTeamsByDivisions().BuildWithDivisions();

            var expected = new StandingsTestFixture().WithStandingsForAllPossibleScores()
                                                     .Build()
                                                     .Select(s => s.Points)
                                                     .ToList();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID).First().Select(s => s.Points).ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }
        
        [TestMethod]
        public void GetStandings_GameResultsAllPossibleScores_CorrectGamesStats()
        {
            // Arrange
            var gameResultsTestData = new GameServiceTestFixture().WithAllPossibleScores().Build();
            var teamsTestData = new TeamServiceTestFixture().TestTeamsByDivisions().BuildWithDivisions();

            var expected = new StandingsTestFixture().WithStandingsForAllPossibleScores()
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

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

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
                .Build()
                .Select(s => new
                {
                    SetsWon = s.SetsWon,
                    SetsLost = s.SetsLost,
                    SetsRatio = s.SetsRatio
                })
                .ToList();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

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
                .Build()
                .Select(s => new
                {
                    BallsWon = s.BallsWon,
                    BallsLost = s.BallsLost,
                    BallsRatio = s.BallsRatio
                })
                .ToList();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

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

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

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

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var result = sut.GetStandings(TOURNAMENT_ID).First()[TOP_TEAM_INDEX];
            var actual = new { SetsRatio = result.SetsRatio, BallsRatio = result.BallsRatio };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetStandings_GameResultsAllPossibleScores_TeamsOrderedByPoints()
        {
            // Arrange
            var gameResultsTestData = new GameServiceTestFixture().WithResultsForUniquePoints().Build();
            var teamsTestData = new TeamServiceTestFixture().TestTeamsByDivisions().BuildWithDivisions();

            var expected = new StandingsTestFixture().WithUniquePoints().Build();
            expected = expected.OrderByDescending(s => s.Points).ToList();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID);

            // Assert
            AssertPositionsAreEqual(actual, expected);
        }

        [TestMethod]
        public void GetStandings_SeveralTeamsWithSamePoints_TeamsOrderedByWonGames()
        {
            // Arrange
            var gameResultsTestData = new GameServiceTestFixture().WithResultsForSamePoints().Build();
            var teamsTestData = new TeamServiceTestFixture().TestTeamsByDivisions().BuildWithDivisions();

            var expected = new StandingsTestFixture().WithSamePoints().Build();
            expected = expected.OrderByDescending(s => s.Points)
                               .ThenByDescending(s => s.GamesWon)
                               .ToList();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID);

            // Assert
            AssertPositionsAreEqual(actual, expected);
        }

        [TestMethod]
        public void GetStandings_SeveralTeamsWithSamePointsAndWonGames_TeamsOrderedBySetsRatio()
        {
            // Arrange
            var gameResultsTestData = new GameServiceTestFixture()
                .WithResultsForSamePointsAndWonGames().Build();
            var teamsTestData = new TeamServiceTestFixture().TestTeamsByDivisions().BuildWithDivisions();

            var expected = new StandingsTestFixture().WithSamePointsAndWonGames().Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID);

            // Assert
            AssertPositionsAreEqual(actual, expected);
        }

        [TestMethod]
        public void GetStandings_SeveralTeamsWithSamePointsWonGamesAndSetsRatio_TeamsOrderedByBallsRatio()
        {
            // Arrange
            var gameResultsTestData = new GameServiceTestFixture()
                .WithResultsForSamePointsWonGamesAndSetsRatio().Build();
            var teamsTestData = new TeamServiceTestFixture().TestTeamsByDivisions().BuildWithDivisions();

            var expected = new StandingsTestFixture().WithSamePointsWonGamesAndSetsRatio().Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID);

            // Assert
            AssertPositionsAreEqual(actual, expected);
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
                .Build()
                .Select(item => item.BallsRatio)
                .ToList();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID).First().Select(item => item.BallsRatio).ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test for GetStandings() method. Game scheduled by not played.
        /// Standings returned with zeroes and ordered by team names.
        /// </summary>
        [TestMethod]
        public void GetStandings_NoGameResults_StandingsAreEmpty()
        {
            // Arrange
            var gameResultsTestData = new GameServiceTestFixture().TestGamesWithoutResult().Build();
            var teamsTestData = new TeamServiceTestFixture().TestTeams().Build();

            var expected = new StandingsTestFixture().WithNoResults()
                .Build()
                .ToList();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, new List<List<Team>> { teamsTestData });

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID).First().ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual, new StandingsEntryComparer());
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

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

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

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

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

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

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

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

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

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

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

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID).First().Teams.Select(t => new { t.BallsRatio }).ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        private static void AssertPositionsAreEqual(List<List<StandingsEntry>> actual, List<StandingsEntry> expected)
        {
            var actualPositions = actual[0].Select((s, i) => (Position: i, s.TeamName, s.Points)).ToList();
            var expectedPositions = expected.Select((s, i) => (Position: i, s.TeamName, s.Points)).ToList();
            CollectionAssert.AreEqual(expectedPositions, actualPositions);
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

        private void MockTournamentGameResultsQuery(int tournamentId, List<GameResultDto> testData)
        {
            _tournamentGameResultsQueryMock.Setup(m =>
                m.Execute(It.Is<TournamentGameResultsCriteria>(c => c.TournamentId == tournamentId)))
                .Returns(testData);
        }

        private void MockTournamentTeamsGroupedByDivisionsQuery(int tournamentId, List<List<Team>> testData)
        {
            _teamsByDivisionIdQueryMock.SetupSequence(m =>
                m.Execute(It.Is<FindTeamsInDivisionsByTournamentIdCriteria>(c => c.TournamentId == tournamentId)))
                .Returns(testData);
        }
    }
}
