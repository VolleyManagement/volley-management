namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using Contracts;
    using Contracts.Authorization;
    using Contracts.Exceptions;
    using Domain.PlayersAggregate;
    using Domain.RolesAggregate;
    using ViewModels.Players;

#pragma warning disable S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)
    /// <summary>
    /// Defines player controller
    /// </summary>
    public class PlayersController : Controller
#pragma warning restore S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)
    {
        /// <summary>
        /// User Id for anonym role.
        /// </summary>
        private const int ANONYM = -1;
        private const int MAX_PLAYERS_ON_PAGE = 12;
        private const string PLAYER_WAS_DELETED_DESCRIPTION = @"The player was not found because he was removed.
                                                                Editing operation is impossible.
                                                                To create a player use the link.";

        /// <summary>
        /// Holds PlayerService instance
        /// </summary>
        private readonly IPlayerService _playerService;
        private readonly IAuthorizationService _authService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IRequestService _requestService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayersController"/> class
        /// </summary>
        /// <param name="playerService">Instance of the class that implements IPlayerService.</param>
        /// <param name="authService">The authorization service</param>
        /// <param name="currentUserService">The interface reference of current user service.</param>
        /// <param name="requestService">The interface reference of request service.</param>
        public PlayersController(
            IPlayerService playerService,
            IAuthorizationService authService,
            ICurrentUserService currentUserService,
            IRequestService requestService)
        {
            _playerService = playerService;
            _authService = authService;
            _currentUserService = currentUserService;
            _requestService = requestService;
        }

#pragma warning disable S2360 // Optional parameters should not be used
        /// <summary>
        /// Gets players from PlayerService
        /// </summary>
        /// <param name="page">Number of the page.</param>
        /// <param name="textToSearch">Substring to search in full name of a player.</param>
        /// <returns>View with collection of players.</returns>
        public ActionResult Index(int? page, string textToSearch = "")
#pragma warning restore S2360 // Optional parameters should not be used
        {
            try
            {
                var playersOnPage = GetPlayersListViewModel(page, textToSearch);
                playersOnPage.AllowedOperations = _authService.GetAllowedOperations(new List<AuthOperation>
                {
                    AuthOperations.Players.Create,
                    AuthOperations.Players.Edit,
                    AuthOperations.Players.Delete
                });
                var referrerViewModel = new PlayersListReferrerViewModel(playersOnPage, HttpContext.Request.RawUrl);
                return View(referrerViewModel);
            }
            catch (ArgumentOutOfRangeException)
            {
                return RedirectToAction("Index");
            }
        }

#pragma warning disable S2360 // Optional parameters should not be used
        /// <summary>
        /// Gets details for specific player
        /// </summary>
        /// <param name="id">Player id.</param>
        /// <param name="returnUrl">URL for back link</param>
        /// <returns>View with specific player.</returns>
        public ActionResult Details(int id, string returnUrl = "")
#pragma warning restore S2360 // Optional parameters should not be used
        {
            var player = _playerService.Get(id);

            if (player == null)
            {
                return HttpNotFound();
            }

            var model = new PlayerRefererViewModel(player, returnUrl) {
                AllowedOperations = _authService.GetAllowedOperations(AuthOperations.Players.Edit)
            };
            return View(model);
        }

        /// <summary>
        /// It links this player to current user.
        /// </summary>
        /// <param name="playerId">Player id.</param>
        /// <returns>Message to user about binding
        /// Player and User. </returns>
        public string LinkWithUser(int playerId)
        {
            var userId = _currentUserService.GetCurrentUserId();
            string message;

            if (userId != ANONYM)
            {
                _requestService.Create(userId, playerId);
                message = Resources.UI.ViewModelResources.MessageAboutLinkToPlayer;
            }
            else
            {
                message = Resources.UI.ViewModelResources.MessageAboutError;
            }

            return message;
        }

        /// <summary>
        /// Create player action GET
        /// </summary>
        /// <returns>Empty player view model</returns>
        public ActionResult Create()
        {
            var playerViewModel = new PlayerViewModel();
            return View(playerViewModel);
        }

        /// <summary>
        /// Create player action POST
        /// </summary>
        /// <param name="playerViewModel">Player view model</param>
        /// <returns>Redirect to players index page</returns>
        [HttpPost]
        public ActionResult Create(PlayerViewModel playerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(playerViewModel);
            }

            try
            {
                var createPlayerDto = playerViewModel.ToCreatePlayerDto();
                var player = _playerService.Create(createPlayerDto);
                playerViewModel.Id = player.Id;
                return RedirectToAction("Index");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(playerViewModel);
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(playerViewModel);
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
                _playerService.Delete(id);
            }
            catch (MissingEntityException ex)
            {
                return Json(new PlayerDeleteResultViewModel { Message = ex.Message, HasDeleted = false });
            }
            catch (ValidationException ex)
            {
                return Json(new PlayerDeleteResultViewModel { Message = ex.Message, HasDeleted = false });
            }

            return Json(new PlayerDeleteResultViewModel {
                Message = Resources.UI.ViewModelResources.PlayerWasDeletedSuccessfully,
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
            return View(playerViewModel);
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
                if (ModelState.IsValid)
                {
                    var player = playerViewModel.ToDomain();
                    _playerService.Edit(player);
                    return RedirectToAction("Index");
                }

                return View(playerViewModel);
            }
            catch (MissingEntityException)
            {
                ModelState.AddModelError(string.Empty, PLAYER_WAS_DELETED_DESCRIPTION);
                return View(playerViewModel);
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(playerViewModel);
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
#pragma warning disable S1226 // Method parameters, caught exceptions and foreach variables' initial values should not be ignored
            searchString = HttpUtility.UrlDecode(searchString).Replace(" ", string.Empty);
#pragma warning restore S1226 // Method parameters, caught exceptions and foreach variables' initial values should not be ignored
            var query = _playerService.Get()
                .Where(p => (p.FirstName + p.LastName).Contains(searchString)
                            || (p.LastName + p.FirstName).Contains(searchString));

            if (includeTeam.HasValue)
            {
                if (string.IsNullOrEmpty(includeList))
                {
                    query = query.Where(p => IsFreePlayer(p, includeTeam));
                }
                else
                {
                    var selectedIds = ParseIntList(includeList);
                    query = query.Where(p => IsFreePlayer(p, includeTeam) || selectedIds.Contains(p.Id));
                }
            }
            else if (string.IsNullOrEmpty(includeList))
            {
                query = query.Where(p => IsFreePlayer(p, null));
            }
            else
            {
                var selectedIds = ParseIntList(includeList);
                query = query.Where(p => IsFreePlayer(p, null) || selectedIds.Contains(p.Id));
            }

            if (!string.IsNullOrEmpty(excludeList))
            {
                var selectedIds = ParseIntList(excludeList);
                query = query.Where(p => !selectedIds.Contains(p.Id));
            }

            var result = query.OrderBy(p => p.LastName)
#pragma warning disable S2971 // "IEnumerable" LINQs should be simplified Enity franework error must be ToList()
                .ToList()
#pragma warning restore S2971 // "IEnumerable" LINQs should be simplified
                .Select(p => PlayerNameViewModel.Map(p));

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private PlayersListViewModel GetPlayersListViewModel(int? page, string textToSearch = "")
        {
            IQueryable<Player> allPlayers = _playerService.Get().OrderBy(p => p.LastName);
            var trimResult = "";
            if (!string.IsNullOrEmpty(textToSearch))
            {
                trimResult = textToSearch.Trim();

                allPlayers = allPlayers.Where(p => (p.LastName + " " + p.FirstName).Contains(trimResult)
                    || (p.FirstName + " " + p.LastName).Contains(trimResult));
            }

            return new PlayersListViewModel(allPlayers, page, MAX_PLAYERS_ON_PAGE, trimResult);
        }

        private static List<int> ParseIntList(string source)
        {
            var splitted = source.Split(',');
            var result = new List<int>();
            foreach (var i in splitted)
            {
                if (int.TryParse(i, out var parsed))
                {
                    result.Add(parsed);
                }
            }

            return result;
        }

        private bool IsFreePlayer(Player player, int? includeTeam)
        {
            var team = _playerService.GetPlayerTeam(player);
            return team == null || (includeTeam.HasValue && team.Id == includeTeam.Value);
        }
    }
}