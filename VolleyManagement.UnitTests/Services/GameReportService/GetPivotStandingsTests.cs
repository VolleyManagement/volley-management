namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System.Collections.Generic;
    using GameService;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TeamService;

    [TestClass]
    public class GetPivotStandingsTests:GameReportsServiceTestsBase
    {
        [TestInitialize]
        public void TestInit()
        {
            InitializeTest();
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

            var expected = new PivotStandingsTestFixture().WithTeamStandingsForAllPossibleScores()
                .Build()
                .Select(t => new { t.Points })
                .ToList();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID);

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
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

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
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

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
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

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
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

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
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID).First().Teams.Select(t => new { t.BallsRatio }).ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}