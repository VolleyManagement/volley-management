namespace VolleyManagement.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.Dal.Exceptions;
    using VolleyManagement.Domain.Players;
    using VolleyManagement.Domain.Teams;
    using DAL = VolleyManagement.Dal.Contracts;
    using IsolationLevel = System.Data.IsolationLevel;

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
                    captain = _playerRepository.FindWhere(p => p.Id == teamToCreate.CaptainId).Single();
                }
                catch (InvalidOperationException ex)
            {
                    throw new MissingEntityException("Player with specified Id can not be found", teamToCreate.CaptainId, ex);
            }

                _teamRepository.Add(teamToCreate);

                captain.TeamId = teamToCreate.Id;
                _playerRepository.Update(captain);
                _playerRepository.UnitOfWork.Commit();

                transaction.Commit();
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
                    player.TeamId = teamId;
                    _playerRepository.Update(player);
                    }

                    _teamRepository.UnitOfWork.Commit();
                transaction.Commit();
            }
        }

        /// <summary>
        /// Find captain of specified team
        /// </summary>
        /// <param name="team">Team which captain should be found</param>
        /// <returns>Team's captain</returns>
        public Player GetTeamCaptain(Team team)
        {
            return _playerRepository.FindWhere(p => p.Id == team.CaptainId).Single();
        }

        /// <summary>
        /// Find players of specified team
        /// </summary>
        /// <param name="teamId">Id of team which players should be found</param>
        /// <returns>Collection of team's players</returns>
        public IEnumerable<Player> GetTeamRoster(int teamId)
        {
            return _playerRepository.FindWhere(p => p.TeamId == teamId).ToList();
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
                player = _playerRepository.FindWhere(p => p.Id == playerId).Single();
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

            player.TeamId = teamId;
            _playerRepository.Update(player);
            _playerRepository.UnitOfWork.Commit();
        }
    }
}
