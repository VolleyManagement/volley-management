namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Contracts;
    using Contracts.Authorization;
    using Contracts.Exceptions;
    using Domain.RolesAggregate;
    using ViewModels.GameResults;

    /// <summary>
    /// Represents a controller that contains game results actions.
    /// </summary>
    public class GameResultsController : Controller
    {
        private readonly IGameService _gameService;
        private readonly ITeamService _teamService;
        private readonly IAuthorizationService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameResultsController"/> class.
        /// </summary>
        /// <param name="gameResultService">Instance of a class which implements <see cref="IGameResultService"/>.</param>
        /// <param name="teamService">Instance of a class which implements <see cref="ITeamService"/>.</param>
        /// <param name="authService">The authorization service</param>
        public GameResultsController(IGameService gameResultService, ITeamService teamService, IAuthorizationService authService)
        {
            _gameService = gameResultService;
            _teamService = teamService;
            _authService = authService;
        }

        /// <summary>
        /// Renders a view with game results of the specified tournament.
        /// </summary>
        /// <param name="tournamentId">Identifier of the tournament.</param>
        /// <param name="tournamentName">Name of the tournament.</param>
        /// <returns>View with game results of the specified tournament.</returns>
        public ActionResult TournamentResults(int tournamentId, string tournamentName)
        {
            var tournamentResults = new TournamentResultsViewModel
            {
                Id = tournamentId,
                Name = tournamentName,
                GameResults = _gameService.GetTournamentResults(tournamentId).Select(gr => GameResultViewModel.Map(gr)).ToList(),
                Authorization = _authService.GetAllowedOperations(new List<AuthOperation>
                {
                AuthOperations.Games.EditResult,
                AuthOperations.Games.Delete
                })
            };

            return View(tournamentResults);
        }

        /// <summary>
        /// Renders a view with empty fields of game result for create operation.
        /// </summary>
        /// <param name="tournamentId">Identifier of the tournament where game result belongs.</param>
        /// <returns>View with empty fields of game result.</returns>
        public ActionResult Create(int tournamentId)
        {
            var gameResultViewModel = new GameResultViewModel
            {
                TournamentId = tournamentId,
            };

            return View(gameResultViewModel);
        }

        /// <summary>
        /// Creates a new game result if fields data is valid and redirects to the specified page.
        /// </summary>
        /// <param name="gameResultViewModel">View model of game result.</param>
        /// <returns>View with empty fields of game result if fields data is not valid;
        /// otherwise, view of the specified page.</returns>
        [HttpPost]
        public ActionResult Create(GameResultViewModel gameResultViewModel)
        {
            var gameResult = gameResultViewModel.ToDomain();

            try
            {
                _gameService.Create(gameResult);
                return RedirectToAction("Details", "Tournaments", new { id = gameResultViewModel.TournamentId });
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("ValidationMessage", ex.Message);
                return View(gameResultViewModel);
            }
        }

        /// <summary>
        /// Renders a view with filled in fields of game result for edit operation.
        /// </summary>
        /// <param name="id">Identifier of the game result.</param>
        /// <returns>View with filled in fields of game result.</returns>
        public ActionResult Edit(int id)
        {
            var gameResult = _gameService.Get(id);

            if (gameResult == null)
            {
                return HttpNotFound();
            }

            var gameResultsViewModel = GameResultViewModel.Map(gameResult);
            return View(gameResultsViewModel);
        }

        /// <summary>
        /// Edit an existing game result if fields data is valid and redirects to the specified page.
        /// </summary>
        /// <param name="gameResultViewModel">View model of game result.</param>
        /// <returns>View with filled in fields of game result if fields data is not valid;
        /// otherwise, view of the specified page.</returns>
        [HttpPost]
        public ActionResult Edit(GameResultViewModel gameResultViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var gameResult = gameResultViewModel.ToDomain();
                    _gameService.EditGameResult(gameResult);
                    return RedirectToAction("ShowSchedule", "Tournaments", new { tournamentId = gameResultViewModel.TournamentId });
                }
            }
            catch (MissingEntityException)
            {
                ModelState.AddModelError(string.Empty, Resources.UI.GameResultsController.GameResultWasDeleted);
            }
            catch (ArgumentException)
            {
                ModelState.AddModelError(string.Empty, Resources.UI.GameResultsController.GameResultNotEdited);
            }

            return View(gameResultViewModel);
        }

        /// <summary>
        /// Deletes an existing game result by its identifier.
        /// </summary>
        /// <param name="id">Identifier of the game result.</param>
        /// <returns>Result message</returns>
        public JsonResult Delete(int id)
        {
            try
            {
                _gameService.Delete(id);
            }
            catch (ArgumentNullException ex)
            {
                return Json(new GameDeleteResultViewModel { Message = ex.Message, HasDeleted = false });
            }
            catch (ArgumentException ex)
            {
                return Json(new GameDeleteResultViewModel { Message = ex.Message, HasDeleted = false });
            }

            return Json(new GameDeleteResultViewModel
            {
                Message = Resources.UI.GameResultsController.GameWasDeletedSuccessfully,
                HasDeleted = true
            });
        }
    }
}
