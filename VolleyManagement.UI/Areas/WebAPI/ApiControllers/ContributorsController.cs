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
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Contributors;

    /// <summary>
    /// The contributors controller.
    /// </summary>
    public class ContributorsController : ODataController
    {
        private readonly IContributorService _contributorService;

        private const string CONTROLLER_NAME = "contributors";

        /// <summary>
        /// Initializes a new instance of the <see cref="ContributorsController"/> class.
        /// </summary>
        /// <param name="contributorService"> The contributor service. </param>
        public ContributorsController(IContributorService contributorService)
        {
            this._contributorService = contributorService;
        }

        /// <summary>
        /// Gets contributors
        /// </summary>
        /// <returns> Contributor list. </returns>
        [EnableQuery]
        [HttpGet]
        public IQueryable<ContributorViewModel> GetContributors()
        {
            return _contributorService.Get()
                                .ToList()
                                .Select(c => ContributorViewModel.Map(c))
                                .AsQueryable();
        }
    }
}
