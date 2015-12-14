namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using VolleyManagement.Contracts;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.GameReports;

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
        /// <returns>View with standings of the tournament.</returns>
        public ActionResult Standings(int tournamentId)
        {
            var standingsViewModel = new StandingsViewModel
            {
                TournamentId = tournamentId,
                TournamentName = _tournamentService.Get(tournamentId).Name,
                Entries = _gameReportService.GetStandings(tournamentId).Select(se => StandingsEntryViewModel.Map(se)).ToList()
            };

            return View(standingsViewModel);
        }
    }
}
