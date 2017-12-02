namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System.Diagnostics.CodeAnalysis;
    using Domain.GameReportsAggregate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for <see cref="GameReportService"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class GetStandingsTests : GameReportsServiceTestsBase
    {
        [TestInitialize]
        public void TestInit()
        {
            InitializeTest();
        }

        [TestMethod]
        public void GetStandings_NoGamesScheduled_EmptyEntryForEachTeamCreated()
        {
            // Arrange
            var gameResultsData = new GameResultsTestFixture().WithNoGamesScheduled().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            var expected = new StandingsTestFixture().WithNoResults().Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID);

            // Assert
            AssertStandingsAreEqual(expected, actual, "For each team in tournament empty standings entry should be created.");
        }

        [TestMethod]
        public void GetStandings_NoResultsButGamesScheduled_StandingEntriesAreEmpty()
        {
            // Arrange
            var gameResultsData = new GameResultsTestFixture().WithNoGameResults().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var expected = new StandingsTestFixture().WithNoResults().Build();

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID);

            // Assert
            AssertStandingsAreEqual(expected, actual, "When there are games scheduled but no results standing entries should be emoty.");
        }

        [TestMethod]
        public void GetStandings_GameResultsAllPossibleScores_CorrectStats()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture().WithAllPossibleScores().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var expected = new StandingsTestFixture().WithStandingsForAllPossibleScores().Build();

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID);

            // Assert
            AssertStandingsAreEqual(expected, actual, "Points, games, sets and balls should be calculated properly.");
        }

        [TestMethod]
        public void GetStandings_OneTeamHasTechnicalDefeatInSet_BallsDontCount()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture().WithTechnicalDefeatInSet().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            var expected = new StandingsTestFixture().WithOneSetTechnicalDefeat().Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID);

            // Assert
            AssertStandingsAreEqual(
                expected,
                actual,
                "When team has got technical defeat in set balls from this set should not be accounted in the statistics");
        }

        [TestMethod]
        public void GetStandings_HomeTeamHasPenalty_PenaltyDeductedFromTotalPoints()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture().WithHomeTeamPenalty().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            var expected = new StandingsTestFixture().WithTeamAPenalty().Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID);

            // Assert
            AssertStandingsAreEqual(expected, actual, "Total points should be reduced by penalty ammount for penalized team");
        }

        [TestMethod]
        public void GetStandings_AwayTeamHasPenalty_PenaltyDeductedFromTotalPoints()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture().WithAwayTeamPenalty().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            var expected = new StandingsTestFixture().WithTeamCPenalty().Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID);

            // Assert
            AssertStandingsAreEqual(expected, actual, "Total points should be reduced by penalty ammount for penalized team");
        }

        [TestMethod]
        public void GetStandings_OneTeamNoLostSets_GetsToTheTopWhenOrderedBySetsRatio()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture().WithNoLostSetsForOneTeam().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();
            var expected = new StandingsTestFixture().WithMaxSetsRatioForOneTeam().Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID);

            // Assert
            AssertStandingsAreEqual(expected, actual, "Team with no lost sets should be considered higher comparing to other teams when ordering by sets ratio");
        }

        [TestMethod]
        public void GetStandings_OneTeamNoLostSetsNoLostBalls_GetsToTheTopWhenOrderedByBallsRatio()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture().WithNoLostSetsNoLostBallsForOneTeam().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();
            var expected = new StandingsTestFixture().WithMaxBallsRatioForOneTeam().Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID);

            // Assert
            AssertStandingsAreEqual(expected, actual, "Team with no lost balls should be considered higher comparing to other teams when ordering by balls ratio");
        }

        [TestMethod]
        public void GetStandings_GameResultsAllPossibleScores_TeamsOrderedByPoints()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture().WithResultsForUniquePoints().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            var expected = new StandingsTestFixture().WithUniquePoints().Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID);

            // Assert
            AssertStandingsAreEqual(expected, actual, "Teams should be ordered by points.");
        }

        [TestMethod]
        public void GetStandings_SeveralTeamsWithSamePoints_TeamsOrderedByWonGames()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture().WithResultsForSamePoints().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            var expected = new StandingsTestFixture().WithSamePoints().Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID);

            // Assert
            AssertStandingsAreEqual(expected, actual, "Teams should be ordered by games won.");
        }

        [TestMethod]
        public void GetStandings_SeveralTeamsWithSamePointsAndWonGames_TeamsOrderedBySetsRatio()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture().WithResultsForSamePointsAndWonGames().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            var expected = new StandingsTestFixture().WithSamePointsAndWonGames().Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID);

            // Assert
            AssertStandingsAreEqual(expected, actual, "Teams should be ordered by set ratios.");
        }

        [TestMethod]
        public void GetStandings_SeveralTeamsWithSamePointsWonGamesAndSetsRatio_TeamsOrderedByBallsRatio()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture()
                .WithResultsForSamePointsWonGamesAndSetsRatio().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            var expected = new StandingsTestFixture().WithSamePointsWonGamesAndSetsRatio().Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID);

            // Assert
            AssertStandingsAreEqual(expected, actual, "Teams should be ordered by balls ratio.");
        }

        private static void AssertStandingsAreEqual(TournamentStandings<StandingsDto> expected, TournamentStandings<StandingsDto> actual, string message)
        {
            int compareResult;
            var errorDetails = string.Empty;
            try
            {
                compareResult = new TournamentStandingsComparer<StandingsDto>(new StandingsDtoComparer())
                    .Compare(expected, actual);
            }
            catch (AssertFailedException e)
            {
                compareResult = -1;
                errorDetails = $" Error Details: {e.Message}";
            }

            Assert.IsTrue(compareResult == 0, $"{message}{errorDetails}");
        }
    }
}
