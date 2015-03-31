namespace VolleyManagement.UI.Areas.WebApi.ApiControllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Web.Http;
    using System.Web.Http.OData;
    using System.Web.Mvc;
    using VolleyManagement.Contracts;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Players;

    /// <summary>
    /// The plauers controller.
    /// </summary>
    public class PlayersController : ODataController
    {
        private readonly IPlayerService _playerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayersController"/> class.
        /// </summary>
        /// <param name="playerService"> The tournament service. </param>
        public PlayersController(IPlayerService playerService)
        {
            this._playerService = playerService;
        }

        /// <summary>
        /// Gets players
        /// </summary>
        /// <returns> Tournament list. </returns>
        [Queryable]
        public IQueryable<PlayerViewModel> GetTournaments()
        {
            return _playerService.Get()
                                .ToList()
                                .Select(t => PlayerViewModel.Map(t))
                                .AsQueryable();
        }

    }
}
