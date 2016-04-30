namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Contracts;
    using VolleyManagement.Domain.GameReportsAggregate;
    using VolleyManagement.UI.Areas.Mvc.Controllers;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.GameReports;
    using VolleyManagement.UnitTests.Mvc.ViewModels;
    using VolleyManagement.UnitTests.Services.GameReportService;

    /// <summary>
    /// Tests for <see cref="GameReportsController"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class GameReportsControllerTests
    {
        private const int TOURNAMENT_ID = 1;

        private const string TOURNAMENT_NAME = "Name";

        private readonly Mock<IGameReportService> _gameReportServiceMock = new Mock<IGameReportService>();

        private IKernel _kernel;

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IGameReportService>().ToConstant(_gameReportServiceMock.Object);
        }

        /// <summary>
        /// Test for Standings() method. Tournament standings are requested. Tournament standings are returned.
        /// </summary>
        [TestMethod]
        public void Standings_StandingsRequested_StandingsReturned()
        {
            // Arrange
            var teams = new TeamStandingsTestFixture().TestTeamStandings().Build();
            var gameResults = new ShortGameResultDtoTetsFixture().GetShortResults().Build();
            var testPivotStandings = new PivotStandingsDto(teams, gameResults);

            var testStandings = new StandingsTestFixture().TestStandings().Build();
            var sut = _kernel.Get<GameReportsController>();
            var expected = new StandingsViewModelBuilder().Build();

            SetupGameReportGetStandings(TOURNAMENT_ID, testStandings);
            SetupGameReportGetPivotStandings(TOURNAMENT_ID, testPivotStandings);

            // Act
            var actual = TestExtensions.GetModel<StandingsViewModel>(sut.Standings(TOURNAMENT_ID, TOURNAMENT_NAME));

            // Assert
            TestHelper.AreEqual(expected, actual, new StandingsViewModelComparer());
        }

        private void SetupGameReportGetStandings(int tournamentId, List<StandingsEntry> testData)
        {
            _gameReportServiceMock.Setup(m => m.GetStandings(It.Is<int>(id => id == tournamentId))).Returns(testData);
        }

        private void SetupGameReportGetPivotStandings(int tournamentId, PivotStandingsDto testData)
        {
            _gameReportServiceMock.Setup(m => m.GetPivotStandings(It.Is<int>(id => id == tournamentId))).Returns(testData);
        }
    }
}