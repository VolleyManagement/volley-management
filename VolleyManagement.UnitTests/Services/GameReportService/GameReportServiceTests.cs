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
    using VolleyManagement.Domain.GamesAggregate;
    using VolleyManagement.Services;
    using VolleyManagement.UnitTests.Services.GameService;

    /// <summary>
    /// Tests for <see cref="GameReportService"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class GameReportServiceTests
    {
        private const int TOURNAMENT_ID = 1;

        private const int TOP_TEAM_INDEX = 0;

        private readonly Mock<IQuery<List<GameResultDto>, TournamentGameResultsCriteria>> _tournamentGameResultsQueryMock =
            new Mock<IQuery<List<GameResultDto>, TournamentGameResultsCriteria>>();

        private IKernel _kernel;

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IQuery<List<GameResultDto>, TournamentGameResultsCriteria>>()
                .ToConstant(_tournamentGameResultsQueryMock.Object);
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
        /// Points statistics in tournament standings is calculated correctly.
        /// </summary>
        [TestMethod]
        public void GetStandings_GameResultsAllPossibleScores_CorrectPointsStats()
        {
            // Arrange
            var gameResultsTestData = new GameResultTestFixture().WithAllPossibleScores().Build();
            var sut = _kernel.Get<GameReportService>();
            var expected = new StandingsTestFixture().WithStandingsForAllPossibleScores()
                .OrderByPointsAndSetsAndBalls()
                .Build()
                .Select(s => s.Points)
                .ToList();

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);

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
            var gameResultsTestData = new GameResultTestFixture().WithAllPossibleScores().Build();
            var sut = _kernel.Get<GameReportService>();
            var expected = new StandingsTestFixture().WithStandingsForAllPossibleScores()
                .OrderByPointsAndSetsAndBalls()
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
            var gameResultsTestData = new GameResultTestFixture().WithAllPossibleScores().Build();
            var sut = _kernel.Get<GameReportService>();
            var expected = new StandingsTestFixture().WithStandingsForAllPossibleScores()
                .OrderByPointsAndSetsAndBalls()
                .Build()
                .Select(s => new
                {
                    SetsWon = s.SetsWon,
                    SetsLost = s.SetsLost,
                    SetsRatio = s.SetsRatio
                })
                .ToList();

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);

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
            var gameResultsTestData = new GameResultTestFixture().WithAllPossibleScores().Build();
            var sut = _kernel.Get<GameReportService>();
            var expected = new StandingsTestFixture().WithStandingsForAllPossibleScores()
                .OrderByPointsAndSetsAndBalls()
                .Build()
                .Select(s => new
                {
                    BallsWon = s.BallsWon,
                    BallsLost = s.BallsLost,
                    BallsRatio = s.BallsRatio
                })
                .ToList();

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);

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
            var gameResultsTestData = new GameResultTestFixture().WithNoLostSetsForOneTeam().Build();
            var sut = _kernel.Get<GameReportService>();
            var expected = float.PositiveInfinity;

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);

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
            var gameResultsTestData = new GameResultTestFixture().WithNoLostSetsNoLostBallsForOneTeam().Build();
            var sut = _kernel.Get<GameReportService>();
            var expected = new { SetsRatio = float.PositiveInfinity, BallsRatio = float.PositiveInfinity };

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);

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
            var gameResultsTestData = new GameResultTestFixture().WithResultsForUniquePoints().Build();
            var sut = _kernel.Get<GameReportService>();
            var expected = new StandingsTestFixture().WithUniquePoints().Build()[TOP_TEAM_INDEX].Points;

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);

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
            var gameResultsTestData = new GameResultTestFixture().WithResultsForRepetitivePoints().Build();
            var sut = _kernel.Get<GameReportService>();
            var expected = new StandingsTestFixture().WithRepetitivePoints().OrderByPoints().Build()[TOP_TEAM_INDEX].SetsRatio;

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);

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
            var gameResultsTestData = new GameResultTestFixture().WithResultsForRepetitivePointsAndSetsRatio().Build();
            var sut = _kernel.Get<GameReportService>();
            var expected = new StandingsTestFixture().WithRepetitivePointsAndSetsRatio()
                .OrderByPointsAndSets()
                .Build()[TOP_TEAM_INDEX]
                .BallsRatio;

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID)[TOP_TEAM_INDEX].BallsRatio;

            // Assert
            Assert.AreNotEqual(expected, actual);
        }

        private void SetupTournamentGameResultsQuery(int tournamentId, List<GameResultDto> testData)
        {
            _tournamentGameResultsQueryMock.Setup(m =>
                m.Execute(It.Is<TournamentGameResultsCriteria>(c => c.TournamentId == tournamentId)))
                .Returns(testData);
        }
    }
}
