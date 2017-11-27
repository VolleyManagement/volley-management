namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.UI;
    using Contracts;
    using ViewModels.ContributorsTeam;

    /// <summary>
    /// Defines contributor team controller
    /// </summary>
    public class ContributorsTeamController : Controller
    {
        /// <summary>
        /// Holds ContributorTeamService instance
        /// </summary>
        private readonly IContributorTeamService _contributorTeamService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContributorsTeamController"/> class
        /// </summary>
        /// <param name="contributorTeamService">Instance of the class that implements
        /// IContributorTeamService.</param>
        public ContributorsTeamController(IContributorTeamService contributorTeamService)
        {
            _contributorTeamService = contributorTeamService;
        }

        /// <summary>
        /// Gets all contributors teams from ContributorService
        /// </summary>
        /// <returns>View with collection of contributors teams</returns>
        [OutputCache(Duration = 86400, Location = OutputCacheLocation.Any)]
        public ActionResult Index()
        {
            var contributorsTeam = _contributorTeamService.Get();

            var contributorsTeamViewModel = new List<ContributorsTeamViewModel>();

            foreach (var item in contributorsTeam)
            {
                contributorsTeamViewModel.Add(ContributorsTeamViewModel.Map(item));
            }

            return View(contributorsTeamViewModel);
        }
    }
}
