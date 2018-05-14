namespace VolleyManagement.UI.Areas.WebAPI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Net;
    using System.Web.Http;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Domain.PlayersAggregate;
    using VolleyManagement.UI.Areas.WebApi.Infrastructure;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Players;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Teams;

    public class PlayersController : ApiController
    {
        private const string CONTROLLER_NAME = "players";
        private readonly IPlayerService _playerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayersController"/> class.
        /// </summary>
        /// <param name="playerService"> The player service. </param>
        public PlayersController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        /// <summary>
        /// Gets players
        /// </summary>
        /// <returns> Player list. </returns>
        public ICollection<PlayerViewModel> GetPlayers()
        {
            var result = _playerService.Get()
                                .Select(p => PlayerViewModel.Map(p));

            return result.ToList();
        }

        /// <summary>
        /// The get player.
        /// </summary>
        /// <param name="key"> The key. </param>
        /// <returns> The <see cref="SingleResult"/>. </returns>
        public PlayerViewModel Get(int key)
        {
            var result = _playerService
                .Get()
                .Where(p => p.Id == key)
                .Select(p => PlayerViewModel.Map(p))
                .First();

            return result;
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

            var playerToCreate = player.ToCreatePlayerDto();
            var createdPlayer = _playerService.Create(playerToCreate);
            player.Id = createdPlayer.Id;

            return Ok(player);
        }

        /// <summary>
        /// Updates player
        /// </summary>
        /// <param name="player">The player view model to update</param>
        /// <returns> Updated player
        /// unsuccessfully - Bad request </returns>
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

            return Ok(player);
        }

        /// <summary>
        /// Gets player Team.
        /// </summary>
        /// <param name="key">ID of the player.</param>
        /// <returns>Team that linked to the player.</returns>
        public TeamViewModel GetTeams(int key)
        {
            TeamViewModel team;
            try
            {
                var player = _playerService.Get(key);
                team = TeamViewModel.Map(_playerService.GetPlayerTeam(player));
            }
            catch (MissingEntityException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return team;
        }

        /// <summary> Deletes tournament </summary>
        /// <param name="key"> The key. </param>
        /// <returns> The <see cref="IHttpActionResult"/>. </returns>
        public IHttpActionResult Delete(int key)
        {
            try
            {
                _playerService.Delete(key);
            }
            catch (MissingEntityException ex)
            {
                return BadRequest(ex.Message);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
