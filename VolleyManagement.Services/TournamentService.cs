namespace VolleyManagement.Services
{
    using System;
    using System.Linq;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.Domain.Tournaments;

    /// <summary>
    /// Defines TournamentService
    /// </summary>
    public class TournamentService : ITournamentService
    {
        private readonly ITournamentRepository _tournamentRepository;

        /// <summary>
        /// The number of month uses for sets the limit date from now for getting expected tournaments 
        /// </summary>
        private const int NUMBER_OF_MONTH_QUERY_LIMIT = 3;

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentService"/> class
        /// </summary>
        /// <param name="tournamentRepository">The tournament repository</param>
        public TournamentService(ITournamentRepository tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        /// <summary>
        /// Get tournaments according to set filter
        /// </summary>
        /// <param name="filter">Tournament status filter</param>
        /// <returns>Filtered tournaments</returns>
        public IQueryable<Tournament> Get(TournamentStatusFilter filter = TournamentStatusFilter.All)
        {
            DateTime now = DateTime.Now;

            if (filter == TournamentStatusFilter.ActualAndExpected)
            {
                DateTime maxStartDateFilter = now.AddMonths( NUMBER_OF_MONTH_QUERY_LIMIT );

                return _tournamentRepository.Find().Where(tr =>
                    tr.EndDate >= now && tr.StartDate <= maxStartDateFilter);
            }
            else if (filter == TournamentStatusFilter.Finished)
            {
                return _tournamentRepository.Find().Where(tr =>
                    tr.EndDate < now );
            }

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
                throw new TournamentValidationException(
                    VolleyManagement.Domain.Properties.Resources.TournamentNameMustBeUnique, "Name");
            }
        }
    }
}