namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using App_GlobalResources;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Domain;
    using VolleyManagement.Domain.GamesAggregate;
    using VolleyManagement.Domain.TeamsAggregate;
    using VolleyManagement.Domain.TournamentsAggregate;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.GameResults;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Teams;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments;

    /// <summary>
    /// Defines TournamentsController
    /// </summary>
    public class TournamentsController : Controller
    {
        private const int DAYS_TO_APPLYING_PERIOD_START = 14;
        private const int DAYS_FOR_APPLYING_PERIOD = 14;
        private const int DAYS_FROM_APPLYING_PERIOD_END_TO_GAMES_START = 7;
        private const int DAYS_FOR_GAMES_PERIOD = 120;
        private const int DAYS_FROM_GAMES_START_TO_TRANSFER_START = 1;
        private const int DAYS_FOR_TRANSFER_PERIOD = 21;
        private const int MIN_ROUND_NUMBER = 1;

        /// <summary>
        /// Holds TournamentService instance
        /// </summary>
        private readonly ITournamentService _tournamentService;

        /// <summary>
        /// Holds GameService instance
        /// </summary>
        private readonly IGameService _gameService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentsController"/> class
        /// </summary>
        /// <param name="tournamentService">The tournament service</param>
        /// <param name="gameService">The game service</param>
        public TournamentsController(
            ITournamentService tournamentService,
            IGameService gameService)
        {
            this._tournamentService = tournamentService;
            this._gameService = gameService;
        }

        /// <summary>
        /// Gets current and upcoming tournaments from TournamentService
        /// </summary>
        /// <returns>View with collection of tournaments</returns>
        public ActionResult Index()
        {
            var tournamentsCollections = new TournamentsCollectionsViewModel();

            var actualTournaments = this._tournamentService.GetActual().ToArray();

            tournamentsCollections.CurrentTournaments = actualTournaments
                .Where(tr => tr.State == TournamentStateEnum.Current);

            tournamentsCollections.UpcomingTournaments = actualTournaments
                .Where(tr => tr.State == TournamentStateEnum.Upcoming);

            return View(tournamentsCollections);
        }

        /// <summary>
        /// Get finished tournaments
        /// </summary>
        /// <returns>Json result</returns>
        public JsonResult GetFinished()
        {
            var result = _tournamentService.GetFinished().ToList()
                 .Select(t => TournamentViewModel.Map(t));
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets details for specific tournament
        /// </summary>
        /// <param name="id">Tournament id</param>
        /// <returns>View with specific tournament</returns>
        public ActionResult Details(int id)
        {
            return GetTournamentView(id);
        }

        /// <summary>
        /// Create tournament action (GET)
        /// </summary>
        /// <returns>View to create a tournament</returns>
        public ActionResult Create()
        {
            var tournamentViewModel = new TournamentViewModel()
            {
                ApplyingPeriodStart = DateTime.Now.AddDays(DAYS_TO_APPLYING_PERIOD_START),
                ApplyingPeriodEnd = DateTime.Now.AddDays(DAYS_TO_APPLYING_PERIOD_START + DAYS_FOR_APPLYING_PERIOD),
                GamesStart = DateTime.Now.AddDays(DAYS_TO_APPLYING_PERIOD_START + DAYS_FOR_APPLYING_PERIOD
                + DAYS_FROM_APPLYING_PERIOD_END_TO_GAMES_START),
                GamesEnd = DateTime.Now.AddDays(DAYS_TO_APPLYING_PERIOD_START + DAYS_FOR_APPLYING_PERIOD
                + DAYS_FROM_APPLYING_PERIOD_END_TO_GAMES_START + DAYS_FOR_GAMES_PERIOD),
                TransferStart = DateTime.Now.AddDays(DAYS_TO_APPLYING_PERIOD_START + DAYS_FOR_APPLYING_PERIOD
                + DAYS_FROM_APPLYING_PERIOD_END_TO_GAMES_START + DAYS_FROM_GAMES_START_TO_TRANSFER_START),
                TransferEnd = DateTime.Now.AddDays(DAYS_TO_APPLYING_PERIOD_START + DAYS_FOR_APPLYING_PERIOD
                + DAYS_FROM_APPLYING_PERIOD_END_TO_GAMES_START + DAYS_FROM_GAMES_START_TO_TRANSFER_START + DAYS_FOR_TRANSFER_PERIOD)
            };

            return this.View(tournamentViewModel);
        }

        /// <summary>
        /// Create tournament action (POST)
        /// </summary>
        /// <param name="tournamentViewModel">Tournament, which the user wants to create</param>
        /// <returns>Index view if tournament was valid, else - create view</returns>
        [HttpPost]
        public ActionResult Create(TournamentViewModel tournamentViewModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var tournament = tournamentViewModel.ToDomain();
                    this._tournamentService.Create(tournament);
                    return this.RedirectToAction("Index");
                }

                return this.View(tournamentViewModel);
            }
            catch (TournamentValidationException e)
            {
                this.ModelState.AddModelError(e.ValidationKey, e.Message);
                return this.View(tournamentViewModel);
            }
        }

        /// <summary>
        /// Edit tournament action (GET)
        /// </summary>
        /// <param name="id">Tournament id</param>
        /// <returns>View to edit specific tournament</returns>
        public ActionResult Edit(int id)
        {
            return GetTournamentView(id);
        }

        /// <summary>
        /// Edit tournament action (POST)
        /// </summary>
        /// <param name="tournamentViewModel">Tournament after editing</param>
        /// <returns>Index view if tournament was valid, else - edit view</returns>
        [HttpPost]
        public ActionResult Edit(TournamentViewModel tournamentViewModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var tournament = tournamentViewModel.ToDomain();
                    this._tournamentService.Edit(tournament);
                    return this.RedirectToAction("Index");
                }

                return this.View(tournamentViewModel);
            }
            catch (TournamentValidationException ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return this.View(tournamentViewModel);
            }
        }

        /// <summary>
        /// Delete tournament action (GET)
        /// </summary>
        /// <param name="id">Tournament id</param>
        /// <returns>View to delete specific tournament</returns>
        public ActionResult Delete(int id)
        {
            return GetTournamentView(id);
        }

        /// <summary>
        /// Delete tournament action (POST)
        /// </summary>
        /// <param name="id">Tournament id</param>
        /// <returns>Index view</returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var tournament = _tournamentService.Get(id);

            if (tournament == null)
            {
                return HttpNotFound();
            }

            this._tournamentService.Delete(id);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Manage tournament teams action
        /// </summary>
        /// <param name="tournamentId">Tournaments id</param>
        /// <returns>View with list of existing teams and adding team form</returns>
        public ActionResult ManageTournamentTeams(int tournamentId)
        {
            var resultTeams = this._tournamentService.GetAllTournamentTeams(tournamentId);
            return View(new TournamentTeamsListViewModel(resultTeams, tournamentId));
        }

        /// <summary>
        /// Adds list of teams to tournament
        /// </summary>
        /// <param name="teams">Object with list of teams and tournament id</param>
        /// <returns>json result with operation data</returns>
        [HttpPost]
        public JsonResult AddTeamsToTournament(TournamentTeamsListViewModel teams)
        {
            JsonResult result = null;
            try
            {
                this._tournamentService.AddTeamsToTournament(teams.ToDomain(), teams.TournamentId);
                result = this.Json(teams, JsonRequestBehavior.AllowGet);
            }
            catch (ArgumentException ex)
            {
                result = this.Json(new TeamsAddToTournamentViewModel { Message = ex.Message });
            }

            return result;
        }

        /// <summary>
        /// Deletes team from tournament
        /// </summary>
        /// <param name="teamId">team to delete</param>
        /// <param name="tournamentId">tournament for team deleting</param>
        /// <returns>json result with operation data</returns>
        [HttpPost]
        public JsonResult DeleteTeamFromTournament(int teamId, int tournamentId)
        {
            JsonResult result = null;
            try
            {
                this._tournamentService.DeleteTeamFromTournament(teamId, tournamentId);
                result = Json(new TeamDeleteFromTournamentViewModel
                {
                    Message = ViewModelResources.TeamWasDeletedSuccessfully,
                    HasDeleted = true
                });
            }
            catch (MissingEntityException ex)
            {
                result = Json(new TeamDeleteFromTournamentViewModel
                {
                    Message = ex.Message,
                    HasDeleted = false
                });
            }

            return result;
        }

        /// <summary>
        /// Gets the view for view model of the schedule with specified identifier.
        /// </summary>
        /// <param name="tournamentId">Identifier of the tournament.</param>
        /// <returns>View for view model of the schedule with specified identifier.</returns>    
        public ActionResult ShowSchedule(int tournamentId)
        {
            var tournament = _tournamentService.Get(tournamentId);

            if (tournament == null)
            {
                this.ModelState.AddModelError("LoadError", TournamentController.TournamentNotFound);
                return View();
            }

            var tournamentTeamsCount = _tournamentService.GetAllTournamentTeams(tournamentId).Count;
            var scheduleViewModel = new ScheduleViewModel
            {
                TournamentId = tournament.Id,
                TournamentName = tournament.Name,
                CountRound = _tournamentService.NumberOfRounds(tournament, tournamentTeamsCount),
                Rounds = _gameService.GetTournamentResults(tournamentId)
                                     .GroupBy(d => d.Round)
                                     .ToDictionary(d => d.Key, c => c.OrderBy(t => t.GameDate)
                                     .Select(x => GameResultViewModel.Map(x)).ToList())
            };

            return View(scheduleViewModel);
        }

        /// <summary>
        /// Schedule game action (GET)
        /// </summary>
        /// <param name="tournamentId">Identifier of the tournament.</param>
        /// <returns>View to schedule a game in tournament</returns>
        public ActionResult ScheduleGame(int tournamentId)
        {
            var tournament = _tournamentService.Get(tournamentId);

            if (tournament == null)
            {
                this.ModelState.AddModelError("LoadError", TournamentController.TournamentNotFound);
                return View();
            }

            var tournamentTeams = _tournamentService.GetAllTournamentTeams(tournamentId);
            var roundsNumber = _tournamentService.NumberOfRounds(tournament, tournamentTeams.Count);
            if (roundsNumber <= 0)
            {
                this.ModelState.AddModelError("LoadError", TournamentController.SchedulingError);
                return View();
            }

            var scheduleGameViewModel = new GameViewModel
            {
                TournamentId = tournamentId,
                Rounds = new SelectList(Enumerable.Range(MIN_ROUND_NUMBER, roundsNumber)),
                Teams = new SelectList(tournamentTeams, "Id", "Name")
            };

            return View(scheduleGameViewModel);
        }

        /// <summary>
        /// Schedule game action (POST)
        /// </summary>
        /// <param name="gameViewModel">Submitted game to be scheduled</param>
        /// <param name="redirectToSchedule">Defines whether it's necessary to redirect to Schedule action, after game was created</param>
        /// <returns>Appropriate view</returns>
        [HttpPost]
        public ActionResult ScheduleGame(GameViewModel gameViewModel, bool redirectToSchedule)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _gameService.Create(gameViewModel.ToDomain());

                    if (redirectToSchedule)
                    {
                        return RedirectToAction("ShowSchedule", new { tournamentId = gameViewModel.TournamentId });                       
                }
                    else
                    {
                        ModelState.Clear();
            }
                }
            }
            catch (ArgumentException e)
        {
                this.ModelState.AddModelError("ValidationError", e.Message);
            }

            return ScheduleGame(gameViewModel.TournamentId);
        }

        /// <summary>
        /// Gets the view for view model of the tournament with specified identifier.
        /// </summary>
        /// <param name="id">Identifier of the tournament.</param>
        /// <returns>View for view model of the tournament with specified identifier.</returns>
        private ActionResult GetTournamentView(int id)
        {
            var tournament = _tournamentService.Get(id);

            if (tournament == null)
            {
                return HttpNotFound();
            }

            var tournamentViewModel = TournamentViewModel.Map(tournament);
            return View(tournamentViewModel);
        }
    }
}
