namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Queries.GameResult;
    using VolleyManagement.Domain.GameResultsAggregate;
    using VolleyManagement.Services;
    using VolleyManagement.UnitTests.Services.GameResultService;

    /// <summary>
    /// Tests for <see cref="GameReportService"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class GameReportServiceTests
    {
        private const int TOURNAMENT_ID = 1;

        private const int TOP_TEAM_INDEX = 0;

        private readonly Mock<IQuery<List<GameResult>, TournamentGameResultsCriteria>> _tournamentGameResultsQueryMock =
            new Mock<IQuery<List<GameResult>, TournamentGameResultsCriteria>>();

        private IKernel _kernel;

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IQuery<List<GameResult>, TournamentGameResultsCriteria>>().ToConstant(_tournamentGameResultsQueryMock.Object);
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
        /// Tournament standings are calculated correctly.
        /// </summary>
        [TestMethod]
        public void GetStandings_GameResultsAllPossibleScores_StandingsCalculatedCorrectly()
        {
            // Arrange
            var gameResultsTestData = new GameResultTestFixture().WithAllPossibleScores().Build();
            var sut = _kernel.Get<GameReportService>();
            var expected = new StandingsTestFixture().WithStandingsForAllPossibleScores().Build();

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID);

            // Assert
            CollectionAssert.AreEqual(expected, actual, new StandingsEntryComparer());
        }

        /// <summary>
        /// Test for GetStandings() method. Test standings entries are ordered only by points and actual standings entries
        /// are ordered correctly (by points, by sets ratio and by balls ratio). Test tournament standings are not equal to actual ones.
        /// </summary>
        [TestMethod]
        public void GetStandings_EntriesOrderedByPoints_ExpectedDontMatchActual()
        {
            // Arrange
            var gameResultsTestData = new GameResultTestFixture().WithResultsForRepetitivePoints().Build();
            var sut = _kernel.Get<GameReportService>();
            var notExpected = new StandingsTestFixture().WithRepetitivePoints().Build();

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID);

            // Assert
            CollectionAssert.AreNotEqual(notExpected, actual, new StandingsEntryComparer());
        }

        /// <summary>
        /// Test for GetStandings() method. Test standings entries are ordered by points and by sets ratio and actual standings entries
        /// are ordered correctly (by points, by sets ratio and by balls ratio). Test tournament standings are not equal to actual ones.
        /// </summary>
        [TestMethod]
        public void GetStandings_EntriesOrderedByPointsAndSetsRatio_ExpectedDontMatchActual()
        {
            // Arrange
            var gameResultsTestData = new GameResultTestFixture().WithResultsForRepetitivePointsAndSetsRatio().Build();
            var sut = _kernel.Get<GameReportService>();
            var notExpected = new StandingsTestFixture().WithRepetitivePointsAndSetsRatio().Build();

            SetupTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);

            // Act
            var actual = sut.GetStandings(TOURNAMENT_ID);

            // Assert
            CollectionAssert.AreNotEqual(notExpected, actual, new StandingsEntryComparer());
        }

        private void SetupTournamentGameResultsQuery(int tournamentId, List<GameResult> testData)
        {
            _tournamentGameResultsQueryMock.Setup(q =>
                q.Execute(It.Is<TournamentGameResultsCriteria>(c => c.TournamentId == tournamentId)))
                .Returns(testData);
        }
    }
}
