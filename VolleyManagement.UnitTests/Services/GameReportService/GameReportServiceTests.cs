namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Data.Queries.GameResult;
    using VolleyManagement.Domain.GameResultsAggregate;
    using VolleyManagement.Domain.TeamsAggregate;
    using VolleyManagement.Services;
    using VolleyManagement.UnitTests.Services.GameResultService;
    using VolleyManagement.UnitTests.Services.TeamService;

    /// <summary>
    /// Tests for <see cref="GameReportService"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class GameReportServiceTests
    {
        private const int TOURNAMENT_ID = 1;

        private readonly Mock<IQuery<List<GameResult>, TournamentGameResultsCriteria>> _getTournamentGameResultsQueryMock =
            new Mock<IQuery<List<GameResult>, TournamentGameResultsCriteria>>();

        private readonly Mock<IQuery<List<Team>, GetAllCriteria>> _getAllTeamsQueryMock =
            new Mock<IQuery<List<Team>, GetAllCriteria>>();

        private GameReportService _sut;

        private IKernel _kernel;

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IQuery<List<GameResult>, TournamentGameResultsCriteria>>().ToConstant(_getTournamentGameResultsQueryMock.Object);
            _kernel.Bind<IQuery<List<Team>, GetAllCriteria>>().ToConstant(_getAllTeamsQueryMock.Object);
            _sut = _kernel.Get<GameReportService>();
        }

        /// <summary>
        /// Test for GetStandings() method. Tournament standings are requested. Tournament standings are returned.
        /// </summary>
        [TestMethod]
        public void GetStandings_StandingsRequested_StandingsReturned()
        {
            // Arrange
            var gameResultsTestData = new GameResultTestFixture().TestGameResults().Build();
            var teamsTestData = new TeamServiceTestFixture().TestTeams().Build();
            var expected = new StandingsTestFixture().TestStandings().Build();

            SetupGetTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupGetAllTeamsQuery(teamsTestData);

            // Act
            var actual = _sut.GetStandings(TOURNAMENT_ID);

            // Assert
            CollectionAssert.AreEqual(expected, actual, new StandingsEntryComparer());
        }

        private void SetupGetTournamentGameResultsQuery(int tournamentId, List<GameResult> testData)
        {
            _getTournamentGameResultsQueryMock.Setup(q =>
                q.Execute(It.Is<TournamentGameResultsCriteria>(c => c.TournamentId == tournamentId)))
                .Returns(testData);
        }

        private void SetupGetAllTeamsQuery(List<Team> testData)
        {
            _getAllTeamsQueryMock.Setup(q => q.Execute(It.IsAny<GetAllCriteria>())).Returns(testData);
        }
    }
}
