namespace VolleyManagement.Services
{
    using System.Collections.Generic;
    using Contracts;
    using Domain.ContributorsAggregate;

    /// <summary>
    /// Defines ContributorService
    /// </summary>
    public class ContributorTeamService : IContributorTeamService
    {
        private readonly IContributorTeamRepository _contributorTeamRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContributorTeamService"/> class.
        /// </summary>
        /// <param name="contributorTeamRepository">The user repository</param>
        public ContributorTeamService(IContributorTeamRepository contributorTeamRepository)
        {
            _contributorTeamRepository = contributorTeamRepository;
        }

        /// <summary>
        /// Method to get all contributors team.
        /// </summary>
        /// <returns>All teams.</returns>
        public ICollection<ContributorTeam> Get()
        {
            return _contributorTeamRepository.Find();
        }
    }
}
