namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using Domain.GameReportsAggregate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GetPivotStandingsTests : GameReportsServiceTestsBase
    {
        [TestInitialize]
        public void TestInit()
        {
            InitializeTest();
        }

        [TestMethod]
        public void GetPivotStandings_NoGamesScheduled_EmptyEntryForEachTeamCreated()
        {
            // Arrange
            var gameResultsData = new GameResultsTestFixture().WithNoGamesScheduled().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            var expected = new PivotStandingsTestFixture().WithNoResults().Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID);

            // Assert
            AssertPivotStandingsAreEqual(expected, actual, "For each team in tournament empty standings entry should be created");
        }

        [TestMethod]
        public void GetPivotStandings_ScheduledGamesButNoResults_StandingEntriesAreEmpty()
        {
            // Arrange
            var gameResultsData = new GameResultsTestFixture().WithNoGameResults().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var expected = new PivotStandingsTestFixture().WithNoResults().Build();

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID);

            // Assert
            AssertPivotStandingsAreEqual(expected, actual, "When there are games scheduled but no results standing entries should be empty");
        }

        [TestMethod]
        public void GetPivotStandings_GameResultsAllPossibleScores_CorrectPoints()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture().WithAllPossibleScores().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            var expected = new PivotStandingsTestFixture().WithStandingsForAllPossibleScores().Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID);

            // Assert
            AssertPivotStandingsAreEqual(expected, actual, "");
        }

        [TestMethod]
        public void GetPivotStandings_GameResultsAllPossibleScores_CorrectSetsRatios()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture().WithAllPossibleScores().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            var expected = new PivotStandingsTestFixture().WithStandingsForAllPossibleScores().Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID);

            // Assert
            AssertPivotStandingsAreEqual(expected, actual, "");
        }

        [TestMethod]
        public void GetPivotStandings_GameResultsAllPossibleScores_CorrectBallsRatios()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture().WithAllPossibleScores().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            var expected = new PivotStandingsTestFixture().WithStandingsForAllPossibleScores().Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID);

            // Assert
            AssertPivotStandingsAreEqual(expected, actual, "");
        }

        [TestMethod]
        public void GetPivotStandings_GameResultOneTeamNoLostSets_TopTeamNoLostSets()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture().WithNoLostSetsForOneTeam().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            var expected = new PivotStandingsTestFixture().WithStandingsForAllPossibleScores().Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID);

            // Assert
            AssertPivotStandingsAreEqual(expected, actual, "");
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
            var gameResultsTestData = new GameResultsTestFixture().WithAllPossibleScores().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            var expected = new PivotStandingsTestFixture().WithStandingsForAllPossibleScores().Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID);

            // Assert
            AssertPivotStandingsAreEqual(expected, actual, "");
        }

        [TestMethod]
        public void GetPivotStandings_OneTeamHasTechnicalDefeat_BallDoesntCount()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture().WithTechnicalDefeatInSet().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            var expected = new PivotStandingsTestFixture().WithTeamStandingsForOneGameTechnicalDefeat()
                .Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID);

            // Assert
            AssertPivotStandingsAreEqual(expected, actual, "");
        }

        private void AssertPivotStandingsAreEqual(
            TournamentStandings<PivotStandingsDto> expected,
            TournamentStandings<PivotStandingsDto> actual,
            string message)
        {
            throw new System.NotImplementedException();
        }
    }
}