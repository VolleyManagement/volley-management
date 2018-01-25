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
                    DivisionName = entry.DivisionName,
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
        public ScheduleViewModel GetSchedule(int tournamentId)
        {
            List<GameViewModel> gamesViewModel = _gameService.GetTournamentGames(tournamentId)
                                                        .Select(t => GameViewModel.Map(t)).ToList();
            foreach (var item in gamesViewModel)
            {
                if (item.Result.TotalScore.IsEmpty)
                {
                    item.Result = null;
                }
            }

            var resultGroupedByWeek = gamesViewModel.GroupBy(gr => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                gr.Date, CalendarWeekRule.FirstDay, DayOfWeek.Monday))
                .OrderBy(w => w.Key)
                .Select(w => new Tuple<int, List<GameViewModel>>(w.Key, w.ToList()))
                .ToList();

            var result = new ScheduleViewModel()
            {
                Schedule = resultGroupedByWeek.Select(it =>
                    new WeekViewModel()
                    {
                        Days = it.Item2.
                        GroupBy(item => item.Date.DayOfWeek).
                        Select(element =>
                            new ScheduleDayViewModel()
                            {
                                Date = element.ToList().Select(d => d.Date).First(),
                                Divisions = element.ToList().Select(data =>
                                    new DivisionTitleViewModel()
                                    {
                                        Id = data.DivisionId,
                                        Name = data.DivisionName,
                                        Rounds = element.Where(g => g.DivisionId == data.DivisionId)
                                                            .Select(item => item.Round)
                                                            .Distinct()
                                                            .OrderBy(i => i)
                                                            .ToList()
                                    }).
                                    Distinct(new DivisionTitleComparer()).
                                    ToList(),
                                Games = element.ToList()
                            }).
                        OrderBy(item => item.Date).
                        ToList()
                    }
                ).ToList()
            };

            return result;
        }
    }
}
