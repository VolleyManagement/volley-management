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
        /// Create a new tournament
        /// </summary>
        /// <param name="tournamentToCreate">A Tournament to create</param>
        public void Create(Tournament tournamentToCreate)
        {
            _tournamentRepository.Add(tournamentToCreate);
            _tournamentRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Finds a Tournament by id
        /// </summary>
        /// <param name="id">id for search</param>
        /// <returns>A found Tournament</returns>
        public Tournament FindById(int id)
        {
            var tournament = _tournamentRepository.FindWhere(t => t.Id == id).Single();
            return tournament;
        }

        /// <summary>
        /// Edit tournament
        /// </summary>
        /// <param name="tournamentToEdit">Tournament to edit</param>
        public void Edit(Tournament tournamentToEdit)
        {
            _tournamentRepository.Update(tournamentToEdit);
            _tournamentRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Delete tournament
        /// </summary>
        /// <param name="id">Tournament id</param>
        public void Delete(int id)
        {
            var tournamentToDelete = new Tournament { Id = id };
            _tournamentRepository.Remove(tournamentToDelete);
            _tournamentRepository.UnitOfWork.Commit();
        }
    }
}