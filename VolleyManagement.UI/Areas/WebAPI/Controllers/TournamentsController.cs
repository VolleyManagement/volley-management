namespace VolleyManagement.UI.Areas.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Contracts;
    using ViewModels.GameReports;
    using ViewModels.Games;
    using ViewModels.Tournaments;
    using VolleyManagement.UI.Areas.WebAPI.ViewModels.Schedule;

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
        public IEnumerable<StandingsEntryViewModel> GetTournamentStandings(int id)
        {
            var entries = _gameReportService.GetStandings(id)
                .Select(t => StandingsEntryViewModel.Map(t))
                .ToList();
            return StandingsEntryViewModel.SetPositions(entries);
        }

        /// <summary>
        /// Gets pivot standings by tournament id
        /// </summary>
        /// <param name="id">Id of tournament</param>
        /// <returns>Pivot standings entries of the tournament with specified id</returns>
        [Route("api/v1/Tournaments/{id}/PivotStandings")]
        public PivotStandingsViewModel GetTournamentPivotStandings(int id)
        {
            return new PivotStandingsViewModel(_gameReportService.GetPivotStandings(id));
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

            return gamesViewModel.GroupBy(gr => gr.Round)
                                 .Select(group => new ScheduleByRoundViewModel()
                                 {
                                     Round = group.Key,
                                     GameResults = group.ToList()
                                 })
                                 .ToList();
        }
    }
}
