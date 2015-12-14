namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Contracts;
    using VolleyManagement.Domain.GameReportsAggregate;
    using VolleyManagement.Domain.TournamentsAggregate;
    using VolleyManagement.UI.Areas.Mvc.Controllers;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.GameReports;
    using VolleyManagement.UnitTests.Mvc.ViewModels;
    using VolleyManagement.UnitTests.Services.GameReportService;
    using VolleyManagement.UnitTests.Services.TournamentService;

    /// <summary>
    /// Tests for <see cref="GameReportsController"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class GameReportsControllerTests
    {
        private const int TOURNAMENT_ID = 1;

        private readonly Mock<IGameReportService> _gameReportServiceMock =
            new Mock<IGameReportService>();

        private readonly Mock<ITournamentService> _tournamentServiceMock =
            new Mock<ITournamentService>();

        private IKernel _kernel;

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IGameReportService>().ToConstant(_gameReportServiceMock.Object);
            _kernel.Bind<ITournamentService>().ToConstant(_tournamentServiceMock.Object);
        }

        /// <summary>
        /// Test for Standings() method. Tournament standings are requested. Tournament standings are returned.
        /// </summary>
        [TestMethod]
        public void Standings_StandingsRequested_StandingsReturned()
        {
            // Arrange
            var testTournament = new TournamentBuilder().Build();
            var testStandings = new StandingsTestFixture().TestStandings().Build();
            var expected = new StandingsViewModelBuilder().Build();
            var sut = _kernel.Get<GameReportsController>();

            SetupTournamentGet(TOURNAMENT_ID, testTournament);
            SetupGameReportGetStandings(TOURNAMENT_ID, testStandings);

            // Act
            var actual = TestExtensions.GetModel<StandingsViewModel>(sut.Standings(TOURNAMENT_ID));

            // Assert
            TestHelper.AreEqual(expected, actual, new StandingsViewModelComparer());
        }

        private void SetupTournamentGet(int tournamentId, Tournament tournament)
        {
            _tournamentServiceMock.Setup(m => m.Get(It.Is<int>(id => id == tournamentId))).Returns(tournament);
        }

        private void SetupGameReportGetStandings(int tournamentId, List<StandingsEntry> testData)
        {
            _gameReportServiceMock.Setup(m => m.GetStandings(It.Is<int>(id => id == tournamentId))).Returns(testData);
        }
    }
}
