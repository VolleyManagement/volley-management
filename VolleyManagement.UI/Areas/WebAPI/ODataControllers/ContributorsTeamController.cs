namespace VolleyManagement.UI.Areas.WebApi.ODataControllers
{
    using System.Linq;
    using System.Web.Http;
    using System.Web.OData;

    using Contracts;
    using ViewModels.ContributorsTeam;

    /// <summary>
    /// The contributors team controller.
    /// </summary>
    public class ContributorsTeamController : ODataController
    {
        private const string CONTROLLER_NAME = "contributorsTeam";
        private readonly IContributorTeamService _contributorTeamService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContributorsTeamController"/> class.
        /// </summary>
        /// <param name="contributorTeamService"> The contributors team service. </param>
        public ContributorsTeamController(IContributorTeamService contributorTeamService)
        {
            _contributorTeamService = contributorTeamService;
        }

        /// <summary>
        /// Gets contributors teams
        /// </summary>
        /// <returns> Contributors teams list. </returns>
        [EnableQuery]
        [HttpGet]
        public IQueryable<ContributorsTeamViewModel> GetContributorsTeam()
        {
            return _contributorTeamService.Get()
                                .ToList()
                                .Select(c => ContributorsTeamViewModel.Map(c))
                                .AsQueryable();
        }
    }
}
