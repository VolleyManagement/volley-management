using System.Collections.Generic;
using System.Text;
using System.Web;

namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Domain.PlayersAggregate;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Players;

    /// <summary>
    /// Defines player controller
    /// </summary>
    public class PlayersController : Controller
    {
        private const int MAX_PLAYERS_ON_PAGE = 5;
        private const string PLAYER_WAS_DELETED_DESCRIPTION = @"Данный игрок не найден, т.к. был удален.
                                                                Операция редактирования невозможна.
                                                                Для создания игрока воспользуйтесь соответствующей ссылкой.";

        private const string HTTP_NOT_FOUND_DESCRIPTION = @"При удалении игрока произошла непредвиденная ситуация.
                                                            Пожалуйста, обратитесь к администратору";

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
        /// <param name="textToSearch">Substring to search in full name of a player.</param>
        /// <returns>View with collection of players.</returns>
        public ActionResult Index(int? page, string textToSearch = "")
        {
            try
            {
                PlayersListViewModel playersOnPage = GetPlayersListViewModel(page, textToSearch);
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
            Player player;
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
        /// Delete player action (POST)
        /// </summary>
        /// <param name="id">Player id</param>
        /// <returns>Result message</returns>
        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                this._playerService.Delete(id);
            }
            catch (MissingEntityException ex)
            {
                return Json(new PlayerDeleteResultViewModel { Message = ex.Message, HasDeleted = false });
            }

            return Json(new PlayerDeleteResultViewModel
            {
                Message = App_GlobalResources.ViewModelResources.PlayerWasDeletedSuccessfully,
                HasDeleted = true
            });
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
                this.ModelState.AddModelError(string.Empty, PLAYER_WAS_DELETED_DESCRIPTION);
                return this.View(playerViewModel);
            }
            catch (ValidationException ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return this.View(playerViewModel);
            }
        }

        /// <summary>
        /// Returns list of free players which are satisfy specified search string, team and exclude list 
        /// </summary>
        /// <param name="searchString">Name of player</param>
        /// <param name="includeTeam">Team which players should be included in search results</param>
        /// <param name="excludeIds">Array of players id will be excluded from search results</param>
        /// <returns>List of free players</returns>
        public JsonResult GetFreePlayers(string searchString, int? includeTeam, string excludeIds)
        {
            IEnumerable<int> excludeIdsArray = null;
            if (!string.IsNullOrEmpty(excludeIds))
            {
                excludeIdsArray = this.GetIntCollection(excludeIds);
            }

            searchString = HttpUtility.UrlDecode(searchString)
                                      .Trim()
                                      .Replace(" ", string.Empty);

            var query = this._playerService.Get()
                            .Where(p => (p.FirstName + p.LastName).Contains(searchString));
                      
            query = includeTeam.HasValue 
                    ? query.Where(p => p.TeamId == includeTeam || p.TeamId == null) 
                    : query.Where(p => p.TeamId == null);

            if (excludeIdsArray != null)
            {
                query = query.Where(p => !excludeIdsArray.Contains(p.Id));
            }                      
            
            return Json(query.OrderBy(p => p.LastName)
                             .ToList()
                             .Select(p => PlayerNameViewModel.Map(p)),
                        JsonRequestBehavior.AllowGet);
        }

        private PlayersListViewModel GetPlayersListViewModel(int? page, string textToSearch = "")
        {
            textToSearch = textToSearch.Trim();
            IQueryable<Player> allPlayers = this._playerService
                                                .Get()
                                                .OrderBy(p => p.LastName);

            if (textToSearch != string.Empty)
            {
                allPlayers = allPlayers
                    .Where(p => (p.FirstName + p.LastName).Contains(textToSearch));
            }

            return new PlayersListViewModel(allPlayers, page, MAX_PLAYERS_ON_PAGE, textToSearch);
        }
    
        private List<int> GetIntCollection(string source)
        {
            var splitted = source.Split(',');
            var result = new List<int>();
            int parsed;
            foreach (var i in splitted)
            {
                if (int.TryParse(i, out parsed))
                {
                    result.Add(parsed);
                }
            }

            return result;
        }
    }
}