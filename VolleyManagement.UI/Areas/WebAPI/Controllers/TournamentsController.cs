namespace VolleyManagement.UI.Areas.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Contracts;
    using ViewModels.GameReports;
    using ViewModels.Games;
    using ViewModels.Tournaments;
    using WebAPI.ViewModels.Schedule;
    using WebAPI.ViewModels.GameReports;

    /// <summary>
    /// The tournaments controller.
    /// </summary>
    public class TournamentsController : ApiController
    {
        private readonly ITournamentService _tournamentService;
        private readonly IGameReportService _gameReportService;
        private readonly IGameService _gameService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentsController"/> class.
        /// </summary>
        /// <param name="tournamentService"> The tournament service. </param>
        /// <param name="gameService"> The game service. </param>
        /// <param name="gameReportService"> The game report service. </param>
        public TournamentsController(
                ITournamentService tournamentService,
                IGameService gameService,
                IGameReportService gameReportService)
        {
            _tournamentService = tournamentService;
            _gameService = gameService;
            _gameReportService = gameReportService;
        }

        /// <summary>
        /// Gets all tournaments
        /// </summary>
        /// <returns>Collection of all tournaments</returns>
        public IEnumerable<TournamentViewModel> GetAllTournaments()
        {
            return _tournamentService.Get().Select(t => TournamentViewModel.Map(t));
        }

        /// <summary>
        /// Gets tournament by id
        /// </summary>
        /// <param name="id">Id of tournament</param>
        /// <returns>Information about the tournament with specified id</returns>
        public IHttpActionResult GetTournament(int id)
        {
            var tournament = _tournamentService.Get(id);
            if (tournament == null)
            {
                return NotFound();
            }

            return Ok(TournamentViewModel.Map(tournament));
        }

        /// <summary>
        /// Gets standings by tournament id
        /// </summary>
        /// <param name="id">Id of tournament</param>
        /// <returns>Standings entries of the tournament with specified id</returns>
        [Route("api/v1/Tournaments/{id}/Standings")]
        public List<DivisionStandingsViewModel> GetTournamentStandings(int id)
        {
            var result = new List<DivisionStandingsViewModel>();
            var entries = _gameReportService.GetStandings(id);
            foreach (var entry in entries.Divisions)
            {
                var standings = new DivisionStandingsViewModel
                {
                    LastUpdateTime = entry.LastUpdateTime,
                    StandingsTable = entry.Standings.Select(StandingsEntryViewModel.Map).ToList()
                };
                StandingsEntryViewModel.SetPositions(standings.StandingsTable);
                result.Add(standings);
            }

            return result;
        }

        /// <summary>
        /// Gets pivot standings by tournament id
        /// </summary>
        /// <param name="id">Id of tournament</param>
        /// <returns>Pivot standings entries of the tournament with specified id</returns>
        [Route("api/v1/Tournaments/{id}/PivotStandings")]
        public List<PivotStandingsViewModel> GetTournamentPivotStandings(int id)
        {
            var pivotData = _gameReportService.GetPivotStandings(id);
            return pivotData.Divisions.Select(item => new PivotStandingsViewModel(item)).ToList();
        }

        /// <summary>
        /// Gets games by tournament id.
        /// </summary>
        /// <param name="tournamentId">Id of tournament.</param>
        /// <returns>Information about games with specified tournament id.</returns>
        [Route("api/Tournament/{tournamentId}/Schedule")]
        public List<ScheduleByRoundViewModel> GetSchedule(int tournamentId)
        {
            List<GameViewModel> gamesViewModel = _gameService.GetTournamentResults(tournamentId)
                                                        .Select(t => GameViewModel.Map(t)).ToList();
            foreach (var item in gamesViewModel)
            {
                if (item.Result.TotalScore.IsEmpty)
                {
                    item.Result = null;
                }
            }

            // Group game results by date after that group them by round
            var result = gamesViewModel
                                 .GroupBy(gr => new { year = gr.Date.Year, month = gr.Date.Month, day = gr.Date.Day })
                                 .Select(group => new ScheduleByDateInRoundViewModel
                                 {
                                     GameDate = new System.DateTime(group.Key.year, group.Key.month, group.Key.day),
                                     GameResults = group.ToList()
                                 })
                                 .GroupBy(item => item.GameResults.First().Round)
                                 .Select(item => new ScheduleByRoundViewModel
                                 {
                                     Round = item.Key,
                                     ScheduleByDate = item.ToList()
                                 }).
                                 ToList();

            return result;
        }
    }
}
