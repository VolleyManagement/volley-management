namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using VolleyManagement.Contracts;
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
                Teams = _teamService.Get().Select(team => new SelectListItem { Value = team.Id.ToString(), Text = team.Name }).ToList()
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
                ModelState.AddModelError("ValidationError", ex.Message);
                gameResultViewModel.Teams = _teamService.Get()
                    .Select(team => new SelectListItem { Value = team.Id.ToString(), Text = team.Name })
                    .ToList();

                return View(gameResultViewModel);
            }
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
    }
}