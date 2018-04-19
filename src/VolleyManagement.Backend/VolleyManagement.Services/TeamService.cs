﻿namespace VolleyManagement.Services
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
        private readonly IQuery<Team, FindByCaptainIdCriteria> _getTeamByCaptainQuery;
        private readonly IQuery<int, FindByPlayerCriteria> _getPlayerTeamQuery;
        private readonly IQuery<ICollection<Team>, GetAllCriteria> _getAllTeamsQuery;
        private readonly IQuery<ICollection<Player>, TeamPlayersCriteria> _getTeamRosterQuery;
        private readonly IQuery<Team, FindByNameCriteria> _getTeamByNameQuery;
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
        /// <param name="getPlayerTeamQuery"></param>
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
            IQuery<Team, FindByNameCriteria> getTeamByNameQuery,
            IAuthorizationService authService)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            _teamRepository = teamRepository;
            _playerRepository = playerRepository;
            _getTeamByIdQuery = getTeamByIdQuery;
            _getPlayerByIdQuery = getPlayerByIdQuery;
            _getTeamByCaptainQuery = getTeamByCaptainQuery;
            _getPlayerTeamQuery = getPlayerTeamQuery;
            _getAllTeamsQuery = getAllTeamsQuery;
            _getTeamRosterQuery = getTeamRosterQuery;
            _getTeamByNameQuery = getTeamByNameQuery;
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
        public Team Create(CreateTeamDto teamToCreate)
        {
            _authService.CheckAccess(AuthOperations.Teams.Create);

            ThrowExceptionIfTeamWithSuchNameExists(teamToCreate.Name);

            var captain = GetPlayerById(teamToCreate.Captain.Id);
            if (captain == null)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.PlayerNotFound, teamToCreate.Captain.Id);
            }

            // Check if captain in teamToCreate is captain of another team
            var existTeam = GetTeamLedByCaptain(captain.Id);
            VerifyExistingTeamOrThrow(existTeam);

            return _teamRepository.Add(teamToCreate);
        }

        /// <summary>
        /// Add players to the team.
        /// </summary>
        /// <param name="team">Team id to which players must be added.</param>
        /// <param name="players">Player's ids that must be added.</param>
        public void AddPlayers(TeamId team, IEnumerable<PlayerId> players)
        {
            _authService.CheckAccess(AuthOperations.Teams.Edit);
            var changedTeam = Get(team.Id);

            if (changedTeam == null)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.TeamNotFound, team.Id);
            }

            CheckIfPlayersAreNotInAnoutherTeam(players);

            changedTeam.AddPlayers(players);
            _teamRepository.Update(changedTeam);
        }

        private void CheckIfPlayersAreNotInAnoutherTeam(IEnumerable<PlayerId> addedPlayers)
        {
            var playersTeams = addedPlayers
                .Select(x => _getPlayerTeamQuery.Execute(new FindByPlayerCriteria { Id = x.Id }));
            //check if players play in another team
            if (playersTeams.Any(x => x > 0))
            {
                throw new ArgumentException("Player can not play in two teams");
            }
        }


        /// <summary>
        /// Remove players from the team.
        /// </summary>
        /// <param name="team">Team id from which players must be removed.</param>
        /// <param name="players">Player's ids that must be removed.</param>
        public void RemovePlayers(TeamId team, IEnumerable<PlayerId> players)
        {
            _authService.CheckAccess(AuthOperations.Teams.Edit);
            var changedTeam = Get(team.Id);

            if (changedTeam == null)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.TeamNotFound, team.Id);
            }

            changedTeam.RemovePlayers(players);
            _teamRepository.Update(changedTeam);
        }

        /// <summary>
        /// Edit team.
        /// </summary>
        /// <param name="teamToEdit">Team to edit.</param>
        public void Edit(Team teamToEdit)
        {
            _authService.CheckAccess(AuthOperations.Teams.Edit);

            var teamInDb = _getTeamByNameQuery.Execute(new FindByNameCriteria { Name = teamToEdit.Name });
            if (teamInDb != null &&
                teamInDb.Id != teamToEdit.Id)
            {
                throw new ArgumentException(TournamentResources.TeamNameInTournamentNotUnique);
            }

            var captainId = teamToEdit.Captain.Id;
            var captain = GetPlayerById(captainId);

            if (captain == null)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.PlayerNotFound, captainId);
            }

            var teamId = _getPlayerTeamQuery.Execute(new FindByPlayerCriteria { Id = captain.Id });
            // Check if captain in teamToCreate is captain of another team
            if (teamId != 0 && teamId != teamToEdit.Id)
            {
                var existTeam = GetTeamLedByCaptain(captain.Id);
                VerifyExistingTeamOrThrow(existTeam);
            }

            var newTeam = Get(teamId);
            newTeam.Name = teamToEdit.Name;
            newTeam.Achievements = teamToEdit.Achievements;
            newTeam.Coach = teamToEdit.Coach;

            try
            {
                _teamRepository.Update(newTeam);
            }
            catch (ConcurrencyException ex)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.TeamNotFound, ex);
            }

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
        public void Delete(TeamId teamId)
        {
            _authService.CheckAccess(AuthOperations.Teams.Delete);
            try
            {
                _teamRepository.Remove(teamId);
            }
            catch (InvalidKeyValueException ex)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.TeamNotFound, teamId.Id, ex);
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
            return GetPlayerById(team.Captain.Id);
        }

        /// <summary>
        /// Change captain for existing team.
        /// </summary>
        /// <param name="team">team for editing.</param>
        /// <param name="captainId">Player who should become captain.</param>
        public void ChangeCaptain(TeamId team, PlayerId captainId)
        {
            _authService.CheckAccess(AuthOperations.Teams.Edit);
            var teamToEdit = Get(team.Id);
            if (teamToEdit == null)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.TeamNotFound, team.Id);
            }
            var captainTeamId = _getPlayerTeamQuery.Execute(new FindByPlayerCriteria { Id = captainId.Id });

            if (captainTeamId == teamToEdit.Id || captainTeamId == 0)
            {
                teamToEdit.SetCaptain(captainId);
                _teamRepository.Update(teamToEdit);
            }
            else
            {
                throw new ValidationException(ServiceResources.ExceptionMessages.PlayerIsPlayerOfAnotherTeam);
            }
        }

        /// <summary>
        /// Find players of specified team
        /// </summary>
        /// <param name="teamId">Id of team which players should be found</param>
        /// <returns>Collection of team's players</returns>
        public ICollection<Player> GetTeamRoster(TeamId teamId)
        {
            return _getTeamRosterQuery.Execute(new TeamPlayersCriteria { TeamId = teamId.Id });
        }

        private void ThrowExceptionIfTeamWithSuchNameExists(string name)
        {
            if (TeamWithNameExists(name))
            {
                throw new ArgumentException(TournamentResources.TeamNameInTournamentNotUnique);
            }
        }

        private bool TeamWithNameExists(string name) =>
            _getTeamByNameQuery
                .Execute(new FindByNameCriteria { Name = name }) != null;

        private static void VerifyExistingTeamOrThrow(Team existTeam)
        {
            if (existTeam != null)
            {
                var ex = new ValidationException(ServiceResources.ExceptionMessages.PlayerIsCaptainOfAnotherTeam);
                ex.Data[Domain.Constants.ExceptionManagement.ENTITY_ID_KEY] = existTeam.Id;
                throw ex;
            }
        }

        private Team GetTeamLedByCaptain(int playerId)
        {
            return _getTeamByCaptainQuery.Execute(new FindByCaptainIdCriteria { CaptainId = playerId });
        }

        private Player GetPlayerById(int id)
        {
            return _getPlayerByIdQuery.Execute(new FindByIdCriteria { Id = id });
        }
    }
}
