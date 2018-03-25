namespace VolleyManagement.UI.Areas.WebApi.Controllers
{
    using Contracts;
    using Domain.TournamentsAggregate;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Http;
    using ViewModels.GameReports;
    using ViewModels.Games;
    using ViewModels.Tournaments;
    using WebAPI.ViewModels.GameReports;
    using WebAPI.ViewModels.Schedule;

#pragma warning disable S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)
    /// <summary>
    /// The tournaments controller.
    /// </summary>
    public class TournamentsController : ApiController
#pragma warning restore S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)
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
        public IList<DivisionStandingsViewModel> GetTournamentStandings(int id)
        {
            var result = new List<DivisionStandingsViewModel>();
            var entries = _gameReportService.GetStandings(id);
            foreach (var entry in entries.Divisions)
            {
                var standings = new DivisionStandingsViewModel {
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
        public IList<PivotStandingsViewModel> GetTournamentPivotStandings(int id)
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
            var games = _gameService.GetTournamentGames(tournamentId)
                                    .Where(g => g.GameDate.HasValue)
                                    .Select(GameViewModel.Map);
            var tournament = _tournamentService.GetTournamentScheduleInfo(tournamentId);

            var resultGroupedByWeek = games.GroupBy(GetWeekOfYear)
                .OrderBy(w => w.Key.Year)
                .ThenBy(w => w.Key.Week)
                .Select(w => new Tuple<int, List<GameViewModel>>(w.Key.Week, w.ToList()))
                .ToList();

            var result = new ScheduleViewModel {
                Schedule = resultGroupedByWeek.Select(it =>
                    new WeekViewModel {
                        Days = it.Item2
                            .GroupBy(item => item.Date.DayOfWeek)
                            .Select(element =>
                            new ScheduleDayViewModel {
                                Date = element.Select(d => d.Date).First(),
                                Divisions = element.Select(data =>
                                    new DivisionTitleViewModel {
                                        Id = data.DivisionId,
                                        Name = data.DivisionName,
                                        Rounds = element.Where(g => g.DivisionId == data.DivisionId)
                                                            .Select(item => item.Round)
                                                            .Distinct()
                                                            .OrderBy(i => i)
                                                            .Select(r => GetRoundName(r, tournament))
                                                            .ToList()
                                    }).
                                    Distinct(new DivisionTitleComparer()).ToList(),
                                Games = element.OrderBy(g => g.AwayTeamName == null)
                                               .ThenBy(g => g.Date)
                                               .ToList()
                            }).OrderBy(item => item.Date).ToList()
                    }
                ).ToList()
            };

            if (tournament.Scheme == TournamentSchemeEnum.PlayOff)
            {
                ClearDivisionNames(result);
            }

            return result;
        }

        private static void ClearDivisionNames(ScheduleViewModel result)
        {
            foreach (var resSchedule in result.Schedule)
            {
                foreach (var daysResSchedule in resSchedule.Days)
                {
                    foreach (var divDaysRes in daysResSchedule.Divisions)
                    {
                        divDaysRes.Id = 0;
                        divDaysRes.Name = null;
                    }

                    foreach (var gamesDaysRes in daysResSchedule.Games)
                    {
                        gamesDaysRes.DivisionId = 0;
                        gamesDaysRes.DivisionName = null;
                        gamesDaysRes.GroupId = 0;
                    }
                }
            }
        }

        private static string GetRoundName(int roundNumber, TournamentScheduleDto tournament)
        {
            string result;

            if (tournament.Scheme != TournamentSchemeEnum.PlayOff)
            {
                result = $"Тур {roundNumber}";
            }
            else
            {
                var playoffRounds = new Dictionary<int, string> {
                    [1] = "Финал",
                    [2] = "Полуфинал",
                    [3] = "Четверть-финал",
                    [4] = "Раунд 16",
                    [5] = "Раунд 32",
                    [6] = "Раунд 64",
                    [7] = "Раунд 128"
                };
                var reversedRoundNumber = tournament.Divisions.First().NumberOfRounds - roundNumber + 1;
                if (!playoffRounds.TryGetValue(reversedRoundNumber, out result))
                {
                    result = $"Тур {roundNumber}";
                }
            }

            return result;
        }

        private static (int Year, int Week) GetWeekOfYear(GameViewModel gr)
        {
            return (
                Year: gr.Date.Year,
                Week: CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(gr.Date, CalendarWeekRule.FirstDay, DayOfWeek.Monday));
        }
    }
}
