namespace VolleyManagement.Services
{
    using System;
    using System.Linq;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.Dal.Exceptions;
    using VolleyManagement.Domain.Teams;

    using DAL = VolleyManagement.Dal.Contracts;

    /// <summary>
    /// Defines TeamService
    /// </summary>
    public class TeamService : ITeamService
    {
        /// <summary>
        /// Holds PlayerRepository instance.
        /// </summary>
        private readonly ITeamRepository _teamRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamService"/> class.
        /// </summary>
        /// <param name="teamRepository">The team repository</param>
        public TeamService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        /// <summary>
        /// Method to get all teams.
        /// </summary>
        /// <returns>All teams.</returns>
        public IQueryable<Team> Get()
        {
            return _teamRepository.Find();
        }

        /// <summary>
        /// Create a new team.
        /// </summary>
        /// <param name="teamToCreate">A Team to create.</param>
        public void Create(Team teamToCreate)
        {
            try
            {
                _teamRepository.Add(teamToCreate);
            }
            catch (InvalidKeyValueException ex)
            {
                throw new MissingEntityException(ex.Message, ex);
            }

            // TODO: update players teamId

            _teamRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Finds a Team by id.
        /// </summary>
        /// <param name="id">id for search.</param>
        /// <returns>founded Team.</returns>
        public Team Get(int id)
        {
            Team team;
            try
            {
                team = _teamRepository.FindWhere(t => t.Id == id).Single();
            }
            catch (InvalidOperationException ex)
            {
                throw new MissingEntityException("Team with specified Id can not be found", ex);
            }

            return team;
        }

        /// <summary>
        /// Delete team by id.
        /// </summary>
        /// <param name="id">The id of team to delete.</param>
        public void Delete(int id)
        {
            try
            {
                _teamRepository.Remove(id);
                _teamRepository.UnitOfWork.Commit();
            }
            catch (InvalidKeyValueException ex)
            {
                var serviceException = new MissingEntityException("Team with specified Id can not be found", ex);
                throw serviceException;
            }
            
            // TODO: update players teamId
        }
    }
}
