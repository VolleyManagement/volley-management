namespace VolleyManagement.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
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
        private readonly IQuery<Player, FindByFullNameCriteria> _getPlayerByNameQuery;
        private readonly IQuery<Team, FindByCaptainIdCriteria> _getTeamByCaptainQuery;
        private readonly IQuery<ICollection<Team>, GetAllCriteria> _getAllTeamsQuery;
        private readonly IQuery<List<Player>, TeamPlayersCriteria> _getTeamRosterQuery;
        private readonly IAuthorizationService _authService;

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
            IQuery<ICollection<Team>, GetAllCriteria> getAllTeamsQuery,
            IQuery<List<Player>, TeamPlayersCriteria> getTeamRosterQuery,
            IAuthorizationService authService)
        {
            _teamRepository = teamRepository;
            _playerRepository = playerRepository;
            _getTeamByIdQuery = getTeamByIdQuery;
            _getPlayerByIdQuery = getPlayerByIdQuery;
            _getPlayerByNameQuery = getPlayerByNameQuery;
            _getTeamByCaptainQuery = getTeamByCaptainQuery;
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
        public void Edit(Team teamToEdit)
        {
            _authService.CheckAccess(AuthOperations.Teams.Edit);
            Player captain = GetPlayerById(teamToEdit.CaptainId);

            if (captain == null)
            {
                // ToDo: Revisit this case
                throw new MissingEntityException(ServiceResources.ExceptionMessages.PlayerNotFound, teamToEdit.CaptainId);
        }

            // Check if captain in teamToCreate is captain of another team
            if ((captain.TeamId != null) && (captain.TeamId != teamToEdit.Id))
            {
                var existTeam = GetPlayerLedTeam(captain.Id);
                VerifyExistingTeamOrThrow(existTeam);
            }

            ValidateTeam(teamToEdit);

            teamToEdit.CaptainId = captain.Id;

            try
            {
                _teamRepository.Update(teamToEdit);
            }
            catch (ConcurrencyException ex)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.TeamNotFound, ex);
            }

            captain.TeamId = teamToEdit.Id;
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
        //зміна
      
        private static bool ValidateTwoTeamsName(Team teamToValidate, List<Team> getExistingTeams)
        {
            var existingTeams = from ex in getExistingTeams
                                where ex.Id != teamToValidate.Id
                                where String.Equals(ex.Name,teamToValidate.Name,StringComparison.InvariantCultureIgnoreCase)
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
            Player player = GetPlayerByFullName(firstName, lastName);

            if (player == null)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.PlayerNotFound);
            }

            // Check if player plays in another team
            if (player.TeamId != null && player.TeamId != teamId)
            {
                throw new ArgumentException(
                    TournamentResources.ValidationPlayerOfAnotherTeam, player.FirstName + " " + player.FirstName);
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
                throw new MissingEntityException(ServiceResources.ExceptionMessages.PlayerNotFound);
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

        private Player GetPlayerByFullName(string firstName, string lastName)
        {
            return _getPlayerByNameQuery.Execute(new FindByFullNameCriteria { FirstName = firstName, LastName = lastName });
        }

        private static void ValidateTeamName(string teamName)
        {
            if (TeamValidation.ValidateTeamName(teamName))
            {
                throw new ArgumentException(
                    string.Format(
                    Resources.ValidationTeamName,
                    Domain.Constants.Team.MAX_NAME_LENGTH),
                    $"Name");
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
                       $"Coach");
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
                    $"Achievements");
            }
        }

        private void ValidateTwoTeamsWithTheSameName(Team teamToValidate)
        {
            var existingTeams = Get();
            if (ValidateTwoTeamsName(teamToValidate, existingTeams as List<Team>))
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
    }
}
