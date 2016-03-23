namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.GameResults;

    /// <summary>
    /// Represents a controller that contains game results actions.
    /// </summary>
    public class GameResultsController : Controller
    {
        private readonly IGameResultService _gameResultService;
        private readonly ITeamService _teamService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameResultsController"/> class.
        /// </summary>
        /// <param name="gameResultService">Instance of a class which implements <see cref="IGameResultService"/>.</param>
        /// <param name="teamService">Instance of a class which implements <see cref="ITeamService"/>.</param>
        public GameResultsController(IGameResultService gameResultService, ITeamService teamService)
        {
            _gameResultService = gameResultService;
            _teamService = teamService;
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
                GameResults = _gameResultService.GetTournamentResults(tournamentId).Select(gr => GameResultViewModel.Map(gr)).ToList()
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
                TeamsList = GetTeamsList()
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
                _gameResultService.Create(gameResult);
                return RedirectToAction("Details", "Tournaments", new { id = gameResultViewModel.TournamentId });
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("ValidationMessage", ex.Message);
                gameResultViewModel.TeamsList = GetTeamsList();
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
            var gameResult = _gameResultService.Get(id);

            if (gameResult == null)
            {
                return HttpNotFound();
            }

            var gameResultsViewModel = GameResultViewModel.Map(gameResult);
            gameResultsViewModel.TeamsList = GetTeamsList();
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
                    _gameResultService.Edit(gameResult);
                    return RedirectToAction("Details", "Tournaments", new { id = gameResultViewModel.TournamentId });
                }
            }
            catch (MissingEntityException)
            {
                ModelState.AddModelError(string.Empty, App_GlobalResources.GameResultsController.GameResultWasDeleted);
            }

            return View(gameResultViewModel);
        }

        /// <summary>
        /// Deletes an existing game result by its identifier.
        /// </summary>
        /// <param name="id">Identifier of the game result.</param>
        [HttpPost]
        public void Delete(int id)
        {
            var gameResult = _gameResultService.Get(id);

            if (gameResult == null)
            {
                throw new MissingEntityException(App_GlobalResources.GameResultsController.GameResultNotFound);
            }

            _gameResultService.Delete(id);
        }

        private List<SelectListItem> GetTeamsList()
        {
            return _teamService.Get().Select(team => new SelectListItem { Value = team.Id.ToString(), Text = team.Name }).ToList();
        }
    }
}
