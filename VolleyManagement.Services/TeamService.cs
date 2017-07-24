namespace VolleyManagement.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Contracts;
    using Contracts.Authorization;
    using Contracts.Exceptions;
    using Data.Contracts;
    using Data.Exceptions;
    using Data.Queries.Common;
    using Data.Queries.Player;
    using Data.Queries.Team;
    using Domain.PlayersAggregate;
    using Domain.Properties;
    using Domain.RolesAggregate;
    using Domain.TeamsAggregate;
    using TournamentResources = Domain.Properties.Resources;

    /// <summary>
    /// Defines TeamService
    /// </summary>
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IQuery<Team, FindByIdCriteria> _getTeamByIdQuery;
        private readonly IQuery<Player, FindByIdCriteria> _getPlayerByIdQuery;
        private readonly IQuery<Team, FindByCaptainIdCriteria> _getTeamByCaptainQuery;
        private readonly IQuery<List<Team>, GetAllCriteria> _getAllTeamsQuery;
        private readonly IQuery<List<Player>, TeamPlayersCriteria> _getTeamRosterQuery;
        private readonly IAuthorizationService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamService"/> class.
        /// </summary>
        /// <param name="teamRepository">The team repository</param>
        /// <param name="playerRepository">The player repository</param>
        /// <param name="getTeamByIdQuery"> Get By ID query for Teams</param>
        /// <param name="getPlayerByIdQuery"> Get By ID query for Players</param>
        /// <param name="getTeamByCaptainQuery"> Get By Captain ID query for Teams</param>
        /// <param name="getAllTeamsQuery"> Get All teams query</param>
        /// <param name="getTeamRosterQuery"> Get players for team query</param>
        /// <param name="authService">Authorization service</param>
        public TeamService(
            ITeamRepository teamRepository,
            IPlayerRepository playerRepository,
            IQuery<Team, FindByIdCriteria> getTeamByIdQuery,
            IQuery<Player, FindByIdCriteria> getPlayerByIdQuery,
            IQuery<Team, FindByCaptainIdCriteria> getTeamByCaptainQuery,
            IQuery<List<Team>, GetAllCriteria> getAllTeamsQuery,
            IQuery<List<Player>, TeamPlayersCriteria> getTeamRosterQuery,
            IAuthorizationService authService)
        {
            _teamRepository = teamRepository;
            _playerRepository = playerRepository;
            _getTeamByIdQuery = getTeamByIdQuery;
            _getPlayerByIdQuery = getPlayerByIdQuery;
            _getTeamByCaptainQuery = getTeamByCaptainQuery;
            _getAllTeamsQuery = getAllTeamsQuery;
            _getTeamRosterQuery = getTeamRosterQuery;
            _authService = authService;
        }

        /// <summary>
        /// Method to get all teams.
        /// </summary>
        /// <returns>All teams.</returns>
        public List<Team> Get()
        {
            return _getAllTeamsQuery.Execute(new GetAllCriteria());
        }

        /// <summary>
        /// Create a new team.
        /// </summary>
        /// <param name="teamToCreate">A Team to create.</param>
        public void Create(Team teamToCreate)
        {
            _authService.CheckAccess(AuthOperations.Teams.Create);

            Player captain = GetPlayerById(teamToCreate.CaptainId);
            if (captain == null)
            {
                // ToDo: Revisit this case
                throw new MissingEntityException(ServiceResources.ExceptionMessages.PlayerNotFound, teamToCreate.CaptainId);
            }

            // Check if captain in teamToCreate is captain of another team
            if (captain.TeamId != null)
            {
                var existTeam = GetPlayerLedTeam(captain.Id);
                VerifyExistingTeamOrThrow(existTeam);
            }

            ValidateTeam(teamToCreate);

            _teamRepository.Add(teamToCreate);

            captain.TeamId = teamToCreate.Id;
            _playerRepository.Update(captain);
            _playerRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Edit team.
        /// </summary>
        /// <param name="team">Team to edit.</param>
        public void Edit(Team team)
        {
            _authService.CheckAccess(AuthOperations.Teams.Edit);
            Player captain = GetPlayerById(team.CaptainId);

            if (captain == null)
            {
                // ToDo: Revisit this case
                throw new MissingEntityException(ServiceResources.ExceptionMessages.PlayerNotFound, team.CaptainId);
        }

            // Check if captain in teamToCreate is captain of another team
            if ((captain.TeamId != null) && (captain.TeamId != team.Id))
            {
                var existTeam = GetPlayerLedTeam(captain.Id);
                VerifyExistingTeamOrThrow(existTeam);
            }

            ValidateTeam(team);

            try
            {
                _teamRepository.Update(team);
            }
            catch (ConcurrencyException ex)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.TeamNotFound, ex);
            }

            captain.TeamId = team.Id;
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
            return _getTeamByIdQuery.Execute(new FindByIdCriteria { Id = id });
        }

        /// <summary>
        /// Delete team by id.
        /// </summary>
        /// <param name="teamId">The id of team to delete.</param>
        public void Delete(int teamId)
        {
            _authService.CheckAccess(AuthOperations.Teams.Delete);
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
            return GetPlayerById(team.CaptainId);
        }

        /// <summary>
        /// Find players of specified team
        /// </summary>
        /// <param name="teamId">Id of team which players should be found</param>
        /// <returns>Collection of team's players</returns>
        public List<Player> GetTeamRoster(int teamId)
        {
            return _getTeamRosterQuery.Execute(new TeamPlayersCriteria { TeamId = teamId });
        }

        /// <summary>
        /// Sets team id to roster
        /// </summary>
        /// <param name="roster">Players to set the team</param>
        /// <param name="teamId">Id of team which should be set to player</param>
        public void UpdateRosterTeamId(List<Player> roster, int teamId)
        {
            if (GetTeamRoster(teamId).Count > 1)
            {
                foreach (var player in GetTeamRoster(teamId))
                {
                    if (roster.SingleOrDefault(t => t.Id == player.Id) == null)
                    {
                        SetPlayerTeamIdToNull(player.Id);
                    }
                }
            }

            foreach (var player in roster)
            {
                UpdatePlayerTeam(player.Id, teamId);
            }
        }

        private static bool ValidateTwoTeamsName(List<Team> existTeams, string name)
        {
            return existTeams.Where(t => t.Name.ToLower().Equals(name.ToLower())).Any();
        }

        private static void VerifyExistingTeamOrThrow(Team existTeam)
        {
            if (existTeam != null)
            {
                var ex = new ValidationException(ServiceResources.ExceptionMessages.PlayerIsCaptainOfAnotherTeam);
                ex.Data[Domain.Constants.ExceptionManagement.ENTITY_ID_KEY] = existTeam.Id;
                throw ex;
            }
        }

        private void UpdatePlayerTeam(int playerId, int teamId)
        {
            Player player = GetPlayerById(playerId);

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

            Team team = _getTeamByIdQuery.Execute(new FindByIdCriteria { Id = teamId });

            if (team == null)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.TeamNotFound, teamId);
            }

            player.TeamId = teamId;
            _playerRepository.Update(player);
            _playerRepository.UnitOfWork.Commit();
        }

        private void SetPlayerTeamIdToNull(int playerId)
        {
            Player player = GetPlayerById(playerId);

            if (player == null)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.PlayerNotFound, playerId);
            }

            player.TeamId = null;
            _playerRepository.Update(player);
            _playerRepository.UnitOfWork.Commit();
        }

        private Team GetPlayerLedTeam(int playerId)
        {
            return _getTeamByCaptainQuery.Execute(new FindByCaptainIdCriteria { CaptainId = playerId });
        }

        private Player GetPlayerById(int id)
        {
            return _getPlayerByIdQuery.Execute(new FindByIdCriteria { Id = id });
        }

        private void ValidateTeamName(string teamName)
        {
            if (TeamValidation.ValidateTeamName(teamName))
            {
                throw new ArgumentException(
                    string.Format(
                    Resources.ValidationTeamName,
                    Domain.Constants.Team.MAX_NAME_LENGTH),
                    "Name");
            }
        }

        private void ValidateCoachName(string teamCoachName)
        {
            if (!string.IsNullOrEmpty(teamCoachName))
            {
            if (TeamValidation.ValidateCoachName(teamCoachName))
            {
                throw new ArgumentException(
                    string.Format(
                    Resources.ValidationCoachName,
                    Domain.Constants.Team.MAX_COACH_NAME_LENGTH),
                    "Coach");
            }
        }
        }

        private void ValidateAchievements(string teamAchievements)
        {
            if (!string.IsNullOrEmpty(teamAchievements))
            {
            if (TeamValidation.ValidateAchievements(teamAchievements))
            {
                throw new ArgumentException(
                    string.Format(
                    Resources.ValidationTeamAchievements,
                    Domain.Constants.Team.MAX_ACHIEVEMENTS_LENGTH),
                    "Achievements");
            }
        }
        }

        private void ValidateTwoTeamsWithTheSameName(Team teamToValidate)
        {
            var existingTeams = GetListOfTeamsForEdit(teamToValidate);
            if (ValidateTwoTeamsName(existingTeams, teamToValidate.Name))
            {
                throw new ArgumentException(
                    TournamentResources.TeamNameInTournamentNotUnique);
            }
        }

        private List<Team> GetListOfTeamsForEdit(Team teamToValidate)
        {
            var existingTeams = Get();
            var teamToRemove = existingTeams.SingleOrDefault(r => r.Id == teamToValidate.Id);
            if (teamToRemove != null)
            {
                existingTeams.Remove(teamToRemove);
            }

            return existingTeams;
        }

        private void ValidateTeam(Team teamToValidate)
        {
            ValidateTeamName(teamToValidate.Name);
            ValidateCoachName(teamToValidate.Coach);
            ValidateAchievements(teamToValidate.Achievements);
            ValidateTwoTeamsWithTheSameName(teamToValidate);
        }
    }
}
