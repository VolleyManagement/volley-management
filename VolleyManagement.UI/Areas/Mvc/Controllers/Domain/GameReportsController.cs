namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System.Collections.Generic;
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
        private readonly ITournamentService _tournamentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameReportsController"/> class.
        /// </summary>
        /// <param name="gameReportService">Instance of a class which implements <see cref="IGameReportService"/>.</param>
        /// <param name="tournamentService">Instance of a class which implements <see cref="ITournamentService"/>.</param>
        public GameReportsController(IGameReportService gameReportService, ITournamentService tournamentService)
        {
            _gameReportService = gameReportService;
            _tournamentService = tournamentService;
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
                var tournament = _tournamentService.Get(tournamentId);
                var standings = _gameReportService.GetStandings(tournamentId);
                var pivots = _gameReportService.GetPivotStandings(tournamentId);
                var mapedStandings = new List<List<StandingsEntryViewModel>>();
                var pivotTables = new List<PivotTableViewModel>();

                mapedStandings = standings.Select(item => TeamStandingsViewModelBase.
                                                            SetPositions(item.Select(se => StandingsEntryViewModel.Map(se)).
                                                                              ToList())).
                                           ToList();

                pivotTables = pivots.Select(item => new PivotTableViewModel(item)).ToList();

                var standingsViewModel = new StandingsViewModel
                {
                    TournamentId = tournamentId,
                    TournamentName = tournamentName,
                    Standings = mapedStandings,
                    PivotTable = pivotTables,
                    LastTimeUpdated = tournament.LastTimeUpdated
                };

                return View(standingsViewModel);
            }
            else
            {
                var standingsViewModel = new StandingsViewModel
                {
                    TournamentId = tournamentId,
                    TournamentName = tournamentName,
                    Message = Resources.UI.GameReportViews.StandingsNotAvaliable
                };
                return View("StandingsNotAvailable", standingsViewModel);
            }
        }
    }
}
