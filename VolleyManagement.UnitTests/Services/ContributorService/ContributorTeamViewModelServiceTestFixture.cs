namespace VolleyManagement.UnitTests.Services.ContributorService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.ContributorsAggregate;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.ContributorsTeam;

    /// <summary>
    /// Class for generating test data
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class ContributorTeamViewModelServiceTestFixture
    {
        /// <summary>
        /// Holds collection of contributors
        /// </summary>
        private IList<ContributorsTeamViewModel> _contributorsTeam = new List<ContributorsTeamViewModel>();

        /// <summary>
        /// Adds contributors to collection
        /// </summary>
        /// <returns>Builder object with collection of contributors</returns>
        public ContributorTeamViewModelServiceTestFixture TestContributor()
        {
            _contributorsTeam.Add(new ContributorsTeamViewModel()
            {
                Id = 1,
                Name = "FirstName1",
                CourseDirection = "Course",
                Contributors = new List<Contributor>
                {
                    new Contributor
                    {
                        Id = 1,
                        Name = "FirstNameA",
                        ContributorTeamId = 1
                    },
                    new Contributor
                    {
                        Id = 2,
                        Name = "FirstNameB",
                        ContributorTeamId = 1
                    },
                    new Contributor
                    {
                        Id = 3,
                        Name = "FirstNameC",
                        ContributorTeamId = 1
                    }
                }
            });
            _contributorsTeam.Add(new ContributorsTeamViewModel()
            {
                Id = 2,
                Name = "FirstName2",
                CourseDirection = "Course",
                Contributors = new List<Contributor>
                {
                    new Contributor
                    {
                        Id = 4,
                        Name = "FirstNameD",
                        ContributorTeamId = 2
                    },
                    new Contributor
                    {
                        Id = 5,
                        Name = "FirstNameE",
                        ContributorTeamId = 2
                    },
                    new Contributor
                    {
                        Id = 6,
                        Name = "FirstNameF",
                        ContributorTeamId = 2
                    }
                }
            });
            return this;
        }

        /// <summary>
        /// Add contributor to collection.
        /// </summary>
        /// <param name="newContributor">Contributor to add.</param>
        /// <returns>Builder object with collection of contributors.</returns>
        public ContributorTeamViewModelServiceTestFixture AddTournament(ContributorsTeamViewModel newContributor)
        {
            _contributorsTeam.Add(newContributor);
            return this;
        }

        /// <summary>
        /// Builds test data
        /// </summary>
        /// <returns>Contributor collection</returns>
        public IList<ContributorsTeamViewModel> Build()
        {
            return _contributorsTeam;
        }
    }
}
