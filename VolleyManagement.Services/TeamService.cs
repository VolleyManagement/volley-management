namespace VolleyManagement.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.Dal.Exceptions;
    using VolleyManagement.Domain.Players;
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

        private readonly IPlayerService _playerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamService"/> class.
        /// </summary>
        /// <param name="teamRepository">The team repository</param>
        /// <param name="playerService">Service which provide basic operation with player repository</param>
        public TeamService(ITeamRepository teamRepository, IPlayerService playerService)
        {
            _teamRepository = teamRepository;
            _playerService = playerService;
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
                using (TransactionScope transaction = new TransactionScope())
                {
                _teamRepository.Add(teamToCreate);
                    Player captain = _playerService.Get(teamToCreate.CaptainId);
                    _playerService.UpdatePlayerTeam(captain, teamToCreate);
            }
            }
            catch (InvalidKeyValueException ex)
            {
                throw new MissingEntityException(ex.Message, ex);
            }
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
                using (TransactionScope transaction = new TransactionScope())
                {
                    // Does repository throw exception because players had RefKeys?
                _teamRepository.Remove(id);

                    IEnumerable<Player> roster = GetTeamRoster(id);
                    foreach (var player in roster)
                    {
                        _playerService.UpdatePlayerTeam(player, null);
                    }

                _teamRepository.UnitOfWork.Commit();

                    transaction.Complete();
                }
            }
            catch (InvalidKeyValueException ex)
            {
                var serviceException = new MissingEntityException("Team with specified Id can not be found", ex);
                throw serviceException;
            }
        }

        /// <summary>
        /// Find captain of specified team
        /// </summary>
        /// <param name="team">Team which captain should be found</param>
        /// <returns>Team's captain</returns>
        public Domain.Players.Player GetTeamCaptain(Team team)
        {
            return _playerService.Get(team.CaptainId);
        }

        /// <summary>
        /// Find players of specified team
        /// </summary>
        /// <param name="team">Team which players should be found</param>
        /// <returns>Collection of team's players</returns>
        public IEnumerable<Player> GetTeamRoster(Team team)
        {
            return GetTeamRoster(team.Id);
        }

        private IEnumerable<Player> GetTeamRoster(int teamId)
        {
            return _playerService.Get().Where(p => p.TeamId == teamId).ToList();
        }
    }
}
