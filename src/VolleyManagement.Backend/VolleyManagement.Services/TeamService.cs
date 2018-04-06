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

#pragma warning disable S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)
    /// <summary>
    /// Defines TeamService
    /// </summary>
    public class TeamService : ITeamService
#pragma warning restore S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IQuery<Team, FindByIdCriteria> _getTeamByIdQuery;
        private readonly IQuery<Player, FindByIdCriteria> _getPlayerByIdQuery;
        private readonly IQuery<Player, FindByFullNameCriteria> _getPlayerByNameQuery;
        private readonly IQuery<Team, FindByCaptainIdCriteria> _getTeamByCaptainQuery;
        private readonly IQuery<int, FindByPlayerCriteria> _getPlayerTeamQuery;
        private readonly IQuery<ICollection<Team>, GetAllCriteria> _getAllTeamsQuery;
        private readonly IQuery<ICollection<Player>, TeamPlayersCriteria> _getTeamRosterQuery;
        private readonly IAuthorizationService _authService;

#pragma warning disable S107 // Methods should not have too many parameters
        /// <summary>
        /// Initializes a new instance of the <see cref="TeamService"/> class.
        /// </summary>
        /// <param name="teamRepository">The team repository</param>
        /// <param name="playerRepository">The player repository</param>
        /// <param name="getTeamByIdQuery"> Get By ID query for Teams</param>
        /// <param name="getPlayerByIdQuery"> Get By ID query for Players</param>
        /// <param name="getPlayerByNameQuery"> Get By Name query for Players</param>
        /// <param name="getTeamByCaptainQuery"> Get By Captain ID query for Teams</param>
        /// <param name="getAllTeamsQuery"> Get All teams query</param>
        /// <param name="getTeamRosterQuery"> Get players for team query</param>
        /// <param name="authService">Authorization service</param>
        public TeamService(
            ITeamRepository teamRepository,
            IPlayerRepository playerRepository,
            IQuery<Team, FindByIdCriteria> getTeamByIdQuery,
            IQuery<Player, FindByIdCriteria> getPlayerByIdQuery,
            IQuery<Player, FindByFullNameCriteria> getPlayerByNameQuery,
            IQuery<Team, FindByCaptainIdCriteria> getTeamByCaptainQuery,
            IQuery<int, FindByPlayerCriteria> getPlayerTeamQuery,
        IQuery<ICollection<Team>, GetAllCriteria> getAllTeamsQuery,
            IQuery<ICollection<Player>, TeamPlayersCriteria> getTeamRosterQuery,
            IAuthorizationService authService)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            _teamRepository = teamRepository;
            _playerRepository = playerRepository;
            _getTeamByIdQuery = getTeamByIdQuery;
            _getPlayerByIdQuery = getPlayerByIdQuery;
            _getPlayerByNameQuery = getPlayerByNameQuery;
            _getTeamByCaptainQuery = getTeamByCaptainQuery;
            _getPlayerTeamQuery = getPlayerTeamQuery;
            _getAllTeamsQuery = getAllTeamsQuery;
            _getTeamRosterQuery = getTeamRosterQuery;
            _authService = authService;
        }

        /// <summary>
        /// Method to get all teams.
        /// </summary>
        /// <returns>All teams.</returns>
        public ICollection<Team> Get()
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

            var captain = GetPlayerById(teamToCreate.CaptainId);
            if (captain == null)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.PlayerNotFound, teamToCreate.CaptainId);
            }

            // Check if captain in teamToCreate is captain of another team
            var existTeam = GetPlayerLedTeam(captain.Id);
            VerifyExistingTeamOrThrow(existTeam);

            ValidateTeam(teamToCreate);

            _teamRepository.Add(teamToCreate);
            _playerRepository.UpdateTeam(captain, teamToCreate.Id);
        }

        /// <summary>
        /// Edit team.
        /// </summary>
        /// <param name="teamToEdit">Team to edit.</param>
        public void Edit(Team teamToEdit)
        {
            _authService.CheckAccess(AuthOperations.Teams.Edit);
            var captainId = teamToEdit.CaptainId.Id;
            var captain = GetPlayerById(captainId);

            if (captain == null)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.PlayerNotFound, captainId);
            }

            var teamId = _getPlayerTeamQuery.Execute(new FindByPlayerCriteria { Id = captain.Id });
            // Check if captain in teamToCreate is captain of another team
            if (teamId != 0 && teamId != teamToEdit.Id)
            {
                var existTeam = GetPlayerLedTeam(captain.Id);
                VerifyExistingTeamOrThrow(existTeam);
            }

            ValidateTeam(teamToEdit);

            try
            {
                _teamRepository.Update(teamToEdit);
            }
            catch (ConcurrencyException ex)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.TeamNotFound, ex);
            }

            teamToEdit.CaptainId = new PlayerId(captainId);
            _playerRepository.UpdateTeam(captain, teamToEdit.Id);
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
                _playerRepository.UpdateTeam(player, null);
            }
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
        public ICollection<Player> GetTeamRoster(int teamId)
        {
            return _getTeamRosterQuery.Execute(new TeamPlayersCriteria { TeamId = teamId });
        }

        /// <summary>
        /// Sets team id to roster
        /// </summary>
        /// <param name="roster">Players to set the team</param>
        /// <param name="teamId">Id of team which should be set to player</param>
        public void UpdateRosterTeamId(ICollection<Player> roster, int teamId)
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
                UpdatePlayerTeam(player.FirstName, player.LastName, teamId);
            }
        }

        private static bool ValidateTwoTeamsName(Team teamToValidate, ICollection<Team> getExistingTeams)
        {
            var existingTeams = from ex in getExistingTeams
                                where ex.Id != teamToValidate.Id
                                where string.Equals(ex.Name, teamToValidate.Name, StringComparison.InvariantCultureIgnoreCase)
                                select ex;
            return existingTeams.Count() != 0;
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

        private void UpdatePlayerTeam(string firstName, string lastName, int teamId)
        {
            var player = GetPlayerByFullName(firstName, lastName);

            if (player == null)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.PlayerNotFound);
            }
            var playerTeamId = _getPlayerTeamQuery.Execute(new FindByPlayerCriteria { Id = player.Id });
            // Check if player plays in another team
            if (playerTeamId != default(int) && playerTeamId != teamId)
            {
                throw new ArgumentException(
                    TournamentResources.ValidationPlayerOfAnotherTeam, player.FirstName + " " + player.FirstName);
            }

            // Check if player is captain of another team
            if (playerTeamId != default(int))
            {
                var existingTeam = GetPlayerLedTeam(player.Id);

                if (existingTeam != null && teamId != existingTeam.Id)
                {
                    var ex = new ValidationException(ServiceResources.ExceptionMessages.PlayerIsCaptainOfAnotherTeam);
                    ex.Data[Domain.Constants.ExceptionManagement.ENTITY_ID_KEY] = existingTeam.Id;
                    throw ex;
                }
            }

            var team = _getTeamByIdQuery.Execute(new FindByIdCriteria { Id = teamId });

            if (team == null)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.TeamNotFound, teamId);
            }

            _playerRepository.UpdateTeam(player, teamId);
        }

        private void SetPlayerTeamIdToNull(int playerId)
        {
            var player = GetPlayerById(playerId);

            if (player == null)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.PlayerNotFound);
            }

            _playerRepository.UpdateTeam(player, null);
        }

        private Team GetPlayerLedTeam(int playerId)
        {
            return _getTeamByCaptainQuery.Execute(new FindByCaptainIdCriteria { CaptainId = playerId });
        }

        private Player GetPlayerById(int id)
        {
            return _getPlayerByIdQuery.Execute(new FindByIdCriteria { Id = id });
        }

        private Player GetPlayerByFullName(string firstName, string lastName)
        {
            return _getPlayerByNameQuery.Execute(new FindByFullNameCriteria {
                FirstName = firstName,
                LastName = lastName
            });
        }

        private static void ValidateTeamName(string teamName)
        {
            if (TeamValidation.ValidateTeamName(teamName))
            {
                throw new ArgumentException(
                    string.Format(
                    Resources.ValidationTeamName,
                    Domain.Constants.Team.MAX_NAME_LENGTH),
                    nameof(teamName));
            }
        }

        private static void ValidateCoachName(string teamCoachName)
        {
            if (!string.IsNullOrEmpty(teamCoachName)
                && TeamValidation.ValidateCoachName(teamCoachName))
            {
                throw new ArgumentException(
                    string.Format(
                    Resources.ValidationCoachName,
                    Domain.Constants.Team.MAX_COACH_NAME_LENGTH),
                    nameof(teamCoachName));
            }
        }

        private static void ValidateAchievements(string teamAchievements)
        {
            if (!string.IsNullOrEmpty(teamAchievements)
                && TeamValidation.ValidateAchievements(teamAchievements))
            {
                throw new ArgumentException(
                    string.Format(
                    TournamentResources.ValidationTeamAchievements,
                    Domain.Constants.Team.MAX_ACHIEVEMENTS_LENGTH),
                    nameof(teamAchievements));
            }
        }

        private void ValidateTwoTeamsWithTheSameName(Team teamToValidate)
        {
            var existingTeams = Get();
            if (ValidateTwoTeamsName(teamToValidate, existingTeams))
            {
                throw new ArgumentException(
                    TournamentResources.TeamNameInTournamentNotUnique);
            }
        }

        private void ValidateTeam(Team teamToValidate)
        {
            ValidateTeamName(teamToValidate.Name);
            ValidateCoachName(teamToValidate.Coach);
            ValidateAchievements(teamToValidate.Achievements);
            ValidateTwoTeamsWithTheSameName(teamToValidate);
        }

        Team ITeamService.Create(Team teamToCreate)
        {
            throw new NotImplementedException();
        }

        public void ChangeCaptain(PlayerId captainId, Team team)
        {
            throw new NotImplementedException();
        }

        public void AddPlayers(IEnumerable<PlayerId> players, Team team)
        {
            throw new NotImplementedException();
        }

        public void RemovePlayers(IEnumerable<PlayerId> players, Team team)
        {
            throw new NotImplementedException();
        }
    }
}
