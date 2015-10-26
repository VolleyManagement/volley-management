namespace VolleyManagement.UI.Areas.WebApi.ODataControllers
{
    using System.Linq;
    using System.Net;
    using System.Web.Http;
    using System.Web.OData;

    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Players;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Teams;

    /// <summary>
    /// The teams controller.
    /// </summary>
    public class TeamsController : ODataController
    {
        private readonly ITeamService _teamService;

        private const string CONTROLLER_NAME = "teams";

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamsController"/> class.
        /// </summary>
        /// <param name="teamService"> The team service. </param>
        public TeamsController(ITeamService teamService)
        {
            this._teamService = teamService;
        }

        /// <summary>
        /// Creates Team
        /// </summary>
        /// <param name="team"> The  team as ViewModel. </param>
        /// <returns> Has been saved successfully - Created OData result
        /// unsuccessfully - Bad request </returns>
        public IHttpActionResult Post(TeamViewModel team)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var teamToCreate = team.ToDomain();

            try
            {
                this._teamService.Create(teamToCreate);
            }
            catch (MissingEntityException ex)
            {
                this.ModelState.AddModelError(string.Format("{0}.{1}", CONTROLLER_NAME, ex.Source), ex.Message);
                return this.BadRequest(this.ModelState);
            }

            team.Id = teamToCreate.Id;
            return this.Created(team);
        }

        /// <summary>
        /// Gets teams.
        /// </summary>
        /// <returns>Team list. </returns>
        [EnableQuery]
        public IQueryable<TeamViewModel> GetTeams()
        {
            return this._teamService.Get()
                                .ToList()
                                .Select(t => TeamViewModel.Map(t))
                                .AsQueryable();
        }

        /// <summary>
        /// Gets roster of the team.
        /// </summary>
        /// <param name="key">Id of the team.</param>
        /// <returns>Players in team roster.</returns>
        [EnableQuery]
        public IQueryable<PlayerViewModel> GetPlayers([FromODataUri] int key)
        {
            return this._teamService.GetTeamRoster(key)
                .Select(p => PlayerViewModel.Map(p))
                .AsQueryable();
        }

        /// <summary> Deletes team </summary>
        /// <param name="id"> The id. </param>
        /// <returns> The <see cref="IHttpActionResult"/>. </returns>
        public IHttpActionResult Delete([FromODataUri] int id)
        {
            try
            {
                this._teamService.Delete(id);
            }
            catch (MissingEntityException ex)
            {
                return this.BadRequest(ex.Message);
            }

            return this.StatusCode(HttpStatusCode.NoContent);
        }
    }
}
