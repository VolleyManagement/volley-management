namespace VolleyManagement.Services
{
    using System.Linq;
    using System.Web.Mvc;
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
        /// Holds the state of model instance
        /// </summary>
        private readonly ModelStateDictionary _modelState;

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentService"/> class
        /// </summary>
        /// <param name="modelState">The state of model</param>
        /// <param name="tournamentRepository">The tournament repository</param>
        public TournamentService(ModelStateDictionary modelState, ITournamentRepository tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
            _modelState = modelState;
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
            if (ValidateTournament(tournamentToCreate))
            {
                _tournamentRepository.Add(tournamentToCreate);
                _tournamentRepository.UnitOfWork.Commit();
            }
        }

        /// <summary>
        /// Finds a Tournament by id
        /// </summary>
        /// <param name="id">id for search</param>
        /// <returns>A found Tournament</returns>
        public Tournament FindById(int id)
        {
            var tournament = _tournamentRepository.FindWhere(t => t.Id == id).First();
            return tournament;
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
        /// Validates the Tournament
        /// </summary>
        /// <param name="tournamentToValidate">Tournament to validate</param>
        /// <returns>Is the Tournament valid</returns>
        private bool ValidateTournament(Tournament tournamentToValidate)
        {
            if (tournamentToValidate.Name.Trim().Length == 0)
            {
                _modelState.AddModelError("Name", "Name is required.");
            }

            if (tournamentToValidate.Description.Trim().Length == 0)
            {
                _modelState.AddModelError("Description", "Description is required.");
            }

            if (tournamentToValidate.Season.Trim().Length == 0)
            {
                _modelState.AddModelError("Season", "Season is required.");
            }

            if (tournamentToValidate.Scheme.Trim().Length == 0)
            {
                _modelState.AddModelError("Scheme", "Scheme is required.");
            }

            if (tournamentToValidate.RegulationsLink.Trim().Length == 0)
            {
                _modelState.AddModelError("RegulationsLink", "Regulations link is required.");
            }
            return _modelState.IsValid;
        }
    }
}
