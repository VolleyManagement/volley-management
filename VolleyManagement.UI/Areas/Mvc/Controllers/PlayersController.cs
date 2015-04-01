
namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System;
    using System.Web;
    using System.Linq;
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
        /// Holds TournamentService instance
        /// </summary>
        private readonly IPlayerService _playerService;

        /// <summary>
        /// Gets playerss from PlayerService
        /// </summary>
        /// <returns>View with collection of playerss</returns>
        public ActionResult Index(int id)
        {
            try
            {
                var allPlayers = this._playerService.Get().OrderBy(p => p.LastName);
                var playersOnPage = new ListOfPlayers(allPlayers, id, MAX_PLAYERS_ON_PAGE);

                return View(playersOnPage);
            }
            catch (Exception)
            {
                return this.HttpNotFound();
            }
        }
    }
}