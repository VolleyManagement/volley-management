namespace VolleyManagement.Services
{
    using System.Linq;

    using VolleyManagement.Contracts;
    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.Domain.Tournaments;

    /// <summary>
    /// Defines TournamentService
    /// </summary>
    public class TournamentService : ITournamentService
    {
        /// <summary>
        /// Holds TournamentRepository instance
        /// </summary>
        private readonly ITournamentRepository _tournamentRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentService"/> class
        /// </summary>
        /// <param name="tournamentRepository">The tournament repository</param>
        public TournamentService(ITournamentRepository tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        /// <summary>
        /// Method to get all tournaments
        /// </summary>
        /// <returns>All tournaments</returns>
        public IQueryable<Tournament> GetAll()
        {
            return _tournamentRepository.FindAll();
        }

        /// <summary>
        /// Create tournament
        /// </summary>
        /// <param name="tournament">New tournament</param>
        public void Create(Tournament tournament)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Edit tournament
        /// </summary>
        /// <param name="tournament">New data</param>
        public void Edit(Tournament tournament)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Delete specific tournament
        /// </summary>
        /// <param name="id">Tournament id</param>
        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Find tournament by id
        /// </summary>
        /// <param name="id">Tournament id</param>
        /// <returns>Specific tournament</returns>
        public Tournament FindById(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
