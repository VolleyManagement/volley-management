namespace VolleyManagement.UI.Areas.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
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
        public List<List<StandingsEntryViewModel>> GetTournamentStandings(int id)
        {
            var result = new List<List<StandingsEntryViewModel>>();
            var entries = _gameReportService.GetStandings(id);
            foreach (var entry in entries)
            {
                var standings = entry.Select(t => StandingsEntryViewModel.Map(t))
                                     .ToList();
                StandingsEntryViewModel.SetPositions(standings);
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
        public IEnumerable<PivotStandingsViewModel> GetTournamentPivotStandings(int id)
        {
            var pivotData = _gameReportService.GetPivotStandings(id);
            return pivotData.Select(item => new PivotStandingsViewModel(item));
        }

        /// <summary>
        /// Gets games by tournament id.
        /// </summary>
        /// <param name="tournamentId">Id of tournament.</param>
        /// <returns>Information about games with specified tournament id.</returns>
        [Route("api/Tournament/{tournamentId}/Schedule")]
        public ScheduleViewModel GetSchedule(int tournamentId)
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

            var rezultGroupedByWeek = gamesViewModel.GroupBy(gr => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                gr.Date, CalendarWeekRule.FirstDay, DayOfWeek.Monday))
                .Select(w => new { weekNumber = w.Key, games = w.ToList() }) // TODO USE TUPLE HERE
                .ToList();

            var result = new ScheduleViewModel()
            {
                Schedule = rezultGroupedByWeek.Select(it =>
                    new Week()
                    {
                        Days = it.games.GroupBy(item => item.Date).Select(element =>
                            new ScheduleDay()
                            {
                                Date = element.Key,
                                Divisions = element.ToList().Select(data =>
                                    new DivisionTitle()
                                    {
                                        Id = data.DivisionId,
                                        Name = data.DivisionName,
                                        Rounds = element.Where(g => g.DivisionId == data.DivisionId).Select(item => item.Round).ToList()
                                    }).ToList(),
                                Games = element.ToList()
                            }).ToList()
                    }
                ).ToList()
            };

            return result;
        }
    }
}
