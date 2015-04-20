namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Domain.Contributors;
    using VolleyManagement.UI.Areas.Mvc.Mappers;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Contributors;
    using System.Collections.Generic;

    /// <summary>
    /// Defines contributor controller
    /// </summary>
    public class ContributorsController : Controller
    {
       
        /// <summary>
        /// Holds ContributorService instance
        /// </summary>
        private readonly IContributorService _contributorService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContributorsController"/> class
        /// </summary>
        /// <param name="contributorService">Instance of the class that implements
        /// IContributorService.</param>
        public ContributorsController(IContributorService contributorService)
        {
            _contributorService = contributorService;
        }

        /// <summary>
        /// Gets all contributors from ContributorService
        /// </summary>
        /// <returns>View with collection of contributors</returns>
        public ActionResult Index()
        {
            try
            {
                var contributors = this._contributorService.Get().ToList();
                return View(contributors);
            }
            catch (Exception)
            {
                return this.HttpNotFound();
            }
        }
    }
}