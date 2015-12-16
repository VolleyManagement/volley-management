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
    /// Defines GameResultsController
    /// </summary>
    public class GameResultsController : Controller
    {
        /// <summary>
        /// Holds TournamentService instance
        /// </summary>
        private readonly ITeamService _teamService;

        /// <summary>
        /// Holds TournamentService instance
        /// </summary>
        private readonly IGameResultService _gameResultsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameResultsController"/> class
        /// </summary>
        /// <param name="teamService">The team service</param>
        /// <param name="gameResultsService">The game result service</param>
        public GameResultsController(ITeamService teamService, IGameResultService gameResultsService)
        {
            _teamService = teamService;
            _gameResultsService = gameResultsService;
        }

        /// <summary>
        /// Details method.
        /// </summary>
        /// <param name="id">Id of game result</param>
        /// <returns>Details view</returns>
        public ActionResult Details(int id)
        {
            var gameResult = GameResultViewModel.Map(_gameResultsService.Get(id));
            return View(gameResult);
        }

        /// <summary>
        /// Create GET method.
        /// </summary>
        /// <param name="id">Id of tournament</param>
        /// <returns>Create view.</returns>
        public ActionResult Create(int id)
        {
            GameResultViewModel gameResultViewModel = new GameResultViewModel
            {
                TournamentId = id,
                TournamentTeams = GetTeams()
            };

            return View(gameResultViewModel);
        }

        /// <summary>
        /// Create POST method
        /// </summary>
        /// <param name="gameResultViewModel">Game result view model</param>
        /// <returns>View of tournament details</returns>
        [HttpPost]
        public ActionResult Create(GameResultViewModel gameResultViewModel)
        {
            var gameResult = gameResultViewModel.ToDomain();

            try
            {
                _gameResultsService.Create(gameResult);
                return RedirectToAction("Details", "Tournaments", new { id = gameResult.TournamentId });
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("ValidationMessage", ex.Message);
                gameResultViewModel.TournamentTeams = GetTeams();
                return View(gameResultViewModel);
            }
        }

        /// <summary>
        /// Edit game results action (GET)
        /// </summary>
        /// <param name="id">Game results id</param>
        /// <returns>View to edit specific game results</returns>
        public ActionResult Edit(int id)
        {
            var gameResult = _gameResultsService.Get(id);

            if (gameResult == null)
            {
                return HttpNotFound();
            }

            var gameResultsViewModel = GameResultViewModel.Map(gameResult);
            gameResultsViewModel.TournamentTeams = GetTeams();
            return View(gameResultsViewModel);
        }

        /// <summary>
        /// Edit game results action (POST)
        /// </summary>
        /// <param name="gameResultViewModel">Game results after editing</param>
        /// <returns>Index view if game results was valid, otherwise - edit view</returns>
        [HttpPost]
        public ActionResult Edit(GameResultViewModel gameResultViewModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var gameResult = gameResultViewModel.ToDomain();
                    _gameResultsService.Edit(gameResult);
                    return this.RedirectToAction("TournamentResults", new { id = gameResult.TournamentId });
                }
            }
            catch (MissingEntityException)
            {
                this.ModelState.AddModelError(
                                string.Empty,
                                App_GlobalResources.GameResultsController.GameResultWasDeleted);
            }

            return this.View(gameResultViewModel);
        }

        /// <summary>
        /// Delete game result action (POST)
        /// </summary>
        /// <param name="id">Game result id</param>
        [HttpPost]
        public void Delete(int id)
        {
            var gameResult = _gameResultsService.Get(id);

            if (gameResult == null)
            {
                throw new MissingEntityException(App_GlobalResources.GameResultsController.GameResultNotFound);
            }

            _gameResultsService.Delete(id);
        }

        /// <summary>
        /// Represents all game results of specified tournament
        /// </summary>
        /// <param name="id">Id of tournament</param>
        /// <returns>View represents results list</returns>
        public ActionResult TournamentResults(int id)
        {
            var gameResults = _gameResultsService.Get()
                .Where(gr => gr.TournamentId == id)
                .Select(gr => GameResultViewModel.Map(gr))
                .ToList();

            return View(gameResults);
        }

        private List<SelectListItem> GetTeams()
        {
            return _teamService.Get().Select(team => new SelectListItem() { Value = team.Id.ToString(), Text = team.Name }).ToList();
        }
    }
}