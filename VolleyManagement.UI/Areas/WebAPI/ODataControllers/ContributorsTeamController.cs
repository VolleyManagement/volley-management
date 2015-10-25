namespace VolleyManagement.UI.Areas.WebApi.ODataControllers
{
    using System.Linq;
    using System.Web.Http;
    using System.Web.OData;

    using VolleyManagement.Contracts;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.ContributorsTeam;

    /// <summary>
    /// The contributors team controller.
    /// </summary>
    public class ContributorsTeamController : ODataController
    {
        private readonly IContributorTeamService _contributorTeamService;

        private const string CONTROLLER_NAME = "contributorsTeam";

        /// <summary>
        /// Initializes a new instance of the <see cref="ContributorsTeamController"/> class.
        /// </summary>
        /// <param name="contributorTeamService"> The contributors team service. </param>
        public ContributorsTeamController(IContributorTeamService contributorTeamService)
        {
            this._contributorTeamService = contributorTeamService;
        }

        /// <summary>
        /// Gets contributors teams
        /// </summary>
        /// <returns> Contributors teams list. </returns>
        [EnableQuery]
        [HttpGet]
        public IQueryable<ContributorsTeamViewModel> GetContributorsTeam()
        {
            return this._contributorTeamService.Get()
                                .ToList()
                                .Select(c => ContributorsTeamViewModel.Map(c))
                                .AsQueryable();
        }
    }
}
