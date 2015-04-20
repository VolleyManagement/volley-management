namespace VolleyManagement.Services
{
    using System;
    using System.Linq;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.Dal.Exceptions;
    using VolleyManagement.Domain.Contributors;

    /// <summary>
    /// Defines ContributorService
    /// </summary>
    public class ContributorService : IContributorService
    {
        /// <summary>
        /// Holds ContributorRepository instance.
        /// </summary>
        private readonly IContributorRepository _contributorRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContributorService"/> class.
        /// </summary>
        /// <param name="contributorRepository">The user repository</param>
        public ContributorService(IContributorRepository contributorRepository)
        {
            _contributorRepository = contributorRepository;
        }

        /// <summary>
        /// Method to get all contributors.
        /// </summary>
        /// <returns>All players.</returns>
        public IQueryable<Contributor> Get()
        {
            return _contributorRepository.Find();
        }
    }
}
