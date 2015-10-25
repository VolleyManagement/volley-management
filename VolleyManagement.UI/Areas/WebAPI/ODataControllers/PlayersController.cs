namespace VolleyManagement.UI.Areas.WebApi.ODataControllers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Net;
    using System.Web.Http;
    using System.Web.OData;

    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Domain.PlayersAggregate;
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
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var playerToCreate = player.ToDomain();
            this._playerService.Create(playerToCreate);
            player.Id = playerToCreate.Id;

            return this.Created(player);
        }

        /// <summary>
        /// Gets players
        /// </summary>
        /// <returns> Player list. </returns>
        [EnableQuery]
        public IQueryable<PlayerViewModel> GetPlayers()
        {
            return this._playerService.Get()
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
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            try
            {
                var playerToUpdate = player.ToDomain(); 
                this._playerService.Edit(playerToUpdate); 
                player.Id = playerToUpdate.Id;
            }
            catch (ArgumentException ex)
            {
                this.ModelState.AddModelError(string.Format("{0}.{1}", CONTROLLER_NAME, ex.ParamName), ex.Message);
                return this.BadRequest(this.ModelState);
            }
            catch (ValidationException ex)
            {
                this.ModelState.AddModelError(string.Format("{0}.{1}", CONTROLLER_NAME, ex.Source), ex.Message);
                return this.BadRequest(this.ModelState);
            }
            catch (MissingEntityException ex)
            {
                this.ModelState.AddModelError(string.Format("{0}.{1}", CONTROLLER_NAME, ex.Source), ex.Message);
                return this.BadRequest(this.ModelState);
            }

            return this.Updated(player);
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
            Player playerToUpdate;
            try
            {
                playerToUpdate = this._playerService.Get(key);
            }
            catch (MissingEntityException ex)
            {
                this.ModelState.AddModelError(string.Format("{0}.{1}", CONTROLLER_NAME, ex.Source), ex.Message);
                return this.BadRequest(this.ModelState);
            }

            switch (navigationProperty)
            {
                case "Teams":
                    return this.AssignTeamToPlayer(playerToUpdate, link);
                default:
                    return this.StatusCode(HttpStatusCode.NotImplemented);
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
            return SingleResult.Create(this._playerService.Get()
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
                var player = this._playerService.Get(key);
                team = TeamViewModel.Map(this._playerService.GetPlayerTeam(player));
            }
            catch (MissingEntityException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return WebApiHelpers.ObjectToSingleResult(team);
        }

        /// <summary> Deletes tournament </summary>
        /// <param name="key"> The key. </param>
        /// <returns> The <see cref="IHttpActionResult"/>. </returns>
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            try
            {
                this._playerService.Delete(key);
            }
            catch (MissingEntityException ex)
            {
                return this.BadRequest(ex.Message);
            }
            
            return this.StatusCode(HttpStatusCode.NoContent);
        }

        private IHttpActionResult AssignTeamToPlayer(Player playerToUpdate, Uri link)
        {
            int teamId;
            try
            {
                teamId = WebApiHelpers.GetKeyFromUri<int>(this.Request, link);
                this._teamService.Get(teamId);
                playerToUpdate.TeamId = teamId;
                this._playerService.Edit(playerToUpdate);
            }
            catch (MissingEntityException ex)
            {
                this.ModelState.AddModelError(string.Format("{0}.{1}", CONTROLLER_NAME, ex.Source), ex.Message);
                return this.BadRequest(this.ModelState);
            }
            catch (InvalidOperationException ex)
            {
                this.ModelState.AddModelError(string.Format("{0}.{1}", CONTROLLER_NAME, ex.Source), ex.Message);
                return this.BadRequest(this.ModelState);
            }

            var player = PlayerViewModel.Map(playerToUpdate);
            return this.Updated(player);
        }
    }
}
