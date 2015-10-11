namespace VolleyManagement.Services
{
    using System.Linq;
    using VolleyManagement.Contracts;
    using VolleyManagement.Domain.ContributorsAggregate;

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
        public IQueryable<ContributorTeam> Get()
        {
            return _contributorTeamRepository.Find();
        }
    }
}
