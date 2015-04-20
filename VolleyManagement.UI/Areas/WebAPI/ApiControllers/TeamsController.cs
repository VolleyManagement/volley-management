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
            _teamService = teamService;
        }

        /// <summary>
        /// Creates Team
        /// </summary>
        /// <param name="team"> The  team as ViewModel. </param>
        /// <returns> Has been saved successfully - Created OData result
        /// unsuccessfully - Bad request </returns>
        public IHttpActionResult Post(TeamViewModel team)
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
            return Created(team);
        }
    }
}
