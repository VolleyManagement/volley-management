namespace VolleyManagement.UnitTests.Services.ContributorService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.ContributorsAggregate;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.ContributorsTeam;

    /// <summary>
    /// Class for generating test data
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class ContributorTeamViewModelServiceTestFixture
    {
        /// <summary>
        /// Holds collection of contributors team
        /// </summary>
        private IList<ContributorsTeamViewModel> _contributorsTeam = new List<ContributorsTeamViewModel>();

        /// <summary>
        /// Adds contributors to collection
        /// </summary>
        /// <returns>Builder object with collection of contributors team</returns>
        public ContributorTeamViewModelServiceTestFixture TestContributor()
        {
            _contributorsTeam.Add(new ContributorsTeamViewModel()
            {
                Id = 1,
                Name = "FirstName1",
                CourseDirection = "Course",
                Contributors = new List<string>
                {
                    "FirstNameA",
                    "FirstNameB",
                    "FirstNameC"
                }
            });
            _contributorsTeam.Add(new ContributorsTeamViewModel()
            {
                Id = 2,
                Name = "FirstName2",
                CourseDirection = "Course",
                Contributors = new List<string>
                {
                    "FirstNameD",
                    "FirstNameE",
                    "FirstNameF"
                }
            });
            return this;
        }

        /// <summary>
        /// Add contributor team to collection.
        /// </summary>
        /// <param name="newContributor">Contributor to add.</param>
        /// <returns>Builder object with collection of contributors.</returns>
        public ContributorTeamViewModelServiceTestFixture AddContributorTeam(ContributorsTeamViewModel newContributor)
        {
            _contributorsTeam.Add(newContributor);
            return this;
        }

        /// <summary>
        /// Builds test data
        /// </summary>
        /// <returns>Contributors team collection</returns>
        public IList<ContributorsTeamViewModel> Build()
        {
            return _contributorsTeam;
        }
    }
}
