namespace VolleyManagement.UnitTests.Services.ContributorService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.Contributors;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Contributors;

    /// <summary>
    /// Class for generating test data
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class ContributorViewModelServiceTestFixture
    {
        /// <summary>
        /// Holds collection of contributors
        /// </summary>
        private IList<ContributorViewModel> _contributors = new List<ContributorViewModel>();

        /// <summary>
        /// Adds contributors to collection
        /// </summary>
        /// <returns>Builder object with collection of contributors</returns>
        public ContributorViewModelServiceTestFixture TestContributor()
        {
            _contributors.Add(new ContributorViewModel()
            {
                Id = 1,
                Name = "FirstNameA",
                ContributorTeamId = 1
            });
            _contributors.Add(new ContributorViewModel()
            {
                Id = 2,
                Name = "FirstNameB",
                ContributorTeamId = 1
            });
            _contributors.Add(new ContributorViewModel()
            {
                Id = 3,
                Name = "FirstNameC",
                ContributorTeamId = 2
            });
            return this;
        }

        /// <summary>
        /// Add contributor to collection.
        /// </summary>
        /// <param name="newContributor">Contributor to add.</param>
        /// <returns>Builder object with collection of contributors.</returns>
        public ContributorViewModelServiceTestFixture AddTournament(ContributorViewModel newContributor)
        {
            _contributors.Add(newContributor);
            return this;
        }

        /// <summary>
        /// Builds test data
        /// </summary>
        /// <returns>Contributor collection</returns>
        public IList<ContributorViewModel> Build()
        {
            return _contributors;
        }
    }
}
