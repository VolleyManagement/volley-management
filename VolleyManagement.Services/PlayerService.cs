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
    using Data.Queries.Team;
    using Domain.PlayersAggregate;
    using Domain.RolesAggregate;
    using Domain.TeamsAggregate;
    using PlayerResources = Domain.Properties.Resources;

    /// <summary>
    /// Defines PlayerService
    /// </summary>
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IQuery<Player, FindByIdCriteria> _getPlayerByIdQuery;
        private readonly IQuery<Team, FindByIdCriteria> _getTeamByIdQuery;
        private readonly IQuery<IQueryable<Player>, GetAllCriteria> _getAllPlayersQuery;
        private readonly IQuery<Team, FindByCaptainIdCriteria> _getTeamByCaptainQuery;
        private readonly IAuthorizationService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerService"/> class.
        /// </summary>
        /// <param name="playerRepository"> The player repository </param>
        /// <param name="getTeamByIdQuery"> Get By ID query for Teams</param>
        /// <param name="getPlayerByIdQuery"> Get By ID query for Players</param>
        /// <param name="getAllPlayersQuery"> Get All players query</param>
        /// <param name="getTeamByCaptainQuery">Get Player by Captain query</param>
        /// <param name="authService">Authorization service</param>
        public PlayerService(
            IPlayerRepository playerRepository,
            IQuery<Team, FindByIdCriteria> getTeamByIdQuery,
            IQuery<Player, FindByIdCriteria> getPlayerByIdQuery,
            IQuery<IQueryable<Player>, GetAllCriteria> getAllPlayersQuery,
            IQuery<Team, FindByCaptainIdCriteria> getTeamByCaptainQuery,
            IAuthorizationService authService)
        {
            _playerRepository = playerRepository;
            _getTeamByIdQuery = getTeamByIdQuery;
            _getPlayerByIdQuery = getPlayerByIdQuery;
            _getAllPlayersQuery = getAllPlayersQuery;
            _getTeamByCaptainQuery = getTeamByCaptainQuery;
            _authService = authService;
        }

        /// <summary>
        /// Method to get all players.
        /// </summary>
        /// <returns>All players.</returns>
        public IQueryable<Player> Get()
        {
            return _getAllPlayersQuery.Execute(new GetAllCriteria());
        }

        /// <summary>
        /// Create a new player.
        /// </summary>
        /// <param name="playerToCreate">A Player to create.</param>
        public void Create(Player playerToCreate)
        {
            _authService.CheckAccess(AuthOperations.Players.Create);
            if (playerToCreate == null)
            {
                throw new ArgumentNullException("playerToCreate");
            }

            _playerRepository.Add(playerToCreate);
            _playerRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Create new players.
        /// </summary>
        /// <param name="playersToCreate">New players.</param>
        public void Create(List<Player> playersToCreate)
        {
            _authService.CheckAccess(AuthOperations.Players.Create);

            var newplayers = GetPlayersToCreate(playersToCreate);

            if (ValidateExistingPlayers(playersToCreate))
            {
                throw new ArgumentException(
                    PlayerResources.ValidationPlayerOfAnotherTeam);
            }

            if (newplayers.Any())
            {
                foreach (var player in newplayers)
                {
                    _playerRepository.Add(player);
                }

                _playerRepository.UnitOfWork.Commit();
            }
        }

        /// <summary>
        /// Finds a Player by id.
        /// </summary>
        /// <param name="id">id for search.</param>
        /// <returns>A found Player.</returns>
        public Player Get(int id)
        {
            return _getPlayerByIdQuery.Execute(new FindByIdCriteria { Id = id });
        }

        /// <summary>
        /// Edit player.
        /// </summary>
        /// <param name="playerToEdit">Player to edit.</param>
        public void Edit(Player playerToEdit)
        {
            // Check if player is captain of team and teamId is null or changed
            ////Team ledTeam = GetPlayerLedTeam(playerToEdit.Id);
            ////if (ledTeam != null &&
            ////    (playerToEdit.TeamId == null || playerToEdit.TeamId != ledTeam.Id))
            ////{
            ////    var ex = new ValidationException(ServiceResources.ExceptionMessages.PlayerIsCaptainOfAnotherTeam);
            ////    ex.Data[Domain.Constants.ExceptionManagement.ENTITY_ID_KEY] = ledTeam.Id;
            ////    throw ex;
            ////}

            ////if (playerToEdit.TeamId != null
            ////    && _teamRepository.FindWhere(t => t.Id == playerToEdit.TeamId).SingleOrDefault() == null)
            ////{
            ////    throw new MissingEntityException(ServiceResources.ExceptionMessages.TeamNotFound, playerToEdit.TeamId);
            ////}

            _authService.CheckAccess(AuthOperations.Players.Edit);
            try
            {
                _playerRepository.Update(playerToEdit);
            }
            catch (InvalidKeyValueException ex)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.PlayerNotFound, ex);
            }

            _playerRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Delete player by id.
        /// </summary>
        /// <param name="id">The id of player to delete.</param>
        public void Delete(int id)
        {
            _authService.CheckAccess(AuthOperations.Players.Delete);
            Team playerTeam = GetPlayerLedTeam(id);

            if (playerTeam != null)
            {
                var ex = new ValidationException(ServiceResources.ExceptionMessages.PlayerIsCaptainOfAnotherTeam);
                ex.Data[Domain.Constants.ExceptionManagement.ENTITY_ID_KEY] = playerTeam.Id;
                throw ex;
            }

            try
            {
                _playerRepository.Remove(id);
                _playerRepository.UnitOfWork.Commit();
            }
            catch (InvalidKeyValueException ex)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.PlayerNotFound, id, ex);
            }
        }

        /// <summary>
        /// Find team of specified player
        /// </summary>
        /// <param name="player">Player which team should be found</param>
        /// <returns>Player's team</returns>
        public Team GetPlayerTeam(Player player)
        {
            if (player.TeamId == null)
            {
                return null;
            }

            return GetTeamById(player.TeamId.GetValueOrDefault());
        }

        private Team GetPlayerLedTeam(int playerId)
        {
            return _getTeamByCaptainQuery.Execute(new FindByCaptainIdCriteria { CaptainId = playerId });
        }

        private Team GetTeamById(int id)
        {
            return _getTeamByIdQuery.Execute(new FindByIdCriteria { Id = id });
        }

        /// <summary>
        /// Remove existing players from List
        /// </summary>
        /// <param name="playersToCreate">List of Players</param>
        /// <returns> List of New Players </returns>
        private List<Player> GetPlayersToCreate(List<Player> playersToCreate)
        {
            var allPlayers = Get().ToList();
            var players = playersToCreate.ToList();
            foreach (var allPlayer in allPlayers)
            {
                players.RemoveAll(t => t.FirstName.ToLower().Equals(allPlayer.FirstName.ToLower())
                                       && t.LastName.ToLower().Equals(allPlayer.LastName.ToLower()));
            }

            return players;
        }

        /// <summary>
        /// Validate if Player of Another Team
        /// </summary>
        /// <param name="playersToCreate">List of Players</param>
        /// <returns> Return true if Player has TeamId </returns>
        private bool ValidateExistingPlayers(List<Player> playersToCreate)
        {
            var existingPlayers = Get().ToList();

            var teamId = playersToCreate.First().Id != 0
                ? Get(playersToCreate.First(t => t.Id != 0).Id).TeamId
                : null;

            return existingPlayers
                .Select(allPlayer => playersToCreate
                    .FindAll(t => t.FirstName.ToLower().Equals(allPlayer.FirstName.ToLower())
                                  && t.LastName.ToLower().Equals(allPlayer.LastName.ToLower())
                                  && allPlayer.TeamId != null
                                  && allPlayer.TeamId != teamId))
                .Any(players => players.Any());
        }
    }
}
