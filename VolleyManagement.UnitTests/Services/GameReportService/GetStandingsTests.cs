namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.GameReportsAggregate;
    using Domain.TeamsAggregate;
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
            var gameResultsData = new GameResultsTestFixture().WithNoGameResults().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            var expected = new StandingsTestFixture().WithNoResults().Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID);

            // Assert
            AssertStandingsAreEqual(expected, actual, "For each team in tournament empty standings entry should be created");
        }

        [TestMethod]
        public void GetStandings_ScheduledGamesButNoResults_StandingEntriesAreEmpty()
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
            AssertStandingsAreEqual(expected, actual, "When there are games scheduled but no results standing entries should be emoty");
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
            AssertStandingsAreEqual(expected, actual, "Points, games, sets and balls should be calculated properly");
        }





        /// <summary>
        /// Test for GetStandings() method. There is one set with technical defeat in results.
        /// Balls in technical defeat set dont count for statistics.
        /// </summary>
        [TestMethod]
        public void GetStandings_OneTeamHasTechnicalDefeatInSet_BallsDontCount()
        {
            // Arrange
            var gameResultsTestData = new GameResultsTestFixture().WithTechnicalDefeatInSet().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            var expected = new StandingsTestFixture().WithTeamStandingsForOneSetTechnicalDefeat()
                .Build()
                .Select(item => item.BallsRatio)
                .ToList();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

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
            var gameResultsTestData = new GameResultsTestFixture().WithHomeTeamPenalty().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

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
            var gameResultsTestData = new GameResultsTestFixture().WithAwayTeamPenalty().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            var expected = new StandingsTestFixture().WithTeamCPenalty()
                .OrderByPoints()
                .Build()
                .Select(item => item.Points)
                .ToList();

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupTournamentTeamsGroupedByDivisionsQuery(TOURNAMENT_ID, teamsTestData);

            var sut = BuildSUT();

            // Act
            var standings = sut.GetStandings(TOURNAMENT_ID);

            var actual = standings.First().Select(item => item.Points).ToList();

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
            var gameResultsTestData = new GameResultsTestFixture().WithNoLostSetsForOneTeam().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();
            var expected = float.PositiveInfinity;

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

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
            var gameResultsTestData = new GameResultsTestFixture().WithNoLostSetsNoLostBallsForOneTeam().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();
            var expected = new { SetsRatio = (float?)float.PositiveInfinity, BallsRatio = (float?)null };

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

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
            var gameResultsTestData = new GameResultsTestFixture().WithResultsForUniquePoints().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            var expected = new StandingsTestFixture().WithUniquePoints().Build();
            expected = expected.OrderByDescending(s => s.Points).ToList();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

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
            var gameResultsTestData = new GameResultsTestFixture().WithResultsForSamePoints().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            var expected = new StandingsTestFixture().WithSamePoints().Build();
            expected = expected.OrderByDescending(s => s.Points)
                               .ThenByDescending(s => s.GamesWon)
                               .ToList();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

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
            var gameResultsTestData = new GameResultsTestFixture()
                .WithResultsForSamePointsAndWonGames().Build();
            var teamsTestData = TeamsInSingleDivisionSingleGroup();

            var expected = new StandingsTestFixture().WithSamePointsAndWonGames().Build();

            MockTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            MockTournamentTeamsQuery(TOURNAMENT_ID, teamsTestData);

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
            AssertPositionsAreEqual(actual, expected);
        }

        private static void AssertStandingsAreEqual(TournamentStandings<StandingsDto> expected, TournamentStandings<StandingsDto> actual, string message)
        {
            Assert.IsTrue(
                new TournamentStandingsComparer<StandingsDto>(new StandingsDtoComparer())
                    .Compare(expected, actual) == 0,
                message);
        }
    }
}
