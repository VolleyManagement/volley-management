namespace VolleyManagement.UI.Areas.WebApi.ApiControllers
{
    using System;
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
                                .Select(p => PlayerViewModel.Map(p, _playerService.GetPlayerTeam(p)))
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

        [AcceptVerbs("POST", "PUT")]
        /// <summary>
        /// Updates player team by id.
        /// </summary>
        /// <param name="playerViewModel">The player view model to update</param>
        /// <param name="teamId">Team id to assign the player to.</param>
        /// <returns><see cref="IHttpActionResult"/></returns>
        public IHttpActionResult CreateRef([FromODataUri] int key,
            string navigationProperty, [FromBody] Uri link)
        {
            Domain.Players.Player playerToUpdate;
            Domain.Teams.Team team;
            switch (navigationProperty)
            {
                case "Teams":
                    int teamId = Helpers.GetKeyFromUri<int>(Request, link);
                    try
                    {
                        playerToUpdate = _playerService.Get(key);
                        team = _teamService.Get(teamId);
                    }
                    catch (MissingEntityException ex)
                    {
                        ModelState.AddModelError(string.Format("{0}.{1}", CONTROLLER_NAME, ex.Source), ex.Message);
                        return BadRequest(ModelState);
                    }
                    playerToUpdate.TeamId = teamId;
                    _playerService.Edit(playerToUpdate);
                    var player = PlayerViewModel.Map(playerToUpdate, team);
                    return Updated(player);
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
                                                         .Select(p => PlayerViewModel.Map(p, _playerService.GetPlayerTeam(p)))
                                                         .AsQueryable());
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
    }
}
