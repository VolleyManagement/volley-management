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
        private const int MAX_PLAYERS_ON_PAGE = 10;
        private const string PLAYER_WAS_DELETED_DESCRIPTION = "Данный игрок не найден, т.к. был удален. Операция редактирования невозможна.";

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
            catch (ArgumentOutOfRangeException)
            {
                return RedirectToAction("Index");
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

        /// <summary>
        /// Edit player action (GET)
        /// </summary>
        /// <param name="id">Player id</param>
        /// <returns>View to edit specific player</returns>
        public ActionResult Edit(int id)
        {
            try
            {
                var player = this._playerService.Get(id);
                PlayerViewModel playerViewModel = PlayerViewModel.Map(player);
                return this.View(playerViewModel);
            }
            catch (Exception)
            {
                return this.HttpNotFound();
            }
        }

        /// <summary>
        /// Edit player action (POST)
        /// </summary>
        /// <param name="playerViewModel">Player after editing</param>
        /// <returns>Index view if player was valid, else - edit view</returns>
        [HttpPost]
        public ActionResult Edit(PlayerViewModel playerViewModel)
        {
            ////playerViewModel.Id = 20; ////its for test while I dont have delete action
            try
            {
                if (this.ModelState.IsValid)
                {
                    var player = playerViewModel.ToDomain();
                    this._playerService.Edit(player);
                    return this.RedirectToAction("Index");
                }

                return this.View(playerViewModel);
            }
            catch (InvalidKeyValueException)
            {
                return this.HttpNotFound(PLAYER_WAS_DELETED_DESCRIPTION);
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