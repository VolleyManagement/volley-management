namespace VolleyManagement.UnitTests.Services.ContributorService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.ContributorsAggregate;

    /// <summary>
    /// Builder for test contributors team
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class ContributorTeamBuilder
    {
        /// <summary>
        /// Holds test contributor team instance
        /// </summary>
        private ContributorTeam _contributorTeam;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContributorTeamBuilder"/> class
        /// </summary>
        public ContributorTeamBuilder()
        {
            _contributorTeam = new ContributorTeam
            {
                Id = 1,
                Name = "FirstName",
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
            };
        }

        /// <summary>
        /// Sets id of test contributor team
        /// </summary>
        /// <param name="id">Id for test contributor</param>
        /// <returns>Contributor builder object</returns>
        public ContributorTeamBuilder WithId(int id)
        {
            _contributorTeam.Id = id;
            return this;
        }

        /// <summary>
        /// Sets name of test contributor
        /// </summary>
        /// <param name="name">Name for test contributor</param>
        /// <returns>Contributor team builder object</returns>
        public ContributorTeamBuilder WithName(string name)
        {
            _contributorTeam.Name = name;
            return this;
        }

        /// <summary>
        /// Sets contributor team test last name
        /// </summary>
        /// <param name="courseDirection">Test contributor team last name</param>
        /// <returns>Contributor team builder object</returns>
        public ContributorTeamBuilder WithcourseDirection(string courseDirection)
        {
            _contributorTeam.CourseDirection = courseDirection;
            return this;
        }

        /// <summary>
        /// Sets contributor team test last name
        /// </summary>
        /// <param name="contributors">Test contributor team last name</param>
        /// <returns>Contributor team builder object</returns>
        public ContributorTeamBuilder Withcontributors(IList<Contributor> contributors)
        {
            _contributorTeam.Contributors = contributors;
            return this;
        }

        /// <summary>
        /// Builds test contributor team
        /// </summary>
        /// <returns>Test contributor team</returns>
        public ContributorTeam Build()
        {
            return _contributorTeam;
        }
    }
}
