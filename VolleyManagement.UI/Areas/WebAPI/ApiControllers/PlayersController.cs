namespace VolleyManagement.UI.Areas.WebApi.ApiControllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Web.Http;
    using System.Web.Http.OData;
    using System.Web.Mvc;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Players;

    /// <summary>
    /// The players controller.
    /// </summary>
    public class PlayersController : ODataController
    {
        private readonly IPlayerService _playerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayersController"/> class.
        /// </summary>
        /// <param name="playerService"> The player service. </param>
        public PlayersController(IPlayerService playerService)
        {
            this._playerService = playerService;
        }

        /// <summary>
        /// Creates Player
        /// </summary>
        /// <param name="player"> The  player as ViewModel. </param>
        /// <returns> Has been saved successfully - Created OData result
        /// unsuccessfully - Bad request </returns>
        public IHttpActionResult Post(PlayerViewModel player)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var playerToCreate = player.ToDomain();
            _playerService.Create(playerToCreate);
            player.Id = playerToCreate.Id;

            return Created(player);
        }

        /// <summary>
        /// Gets players
        /// </summary>
        /// <returns> Player list. </returns>
        [Queryable]
        public IQueryable<PlayerViewModel> GetPlayers()
        {
            return _playerService.Get()
                                .ToList()
                                .Select(t => PlayerViewModel.Map(t))
                                .AsQueryable();
        }

        /// <summary>
        /// The get player.
        /// </summary>
        /// <param name="key"> The key. </param>
        /// <returns> The <see cref="SingleResult"/>. </returns>
        [Queryable]
        public SingleResult<PlayerViewModel> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_playerService.Get()
                                                         .Where(p => p.Id == key)
                                                         .ToList()
                                                         .Select(p => PlayerViewModel.Map(p))
                                                         .AsQueryable());
        }
    }
}
