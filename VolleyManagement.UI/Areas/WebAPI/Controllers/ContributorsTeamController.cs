namespace VolleyManagement.UI.Areas.WebAPI.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using VolleyManagement.Contracts;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.ContributorsTeam;

    public class ContributorsTeamController : ApiController
    {
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
        [HttpGet]
        public async Task<IEnumerable<ContributorsTeamViewModel>> GetContributorsTeam()
        {
            var list = await _contributorTeamService.Get();

            return list.Select(c => ContributorsTeamViewModel.Map(c));
        }
    }
}
