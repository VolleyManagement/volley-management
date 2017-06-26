namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Contracts;
    using Contracts.Authorization;
    using Contracts.Exceptions;
    using Crosscutting.Contracts.Providers;
    using Domain.RolesAggregate;
    using Domain.TournamentsAggregate;
    using Resources.UI;
    using ViewModels.GameResults;
    using ViewModels.Teams;
    using ViewModels.Tournaments;

    /// <summary>
    /// Defines TournamentsController
    /// </summary>
    public class TournamentsController : Controller
    {
        private const int ANONYM = -1;
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
        /// Holds AuthorizationService instance
        /// </summary>
        private readonly IAuthorizationService _authService;

        /// <summary>
        /// Holds GameService instance
        /// </summary>
        private readonly IGameService _gameService;

        private readonly ITournamentRequestService _requestService;

        private readonly ICurrentUserService _currentUserService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentsController"/> class
        /// </summary>
        /// <param name="tournamentService">The tournament service</param>
        /// <param name="gameService">The game service</param>
        /// <param name="authService">The authorization service</param>
        /// <param name="requestService">The request service</param>
        /// <param name="currentUserService">The current user service</param>
        public TournamentsController(
            ITournamentService tournamentService,
            IGameService gameService,
            IAuthorizationService authService,
            ITournamentRequestService requestService,
            ICurrentUserService currentUserService)
        {
            _tournamentService = tournamentService;
            _gameService = gameService;
            _requestService = requestService;
            _currentUserService = currentUserService;
            _authService = authService;
        }

        /// <summary>
        /// Gets current and upcoming tournaments from TournamentService
        /// </summary>
        /// <returns>View with collection of tournaments</returns>
        public ActionResult Index()
        {
            var tournamentsCollections = new TournamentsCollectionsViewModel
            {
                Authorization = _authService.GetAllowedOperations(AuthOperations.Tournaments.Create)
            };

            var actualTournaments = _tournamentService.GetActual().ToArray();

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
            var tournament = _tournamentService.Get(id);

            if (tournament == null)
            {
                return HttpNotFound();
            }

            var tournamentViewModel = TournamentViewModel.Map(tournament);
            tournamentViewModel.Authorization = _authService.GetAllowedOperations(new List<AuthOperation>
            {
                AuthOperations.Tournaments.Edit,
                AuthOperations.Tournaments.ManageTeams
            });

            return View(tournamentViewModel);
        }

        /// <summary>
        /// Create tournament action (GET)
        /// </summary>
        /// <returns>View to create a tournament</returns>
        public ActionResult Create()
        {
            var now = TimeProvider.Current.DateTimeNow;

            var tournamentViewModel = new TournamentViewModel()
            {
                ApplyingPeriodStart = now.AddDays(DAYS_TO_APPLYING_PERIOD_START),
                ApplyingPeriodEnd = now.AddDays(DAYS_TO_APPLYING_PERIOD_START
                                              + DAYS_FOR_APPLYING_PERIOD),
                GamesStart = now.AddDays(DAYS_TO_APPLYING_PERIOD_START
                                               + DAYS_FOR_APPLYING_PERIOD
                                               + DAYS_FROM_APPLYING_PERIOD_END_TO_GAMES_START),
                GamesEnd = now.AddDays(DAYS_TO_APPLYING_PERIOD_START
                                     + DAYS_FOR_APPLYING_PERIOD
                                     + DAYS_FROM_APPLYING_PERIOD_END_TO_GAMES_START
                                     + DAYS_FOR_GAMES_PERIOD),
                TransferStart = now.AddDays(DAYS_TO_APPLYING_PERIOD_START
                                          + DAYS_FOR_APPLYING_PERIOD
                                          + DAYS_FROM_APPLYING_PERIOD_END_TO_GAMES_START
                                          + DAYS_FROM_GAMES_START_TO_TRANSFER_START),
                TransferEnd = now.AddDays(DAYS_TO_APPLYING_PERIOD_START
                                        + DAYS_FOR_APPLYING_PERIOD
                                        + DAYS_FROM_APPLYING_PERIOD_END_TO_GAMES_START
                                        + DAYS_FROM_GAMES_START_TO_TRANSFER_START
                                        + DAYS_FOR_TRANSFER_PERIOD)
            };

            return View(tournamentViewModel);
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
                if (ModelState.IsValid)
                {
                    var tournament = tournamentViewModel.ToDomain();
                    _tournamentService.Create(tournament);
                    return RedirectToAction("Index");
                }

                return View(tournamentViewModel);
            }
            catch (TournamentValidationException e)
            {
                ModelState.AddModelError(e.ValidationKey, e.Message);
                return View(tournamentViewModel);
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
                if (ModelState.IsValid)
                {
                    var tournament = tournamentViewModel.ToDomain();
                    _tournamentService.Edit(tournament);
                    return RedirectToAction("Index");
                }

                return View(tournamentViewModel);
            }
            catch (TournamentValidationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(tournamentViewModel);
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
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var tournament = _tournamentService.Get(id);

            if (tournament == null)
            {
                return HttpNotFound();
            }

            _tournamentService.Delete(id);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Manage tournament teams action
        /// </summary>
        /// <param name="tournamentId">Tournaments id</param>
        /// <returns>View with list of existing teams and adding team form</returns>
        public ActionResult ManageTournamentTeams(int tournamentId)
        {
            var resultTeams = _tournamentService.GetAllTournamentTeams(tournamentId);
            var teams = new TournamentTeamsListViewModel(resultTeams, tournamentId);
            var referrerViewModel = new TournamentTeamsListReferrerViewModel(teams, HttpContext.Request.RawUrl);
            return View(referrerViewModel);
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
                _tournamentService.AddTeamsToTournament(teams.ToDomain(), teams.TournamentId);
                result = Json(teams, JsonRequestBehavior.AllowGet);
            }
            catch (ArgumentException ex)
            {
                result = Json(new TeamsAddToTournamentViewModel { Message = ex.Message });
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
                _tournamentService.DeleteTeamFromTournament(teamId, tournamentId);
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
            var tournament = _tournamentService.GetTournamentScheduleInfo(tournamentId);

            if (tournament == null)
            {
                ModelState.AddModelError("LoadError", TournamentController.TournamentNotFound);
                return View();
            }

            var scheduleViewModel = new ScheduleViewModel
            {
                TournamentId = tournament.Id,
                TournamentName = tournament.Name,
                TournamentScheme = tournament.Scheme,
                NumberOfRounds = _tournamentService.GetNumberOfRounds(tournament),
                Rounds = _gameService.GetTournamentResults(tournamentId)
                .GroupBy(d => d.Round)
                .ToDictionary(
                     d => d.Key,
                     c => c.OrderBy(t => t.GameNumber).ThenBy(t => t.GameDate)
                    .Select(x => GameResultViewModel.Map(x)).ToList()),
                AllowedOperations = _authService.GetAllowedOperations(new List<AuthOperation>()
                                                                          {
                                                                            AuthOperations.Games.Create,
                                                                            AuthOperations.Games.Edit,
                                                                            AuthOperations.Games.Delete,
                                                                            AuthOperations.Games.SwapRounds,
                                                                            AuthOperations.Games.EditResult
                                                                          })
            };

            if (tournament.Scheme == TournamentSchemeEnum.PlayOff)
            {
                FillRoundNames(scheduleViewModel);
            }

            for (byte i = 0; i < scheduleViewModel.Rounds.Count; i++)
            {
                foreach (var game in scheduleViewModel.Rounds.ElementAt(i).Value)
                {
                    game.AllowedOperations = _authService.GetAllowedOperations(new List<AuthOperation>()
                                                                          {
                                                                            AuthOperations.Games.Edit,
                                                                            AuthOperations.Games.Delete,
                                                                            AuthOperations.Games.EditResult
                                                                          });
                }
            }

            return View(scheduleViewModel);
        }

        /// <summary>
        /// Schedule game action (GET)
        /// </summary>
        /// <param name="tournamentId">Identifier of the tournament.</param>
        /// <returns>View to schedule a game in tournament</returns>
        public ActionResult ScheduleGame(int tournamentId)
        {
            var scheduleGameViewModel = GetGameViewModelFor(tournamentId);
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
                ModelState.AddModelError("ValidationError", e.Message);
            }

            return ScheduleGame(gameViewModel.TournamentId);
        }

        /// <summary>
        /// Edit scheduled game action (GET)
        /// </summary>
        /// <param name="gameId">Identifier of the game.</param>
        /// <returns>View to edit scheduled game in the tournament.</returns>
        public ActionResult EditScheduledGame(int gameId)
        {
            var game = _gameService.Get(gameId);

            if (game == null)
            {
                ModelState.AddModelError("LoadError", TournamentViews.GameNotFoundInTournament);
                return View();
            }

            var gameViewModel = GetGameViewModelFor(game.TournamentId);

            gameViewModel.Id = game.Id;
            gameViewModel.AwayTeamId = game.AwayTeamId;
            gameViewModel.HomeTeamId = game.HomeTeamId;
            gameViewModel.GameNumber = game.GameNumber;
            if (game.GameDate.HasValue)
            {
                gameViewModel.GameDate = game.GameDate.Value;
            }

            gameViewModel.Round = game.Round;

            return View(gameViewModel);
        }

        /// <summary>
        /// Schedule game action (POST)
        /// </summary>
        /// <param name="gameViewModel">Submitted game to be edited</param>
        /// <returns>Appropriate view</returns>
        [HttpPost]
        public ActionResult EditScheduledGame(GameViewModel gameViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _gameService.Edit(gameViewModel.ToDomain());
                    return RedirectToAction("ShowSchedule", new { tournamentId = gameViewModel.TournamentId });
                }
            }
            catch (ArgumentException e)
            {
                ModelState.AddModelError("ValidationError", e.Message);
            }
            catch (MissingEntityException e)
            {
                ModelState.AddModelError("LoadError", e.Message);
                return View();
            }

            return EditScheduledGame(gameViewModel.Id);
        }

        /// <summary>
        /// Swap rounds action.
        /// </summary>
        /// <param name="tournamentId">Identifier of the tournament.</param>
        /// <param name="firstRoundNumber">Identifier of the first round number.</param>
        /// <param name="secondRoundNumber">Identifier of the second round number.</param>
        /// <returns>Redirect to schedule page.</returns>
        public ActionResult SwapRounds(int tournamentId, byte firstRoundNumber, byte secondRoundNumber)
        {
            var tournament = _tournamentService.GetTournamentScheduleInfo(tournamentId);

            if (tournament == null)
            {
                ModelState.AddModelError("LoadError", TournamentController.TournamentNotFound);
                return View();
            }

            try
            {
                _gameService.SwapRounds(tournamentId, firstRoundNumber, secondRoundNumber);
                return RedirectToAction("ShowSchedule", new { tournamentId = tournamentId });
            }
            catch (MissingEntityException ex)
            {
                ModelState.AddModelError("LoadError", ex.Message);
                return View();
            }
        }

        /// <summary>
        /// Apply for tournament
        /// </summary>
        /// <param name="tournamentId">Tournament id</param>
        /// <returns>TournamentApply view</returns>
        public ActionResult ApplyForTournament(int tournamentId)
        {
            var tournament = _tournamentService.Get(tournamentId);

            if (tournament == null)
            {
                return HttpNotFound();
            }

            var noTournamentTeams = _tournamentService.GetAllNoTournamentTeams(tournamentId);
            var tournamentApplyViewModel = new TournamentApplyViewModel
            {
                Id = tournamentId,
                TournamentTitle = tournament.Name,
                Teams = noTournamentTeams.Select(t => TeamNameViewModel.Map(t)),
            };
            return View(tournamentApplyViewModel);
        }

        /// <summary>
        /// Apply for tournament
        /// </summary>
        /// <param name="tournamentId">Tournament id</param>
        /// /// <param name="teamId">Team id</param>
        /// <returns>TournamentApply view</returns>
        [HttpPost]
        public JsonResult ApplyForTournament(int tournamentId, int teamId)
        {
            JsonResult result = null;
            try
            {
                int userId = _currentUserService.GetCurrentUserId();
                if (userId == ANONYM)
                {
                    result = Json(ViewModelResources.NoRights);
                }
                else
                {
                    _requestService.Create(userId, tournamentId, teamId);
                    result = Json(ViewModelResources.SuccessRequest);
                }
            }
            catch (ArgumentException ex)
            {
                result = Json(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Returns available list of all teams.
        /// </summary>
        /// <param name="tournamentId">Identifier of the tournament.</param>
        /// <returns>Json list of teams</returns>
        public JsonResult GetAllAvailableTeams(int tournamentId)
        {
            var nonTournamentTeamsList = _tournamentService.GetAllNoTournamentTeams(tournamentId);
            var teams = nonTournamentTeamsList.Select(TeamNameViewModel.Map);
            return Json(teams, JsonRequestBehavior.AllowGet);
        }

        #region Private

        /// <summary>
        /// Gets info about the tournament with a specified identifier.
        /// </summary>
        /// <param name="id">Identifier of the tournament.</param>
        /// <returns>View with the TournamentViewModel.</returns>
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

        /// <summary>
        /// Gets GameViewModel with loaded teams list and rounds.
        /// </summary>
        /// <param name="tournamentId">Id of the tournament to create GameViewModel for.</param>
        /// <returns>GameViewModel with loaded teams list and rounds.</returns>
        private GameViewModel GetGameViewModelFor(int tournamentId)
        {
            var tournament = _tournamentService.GetTournamentScheduleInfo(tournamentId);

            if (tournament == null)
            {
                ModelState.AddModelError("LoadError", TournamentController.TournamentNotFound);
                return null;
            }

            var tournamentTeams = _tournamentService.GetAllTournamentTeams(tournamentId);
            var roundsNumber = _tournamentService.GetNumberOfRounds(tournament);
            if (roundsNumber <= 0)
            {
                ModelState.AddModelError("LoadError", TournamentController.SchedulingError);
                return null;
            }

            return new GameViewModel
            {
                TournamentId = tournamentId,
                TournamentScheme = tournament.Scheme,
                GameDate = tournament.StartDate,
                Rounds = new SelectList(Enumerable.Range(MIN_ROUND_NUMBER, roundsNumber)),
                Teams = new SelectList(tournamentTeams, "Id", "Name")
            };
        }

        /// <summary>
        /// Fills round names for playoff scheme
        /// </summary>
        /// <param name="scheduleViewModel">View model which contains round names</param>
        private void FillRoundNames(ScheduleViewModel scheduleViewModel)
        {
            var roundNames = new string[scheduleViewModel.NumberOfRounds];

            for (byte i = 1; i <= scheduleViewModel.NumberOfRounds; i++)
            {
                var roundName = string.Empty;
                switch (i)
                {
                    case 1:
                        roundName = TournamentController.FinalRoundName;
                        break;
                    case 2:
                        roundName = TournamentController.SemifinalRoundName;
                        break;
                    case 3:
                        roundName = TournamentController.QuarterFinalRoundName;
                        break;
                    default:
                        roundName = string.Format(TournamentController.RoundNumber, Math.Pow(2, i));
                        break;
                }

                roundNames[roundNames.Length - i] = roundName;
            }

            scheduleViewModel.RoundNames = roundNames;
        }
        #endregion
    }
}