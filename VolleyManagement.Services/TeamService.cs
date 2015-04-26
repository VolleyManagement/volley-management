namespace VolleyManagement.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Linq.Expressions;
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
        private readonly ITeamRepository _teamRepository;

        private readonly IPlayerRepository _playerRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamService"/> class.
        /// </summary>
        /// <param name="teamRepository">The team repository</param>
        /// <param name="playerRepository">The player repository</param>
        public TeamService(ITeamRepository teamRepository, IPlayerRepository playerRepository)
        {
            _teamRepository = teamRepository;
            _playerRepository = playerRepository;
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
            using (IDbTransaction transaction = _teamRepository.UnitOfWork.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                Player captain;
                try
                {
                    captain = GetPlayerWhere(p => p.Id == teamToCreate.CaptainId).Single();
                }
                catch (InvalidOperationException ex)
                {
                    throw new MissingEntityException("Player with specified Id can not be found", teamToCreate.CaptainId, ex);
                }

                _teamRepository.Add(teamToCreate);
                SetPlayerTeam(captain, teamToCreate.Id);

                _teamRepository.UnitOfWork.Commit();
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
                throw new MissingEntityException("Team with specified Id can not be found", id, ex);
            }

            return team;
        }

        /// <summary>
        /// Delete team by id.
        /// </summary>
        /// <param name="teamId">The id of team to delete.</param>
        public void Delete(int teamId)
        {
            using (IDbTransaction transaction = _teamRepository.UnitOfWork.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    _teamRepository.Remove(teamId);
                }
                catch (InvalidKeyValueException ex)
                {
                    throw new MissingEntityException("Team with specified Id can not be found", teamId, ex);
                }

                IEnumerable<Player> roster = GetTeamRoster(teamId);
                foreach (var player in roster)
                {
                    SetPlayerTeam(player, null);
                }

                _teamRepository.UnitOfWork.Commit();
            }
        }

        /// <summary>
        /// Find captain of specified team
        /// </summary>
        /// <param name="team">Team which captain should be found</param>
        /// <returns>Team's captain</returns>
        public Player GetTeamCaptain(Team team)
        {
            return GetPlayerWhere(p => p.Id == team.CaptainId).Single();
        }

        /// <summary>
        /// Find players of specified team
        /// </summary>
        /// <param name="teamId">Id of team which players should be found</param>
        /// <returns>Collection of team's players</returns>
        public IEnumerable<Player> GetTeamRoster(int teamId)
        {
            return GetPlayerWhere(p => p.TeamId == teamId).ToList();
        }

        /// <summary>
        /// Sets team to player
        /// </summary>
        /// <param name="playerId">Id of player to set the team</param>
        /// <param name="teamId">Id of team which should be set to player</param>
        public void SetPlayerTeam(int playerId, int teamId)
        {
            Player player;
            try
            {
                player = GetPlayerWhere(p => p.Id == playerId).Single();
            }
            catch (InvalidOperationException ex)
            {
                throw new MissingEntityException("Player with specified Id can not be found", playerId, ex);
            }

            Team team;
            try
            {
                team = _teamRepository.FindWhere(t => t.Id == teamId).Single();
            }
            catch (InvalidOperationException ex)
            {
                throw new MissingEntityException("Team with specified Id can not be found", teamId, ex);
            }

            SetPlayerTeam(player, teamId);
        }

        private void SetPlayerTeam(Player player, int? teamId)
        {
            player.TeamId = teamId;
            _playerRepository.Update(player);
        }

        private IQueryable<Player> GetPlayerWhere(Expression<Func<Player, bool>> predicate)
        {
            return _playerRepository.FindWhere(predicate);
        }
    }
}
