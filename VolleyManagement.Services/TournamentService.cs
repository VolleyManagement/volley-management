namespace VolleyManagement.Services
{
    using System;
    using System.Linq;
    using VolleyManagement.Contracts;
    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.Domain.Tournaments;

    /// <summary>
    /// Defines TournamentService
    /// </summary>
    public class TournamentService : ITournamentService
    {
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
        public IQueryable<Tournament> Get()
        {
            return _tournamentRepository.Find();
        }

        /// <summary>
        /// Create a new tournament
        /// </summary>
        /// <param name="tournamentToCreate">A Tournament to create</param>
        public void Create(Tournament tournamentToCreate)
        {
            IsTournamentNameUnique(tournamentToCreate);
            _tournamentRepository.Add(tournamentToCreate);
            _tournamentRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Finds a Tournament by id
        /// </summary>
        /// <param name="id">id for search</param>
        /// <returns>A found Tournament</returns>
        public Tournament Get(int id)
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
            IsTournamentNameUnique(tournamentToEdit);
            _tournamentRepository.Update(tournamentToEdit);
            _tournamentRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Delete tournament by id.
        /// </summary>
        /// <param name="id">The id of tournament to delete.</param>
        public void Delete(int id)
        {
            _tournamentRepository.Remove(id);
            _tournamentRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Checks whether tournament name is unique or not.
        /// </summary>
        /// <param name="newTournament">tournament to edit or create</param>
        private void IsTournamentNameUnique(Tournament newTournament)
        {
            var tournament = _tournamentRepository.FindWhere(t => t.Name == newTournament.Name
                && t.Id != newTournament.Id).FirstOrDefault();

            if (tournament != null)
            {
                throw new ArgumentException(VolleyManagement.Domain.Properties.Resources.TournamentNameMustBeUnique);
            }
        }
    }
}