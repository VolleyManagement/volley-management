namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Domain.ContributorsAggregate;
    using VolleyManagement.UI.Areas.Mvc.Mappers;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.ContributorsTeam;
    using System.Collections.Generic;

    /// <summary>
    /// Defines contributor controller
    /// </summary>
    public class ContributorsTeamController : Controller
    {
       
        /// <summary>
        /// Holds ContributorService instance
        /// </summary>
        private readonly IContributorTeamService _contributorTeamService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContributorsTeamController"/> class
        /// </summary>
        /// <param name="contributorTeamService">Instance of the class that implements
        /// IContributorService.</param>
        public ContributorsTeamController(IContributorTeamService contributorTeamService)
        {
            _contributorTeamService = contributorTeamService;
        }

        /// <summary>
        /// Gets all contributors from ContributorService
        /// </summary>
        /// <returns>View with collection of contributors</returns>
        public ActionResult Index()
        {
            try
            {
                var contributors = this._contributorTeamService.Get().ToList();
                return View(contributors);
            }
            catch (Exception)
            {
                return this.HttpNotFound();
            }
        }
    }
}