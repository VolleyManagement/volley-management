namespace VolleyManagement.UI.Areas.WebApi.ApiControllers
{
    using System;
    using System.ComponentModel.DataAnnotations;
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

        private const string CONTROLLER_NAME = "players";

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayersController"/> class.
        /// </summary>
        /// <param name="playerService"> The tournament service. </param>
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
        /// Updates player
        /// </summary>
        /// <param name="playerViewModel">The player view model to update</param>
        /// <returns><see cref="IHttpActionResult"/></returns>
        public IHttpActionResult Put(PlayerViewModel player)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var playerToUpdate = player.ToDomain(); 
                _playerService.Edit(playerToUpdate); 
                player.Id = playerToUpdate.Id;
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Format("{0}.{1}", CONTROLLER_NAME, ex.ParamName), ex.Message);
                return BadRequest(ModelState);
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(string.Format("{0}.{1}", CONTROLLER_NAME, ex.Source), ex.Message);
                return BadRequest(ModelState);
            }
            catch (MissingEntityException ex)
            {
                ModelState.AddModelError(string.Format("{0}.{1}", CONTROLLER_NAME, ex.Source), ex.Message);
                return BadRequest(ModelState);
            }

            return Updated(player);
        }
    }
}
