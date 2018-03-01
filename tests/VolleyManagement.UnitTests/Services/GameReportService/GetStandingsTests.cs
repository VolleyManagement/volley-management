namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.GameReportsAggregate;
    using Domain.TournamentsAggregate;
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
            AssertStandingsAreEqual(expected, actual, "When there are games scheduled but no results standing entries should be empty.");
        }

        [TestMethod]
        public void GetStandings_GameResultsAllPossibleScores_CorrectPointStats()
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
            AssertPointsAreEqual(expected, actual, "Points should be calculated properly.");
        }

        [TestMethod]
        public void GetStandings_GameResultsAllPossibleScores_CorrectGamesStats()
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
            AssertGamesAreEqual(expected, actual, "Games should be calculated properly.");
        }

        [TestMethod]
        public void GetStandings_GameResultsAllPossibleScores_CorrectSetsStats()
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
            AssertSetsAreEqual(expected, actual, "Sets should be calculated properly.");
        }

        [TestMethod]
        public void GetStandings_GameResultsAllPossibleScores_CorrectBallStats()
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
            AssertBallsAreEqual(expected, actual, "Balls should be calculated properly.");
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
        public void GetStandings_OneTeamHasTechnicalDefeatInGame_BallsDontCount()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture().WithTechnicalDefeatInGame().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            var expected = new StandingsTestFixture().WithTechnicalDefeatInGame().Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID);

            // Assert
            AssertStandingsAreEqual(
                expected,
                actual,
                "When team has got technical defeat in game balls from this game should not be accounted in the statistics");
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
        public void GetStandings_HasGameResults_LastStandingsUpdateTimeIsReturned()
        {
            // Arrange
            var LAST_UPDATE_TIME = new DateTime(2017, 4, 7, 23, 7, 45);

            var gameResultsTestData = new GameResultsTestFixture().WithAllPossibleScores().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();
            var testTour = CreateSingleDivisionTournament(TOURNAMENT_ID, LAST_UPDATE_TIME);

            var expected = new StandingsTestFixture()
                                    .WithStandingsForAllPossibleScores()
                                    .WithLastUpdateTime(LAST_UPDATE_TIME)
                                    .Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);
            MockTournamentByIdQuery(TOURNAMENT_ID, testTour);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID);

            // Assert
            AssertStandingsAreEqual(
                expected,
                actual,
                "Standings should be properly calclulated for case of several divisions");
        }

        [TestMethod]
        public void GetMultipleDivisionStandings_GameResultsAllPossibleScores_CorrectStats()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture().WithMultipleDivisionsAllPosibleScores().Build();
            var teamsTestData = TeamsInTwoDivisionTwoGroups();
            var testTour = CreateTwoDivisionsTournament(TOURNAMENT_ID);

            var expected = new StandingsTestFixture().WithMultipleDivisionsAllPossibleScores().Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);
            MockTournamentByIdQuery(TOURNAMENT_ID, testTour);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID);

            // Assert
            AssertStandingsAreEqual(
                expected,
                actual,
                "Standings should be properly calclulated for case of several divisions");
        }

        [TestMethod]
        public void GetMultipleDivisionStandings_NoGameResults_StandingsAreEmpty()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture().WithNoGameResults().Build();
            var teamsTestData = TeamsInTwoDivisionTwoGroups();
            var testTour = CreateTwoDivisionsTournament(TOURNAMENT_ID);

            var expected = new StandingsTestFixture().WithMultipleDivisionsEmptyStandings().Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);
            MockTournamentByIdQuery(TOURNAMENT_ID, testTour);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID);

            // Assert
            AssertStandingsAreEqual(
                expected,
                actual,
                "Standings should be properly calclulated for case of several divisions");
        }

        private static void AssertStandingsAreEqual(TournamentStandings<StandingsDto> expected, TournamentStandings<StandingsDto> actual, string message)
        {
            StandingsEntryComparer standingsComparer = new StandingsEntryComparer();
            AssertTournamentStandingsAreEqual(expected, actual, message, new StandingsDtoComparer(standingsComparer));
        }

        private static void AssertPointsAreEqual(TournamentStandings<StandingsDto> expected, TournamentStandings<StandingsDto> actual, string message)
        {
            StandingsEntryComparer standingsComparer = new StandingsEntryComparer();
            standingsComparer.WithPointsComparer();
            AssertTournamentStandingsAreEqual(expected, actual, message, new StandingsDtoComparer(standingsComparer));
        }

        private static void AssertGamesAreEqual(TournamentStandings<StandingsDto> expected, TournamentStandings<StandingsDto> actual, string message)
        {
            StandingsEntryComparer standingsComparer = new StandingsEntryComparer();
            standingsComparer.WithGamesComparer();
            AssertTournamentStandingsAreEqual(expected, actual, message, new StandingsDtoComparer(standingsComparer));
        }

        private static void AssertSetsAreEqual(TournamentStandings<StandingsDto> expected, TournamentStandings<StandingsDto> actual, string message)
        {
            StandingsEntryComparer standingsComparer = new StandingsEntryComparer();
            standingsComparer.WithSetsComparer();
            AssertTournamentStandingsAreEqual(expected, actual, message, new StandingsDtoComparer(standingsComparer));
        }

        private static void AssertBallsAreEqual(TournamentStandings<StandingsDto> expected, TournamentStandings<StandingsDto> actual, string message)
        {
            StandingsEntryComparer standingsComparer = new StandingsEntryComparer();
            standingsComparer.WithBallsComparer();
            AssertTournamentStandingsAreEqual(expected, actual, message, new StandingsDtoComparer(standingsComparer));
        }

    }
}
