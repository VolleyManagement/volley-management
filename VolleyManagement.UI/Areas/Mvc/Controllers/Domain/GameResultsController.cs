namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Contracts;
    using Domain.GameResultsAggregate;
    using ViewModels.GameResults;
    using ViewModels.Teams;

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
            this._teamService = teamService;
            this._gameResultsService = gameResultsService;
        }

        /// <summary>
        /// Details method.
        /// </summary>
        /// <param name="id">Id of game result</param>
        /// <returns>Details view</returns>
        public ActionResult Details(int id)
        {
            var gameResult = GameResultViewModel.Map(_gameResultsService.Get(id));
            gameResult.HomeTeamName = _teamService.Get(gameResult.HomeTeamId).Name;
            gameResult.AwayTeamName = _teamService.Get(gameResult.AwayTeamId).Name;
            return View(gameResult);
        }

        /// <summary>
        /// Create GET method.
        /// </summary>
        /// <param name="Id">Id of tournament</param>
        /// <returns>Create view.</returns>
        public ActionResult Create(int Id)
        {
            ViewBag.Teams = _teamService.Get().Select(team => new SelectListItem() { Value = team.Id.ToString(), Text = team.Name }).ToList();
            GameResultViewModel gameResultViewModel = new GameResultViewModel() { TournamentId = Id };
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
                var teams = _teamService.Get().Select(team => new SelectListItem() { Value = team.Id.ToString(), Text = team.Name }).ToList();
                ViewBag.Teams = teams;
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
            var gameResults = _gameResultsService.Get().Where(
                                                            gr =>
                                                            gr.TournamentId == id)
                                                            .Select(
                gr =>
                {
                    var gameResult = GameResultViewModel.Map(gr);
                    gameResult.HomeTeamName = _teamService.Get(gameResult.HomeTeamId).Name;
                    gameResult.AwayTeamName = _teamService.Get(gameResult.AwayTeamId).Name;
                    return gameResult;
                })
                   .ToList();

            return View(gameResults);
        }
    }
}
