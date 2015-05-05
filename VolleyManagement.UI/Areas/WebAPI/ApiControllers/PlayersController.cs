namespace VolleyManagement.UI.Areas.WebApi.ApiControllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Net;
    using System.Web.Http;
    using System.Web.OData;

    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.UI.Areas.WebApi.Infrastructure;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Players;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Teams;

    /// <summary>
    /// The players controller.
    /// </summary>
    public class PlayersController : ODataController
    {
        private readonly IPlayerService _playerService;
        private readonly ITeamService _teamService;

        private const string CONTROLLER_NAME = "players";

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayersController"/> class.
        /// </summary>
        /// <param name="playerService"> The player service. </param>
        public PlayersController(IPlayerService playerService, ITeamService teamService)
        {
            this._playerService = playerService;
            this._teamService = teamService;
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
        [EnableQuery]
        public IQueryable<PlayerViewModel> GetPlayers()
        {
            return _playerService.Get()
                                .ToList()
                                .Select(p => PlayerViewModel.Map(p))
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

        /// <summary>
        /// Creates reference between Player and connected entity.
        /// </summary>
        /// <param name="key">ID of the Player.</param>
        /// <param name="navigationProperty">Name of the property.</param>
        /// <param name="link">Link to the entity.</param>
        /// <returns><see cref="IHttpActionResult"/></returns>
        [AcceptVerbs("POST", "PUT")]
        public IHttpActionResult CreateRef([FromODataUri] int key, string navigationProperty, [FromBody] Uri link)
        {
            Domain.Players.Player playerToUpdate;
            try
            {
                playerToUpdate = _playerService.Get(key);
            }
            catch (MissingEntityException ex)
            {
                ModelState.AddModelError(string.Format("{0}.{1}", CONTROLLER_NAME, ex.Source), ex.Message);
                return BadRequest(ModelState);
            }

            switch (navigationProperty)
            {
                case "Teams":
                    return AssignTeamToPlayer(playerToUpdate, link);
                default:
                    return StatusCode(HttpStatusCode.NotImplemented);
            }
        }

        /// <summary>
        /// The get player.
        /// </summary>
        /// <param name="key"> The key. </param>
        /// <returns> The <see cref="SingleResult"/>. </returns>
        [EnableQuery]
        public SingleResult<PlayerViewModel> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_playerService.Get()
                                                         .Where(p => p.Id == key)
                                                         .ToList()
                                                         .Select(p => PlayerViewModel.Map(p))
                                                         .AsQueryable());
        }

        /// <summary>
        /// Gets player Team.
        /// </summary>
        /// <param name="key">ID of the player.</param>
        /// <returns>Team that linked to the player.</returns>
        [EnableQuery]
        public SingleResult<TeamViewModel> GetTeams([FromODataUri] int key)
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

            return Helpers.ObjectToSingleResult(team);
        }

        /// <summary> Deletes tournament </summary>
        /// <param name="key"> The key. </param>
        /// <returns> The <see cref="IHttpActionResult"/>. </returns>
        public IHttpActionResult Delete([FromODataUri] int key)
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

        private IHttpActionResult AssignTeamToPlayer(Domain.Players.Player playerToUpdate, Uri link)
        {
            int teamId;
            try
            {
                teamId = Helpers.GetKeyFromUri<int>(Request, link);
                _teamService.Get(teamId);
            }
            catch (MissingEntityException ex)
            {
                ModelState.AddModelError(string.Format("{0}.{1}", CONTROLLER_NAME, ex.Source), ex.Message);
                return BadRequest(ModelState);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Format("{0}.{1}", CONTROLLER_NAME, ex.Source), ex.Message);
                return BadRequest(ModelState);
            }
            playerToUpdate.TeamId = teamId;
            _playerService.Edit(playerToUpdate);
            var player = PlayerViewModel.Map(playerToUpdate);
            return Updated(player);
        }
    }
}
