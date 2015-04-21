namespace VolleyManagement.UI.Areas.WebApi.ApiControllers
{
    using System.Net;
    using System.Web.Http;
    using System.Web.OData;

    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;

    public class TeamsController : ODataController
    {
        private readonly ITeamService _teamService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamsController"/> class.
        /// </summary>
        /// <param name="teamService"> The team service. </param>
        public TeamsController(ITeamService teamService)
        {
            this._teamService = teamService;
        }

        /// <summary> Deletes team </summary>
        /// <param name="id"> The id. </param>
        /// <returns> The <see cref="IHttpActionResult"/>. </returns>
        public IHttpActionResult Delete([FromODataUri] int id)
        {
            try
            {
                _teamService.Delete(id);
            }
            catch (MissingEntityException ex)
            {
                return BadRequest(ex.Message);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
