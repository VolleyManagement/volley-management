namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System;
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

            var expected = new PivotStandingsTestFixture().WithEmptyResults().Build();

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID);

            // Assert
            AssertPivotStandingsAreEqual(expected, actual, "When there are games scheduled but no results standing entries should be empty");
        }

        [TestMethod]
        public void GetPivotStandings_GameResultsAllPossibleScores_CorrectPointStats()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture().WithAllPossibleScores().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var expected = new PivotStandingsTestFixture().WithStandingsForAllPossibleScores().Build();

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID);

            // Assert
            AssertPointsAreEqual(expected, actual, "Points should be calculated properly.");
        }

        [TestMethod]
        public void GetPivotStandings_GameResultsAllPossibleScores_CorrectBallStats()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture().WithAllPossibleScores().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var expected = new PivotStandingsTestFixture().WithStandingsForAllPossibleScores().Build();

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID);

            // Assert
            AssertBallsAreEqual(expected, actual, "Balls should be calculated properly.");
        }
        [TestMethod]
        public void GetPivotStandings_GameResultsAllPossibleScores_CorrectGamesStats()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture().WithAllPossibleScores().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var expected = new PivotStandingsTestFixture().WithStandingsForAllPossibleScores().Build();

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID);

            // Assert
            AssertGameStatusAreEqual(expected, actual, "Game status should be correct.");
        }
        [TestMethod]
        public void GetPivotStandings_GameResultsAllPossibleScores_CorrectSetsStats()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture().WithAllPossibleScores().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var expected = new PivotStandingsTestFixture().WithStandingsForAllPossibleScores().Build();

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID);

            // Assert
            AssertSetsAreEqual(expected, actual, "Sets should be calculated properly.");
        }
        [TestMethod]
        public void GetPivotStandings_GameResultsAllPossibleScores_TeamsOrderedByPoints()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture().WithResultsForUniquePoints().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            var expected = new PivotStandingsTestFixture().WithUniquePoints().Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID);

            // Assert
            AssertPivotStandingsAreEqual(expected, actual, "Teams should be ordered by points.");
        }

        [TestMethod]
        public void GetPivotStandings_SeveralTeamsWithSamePoints_TeamsOrderedByWonGames()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture().WithResultsForSamePoints().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            var expected = new PivotStandingsTestFixture().WithSamePoints().Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID);

            // Assert
            AssertPivotStandingsAreEqual(expected, actual, "Teams should be ordered by games won.");
        }

        [TestMethod]
        public void GetPivotStandings_SeveralTeamsWithSamePointsAndWonGames_TeamsOrderedBySetsRatio()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture().WithResultsForSamePointsAndWonGames().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            var expected = new PivotStandingsTestFixture().WithSamePointsAndWonGames().Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID);

            // Assert
            AssertPivotStandingsAreEqual(expected, actual, "Teams should be ordered by set ratios.");
        }

        [TestMethod]
        public void GetPivotStandings_SeveralTeamsWithSamePointsWonGamesAndSetsRatio_TeamsOrderedByBallsRatio()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture()
                .WithResultsForSamePointsWonGamesAndSetsRatio().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            var expected = new PivotStandingsTestFixture().WithSamePointsWonGamesAndSetsRatio().Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID);

            // Assert
            AssertPivotStandingsAreEqual(expected, actual, "Teams should be ordered by balls ratio.");
        }

        [TestMethod]
        public void GetPivotStandings_OneTeamNoLostSets_GetsToTheTopWhenOrderedBySetsRatio()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture().WithNoLostSetsForOneTeam().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            var expected = new PivotStandingsTestFixture().WithMaxSetsRatioForOneTeam().Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID);

            // Assert
            AssertPivotStandingsAreEqual(expected, actual, "Team with no lost sets should be considered higher comparing to other teams when ordering by sets ratio");
        }

        [TestMethod]
        public void GetPivotStandings_OneTeamNoLostSetsNoLostBalls_GetsToTheTopWhenOrderedByBallsRatio()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture().WithNoLostSetsNoLostBallsForOneTeam().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();
            var expected = new PivotStandingsTestFixture().WithMaxBallsRatioForOneTeam().Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID);

            // Assert
            AssertPivotStandingsAreEqual(expected, actual, "Team with no lost balls should be considered higher comparing to other teams when ordering by balls ratio");
        }

        [TestMethod]
        public void GetPivotStandings_HomeTeamHasPenalty_PenaltyDeductedFromTotalPoints()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture().WithHomeTeamPenalty().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            var expected = new PivotStandingsTestFixture().WithTeamAPenalty().Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID);

            // Assert
            AssertPivotStandingsAreEqual(expected, actual, "Total points should be reduced by penalty amount for penalized team");
        }

        [TestMethod]
        public void GetPivotStandings_AwayTeamHasPenalty_PenaltyDeductedFromTotalPoints()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture().WithAwayTeamPenalty().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();
            var testTour = CreateSingleDivisionTournament(TOURNAMENT_ID);

            var expected = new PivotStandingsTestFixture().WithTeamCPenalty().Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);
            MockTournamentByIdQuery(TOURNAMENT_ID, testTour);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID);

            // Assert
            AssertPivotStandingsAreEqual(expected, actual, "Total points should be reduced by penalty ammount for penalized team");
        }

        [TestMethod]
        public void GetPivotStandings_OneTeamHasTechnicalDefeatInGame_BallsDontCount()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture().WithTechnicalDefeatInGame().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            var expected = new PivotStandingsTestFixture().WithTechnicalDefeatInGame().Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID);

            // Assert
            AssertPivotStandingsAreEqual(
                expected,
                actual,
                "When team has got technical defeat in game balls from this game should not be accounted in the statistics");
        }

        [TestMethod]
        public void GetPivotStandings_OneTeamHasTechnicalDefeatInSet_BallDoesntCount()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture().WithTechnicalDefeatInSet().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            var expected = new PivotStandingsTestFixture().WithTechnicalDefeatInSet().Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID);

            // Assert
            AssertPivotStandingsAreEqual(expected, actual, "When team has got technical defeat in set balls from this set should not be accounted in the statistics");
        }

        [TestMethod]
        public void GetPivotStandings_HasGameResults_LastStandingsUpdateTimeIsReturned()
        {
            // Arrange
            var LAST_UPDATE_TIME = new DateTime(2017, 4, 7, 23, 7, 45);

            var gameResultsTestData = new GameResultsTestFixture().WithAllPossibleScores().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();
            var testTour = CreateSingleDivisionTournament(TOURNAMENT_ID, LAST_UPDATE_TIME);

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);
            MockTournamentByIdQuery(TOURNAMENT_ID, testTour);

            var expected = new PivotStandingsTestFixture()
                .WithStandingsForAllPossibleScores()
                .WithLastUpdateTime(LAST_UPDATE_TIME)
                .Build();

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID);

            // Assert
            AssertPivotStandingsAreEqual(expected, actual, "Points, games, sets and balls should be calculated properly.");
        }

        [TestMethod]
        public void GetMultipleDivisionPivotStandings_GameResultsAllPossibleScores_CorrectStats()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture()
                .WithMultipleDivisionsAllPosibleScores()
                .Build();

            var teamsTestData = TeamsInTwoDivisionTwoGroups();
            var testTour = CreateTwoDivisionsTournament(TOURNAMENT_ID);

            var expected = new PivotStandingsTestFixture()
                .WithMultipleDivisionsAllPossibleScores()
                .Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);
            MockTournamentByIdQuery(TOURNAMENT_ID, testTour);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID);

            // Assert
            AssertPivotStandingsAreEqual(
                expected,
                actual,
                "Standings should be properly calclulated for case of several divisions");
        }

        [TestMethod]
        public void GetMultipleDivisionPivotStandings_NoGameResults_StandingsAreEmpty()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture().WithMultipleDivisionsEmptyResults().Build();
            var teamsTestData = TeamsInTwoDivisionTwoGroups();
            var testTour = CreateTwoDivisionsTournament(TOURNAMENT_ID);

            var expected = new PivotStandingsTestFixture().WithMultipleDivisionsEmptyStandings().Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);
            MockTournamentByIdQuery(TOURNAMENT_ID, testTour);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID);

            // Assert
            AssertPivotStandingsAreEqual(
                expected,
                actual,
                "Standings should be properly calclulated for case of several divisions");
        }

        [TestMethod]
        public void GetPivotStandings_PlannedGamesWithoutResults_StandingsContainPlannedGames()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture()
                .WithNotAllGamesPlayed()
                .Build();

            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var expected = new PivotStandingsTestFixture()
                .WithNotAllGamesPlayed()
                .Build();

            var sut = BuildSUT();

            // Act
            var actual = sut.GetPivotStandings(TOURNAMENT_ID);

            // Assert
            AssertPivotStandingsAreEqual(expected, actual, "Points, games, sets and balls should be calculated properly.");
        }

        private void AssertPivotStandingsAreEqual(
            TournamentStandings<PivotStandingsDto> expected,
            TournamentStandings<PivotStandingsDto> actual,
            string message)
        {
            AssertTournamentStandingsAreEqual(expected, actual, message, new PivotStandingsComparer());
        }

        private void AssertPointsAreEqual(
           TournamentStandings<PivotStandingsDto> expected,
           TournamentStandings<PivotStandingsDto> actual,
           string message)
        {
            PivotStandingsComparer comparer = new PivotStandingsComparer();
            comparer.WithPointsComparer();
            AssertTournamentStandingsAreEqual(expected, actual, message, comparer);
        }

        private void AssertSetsAreEqual(
           TournamentStandings<PivotStandingsDto> expected,
           TournamentStandings<PivotStandingsDto> actual,
           string message)
        {
            PivotStandingsComparer comparer = new PivotStandingsComparer();
            comparer.WithSetRatioComparer();
            AssertTournamentStandingsAreEqual(expected, actual, message, comparer);
        }

        private void AssertBallsAreEqual(
          TournamentStandings<PivotStandingsDto> expected,
          TournamentStandings<PivotStandingsDto> actual,
          string message)
        {
            PivotStandingsComparer comparer = new PivotStandingsComparer();
            comparer.WithBallsRatioComparer();
            AssertTournamentStandingsAreEqual(expected, actual, message, comparer);
        }

        private void AssertGameStatusAreEqual(
           TournamentStandings<PivotStandingsDto> expected,
           TournamentStandings<PivotStandingsDto> actual,
           string message)
        {
            PivotStandingsComparer comparer = new PivotStandingsComparer();
            comparer.WithGameResultComparer();
            AssertTournamentStandingsAreEqual(expected, actual, message, comparer);
        }
    }
}