namespace VolleyManagement.Services
{
    using System.Linq;

    using global::VolleyManagement.Contracts;
    using global::VolleyManagement.Dal.Contracts;
    using global::VolleyManagement.Domain.Tournaments;

    /// <summary>
    /// Defines TournamentService
    /// </summary>
    public class TournamentService : ITournamentService
    {
        /// <summary>
        /// Holds UnitOfWork instance to access to repository
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentService"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public TournamentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Method to get all tournaments
        /// </summary>
        /// <returns>all tournaments</returns>
        public IQueryable<Tournament> GetAll()
        {
            return _unitOfWork.Tournaments.FindAll();
        }

        /// <summary>
        /// Create new tournament
        /// </summary>
        /// <param name="tournament">New tournament</param>
        public void Create(Tournament tournament)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Edit tournament
        /// </summary>
        /// <param name="tournament">New tournament data</param>
        /// <param name="dalTournament">New data to database</param>
        public void Edit(Tournament tournament, VolleyManagement.Dal.MsSql.Tournament dalTournament)
        {
            throw new System.NotImplementedException();
        }
    }
}
