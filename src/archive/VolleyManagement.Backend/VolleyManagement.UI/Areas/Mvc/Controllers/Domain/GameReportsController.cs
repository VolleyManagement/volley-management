namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Contracts;
    using ViewModels.GameReports;

    /// <summary>
    /// Represents a controller that contains game reports actions.
    /// </summary>
    public class GameReportsController : Controller
    {
        private readonly IGameReportService _gameReportService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameReportsController"/> class.
        /// </summary>
        /// <param name="gameReportService">Instance of a class which implements <see cref="IGameReportService"/>.</param>
        public GameReportsController(IGameReportService gameReportService)
        {
            _gameReportService = gameReportService;
        }

        /// <summary>
        /// Renders view with standings of the tournament specified by identifier.
        /// </summary>
        /// <param name="tournamentId">Identifier of the tournament.</param>
        /// <param name="tournamentName">Name of the tournament.</param>
        /// <returns>View with standings of the tournament.</returns>
        public ActionResult Standings(int tournamentId, string tournamentName)
        {
            if (_gameReportService.IsStandingAvailable(tournamentId))
            {
                var standings = _gameReportService.GetStandings(tournamentId);
                var pivots = _gameReportService.GetPivotStandings(tournamentId);

                var mapedStandings = standings.Divisions
                    .Select(item =>
                    {
                        var standingsTable = DivisionStandingsViewModel.Map(item);
                        TeamStandingsViewModelBase.SetPositions(standingsTable.StandingsEntries);
                        return standingsTable;
                    })
                    .ToList();

                var pivotTables = pivots.Divisions.Select(item => new PivotTableViewModel(item)).ToList();

                var standingsViewModel = new StandingsViewModel {
                    TournamentId = tournamentId,
                    TournamentName = tournamentName,
                    StandingsTable = mapedStandings,
                    PivotTable = pivotTables
                };

                return View(standingsViewModel);
            }
            else
            {
                var standingsViewModel = new StandingsViewModel {
                    TournamentId = tournamentId,
                    TournamentName = tournamentName,
                    Message = Resources.UI.GameReportViews.StandingsNotAvaliable
                };
                return View("StandingsNotAvailable", standingsViewModel);
            }
        }
    }
}
