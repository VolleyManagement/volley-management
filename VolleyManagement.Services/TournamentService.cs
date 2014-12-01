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
        public IQueryable<Tournament> GetAllTournaments()
        {
            return _unitOfWork.Tournaments.FindAll();
        }
    }
}
