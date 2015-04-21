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
        /// <returns>All contributors.</returns>
        public IQueryable<Contributor> Get()
        {
            return _contributorRepository.Find();
        }

        /// <summary>
        /// Create a new contributor.
        /// </summary>
        /// <param name="contributorToCreate">A Contributor to create.</param>
        public void Create(Contributor contributor)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Edit contributor.
        /// </summary>
        /// <param name="contributorToEdit">Contributor to edit.</param>
        public void Edit(Contributor contributor)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds a Contributor by id.
        /// </summary>
        /// <param name="id">id for search.</param>
        /// <returns>A found Contributor.</returns>
        public Contributor Get(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete contributor by id.
        /// </summary>
        /// <param name="id">The id of contributor to delete.</param>
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
