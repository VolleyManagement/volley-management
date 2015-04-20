namespace VolleyManagement.UnitTests.Services.ContributorService
{
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.Contributors;

    /// <summary>
    /// Builder for test contributors
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class ContributorBuilder
    {
        /// <summary>
        /// Holds test contributor instance
        /// </summary>
        private Contributor _contributor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContributorBuilder"/> class
        /// </summary>
        public ContributorBuilder()
        {
            this._contributor = new Contributor
            {
                Id = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                ContributorTeamId = 1
            };
        }

        /// <summary>
        /// Sets id of test contributor
        /// </summary>
        /// <param name="id">Id for test contributor</param>
        /// <returns>Contributor builder object</returns>
        public ContributorBuilder WithId(int id)
        {
            this._contributor.Id = id;
            return this;
        }

        /// <summary>
        /// Sets name of test contributor
        /// </summary>
        /// <param name="firstName">Name for test contributor</param>
        /// <returns>Contributor builder object</returns>
        public ContributorBuilder WithFirstName(string firstName)
        {
            this._contributor.FirstName = firstName;
            return this;
        }

        /// <summary>
        /// Sets contributor test last name
        /// </summary>
        /// <param name="lastName">Test contributor last name</param>
        /// <returns>Contributor builder object</returns>
        public ContributorBuilder WithLastName(string lastName)
        {
            this._contributor.LastName = lastName;
            return this;
        }

        /// <summary>
        /// Sets contributor test last name
        /// </summary>
        /// <param name="lastName">Test contributor last name</param>
        /// <returns>Contributor builder object</returns>
        public ContributorBuilder WithContributorTeamId(int contributorTeamId)
        {
            this._contributor.ContributorTeamId = contributorTeamId;
            return this;
        }

      
        /// <summary>
        /// Builds test contributor
        /// </summary>
        /// <returns>Test contributor</returns>
        public Contributor Build()
        {
            return this._contributor;
        }
    }
}
