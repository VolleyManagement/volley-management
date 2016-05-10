namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Authorization;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Domain.PlayersAggregate;
    using VolleyManagement.Domain.RolesAggregate;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Players;

    /// <summary>
    /// Defines player controller
    /// </summary>
    public class PlayersController : Controller
    {
        private const int MAX_PLAYERS_ON_PAGE = 5;
        private const string PLAYER_WAS_DELETED_DESCRIPTION = @"The player was not found because he was removed.
                                                                Editing operation is impossible.
                                                                To create a player use the link.";

        private const string HTTP_NOT_FOUND_DESCRIPTION = @"While removing the player unexpected error occurred.
                                                            Please contact the administrator";

        /// <summary>
        /// Holds PlayerService instance
        /// </summary>
        private readonly IPlayerService _playerService;
        private readonly IAuthorizationService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayersController"/> class
        /// </summary>
        /// <param name="playerService">Instance of the class that implements IPlayerService.</param>
        /// <param name="authService">The authorization service</param>
        public PlayersController(IPlayerService playerService, IAuthorizationService authService)
        {
            this._playerService = playerService;
            this._authService = authService;
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
                playersOnPage.AllowedOperations = this._authService.GetAllowedOperations(new List<AuthOperation>()
                {
                    AuthOperations.Players.Create,
                    AuthOperations.Players.Edit,
                    AuthOperations.Players.Delete
                });
                ViewBag.ReturnUrl = this.HttpContext.Request.RawUrl;
                return View(playersOnPage);
            }
            catch (ArgumentOutOfRangeException)
            {
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Gets details for specific player
        /// </summary>
        /// <param name="id">Player id.</param>
        /// <param name="returnUrl">URL for back link</param>
        /// <returns>View with specific player.</returns>
        public ActionResult Details(int id, string returnUrl = "")
        {
            var player = _playerService.Get(id);

            if (player == null)
            {
                return HttpNotFound();
            }

            var model = new PlayerRefererViewModel(player, returnUrl);
            model.AllowedOperations = this._authService.GetAllowedOperations(AuthOperations.Players.Edit);
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
            catch (ValidationException ex)
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
            var player = _playerService.Get(id);

            if (player == null)
            {
                return HttpNotFound();
            }

            var playerViewModel = PlayerViewModel.Map(player);
            return this.View(playerViewModel);
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
        /// <param name="excludeList">list of players ids should be excluded from result</param>
        /// <param name="includeList">list of players ids should be included to result</param>
        /// <param name="includeTeam">Id of team which players should be included to the search result</param>
        /// <returns>List of free players</returns>
        public JsonResult GetFreePlayers(string searchString, string excludeList, string includeList, int? includeTeam)
        {
            searchString = HttpUtility.UrlDecode(searchString).Replace(" ", string.Empty);
            var query = this._playerService.Get()
                            .Where(p => (p.FirstName + p.LastName).Contains(searchString) 
                                   || (p.LastName + p.FirstName).Contains(searchString));

            if (includeTeam.HasValue)
            {
                if (string.IsNullOrEmpty(includeList))
                {
                    query = query.Where(p => p.TeamId == null || p.TeamId == includeTeam.Value);
                }
                else
                {
                    var selectedIds = this.ParseIntList(includeList);
                    query = query.Where(p => p.TeamId == null || p.TeamId == includeTeam.Value || selectedIds.Contains(p.Id));
                }
            } 
            else if (string.IsNullOrEmpty(includeList))
            {
                query = query.Where(p => p.TeamId == null);
            }
            else
            {
                var selectedIds = this.ParseIntList(includeList);
                query = query.Where(p => p.TeamId == null || selectedIds.Contains(p.Id));
            }

            if (!string.IsNullOrEmpty(excludeList))
            {
                var selectedIds = this.ParseIntList(excludeList);
                query = query.Where(p => !selectedIds.Contains(p.Id));
            }
            
            var result = query.OrderBy(p => p.LastName)
                              .ToList()
                              .Select(p => PlayerNameViewModel.Map(p));

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private PlayersListViewModel GetPlayersListViewModel(int? page, string textToSearch = "")
        {
            textToSearch = textToSearch.Trim();
            IQueryable<Player> allPlayers = this._playerService.Get().OrderBy(p => p.LastName);

            if (textToSearch != string.Empty)
            {
                allPlayers = allPlayers.Where(p => (p.LastName + " " + p.FirstName).Contains(textToSearch)
                    || (p.FirstName + " " + p.LastName).Contains(textToSearch));
            }

            return new PlayersListViewModel(allPlayers, page, MAX_PLAYERS_ON_PAGE, textToSearch);
        }

        private List<int> ParseIntList(string source)
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