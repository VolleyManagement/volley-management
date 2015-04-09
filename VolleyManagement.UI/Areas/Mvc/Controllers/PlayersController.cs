namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Domain.Players;
    using VolleyManagement.Domain.Tournaments;
    using VolleyManagement.Dal.Exceptions;
    using VolleyManagement.UI.Areas.Mvc.Mappers;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Players;

    /// <summary>
    /// Defines player controller
    /// </summary>
    public class PlayersController : Controller
    {
        private const int MAX_PLAYERS_ON_PAGE = 10;

        private const string HTTP_NOT_FOUND_DESCRIPTION = "При удалении игрока произошла непредвиденная ситуация. Пожалуйста, обратитесь к администратору";

        /// <summary>
        /// Holds PlayerService instance
        /// </summary>
        private readonly IPlayerService _playerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayersController"/> class
        /// </summary>
        /// <param name="playerSerivce">Instance of the class that implements
        /// IPlayerService.</param>
        public PlayersController(IPlayerService playerSerivce)
        {
            _playerService = playerSerivce;
        }

        /// <summary>
        /// Gets players from PlayerService
        /// </summary>
        /// <param name="page">Number of the page.</param>
        /// <returns>View with collection of players.</returns>
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
        /// Gets details for specific player
        /// </summary>
        /// <param name="id">Player id.</param>
        /// <returns>View with specific player.</returns>
        public ActionResult Details(int id)
        {
            Domain.Players.Player player;
            try
            {
                player = _playerService.Get(id);
            }
            catch (MissingEntityException)
            {
                return this.HttpNotFound();
            }

            var referer = (string)RouteData.Values["controller"];
            var model = new PlayerRefererViewModel(player, referer);
            return View(model);
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
        /// Delete player action (GET)
        /// </summary>
        /// <param name="id">Player id</param>
        /// <returns>View to delete specific player</returns>
        public ActionResult Delete(int id, string firstName, string lastName)
        {
            PlayerViewModel playerViewModel = new PlayerViewModel()
            {
                FirstName = firstName,
                LastName = lastName,
                Id = id
            };

            return View(playerViewModel);
        }

        /// <summary>
        /// Delete player action (POST)
        /// </summary>
        /// <param name="id">Player id</param>
        /// <returns>Index view</returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ActionResult result;
            try
            {
                this._playerService.Delete(id);
                result = this.RedirectToAction("Index");
            }
            catch (MissingEntityException)
            {
                result = this.HttpNotFound(HTTP_NOT_FOUND_DESCRIPTION);
            }

            return result;

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
            catch (MissingEntityException)
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
            catch (MissingEntityException)
            {
                this.ModelState.AddModelError(string.Empty, Resources.PlayerViews.PlayerWasDeleted);
                return this.View(playerViewModel);
            }
            catch (ValidationException ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return this.View(playerViewModel);
            }
        }
    }
}