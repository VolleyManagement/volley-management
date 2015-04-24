namespace VolleyManagement.Services
{
    using System;
    using System.Linq;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.Dal.Exceptions;
    using VolleyManagement.Domain.ContributorTeams;

    /// <summary>
    /// Defines ContributorService
    /// </summary>
    public class ContributorTeamService : IContributorTeamService
    {
        /// <summary>
        /// Holds ContributorRepositoryTeam instance.
        /// </summary>
        private readonly IContributorTeamService _contributorTeamRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContributorTeamService"/> class.
        /// </summary>
        /// <param name="contributorRepository">The user repository</param>
        public ContributorTeamService(IContributorTeamService contributorTeamRepository)
        {
            _contributorTeamRepository = contributorTeamRepository;
        }

        /// <summary>
        /// Method to get all contributors team.
        /// </summary>
        /// <returns>All teams.</returns>
        public IQueryable<ContributorTeam> Get()
        {
            return _contributorTeamRepository.Find();
        }
    }
}
