namespace VolleyManagement.UnitTests.Services.ContributorService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.ContributorsAggregate;

    /// <summary>
    /// Class for generating test data
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ContributorTeamServiceTestFixture
    {
        /// <summary>
        /// Holds collection of contributors team
        /// </summary>
        private List<ContributorTeam> _contributorsTeam = new List<ContributorTeam>();

        /// <summary>
        /// Adds contributors to collection team
        /// </summary>
        /// <returns>Builder object with collection teams of contributors</returns>
        public ContributorTeamServiceTestFixture TestContributors()
        {
            _contributorsTeam.Add(new ContributorTeam()
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
            _contributorsTeam.Add(new ContributorTeam()
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
        /// Builds test data
        /// </summary>
        /// <returns>Contributors team collection</returns>
        public List<ContributorTeam> Build()
        {
            return _contributorsTeam;
        }
    }
}