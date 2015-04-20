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
    public class ContributorServiceTestFixture
    {
        /// <summary>
        /// Holds collection of contributors
        /// </summary>
        private IList<Contributor> _contributors = new List<Contributor>();

        /// <summary>
        /// Adds contributors to collection
        /// </summary>
        /// <returns>Builder object with collection of contributors</returns>
        public ContributorServiceTestFixture TestContributors()
        {
            _contributors.Add(new Contributor()
            {
                Id = 1,
                FirstName = "FirstNameA",
                LastName = "LastNameA",
                ContributorTeamId = 1
            });
            _contributors.Add(new Contributor()
            {
                Id = 2,
                FirstName = "FirstNameB",
                LastName = "LastNameB",
                ContributorTeamId = 1
            });
            _contributors.Add(new Contributor()
            {
                Id = 3,
                FirstName = "FirstNameC",
                LastName = "LastNameC",
                ContributorTeamId = 2
            });
            return this;
        }

        /// <summary>
        /// Add player to collection.
        /// </summary>
        /// <param name="newContributor">Contributor to add.</param>
        /// <returns>Builder object with collection of tournaments.</returns>
        public ContributorServiceTestFixture AddContributor(Contributor newContributor)
        {
            _contributors.Add(newContributor);
            return this;
        }

        /// <summary>
        /// Builds test data
        /// </summary>
        /// <returns>Contributor collection</returns>
        public IList<Contributor> Build()
        {
            return _contributors;
        }
    }
}