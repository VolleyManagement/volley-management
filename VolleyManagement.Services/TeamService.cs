namespace VolleyManagement.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Data.Exceptions;
    using VolleyManagement.Domain.PlayersAggregate;
    using VolleyManagement.Domain.TeamsAggregate;

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
            Player captain = _playerRepository.FindWhere(p => p.Id == teamToCreate.CaptainId).SingleOrDefault();
            if (captain == null)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.PlayerNotFound, teamToCreate.CaptainId);
            }

            // Check if captain in teamToCreate is captain of another team
            if (captain.TeamId != null)
            {
                var existTeam = GetPlayerLedTeam(captain.Id);
                if (existTeam != null)
                {
                    var ex = new ValidationException(ServiceResources.ExceptionMessages.PlayerIsCaptainOfAnotherTeam);
                    ex.Data[Domain.Constants.ExceptionManagement.ENTITY_ID_KEY] = existTeam.Id;
                    throw ex;
                }
            }

            _teamRepository.Add(teamToCreate);

            captain.TeamId = teamToCreate.Id;
            _playerRepository.Update(captain);
            _playerRepository.UnitOfWork.Commit();
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
                throw new MissingEntityException(ServiceResources.ExceptionMessages.TeamNotFound, id, ex);
            }

            return team;
        }

        /// <summary>
        /// Delete team by id.
        /// </summary>
        /// <param name="teamId">The id of team to delete.</param>
        public void Delete(int teamId)
        {
            try
            {
                _teamRepository.Remove(teamId);
            }
            catch (InvalidKeyValueException ex)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.TeamNotFound, teamId, ex);
            }

            IEnumerable<Player> roster = GetTeamRoster(teamId);
            foreach (var player in roster)
            {
                player.TeamId = null;
                _playerRepository.Update(player);
            }

            _teamRepository.UnitOfWork.Commit();
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
        public void UpdatePlayerTeam(int playerId, int? teamId)
        {
            Player player = _playerRepository.FindWhere(p => p.Id == playerId).SingleOrDefault();
            if (player == null)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.PlayerNotFound, playerId);
            }

            // Check if player is captain of another team
            if (player.TeamId != null)
            {
                var existingTeam = GetPlayerLedTeam(player.Id);
                if (existingTeam != null && teamId != existingTeam.Id)
                {
                    var ex = new ValidationException(ServiceResources.ExceptionMessages.PlayerIsCaptainOfAnotherTeam);
                    ex.Data[Domain.Constants.ExceptionManagement.ENTITY_ID_KEY] = existingTeam.Id;
                    throw ex;
                }
            }

            Team team = _teamRepository.FindWhere(t => t.Id == teamId).SingleOrDefault();
            if (team == null)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.TeamNotFound, teamId);
            }

            player.TeamId = teamId;
            _playerRepository.Update(player);
            _playerRepository.UnitOfWork.Commit();
        }

        private Team GetPlayerLedTeam(int playerId)
        {
            return _teamRepository.FindWhere(t => t.CaptainId == playerId).SingleOrDefault();
        }
    }
}
