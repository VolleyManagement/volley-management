namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Domain.Tournaments;
    using VolleyManagement.UI.Areas.Mvc.Mappers;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Players;

    /// <summary>
    /// Defines player controller
    /// </summary>
    public class PlayersController : Controller
    {
        /// <summary>
        /// Max Players on page
        /// </summary>
        private const int MAX_PLAYERS_ON_PAGE = 10;

        /// <summary>
        /// Holds PlayerService instance
        /// </summary>
        private readonly IPlayerService _playerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayersController"/> class
        /// </summary>
        /// <param name="playerSerivce"></param>
        public PlayersController(IPlayerService playerSerivce)
        {
            _playerService = playerSerivce;
        }

        /// <summary>
        /// Gets players from PlayerService
        /// </summary>
        /// <returns>View with collection of playerss</returns>
        public ActionResult Index(int? page)
        {
            try
            {
                var allPlayers = this._playerService.Get().OrderBy(p => p.LastName);
                var playersOnPage = new PlayersListViewModel(allPlayers, page, MAX_PLAYERS_ON_PAGE);
                return View(playersOnPage);
            }
            catch (Exception)
            {
                return this.HttpNotFound();
            }
        }

        /// <summary>
        /// Create player action GET       
        /// </summary>
        /// <returns>Empty player view model</returns>
        public ActionResult Create()
        {
            var playerViewModel = new PlayerViewModel();
            return this.View(playerViewModel);
        }

        /// <summary>
        /// Create player action POST
        /// </summary>
        /// <param name="playerViewModel">Player view model</param>
        /// <returns>Redirect to players index page</returns>
        [HttpPost]
        public ActionResult Create(PlayerViewModel playerViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(playerViewModel);
            }

            try
            {
                var domainPlayer = playerViewModel.ToDomain();
                this._playerService.Create(domainPlayer);
                playerViewModel.Id = domainPlayer.Id;
                return this.RedirectToAction("Index");
            }
            catch (ArgumentException ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return this.View(playerViewModel);
            }
            catch (ValidationException ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return this.View(playerViewModel);
            }
            catch (Exception)
            {
                return this.HttpNotFound();
            }
        }
    }
}