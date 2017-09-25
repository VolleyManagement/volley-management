namespace VolleyManagement.UI.Areas.WebAPI.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Players;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Teams;

    public class TeamsController : ApiController
    {
        private const string CONTROLLER_NAME = "teams";
        private readonly ITeamService _teamService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamsController" /> class
        /// </summary>
        /// <param name="teamService"> The team service. </param>
        public TeamsController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        /// <summary>
        /// Creates Team
        /// </summary>
        /// <param name="team"> The  team as ViewModel. </param>
        /// <returns> Has been saved successfully - Created OData result
        /// unsuccessfully - Bad request </returns>
        public async Task<IHttpActionResult> Post(TeamViewModel team)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var teamToCreate = team.ToDomain();

            try
            {
                _teamService.Create(teamToCreate);
            }
            catch (MissingEntityException ex)
            {
                ModelState.AddModelError(string.Format("{0}.{1}", CONTROLLER_NAME, ex.Source), ex.Message);
                return BadRequest(ModelState);
            }

            team.Id = teamToCreate.Id;
            return await Task.FromResult(Ok(team));
        }

        /// <summary>
        /// Gets teams.
        /// </summary>
        /// <returns>Team list. </returns>
        public IQueryable<TeamViewModel> GetTeams()
        {
            return _teamService.Get()
                                .ToList()
                                .Select(t => TeamViewModel.Map(t))
                                .AsQueryable();
        }

        /// <summary>
        /// Gets roster of the team.
        /// </summary>
        /// <param name="key">Id of the team.</param>
        /// <returns>Players in team roster.</returns>
        public async Task<IEnumerable<PlayerViewModel>> GetPlayers(int key)
        {
            var result = _teamService
                .GetTeamRoster(key)
                .Select(p => PlayerViewModel.Map(p));

            return await Task.FromResult(result);
        }

        /// <summary> Deletes team </summary>
        /// <param name="id"> The id. </param>
        /// <returns> The <see cref="IHttpActionResult"/>. </returns>
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                _teamService.Delete(id);
            }
            catch (MissingEntityException ex)
            {
                return BadRequest(ex.Message);
            }

            return await Task.FromResult(StatusCode(HttpStatusCode.NoContent));
        }
    }
}
