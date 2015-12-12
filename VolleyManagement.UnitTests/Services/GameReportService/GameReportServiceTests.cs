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

        private readonly Mock<IQuery<Team, FindByIdCriteria>> _getTeamByIdQueryMock =
            new Mock<IQuery<Team, FindByIdCriteria>>();

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
            _kernel.Bind<IQuery<Team, FindByIdCriteria>>().ToConstant(_getTeamByIdQueryMock.Object);
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
            var teamIds = teamsTestData.Select(t => t.Id);
            var expected = new StandingsTestFixture().TestStandings().Build();

            SetupGetTournamentGameResultsQuery(TOURNAMENT_ID, gameResultsTestData);
            SetupGetTeamByIdQuery(teamIds, teamsTestData);

            // Act
            var actual = _sut.GetStandings(TOURNAMENT_ID);

            // Assert
            CollectionAssert.AreEqual(expected, actual, new StandingsEntryComparer());
        }

        private void SetupGetTournamentGameResultsQuery(int tournamentId, List<GameResult> testData)
        {
            _getTournamentGameResultsQueryMock.Setup(m =>
                m.Execute(It.Is<TournamentGameResultsCriteria>(c => c.TournamentId == tournamentId)))
                .Returns(testData);
        }

        private void SetupGetTeamByIdQuery(IEnumerable<int> teamIds, List<Team> testData)
        {
            _getTeamByIdQueryMock.Setup(m => m.Execute(It.Is<FindByIdCriteria>(c => teamIds.Contains(c.Id))))
                .Returns<FindByIdCriteria>(c => testData.SingleOrDefault(td => td.Id == c.Id));
        }
    }
}
